using Icon.Logging;
using Icon.Framework;
using RegionalEventController.DataAccess.Interfaces;
using System;
using System.Linq;

namespace RegionalEventController.DataAccess.Commands
{
    public class InsertEventQueueToIconCommandHandler : ICommandHandler<InsertEventQueueToIconCommand>
    {
        private ILogger<InsertEventQueueToIconCommandHandler> logger;
        private IconContext context;
        public InsertEventQueueToIconCommandHandler(ILogger<InsertEventQueueToIconCommandHandler> logger, IconContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public void Execute(InsertEventQueueToIconCommand command)
        {
            context.EventQueue.Add(command.eventQueueEntry);
            context.SaveChanges();

            string message = String.Format("New event added to Icon.  EventType: {0}  EventReferenceId: {1}", command.eventName, command.eventQueueEntry.EventReferenceId);
            logger.Info(message);
        }
    }
}
