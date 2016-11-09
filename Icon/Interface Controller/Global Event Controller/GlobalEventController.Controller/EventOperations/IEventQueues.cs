using GlobalEventController.Common;
using Icon.Framework;
using System.Collections.Generic;

namespace GlobalEventController.Controller.EventOperations
{
    public interface IEventQueues
    {
        List<EventQueue> QueuedEvents { get; set; }
        List<EventQueue> ProcessedEvents { get; set; }
        List<FailedEvent> FailedEvents { get; set; }
        Dictionary<string, List<EventQueue>> RegionToEventQueueDictionary { get; set; }

    }
}
