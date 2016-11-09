using Icon.Framework;
using RegionalEventController.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace RegionalEventController.DataAccess.Queries
{
    public class GetBrandAbbreviationQueryHandler : IQueryHandler<GetBrandAbbreviationQueryQuery, Dictionary<string, string>>
    {
        IconContext context;

        public GetBrandAbbreviationQueryHandler(IconContext context)
        {
            this.context = context;
        }

        public Dictionary<string, string> Execute(GetBrandAbbreviationQueryQuery parameters)
        {
            var brandDictionary = context.HierarchyClassTrait
                         .Where(hct => hct.traitID == Traits.BrandAbbreviation)
                         .Select(hct => new { tkey = hct.HierarchyClass.hierarchyClassName.ToLower(), tvalue = hct.traitValue })
                         .OrderBy(bl => bl.tkey)
                         .ToDictionary(t => t.tkey.Length > 25 ? t.tkey.Substring(0,25): t.tkey, t => t.tvalue);

            return brandDictionary;
        }
    }
}
