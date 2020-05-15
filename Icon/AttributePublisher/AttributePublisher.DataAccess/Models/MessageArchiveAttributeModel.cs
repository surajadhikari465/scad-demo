using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributePublisher.DataAccess.Models
{
    public class MessageArchiveAttributeModel
    {
        public int MessageQueueAttributeArchiveId { get; set; }
        public string MessageId { get; set; }
        public int AttributeId { get; set; }
        public string MessageQueueAttributeJson { get; set; }
        public DateTime InsertDateUtc { get; set; }
    }
}
