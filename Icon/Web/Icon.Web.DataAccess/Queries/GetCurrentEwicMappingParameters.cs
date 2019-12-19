using Icon.Common.DataAccess;

namespace Icon.Web.DataAccess.Queries
{
    public class GetCurrentEwicMappingParameters : IQuery<string> 
    {
        public string AplScanCode { get; set; }
        public string WfmScanCode { get; set; }
    }
}
