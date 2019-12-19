using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetEwicAgenciesWithoutExclusionParameters : IQuery<List<Agency>> 
    {
        public string ScanCode { get; set; }
    }
}
