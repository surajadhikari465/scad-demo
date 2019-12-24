using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using System;

namespace Icon.Web.DataAccess.Managers
{
    public class DeleteHierarchyClassManagerHandler : IManagerHandler<DeleteHierarchyClassManager>
    {
        private IconContext context;
        private ICommandHandler<DeleteHierarchyClassCommand> deleteHierarchyClassHandler;
        private ICommandHandler<AddHierarchyClassMessageCommand> addHierarchyClassMessageHandler;
        private IMapper mapper;

        public DeleteHierarchyClassManagerHandler(
            IconContext context,
            ICommandHandler<DeleteHierarchyClassCommand> deleteHierarchyClassHandler,
            ICommandHandler<AddHierarchyClassMessageCommand> addHierarchyClassMessageHandler,
            IMapper mapper)
        {
            this.context = context;
            this.deleteHierarchyClassHandler = deleteHierarchyClassHandler;
            this.addHierarchyClassMessageHandler = addHierarchyClassMessageHandler;
            this.mapper = mapper;
        }

        public void Execute(DeleteHierarchyClassManager data)
        {
            using (var transaction = this.context.Database.BeginTransaction())
            {
                DeleteHierarchyClassCommand deleteHierarchyClassCommand = mapper.Map<DeleteHierarchyClassCommand>(data);
                try
                {
                    if (data.EnableHierarchyMessages)
                    {
                        //Creating the message first because when removing an entity from the context some of the nullable properties will be wiped
                        AddHierarchyClassMessageCommand addHierarchyClassMessageCommand =
                            new AddHierarchyClassMessageCommand
                            {
                                ClassNameChange = true,
                                DeleteMessage = true,
                                HierarchyClass = deleteHierarchyClassCommand.DeletedHierarchyClass
                            };
                        addHierarchyClassMessageHandler.Execute(addHierarchyClassMessageCommand);
                    }

                    deleteHierarchyClassHandler.Execute(deleteHierarchyClassCommand);

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