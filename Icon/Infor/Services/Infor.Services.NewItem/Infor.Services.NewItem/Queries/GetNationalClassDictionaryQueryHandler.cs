using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.Framework;
using Infor.Services.NewItem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Infor.Services.NewItem.Queries
{
    public class GetNationalClassDictionaryQueryHandler : IQueryHandler<GetNationalClassDictionaryQuery, Dictionary<string, NationalClassModel>>
    {
        private IRenewableContext<IconContext> context;

        public GetNationalClassDictionaryQueryHandler(IRenewableContext<IconContext> context)
        {
            this.context = context;
        }
        
        public Dictionary<string, NationalClassModel> Search(GetNationalClassDictionaryQuery parameters)
        {
            return context.Context.HierarchyClass
                .Include(hc => hc.HierarchyClassTrait)
                .Where(hc => hc.hierarchyID == Hierarchies.National && hc.hierarchyLevel.Value == HierarchyLevels.NationalClass)
                .ToDictionary(
                    hc => hc.HierarchyClassTrait.FirstOrDefault(hct => hct.traitID == Traits.NationalClassCode).traitValue,
                    hc => new NationalClassModel
                    {
                        HierarchyClassId = hc.hierarchyClassID,
                        Name = hc.hierarchyClassName,
                        HierarchyParentClassId = hc.hierarchyParentClassID.Value
                    });
        }
    }
}
