using System;

namespace GlobalEventController.Common
{
    public class EventQueueArchive
    {
        public int EventQueueArchiveId { get; set; }
        public int QueueId { get; set; }
        public int EventId { get; set; }
        public string EventMessage { get; set; }
        public int? EventReferenceId { get; set; }
        public string RegionCode { get; set; }
        public DateTime EventQueueInsertDate { get; set; }
        public string Context { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDetails { get; set; }
    }
}
