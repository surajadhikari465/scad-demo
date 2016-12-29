using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalEventController.Common;
using Icon.Framework;

namespace GlobalEventController.DataAccess.Commands
{
    public class ArchiveEventsCommand
    {
        public List<EventQueueArchive> Events { get; set; }
    }
}
