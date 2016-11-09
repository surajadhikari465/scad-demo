namespace Icon.Monitoring.Monitors
{
    using System;
    using System.Collections.Generic;

    using Icon.Common.DataAccess;
    using Icon.Logging;
    using Icon.Monitoring.Common.PagerDuty;
    using Icon.Monitoring.Common.Settings;
    using Icon.Monitoring.DataAccess.Queries;

    public class PushControllerMonitor : TimedControllerMonitor
    {
        private const string PagerDutyDescription = "POS Push Controller appears to be stuck or not processing data.";

        private readonly IQueryHandler<GetIrmaPushIdParameters, int> pushQuery;
        private readonly IPagerDutyTrigger pagerDutyTrigger;
        public Dictionary<int, int> IrmaPushJobTracker { get; set; }

        public PushControllerMonitor(IMonitorSettings settings,
            IQueryHandler<GetIrmaPushIdParameters, int> pushQuery,
            IPagerDutyTrigger pagerDutyTrigger,
            ILogger logger)
        {
            this.IrmaPushJobTracker = new Dictionary<int, int>();

            this.settings = settings;
            this.pushQuery = pushQuery;
            this.pagerDutyTrigger = pagerDutyTrigger;
            this.logger = logger;
        }

        protected override void TimedCheckStatusAndNotify()
        {
            var pushQueryResultId = this.pushQuery.Search(new GetIrmaPushIdParameters());
            if (pushQueryResultId == 0)
            {
                this.IrmaPushJobTracker.Clear();
                return;
            }

            int numberOfTimesDetected; // 1: first entry, 2: has been paged.
            if (this.IrmaPushJobTracker.TryGetValue(pushQueryResultId, out numberOfTimesDetected))
            {
                if (numberOfTimesDetected == 1)
                {
                    this.IrmaPushJobTracker[pushQueryResultId] = 2;
                    this.TriggerPagerDutyForPushJob(pushQueryResultId);
                }
            }
            else
            {
                this.IrmaPushJobTracker.Clear();
                this.IrmaPushJobTracker[pushQueryResultId] = 1;
            }
        }

        private void TriggerPagerDutyForPushJob(int id)
        {
            logger.Info("The POS Push Controller Monitor has detected a job that is taking too long.");

            var response = this.pagerDutyTrigger.TriggerIncident(
                            PagerDutyDescription,
                            new Dictionary<string, string>()
                            {
                                { id.ToString(), DateTime.Now.ToString() }
                            });
        }
    }
}
