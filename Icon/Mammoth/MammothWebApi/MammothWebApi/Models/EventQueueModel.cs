using System;

namespace MammothWebApi.Models
{
    public class EventQueueModel
    {
        public int? EventReferenceId { get; set; }
        public int EventTypeId { get; set; }
        public string Identifier { get; set; }
        public int? InProcessBy { get; set; }
        public DateTime InsertDate { get; set; }
        public int ItemKey { get; set; }
        public DateTime? ProcessFailedDate { get; set; }
        public int QueueId { get; set; }
        public int? StoreNo { get; set; }
    }
}