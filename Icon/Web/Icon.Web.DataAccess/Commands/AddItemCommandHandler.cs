using Icon.Common.DataAccess;
using Dapper;
using System.Linq;
using Newtonsoft.Json;
using System.Data;

namespace Icon.Web.DataAccess.Commands
{
    public class AddItemCommandHandler : ICommandHandler<AddItemCommand>
    {
        private IDbConnection dbConnection;

        public AddItemCommandHandler(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public void Execute(AddItemCommand data)
        {
            string itemScanCode = (this.dbConnection.Query<string>(sql: "dbo.AddItem",
                                           param: new
                                           {   
                                               brandsHierarchyClassId = data.BrandsHierarchyClassId,
                                               financialHierarchyClassId = data.FinancialHierarchyClassId,
                                               merchandiseHierarchyClassId = data.MerchandiseHierarchyClassId,
                                               nationalHierarchyClassId = data.NationalHierarchyClassId,
                                               taxHierarchyClassId = data.TaxHierarchyClassId,
                                               manufacturerHierarchyClassId = data.ManufacturerHierarchyClassId,
                                               itemTypeCode = data.ItemTypeCode,
                                               selectedBarCodeTypeId = data.SelectedBarCodeTypeId,
                                               scanCode = data.ScanCode,
                                               itemAttributesJson = JsonConvert.SerializeObject(data.ItemAttributes),
                                               itemScanCode = data.ScanCode
                                           },
                                           commandType: System.Data.CommandType.StoredProcedure)).First();

            data.ScanCode = itemScanCode;       
        }
    }
}