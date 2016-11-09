using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetEwicAgenciesWithExclusionQuery : IQueryHandler<GetEwicAgenciesWithExclusionParameters, List<Agency>>
    {
        private readonly IconContext context;

        public GetEwicAgenciesWithExclusionQuery(IconContext context)
        {
            this.context = context;
        }

        public List<Agency> Search(GetEwicAgenciesWithExclusionParameters parameters)
        {
            var excludedAgencies = context.Agency.Where(a => a.ScanCode.Any(sc => sc.scanCode == parameters.ScanCode)).ToList();

            return excludedAgencies;
        }
    }
}
