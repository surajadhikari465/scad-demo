using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esb.Core.EsbServices
{
    public class EsbServiceMessage
    {
        public string MessageId { get; set; }
        public string Text { get; set; }
        public Dictionary<string, string> Properties { get; set; }
    }
}
