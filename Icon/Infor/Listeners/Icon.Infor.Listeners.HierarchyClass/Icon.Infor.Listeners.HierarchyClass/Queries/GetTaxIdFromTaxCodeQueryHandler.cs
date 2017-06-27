using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.DbContextFactory;
using Icon.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Infor.Listeners.HierarchyClass.Queries
{
    public class GetTaxIdFromTaxCodeQueryHandler : IQueryHandler<GetTaxIdFromTaxCodeParameters, Dictionary<string, int>>
    {
        private IDbContextFactory<IconContext> contextFactory;

        public GetTaxIdFromTaxCodeQueryHandler(IDbContextFactory<IconContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public Dictionary<string, int> Search(GetTaxIdFromTaxCodeParameters parameters)
        {
            using (var context = contextFactory.CreateContext())
            {
                return context.HierarchyClass.Where(hc => hc.hierarchyID == Hierarchies.Tax)
                    .ToDictionary(
                        hc => hc.hierarchyClassName.Substring(0, 7),
                        hc => hc.hierarchyClassID);
            }
        }
    }
}
