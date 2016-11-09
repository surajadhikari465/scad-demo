using Icon.Esb.R10Listener.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.R10Listener.Commands
{
    public class ProcessR10MessageResponseCommand
    {
        public R10MessageResponseModel MessageResponse { get; set; }
    }
}
