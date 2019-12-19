using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.Framework;
using Services.NewItem.Extensions;
using Services.NewItem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Services.NewItem.Queries
{
    public class GetTaxClassDictionaryQueryHandler : IQueryHandler<GetTaxClassDictionaryQuery, Dictionary<string, TaxClassModel>>
    {
        private IRenewableContext<IconContext> context;

        public GetTaxClassDictionaryQueryHandler(IRenewableContext<IconContext> context)
        {
            this.context = context;
        }
        
        public Dictionary<string, TaxClassModel> Search(GetTaxClassDictionaryQuery parameters)
        {
            return context.Context.HierarchyClass
                .Include(hc => hc.HierarchyClassTrait)
                .Where(hc => hc.hierarchyID == Hierarchies.Tax)
                .ToDictionary(
                    hc => hc.hierarchyClassName.ToTaxCode(),
                    hc => new TaxClassModel
                    {
                        HierarchyClassId = hc.hierarchyClassID,
                        Name = hc.hierarchyClassName,
                        TaxCode = hc.hierarchyClassName.ToTaxCode()
                    });
        }
    }
}
