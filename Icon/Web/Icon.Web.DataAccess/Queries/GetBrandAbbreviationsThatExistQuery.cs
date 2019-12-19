using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetBrandAbbreviationsThatExistQuery : IQueryHandler<GetBrandAbbreviationsThatExistParameters, List<string>>
    {
        private IconContext context;

        public GetBrandAbbreviationsThatExistQuery(IconContext context)
        {
            this.context = context;
        }

        public List<string> Search(GetBrandAbbreviationsThatExistParameters parameters)
        {
            return context.HierarchyClassTrait
                .Where(hct => hct.traitID == Traits.BrandAbbreviation && parameters.BrandAbbreviations.Contains(hct.traitValue))
                .Select(hct => hct.traitValue)
                .ToList();
        }
    }
}
