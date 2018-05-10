using Icon.Common.DataAccess;
using System.Collections.Generic;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Queries
{
    public class GetItemPsgDataParameters : IQuery<IEnumerable<ItemPsgModel>>
    {
        public string Region { get; set; }
        public List<int> BusinessUnitIds { get; set; }
        public List<string> ScanCodes { get; set; }
        public List<string> PriceTypes { get; set; }
        public List<int> ExcludedPsNumbers { get; set; }
    }
}
