using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using System;

namespace Icon.Web.DataAccess.Managers
{
    public class UpdateBrandManagerHandler : IManagerHandler<UpdateBrandManager>
    {
        private IconContext context;
        private ICommandHandler<UpdateBrandCommand> updateBrandCommandHandler;
        private ICommandHandler<UpdateBrandHierarchyClassTraitsCommand> updateHierarchyClassTraitsCommandHandler;
        private ICommandHandler<AddBrandMessageCommand> addBrandMessageCommandHandler;

        public UpdateBrandManagerHandler(
            IconContext context,
            ICommandHandler<UpdateBrandCommand> updateBrandCommandHandler,
            ICommandHandler<UpdateBrandHierarchyClassTraitsCommand> updateHierarchyClassTraitsCommandHandler,
            ICommandHandler<AddBrandMessageCommand> addBrandMessageCommandHandler)
        {
            this.context = context;
            this.updateBrandCommandHandler = updateBrandCommandHandler;
            this.updateHierarchyClassTraitsCommandHandler = updateHierarchyClassTraitsCommandHandler;
            this.addBrandMessageCommandHandler = addBrandMessageCommandHandler;
        }

        public void Execute(UpdateBrandManager data)
        {
            UpdateBrandCommand updateBrandCommand = Mapper.Map<UpdateBrandCommand>(data);
            UpdateBrandHierarchyClassTraitsCommand updateHierarchyClassTraitCommand = Mapper.Map<UpdateBrandHierarchyClassTraitsCommand>(data);
            AddBrandMessageCommand addBrandMessageCommand = new AddBrandMessageCommand
            {
                Brand = data.Brand,
                Action = MessageActionTypes.AddOrUpdate
            };

            try
            {
                updateBrandCommandHandler.Execute(updateBrandCommand);
                updateHierarchyClassTraitsCommandHandler.Execute(updateHierarchyClassTraitCommand);
                addBrandMessageCommandHandler.Execute(addBrandMessageCommand);
            }
            catch (DuplicateValueException ex)
            {
                throw new CommandException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new CommandException(String.Format("An error occurred when updating Brand ID {0}.", data.Brand.hierarchyClassID), ex);
            }
        }
    }
}
