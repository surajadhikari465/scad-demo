namespace Icon.Monitoring.Monitors
{
    using System.Collections.Generic;

    using Icon.Common.DataAccess;
    using Icon.Logging;
    using Icon.Monitoring.Common;
    using Icon.Monitoring.Common.PagerDuty;
    using Icon.Monitoring.Common.Settings;
    using Icon.Monitoring.DataAccess.Queries;

    public class ApiControllerMonitor : TimedControllerMonitor
    {
        private readonly IQueryHandler<GetApiMessageQueueIdParameters, int> messageQueueQuery;
        private readonly IPagerDutyTrigger pagerDutyTrigger;

        public ApiControllerMonitor(
            IMonitorSettings settings,
            IQueryHandler<GetApiMessageQueueIdParameters, int> messageQueueQuery,
            IPagerDutyTrigger pagerDutyTrigger,
            ILogger logger)
        {
            this.settings = settings;
            this.logger = logger;
            this.messageQueueQuery = messageQueueQuery;
            this.pagerDutyTrigger = pagerDutyTrigger;
        }

        protected override void TimedCheckStatusAndNotify()
        {
            // Get MessageQueueIDs for each type of MessageQueue
            this.CheckMessageQueueId(MessageQueueTypes.Product);
            this.CheckMessageQueueId(MessageQueueTypes.Price);
            this.CheckMessageQueueId(MessageQueueTypes.ItemLocale);
            this.CheckMessageQueueId(MessageQueueTypes.Hierarchy);
            this.CheckMessageQueueId(MessageQueueTypes.Locale);
            this.CheckMessageQueueId(MessageQueueTypes.ProductSelectionGroup);
        }

        private void CheckMessageQueueId(string queueType)
        {
            GetApiMessageQueueIdParameters queryParameters = new GetApiMessageQueueIdParameters();
            queryParameters.MessageQueueType = queueType;
            int id = this.messageQueueQuery.Search(queryParameters);

            if (MessageQueueCache.QueueTypeToIdMapper[queueType].LastMessageQueueId == id
                && MessageQueueCache.QueueTypeToIdMapper[queueType].NumberOfTimesMatched == 0
                && id != 0)
            {
                string description = BuildTriggerDescription(queueType);
                Dictionary<string, string> details = BuildTriggerDetails(id);
                TriggerPagerDutyIncident(description, details);

                MessageQueueCache.QueueTypeToIdMapper[queueType].NumberOfTimesMatched++;
            }
            else if (MessageQueueCache.QueueTypeToIdMapper[queueType].LastMessageQueueId == id
                && MessageQueueCache.QueueTypeToIdMapper[queueType].NumberOfTimesMatched > 0)
            {
                MessageQueueCache.QueueTypeToIdMapper[queueType].NumberOfTimesMatched++;
            }
            else
            {
                MessageQueueCache.QueueTypeToIdMapper[queueType].LastMessageQueueId = id;
                MessageQueueCache.QueueTypeToIdMapper[queueType].NumberOfTimesMatched = 0;
            }
        }

        private void TriggerPagerDutyIncident(string description, Dictionary<string, string> jsonDetails)
        {
            PagerDutyResponse response = this.pagerDutyTrigger.TriggerIncident(description, jsonDetails);
        }

        private string BuildTriggerDescription(string queueType)
        {
            return string.Format("{0} API Controller appears to be stuck or not processing data.", queueType);
        }

        private Dictionary<string, string> BuildTriggerDetails(int? messageQueueId)
        {
            Dictionary<string, string> details = new Dictionary<string, string>
            {
                { "Stuck MessageQueueId", messageQueueId.ToString() }
            };
            return details;
        }
    }
}
