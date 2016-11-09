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
    public class SubscriptionOnlyInsertionService : IUpdateService
    {
        private ILogger<SubscriptionOnlyInsertionService> logger;
        private IconContext iconContext;
        private List<IrmaNewItem> irmaNewItems;
        private IBulkCommandHandler<InsertIrmaItemSubscriptionsToIconBulkCommand> insertIrmaItemSubscriptionsToIconBulkCommandHandler;
        private INewItemProcessingModule newItemProcessingModule;

        public SubscriptionOnlyInsertionService(ILogger<SubscriptionOnlyInsertionService> logger,
            IconContext iconContext,
            List<IrmaNewItem> irmaNewItems,
            IBulkCommandHandler<InsertIrmaItemSubscriptionsToIconBulkCommand> insertIrmaItemSubscriptionsToIconBulkCommandHandler,
            INewItemProcessingModule newItemProcessingModule)
        {
            this.logger = logger;
            this.iconContext = iconContext;
            this.irmaNewItems = irmaNewItems;
            this.insertIrmaItemSubscriptionsToIconBulkCommandHandler = insertIrmaItemSubscriptionsToIconBulkCommandHandler;
            this.newItemProcessingModule = newItemProcessingModule;
        }

        public void UpdateBulk()
        {
            using (DbContextTransaction transaction = iconContext.Database.BeginTransaction())
            {
                try
                {
                    List<IRMAItemSubscription> subscriptions = new List<IRMAItemSubscription>();

                    foreach (IrmaNewItem item in irmaNewItems)
                    {
                        subscriptions.Add(new IRMAItemSubscription
                        {
                            regioncode = item.IrmaItem.regioncode,
                            identifier = item.IrmaItem.identifier
                        });
                    }

                    InsertIrmaItemSubscriptionsToIconBulkCommand insertIrmaItemSubscriptionsToIconBulkCommand = new InsertIrmaItemSubscriptionsToIconBulkCommand { IrmaNewItemSubscriptions = subscriptions };
                    insertIrmaItemSubscriptionsToIconBulkCommandHandler.Execute(insertIrmaItemSubscriptionsToIconBulkCommand);

                    foreach (IrmaNewItem item in irmaNewItems)
                    {
                        item.ProcessedByController = true;
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    ExceptionHandler<SubscriptionOnlyInsertionService>.logger = this.logger;
                    ExceptionHandler<SubscriptionOnlyInsertionService>.HandleException("An unhandled exception occurred in the bulk update for Subscription Only Scan Codes. ", ex, this.GetType(), MethodBase.GetCurrentMethod());

                    foreach (IrmaNewItem item in irmaNewItems)
                    {
                        item.ProcessedByController = false;
                        item.FailureReason = ex.Message;
                    }

                    transaction.Rollback();
                    throw new Exception("Unable to successfully complete the bulk update for Scan Codes that need Subscription only.");
                }
            }
        }

        public void UpdateRowByRow()
        {
            foreach (IrmaNewItem newItem in irmaNewItems)
            {
                try
                {
                    newItemProcessingModule.CreateIrmaItemSubscription(newItem.IrmaItem.regioncode, newItem.IrmaItem.identifier);

                    newItem.ProcessedByController = true; ;
                }
                catch (Exception ex)
                {
                    ExceptionHandler<SubscriptionOnlyInsertionService>.logger = this.logger;
                    ExceptionHandler<SubscriptionOnlyInsertionService>.HandleException(String.Format("An unhandled exception occurred in the Row-by-Row update for Subscription Only Scan Code {0} in region {1}. ", newItem.IrmaItem.identifier, newItem.IrmaItem.regioncode), ex, this.GetType(), MethodBase.GetCurrentMethod());

                    newItem.ProcessedByController = false;
                    newItem.FailureReason = ex.Message;
                    continue;
                }
            }
        }
    }
}