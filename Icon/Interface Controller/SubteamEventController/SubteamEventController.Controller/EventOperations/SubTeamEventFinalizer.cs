using GlobalEventController.Common;
using GlobalEventController.Controller.Email;
using GlobalEventController.Controller.EventOperations;
using Icon.Common.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubteamEventController.Controller.BaseClasses;

namespace SubteamEventController.Controller.EventOperations
{
    public class SubTeamEventFinalizer : AbstractEventFinalizerEmailBase
    {
        public SubTeamEventFinalizer(IEventFinalizer eventFinalizer, IEventQueues queues, IEmailClient emailClient)
            : base(eventFinalizer, queues, emailClient)
        {
            
        }

        public override void SendEmail()
        {
            if (queues.FailedEvents.Any())
            {
                var failedEvents = EmailHelper.BuildEventQueueTable(queues.FailedEvents);
                emailClient.Send(String.Format(SubTeamConstants.ErrorFailedEvents, failedEvents), SubTeamConstants.EmailSubjectFailedEvents);
            }
        }
    }
}
