using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.Commands
{
    public class UpdateEventQueueFailuresCommand
    {
        public List<EventQueue> FailedEvents { get; set; }
    }
}
