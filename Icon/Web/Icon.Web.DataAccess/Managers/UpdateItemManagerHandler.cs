using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Managers
{
    public class UpdateItemManagerHandler : IManagerHandler<UpdateItemManager>
    {
        private ICommandHandler<UpdateItemCommand> updateItemCommandHandler;
        private ICommandHandler<PublishItemUpdatesCommand> publishItemUpdatesCommandHandler;

        public UpdateItemManagerHandler(
            ICommandHandler<UpdateItemCommand> updateItemCommandHandler,
            ICommandHandler<PublishItemUpdatesCommand> publishItemUpdatesCommandHandler)
        {
            this.updateItemCommandHandler = updateItemCommandHandler;
            this.publishItemUpdatesCommandHandler = publishItemUpdatesCommandHandler;
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
            publishItemUpdatesCommandHandler.Execute(
                new PublishItemUpdatesCommand
                {
                    ScanCodes = new List<string> { data.ScanCode }
                });
        }
    }
}
