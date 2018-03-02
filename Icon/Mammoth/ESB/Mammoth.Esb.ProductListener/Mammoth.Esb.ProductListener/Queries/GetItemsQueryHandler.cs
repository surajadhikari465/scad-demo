using Dapper;
using Icon.Common;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Esb.ProductListener.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Mammoth.Esb.ProductListener.Queries
{
    public class GetItemsQueryHandler : IQueryHandler<GetItemsParameters, IEnumerable<ItemDataAccessModel>>
    {
        private IDbConnection connection;

        public GetItemsQueryHandler(IDbConnection connection)
        {
            this.connection = connection;
        }

        public IEnumerable<ItemDataAccessModel> Search(GetItemsParameters parameters)
        {
            return connection.Query<ItemDataAccessModel>(
                   $@"  CREATE TABLE #ItemIds
                         (
	                         ItemId INT PRIMARY KEY
                         )
                         INSERT INTO #ItemIds(ItemId)
                         SELECT Value FROM @ItemIds

                        SELECT 
                            i.ItemID
                            ,i.ItemTypeID
                            ,it.itemTypeCode AS ItemTypeCode
                            ,i.ScanCode
                            ,i.HierarchyMerchandiseID
                            ,i.HierarchyNationalClassID
                            ,i.BrandHCID
                            ,i.TaxClassHCID
                            ,i.PSNumber
                            ,i.Desc_Product
                            ,i.Desc_POS
                            ,i.PackageUnit
                            ,i.RetailSize
                            ,i.RetailUOM
                            ,i.FoodStampEligible
                            ,i.Desc_CustomerFriendly
                            ,i.AddedDate
                            ,i.ModifiedDate
                        FROM #ItemIds ids
                        JOIN dbo.Items i ON ids.ItemID = i.ItemID
                        JOIN dbo.ItemTypes it ON i.ItemTypeID = it.itemTypeID",
                        new
                        {
                            ItemIds = parameters.ItemIds
                                .Select(i => new { Value = i })
                                .ToDataTable()
                                .AsTableValuedParameter("dbo.IntListType")
                        });

        }
    }
}