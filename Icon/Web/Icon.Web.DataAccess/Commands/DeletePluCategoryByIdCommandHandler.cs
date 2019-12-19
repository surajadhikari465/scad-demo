using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using System;

namespace Icon.Web.DataAccess.Commands
{
    public class DeletePluCategoryByIdCommandHandler : ICommandHandler<DeletePluCategoryByIdCommand>
    {
        private IconContext context;

        public DeletePluCategoryByIdCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(DeletePluCategoryByIdCommand data)
        {
            try
            {                
                var deletedPluCategoryClass = context.PLUCategory.Find(data.PluCategoryID);

                context.PLUCategory.Remove(deletedPluCategoryClass);
                context.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new CommandException(String.Format("There was an error deleting PLUCategory ID {0}.  Error: {1}",
                    data.PluCategoryID, exception.Message), exception);
            }
        }
    }
}
