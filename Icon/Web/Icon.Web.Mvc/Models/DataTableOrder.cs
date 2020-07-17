using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.Models
{
    /// <summary>
    /// DataTable Ordering information.
    /// Note: properties are not in pascal case due to json mapping.
    /// </summary>
    public class DataTableOrder
    {
        /// <summary>
        /// Index of the column used for ordering.
        /// </summary>
        public int column { get; set; }

        /// <summary>
        /// Direction of the sort (Asc/ desc).
        /// </summary>
        public string dir { get; set; }
    }
}