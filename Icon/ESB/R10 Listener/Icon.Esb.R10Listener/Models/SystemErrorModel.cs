using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.R10Listener.Models
{
    public class SystemErrorModel
    {
        public string Description { get; set; }
        public string ReasonCode { get; set; }
        public string MessageDescription { get; set; }
        public string MessageException { get; set; }
    }
}
