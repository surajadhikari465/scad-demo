using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    /// <summary>
    /// Get Item Group Members Parameters.
    /// </summary>
    public class GetItemGroupMembersParameters: IQuery<IEnumerable<ItemGroupMember>>
    {
        /// <summary>
        /// Target ItemGroupId.
        /// </summary>
        public int ItemGroupId { get; set; }
    }
}
