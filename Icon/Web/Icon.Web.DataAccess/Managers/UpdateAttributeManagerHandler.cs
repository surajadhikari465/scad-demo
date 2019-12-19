using Icon.Common.DataAccess;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using System;

namespace Icon.Web.DataAccess.Managers
{
    public class UpdateAttributeManagerHandler : IManagerHandler<UpdateAttributeManager>
    {
        private ICommandHandler<UpdateAttributeCommand> updateAttributeCommandHandler;
        private ICommandHandler<AddUpdateCharacterSetCommand> addUpdateCharacterSetCommandHandler;
        private ICommandHandler<AddUpdatePickListDataCommand> addUpdatePickListDataCommandHandler;

        public UpdateAttributeManagerHandler(
            ICommandHandler<UpdateAttributeCommand> updateAttributeCommandHandler,
            ICommandHandler<AddUpdateCharacterSetCommand> addUpdateCharacterSetCommandHandler,
            ICommandHandler<AddUpdatePickListDataCommand> addUpdatePickListDataCommandHandler)
        {
            this.updateAttributeCommandHandler = updateAttributeCommandHandler;
            this.addUpdateCharacterSetCommandHandler = addUpdateCharacterSetCommandHandler;
            this.addUpdatePickListDataCommandHandler = addUpdatePickListDataCommandHandler;
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
            }
            catch (Exception ex)
            {
                throw new CommandException(String.Format("An error occurred while editing attribute. {0}", ex.Message), ex);
            }
        }
    }
}