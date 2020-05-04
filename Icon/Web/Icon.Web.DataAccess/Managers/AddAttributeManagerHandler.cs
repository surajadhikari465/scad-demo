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
        private readonly ICommandHandler<AddAttributeCommand> addAttributeCommandHandler;
        private readonly ICommandHandler<AddUpdateCharacterSetCommand> addUpdateCharacterSetCommandHandler;
        private readonly ICommandHandler<AddUpdatePickListDataCommand> addUpdatePickListDataCommandHandler;
        private readonly ICommandHandler<AddAttributeMessageCommand> addAttributeMessageCommandHandler;
        private readonly ICommandHandler<AddMissingColumnsToItemColumnDisplayTableCommand> addMissingColumnsToItemColumnDisplayTableCommandHandler;

        public AddAttributeManagerHandler(
            ICommandHandler<AddAttributeCommand> addAttributeCommandHandler,
            ICommandHandler<AddUpdateCharacterSetCommand> addUpdateCharacterSetCommandHandler,
            ICommandHandler<AddUpdatePickListDataCommand> addUpdatePickListDataCommandHandler,
            ICommandHandler<AddAttributeMessageCommand> addAttributeMessageCommandHandler, 
            ICommandHandler<AddMissingColumnsToItemColumnDisplayTableCommand> addMissingColumnsToItemColumnDisplayTableCommandHandler)
        {
            this.addAttributeCommandHandler = addAttributeCommandHandler;
            this.addUpdateCharacterSetCommandHandler = addUpdateCharacterSetCommandHandler;
            this.addUpdatePickListDataCommandHandler = addUpdatePickListDataCommandHandler;
            this.addAttributeMessageCommandHandler = addAttributeMessageCommandHandler;
            this.addMissingColumnsToItemColumnDisplayTableCommandHandler = addMissingColumnsToItemColumnDisplayTableCommandHandler;
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
                addMissingColumnsToItemColumnDisplayTableCommandHandler.Execute(new AddMissingColumnsToItemColumnDisplayTableCommand());
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