using Icon.Esb.R10Listener.Models;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.R10Listener.Commands
{
    public class ResendMessageQueueEntriesCommand
    {
        public R10MessageResponseModel MessageResponse { get; set; }
        public MessageHistory MessageHistory { get; set; }
    }
}
