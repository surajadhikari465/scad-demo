using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributePublisher.DataAccess.Models
{
    public class MessageArchiveModel
    {
        public int MessageArchiveId { get; set; }
        public string MessageId { get; set; }
        public int MessageTypeId { get; set; }
        public string Message { get; set; }
        public string MessageHeaders { get; set; }
        public DateTime InsertDateUtc { get; set; }
        public List<MessageArchiveAttributeModel> AttributeModels { get; set; }
    }
}
