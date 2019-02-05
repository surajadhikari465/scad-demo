namespace Icon.Monitoring.Monitors
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Icon.Monitoring.Common;
    using Icon.Monitoring.Common.Enums;
    using Icon.Monitoring.Common.Opsgenie;
    using Icon.Monitoring.Common.Settings;
    using Icon.Monitoring.DataAccess.Queries;
    using Logging;
    using OpsgenieAlert;

    public class MammothPriceControllerMonitor : TimedControllerMonitor
    {
        private const string MammothPriceControllerOpsgenieDescription = "Mammoth Price Controller is taking too long for the following regions: ";
        private readonly IQueryByRegionHandler<GetMammothPriceChangeQueueIdQueryParameters, int> priceChangeQueueQuery;
        private readonly IOpsgenieTrigger opsgenieTrigger;

        public MammothPriceControllerMonitor(
            IMonitorSettings settings,
            IOpsgenieTrigger opsgenieTrigger,
             IQueryByRegionHandler<GetMammothPriceChangeQueueIdQueryParameters, int> priceChangeQueueQuery,
            ILogger logger)
        {
            this.settings = settings;
            this.logger = logger;
            this.priceChangeQueueQuery = priceChangeQueueQuery;
            this.opsgenieTrigger = opsgenieTrigger;
        }

        protected override void TimedCheckStatusAndNotify()
        {
            var opsgenieDetails = new Dictionary<string, string>();
            var opsgenieDescription = new StringBuilder(MammothPriceControllerOpsgenieDescription);
            var queryParameters = new GetMammothPriceChangeQueueIdQueryParameters();
            int numberFailed = 0;

            foreach (IrmaRegions region in Enum.GetValues(typeof(IrmaRegions)))
            {
                //changes region to iterate through all dbs
                this.priceChangeQueueQuery.TargetRegion = region;

                if (this.CheckMessageQueueId(region))
                {
                    opsgenieDescription.Append(region).Append(" ");
                    opsgenieDetails.Add("Region:" + region.ToString(), region.ToString());
                    numberFailed++;
                }
            }

            if (numberFailed > 0)
            {
                logger.Info("The Mammoth Price Controller Monitor has detected a region that is taking too long.");
                OpsgenieResponse response =
                    this.opsgenieTrigger.TriggerAlert("Mammoth Price Controller Monitor Issue", opsgenieDescription.ToString(), opsgenieDetails);
            }
            else
            {
                logger.Info("The Mammoth Price Controller Monitor has not detected a region that is taking too long.");
            }
        }

        private bool CheckMessageQueueId(IrmaRegions region)
        {
            int id = this.priceChangeQueueQuery.Search(new GetMammothPriceChangeQueueIdQueryParameters());

            if (MammothPriceChangeQueueCache.IrmaRegionMapper[region].LastMessageQueueId == id
                && MammothPriceChangeQueueCache.IrmaRegionMapper[region].NumberOfTimesMatched == 0
                && id != 0)
            {
                MammothPriceChangeQueueCache.IrmaRegionMapper[region].NumberOfTimesMatched++;
                return true;
            }
            else if (MammothPriceChangeQueueCache.IrmaRegionMapper[region].LastMessageQueueId == id
                && MammothPriceChangeQueueCache.IrmaRegionMapper[region].NumberOfTimesMatched > 0)
            {
                MammothPriceChangeQueueCache.IrmaRegionMapper[region].NumberOfTimesMatched++;
                return false;
            }
            else
            {
                MammothPriceChangeQueueCache.IrmaRegionMapper[region].LastMessageQueueId = id;
                MammothPriceChangeQueueCache.IrmaRegionMapper[region].NumberOfTimesMatched = 0;
                return false;
            }
        }

        private void TriggerOpsgenieIncident(string description, Dictionary<string, string> jsonDetails)
        {
            OpsgenieResponse response = this.opsgenieTrigger.TriggerAlert("Mammoth Price Controller Issue",description, jsonDetails);
        }
    }
}
