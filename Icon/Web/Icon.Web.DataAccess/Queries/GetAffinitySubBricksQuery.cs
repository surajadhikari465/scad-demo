using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Queries
{
    public class GetAffinitySubBricksQuery : IQueryHandler<GetAffinitySubBricksParameters, List<HierarchyClass>>
    {
        private IconContext context;

        public GetAffinitySubBricksQuery(IconContext context)
        {
            this.context = context;
        }

        public List<HierarchyClass> Search(GetAffinitySubBricksParameters parameters)
        {
            return context.HierarchyClass
                .Where(hc => hc.hierarchyID == Hierarchies.Merchandise 
                    && hc.hierarchyLevel == HierarchyLevels.SubBrick 
                    && hc.HierarchyClassTrait.Any(hct => hct.traitID == Traits.Affinity))
                .ToList();
        }
    }
}
