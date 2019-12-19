using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetTaxClassesWithNoAbbreviationQuery : IQueryHandler<GetTaxClassesWithNoAbbreviationParameters, List<string>>
    {
        private readonly IconContext context;
        
        public 
            GetTaxClassesWithNoAbbreviationQuery(IconContext context)
        {
            this.context = context;
        }

        public List<string> Search(GetTaxClassesWithNoAbbreviationParameters parameters)
        {
            var taxClassesWithNoAbbreviation = context.HierarchyClass
                .Where(hc => 
                    parameters.TaxClasses.Contains(hc.hierarchyClassID.ToString()) && 
                    !hc.HierarchyClassTrait.Any(hct => hct.Trait.traitCode == TraitCodes.TaxAbbreviation))
                .ToList();

            return taxClassesWithNoAbbreviation.Select(tc => tc.hierarchyClassID.ToString()).ToList();
        }
    }
}
