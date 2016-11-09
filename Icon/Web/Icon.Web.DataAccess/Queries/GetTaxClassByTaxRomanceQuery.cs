using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetTaxClassByTaxRomanceQuery : IQueryHandler<GetTaxClassByTaxRomanceParameters, HierarchyClass>
    {
        private IconContext context;

        public GetTaxClassByTaxRomanceQuery(IconContext context)
        {
            this.context = context;
        }

        public HierarchyClass Search(GetTaxClassByTaxRomanceParameters parameters)
        {
            var hierarchyQuery = context.HierarchyClass
                .Where(hc => hc.HierarchyClassTrait.Any(hct => hct.traitID == Traits.TaxRomance && hct.traitValue.Equals(parameters.TaxRomance))
                && hc.Hierarchy.hierarchyID == Hierarchies.Tax && hc.hierarchyLevel == HierarchyLevels.Tax);
            
            if (hierarchyQuery.Count() > 0)
            {
                return hierarchyQuery.First();
            }
            else
            {
                return null;
            }
        }
    }
}
