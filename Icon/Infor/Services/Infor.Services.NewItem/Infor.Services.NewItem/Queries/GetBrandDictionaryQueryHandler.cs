using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.Framework;
using Infor.Services.NewItem.Infrastructure;
using Infor.Services.NewItem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Infor.Services.NewItem.Queries
{
    public class GetBrandDictionaryQueryHandler : IQueryHandler<GetBrandDictionaryQuery, Dictionary<int, BrandModel>>
    {
        private IRenewableContext<IconContext> context;

        public GetBrandDictionaryQueryHandler(IRenewableContext<IconContext> context)
        {
            this.context = context;
        }
        
        public Dictionary<int, BrandModel> Search(GetBrandDictionaryQuery parameters)
        {
            return context.Context.HierarchyClass
                .Include(hc => hc.HierarchyClassTrait)
                .Where(hc => hc.hierarchyID == Hierarchies.Brands)
                .ToDictionary(
                    hc => hc.hierarchyClassID,
                    hc => new BrandModel
                    {
                        HierarchyClassId = hc.hierarchyClassID,
                        Name = hc.hierarchyClassName,
                        BrandAbbreviation = hc.HierarchyClassTrait.FirstOrDefault(hct => hct.traitID == Traits.BrandAbbreviation)?.traitValue
                    });
        }
    }
}
