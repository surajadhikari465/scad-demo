using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetDuplicateBrandsByTrimmedNameQuery : IQueryHandler<GetDuplicateBrandsByTrimmedNameParameters, List<string>>
    {
        private readonly IconContext context;

        public GetDuplicateBrandsByTrimmedNameQuery(IconContext context)
        {
            this.context = context;
        }

        public List<string> Search(GetDuplicateBrandsByTrimmedNameParameters parameters)
        {
            var duplicateBrandNames = parameters.LongBrandNameList.Where(lb => this.context.HierarchyClass.Where(hc => hc.hierarchyID == Hierarchies.Brands)
                .Select(hc => hc.hierarchyClassName).Any(hcn => !hcn.Equals(lb.Key, StringComparison.InvariantCultureIgnoreCase) && hcn.Substring(0, Constants.IrmaBrandNameMaxLength).Equals(lb.Value, StringComparison.InvariantCultureIgnoreCase)))
                .Select(lb => lb.Key)
                .ToList();


            return duplicateBrandNames == null ? new List<string>() : duplicateBrandNames;
        }
    }
}
