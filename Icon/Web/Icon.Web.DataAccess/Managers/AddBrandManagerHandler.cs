using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using System;

namespace Icon.Web.DataAccess.Managers
{
    public class AddBrandManagerHandler : IManagerHandler<AddBrandManager>
    {
        private IconContext context;
        private ICommandHandler<AddBrandCommand> addBrandCommandHandler;
        private ICommandHandler<AddBrandMessageCommand> addBrandMessageCommandHandler;

        public AddBrandManagerHandler(
            IconContext context,
            ICommandHandler<AddBrandCommand> addBrandCommandHandler,
            ICommandHandler<AddBrandMessageCommand> addBrandMessageCommandHandler)
        {
            this.context = context;
            this.addBrandCommandHandler = addBrandCommandHandler;
            this.addBrandMessageCommandHandler = addBrandMessageCommandHandler;
        }

        public void Execute(AddBrandManager data)
        {

            AddBrandCommand addBrandCommand = Mapper.Map<AddBrandCommand>(data);

            var addBrandMessageCommand = new AddBrandMessageCommand
            {
                Brand = data.Brand,
                Action = MessageActionTypes.AddOrUpdate
            };

            try
            {
                addBrandCommandHandler.Execute(addBrandCommand);
                addBrandMessageCommandHandler.Execute(addBrandMessageCommand);
            }
            catch (DuplicateValueException ex)
            {
                throw new CommandException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new CommandException(String.Format("An error occurred when adding Brand {0}.", data.Brand.hierarchyClassName), ex);
            }
        }
    }
}
