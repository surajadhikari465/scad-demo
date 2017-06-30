using GlobalEventController.Common;
using Icon.Framework;
using System.Collections.Generic;

namespace GlobalEventController.Controller.EventOperations
{
    public class EventQueues : IEventQueues
    {
        public List<EventQueue> QueuedEvents { get; set; }
        public List<EventQueue> ProcessedEvents { get; set; }
        public List<FailedEvent> FailedEvents { get; set; }
        public Dictionary<string, List<EventQueue>> RegionToEventQueueDictionary { get; set; }

        public EventQueues()
        {
            QueuedEvents = new List<EventQueue>();
            ProcessedEvents = new List<EventQueue>();
            FailedEvents = new List<FailedEvent>();
            RegionToEventQueueDictionary = new Dictionary<string, List<EventQueue>>();
        }
    }
}
