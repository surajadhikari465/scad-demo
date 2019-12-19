using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Services.NewItem.Commands;
using Services.NewItem.Constants;
using Services.NewItem.Models;
using Services.NewItem.Queries;
using Services.NewItem.Services;
using Irma.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.NewItem.Processor
{
    public class NewItemProcessor : INewItemProcessor
    {
        private NewItemApplicationSettings settings;
        private ICommandHandler<UpdateItemSubscriptionInIconCommand> updateItemSubscriptionInIconCommandHandler;
        private IIconItemService iconItemService;
        private ICommandHandler<FinalizeNewItemEventsCommand> finalizeNewItemEventsCommandHandler;
        private IQueryHandler<GetNewItemsQuery, IEnumerable<NewItemModel>> getNewItemsQueryHandler;
        private ICommandHandler<ArchiveNewItemsCommand> archiveNewItemsCommandHandler;
        private IRenewableContext<IconContext> iconContext;
        private IRenewableContext<IrmaContext> irmaContext;
        private ILogger<NewItemProcessor> logger;

        public NewItemProcessor(
            NewItemApplicationSettings settings,
            IQueryHandler<GetNewItemsQuery, IEnumerable<NewItemModel>> getNewItemsQueryHandler,
            ICommandHandler<UpdateItemSubscriptionInIconCommand> updateItemSubscriptionInIconCommandHandler,
            IIconItemService iconItemService,
            ICommandHandler<FinalizeNewItemEventsCommand> finalizeNewItemEventsCommandHandler,
            ICommandHandler<ArchiveNewItemsCommand> archiveNewItemsCommandHandler,
            IRenewableContext<IconContext> iconContext,
            IRenewableContext<IrmaContext> irmaContext,
            ILogger<NewItemProcessor> logger)
        {
            this.settings = settings;
            this.getNewItemsQueryHandler = getNewItemsQueryHandler;
            this.updateItemSubscriptionInIconCommandHandler = updateItemSubscriptionInIconCommandHandler;
            this.iconItemService = iconItemService;
            this.finalizeNewItemEventsCommandHandler = finalizeNewItemEventsCommandHandler;
            this.archiveNewItemsCommandHandler = archiveNewItemsCommandHandler;
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
                    bool errorOccurredWhileProcessing = false;
                    try
                    {
                        irmaContext.Refresh(region);
                        iconContext.Refresh();
                        newItems = getNewItemsQueryHandler.Search(
                            new GetNewItemsQuery
                            {
                                Instance = controllerInstanceId,
                                Region = region,
                                NumberOfItemsInMessage = settings.NumberOfItemsPerMessage
                            });
                        updateItemSubscriptionInIconCommandHandler.Execute(new UpdateItemSubscriptionInIconCommand { NewItems = newItems });

                        //Items that already exist in Icon do not need to be sent to 
                        //Infor and instead will be sent to the Icon queue to be picked up by GloCon.
                        var newItemsThatExistInIcon = newItems.Where(m => m.IconItemId.HasValue).ToList();
                          AddItemEventsToIconEventQueue(newItemsThatExistInIcon);
                     }
                    catch (Exception ex)
                    {
                        LogException(ex, region, newItems);
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
                        archiveNewItemsCommandHandler.Execute(new ArchiveNewItemsCommand { NewItems = newItems });
                    }
                } while (newItems.Any());
            }
        }

        private void AddItemEventsToIconEventQueue(IEnumerable<NewItemModel> newItemsThatExistInIcon)
        {
            iconItemService.AddItemEventsToIconEventQueue(newItemsThatExistInIcon);
        }
        private void LogException(Exception ex, string region, IEnumerable<NewItemModel> newItems)
        {
            logger.Error(JsonConvert.SerializeObject(
                new
                {
                    Message = ApplicationErrors.Codes.UnexpectedProcessingError,
                    Region = region,
                    Items = newItems,
                    ExceptionMessage = ex.Message,
                    Exception = ex.ToString()
                }));
        }
    }
}
