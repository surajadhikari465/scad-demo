using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Managers
{
    public class DeleteNationalHierarchyManagerHandler : IManagerHandler<DeleteNationalHierarchyManager>
    {
        private IconContext context;
        private ICommandHandler<DeleteHierarchyClassCommand> deleteHierarchyClassHandler;

        public DeleteNationalHierarchyManagerHandler(
            IconContext context,
            ICommandHandler<DeleteHierarchyClassCommand> deleteHierarchyClassHandler)
        {
            this.context = context;
            this.deleteHierarchyClassHandler = deleteHierarchyClassHandler; 
        }

        public void Execute(DeleteNationalHierarchyManager data)
        {
            using (var transaction = this.context.Database.BeginTransaction())
            {
                DeleteHierarchyClassCommand deleteHierarchyClassCommand = new DeleteHierarchyClassCommand()
                {
                    DeletedHierarchyClass = data.DeletedHierarchyClass
                };

                try
                {
                   
                    deleteHierarchyClassHandler.Execute(deleteHierarchyClassCommand);                    

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new CommandException(String.Format("There was an error deleting national hierarchy hierarchyClassId {0}. Error: {1}",
                         data.DeletedHierarchyClass.hierarchyClassID, ex.Message), ex);
                }
            }
        }
    }
}