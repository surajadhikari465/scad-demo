using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Infor.Services.NewItem.Commands;
using Infor.Services.NewItem.Constants;
using Infor.Services.NewItem.Infrastructure;
using Infor.Services.NewItem.Models;
using Infor.Services.NewItem.Queries;
using Infor.Services.NewItem.Services;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infor.Services.NewItem.Processor
{
    public class NewItemProcessor : INewItemProcessor
    {
        private InforNewItemApplicationSettings settings;
        private ICommandHandler<AddNewItemsToIconCommand> addNewItemsToIconCommandHandler;
        private IInforItemService inforItemService;
        private ICommandHandler<FinalizeNewItemEventsCommand> finalizeNewItemEventsCommandHandler;
        private IQueryHandler<GetNewItemsQuery, IEnumerable<NewItemModel>> getNewItemsQueryHandler;
        private IRenewableContext<IconContext> iconContext;
        private IRenewableContext<IrmaContext> irmaContext;
        private ILogger<NewItemProcessor> logger;
        private ICommandHandler<ArchiveNewItemsCommand> archiveNewItemsCommandHandler;

        public NewItemProcessor(
            InforNewItemApplicationSettings settings,
            IQueryHandler<GetNewItemsQuery, IEnumerable<NewItemModel>> getNewItemsQueryHandler,
            ICommandHandler<AddNewItemsToIconCommand> addNewItemsToIconCommandHandler,
            IInforItemService inforItemService,
            ICommandHandler<FinalizeNewItemEventsCommand> finalizeNewItemEventsCommandHandler,
            ICommandHandler<ArchiveNewItemsCommand> archiveNewItemsCommandHandler,
            IRenewableContext<IconContext> iconContext,
            IRenewableContext<IrmaContext> irmaContext,
            ILogger<NewItemProcessor> logger)
        {
            this.settings = settings;
            this.getNewItemsQueryHandler = getNewItemsQueryHandler;
            this.addNewItemsToIconCommandHandler = addNewItemsToIconCommandHandler;
            this.inforItemService = inforItemService;
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
                    irmaContext.Refresh(region);
                    iconContext.Refresh();
                    bool errorOccurredWhileProcessing = false;
                    try
                    {
                        newItems = getNewItemsQueryHandler.Search(new GetNewItemsQuery { Instance = controllerInstanceId, Region = region, NumberOfItemsInMessage = settings.NumberOfItemsPerMessage });
                        addNewItemsToIconCommandHandler.Execute(new AddNewItemsToIconCommand { NewItems = newItems });
                        AddNewItemsToInforResponse response = inforItemService.AddNewItemsToInfor(new AddNewItemsToInforRequest { NewItems = newItems, Region = region });
                        errorOccurredWhileProcessing = response.ErrorOccurred;
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
                        archiveNewItemsCommandHandler.Execute(new ArchiveNewItemsCommand
                        {
                            NewItems = newItems
                        });
                    }
                } while (newItems.Any());
            }
        }
    }
}
