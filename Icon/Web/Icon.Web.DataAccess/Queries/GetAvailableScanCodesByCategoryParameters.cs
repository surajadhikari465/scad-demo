using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetAvailableScanCodesByCategoryParameters : IQuery<List<IRMAItem>>
    {
        public int CategoryId { get; set; }
        public int MaxScanCodes { get; set; }
    }
}
