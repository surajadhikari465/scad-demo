using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Infor.Listeners.HierarchyClass.Queries
{
    public class GetTaxIdFromTaxCodeQueryHandler : IQueryHandler<GetTaxIdFromTaxCodeParameters, Dictionary<string, int>>
    {
        IRenewableContext<IconContext> context;

        public GetTaxIdFromTaxCodeQueryHandler(IRenewableContext<IconContext> context)
        {
            this.context = context;
        }

        public Dictionary<string, int> Search(GetTaxIdFromTaxCodeParameters parameters)
        {
            return context.Context.HierarchyClass.Where(hc => hc.hierarchyID == Hierarchies.Tax)
                .ToDictionary(
                    hc => hc.hierarchyClassName.Substring(0, 7), 
                    hc => hc.hierarchyClassID);
        }
    }
}
