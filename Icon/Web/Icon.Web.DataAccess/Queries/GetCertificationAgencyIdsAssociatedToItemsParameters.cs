using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Queries
{
    public class GetCertificationAgencyIdsAssociatedToItemsParameters : IQuery<List<int>>
    {
        public List<int> HierarchyClassIds { get; set; }
        public int TraitId { get; set; }
    }
}
