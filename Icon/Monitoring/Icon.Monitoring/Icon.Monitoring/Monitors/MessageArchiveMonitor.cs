using Icon.Monitoring.Common.Opsgenie;
using Icon.Monitoring.Common.Settings;
using Icon.Common.DataAccess;
using Icon.Monitoring.DataAccess.Queries;
using System.Collections.Generic;
using System;
using Icon.Logging;
using Icon.Monitoring.Service;

namespace Icon.Monitoring.Monitors
{
    public class MessageArchiveMonitor : TimedControllerMonitor
    {
        private readonly IOpsgenieTrigger opsgenieTrigger;
        private readonly IQueryHandler<GetMessageArchiveParameters, int> messageQueueItemQuery;
        private readonly IDateService dateService;
        DateTime lastMonitorDate;
        public MessageArchiveMonitor(
            IMonitorSettings settings,
            IOpsgenieTrigger opsgenieTrigger,
            ILogger logger,
            IQueryHandler<GetMessageArchiveParameters, int> messageQueueItemQuery,
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
            int response = this.messageQueueItemQuery.Search(new GetMessageArchiveParameters()
            {
                LastMonitorDate = this.lastMonitorDate
            });

            var details = new Dictionary<string, string>()
            {
                {"MessageArchiveQueueRecordCount", response.ToString()}
            };

            if (response > 0)
            {
                string message = $"{response} records were added to the message archive queue. This means the product listener was not able to process the records and they will need to be retried. You will need to look at the esb.MessageArchive to determine the cause.";
                this.TriggerOpsGenieForProductListener(message, details);
            }

            lastMonitorDate = this.dateService.UtcNow;
        }

        private void TriggerOpsGenieForProductListener(string errorMessage, Dictionary<string,string> details)
        {
            logger.Info(errorMessage);
            var response = this.opsgenieTrigger.TriggerAlert("Product Listener Service Issue",
                            errorMessage,
                            details);
        }
    }
}