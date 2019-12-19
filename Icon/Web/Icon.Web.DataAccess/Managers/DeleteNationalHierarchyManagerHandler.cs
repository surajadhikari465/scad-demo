using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using System;

namespace Icon.Web.DataAccess.Managers
{
    public class DeleteNationalHierarchyManagerHandler : IManagerHandler<DeleteNationalHierarchyManager>
    {
        private IconContext context;
        private ICommandHandler<DeleteHierarchyClassCommand> deleteHierarchyClassHandler;
        private ICommandHandler<AddHierarchyClassMessageCommand> addHierarchyClassMessageHandler;
        private IMapper mapper;

        public DeleteNationalHierarchyManagerHandler(
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

        public void Execute(DeleteNationalHierarchyManager data)
        {
            DeleteHierarchyClassCommand deleteHierarchyClassCommand = mapper.Map<DeleteHierarchyClassCommand>(data);

            AddHierarchyClassMessageCommand addHierarchyClassMessageCommand = new AddHierarchyClassMessageCommand()
            {
                HierarchyClass = deleteHierarchyClassCommand.DeletedHierarchyClass,
                ClassNameChange = true,
                DeleteMessage = true,
                NationalClassCode = data.NationalClassCode
            };

            try
            {
                this.addHierarchyClassMessageHandler.Execute(addHierarchyClassMessageCommand);
                this.deleteHierarchyClassHandler.Execute(deleteHierarchyClassCommand);
            }
            catch (Exception ex)
            {
                throw new CommandException(String.Format("There was an error deleting national hierarchy hierarchyClassId {0}. Error: {1}",
                     data.DeletedHierarchyClass.hierarchyClassID, ex.Message), ex);
            }
        }
    }
}