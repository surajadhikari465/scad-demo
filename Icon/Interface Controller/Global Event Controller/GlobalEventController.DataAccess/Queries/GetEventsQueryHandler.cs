using GlobalEventController.Common;
using GlobalEventController.DataAccess.Infrastructure;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.Queries
{
    public class GetEventsQueryHandler : IQueryHandler<GetEventsQuery, List<EventQueue>>
    {
        private readonly IconContext context;

        public GetEventsQueryHandler(IconContext context)
        {
            this.context = context;
        }

        public List<EventQueue> Handle(GetEventsQuery parameters)
        {
            List<string> registeredEventNames = new List<string>();

            foreach (string registeredEvent in parameters.RegisteredEvents)
            {
                registeredEventNames.Add(registeredEvent.MapToIconEvent());
            }

            List<EventQueue> events = context.EventQueue
                .Where(eq => registeredEventNames.Contains(eq.EventType.EventName) 
                    && eq.ProcessFailedDate == null
                    && (eq.InProcessBy == null || eq.InProcessBy == StartupOptions.Instance.ToString()))
                .ToList();
            events.Sort(new EventPriorityComparer(parameters.RegisteredEvents));

            return events.Take(parameters.MaxNumberOfEvents).ToList();
        }
    }
}
