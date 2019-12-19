using System.Collections.Generic;
using BulkItemUploadProcessor.DataAccess.Commands;
using Icon.Common.DataAccess;

namespace BulkItemUploadProcessor.DataAccess.Managers
{
    public class UpdateItemManagerHandler : IManagerHandler<UpdateItemManager>
    {
        private readonly ICommandHandler<UpdateItemCommand> UpdateItemCommandHandler;
        private readonly ICommandHandler<PublishItemUpdatesCommand> PublishItemUpdatesCommandHandler;

        public UpdateItemManagerHandler(
            ICommandHandler<UpdateItemCommand> updateItemCommandHandler,
            ICommandHandler<PublishItemUpdatesCommand> publishItemUpdatesCommandHandler)
        {
            this.UpdateItemCommandHandler = updateItemCommandHandler;
            this.PublishItemUpdatesCommandHandler = publishItemUpdatesCommandHandler;
        }

        public void Execute(UpdateItemManager data)
        {


            var updateItemCommand = new UpdateItemCommand
            {
                BrandsHierarchyClassId = data.BrandsHierarchyClassId,
                FinancialHierarchyClassId = data.FinancialHierarchyClassId,
                ItemAttributes = data.ItemAttributes,
                ItemId = data.ItemId,
                MerchandiseHierarchyClassId = data.MerchandiseHierarchyClassId,
                NationalHierarchyClassId = data.NationalHierarchyClassId,
                TaxHierarchyClassId = data.TaxHierarchyClassId,
                ItemTypeCode = data.ItemTypeCode, 
                ManufacturerHierarchyClassId =  data.ManufacturerHierarchyClassId
            };


            var publishItemUpdatesCommand = new PublishItemUpdatesCommand
            {
                ScanCodes = new List<string> {data.ScanCode}
            };

            UpdateItemCommandHandler.Execute(updateItemCommand);
            PublishItemUpdatesCommandHandler.Execute(publishItemUpdatesCommand);
        }
    }
}