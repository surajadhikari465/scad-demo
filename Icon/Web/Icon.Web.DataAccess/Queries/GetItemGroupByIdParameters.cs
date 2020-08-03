using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
    /// <summary>
    /// GetItemGroupQuery Parameters.
    /// </summary>
    public class GetItemGroupByIdParameters : IQuery<ItemGroupModel>
    {
        /// <summary>
        /// Gets or sets Item Group Id to query.
        /// </summary>
        public int ItemGroupId { get; set; }
    }
}
