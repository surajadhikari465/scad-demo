using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;
using System.Linq;
using Icon.DbContextFactory;

namespace Icon.ApiController.DataAccess.Queries
{
    public class GetFinancialHierarchyClassesQuery : IQueryHandler<GetFinancialHierarchyClassesParameters, List<HierarchyClass>>
    {
        private IDbContextFactory<IconContext> iconContextFactory;

        public GetFinancialHierarchyClassesQuery(IDbContextFactory<IconContext> iconContextFactory)
        {
            this.iconContextFactory = iconContextFactory;
        }

        public List<HierarchyClass> Search(GetFinancialHierarchyClassesParameters parameters)
        {
            using (var context = iconContextFactory.CreateContext())
            {
                return context.HierarchyClass.Where(hc => hc.Hierarchy.hierarchyName == HierarchyNames.Financial).ToList();
            }
        }
    }
}
