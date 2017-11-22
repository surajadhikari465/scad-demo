using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MammothWebApi.Models
{
    public class PriceCollectionRequestModel
    {
        public IEnumerable<StoreItem> StoreItems { get; set; }
        public bool IncludeFuturePrices { get; set; }
    }
}