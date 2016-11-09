using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Extensions;
using Icon.Web.DataAccess.Infrastructure;
using System;

namespace Icon.Web.DataAccess.Managers
{
    public class AddPluCategoryManagerHandler : IManagerHandler<AddPluCategoryManager>
    {
        private IconContext context;
        private ICommandHandler<AddPluCategoryCommand> addPluCategoryCommandHandler;

        public AddPluCategoryManagerHandler(
            IconContext context,
            ICommandHandler<AddPluCategoryCommand> addPluCategoryCommandHandler)
        {
            this.context = context;
            this.addPluCategoryCommandHandler = addPluCategoryCommandHandler;
        }

        public void Execute(AddPluCategoryManager data)
        {
            if (context.PLUCategory.ContainsDuplicatePluCategoryName(null, data.PluCategoryName, Convert.ToInt64(data.BeginRange), Convert.ToInt64(data.EndRange)))
            {
                throw new CommandException("Another PLU category with specified name exists.  Please enter new PLU category name.");
            }

            if (context.PLUCategory.ContainsDuplicatePluCategoryRange(null, data.PluCategoryName, Convert.ToInt64(data.BeginRange), Convert.ToInt64(data.EndRange)))
            {
                throw new CommandException("PLU category range overlaps with another existing PLU range.  Please enter new values.");
            }

            var addPluCategoryCommand = new AddPluCategoryCommand
            {
                PluCategory = new PLUCategory() { PluCategoryName = data.PluCategoryName, BeginRange = data.BeginRange, EndRange = data.EndRange }
            };

            try
            {
                addPluCategoryCommandHandler.Execute(addPluCategoryCommand);
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
