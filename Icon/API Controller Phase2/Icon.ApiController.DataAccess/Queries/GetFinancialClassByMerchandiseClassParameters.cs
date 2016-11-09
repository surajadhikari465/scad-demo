
using Icon.Common.DataAccess;
using Icon.Framework;

namespace Icon.ApiController.DataAccess.Queries
{
    public class GetFinancialClassByMerchandiseClassParameters : IQuery<HierarchyClass>
    {
        public HierarchyClass MerchandiseHierarchyClass { get; set; }
    }
}
