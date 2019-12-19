using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using System;

namespace Icon.Web.DataAccess.Managers
{
    public class AddProductSelectionGroupManagerHandler : IManagerHandler<AddProductSelectionGroupManager>
    {
        private IconContext context;
        private ICommandHandler<AddProductSelectionGroupCommand> addProductSelectionGroupCommandHandler;
        private ICommandHandler<AddProductSelectionGroupMessageCommand> addProductSelectionGroupMessageCommandHandler;

        public AddProductSelectionGroupManagerHandler(IconContext context,
            ICommandHandler<AddProductSelectionGroupCommand> addProductSelectionGroupCommandHandler,
            ICommandHandler<AddProductSelectionGroupMessageCommand> addProductSelectionGroupMessageCommandHandler)
        {
            this.context = context;
            this.addProductSelectionGroupCommandHandler = addProductSelectionGroupCommandHandler;
            this.addProductSelectionGroupMessageCommandHandler = addProductSelectionGroupMessageCommandHandler;
        }

        public void Execute(AddProductSelectionGroupManager data)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var command = new AddProductSelectionGroupCommand
                    {
                        ProductSelectionGroupName = data.ProductSelectionGroupName,
                        ProductSelectionGroupTypeId = data.ProductSelectionGroupTypeId,
                        TraitId = data.TraitId,
                        TraitValue = data.TraitValue,
                        MerchandiseHierarchyClassId = data.MerchandiseHierarchyClassId,
                        AttributeId = data.AttributeId,
                        AttributeValue = data.AttributeValue
                    };

                    this.addProductSelectionGroupCommandHandler.Execute(command);

                    var addProductSelectionGroupMessageCommand = new AddProductSelectionGroupMessageCommand
                    {
                        ProductSelectionGroupId = command.ProductSelectionGroupId,
                        ProductSelectionGroupName = data.ProductSelectionGroupName,
                        ProductSelectionGroupTypeId = data.ProductSelectionGroupTypeId
                    };

                    this.addProductSelectionGroupMessageCommandHandler.Execute(addProductSelectionGroupMessageCommand);

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
