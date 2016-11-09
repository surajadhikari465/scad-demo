using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Framework;

namespace GlobalEventController.DataAccess.Commands
{
    public class DeleteEventQueueCommand
    {
        public List<EventQueue> ProcessedEvents { get; set; }
    }
}
