using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetEwicAgenciesWithoutMappingQuery : IQueryHandler<GetEwicAgenciesWithoutMappingParameters, List<Agency>>
    {
        private readonly IconContext context;

        public GetEwicAgenciesWithoutMappingQuery(IconContext context)
        {
            this.context = context;
        }

        public List<Agency> Search(GetEwicAgenciesWithoutMappingParameters parameters)
        {
            var currentlyMappedAgencies = context.Mapping.Where(m =>
                m.ScanCode.scanCode == parameters.WfmScanCode && m.AplScanCode == parameters.AplScanCode).Select(m => m.AgencyId).ToList();

            var agenciesWithoutMapping = context.AuthorizedProductList
                .Where(apl => apl.ScanCode == parameters.AplScanCode && !currentlyMappedAgencies.Contains(apl.AgencyId))
                .Select(apl => apl.Agency)
                .ToList();

            return agenciesWithoutMapping;
        }
    }
}
