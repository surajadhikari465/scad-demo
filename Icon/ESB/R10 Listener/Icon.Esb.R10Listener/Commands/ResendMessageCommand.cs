using Icon.Esb.R10Listener.Models;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.R10Listener.Commands
{
    public class ResendMessageCommand
    {
        public int MessageHistoryId { get; set; }
    }
}
