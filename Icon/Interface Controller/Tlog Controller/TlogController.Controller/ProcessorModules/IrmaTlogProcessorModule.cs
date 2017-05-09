using Icon.Logging;
using TlogController.Common;
using System;
using Irma.Framework;
using System.Collections.Generic;
using TlogController.DataAccess.Models;
using TlogController.DataAccess.BulkCommands;
using TlogController.DataAccess.Interfaces;
using System.Data.SqlClient;
using System.Reflection;
using System.Data.Entity;


namespace TlogController.Controller.ProcessorModules
{
    public class IrmaTlogProcessorModule : IIrmaTlogProcessorModule
    {
        private ILogger<IrmaTlogProcessorModule> logger;
        private IrmaContext irmaContext;
        private IBulkCommandHandler<BulkUpdateSalesSumByitemCommand> bulkUpdateSalesSumByitemCommandHandler;
        private IIconTlogProcessorModule iconTlogProcessorModule;

        public IrmaTlogProcessorModule(
            ILogger<IrmaTlogProcessorModule> logger,
            IrmaContext irmaContext,
            IBulkCommandHandler<BulkUpdateSalesSumByitemCommand> bulkUpdateSalesSumByitemCommandHandler,
            IIconTlogProcessorModule iconTlogProcessorModule
            )
        {
            this.logger = logger;
            this.irmaContext = irmaContext;
            this.bulkUpdateSalesSumByitemCommandHandler = bulkUpdateSalesSumByitemCommandHandler;
            this.iconTlogProcessorModule = iconTlogProcessorModule;
        }

        public void PushSalesSumByitemDataInBulkToIrma(IrmaTlog irmaTlog)
        {
            if (irmaTlog.ItemMovementToIrmaList != null && irmaTlog.ItemMovementToIrmaList.Count > 0)
            {
                using (DbContextTransaction transaction = irmaContext.Database.BeginTransaction())
                {
                    try
                    { 
                        BulkUpdateSalesSumByitemCommand bulkUpdateSalesSumByitemCommand = new BulkUpdateSalesSumByitemCommand { ItemMovementsToIrma = irmaTlog.ItemMovementToIrmaList };
                        bulkUpdateSalesSumByitemCommandHandler.Execute(bulkUpdateSalesSumByitemCommand);

                        foreach (ItemMovementTransaction itemMovementTransaction in irmaTlog.ItemMovementTransactionList)
                        {
                            itemMovementTransaction.Processed = true;
                        }

                        iconTlogProcessorModule.DeleteProcessedItemMovement(irmaTlog.ItemMovementTransactionList);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler<IrmaTlogProcessorModule>.logger = this.logger;
                        ExceptionHandler<IrmaTlogProcessorModule>.HandleException(String.Format("An unhandled exception occurred in the bulk update for Sales_SumByitem records in {0} region. ", irmaTlog.RegionCode), ex, this.GetType(), MethodBase.GetCurrentMethod());

                        foreach (ItemMovementTransaction itemMovementTransaction in irmaTlog.ItemMovementTransactionList)
                        {
                            itemMovementTransaction.Processed = false;
                        }

                        transaction.Rollback();
                        throw new Exception("Unable to successfully complete the bulk update for Sales_SumByitem records.");
                    }
                }
            }
        }

        public void PushSalesSumByitemDataTransactionByTransactionToIrma(IrmaTlog irmaTlog)
        {
            if (irmaTlog.ItemMovementTransactionList != null && irmaTlog.ItemMovementTransactionList.Count > 0)
            {
                foreach (ItemMovementTransaction itemMovementTransaction in irmaTlog.ItemMovementTransactionList)
                {
                    using (DbContextTransaction transaction = irmaContext.Database.BeginTransaction())
                    {
                        try
                        {
                            List<ItemMovementToIrma> itemMovementToIrmaPerTransaction = irmaTlog.ItemMovementToIrmaList.GetRange(itemMovementTransaction.FirstItemMovementToIrmaIndex, itemMovementTransaction.LastItemMovementToIrmaIndex - itemMovementTransaction.FirstItemMovementToIrmaIndex + 1);
                            BulkUpdateSalesSumByitemCommand bulkUpdateSalesSumByitemCommand = new BulkUpdateSalesSumByitemCommand { ItemMovementsToIrma = itemMovementToIrmaPerTransaction };
                            bulkUpdateSalesSumByitemCommandHandler.Execute(bulkUpdateSalesSumByitemCommand);

                            itemMovementTransaction.Processed = true;

                            iconTlogProcessorModule.DeleteProcessedItemMovement(irmaTlog.ItemMovementTransactionList);
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            ExceptionHandler<IrmaTlogProcessorModule>.logger = this.logger;
                            ExceptionHandler<IrmaTlogProcessorModule>.HandleException(String.Format("An unhandled exception occurred in the transaction-by-transaction update for Sales_SumByitem record in {0} region. ", irmaTlog.RegionCode), ex, this.GetType(), MethodBase.GetCurrentMethod());

                            itemMovementTransaction.Processed = false;
                            transaction.Rollback();
                            continue;
                        }
                    }
                }
            }
        }
    }
}
