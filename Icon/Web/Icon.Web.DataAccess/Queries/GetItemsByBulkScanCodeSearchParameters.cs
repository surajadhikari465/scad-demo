using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetItemsByBulkScanCodeSearchParameters : IQuery<List<ItemSearchModel>>
    {
        public List<string> ScanCodes { get; set; }        
    }
}
