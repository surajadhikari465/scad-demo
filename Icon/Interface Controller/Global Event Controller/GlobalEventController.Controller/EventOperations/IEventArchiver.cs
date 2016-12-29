using GlobalEventController.Common;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.Controller.EventOperations
{
    public interface IEventArchiver
    {
        List<EventQueueArchive> Events { get; set; }
        void ArchiveEvents();
        void ClearArchiveEvents();
    }
}
