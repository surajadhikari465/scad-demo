using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetCertificationAgenciesByTraitQuery : IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>>
    {
        private readonly IconContext context;

        public GetCertificationAgenciesByTraitQuery(IconContext context)
        {
            this.context = context;
        }

        public List<HierarchyClass> Search(GetCertificationAgenciesByTraitParameters parameters)
        {
            var agencies = context.HierarchyClass
                .Where(hc =>
                    hc.Hierarchy.hierarchyName == HierarchyNames.CertificationAgencyManagement &&
                    hc.HierarchyClassTrait.Any(hct => hct.Trait.traitCode == parameters.AgencyTypeTraitCode))
                .ToList();

            return agencies;
        }
    }
}
