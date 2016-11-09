using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Extensions;
using Icon.Web.DataAccess.Infrastructure;
using System;

namespace Icon.Web.DataAccess.Managers
{
    public class UpdatePluCategoryManagerHandler : IManagerHandler<UpdatePluCategoryManager>
    {
        private IconContext context;
        private ICommandHandler<UpdatePluCategoryCommand> updatePluCategoryCommandHandler;

        public UpdatePluCategoryManagerHandler(
            IconContext context,
            ICommandHandler<UpdatePluCategoryCommand> updatePluCategoryCommandHandler)
        {
            this.context = context;
            this.updatePluCategoryCommandHandler = updatePluCategoryCommandHandler;
        }

        public void Execute(UpdatePluCategoryManager data)
        {
            if (context.PLUCategory.ContainsDuplicatePluCategoryName(data.PluCategoryId, data.PluCategoryName, Convert.ToInt64(data.BeginRange), Convert.ToInt64(data.EndRange)))
            {
                throw new CommandException("Another PLU category with the specified name already exists.  Please enter a new PLU category name.");
            }

            if (context.PLUCategory.ContainsDuplicatePluCategoryRange(data.PluCategoryId, data.PluCategoryName, Convert.ToInt64(data.BeginRange), Convert.ToInt64(data.EndRange)))
            {
                throw new CommandException("The PLU category range overlaps with another existing range.  Please enter new values.");
            }

            var updatePluCategoryCommand = new UpdatePluCategoryCommand
            {
                PluCategory = new PLUCategory() { PluCategoryID = data.PluCategoryId, PluCategoryName = data.PluCategoryName, BeginRange = data.BeginRange, EndRange = data.EndRange }
            };

            try
            {
                updatePluCategoryCommandHandler.Execute(updatePluCategoryCommand);

            }
            catch (DuplicateValueException ex)
            {
                throw new CommandException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new CommandException(String.Format("An error occurred when adding PLU Category {0}.", data.PluCategoryName), ex);
            }
        }
    }
}
