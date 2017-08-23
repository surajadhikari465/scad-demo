using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Infor.Services.NewItem.Commands;
using Infor.Services.NewItem.Constants;
using Infor.Services.NewItem.Models;
using Infor.Services.NewItem.Notifiers;
using Infor.Services.NewItem.Queries;
using Infor.Services.NewItem.Services;
using Infor.Services.NewItem.Validators;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infor.Services.NewItem.Processor
{
    public class NewItemProcessor : INewItemProcessor
    {
        private InforNewItemApplicationSettings settings;
        private ICommandHandler<AddNewItemsToIconCommand> addNewItemsToIconCommandHandler;
        private IInforItemService inforItemService;
        private IIconItemService iconItemService;
        private ICommandHandler<FinalizeNewItemEventsCommand> finalizeNewItemEventsCommandHandler;
        private IQueryHandler<GetNewItemsQuery, IEnumerable<NewItemModel>> getNewItemsQueryHandler;
        private ICollectionValidator<NewItemModel> newItemCollectionValidator;
        private ICommandHandler<ArchiveNewItemsCommand> archiveNewItemsCommandHandler;
        private INewItemNotifier notifier;
        private IRenewableContext<IconContext> iconContext;
        private IRenewableContext<IrmaContext> irmaContext;
        private ILogger<NewItemProcessor> logger;

        public NewItemProcessor(
            InforNewItemApplicationSettings settings,
            IQueryHandler<GetNewItemsQuery, IEnumerable<NewItemModel>> getNewItemsQueryHandler,
            ICollectionValidator<NewItemModel> newItemCollectionValidator,
            ICommandHandler<AddNewItemsToIconCommand> addNewItemsToIconCommandHandler,
            IInforItemService inforItemService,
            IIconItemService iconItemService,
            ICommandHandler<FinalizeNewItemEventsCommand> finalizeNewItemEventsCommandHandler,
            ICommandHandler<ArchiveNewItemsCommand> archiveNewItemsCommandHandler,
            INewItemNotifier notifier,
            IRenewableContext<IconContext> iconContext,
            IRenewableContext<IrmaContext> irmaContext,
            ILogger<NewItemProcessor> logger)
        {
            this.settings = settings;
            this.getNewItemsQueryHandler = getNewItemsQueryHandler;
            this.newItemCollectionValidator = newItemCollectionValidator;
            this.addNewItemsToIconCommandHandler = addNewItemsToIconCommandHandler;
            this.inforItemService = inforItemService;
            this.iconItemService = iconItemService;
            this.finalizeNewItemEventsCommandHandler = finalizeNewItemEventsCommandHandler;
            this.archiveNewItemsCommandHandler = archiveNewItemsCommandHandler;
            this.notifier = notifier;
            this.iconContext = iconContext;
            this.irmaContext = irmaContext;
            this.logger = logger;
        }

        public void ProcessNewItemEvents(int controllerInstanceId)
        {
            foreach (var region in settings.Regions)
            {
                IEnumerable<NewItemModel> newItems = new List<NewItemModel>();
                do
                {
                    irmaContext.Refresh(region);
                    iconContext.Refresh();
                    bool errorOccurredWhileProcessing = false;
                    try
                    {
                        newItems = getNewItemsQueryHandler.Search(
                            new GetNewItemsQuery
                            {
                                Instance = controllerInstanceId,
                                Region = region,
                                NumberOfItemsInMessage = settings.NumberOfItemsPerMessage
                            });
                        addNewItemsToIconCommandHandler.Execute(new AddNewItemsToIconCommand { NewItems = newItems });

                        //Items that already exist in Icon do not need to be sent to 
                        //Infor and instead will be sent to the Icon queue to be picked up by GloCon.
                        var newItemsThatExistInIcon = newItems.Where(m => m.IconItemId.HasValue).ToList();
                        var newItemsThatDontExistInIcon = newItems.Where(m => !m.IconItemId.HasValue).ToList();
                        AddItemEventsToIconEventQueue(newItemsThatExistInIcon);
                        SendItemsToInfor(region, newItemsThatDontExistInIcon);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                        errorOccurredWhileProcessing = true;
                        foreach (var item in newItems.Where(i => i.ErrorCode == null))
                        {
                            item.ErrorCode = ApplicationErrors.Codes.UnexpectedProcessingError;
                            item.ErrorDetails = ex.ToString();
                        }
                    }
                    finally
                    {
                        finalizeNewItemEventsCommandHandler.Execute(new FinalizeNewItemEventsCommand
                        {
                            Instance = controllerInstanceId,
                            Region = region,
                            NewItems = newItems,
                            ErrorOccurred = errorOccurredWhileProcessing
                        });
                        notifier.NotifyOfNewItemError(newItems);
                        archiveNewItemsCommandHandler.Execute(new ArchiveNewItemsCommand { NewItems = newItems });
                    }
                } while (newItems.Any());
            }
        }

        private void AddItemEventsToIconEventQueue(IEnumerable<NewItemModel> newItemsThatExistInIcon)
        {
            iconItemService.AddItemEventsToIconEventQueue(newItemsThatExistInIcon);
        }

        private bool SendItemsToInfor(string region, IEnumerable<NewItemModel> newItems)
        {
            bool errorOccurredWhileProcessing = false;
            var validationResult = newItemCollectionValidator.ValidateCollection(newItems);
            if (validationResult.ValidEntities.Any())
            {
                AddNewItemsToInforResponse response = inforItemService.AddNewItemsToInfor(
                    new AddNewItemsToInforRequest
                    {
                        NewItems = newItems,
                        Region = region
                    });
                errorOccurredWhileProcessing = response.ErrorOccurred;
            }

            return errorOccurredWhileProcessing;
        }
    }
}
