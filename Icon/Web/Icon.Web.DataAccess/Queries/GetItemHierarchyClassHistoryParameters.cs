using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetItemHierarchyClassHistoryParameters : IQuery<ItemHierarchyClassHistoryAllModel>
    {
        public int ItemId { get; set; }

    }
}
