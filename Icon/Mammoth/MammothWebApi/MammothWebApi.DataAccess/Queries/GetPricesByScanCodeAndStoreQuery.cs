using Mammoth.Common.DataAccess.CommandQuery;
using MammothWebApi.DataAccess.Models;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Queries
{
    public class GetPricesByScanCodeAndStoreQuery : IQuery<List<ItemPriceModel>>
    {
        public string Region { get; set; }
        public List<string> ScanCodes { get; set; }
        public List<int> BusinessUnitIds { get; set; }
    }
}
