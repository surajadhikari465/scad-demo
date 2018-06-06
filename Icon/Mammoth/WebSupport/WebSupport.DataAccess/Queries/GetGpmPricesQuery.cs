using Dapper;
using Icon.Common.DataAccess;
using System;
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
                          ,p.TagExpirationDate
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
                      JOIN dbo.Locales_{parameters.Region} l ON p.BusinessUnitID = l.BusinessUnitID
                      LEFT JOIN gpm.MessageSequence ms ON i.ItemID = ms.ItemID
                        AND l.BusinessUnitID = ms.BusinessUnitID
                      WHERE l.BusinessUnitID = @BusinessUnitId
                        AND i.ScanCode = @ScanCode",
                      parameters)
                      .Where(p => p.StartDate <= DateTime.Today && (!p.EndDate.HasValue || p.EndDate >= DateTime.Today) )
                      .ToList();

            var activePrices = new List<GpmPrice>();

            var priceGroups = prices.GroupBy(p => p.PriceType);
            foreach (var priceGroup in priceGroups)
            {
                var activePrice = priceGroup.OrderBy(p => p.StartDate).Last();
                activePrices.Add(activePrice);
            }

            return activePrices;
        }
    }
}
