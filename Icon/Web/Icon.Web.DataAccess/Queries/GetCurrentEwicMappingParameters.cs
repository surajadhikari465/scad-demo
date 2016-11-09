using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Infrastructure;

namespace Icon.Web.DataAccess.Queries
{
    public class GetCurrentEwicMappingParameters : IQuery<string> 
    {
        public string AplScanCode { get; set; }
        public string WfmScanCode { get; set; }
    }
}
