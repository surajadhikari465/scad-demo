using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Managers
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
            AddItemCommand command = new AddItemCommand();
            command.MerchandiseHierarchyClassId = data.MerchandiseHierarchyClassId;
            command.BrandsHierarchyClassId = data.BrandsHierarchyClassId;
            command.TaxHierarchyClassId = data.TaxHierarchyClassId;
            command.NationalHierarchyClassId = data.NationalHierarchyClassId;
            command.SelectedBarCodeTypeId = data.BarCodeTypeId;
            command.ScanCode = data.ScanCode;
            command.FinancialHierarchyClassId = data.FinancialHierarchyClassId;
            command.ItemTypeCode = data.ItemTypeCode;
            command.ItemAttributes = data.ItemAttributes;
            command.ManufacturerHierarchyClassId = data.ManufacturerHierarchyClassId;

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