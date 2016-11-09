using Icon.Framework;
using Icon.Logging;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using TlogController.Common;
using TlogController.Controller.ProcessorModules;
using TlogController.DataAccess.BulkCommands;
using TlogController.DataAccess.Infrastructure;
using TlogController.DataAccess.Interfaces;
using TlogController.DataAccess.Models;
using TlogController.DataAccess.Queries;

namespace TlogController.Controller.Processors
{
    public class TlogProcessor : ITlogProcessor
    {
        private IIrmaTlogProcessorModule irmaTlogProcessorModule;
        private IIrmaTlogProcessor irmaTlogProcessor;

        private ILogger<TlogProcessor> logger;
        private IIconTlogProcessorModule iconTlogProcessorModule;
        
        public TlogProcessor(
            ILogger<TlogProcessor> logger,
            IIconTlogProcessorModule iconTlogProcessorModule)
        {
            this.logger = logger;
            this.iconTlogProcessorModule = iconTlogProcessorModule;
        }
        public void Run()
        {
            iconTlogProcessorModule.LoadBusinessUnitToRegionCodeMapping();

            List<IrmaTlog> irmaTlogs = new List<IrmaTlog>();
            irmaTlogs = iconTlogProcessorModule.GroupTlogEntiesByRegion();

            while (irmaTlogs != null && irmaTlogs.Count > 0)
            {
                //There will be at most one IrmaTlog per region
                foreach (IrmaTlog irmaTlog in irmaTlogs)
                {
                    if (!String.IsNullOrEmpty(irmaTlog.RegionCode)) 
                    {
                        irmaTlogProcessorModule = BuildIrmaTlogProcessorModule(irmaTlog.RegionCode);
                        irmaTlogProcessor = BuildIrmaTlogProcessor(irmaTlog);

                        logger.Info(String.Format("Start processing the {0} region.", irmaTlog.RegionCode));

                        irmaTlogProcessor.PopulateTlogReprocessRequests();

                        irmaTlogProcessor.UpdateSalesSumByitem();
                    }

                    List<ItemMovementTransaction> failedItemMovementTransactionList = irmaTlog.ItemMovementTransactionList.Select(s => s).Where(s => s.Processed == false).ToList();
                    if (failedItemMovementTransactionList != null && failedItemMovementTransactionList.Count > 0)
                    {
                        try
                        {
                            iconTlogProcessorModule.DeleteProcessedItemMovement(failedItemMovementTransactionList);
                        }
                        catch (Exception ex)
                        {
                            ExceptionHandler<TlogProcessor>.logger = this.logger;
                            ExceptionHandler<TlogProcessor>.HandleException("An unhandled exception occurred in deleting processed ItemMovement records. ", ex, this.GetType(), MethodBase.GetCurrentMethod());
                        }
                    }
                }

                irmaTlogs = iconTlogProcessorModule.GroupTlogEntiesByRegion();
            }
        }

        
        private IIrmaTlogProcessorModule BuildIrmaTlogProcessorModule(string region)
        {
            var Logger = new NLogLoggerInstance<IrmaTlogProcessorModule>(StartupOptions.Instance.ToString());

            var irmaContext = new IrmaContext(ConnectionBuilder.GetConnection(region));

            var bulkUpdateSalesSumByitemCommandHandler = new BulkUpdateSalesSumByitemCommandHandler(new NLogLoggerInstance<BulkUpdateSalesSumByitemCommandHandler>(StartupOptions.Instance.ToString()), irmaContext);
            var bulkInsertTlogReprocessRequestsCommandHandler = new BulkInsertTlogReprocessRequestsCommandHandler(new NLogLoggerInstance<BulkInsertTlogReprocessRequestsCommandHandler>(StartupOptions.Instance.ToString()), irmaContext);
            return new IrmaTlogProcessorModule(Logger, irmaContext, bulkUpdateSalesSumByitemCommandHandler, bulkInsertTlogReprocessRequestsCommandHandler, iconTlogProcessorModule);
        }

        private IIrmaTlogProcessor BuildIrmaTlogProcessor(IrmaTlog irmaTlog)
        {
            return new IrmaTlogProcessor(iconTlogProcessorModule, irmaTlogProcessorModule, irmaTlog);
        }
    }
}
