using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;
using System;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdatePluCommandHandler : ICommandHandler<UpdatePluCommand>
    {
        private ILogger logger;
        private IconContext context;

        public UpdatePluCommandHandler(ILogger logger, IconContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public void Execute(UpdatePluCommand data)
        {
            try
            {
                var existingMap = context.PLUMap.Find(data.Plu.itemID);
                context.Entry(existingMap).CurrentValues.SetValues(data.Plu);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new CommandException("The PluCommandHandler threw an exception.", ex);
            }
        }
    }
}
