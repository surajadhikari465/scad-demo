namespace Icon.Monitoring.Monitors
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Icon.Logging;
    using Icon.Monitoring.Common.Enums;
    using Icon.Monitoring.Common.Opsgenie;
    using Icon.Monitoring.Common.Settings;
    using Icon.Monitoring.DataAccess.Queries;
    using OpsgenieAlert;

    public class InStockDequeueProcessMonitor : TimedControllerMonitor
    {
        private const string InStockOpsgenieDescription = "InStock Dequeue Service is taking too long for the following regions: ";

        private readonly IQueryByRegionHandler<GetInStockOldestUnprocessedRecordDateByQueueTableParameters, DateTime?> oldestUnprocessedRecordQuery;
        private readonly IOpsgenieTrigger opsgenieTrigger;
        private IInStockDequeueProcessMonitorSettings inStockDequeueProcessMonitorSettings;

        public InStockDequeueProcessMonitor(
            IMonitorSettings settings,
            IInStockDequeueProcessMonitorSettings inStockDequeueProcessMonitorSettings,
            IQueryByRegionHandler<GetInStockOldestUnprocessedRecordDateByQueueTableParameters, DateTime?> oldestUnprocessedRecordQuery,
            IOpsgenieTrigger opsgenieTrigger,
            ILogger logger)
        {
            this.settings = settings;
            this.oldestUnprocessedRecordQuery = oldestUnprocessedRecordQuery;
            this.opsgenieTrigger = opsgenieTrigger;
            this.logger = logger;
            this.inStockDequeueProcessMonitorSettings = inStockDequeueProcessMonitorSettings;
        }

        protected override void TimedCheckStatusAndNotify()
        {
            var opsgenieDetails = new Dictionary<string, string>();
            var opsgenieDescription = new StringBuilder(InStockOpsgenieDescription);

            int numberFailed = 0;

            var regions = inStockDequeueProcessMonitorSettings.InStockDequeueProcessRegions;


            foreach (var region in regions)
            {
                //changes region to iterate through all dbs
                var parseEnum = Enum.TryParse(region, out IrmaRegions irmaRegion);
                this.oldestUnprocessedRecordQuery.TargetRegion = irmaRegion;
                var queryOrderQueueParameters = new GetInStockOldestUnprocessedRecordDateByQueueTableParameters { QueueTableName = "amz.OrderQueue" };
                var queryReceiptQueueParameters = new GetInStockOldestUnprocessedRecordDateByQueueTableParameters { QueueTableName = "amz.ReceiptQueue" };
                var queryTransferQueueParameters = new GetInStockOldestUnprocessedRecordDateByQueueTableParameters { QueueTableName = "amz.TransferQueue" };
                var queryInventoryQueueParameters = new GetInStockOldestUnprocessedRecordDateByQueueTableParameters { QueueTableName = "amz.InventoryQueue" };

                if (this.AreRecordsGettingProcessed(queryOrderQueueParameters))
                {
                    opsgenieDescription.Append(region).Append(" ");
                    opsgenieDetails.Add("Order Dequeue" + ":" + region.ToString(), region.ToString());
                    numberFailed++;
                }
                if (this.AreRecordsGettingProcessed(queryReceiptQueueParameters))
                {
                    opsgenieDescription.Append(region).Append(" ");
                    opsgenieDetails.Add("Receipt Dequeue" + ":" + region.ToString(), region.ToString());
                    numberFailed++;
                }
                if (this.AreRecordsGettingProcessed(queryTransferQueueParameters))
                {
                    opsgenieDescription.Append(region).Append(" ");
                    opsgenieDetails.Add("Transfer Order Dequeue" + ":" + region.ToString(), region.ToString());
                    numberFailed++;
                }

                if (this.AreRecordsGettingProcessed(queryInventoryQueueParameters))
                {
                    opsgenieDescription.Append(region).Append(" ");
                    opsgenieDetails.Add("Inventory Dequeue" + ":" + region.ToString(), region.ToString());
                    numberFailed++;
                }
            }

            if (numberFailed > 0)
            {
                logger.Info("InStock Dequeue Process is taking too long.");
                OpsgenieResponse response =
                    this.opsgenieTrigger.TriggerAlert("InStock Dequeue Process Issue", opsgenieDescription.ToString(), opsgenieDetails);
            }
            else
            {
                logger.Info("The InStock Dequeue Process Monitor has not detected a job that is taking too long.");
            }
        }

        private bool AreRecordsGettingProcessed(GetInStockOldestUnprocessedRecordDateByQueueTableParameters queryParameters)
        {
            var oldestUnprocessedRecordInsertDateUtc = this.oldestUnprocessedRecordQuery.Search(queryParameters);

            if (oldestUnprocessedRecordInsertDateUtc != null)
            {
                TimeSpan span = DateTime.UtcNow - (DateTime)oldestUnprocessedRecordInsertDateUtc;
                if (span.TotalMinutes > inStockDequeueProcessMonitorSettings.NumberOfMaximumMinutesRecordCanBeInUnprocessedStatus)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

    }
}