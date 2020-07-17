using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.Models
{
    /// <summary>
    /// DataTable Column information.
    /// Note: properties are not in pascal case due to json mapping.
    /// </summary>
    public class DataTableColumn
    {
        /// <summary>
        /// Gets or sets the Name of the data field.
        /// </summary>
        public string data { get; set; }

        /// <summary>
        /// Gets or sets the Name/Title of the column.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Gets or sets if the column is searchable .
        /// </summary>
        public bool searchable { get; set; }

        /// <summary>
        /// Gets or sets if the column can be used to sort.
        /// </summary>
        public bool orderable { get; set; }

        /// <summary>
        /// Gets or sets the column search data.
        /// </summary>
        public DataTableSearch search { get; set; }
    }
}