using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;

namespace Icon.Web.DataAccess.Managers
{
    public class UpdateItemManagerHandler : IManagerHandler<UpdateItemManager>
    {
        private ICommandHandler<UpdateItemCommand> updateItemCommandHandler;

        public UpdateItemManagerHandler(
            ICommandHandler<UpdateItemCommand> updateItemCommandHandler)
        {
            this.updateItemCommandHandler = updateItemCommandHandler;
        }

        public void Execute(UpdateItemManager data)
        {
            updateItemCommandHandler.Execute(
                new UpdateItemCommand
                {
                    BrandsHierarchyClassId = data.BrandsHierarchyClassId,
                    FinancialHierarchyClassId = data.FinancialHierarchyClassId,
                    ItemAttributes = data.ItemAttributes,
                    ItemId = data.ItemId,
                    MerchandiseHierarchyClassId = data.MerchandiseHierarchyClassId,
                    NationalHierarchyClassId = data.NationalHierarchyClassId,
                    TaxHierarchyClassId = data.TaxHierarchyClassId,
                    ManufacturerHierarchyClassId = data.ManufacturerHierarchyClassId,
                    ItemTypeId = data.ItemTypeId
                });
        }
    }
}
