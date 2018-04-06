using System.Collections.Generic;

namespace Icon.Monitoring.Common.ApiController
{
    public class MessageQueueCache
    {
        public Dictionary<string, QueueData> QueueTypeToIdMapper { get; set; }

        public MessageQueueCache()
        {
            QueueTypeToIdMapper = new Dictionary<string, QueueData>
            {
                { MessageQueueTypes.Product,    new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
                { MessageQueueTypes.Price,      new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
                { MessageQueueTypes.ItemLocale, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
                { MessageQueueTypes.Hierarchy,  new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
                { MessageQueueTypes.Locale,     new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
                { MessageQueueTypes.ProductSelectionGroup,     new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } }
            };
        }
    }
}
