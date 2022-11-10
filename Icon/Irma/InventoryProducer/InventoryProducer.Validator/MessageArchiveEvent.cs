using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryProducer.Validator
{
    internal record MessageArchiveEvent
    {
        public long MessageArchiveEventID { get; set; }
        public int KeyID { get; set; }
        public int? SecondaryKeyID { get; set; }
        public DateTime InsertDate { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string MessageID { get; set; }
        public string MessageType { get; set; }
        public string EventTypeCode { get; set; }
        public DateTime ArchiveInsertDateUtc { get; set; }
        public string MessageHeadersJSON { get; set; }
        public MessageHeaders MessageHeaders { get { return Newtonsoft.Json.JsonConvert.DeserializeObject<MessageHeadersWrapper>(this.MessageHeadersJSON).MessageHeaders;  } }
    }

    public class MessageHeadersWrapper
    {
        public MessageHeaders MessageHeaders { get; set; }
    }

    public class MessageHeaders
    {
        public string TransactionID { get; set; }
        public string TransactionType { get; set; }
        public string Source { get; set; }
        public string MessageType { get; set; }
        public long MessageNumber { get; set; }
        public string RegionCode { get; set; }
    }

}
