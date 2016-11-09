using Icon.Logging;
using Icon.Framework;
using RegionalEventController.DataAccess.Interfaces;
using System;
using System.Linq;

namespace RegionalEventController.DataAccess.Commands
{
    public class InsertIrmaItemToIconCommandHandler : ICommandHandler<InsertIrmaItemToIconCommand>
    {
        private ILogger<InsertIrmaItemToIconCommandHandler> logger;
        private IconContext context;
        public InsertIrmaItemToIconCommandHandler(ILogger<InsertIrmaItemToIconCommandHandler> logger, IconContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public void Execute(InsertIrmaItemToIconCommand command)
        {
            context.IRMAItem.Add(command.irmaNewItem);
            context.SaveChanges();
        }
    }
}
