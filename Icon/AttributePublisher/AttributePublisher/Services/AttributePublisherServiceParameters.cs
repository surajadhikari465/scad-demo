using AttributePublisher.DataAccess.Models;
using AttributePublisher.Models;
using System.Collections.Generic;

namespace AttributePublisher.Services
{
    public class AttributePublisherServiceParameters
    {
        public bool ContinueProcessing { get; set; }
        public List<AttributeModel> Attributes { get; set; } = new List<AttributeModel>();
        public List<AttributeMessageModel> AttributeMessages { get; set; } = new List<AttributeMessageModel>();
    }
}