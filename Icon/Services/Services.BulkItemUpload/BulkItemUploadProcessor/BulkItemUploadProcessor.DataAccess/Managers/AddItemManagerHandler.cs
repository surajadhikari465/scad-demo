using System.Collections.Generic;
using BulkItemUploadProcessor.DataAccess.Commands;
using Icon.Common.DataAccess;

namespace BulkItemUploadProcessor.DataAccess.Managers
{
    public class AddItemManagerHandler : IManagerHandler<AddItemManager>
    {
        private ICommandHandler<AddItemCommand> addItemCommandHandler;
        private ICommandHandler<PublishItemUpdatesCommand> publishItemUpdatesCommandHandler;

        public AddItemManagerHandler(
            ICommandHandler<AddItemCommand> addItemCommandHandler,
            ICommandHandler<PublishItemUpdatesCommand> publishItemUpdatesCommandHandler)
        {
            this.addItemCommandHandler = addItemCommandHandler;
            this.publishItemUpdatesCommandHandler = publishItemUpdatesCommandHandler;
        }

        public void Execute(AddItemManager data)
        {
            AddItemCommand command = new AddItemCommand
            {
                MerchandiseHierarchyClassId = data.MerchandiseHierarchyClassId,
                BrandsHierarchyClassId = data.BrandsHierarchyClassId,
                TaxHierarchyClassId = data.TaxHierarchyClassId,
                NationalHierarchyClassId = data.NationalHierarchyClassId,
                SelectedBarCodeTypeId = data.BarCodeTypeId,
                ScanCode = data.ScanCode,
                FinancialHierarchyClassId = data.FinancialHierarchyClassId,
                ItemTypeCode = data.ItemTypeCode,
                ItemAttributes = data.ItemAttributes,
                ManufacturerHierarchyClassId = data.ManufacturerHierarchyClassId
            };

            addItemCommandHandler.Execute(command);
            data.ScanCode = command.ScanCode;

            publishItemUpdatesCommandHandler.Execute(
                new PublishItemUpdatesCommand
                {
                    ScanCodes = new List<string> { command.ScanCode }
                });
        }
    }
}