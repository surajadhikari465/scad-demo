using Icon.Common.DataAccess;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Icon.Web.DataAccess.Managers
{
    public class AddAttributeManagerHandler : IManagerHandler<AddAttributeManager>
    {
        private ICommandHandler<AddAttributeCommand> addAttributeCommandHandler;
        private ICommandHandler<AddUpdateCharacterSetCommand> addUpdateCharacterSetCommandHandler;
        private ICommandHandler<AddUpdatePickListDataCommand> addUpdatePickListDataCommandHandler;
        private ICommandHandler<AddAttributeMessageCommand> addAttributeMessageCommandHandler;

        public AddAttributeManagerHandler(
            ICommandHandler<AddAttributeCommand> addAttributeCommandHandler,
            ICommandHandler<AddUpdateCharacterSetCommand> addUpdateCharacterSetCommandHandler,
            ICommandHandler<AddUpdatePickListDataCommand> addUpdatePickListDataCommandHandler,
            ICommandHandler<AddAttributeMessageCommand> addAttributeMessageCommandHandler)
        {
            this.addAttributeCommandHandler = addAttributeCommandHandler;
            this.addUpdateCharacterSetCommandHandler = addUpdateCharacterSetCommandHandler;
            this.addUpdatePickListDataCommandHandler = addUpdatePickListDataCommandHandler;
            this.addAttributeMessageCommandHandler = addAttributeMessageCommandHandler;
        }

        public void Execute(AddAttributeManager data)
        {
            try
            {
                addAttributeCommandHandler.Execute(new AddAttributeCommand { AttributeModel = data.Attribute });
                addUpdateCharacterSetCommandHandler.Execute(new AddUpdateCharacterSetCommand
                {
                    CharacterSetModelList = data.CharacterSetModelList,
                    AttributeId = data.Attribute.AttributeId
                });
                addUpdatePickListDataCommandHandler.Execute(new AddUpdatePickListDataCommand()
                {
                    PickListModel = data.PickListModel,
                    AttributeId = data.Attribute.AttributeId
                });
                addAttributeMessageCommandHandler.Execute(new AddAttributeMessageCommand
                {
                    AttributeModel = data.Attribute
                });
            }
            catch (DuplicateValueException ex)
            {
                throw new CommandException(ex.Message, ex);
            }
            catch (DataException ex)
            {
                throw new CommandException(ex.Message, ex);
            }
            catch (SqlException ex)
            {
                throw new CommandException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new CommandException($"An error occurred in creating attribute.", ex);
            }
        }
    }
}