using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.DataAccess.Models;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MoreLinq.Extensions;

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
                foreach (var priceGroup in prices.GroupBy(p => new {p.BusinessUnitID, p.ItemId}))
                {
                    // Get current regular price to filter out any expired regular prices that might still be in the database
                    var currentRegularPrice = priceGroup.Where(p => p.StartDate <= parameters.EffectiveDate
                                                                    && p.PriceType == "REG").MaxBy(x => x.StartDate).FirstOrDefault();

                    var activeTprs = priceGroup.Where(p => p.PriceType != "REG"
                                                        && p.StartDate <= parameters.EffectiveDate
                                                        && p.EndDate >= parameters.EffectiveDate);

                    if (String.IsNullOrWhiteSpace(parameters.PriceType))
                    {
                        if (currentRegularPrice != null)
                        {
                            allPrices.Add(currentRegularPrice);
                        }

                        allPrices.AddRange(activeTprs);
                    }
                    else
                    {
                        if (parameters.PriceType == "REG")
                        {
                            if (currentRegularPrice != null)
                            {
                                allPrices.Add(currentRegularPrice);
                            }
                        }
                        else
                        {
                            allPrices.AddRange(activeTprs.Where(p => p.PriceType == parameters.PriceType));
                        }
                    }

                    // Include
                    if (parameters.IncludeFuturePrices)
                    {
                        var futurePrices = priceGroup.Where(p => p.StartDate > DateTime.Today);

                        if (String.IsNullOrWhiteSpace(parameters.PriceType))
                        {
                            allPrices.AddRange(futurePrices);
                        }
                        else
                        {
                            allPrices.AddRange(futurePrices.Where(p => p.PriceType == parameters.PriceType));
                        }
                    }
                }
            }

            return allPrices;
        }
    }
}
