using System.Collections.Generic;

namespace Icon.Monitoring.Common.ApiController
{
    public class MammothMessageQueueCache
    {
        public Dictionary<string, QueueData> QueueTypeToIdMapper { get; set; }

        public MammothMessageQueueCache()
        {
            QueueTypeToIdMapper = new Dictionary<string, QueueData>
            {
                { MessageQueueTypes.Price,      new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
                { MessageQueueTypes.ItemLocale, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } }
            };
        }
    }
}
