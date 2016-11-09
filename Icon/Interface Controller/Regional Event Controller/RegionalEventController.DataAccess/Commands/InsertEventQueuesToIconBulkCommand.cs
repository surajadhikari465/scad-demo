using Icon.Framework;
using System.Collections.Generic;

namespace RegionalEventController.DataAccess.Commands
{
    public class InsertEventQueuesToIconBulkCommand
    {
        public List<EventQueue> EventQueueEntries { get; set; }
    }
}
