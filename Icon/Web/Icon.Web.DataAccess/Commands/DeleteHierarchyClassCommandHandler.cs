using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;
using System;

namespace Icon.Web.DataAccess.Commands
{
    public class DeleteHierarchyClassCommandHandler : ICommandHandler<DeleteHierarchyClassCommand>
    {
        private IconContext context;

        public DeleteHierarchyClassCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(DeleteHierarchyClassCommand data)
        {
            try
            {
                var deletedHierarchyClass = context.HierarchyClass.Find(data.DeletedHierarchyClass.hierarchyClassID);

                if (deletedHierarchyClass.HierarchyClassTrait.Count > 0)
                {
                    context.HierarchyClassTrait.RemoveRange(deletedHierarchyClass.HierarchyClassTrait);
                }
                
                context.HierarchyClass.Remove(deletedHierarchyClass);
                context.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new CommandException(String.Format("There was an error deleting Hierarchy Class ID {0}.  Error: {1}",
                    data.DeletedHierarchyClass.hierarchyClassID, exception.Message), exception);
            }
        }
    }
}
