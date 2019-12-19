using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using System;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdatePluCategoryCommandHandler : ICommandHandler<UpdatePluCategoryCommand>
    {
        private IconContext context;

        public UpdatePluCategoryCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(UpdatePluCategoryCommand data)
        {
            PLUCategory updatedItem = context.PLUCategory.Find(data.PluCategory.PluCategoryID);
            updatedItem.PluCategoryName = data.PluCategory.PluCategoryName;
            updatedItem.BeginRange = data.PluCategory.BeginRange;
            updatedItem.EndRange = data.PluCategory.EndRange;

            try
            {
                context.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new CommandException(String.Format("Error updating PLU Category {0}.", data.PluCategory.PluCategoryName), exception);
            }
        }
    }
}
