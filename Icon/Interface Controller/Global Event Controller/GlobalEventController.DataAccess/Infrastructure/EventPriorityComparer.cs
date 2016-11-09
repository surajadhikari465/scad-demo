using Icon.Framework;
using System.Collections.Generic;

namespace GlobalEventController.DataAccess.Infrastructure
{
    public class EventPriorityComparer : IComparer<EventQueue>
    {
        IList<string> eventPriority;

        public EventPriorityComparer(List<string> eventPriority)
        {
            this.eventPriority = eventPriority;
        }

        public int Compare(EventQueue x, EventQueue y)
        {
            return eventPriority.IndexOf(x.EventType.EventName.MapToRegisteredEvent()) - eventPriority.IndexOf(y.EventType.EventName.MapToRegisteredEvent());
        }
    }
}
