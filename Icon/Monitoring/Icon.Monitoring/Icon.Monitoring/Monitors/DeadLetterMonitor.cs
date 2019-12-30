using Icon.Monitoring.Common.Opsgenie;
using Icon.Monitoring.Common.Settings;
using Icon.Common.DataAccess;
using Icon.Monitoring.DataAccess.Queries;
using System.Collections.Generic;
using System;
using Icon.Logging;
using Icon.Monitoring.DataAccess.Model;
using Icon.Monitoring.Service;

namespace Icon.Monitoring.Monitors
{
    public class DeadLetterMonitor : TimedControllerMonitor
    {
        private readonly IOpsgenieTrigger opsgenieTrigger;
        private readonly IQueryHandler<GetIconDeadLetterParameters, int> messageQueueItemQuery;
        private readonly IDateService dateService;
        DateTime lastMonitorDate;
        public DeadLetterMonitor(
            IMonitorSettings settings,
            IOpsgenieTrigger opsgenieTrigger,
            ILogger logger,
            IQueryHandler<GetIconDeadLetterParameters, int> messageQueueItemQuery,
            IDateService dateService)
        {
            this.opsgenieTrigger = opsgenieTrigger;
            this.logger = logger;
            this.messageQueueItemQuery = messageQueueItemQuery;
            this.settings = settings;
            this.dateService = dateService;
            this.lastMonitorDate = this.dateService.UtcNow;
        }

        protected override void TimedCheckStatusAndNotify()
        {
            int response = this.messageQueueItemQuery.Search(new GetIconDeadLetterParameters()
            {
                LastMonitorDate = this.lastMonitorDate
            });

            var details = new Dictionary<string, string>()
            {
                {"DeadLetterQueueRecordCount", response.ToString()}
            };

            if (response > 0)
            {
                string message = $"{response} records were added to the dead letter queue. This means the item publisher was not able to process the records and they will need to be retried. You will need to look at the esb.MessageDeadLetterQueue to determine the cause.";
                this.TriggerOpsGenieForItemPublisher(message, details);
            }

            lastMonitorDate = this.dateService.UtcNow;
        }

        private void TriggerOpsGenieForItemPublisher(string errorMessage, Dictionary<string,string> details)
        {
            logger.Info(errorMessage);
            var response = this.opsgenieTrigger.TriggerAlert("Item Publisher Service Issue",
                            errorMessage,
                            details);
        }
    }
}