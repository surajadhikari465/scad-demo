using System;

namespace InventoryProducer.Common.InstockDequeue.Model.DBModel
{
    public class InstockDequeueModel
    {
        public int QueueID { get; set; }
        public string EventTypeCode { get; set; }
        public string MessageType { get; set; }
        public int KeyID { get; set; }
        public int? SecondaryKeyID { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime MessageTimestampUtc { get; set; }
    }
}
