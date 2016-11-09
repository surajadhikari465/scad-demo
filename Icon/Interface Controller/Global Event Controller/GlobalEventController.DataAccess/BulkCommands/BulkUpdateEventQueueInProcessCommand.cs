using GlobalEventController.DataAccess.Infrastructure;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.BulkCommands
{
    public class BulkUpdateEventQueueInProcessCommand : IQuery<List<EventQueueCustom>>
    {
        public List<string> RegisteredEventNames { get; set; }
        public int MaxRows { get; set; }
        public string Instance { get; set; }
    }
}
