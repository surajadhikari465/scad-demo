using Icon.Framework;
using System.Collections.Generic;
using System.Linq;
using TlogController.DataAccess.Interfaces;

namespace TlogController.DataAccess.Queries
{
    public class GetBusinessUnitToRegionCodeMappingQueryHandler : IQueryHandler<GetBusinessUnitToRegionCodeMappingQuery, Dictionary<int, string>>
    {
        IconContext context;

        public GetBusinessUnitToRegionCodeMappingQueryHandler(IconContext context)
        {
            this.context = context;
        }

        public Dictionary<int, string> Execute(GetBusinessUnitToRegionCodeMappingQuery parameters)
        {
            var businessUnits = context.BusinessUnitRegionMapping.ToList();

            var mapping = new Dictionary<int, string>();
            foreach (var businessUnit in businessUnits)
            {
                mapping.Add(businessUnit.businessUnit, businessUnit.regionCode);
            }

            return mapping;
        }
    }
}
