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

    public class MammothItemLocaleControllerMonitor : TimedControllerMonitor
    {
        private const string MammothItemLocaleControllerOpsgenieDescription = "Mammoth Item Locale Controller is taking too long for the following regions: ";
        private readonly IQueryByRegionHandler<GetMammothItemLocaleChangeQueueIdQueryParameters, int> itemLocaleChangeQueueQuery;
        private readonly IOpsgenieTrigger opsgenieTrigger;

        public MammothItemLocaleControllerMonitor(
            IMonitorSettings settings,
            IOpsgenieTrigger opsgenieTrigger,
            IQueryByRegionHandler<GetMammothItemLocaleChangeQueueIdQueryParameters, int> itemLocaleChangeQueueQuery,
            ILogger logger)
        {
            this.settings = settings;
            this.logger = logger;
            this.itemLocaleChangeQueueQuery = itemLocaleChangeQueueQuery;
            this.opsgenieTrigger = opsgenieTrigger;
        }

        protected override void TimedCheckStatusAndNotify()
        {
            try
            {
                var opsGenieDetails = new Dictionary<string, string>();
                var opsGenieDescription = new StringBuilder(MammothItemLocaleControllerOpsgenieDescription);
                var queryParameters = new GetMammothItemLocaleChangeQueueIdQueryParameters();
                int numberFailed = 0;

                foreach (IrmaRegions region in Enum.GetValues(typeof(IrmaRegions)))
                {
                    //changes region to iterate through all dbs
                    this.itemLocaleChangeQueueQuery.TargetRegion = region;

                    if (this.CheckMessageQueueId(region))
                    {
                        opsGenieDescription.Append(region).Append(" ");
                        opsGenieDetails.Add("Region:" + region.ToString(), region.ToString());
                        numberFailed++;
                    }
                }

                if (numberFailed > 0)
                {
                    logger.Info("The Mammoth Item Locale Controller Monitor has detected a region that is taking too long.");
                    OpsgenieResponse response =
                        this.opsgenieTrigger.TriggerAlert(" Mammoth Item Locale Issue", opsGenieDescription.ToString(), opsGenieDetails);
                }
                else
                {
                    logger.Info("The Mammoth Item Locale Controller Monitor has not detected a region that is taking too long.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("{0} failed to execute.", this.GetType().Name));
                Console.WriteLine(ex.Message);
                logger.Error("The Mammoth Item Locale Controller Monitor has thrown an error:" + ex.Message);
            }
        }

        private bool CheckMessageQueueId(IrmaRegions region)
        {
            int id = this.itemLocaleChangeQueueQuery.Search(new GetMammothItemLocaleChangeQueueIdQueryParameters());

            if (MammothItemLocaleChangeQueueCache.IrmaRegionMapper[region].LastMessageQueueId == id
                && MammothItemLocaleChangeQueueCache.IrmaRegionMapper[region].NumberOfTimesMatched == 0
                && id != 0)
            {
                MammothItemLocaleChangeQueueCache.IrmaRegionMapper[region].NumberOfTimesMatched++;
                return true;
            }
            else if (MammothItemLocaleChangeQueueCache.IrmaRegionMapper[region].LastMessageQueueId == id
                && MammothItemLocaleChangeQueueCache.IrmaRegionMapper[region].NumberOfTimesMatched > 0)
            {
                MammothItemLocaleChangeQueueCache.IrmaRegionMapper[region].NumberOfTimesMatched++;
                return false;
            }
            else
            {
                MammothItemLocaleChangeQueueCache.IrmaRegionMapper[region].LastMessageQueueId = id;
                MammothItemLocaleChangeQueueCache.IrmaRegionMapper[region].NumberOfTimesMatched = 0;
                return false;
            }
        }
    }
}
