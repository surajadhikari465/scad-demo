namespace Icon.Monitoring.Monitors
{
    using System;
    using System.Collections.Generic;

    using Icon.Logging;
    using Icon.Monitoring.Common;
    using Icon.Monitoring.Common.ApiController;
    using Icon.Monitoring.Common.PagerDuty;
    using Icon.Monitoring.Common.Settings;
    using Icon.Monitoring.DataAccess.Queries;

    public class MammothApiControllerMonitor : TimedControllerMonitor
    {
        private readonly IQueryHandlerMammoth<GetApiMessageQueueIdParameters, int> messageQueueQuery;
        private readonly IPagerDutyTrigger pagerDutyTrigger;
        private MammothMessageQueueCache messageQueueCache;

        public MammothApiControllerMonitor(
            IMonitorSettings settings,
            IQueryHandlerMammoth<GetApiMessageQueueIdParameters, int> messageQueueQuery,
            IPagerDutyTrigger pagerDutyTrigger,
            ILogger logger,
            MammothMessageQueueCache messageQueueCache)
        {
            this.settings = settings;
            this.logger = logger;
            this.messageQueueQuery = messageQueueQuery;
            this.pagerDutyTrigger = pagerDutyTrigger;
            this.messageQueueCache = messageQueueCache;
        }

        protected override void TimedCheckStatusAndNotify()
        {
            // Get MessageQueueIDs for each type of MessageQueue
            this.CheckMessageQueueId(MessageQueueTypes.Price);
            this.CheckMessageQueueId(MessageQueueTypes.ItemLocale);
        }

        private void CheckMessageQueueId(string queueType)
        {
            GetApiMessageQueueIdParameters queryParameters = new GetApiMessageQueueIdParameters();
            queryParameters.MessageQueueType = queueType;
            int id = this.messageQueueQuery.Search(queryParameters);

            if (messageQueueCache.QueueTypeToIdMapper[queueType].LastMessageQueueId == id
                && messageQueueCache.QueueTypeToIdMapper[queueType].NumberOfTimesMatched == 0
                && id != 0)
            {
                string description = BuildTriggerDescription(queueType);
                Dictionary<string, string> details = BuildTriggerDetails(id);
                TriggerPagerDutyIncident(description, details);

                messageQueueCache.QueueTypeToIdMapper[queueType].NumberOfTimesMatched++;
            }
            else if (messageQueueCache.QueueTypeToIdMapper[queueType].LastMessageQueueId == id
                && messageQueueCache.QueueTypeToIdMapper[queueType].NumberOfTimesMatched > 0)
            {
                messageQueueCache.QueueTypeToIdMapper[queueType].NumberOfTimesMatched++;
            }
            else
            {
                messageQueueCache.QueueTypeToIdMapper[queueType].LastMessageQueueId = id;
                messageQueueCache.QueueTypeToIdMapper[queueType].NumberOfTimesMatched = 0;
            }
        }

        private void TriggerPagerDutyIncident(string description, Dictionary<string, string> jsonDetails)
        {
            PagerDutyResponse response = this.pagerDutyTrigger.TriggerIncident(description, jsonDetails);
        }

        private string BuildTriggerDescription(string queueType)
        {
            return string.Format("{0} Mammoth API Controller appears to be stuck or not processing data.", queueType);
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
