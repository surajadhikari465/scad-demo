using Icon.Logging;
using Icon.Framework;
using RegionalEventController.DataAccess.Commands;
using RegionalEventController.DataAccess.Queries;
using RegionalEventController.DataAccess.Interfaces;
using RegionalEventController.DataAccess.Models;
using RegionalEventController.Controller.ProcessorModules;
using RegionalEventController.Common;
using System.Collections.Generic;
using System.Data.Entity;
using System;
using System.Reflection;
using System.Linq;

namespace RegionalEventController.Controller.UpdateServices
{
    public class ItemSubTeamEventService : IUpdateService
    {
        private ILogger<ItemSubTeamEventService> logger;
        private IconContext iconContext;
        private List<IrmaNewItem> irmaNewItems;
        private IQueryHandler<GetIconIrmaItemsBulkQuery, Dictionary<string, int>> getIconIrmaItemsBulkQueryHandler;
        private IBulkCommandHandler<InsertEventQueuesToIconBulkCommand> insertEventQueuesToIconBulkCommandHandler;
        private INewItemProcessingModule newItemProcessingModule;
        private string regionCode;

        public ItemSubTeamEventService(ILogger<ItemSubTeamEventService> logger,
            IconContext iconContext,
            List<IrmaNewItem> irmaNewItems,
            IQueryHandler<GetIconIrmaItemsBulkQuery, Dictionary<string, int>> getIconIrmaItemsBulkQueryHandler,
            IBulkCommandHandler<InsertEventQueuesToIconBulkCommand> insertEventQueuesToIconBulkCommandHandler,
            INewItemProcessingModule newItemProcessingModule,
            string regionCode)
        {
            this.logger = logger;
            this.iconContext = iconContext;
            this.irmaNewItems = irmaNewItems;
            this.getIconIrmaItemsBulkQueryHandler = getIconIrmaItemsBulkQueryHandler;
            this.insertEventQueuesToIconBulkCommandHandler = insertEventQueuesToIconBulkCommandHandler;
            this.newItemProcessingModule = newItemProcessingModule;
            this.regionCode = regionCode;
        }

        public void UpdateBulk()
        {
            using (DbContextTransaction transaction = iconContext.Database.BeginTransaction())
            {
                try
                {
                    GetIconIrmaItemsBulkQuery getIconIrmaItemsBulkQuery = new GetIconIrmaItemsBulkQuery();

                    getIconIrmaItemsBulkQuery.identifiers = irmaNewItems.Select(i => i.Identifier).ToList();
                    Dictionary<string, int> iconItems = getIconIrmaItemsBulkQueryHandler.Execute(getIconIrmaItemsBulkQuery);

                    List<EventQueue> EventQueueEntries = new List<EventQueue>();

                    foreach (string scanCode in iconItems.Keys)
                    {                      
                        EventQueueEntries.Add(new EventQueue
                        {
                            EventId = EventTypes.ItemSubTeamUpdate,
                            EventMessage = scanCode,
                            EventReferenceId = iconItems[scanCode],
                            RegionCode = this.regionCode
                        });
                    }
                    
                    InsertEventQueuesToIconBulkCommand insertEventQueuesToIconBulkCommand = new InsertEventQueuesToIconBulkCommand { EventQueueEntries = EventQueueEntries };
                    insertEventQueuesToIconBulkCommandHandler.Execute(insertEventQueuesToIconBulkCommand);
                    transaction.Commit();                    
                }
                catch (Exception ex)
                {
                    ExceptionHandler<ItemSubTeamEventService>.logger = this.logger;
                    ExceptionHandler<ItemSubTeamEventService>.HandleException("An exception occurred in ItemSubTeamEventService Bulk Process: item sub team events generation will be done row by row. ", ex, this.GetType(), MethodBase.GetCurrentMethod());
                    
                    transaction.Rollback();
                    throw new Exception("Unable to successfully complete generating Item sub team events in bulk.");
                }
            }
        }

        public void UpdateRowByRow()
        {
            foreach (IrmaNewItem newItem in irmaNewItems)
            {
                var iconItem = iconContext.ScanCode.SingleOrDefault(sc => newItem.Identifier.Equals(sc.scanCode));

                using (DbContextTransaction transaction = iconContext.Database.BeginTransaction())
                {
                    try
                    {
                        newItemProcessingModule.CreateEventQueueEntry(EventTypes.ItemSubTeamUpdate, iconItem.scanCode, iconItem.itemID, regionCode);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler<ItemSubTeamEventService>.logger = this.logger;
                        ExceptionHandler<ItemSubTeamEventService>.HandleException(String.Format("An unhandled exception occurred in the Row-by-Row update for validated Scan Code {0} in region {1}. ", newItem.IrmaItem.identifier, regionCode), ex, this.GetType(), MethodBase.GetCurrentMethod());

                        newItem.ProcessedByController = false;
                        newItem.FailureReason = ex.Message;
                        transaction.Rollback();
                    }
                }
            }
        }
    }
}
