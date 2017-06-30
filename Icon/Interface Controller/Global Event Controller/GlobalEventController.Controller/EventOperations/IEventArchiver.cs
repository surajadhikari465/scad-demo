using GlobalEventController.Common;
using System.Collections.Generic;

namespace GlobalEventController.Controller.EventOperations
{
    public interface IEventArchiver
    {
        List<EventQueueArchive> Events { get; set; }
        void ArchiveEvents();
        void ClearArchiveEvents();
    }
}
