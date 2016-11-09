using Icon.Framework;
using RegionalEventController.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace RegionalEventController.DataAccess.Queries
{
    public class GetDefaultCertificationAgenciesQueryHandler : IQueryHandler<GetDefaultCertificationAgenciesQuery, Dictionary<string, int>>
    {
        IconContext context;

        public GetDefaultCertificationAgenciesQueryHandler(IconContext context)
        {
            this.context = context;
        }

        public Dictionary<string, int> Execute(GetDefaultCertificationAgenciesQuery parameters)
        {
            var agencies = context.HierarchyClassTrait
                         .Where(hct => hct.Trait.traitCode == TraitCodes.DefaultCertificationAgency)
                         .ToList();

            var mapping = new Dictionary<string, int>();
            foreach (var agency in agencies)
            {
                mapping.Add(agency.traitValue, agency.hierarchyClassID);
            }

            return mapping;
        }
    }
}
