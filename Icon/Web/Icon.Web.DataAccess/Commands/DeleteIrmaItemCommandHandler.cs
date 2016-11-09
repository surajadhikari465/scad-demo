using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;
using System;

namespace Icon.Web.DataAccess.Commands
{
    public class DeleteIrmaItemCommandHandler : ICommandHandler<DeleteIrmaItemCommand>
    {
        private IconContext context;

        public DeleteIrmaItemCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(DeleteIrmaItemCommand data)
        {
            IRMAItem deleteItem = context.IRMAItem.Find(data.IrmaItemId);

            try
            {
                context.IRMAItem.Remove(deleteItem);
                context.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new CommandException(String.Format("Error deleteting IRMAItem {0}", deleteItem == null ? "" : deleteItem.identifier), exception);
            }
            
        }
    }
}

