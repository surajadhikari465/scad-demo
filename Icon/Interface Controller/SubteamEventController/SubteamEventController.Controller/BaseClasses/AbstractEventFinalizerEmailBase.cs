using GlobalEventController.Common;
using GlobalEventController.Controller.Email;
using GlobalEventController.Controller.EventOperations;
using Icon.Common.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubteamEventController.Controller.BaseClasses
{
    public abstract class AbstractEventFinalizerEmailBase : IEventFinalizer
    {
        protected IEventFinalizer eventFinalizer;
        protected IEventQueues queues;
        protected IEmailClient emailClient;

        protected AbstractEventFinalizerEmailBase(IEventFinalizer eventFinalizer, IEventQueues queues, IEmailClient emailClient)
        {
            this.eventFinalizer = eventFinalizer;
            this.queues = queues;
            this.emailClient = emailClient;
        }

        public void HandleFailedEvents()
        {
            SendEmail();
            eventFinalizer.HandleFailedEvents();
        }

        public abstract void SendEmail();

        public void DeleteEvents()
        {
            eventFinalizer.DeleteEvents();
        }
    }
}
