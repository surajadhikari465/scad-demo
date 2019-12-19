using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.Framework;
using Services.NewItem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.SqlClient;

namespace Services.NewItem.Queries
{
    public class GetSubTeamsDictionaryQueryHandler : IQueryHandler<GetSubTeamsDictionaryQuery, Dictionary<string, SubTeamModel>>
    {
        private IRenewableContext<IconContext> context;

        public GetSubTeamsDictionaryQueryHandler(IRenewableContext<IconContext> context)
        {
            this.context = context;
        }

        public Dictionary<string, SubTeamModel> Search(GetSubTeamsDictionaryQuery parameters)
        {
            return context.Context.HierarchyClass
                .Include(hc => hc.HierarchyClassTrait)
                .Where(hc => hc.hierarchyID == Hierarchies.Financial
                    && hc.HierarchyClassTrait.Any(hct => hct.traitID == Traits.PosDepartmentNumber)
                    && hc.HierarchyClassTrait.Any(hct => hct.traitID == Traits.FinancialHierarchyCode))
                .ToDictionary(
                    hc => hc.HierarchyClassTrait.FirstOrDefault(hct => hct.traitID == Traits.PosDepartmentNumber).traitValue,
                    hc => new SubTeamModel
                    {
                        HierarchyClassId = hc.hierarchyClassID,
                        HierarchyClassName = hc.hierarchyClassName,
                        FinancialHierarchyCode = hc.HierarchyClassTrait.FirstOrDefault(hct => hct.traitID == Traits.FinancialHierarchyCode).traitValue
                    });
        }
    }
}
