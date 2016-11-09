using System;

namespace GlobalEventController.DataAccess.Infrastructure
{
    public class EventQueueCustom
    {
        public int QueueId { get; set; }
        public int EventId { get; set; }
        public string EventMessage { get; set; }
        public Nullable<int> EventReferenceId { get; set; }
        public string RegionCode { get; set; }
        public System.DateTime InsertDate { get; set; }
        public Nullable<System.DateTime> ProcessFailedDate { get; set; }
        public string InProcessBy { get; set; }
        public string EventName { get; set; }
    }
}
