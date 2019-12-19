using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;

namespace Icon.Web.DataAccess.Managers
{
    public class UpdateProductSelectionGroupManagerHandler : IManagerHandler<UpdateProductSelectionGroupManager>
    {
        private IconContext context;
        private ICommandHandler<UpdateProductSelectionGroupCommand> updateProductSelectionGroupCommandHandler;
        private ICommandHandler<AddProductSelectionGroupMessageCommand> addProductSelectionGroupMessageCommandHandler;

        public UpdateProductSelectionGroupManagerHandler(IconContext context,
            ICommandHandler<UpdateProductSelectionGroupCommand> updateProductSelectionGroupCommandHandler,
            ICommandHandler<AddProductSelectionGroupMessageCommand> addProductSelectionGroupMessageCommandHandler)
        {
            this.context = context;
            this.updateProductSelectionGroupCommandHandler = updateProductSelectionGroupCommandHandler;
            this.addProductSelectionGroupMessageCommandHandler = addProductSelectionGroupMessageCommandHandler;
        }

        public void Execute(UpdateProductSelectionGroupManager data)
        {
            var command = new UpdateProductSelectionGroupCommand
            {
                ProductSelectionGroupId = data.ProductSelectionGroupId,
                ProductSelectionGroupName = data.ProductSelectionGroupName,
                ProductSelectionGroupTypeId = data.ProductSelectionGroupTypeId,
                TraitId = data.TraitId,
                TraitValue = data.TraitValue,
                MerchandiseHierarchyClassId = data.MerchandiseHierarchyClassId,
                AttributeId = data.AttributeId,
                AttributeValue = data.AttributeValue
            };

            this.updateProductSelectionGroupCommandHandler.Execute(command);

            if(command.ProductSelectionGroupNameChanged || command.ProductSelectionGroupTypeChanged)
            {
                var addPsgMessageCommand = new AddProductSelectionGroupMessageCommand
                {
                    ProductSelectionGroupId = data.ProductSelectionGroupId,
                    ProductSelectionGroupName = data.ProductSelectionGroupName,
                    ProductSelectionGroupTypeId = data.ProductSelectionGroupTypeId
                };

                this.addProductSelectionGroupMessageCommandHandler.Execute(addPsgMessageCommand);
            }
        }
    }
}
