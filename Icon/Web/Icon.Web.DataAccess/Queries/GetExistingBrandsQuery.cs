using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetExistingBrandsQuery : IQueryHandler<GetExistingBrandsParameters, List<string>>
    {
        private IconContext context;

        public GetExistingBrandsQuery(IconContext context)
        {
            this.context = context;
        }

        public List<string> Search(GetExistingBrandsParameters parameters)
        {
            return context.HierarchyClass.Where(hc => hc.hierarchyID == Hierarchies.Brands && parameters.BrandNames.Contains(hc.hierarchyClassName))
                .Select(hc => hc.hierarchyClassName)
                .ToList();
        }
    }
}
