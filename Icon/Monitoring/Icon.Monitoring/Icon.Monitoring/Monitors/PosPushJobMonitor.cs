namespace Icon.Monitoring.Monitors
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Icon.Logging;
    using Icon.Monitoring.Common;
    using Icon.Monitoring.Common.Constants;
    using Icon.Monitoring.Common.Enums;
    using Icon.Monitoring.Common.PagerDuty;
    using Icon.Monitoring.Common.Settings;
    using Icon.Monitoring.DataAccess.Model;
    using Icon.Monitoring.DataAccess.Queries;


    public class PosPushJobMonitor : TimedControllerMonitor
    {
        private const string PosPushTimeConfigPrefix = "PosPushTimeSpan_";
        private const string PosPushPagerDutyDescription = "POS Push is taking too long for the following regions: ";

        private readonly IQueryByRegionHandler<GetIrmaJobStatusQueryParameters, JobStatus> jobStatusQuery;
        private readonly IPagerDutyTrigger pagerDutyTrigger;

        public PosPushJobMonitor(
            IMonitorSettings settings,
            IQueryByRegionHandler<GetIrmaJobStatusQueryParameters, JobStatus> jobStatusQuery,
            IPagerDutyTrigger pagerDutyTrigger,
            ILogger logger)
        {
            this.settings = settings;
            this.jobStatusQuery = jobStatusQuery;
            this.pagerDutyTrigger = pagerDutyTrigger;
            this.logger = logger;
        }

        protected override void TimedCheckStatusAndNotify()
        {
            var pagerDutyDetails = new Dictionary<string, string>();
            var pagerDutyDescription = new StringBuilder(PosPushPagerDutyDescription);
            var queryParameters = new GetIrmaJobStatusQueryParameters { Classname = IrmaJobClassnames.POSPushJob };
            int numberFailed = 0;

            foreach (IrmaRegions region in Enum.GetValues(typeof(IrmaRegions)))
            {
                //changes region to iterate through all dbs
                this.jobStatusQuery.TargetRegion = region;

                if (this.CheckPosPushStatus(queryParameters))
                {
                    pagerDutyDescription.Append(region).Append(" ");
                    pagerDutyDetails.Add(IrmaJobClassnames.POSPushJob + ":" + region.ToString(), region.ToString());
                    numberFailed++;
                }
            }

            if (numberFailed > 0)
            {
                logger.Info("The POS Push Monitor has detected a job that is taking too long.");
                PagerDutyResponse response =
                    this.pagerDutyTrigger.TriggerIncident(pagerDutyDescription.ToString(), pagerDutyDetails);
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

            shouldPage &= JobStatusCache.PagerDutyTracker.TryGetValue(pagerEntryKey, out lastPagedRunTime)
                ? lastPagedRunTime != posPushStatus.LastRun
                : true;

            if (shouldPage)
            {
                JobStatusCache.PagerDutyTracker[pagerEntryKey] = posPushStatus.LastRun.Value;
            }

            return shouldPage;
        }
    }
}
