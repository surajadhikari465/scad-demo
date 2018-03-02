using Dapper;
using Icon.Common;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Esb.ProductListener.Models;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.Esb.ProductListener.Queries
{
    public class GetPrimeAffinityItemStoreModelsQuery : IQueryHandler<GetPrimeAffinityItemStoreModelsParameters, IEnumerable<PrimeAffinityItemStoreModel>>
    {
        private IDbProvider dbProvider;

        public GetPrimeAffinityItemStoreModelsQuery(IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }

        public IEnumerable<PrimeAffinityItemStoreModel> Search(GetPrimeAffinityItemStoreModelsParameters parameters)
        {
            return dbProvider.Connection.Query<PrimeAffinityItemStoreModel>(
                   @"CREATE TABLE #ItemIds
                     (
	                     ItemId INT
                     )
                     INSERT INTO #ItemIds(ItemId)
                     SELECT Value FROM @ItemIds

                     SELECT 
	                     l.Region,
	                     i.ItemID,
	                     l.BusinessUnitID,
	                     i.ScanCode,
	                     it.itemTypeCode AS ItemTypeCode,
	                     l.StoreName
                     FROM #ItemIds ids
                     JOIN dbo.Items i ON ids.ItemId = i.ItemID
                     JOIN dbo.ItemTypes it on i.ItemTypeID = it.itemTypeID
                     JOIN dbo.ItemLocaleAttributes ila on i.ItemID = ila.ItemID
                     JOIN dbo.Locale l on ila.BusinessUnitID = l.BusinessUnitID
                     WHERE ila.Authorized = 1
                     ORDER BY l.BusinessUnitID, i.ItemID",
                    new
                    {
                        ItemIds = parameters.ItemIds
                            .Distinct()
                            .Select(i => new { Value = i })
                            .ToDataTable()
                            .AsTableValuedParameter("IntListType")
                    },
                    dbProvider.Transaction).ToList();
        }
    }
}
