using Mammoth.Common.DataAccess.CommandQuery;
using MammothWebApi.DataAccess.Models;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Queries
{
    public class GetActivePricesByScanCodeAndStoreQuery : IQuery<List<ItemPriceModel>>
    {
        public string Region { get; set; }
        public List<StoreScanCode> StoreScanCodes { get; set; }
    }
}
