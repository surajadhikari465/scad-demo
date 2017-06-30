using GlobalEventController.DataAccess.Infrastructure;
using Icon.DbContextFactory;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GlobalEventController.DataAccess.Commands
{
    public class UpdateEventQueueFailuresCommandHandler : ICommandHandler<UpdateEventQueueFailuresCommand>
    {
        private IDbContextFactory<IconContext> contextFactory;

        public UpdateEventQueueFailuresCommandHandler(IDbContextFactory<IconContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Handle(UpdateEventQueueFailuresCommand command)
        {
            List<int> queueIdList = command.FailedEvents.Select(e => e.QueueId).ToList();
            using (var context = contextFactory.CreateContext())
            {
                List<EventQueue> failedEvents = context.EventQueue.Where(eq => queueIdList.Contains(eq.QueueId)).ToList();

                foreach (EventQueue eventQueue in failedEvents)
                {
                    eventQueue.ProcessFailedDate = DateTime.Now;
                    eventQueue.InProcessBy = null;
                }

                context.SaveChanges();
            }
        }
    }
}
