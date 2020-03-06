using System;

namespace Mammoth.Esb.ProductListener.Commands
{
    public class MessageArchiveCommand
    {
        public Guid MessageId { get; set; }
        public int MessageTypeId { get; set; }
        public int MessageStatusId { get; set; }
        public string MessageHeadersJson { get; set; }
        public string MessageBody { get; set; }
        public DateTime InsertDateUtc { get; set; }
    }
}