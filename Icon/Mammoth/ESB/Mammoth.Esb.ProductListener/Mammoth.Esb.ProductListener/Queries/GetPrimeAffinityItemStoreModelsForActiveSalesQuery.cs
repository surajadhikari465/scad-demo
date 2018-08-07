using Dapper;
using Icon.Common;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Esb.ProductListener.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.Esb.ProductListener.Queries
{
    public class GetPrimeAffinityItemStoreModelsForActiveSalesQuery : IQueryHandler<GetPrimeAffinityItemStoreModelsForActiveSalesParameters, List<PrimeAffinityItemStoreModel>>
    {
        private IDbProvider dbProvider;

        public GetPrimeAffinityItemStoreModelsForActiveSalesQuery(IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }

        public List<PrimeAffinityItemStoreModel> Search(GetPrimeAffinityItemStoreModelsForActiveSalesParameters parameters)
        {
            return dbProvider.Connection.Query<PrimeAffinityItemStoreModel>(
                @"
                SELECT Value AS ItemID
                INTO #ItemIDs
                FROM @ItemIDs

                SELECT l.Region
	                ,i.ItemID
	                ,l.BusinessUnitID
	                ,i.ScanCode
	                ,it.itemTypeCode AS ItemTypeCode
	                ,l.StoreName
					,p.PriceType
                    ,p.Price
					,p.StartDate
					,p.EndDate
                FROM #ItemIDs ids
                JOIN dbo.Items i ON ids.ItemId = i.ItemID
                JOIN dbo.ItemTypes it ON i.ItemTypeID = it.itemTypeID
                JOIN dbo.ItemLocaleAttributes ila ON ids.ItemID = ila.ItemID
                JOIN (
	                SELECT ItemID
		                ,BusinessUnitID
		                ,MAX(StartDate) AS StartDate
	                FROM dbo.Price
	                WHERE PriceType <> 'REG'
		                AND StartDate <= @Today
		                AND EndDate >= @Today
	                GROUP BY ItemID
		                ,BusinessUnitID
	                ) activePrice ON ila.ItemID = activePrice.ItemID
						AND ila.BusinessUnitID = activePrice.BusinessUnitID
                JOIN dbo.Price p ON activePrice.ItemID = p.ItemID
	                AND activePrice.BusinessUnitID = p.BusinessUnitID
	                AND activePrice.StartDate = p.StartDate
                JOIN dbo.Locale l ON ila.BusinessUnitID = l.BusinessUnitID
                LEFT JOIN dbo.RegionGpmStatus rgs ON rgs.Region = l.Region
                WHERE ila.Authorized = 1
	                AND p.PriceType IN @EligiblePriceTypes
                    AND ISNULL(rgs.IsGpmEnabled, 0) = 0
                UNION
                SELECT l.Region
	                ,i.ItemID
	                ,l.BusinessUnitID
	                ,i.ScanCode
	                ,it.itemTypeCode AS ItemTypeCode
	                ,l.StoreName
					,p.PriceType
                    ,p.Price
					,p.StartDate
					,p.EndDate
                FROM #ItemIDs ids
                JOIN dbo.Items i ON ids.ItemId = i.ItemID
                JOIN dbo.ItemTypes it ON i.ItemTypeID = it.itemTypeID
                JOIN dbo.ItemLocaleAttributes ila ON ids.ItemID = ila.ItemID
                JOIN gpm.Prices p ON ila.ItemID = p.ItemID
	                AND ila.BusinessUnitID = p.BusinessUnitID
                    AND ila.Region = p.Region
                JOIN dbo.Locale l ON ila.BusinessUnitID = l.BusinessUnitID
                JOIN dbo.RegionGpmStatus rgs ON rgs.Region = l.Region
                WHERE rgs.IsGpmEnabled = 1
                    AND ila.Authorized = 1
                    AND rgs.IsGpmEnabled = 1 
                    AND PriceType = 'TPR'
		            AND StartDate <= @Today
		            AND EndDate >= @Today
                ORDER BY l.BusinessUnitID",
                new
                {
                    ItemIDs = parameters.Items
                        .Select(i => new { Value = i.ItemID })
                        .ToDataTable()
                        .AsTableValuedParameter("IntListType"),
                    DateTime.Today,
                    parameters.EligiblePriceTypes
                },
                dbProvider.Transaction).ToList();
        }
    }
}
