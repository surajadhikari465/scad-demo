using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    /// <summary>
    /// Get Item Group Association Search Item Partial Parameters.
    /// </summary>
    public class GetItemGroupAssociationSearchItemPartialParameters : IQuery<IEnumerable<ItemGroupAssociationItemModel>>
    {
        /// <summary>
        /// ItemGroup Type (Sku/Priceline)
        /// </summary>
        public ItemGroupTypeId ItemGroupTypeId { get; set; }

        /// <summary>
        /// Scan code prefix of the items to search.
        /// </summary>
        public string ScanCodePrefix { get; set; }
        
        /// <summary>
        /// Max number of results
        /// </summary>
        public int MaxResultSize { get; set; }

        /// <summary>
        /// Exclude items associated witht this item group.
        /// </summary>
        public int ExcludeItemGroupId { get; set; }
    }
}
