using Icon.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Icon.Common.DataAccess;

namespace Icon.Web.DataAccess.Queries
{
    public class GetExistingHierarchyClassesQuery : IQueryHandler<GetExistingHierarchyClassesParameters, List<HierarchyClass>>
    {
        private readonly IconContext context;

        public GetExistingHierarchyClassesQuery(IconContext context)
        {
            this.context = context;
        }

        public List<HierarchyClass> Search(GetExistingHierarchyClassesParameters parameters)
        {
            List<HierarchyClass> existingHierarchyClasses = this.context.HierarchyClass
                .Include(hc => hc.Hierarchy)
                .Include(hc => hc.HierarchyClassTrait)
                .Where(hc => parameters.HierarchyClassNameList.Select(hcn => hcn.ToLower()).Contains(hc.hierarchyClassName.ToLower()))
                .ToList();

            return existingHierarchyClasses;
        }
    }
}
