using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.Models
{
    /// <summary>
    /// Model for DataTable Ajax Post for Server processing.
    /// Note: properties are not in pascal case due to json mapping.
    /// </summary>
    public class DataTableAjaxPostModel
    {
        /// <summary>
        /// Gets or sets the draw status
        /// </summary>
        public int draw { get; set; }

        /// <summary>
        /// Gets or sets on which record the page starts.
        /// </summary>
        public int start { get; set; }

        /// <summary>
        /// Gets or sets on which the length of the page.
        /// </summary>
        public int length { get; set; }

        /// <summary>
        /// Gets or sets the list of columns in the datatable.
        /// </summary>
        public List<DataTableColumn> columns { get; set; }

        /// <summary>
        /// Gets or sets the term to search.
        /// </summary>
        public DataTableSearch search { get; set; }

        /// <summary>
        /// Gets or sets in which order the data should be presented.
        /// </summary>
        public List<DataTableOrder> order { get; set; }
    }
}