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
    public class BrandNewUpcInsertionService : IUpdateService
    {
        private ILogger<BrandNewUpcInsertionService> logger;
        private IconContext iconContext;
        private List<IrmaNewItem> irmaNewItems;
        private IBulkCommandHandler<InsertIrmaItemSubscriptionsToIconBulkCommand> insertIrmaItemSubscriptionsToIconBulkCommandHandler;
        private IBulkCommandHandler<InsertIrmaItemsToIconBulkCommand> insertIrmaItemsToIconBulkCommandHandler;
        private INewItemProcessingModule newItemProcessingModule;

        public BrandNewUpcInsertionService(ILogger<BrandNewUpcInsertionService> logger,
            IconContext iconContext,
            List<IrmaNewItem> irmaNewItems,
            IBulkCommandHandler<InsertIrmaItemSubscriptionsToIconBulkCommand> insertIrmaItemSubscriptionsToIconBulkCommandHandler,
            IBulkCommandHandler<InsertIrmaItemsToIconBulkCommand> insertIrmaItemsToIconBulkCommandHandler,
            INewItemProcessingModule newItemProcessingModule)
        {
            this.logger = logger;
            this.iconContext = iconContext;
            this.irmaNewItems = irmaNewItems;
            this.insertIrmaItemSubscriptionsToIconBulkCommandHandler = insertIrmaItemSubscriptionsToIconBulkCommandHandler;
            this.insertIrmaItemsToIconBulkCommandHandler = insertIrmaItemsToIconBulkCommandHandler;
            this.newItemProcessingModule = newItemProcessingModule;
        }

        public void UpdateBulk()
        {
            using (DbContextTransaction transaction = iconContext.Database.BeginTransaction())
            {
                try
                {
                    List<IRMAItemSubscription> subscriptions = new List<IRMAItemSubscription>();
                    List<IRMAItem> IrmaItemEntries = new List<IRMAItem>();

                    foreach (IrmaNewItem item in irmaNewItems)
                    {
                        subscriptions.Add(new IRMAItemSubscription
                        {
                            regioncode = item.IrmaItem.regioncode,
                            identifier = item.IrmaItem.identifier,
                            insertDate = DateTime.Now
                        });

                        try
                        {
                            item.IrmaItem.taxClassID = item.IrmaTaxClass.Split(' ')[0].MapToTaxId();                            
                        }
                        catch
                        {
                            item.IrmaItem.taxClassID = null;
                        }
                        item.IrmaItem.nationalClassID = item.IrmaNationalClass.MapToNationalClassId();
                        item.IrmaItem.posDescription = item.IrmaItem.PrependedPosDescription();
                        IrmaItemEntries.Add(item.IrmaItem);
                    }

                    InsertIrmaItemSubscriptionsToIconBulkCommand insertIrmaItemSubscriptionsToIconBulkCommand = new InsertIrmaItemSubscriptionsToIconBulkCommand { IrmaNewItemSubscriptions = subscriptions };
                    insertIrmaItemSubscriptionsToIconBulkCommandHandler.Execute(insertIrmaItemSubscriptionsToIconBulkCommand);

                    InsertIrmaItemsToIconBulkCommand insertIrmaItemsToIconBulkCommand = new InsertIrmaItemsToIconBulkCommand { irmaNewItems = IrmaItemEntries };
                    insertIrmaItemsToIconBulkCommandHandler.Execute(insertIrmaItemsToIconBulkCommand);


                    foreach (IrmaNewItem item in irmaNewItems)
                    {
                        item.ProcessedByController = true;
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    ExceptionHandler<BrandNewUpcInsertionService>.logger = this.logger;
                    ExceptionHandler<BrandNewUpcInsertionService>.HandleException("An unhandled exception occurred in the bulk update for brand new UPC Scan Codes. ", ex, this.GetType(), MethodBase.GetCurrentMethod());

                    foreach (IrmaNewItem item in irmaNewItems)
                    {
                        item.ProcessedByController = false;
                        item.FailureReason = ex.Message;
                    }
                    transaction.Rollback();
                    throw new Exception("Unable to successfully complete the bulk update for Brand New UPCs.");
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
                        newItemProcessingModule.InsertIrmaItemToIcon(newItem.IrmaItem);
                        newItemProcessingModule.CreateIrmaItemSubscription(newItem.IrmaItem.regioncode, newItem.IrmaItem.identifier);

                        newItem.ProcessedByController = true;

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler<BrandNewUpcInsertionService>.logger = this.logger;
                        ExceptionHandler<BrandNewUpcInsertionService>.HandleException(String.Format("An unhandled exception occurred in the Row-by-Row update for brand new UPC Scan Code {0} in region {1}. ", newItem.IrmaItem.identifier, newItem.IrmaItem.regioncode), ex, this.GetType(), MethodBase.GetCurrentMethod());

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
