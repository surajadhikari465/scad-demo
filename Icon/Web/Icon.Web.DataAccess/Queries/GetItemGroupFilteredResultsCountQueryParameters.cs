using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Queries
{
    /// <summary>
    /// GetItemGroupFilteredResultsCountQuery Parameters
    /// </summary>
    public class GetItemGroupFilteredResultsCountQueryParameters : IQuery<int>
    {
        /// <summary>
        /// ItemGroupTypeId
        /// </summary>
        public ItemGroupTypeId ItemGroupTypeId { get; set; }

        /// <summary>
        /// Search term
        /// </summary>
        public string SearchTerm { get; set; }
    }
}
