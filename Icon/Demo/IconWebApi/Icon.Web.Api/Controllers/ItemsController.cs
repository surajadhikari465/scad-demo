using Icon.Web.Api.DataLayer;
using Icon.Web.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;

namespace Icon.Web.Api.Controllers
{
    public class ItemsController : ApiController
    {
        public ItemsController()
        {
        }

        [HttpPost]
        public List<ItemModel> GetItemsByScanCodes(List<string> scanCodes)
        {
            List<ItemSearchModel> queryResults = null;
            using(var connection = new SqlConnection(@"data source=cewd1815\SQLSHARED2012D;initial catalog=IconDev;Integrated Security=True;MultipleActiveResultSets=True"))
            {
                queryResults = connection.Query<ItemSearchModel>(
                        "app.GetItemsByBulkScanCodeSearch",
                        new { SearchScanCodes = scanCodes.ConvertAll(sc => new { ScanCode = sc }).ToDataTable().AsTableValuedParameter("app.ScanCodeListType") },
                        commandType: CommandType.StoredProcedure)
                    .ToList();
            }
            var items = queryResults
                .Select(ism => new ItemModel
                    {
                        ScanCode = ism.ScanCode,
                        Brand = ism.BrandName,
                        ProductDescription = ism.ProductDescription,
                        PosDescription = ism.PosDescription,
                        PackageUnit = ism.PackageUnit,
                        FoodStampEligible = ism.GetFoodStampEligible(),
                        PosScaleTare = ism.PosScaleTare,
                        RetailSize = ism.RetailSize,
                        RetailUom = ism.RetailUom,
                        MerchandiseHierarchyName = ism.MerchandiseHierarchyName,
                        NationalHierarchyName = ism.NationalHierarchyName,
                        TaxHierarchyName = ism.TaxHierarchyName
                    })
                    .ToList();

            return items;
        }
    }
}
