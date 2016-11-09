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
    public class BrandNewUpcWithoutSubscriptionInsertionService : IUpdateService
    {
        private ILogger<BrandNewUpcWithoutSubscriptionInsertionService> logger;
        private IconContext iconContext;
        private List<IrmaNewItem> irmaNewItems;
        private IBulkCommandHandler<InsertIrmaItemsToIconBulkCommand> insertIrmaItemsToIconBulkCommandHandler;
        private INewItemProcessingModule newItemProcessingModule;

        public BrandNewUpcWithoutSubscriptionInsertionService(ILogger<BrandNewUpcWithoutSubscriptionInsertionService> logger,
            IconContext iconContext,
            List<IrmaNewItem> irmaNewItems,
            IBulkCommandHandler<InsertIrmaItemsToIconBulkCommand> insertIrmaItemsToIconBulkCommandHandler,
            INewItemProcessingModule newItemProcessingModule)
        {
            this.logger = logger;
            this.iconContext = iconContext;
            this.irmaNewItems = irmaNewItems;
            this.insertIrmaItemsToIconBulkCommandHandler = insertIrmaItemsToIconBulkCommandHandler;
            this.newItemProcessingModule = newItemProcessingModule;
        }

        public void UpdateBulk()
        {
            using (DbContextTransaction transaction = iconContext.Database.BeginTransaction())
            {
                try
                {
                    List<IRMAItem> IrmaItemEntries = new List<IRMAItem>();

                    foreach (IrmaNewItem item in irmaNewItems)
                    {
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
                    ExceptionHandler<BrandNewUpcWithoutSubscriptionInsertionService>.logger = this.logger;
                    ExceptionHandler<BrandNewUpcWithoutSubscriptionInsertionService>.HandleException("An unhandled exception occurred in the bulk update for brand new UPC Scan Codes. ", ex, this.GetType(), MethodBase.GetCurrentMethod());

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
                try
                {
                    newItemProcessingModule.InsertIrmaItemToIcon(newItem.IrmaItem);
                    newItem.ProcessedByController = true;
                }
                catch (Exception ex)
                {
                    ExceptionHandler<BrandNewUpcWithoutSubscriptionInsertionService>.logger = this.logger;
                    ExceptionHandler<BrandNewUpcWithoutSubscriptionInsertionService>.HandleException(String.Format("An unhandled exception occurred in the Row-by-Row update for brand new UPC Scan Code {0} in region {1}. ", newItem.IrmaItem.identifier, newItem.IrmaItem.regioncode), ex, this.GetType(), MethodBase.GetCurrentMethod());

                    newItem.ProcessedByController = false;
                    newItem.FailureReason = ex.Message;
                    continue;
                }
            }
        }
    }
}
