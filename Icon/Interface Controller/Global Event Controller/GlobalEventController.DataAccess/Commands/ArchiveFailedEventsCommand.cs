using GlobalEventController.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.Commands
{
    public class ArchiveFailedEventsCommand
    {
        public List<ArchiveEventModelWrapper<FailedEvent>> FailedEvents { get; set; }
    }
}
