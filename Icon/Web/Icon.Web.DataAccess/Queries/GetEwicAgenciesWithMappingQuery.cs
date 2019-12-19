using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetEwicAgenciesWithMappingQuery : IQueryHandler<GetEwicAgenciesWithMappingParameters, List<Agency>>
    {
        private readonly IconContext context;

        public GetEwicAgenciesWithMappingQuery(IconContext context)
        {
            this.context = context;
        }

        public List<Agency> Search(GetEwicAgenciesWithMappingParameters parameters)
        {
            var mappedAgencies = context.Mapping
                .Where(m => m.AplScanCode == parameters.AplScanCode && m.ScanCode.scanCode == parameters.WfmScanCode)
                .Select(m => m.Agency)
                .ToList();

            return mappedAgencies;
        }
    }
}
