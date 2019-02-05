namespace Icon.Monitoring.Monitors
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Icon.Logging;
    using Icon.Monitoring.Common;
    using Icon.Monitoring.Common.Constants;
    using Icon.Monitoring.Common.Enums;
    using Icon.Monitoring.Common.Opsgenie;
    using Icon.Monitoring.Common.Settings;
    using Icon.Monitoring.DataAccess.Model;
    using Icon.Monitoring.DataAccess.Queries;
    using OpsgenieAlert;

    public class PosPushJobMonitor : TimedControllerMonitor
    {
        private const string PosPushTimeConfigPrefix = "PosPushTimeSpan_";
        private const string PosPushOpsgenieDescription = "POS Push is taking too long for the following regions: ";

        private readonly IQueryByRegionHandler<GetIrmaJobStatusQueryParameters, JobStatus> jobStatusQuery;
        private readonly IOpsgenieTrigger opsgenieTrigger;

        public PosPushJobMonitor(
            IMonitorSettings settings,
            IQueryByRegionHandler<GetIrmaJobStatusQueryParameters, JobStatus> jobStatusQuery,
            IOpsgenieTrigger opsgenieTrigger,
            ILogger logger)
        {
            this.settings = settings;
            this.jobStatusQuery = jobStatusQuery;
            this.opsgenieTrigger = opsgenieTrigger;
            this.logger = logger;
        }

        protected override void TimedCheckStatusAndNotify()
        {
            var opsgenieDetails = new Dictionary<string, string>();
            var opsgenieDescription = new StringBuilder(PosPushOpsgenieDescription);
            var queryParameters = new GetIrmaJobStatusQueryParameters { Classname = IrmaJobClassNames.POSPushJob };
            int numberFailed = 0;

            foreach (IrmaRegions region in Enum.GetValues(typeof(IrmaRegions)))
            {
                //changes region to iterate through all dbs
                this.jobStatusQuery.TargetRegion = region;

                if (this.CheckPosPushStatus(queryParameters))
                {
                    opsgenieDescription.Append(region).Append(" ");
                    opsgenieDetails.Add(IrmaJobClassNames.POSPushJob + ":" + region.ToString(), region.ToString());
                    numberFailed++;
                }
            }

            if (numberFailed > 0)
            {
                logger.Info("The POS Push Monitor has detected a job that is taking too long.");
                OpsgenieResponse response =
                    this.opsgenieTrigger.TriggerAlert("POS Push Job Issue", opsgenieDescription.ToString(), opsgenieDetails);
            }
            else
            {
                logger.Info("The POS Push Monitor has not detected a job that is taking too long.");
            }
        }

        /// <summary>
        /// This method will return whether or not the job has taken too long and a pager duty needs to be triggered.
        /// </summary>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        private bool CheckPosPushStatus(GetIrmaJobStatusQueryParameters queryParameters)
        {
            var posPushStatus = this.jobStatusQuery.Search(queryParameters);

            var shouldPage = posPushStatus.Status.Equals("RUNNING");

            var elapsedRunningTime = DateTime.Now - posPushStatus.LastRun.Value;
            var maxRunningTime = TimeSpan.FromMinutes(
                Icon.Common.AppSettingsAccessor.GetIntSetting(
                    PosPushTimeConfigPrefix + posPushStatus.Region.ToString()));

            shouldPage &= elapsedRunningTime > maxRunningTime;

            var pagerEntryKey = queryParameters.Classname + "_" + this.jobStatusQuery.TargetRegion;
            DateTime lastPagedRunTime;

            shouldPage &= JobStatusCache.OpsgenieTracker.TryGetValue(pagerEntryKey, out lastPagedRunTime)
                ? lastPagedRunTime != posPushStatus.LastRun
                : true;

            if (shouldPage)
            {
                JobStatusCache.OpsgenieTracker[pagerEntryKey] = posPushStatus.LastRun.Value;
            }

            return shouldPage;
        }
    }
}
