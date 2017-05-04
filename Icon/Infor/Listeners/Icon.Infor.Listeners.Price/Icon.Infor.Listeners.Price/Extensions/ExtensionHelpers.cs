using Icon.Infor.Listeners.Price.DataAccess.Models;
using Icon.Infor.Listeners.Price.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Infor.Listeners.Price.Extensions
{
    public static class ExtensionHelpers
    {
        public static IEnumerable<DbPriceModel> ToDbPriceModel(this IEnumerable<PriceModel> prices)
        {
            IEnumerable<DbPriceModel> dbPrices = prices
                .Select(p => new DbPriceModel
                {
                    BusinessUnitID = p.BusinessUnitId,
                    CurrencyID = p.CurrencyId,
                    EndDate = p.EndDate,
                    GpmID = p.GpmId,
                    ItemID = p.ItemId,
                    Multiple = p.Multiple,
                    NewTagExpiration = p.NewTagExpiration,
                    Price = p.Price,
                    PriceType = p.PriceType.ToString(),
                    PriceTypeAttribute = p.PriceTypeAttribute.ToString(),
                    PriceUOM = p.SellableUom,
                    Region = p.Region,
                    StartDate = p.StartDate,
                    ReplaceGpmId = p.ReplacedGpmId,
                    AddedDate = p.AddedDate
                });

            return dbPrices;
        }

        public static DateTime? ParseToNullableDateTime(this string text)
        {
            DateTime date;
            if (DateTime.TryParse(text, out date))
            {
                return date;
            }
            else
            {
                return null;
            }
        }
    }
}
