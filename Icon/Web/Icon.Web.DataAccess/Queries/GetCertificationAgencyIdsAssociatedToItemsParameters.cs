using Icon.Common.DataAccess;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetCertificationAgencyIdsAssociatedToItemsParameters : IQuery<List<int>>
    {
        public List<int> HierarchyClassIds { get; set; }
        public int TraitId { get; set; }
    }
}
