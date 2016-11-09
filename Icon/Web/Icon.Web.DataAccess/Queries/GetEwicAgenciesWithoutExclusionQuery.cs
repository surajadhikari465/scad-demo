using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetEwicAgenciesWithoutExclusionQuery : IQueryHandler<GetEwicAgenciesWithoutExclusionParameters, List<Agency>>
    {
        private readonly IconContext context;

        public GetEwicAgenciesWithoutExclusionQuery(IconContext context)
        {
            this.context = context;
        }

        public List<Agency> Search(GetEwicAgenciesWithoutExclusionParameters parameters)
        {
            var agenciesForExclusion = context.Agency.Where(a => !a.ScanCode.Any(sc => sc.scanCode == parameters.ScanCode)).ToList();

            return agenciesForExclusion;
        }
    }
}
