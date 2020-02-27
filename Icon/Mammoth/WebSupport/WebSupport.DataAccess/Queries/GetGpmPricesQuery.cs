using Dapper;
using Icon.Common.DataAccess;
using Icon.Common;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Queries
{
    public class GetGpmPricesQuery : IQueryHandler<GetGpmPricesParameters, List<GpmPrice>>
    {
        private IDbConnection connection;

        public GetGpmPricesQuery(IDbConnection connection)
        {
            this.connection = connection;
        }

        public List<GpmPrice> Search(GetGpmPricesParameters parameters)
        {
            var sql = $@"SET NOCOUNT ON;
                DECLARE @today DATETIME2(7) = CAST(GetDate() AS DATE);

                IF(object_id('tempdb..#ids') IS NOT NULL) DROP TABLE #ids;
                CREATE TABLE #ids(ItemId INT);

                IF(IsNull(@itemId, -1) >= 0)
                    INSERT INTO #ids(ItemId)
                	SELECT TOP 1000 i.ItemID
                	FROM Items i 
                	JOIN gpm.Price_{parameters.Region} p ON p.ItemID = i.ItemID
                	WHERE p.ItemID > @itemId
                		AND p.BusinessUnitID = {parameters.BusinessUnitId}
                		AND (p.StartDate <= @today AND IsNull(p.EndDate, @today) >= @today)
                	GROUP BY i.ItemId
                ELSE
                    INSERT INTO #ids(ItemId)
                	SELECT i.ItemID
                	FROM Items i
                	JOIN @scanCodes sc ON sc.ScanCode = i.ScanCode
                	GROUP BY i.ItemID;

                SELECT p.Region
                	,p.PriceID AS PriceId
                	,p.GpmID AS GpmId
                	,p.ItemID AS ItemId
                	,p.BusinessUnitID AS BusinessUnitId
                	,p.StartDate
                	,p.EndDate
                	,p.Price
                	,p.PriceType
                	,p.PriceTypeAttribute
                	,p.SellableUOM
                	,p.CurrencyCode
                	,p.Multiple
                	,p.TagExpirationDate
                	,p.InsertDateUtc
                	,p.ModifiedDateUtc
                	,it.itemTypeCode AS ItemTypeCode
                	,l.StoreName
                	,i.ScanCode
                	,ms.PatchFamilyID AS PatchFamilyId
                	,ms.PatchFamilySequenceID AS SequenceId
                	,p.PercentOff
                FROM gpm.Price_{parameters.Region} p
                JOIN dbo.Items i ON p.ItemID = i.ItemID
                JOIN dbo.ItemTypes it ON i.itemTypeID = it.ItemTypeID
                JOIN dbo.Locales_{parameters.Region} l ON p.BusinessUnitID = l.BusinessUnitID
                LEFT JOIN gpm.MessageSequence ms ON i.ItemID = ms.ItemID
                	AND l.BusinessUnitID = ms.BusinessUnitID
                WHERE l.BusinessUnitID = {parameters.BusinessUnitId}
                	AND (p.StartDate <= @today AND IsNull(p.EndDate, @today) >= @today)
                	AND i.ItemID IN (SELECT ItemID FROM #ids);

                IF (object_id('tempdb..#ids') IS NOT NULL) DROP TABLE #ids;";

            var prices = connection.Query<GpmPrice>(sql, new
                {
                    itemId = parameters.ItemId,
                    scanCodes = parameters.ScanCodes.Select(x => new{ ScanCode = x }).ToDataTable().AsTableValuedParameter("gpm.ScanCodesType")
                });

            return prices.GroupBy(p => new{ p.ItemId, p.PriceType })
                         .Select(x => x.OrderBy(p => p.StartDate).Last())
                         .OrderBy(x => x.ItemId)
                         .ToList();
        }
    }
}
