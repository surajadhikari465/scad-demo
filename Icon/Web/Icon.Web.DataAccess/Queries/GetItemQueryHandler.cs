using Dapper;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetItemQueryHandler : IQueryHandler<GetItemParameters, ItemDbModel>
    {
        private IDbConnection dbConnection;

        public GetItemQueryHandler(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public ItemDbModel Search(GetItemParameters parameters)
        {
            var hsFilter = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase) { "RecipeId", "Plu" };

            string sql = @"
                SELECT ItemId,
                    ItemTypeId,
                    ItemTypeCode,
                    ItemTypeDescription,
                    ScanCode,
                    BarcodeType,
                    BarcodeTypeId,
                    ScanCodeTypeDescription,
                    MerchandiseHierarchyClassId,
                    BrandsHierarchyClassId,
                    TaxHierarchyClassId,
                    FinancialHierarchyClassId,
                    NationalHierarchyClassId,
                    ManufacturerHierarchyClassId,
                    ItemAttributesJson
                FROM dbo.ItemView
                WHERE ScanCode = @ScanCode;

                SELECT * FROM nutrition.ItemNutrition
                WHERE Plu = @ScanCode;";

            var results = dbConnection.QueryMultiple(sql, parameters);
            var item = results.ReadFirstOrDefault<ItemDbModel>();
            if (item == null)
            {
                throw new InvalidOperationException($"No item was found given item scan code: {parameters.ScanCode}.");
            }
            else
            {
                var nutrition = results.ReadSingleOrDefault();
                if(nutrition != null)
                {
                    item.Nutritions = ((IDictionary<string, object>)nutrition)
                        .Where(x => x.Value != null && !hsFilter.Contains(x.Key))
                        .OrderBy(x => x.Key)
                        .ToDictionary(x => x.Key, x => x.Value.ToString());
                }

                return item;
            }
        }
    }
}
