using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.Models
{
    /// <summary>
    /// DataTable structure to represent a Search.
    /// Note: properties are not in pascal case due to json mapping
    /// </summary>
    public class DataTableSearch
    {
        /// <summary>
        /// Gets or sets the value to search.
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// Gets or sets the Regex to search.
        /// </summary>
        public string regex { get; set; }
    }
}