using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Icon.ApiController.DataAccess.Queries
{
    public class GetFinancialHierarchyClassesQuery : IQueryHandler<GetFinancialHierarchyClassesParameters, List<HierarchyClass>>
    {
        private IRenewableContext<IconContext> globalContext;

        public GetFinancialHierarchyClassesQuery(IRenewableContext<IconContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public List<HierarchyClass> Search(GetFinancialHierarchyClassesParameters parameters)
        {
            return globalContext.Context.HierarchyClass.Where(hc => hc.Hierarchy.hierarchyName == HierarchyNames.Financial).ToList();
        }
    }
}
