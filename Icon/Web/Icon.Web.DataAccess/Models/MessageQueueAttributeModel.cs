using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Models
{
    public class MessageQueueAttributeModel
    {
        public int MessageQueueAttributeId { get; set; }
        public int AttributeId { get; set; }
        public DateTime InsertDateUtc { get; set; }
    }
}
