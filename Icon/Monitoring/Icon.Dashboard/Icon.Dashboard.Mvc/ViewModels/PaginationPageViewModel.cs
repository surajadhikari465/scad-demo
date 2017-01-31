using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.ViewModels
{
    /// <summary>
    /// View Model to use when rendering a paging link
    /// </summary>
    public class PaginationPageViewModel
    {
        public PaginationPageViewModel()
        {
            TextForLink = PageNumber.ToString();
        }

        public PaginationPageViewModel( int page, int pageSize) : this()
        {
            PageNumber = page;
            PageSize = pageSize;
        }

        public PaginationPageViewModel(int page, int pageSize, string id, string textForLink)
            : this(page, pageSize)
        {
            RouteParameter = id;
            TextForLink = textForLink;
        }

        /// <summary>
        /// Current page number for this page, used when building the route
        /// </summary>
        public int PageNumber { get; }        

        /// <summary>
        /// Number of records per page, used when building the route
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Parameter passed when building the route, such as id or type
        /// </summary>
        public string RouteParameter { get; set; }

        /// <summary>
        /// Text to display for the link
        /// </summary>
        public string TextForLink { get; set; }
    }

}