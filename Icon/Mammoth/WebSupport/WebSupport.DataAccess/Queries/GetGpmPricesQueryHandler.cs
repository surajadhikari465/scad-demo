using Dapper;
using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Queries
{
    public class GetGpmPricesQueryHandler : IQueryHandler<GetGpmPricesParameters, List<GpmPrice>>
    {
        private IDbConnection connection;

        public GetGpmPricesQueryHandler(IDbConnection connection)
        {
            this.connection = connection;
        }

        public List<GpmPrice> Search(GetGpmPricesParameters parameters)
        {
            var prices = connection.Query<GpmPrice>($@"
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
                          ,p.NewTagExpiration
                          ,p.InsertDateUtc
                          ,p.ModifiedDateUtc
                          ,it.itemTypeCode AS ItemTypeCode
                          ,l.StoreName
                          ,i.ScanCode
                          ,ms.PatchFamilyID AS PatchFamilyId
                          ,ms.PatchFamilySequenceID AS SequenceId
                      FROM gpm.Price_{parameters.Region} p
                      JOIN dbo.Items i ON p.ItemID = i.ItemID
                      JOIN dbo.ItemTypes it ON i.itemTypeID = it.ItemTypeID
                      JOIN dbo.Locale l ON p.BusinessUnitID = l.BusinessUnitID
                      JOIN gpm.MessageSequence ms ON i.ItemID = ms.ItemID
                        AND l.BusinessUnitID = ms.BusinessUnitID
                      WHERE l.BusinessUnitID = @BusinessUnitId
                        AND i.ScanCode = @ScanCode",
                      parameters)
                      .Where(p => p.StartDate < DateTime.Now)
                      .ToList();

            var activeReg = prices.Where(p => p.PriceType == "REG").OrderBy(p => p.StartDate).LastOrDefault();
            var activeTpr = prices.Where(p => p.PriceType == "TPR").OrderBy(p => p.StartDate).LastOrDefault();

            var activePrices = new List<GpmPrice>();
            if(activeReg != null)
            {
                activePrices.Add(activeReg);
            }
            if(activeTpr != null)
            {
                activePrices.Add(activeTpr);
            }

            return activePrices;
        }
    }
}
