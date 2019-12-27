using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;

namespace Icon.Web.DataAccess.Managers
{
    public class AddItemManagerHandler : IManagerHandler<AddItemManager>
    {
        private ICommandHandler<AddItemCommand> addItemCommandHandler;

        public AddItemManagerHandler(
            ICommandHandler<AddItemCommand> addItemCommandHandler)
        {
            this.addItemCommandHandler = addItemCommandHandler;
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
            command.ItemTypeId = data.ItemTypeId;
            command.ItemAttributes = data.ItemAttributes;
            command.ManufacturerHierarchyClassId = data.ManufacturerHierarchyClassId;

            addItemCommandHandler.Execute(command);

            data.ItemId = command.ItemId;
            data.ScanCode = command.ScanCode;
        }
    }
}