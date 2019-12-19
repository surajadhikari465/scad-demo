using System;

namespace Icon.Services.ItemPublisher.Repositories.Entities
{
    public class MessageQueueItem
    {
        public int MessageQueueItemId { get; set; }

        public DateTime EsbReadyDatetimeUtc { get; set; }

        public int ItemId { get; set; }

        public DateTime InsertDateUtc { get; set; }
    }
}