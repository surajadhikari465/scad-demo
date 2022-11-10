using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryProducer.Validator
{
    internal record Event
    {
        public string EventTypeCode { get; set; }
        public string MessageType { get; set; }
        public int KeyID { get; set; }
        public int? SecondaryKeyID { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime MessageTimestampUtc { get; set; }
        public MessageArchiveEvent OriginalEvent { get; set; }

        public static implicit operator Event(MessageArchiveEvent v)
        {
            return new Event()
            {
                EventTypeCode = v.EventTypeCode,
                MessageType = v.MessageType,
                KeyID = v.KeyID,
                SecondaryKeyID = v.SecondaryKeyID,
                OriginalEvent = v
            };
        }
    }
}
