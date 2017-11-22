using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.DataAccess.Models;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MammothWebApi.DataAccess.Queries
{
    /// <summary>
    /// This query will retrieve every price for a particular item and store for stores that are live on GPM
    /// </summary>
    public class GetItemPriceAttributesByStoreAndScanCodeQueryHandler : IQueryHandler<GetItemPriceAttributesByStoreAndScanCodeQuery, IEnumerable<ItemStorePriceModel>>
    {
        private IDbProvider db;

        public GetItemPriceAttributesByStoreAndScanCodeQueryHandler(IDbProvider db)
        {
            this.db = db;
        }

        public IEnumerable<ItemStorePriceModel> Search(GetItemPriceAttributesByStoreAndScanCodeQuery parameters)
        {
            IEnumerable<ItemStorePriceModel> prices = this.db.Connection.Query<ItemStorePriceModel>(
                "[dbo].[GetItemStorePriceAttributes]",
                new
                {
                    Region = parameters.Region,
                    ItemStores = parameters.StoreScanCodeCollection.ToDataTable()
                },
                commandType: CommandType.StoredProcedure,
                transaction: this.db.Transaction);

            var allPrices = new List<ItemStorePriceModel>();
            if (prices.Any())
            {
                foreach (var priceGroup in prices.GroupBy(p => p.BusinessUnitID))
                {
                    // Get current regular price to filter out any expired regular prices that might still be in the database
                    var currentRegularPrice = priceGroup.Aggregate((p, next) =>
                    {
                        if (p.PriceType != "REG" && next.PriceType == "REG")
                            return next;
                        else if (p.PriceType == "REG" && next.PriceType == "REG"
                            && next.StartDate > p.StartDate && next.StartDate <= parameters.EffectiveDate)
                            return next;
                        else
                            return p;
                    });
                    var activeTprs = priceGroup.Where(p => p.PriceType != "REG" && p.StartDate <= DateTime.Today && p.EndDate >= DateTime.Today);
                    allPrices.Add(currentRegularPrice);
                    allPrices.AddRange(activeTprs);

                    // Include
                    if (parameters.IncludeFuturePrices)
                    {
                        var futurePrices = priceGroup.Where(p => p.StartDate > DateTime.Today);
                        allPrices.AddRange(futurePrices);
                    }
                }
            }

            return allPrices;
        }
    }
}
