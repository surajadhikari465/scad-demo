using GlobalEventController.DataAccess.Infrastructure;
using Icon.Common;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace GlobalEventController.DataAccess.Commands
{
    public class UpdateEventQueueStatusCommandHandler : ICommandHandler<UpdateEventQueueStatusCommand>
    {
        private readonly IconContext context;

        public UpdateEventQueueStatusCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Handle(UpdateEventQueueStatusCommand command)
        {
            if (command.EventQueueIdList == null || command.EventQueueIdList.Count == 0)
            {
                throw new ArgumentException("The EventQueue Id List is empty or null.");
            }

            List<EventQueue> queuedEvents = this.context.EventQueue.Where(q => command.EventQueueIdList.Contains(q.QueueId)).ToList();
            foreach (EventQueue eventQueueRow in queuedEvents)
            {
                eventQueueRow.InProcessBy = command.Instance.ToString();
            }

            // This could cause a concurrency clash which is handled by the calling class
            try
            {
                this.context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                // Detach all rows
                foreach (EventQueue eventQueueRow in queuedEvents)
                {
                    this.context.Entry(eventQueueRow).State = EntityState.Detached;
                }
                throw new ConcurrencyException(exception.Message, exception);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
