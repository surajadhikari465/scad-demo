using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetTaxHierarchyClassesWithNoAbbreviationQuery : IQueryHandler<GetTaxHierarchyClassesWithNoAbbreviationParameters, List<HierarchyClass>>
    {
        private IconContext context;

        public GetTaxHierarchyClassesWithNoAbbreviationQuery(IconContext context)
        {
            this.context = context;
        }

        public List<HierarchyClass> Search(GetTaxHierarchyClassesWithNoAbbreviationParameters parameters)
        {
            return context.HierarchyClass
                .Where(hc => hc.hierarchyID == Hierarchies.Tax && !hc.HierarchyClassTrait.Any(hct => hct.traitID == Traits.TaxAbbreviation))
                .ToList();
        }
    }
}
