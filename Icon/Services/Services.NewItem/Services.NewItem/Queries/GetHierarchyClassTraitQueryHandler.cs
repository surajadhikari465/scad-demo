using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Icon.Common.Context;
using Services.NewItem.Infrastructure;

namespace Services.NewItem.Queries
{
    public class GetHierarchyClassTraitQueryHandler : IQueryHandler<GetHierarchyClassTraitQuery, Dictionary<int, string>>
    {
        private IRenewableContext<IconContext> context;

        public GetHierarchyClassTraitQueryHandler(IRenewableContext<IconContext> context)
        {
            this.context = context;
        }
        
        public Dictionary<int, string> Search(GetHierarchyClassTraitQuery parameters)
        {
            Dictionary<int, string> hierarchyClassTraitDictionary = this.context.Context.HierarchyClassTrait
                .Where(hct => hct.traitID == parameters.TraitId)
                .ToDictionary(hct => hct.hierarchyClassID, hct => hct.traitValue);

            return hierarchyClassTraitDictionary;
        }
    }
}
