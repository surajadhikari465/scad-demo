using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSupport.DataAccess.Commands
{
    public class SaveSentMessageCommand
    {
        public Guid MessageId { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> MessageProperties { get; set; }
    }
}
