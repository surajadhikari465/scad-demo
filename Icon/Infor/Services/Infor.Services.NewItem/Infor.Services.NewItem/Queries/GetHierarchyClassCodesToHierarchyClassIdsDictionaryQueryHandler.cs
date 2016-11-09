using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Icon.Common.Context;
using Infor.Services.NewItem.Infrastructure;

namespace Infor.Services.NewItem.Queries
{
    public class GetHierarchyClassCodesToHierarchyClassIdsDictionaryQueryHandler : IQueryHandler<GetHierarchyClassCodesToHierarchyClassIdsDictionaryQuery, Dictionary<string, int>>
    {
        private IRenewableContext<IconContext> context;

        public GetHierarchyClassCodesToHierarchyClassIdsDictionaryQueryHandler(IRenewableContext<IconContext> context)
        {
            this.context = context;
        }
        
        public Dictionary<string, int> Search(GetHierarchyClassCodesToHierarchyClassIdsDictionaryQuery parameters)
        {
            if(parameters.HierarchyId == Hierarchies.Tax)
            {
                return context.Context.HierarchyClass
                    .Where(hc => hc.hierarchyID == parameters.HierarchyId)
                    .ToDictionary(hc => hc.hierarchyClassName.Substring(0, 7), hc => hc.hierarchyClassID);
            }
            else if(parameters.HierarchyId == Hierarchies.National)
            {
                return context.Context.HierarchyClass
                    .Include(hc => hc.HierarchyClassTrait)
                    .Where(hc => hc.hierarchyID == parameters.HierarchyId
                        && hc.hierarchyLevel == HierarchyLevels.NationalClass
                        && hc.HierarchyClassTrait.Any(hct => hct.traitID == Traits.NationalClassCode))
                    .ToDictionary(hc => hc.HierarchyClassTrait.FirstOrDefault(hct => hct.traitID == Traits.NationalClassCode).traitValue, hc => hc.hierarchyClassID);
            }
            else
            {
                throw new ArgumentException(string.Format("No Hierarchy defined with ID {0}", parameters.HierarchyId));
            }
        }
    }
}
