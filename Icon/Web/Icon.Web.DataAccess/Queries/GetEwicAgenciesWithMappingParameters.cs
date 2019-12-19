using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetEwicAgenciesWithMappingParameters : IQuery<List<Agency>> 
    {
        public string AplScanCode { get; set; }
        public string WfmScanCode { get; set; }
    }
}
