using Dapper;
using Icon.Common.DataAccess;
using Icon.Common;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Queries
{
    class GetPricesAll : IQueryHandler<GetPricesAllParameters, List<PriceResetPrice>>
    {
        private IDbConnection db;
        
        public GetPricesAll(IDbConnection db)
        {
            this.db = db;
        }

        public List<PriceResetPrice> Search(GetPricesAllParameters parameters)
        {
            var sql = $@"
                    DECLARE @today DATETIME2(7) = CAST(GETDATE() AS DATE);

                    SELECT DISTINCT l.BusinessUnitId, l.StoreName
	                INTO #locale
	                FROM @BusinessUnitIds bu
                    JOIN dbo.Locales_{parameters.Region} l on l.BusinessUnitID = bu.BusinessUnitId;

                    CREATE NONCLUSTERED INDEX IX_BU ON #locale(BusinessUnitId);

                    SELECT 
                         p.ItemID AS ItemId
                        ,i.ScanCode
                        ,p.GpmID AS GpmId
                        ,it.itemTypeCode AS ItemTypeCode
                        ,it.itemTypeDesc AS ItemTypeDesc
                        ,p.BusinessUnitID AS BusinessUnitId
                        ,l.StoreName
                        ,p.PriceType
                        ,p.PriceTypeAttribute
                        ,p.SellableUOM
                        ,p.CurrencyCode
                        ,p.Price
                        ,p.Multiple
                        ,p.TagExpirationDate
                        ,ms.PatchFamilySequenceID AS SequenceId
                        ,ms.PatchFamilyID AS PatchFamilyId
                        ,p.StartDate
                        ,p.EndDate
                        ,p.PercentOff
                      FROM gpm.Price_{parameters.Region} p
                      JOIN dbo.Items i ON p.ItemID = i.ItemID
                      JOIN dbo.ItemTypes it ON i.itemTypeID = it.ItemTypeID
                      JOIN #locale l ON p.BusinessUnitID = l.BusinessUnitID
                      LEFT JOIN gpm.MessageSequence ms ON i.ItemID = ms.ItemID AND l.BusinessUnitID = ms.BusinessUnitID
                      WHERE  p.StartDate >= @today or (p.StartDate < @today AND IsNull(p.EndDate, @today) >= @today);";
            
                      var prices = db.Query<PriceResetPrice>(sql,  new
                        {
                          BusinessUnitIds = parameters.BusinessUnitId.Distinct().Select(x => new{ BusinessUnitId = x }).ToDataTable().AsTableValuedParameter("gpm.BusinessUnitIdsType")
                        },
                        commandTimeout: 600)
                      .ToList();

            return  prices;
        }
    }
}