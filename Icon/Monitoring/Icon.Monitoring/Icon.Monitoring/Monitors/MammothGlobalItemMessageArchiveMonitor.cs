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
    public class MammothGlobalItemMessageArchiveMonitor : TimedControllerMonitor
    {
        private readonly IOpsgenieTrigger opsgenieTrigger;
        private readonly IQueryHandlerMammoth<GetGlobalItemErrorsFromMessageArchaiveParameters, int> messageQueueItemQuery;
        private readonly IDateService dateService;
        DateTime lastMonitorDate;
        public MammothGlobalItemMessageArchiveMonitor(
            IMonitorSettings settings,
            IOpsgenieTrigger opsgenieTrigger,
            ILogger logger,
            IQueryHandlerMammoth<GetGlobalItemErrorsFromMessageArchaiveParameters, int> messageQueueItemQuery,
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
            int response = this.messageQueueItemQuery.Search(new GetGlobalItemErrorsFromMessageArchaiveParameters()
            {
                LastMonitorDate = this.lastMonitorDate
            });

            var details = new Dictionary<string, string>()
            {
                {"MessageArchiveQueueRecordCount", response.ToString()}
            };

            if (response > 0)
            {
                string message = $@"
{response} failed Global Item record(s) were added to esb.MessageArchive since '{this.lastMonitorDate}' UTC. This means the product listener was not able to process the records and they will need to be retried. 
You will need to look at the esb.MessageArchive to determine the cause.

select * from esb.MessageArchive where MessageStatusId = 3 and MessageTypeID = 8 and InsertDateUTC > '{this.lastMonitorDate}'";
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