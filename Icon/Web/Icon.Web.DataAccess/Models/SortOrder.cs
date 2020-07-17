using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Models
{
    /// <summary>
    /// Sort order of a query.
    /// </summary>
    public enum SortOrder
    {
        /// <summary>
        /// The results sorting is Ascending.
        /// </summary>
        Ascending = 1,

        /// <summary>
        /// The results sorting is Descending.
        /// </summary>
        Descending = 2
    }
}
