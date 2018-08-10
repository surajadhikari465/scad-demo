using Mammoth.Common.DataAccess.CommandQuery;
using MammothWebApi.DataAccess.Models;
using System;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Queries
{
    public class GetItemPriceAttributesByStoreAndScanCodeQuery : IQuery<IEnumerable<ItemStorePriceModel>>
    {
        public string Region { get; set; }
        public DateTime EffectiveDate { get; set; }
        public IEnumerable<StoreScanCode> StoreScanCodeCollection { get; set; }
        public bool IncludeFuturePrices { get; set; }
        public string PriceType { get; set; }
    }
}
