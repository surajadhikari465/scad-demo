using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.BulkCommands
{
    public class BulkDeleteEventQueueCommand
    {
        public List<EventQueue> EventsToDelete { get; set; }
    }
}
