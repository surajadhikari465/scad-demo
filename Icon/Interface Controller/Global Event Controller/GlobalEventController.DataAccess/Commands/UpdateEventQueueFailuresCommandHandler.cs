using GlobalEventController.DataAccess.Infrastructure;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.Commands
{
    public class UpdateEventQueueFailuresCommandHandler : ICommandHandler<UpdateEventQueueFailuresCommand>
    {
        private readonly ContextManager contextManager;

        public UpdateEventQueueFailuresCommandHandler(ContextManager contextManager)
        {
            this.contextManager = contextManager;
        }

        public void Handle(UpdateEventQueueFailuresCommand command)
        {
            List<int> queueIdList = command.FailedEvents.Select(e => e.QueueId).ToList();
            List<EventQueue> failedEvents = this.contextManager.IconContext.EventQueue.Where(eq => queueIdList.Contains(eq.QueueId)).ToList();

            foreach (EventQueue eventQueue in failedEvents)
            {
                eventQueue.ProcessFailedDate = DateTime.Now;
                eventQueue.InProcessBy = null;
            }

            this.contextManager.IconContext.SaveChanges();
        }
    }
}
