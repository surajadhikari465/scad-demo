using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetEwicAgenciesWithExclusionParameters : IQuery<List<Agency>> 
    {
        public string ScanCode { get; set; }
    }
}
