using System;

namespace Icon.Web.DataAccess.Models
{
    public class EventQueueModel
    {
        public int QueueId { get; set; }
        public int EventId { get; set; }
        public string EventMessage { get; set; }
        public int EventReferenceId { get; set; }
        public string RegionCode { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime ProcessedFailedDate { get; set; }
        public string InProcessBy { get; set; }
    }
}
