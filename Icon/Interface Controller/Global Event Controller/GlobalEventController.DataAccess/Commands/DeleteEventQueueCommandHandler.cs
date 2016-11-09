using GlobalEventController.DataAccess.Infrastructure;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.Commands
{
    public class DeleteEventQueueCommandHandler : ICommandHandler<DeleteEventQueueCommand>
    {
        private readonly IconContext context;

        public DeleteEventQueueCommandHandler(IconContext context)
        {
            this.context = context;
        }
        public void Handle(DeleteEventQueueCommand command)
        {
            this.context.EventQueue.RemoveRange(command.ProcessedEvents);
            this.context.SaveChanges();
        }
    }
}
