using Icon.Common.DataAccess;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using System;

namespace Icon.Web.DataAccess.Managers
{
    public class UpdateAttributeManagerHandler : IManagerHandler<UpdateAttributeManager>
    {
        private readonly ICommandHandler<UpdateAttributeCommand> updateAttributeCommandHandler;
        private readonly ICommandHandler<AddUpdateCharacterSetCommand> addUpdateCharacterSetCommandHandler;
        private readonly ICommandHandler<AddUpdatePickListDataCommand> addUpdatePickListDataCommandHandler;
        private readonly ICommandHandler<AddMissingColumnsToItemColumnDisplayTableCommand> addMissingColumnToItemColumnOrderTableCommandHandler;

        public UpdateAttributeManagerHandler(
            ICommandHandler<UpdateAttributeCommand> updateAttributeCommandHandler,
            ICommandHandler<AddUpdateCharacterSetCommand> addUpdateCharacterSetCommandHandler,
            ICommandHandler<AddUpdatePickListDataCommand> addUpdatePickListDataCommandHandler,
            ICommandHandler<AddMissingColumnsToItemColumnDisplayTableCommand> addMissingColumnToItemColumnOrderTableCommandHandler)
        {
            this.updateAttributeCommandHandler = updateAttributeCommandHandler;
            this.addUpdateCharacterSetCommandHandler = addUpdateCharacterSetCommandHandler;
            this.addUpdatePickListDataCommandHandler = addUpdatePickListDataCommandHandler;
            this.addMissingColumnToItemColumnOrderTableCommandHandler = addMissingColumnToItemColumnOrderTableCommandHandler;
        }

        public void Execute(UpdateAttributeManager data)
        {
            try
            {
                updateAttributeCommandHandler.Execute(new UpdateAttributeCommand { AttributeModel = data.Attribute });
                addUpdateCharacterSetCommandHandler.Execute(new AddUpdateCharacterSetCommand
                    {
                        CharacterSetModelList = data.CharacterSetModelList,
                        AttributeId = data.Attribute.AttributeId
                    });
                if (data.Attribute.IsPickList)
                {
                    addUpdatePickListDataCommandHandler.Execute(new AddUpdatePickListDataCommand
                    {
                        PickListModel = data.PickListModel,
                        AttributeId = data.Attribute.AttributeId
                    });
                }

                addMissingColumnToItemColumnOrderTableCommandHandler.Execute(new AddMissingColumnsToItemColumnDisplayTableCommand());
            }
            catch (Exception ex)
            {
                throw new CommandException(String.Format("An error occurred while editing attribute. {0}", ex.Message), ex);
            }
        }
    }
}