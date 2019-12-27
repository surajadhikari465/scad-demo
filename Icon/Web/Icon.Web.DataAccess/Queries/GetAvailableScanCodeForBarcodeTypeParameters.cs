using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetAvailableScanCodeForBarcodeTypeParameters : IQuery<List<string>>
    {
        public int BarCodeTypeId { get; set; }
        public int Count { get; set; }
        public List<ItemIdAndScanCode> ExcludedScanCodes { get; set; }
    }
}
