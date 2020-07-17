using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Icon.Web.Mvc.Models
{
    /// <summary>
    /// Response of a DataTable process in server request.
    /// </summary>
    /// <typeparam name="T">Type for the rows.</typeparam>
    public class DataTableResponse<T>
    {
        /// <summary>
        /// Get's or sets if the page is to be drawn.
        /// </summary>
        public int draw { get; set; }

        /// <summary>
        /// Gets or sets the total number of records.
        /// </summary>
        public int recordsTotal { get; set; }

        /// <summary>
        /// Gets or sets the total number of records after filters applied.
        /// </summary>
        public int recordsFiltered { get; set; }

        /// <summary>
        /// Page of data.
        /// </summary>
        public List<T> data { get; set; }
    }
}
