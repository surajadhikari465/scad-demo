using GlobalEventController.Common;
using GlobalEventController.Controller.EventOperations;
using Icon.Common.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.Controller.Decorators.EventFinalizer
{
    public class EventFinalizerEmailDecorator : IEventFinalizer
    {
        private IEventFinalizer eventFinalizer;
        private IEventQueues queues;
        private IEmailClient emailClient;

        public EventFinalizerEmailDecorator(IEventFinalizer eventFinalizer, IEventQueues queues, IEmailClient emailClient)
        {
            this.eventFinalizer = eventFinalizer;
            this.queues = queues;
            this.emailClient = emailClient;
        }

        public void HandleFailedEvents()
        {
            if(queues.FailedEvents.Any())
            {
                var failedEvents = EmailHelper.BuildEventQueueTable(queues.FailedEvents);
                emailClient.Send(String.Format(Resources.ErrorFailedEvents, failedEvents), Resources.EmailSubjectFailedEvents);
            }

            eventFinalizer.HandleFailedEvents();
        }

        public void DeleteEvents()
        {
            eventFinalizer.DeleteEvents();
        }
    }
}
