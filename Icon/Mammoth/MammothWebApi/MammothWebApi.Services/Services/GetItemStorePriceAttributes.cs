using MammothWebApi.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.Service.Services
{
    public class GetItemStorePriceAttributes
    {
        public DateTime EffectiveDate { get; set; }
        public IEnumerable<StoreScanCodeServiceModel> ItemStores { get; set; }
        public bool IncludeFuturePrices { get; set; }
    }
}
