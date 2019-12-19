using Icon.Common.DataAccess;
using Icon.Framework;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetHierarchyClassByNameQuery : IQueryHandler<GetHierarchyClassByNameParameters, HierarchyClass>
    {
        private IconContext context;

        public GetHierarchyClassByNameQuery(IconContext context)
        {
            this.context = context;
        }

        public HierarchyClass Search(GetHierarchyClassByNameParameters parameters)
        {
            var hierarchyQuery = context.HierarchyClass
                .Where(hc => hc.hierarchyClassName == parameters.HierarchyClassName && hc.Hierarchy.hierarchyName == parameters.HierarchyName && hc.hierarchyLevel == parameters.HierarchyLevel);
            
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
