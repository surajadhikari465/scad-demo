using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class HierarchyClassExistsQuery : IQueryHandler<HierarchyClassExistsParameters, bool>
    {
        private IconContext context;

        public HierarchyClassExistsQuery(IconContext context)
        {
            this.context = context;
        }

        public bool Search(HierarchyClassExistsParameters parameters)
        {
            return context.HierarchyClass
                .Any(hc => hc.hierarchyClassName == parameters.HierarchyClassName && hc.Hierarchy.hierarchyName == parameters.HierarchyName);
        }
    }
}
