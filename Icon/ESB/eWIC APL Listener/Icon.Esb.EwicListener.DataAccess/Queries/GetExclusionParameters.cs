using Icon.Common.DataAccess;
using Icon.Esb.EwicAplListener.Common.Models;

namespace Icon.Esb.EwicAplListener.DataAccess.Queries
{
    public class GetExclusionParameters : IQuery<ScanCodeModel>
    {
        public string ExcludedScanCode { get; set; }
    }
}
