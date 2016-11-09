using Icon.Logging;
using Icon.Framework;
using RegionalEventController.DataAccess.Commands;
using RegionalEventController.DataAccess.Interfaces;
using RegionalEventController.DataAccess.Models;
using RegionalEventController.Controller.ProcessorModules;
using RegionalEventController.Common;
using System.Collections.Generic;
using System.Data.Entity;
using System;
using System.Reflection;

namespace RegionalEventController.Controller.UpdateServices
{
    public class ValidatedScanCodeInsertionService : IUpdateService
    {
        private ILogger<ValidatedScanCodeInsertionService> logger;
        private IconContext iconContext;
        private List<IrmaNewItem> irmaNewItems;
        private IBulkCommandHandler<InsertIrmaItemSubscriptionsToIconBulkCommand> insertIrmaItemSubscriptionsToIconBulkCommandHandler;
        private IBulkCommandHandler<InsertEventQueuesToIconBulkCommand> insertEventQueuesToIconBulkCommandHandler;
        private INewItemProcessingModule newItemProcessingModule;

        public ValidatedScanCodeInsertionService(ILogger<ValidatedScanCodeInsertionService> logger,
            IconContext iconContext,
            List<IrmaNewItem> irmaNewItems,
            IBulkCommandHandler<InsertIrmaItemSubscriptionsToIconBulkCommand> insertIrmaItemSubscriptionsToIconBulkCommandHandler,
            IBulkCommandHandler<InsertEventQueuesToIconBulkCommand> insertEventQueuesToIconBulkCommandHandler,
            INewItemProcessingModule newItemProcessingModule)
        {
            this.logger = logger;
            this.iconContext = iconContext;
            this.irmaNewItems = irmaNewItems;
            this.insertIrmaItemSubscriptionsToIconBulkCommandHandler = insertIrmaItemSubscriptionsToIconBulkCommandHandler;
            this.insertEventQueuesToIconBulkCommandHandler = insertEventQueuesToIconBulkCommandHandler;
            this.newItemProcessingModule = newItemProcessingModule;
        }

        public void UpdateBulk()
        {
            using (DbContextTransaction transaction = iconContext.Database.BeginTransaction())
            {
                try
                {
                    List<IRMAItemSubscription> subscriptions = new List<IRMAItemSubscription>();
                    List<EventQueue> EventQueueEntries = new List<EventQueue>();

                    foreach (IrmaNewItem item in irmaNewItems)
                    {
                        subscriptions.Add(new IRMAItemSubscription
                        {
                            regioncode = item.IrmaItem.regioncode,
                            identifier = item.IrmaItem.identifier
                        });

                        EventQueueEntries.Add(new EventQueue
                        {
                            EventId = EventTypes.NewIrmaItem,
                            EventMessage = item.IrmaItem.identifier,
                            EventReferenceId = item.IconItemId,
                            RegionCode = item.IrmaItem.regioncode
                        });
                    }

                    InsertIrmaItemSubscriptionsToIconBulkCommand insertIrmaItemSubscriptionsToIconBulkCommand = new InsertIrmaItemSubscriptionsToIconBulkCommand { IrmaNewItemSubscriptions = subscriptions };
                    insertIrmaItemSubscriptionsToIconBulkCommandHandler.Execute(insertIrmaItemSubscriptionsToIconBulkCommand);

                    InsertEventQueuesToIconBulkCommand insertEventQueuesToIconBulkCommand = new InsertEventQueuesToIconBulkCommand { EventQueueEntries = EventQueueEntries };
                    insertEventQueuesToIconBulkCommandHandler.Execute(insertEventQueuesToIconBulkCommand);


                    foreach (IrmaNewItem item in irmaNewItems)
                    {
                        item.ProcessedByController = true;
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    ExceptionHandler<ValidatedScanCodeInsertionService>.logger = this.logger;
                    ExceptionHandler<ValidatedScanCodeInsertionService>.HandleException("An unhandled exception occurred in the bulk update for validated Scan Codes. ", ex, this.GetType(), MethodBase.GetCurrentMethod());

                    foreach (IrmaNewItem item in irmaNewItems)
                    {
                        item.ProcessedByController = false;
                        item.FailureReason = ex.Message;
                    }

                    transaction.Rollback();
                    throw new Exception("Unable to successfully complete the bulk update for validated Scan Codes.");
                }
            }
        }

        public void UpdateRowByRow()
        {
            foreach (IrmaNewItem newItem in irmaNewItems)
            {
                using (DbContextTransaction transaction = iconContext.Database.BeginTransaction())
                {
                    try
                    {
                        newItemProcessingModule.CreateEventQueueEntry(EventTypes.NewIrmaItem, newItem.IrmaItem.identifier, newItem.IconItemId, newItem.IrmaItem.regioncode);
                        newItemProcessingModule.CreateIrmaItemSubscription(newItem.IrmaItem.regioncode, newItem.IrmaItem.identifier);

                        newItem.ProcessedByController = true;

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler<ValidatedScanCodeInsertionService>.logger = this.logger;
                        ExceptionHandler<ValidatedScanCodeInsertionService>.HandleException(String.Format("An unhandled exception occurred in the Row-by-Row update for validated Scan Code {0} in region {1}. ", newItem.IrmaItem.identifier, newItem.IrmaItem.regioncode), ex, this.GetType(), MethodBase.GetCurrentMethod());

                        newItem.ProcessedByController = false;
                        newItem.FailureReason = ex.Message;
                        transaction.Rollback();
                        continue;
                    }
                }
            }
        }
    }
}
