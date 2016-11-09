namespace Icon.Monitoring.Monitors
{
    using System;
    using System.Collections.Generic;

    using Icon.Common.DataAccess;
    using Icon.Logging;
    using Icon.Monitoring.Common.PagerDuty;
    using Icon.Monitoring.Common.Settings;
    using Icon.Monitoring.DataAccess.Queries;

    public class GloConControllerMonitor : TimedControllerMonitor
    {
        private const string PagerDutyDescription = "GloCon Controller appears to be stuck or not processing data.";

        private readonly IQueryHandler<GetGloConItemQueueIdParameters, int> gloConQuery;
        private readonly IPagerDutyTrigger pagerDutyTrigger;

        public Dictionary<int, int> GloConJobTracker { get; set; }

        public GloConControllerMonitor(
            IMonitorSettings settings,
            IQueryHandler<GetGloConItemQueueIdParameters, int> gloConQuery,
            IPagerDutyTrigger pagerDutyTrigger,
            ILogger logger)
        {
            this.GloConJobTracker = new Dictionary<int, int>();

            this.settings = settings;
            this.gloConQuery = gloConQuery;
            this.pagerDutyTrigger = pagerDutyTrigger;
            this.logger = logger;
        }

        protected override void TimedCheckStatusAndNotify()
        {
            var gloConResultId = this.gloConQuery.Search(new GetGloConItemQueueIdParameters());
            if (gloConResultId == 0)
            {
                this.GloConJobTracker.Clear();
                return;
            }

            int numberOfTimesDetected; // 1: first entry, 2: has been paged.
            if (this.GloConJobTracker.TryGetValue(gloConResultId, out numberOfTimesDetected))
            {
                if (numberOfTimesDetected == 1)
                {
                    this.GloConJobTracker[gloConResultId] = 2;
                    this.TriggerPagerDutyForGloConJob(gloConResultId);
                }
            }
            else
            {
                this.GloConJobTracker.Clear();
                this.GloConJobTracker[gloConResultId] = 1;
            }
        }

        private void TriggerPagerDutyForGloConJob(int id)
        {
            logger.Info("The GloCon Controller Monitor has detected a job that is taking too long.");

            var response = this.pagerDutyTrigger.TriggerIncident(
                            PagerDutyDescription,
                            new Dictionary<string, string>()
                            {
                                { id.ToString(), DateTime.Now.ToString() }
                            });
        }
    }
}
