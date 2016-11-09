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
    public class DeleteHierarchyClassManagerHandler : IManagerHandler<DeleteHierarchyClassManager>
    {
        private IconContext context;
        private ICommandHandler<DeleteHierarchyClassCommand> deleteHierarchyClassHandler;
        private ICommandHandler<AddHierarchyClassMessageCommand> addHierarchyClassMessageHandler;
        private ICommandHandler<RemoveHierarchyClassFromIrmaItemsCommand> removeHierarchyClassFromIrmaItemsCommandHandler;
        
        public DeleteHierarchyClassManagerHandler(
            IconContext context,
            ICommandHandler<DeleteHierarchyClassCommand> deleteHierarchyClassHandler,
            ICommandHandler<AddHierarchyClassMessageCommand> addHierarchyClassMessageHandler,
            ICommandHandler<RemoveHierarchyClassFromIrmaItemsCommand> removeHierarchyClassFromIrmaItemsCommandHandler)
        {
            this.context = context;
            this.deleteHierarchyClassHandler = deleteHierarchyClassHandler;
            this.addHierarchyClassMessageHandler = addHierarchyClassMessageHandler;
            this.removeHierarchyClassFromIrmaItemsCommandHandler = removeHierarchyClassFromIrmaItemsCommandHandler;
        }

        public void Execute(DeleteHierarchyClassManager data)
        {
            using (var transaction = this.context.Database.BeginTransaction())
            {
                DeleteHierarchyClassCommand deleteHierarchyClassCommand = Mapper.Map<DeleteHierarchyClassCommand>(data);

                try
                {
                        //Creating the message first because when removing an entity from the context some of the nullable properties will be wiped
                        AddHierarchyClassMessageCommand addHierarchyClassMessageCommand = new AddHierarchyClassMessageCommand
                        {
                            ClassNameChange = true,
                            DeleteMessage = true,
                            HierarchyClass = deleteHierarchyClassCommand.DeletedHierarchyClass
                        };
                        addHierarchyClassMessageHandler.Execute(addHierarchyClassMessageCommand);
                    
                    deleteHierarchyClassHandler.Execute(deleteHierarchyClassCommand);
                    removeHierarchyClassFromIrmaItemsCommandHandler.Execute(
                        new RemoveHierarchyClassFromIrmaItemsCommand
                        {
                            HierarchyId = data.DeletedHierarchyClass.hierarchyID,
                            HierarchyClassId = data.DeletedHierarchyClass.hierarchyClassID
                        });

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new CommandException(String.Format("There was an error deleting hierarchyClassId {0}. Error: {1}",
                         data.DeletedHierarchyClass.hierarchyClassID, ex.Message), ex);
                }
            }
        }
    }
}