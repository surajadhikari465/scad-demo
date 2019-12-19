using AttributePublisher.DataAccess.Models;
using System.Collections.Generic;

namespace AttributePublisher.Models
{
    public class AttributeMessageModel
    {
        public string MessageId { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> MessageHeaders { get; set; }
        public List<AttributeModel> Attributes { get; set; }
    }
}
