using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    /// <summary>
    /// GetItemGroupQuery Parameters.
    /// </summary>
    public class GetItemGroupParameters : IQuery<List<ItemGroupModel>>
    {
        /// <summary>
        /// Gets or sets Item Group Type Id to query.
        /// </summary>
        public ItemGroupTypeId ItemGroupTypeId { get; set; }

        /// <summary>
        /// Gets or sets the sort column.
        /// </summary>
        public ItemGroupColumns SortColumn { get; set;}

        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        public SortOrder SortOrder { get; set; }

        /// <summary>
        /// Rows Offset (How many rows to skip fromt he result).
        /// </summary>
        public int RowsOffset { get; set; }

        /// <summary>
        /// Page size.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Search term to filter the result by. Null or empty for none.
        /// </summary>
        public string SearchTerm { get; set; }
    }
}
