using Icon.Dashboard.CommonDatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.ViewModels
{
    /// <summary>
    /// Class to represent a set of pagination page view models,
    ///  used when rendering a view for paging controls
    /// </summary>
    public class PaginationPageSetViewModel
    {
        public const int DefaultNumberOfPagesInQuickLinks = PagingConstants.NumberOfQuickLinks;

        public PaginationPageSetViewModel()
            : this(null, null, DefaultNumberOfPagesInQuickLinks) { }

        public PaginationPageSetViewModel(int numberOfPagesInQuickLinks)
            : this(null, null, numberOfPagesInQuickLinks) { }

        public PaginationPageSetViewModel(string actionName, string controllerName)
            : this(actionName, controllerName, DefaultNumberOfPagesInQuickLinks) { }

        public PaginationPageSetViewModel(string actionName,
            string controllerName,
            int numberOfPagesInQuickLinks)
        {
            ActionName = actionName;
            ControllerName = controllerName;
            NumberOfPagesInQuickLinks = numberOfPagesInQuickLinks;
            QuickLinks = new List<PaginationPageViewModel>(NumberOfPagesInQuickLinks);
        }

        public PaginationPageSetViewModel(string actionName, string controllerName, int numberOfPagesInQuickLinks, int page, int pageSize) 
            : this(actionName, controllerName, numberOfPagesInQuickLinks, page, pageSize, null)
        {
        }

        public PaginationPageSetViewModel(string actionName, string controllerName, int numberOfPagesInQuickLinks, int page, int pageSize, string id) 
            : this(actionName, controllerName, numberOfPagesInQuickLinks)
        {
            CurrentPage = page;
            PageSize = pageSize;
            RouteParameter = id;

            BuildPageLinks();
        }

        public void BuildPageLinks()
        {
            if (CurrentPage > 1)
            {
                PreviousPage = new PaginationPageViewModel(CurrentPage - 1, PageSize, RouteParameter, "Newer");
            }

            NextPage = new PaginationPageViewModel(CurrentPage + 1, PageSize, RouteParameter, "Older");

            if (NumberOfPagesInQuickLinks > 0)
            {
                if (QuickLinks == null)
                {
                    QuickLinks = new List<PaginationPageViewModel>(NumberOfPagesInQuickLinks);
                }
                for (int i = QuickLinkStartingPageNumber; i < QuickLinkStartingPageNumber + NumberOfPagesInQuickLinks; i++)
                {
                    QuickLinks.Add(new PaginationPageViewModel(i, PageSize, RouteParameter, i.ToString()));
                }
            }
        }

        public int CurrentPage { get; set; }

        /// <summary>
        /// Number of records per page, used when building the route
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Parameter passed when building the route, such as id or type
        /// </summary>
        public string RouteParameter { get; set; }

        /// <summary>
        /// Name of MVC Action to use when building link
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Name of MVC Controller to use when building link
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// Number of pages (e.g. 10 to use when builidng quick-jump links for nearby pages
        /// </summary>
        public int NumberOfPagesInQuickLinks { get; set; }

        /// <summary>
        /// Data used to populate a "Previous Page" link
        /// </summary>
        public PaginationPageViewModel PreviousPage { get; set; }

        /// <summary>
        /// Data used to populate a "Next Page" link
        /// </summary>
        public PaginationPageViewModel NextPage { get; set; }

        /// <summary>
        /// Data used to for list of page links used for quickly skipping to pages, typically pages 1-10 for example
        /// </summary>
        public List<PaginationPageViewModel> QuickLinks { get; set; }

        public int QuickLinkStartingPageNumber
        {
            get
            {
                if (NumberOfPagesInQuickLinks > 0)
                {
                    int halfway = (int)NumberOfPagesInQuickLinks / 2;
                    if (CurrentPage > halfway)
                    {
                        return CurrentPage - halfway;
                    }
                }
                return 1;
            }
        }

        public bool HasPreviousPage
        {
            get { return PreviousPage != null; }
        }

        public bool HasNextPage
        {
            get { return NextPage != null; }
        }

        public bool HasQuickLinks
        {
            get { return QuickLinks != null && QuickLinks.Count > 0; }
        }
    }
}