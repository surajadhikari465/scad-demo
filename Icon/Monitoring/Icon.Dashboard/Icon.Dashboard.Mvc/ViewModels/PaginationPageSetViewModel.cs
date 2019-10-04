using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Models;
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
            : this(null, null) { }

        public PaginationPageSetViewModel(
            string actionName,
            string controllerName)
        {
            ActionName = actionName;
            ControllerName = controllerName;
            NumberOfPagesInQuickLinks = DefaultNumberOfPagesInQuickLinks;
            QuickLinks = new List<PaginationPageViewModel>(NumberOfPagesInQuickLinks);
        }

        public PaginationPageSetViewModel(
            string actionName,
            string controllerName,
            int page,
            int pageSize,
            LogErrorLevelEnum errorLevelEnum = LogErrorLevelEnum.Error) 
            : this(actionName, controllerName, page, pageSize, null, errorLevelEnum)
        {
        }

        public PaginationPageSetViewModel(
            string actionName,
            string controllerName,
            int page,
            int pageSize,
            string id,
            LogErrorLevelEnum errorLevelEnum = LogErrorLevelEnum.Error)  
            : this(actionName, controllerName)
        {
            CurrentPage = page;
            PageSize = pageSize;
            AppName = id;
            ErrorLevel = errorLevelEnum;

            BuildPageLinks();
        }

        public void BuildPageLinks()
        {
            if (CurrentPage > 1)
            {
                PreviousPage = new PaginationPageViewModel(CurrentPage - 1, PageSize, AppName, "Newer");
            }

            NextPage = new PaginationPageViewModel(CurrentPage + 1, PageSize, AppName, "Older");

            if (NumberOfPagesInQuickLinks > 0)
            {
                if (QuickLinks == null)
                {
                    QuickLinks = new List<PaginationPageViewModel>(NumberOfPagesInQuickLinks);
                }
                for (int i = QuickLinkStartingPageNumber; i < QuickLinkStartingPageNumber + NumberOfPagesInQuickLinks; i++)
                {
                    QuickLinks.Add(new PaginationPageViewModel(i, PageSize, AppName, i.ToString()));
                }
            }
        }

        public int CurrentPage { get; set; }

        /// <summary>
        /// Number of records per page, used when building the route
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Parameter passed when building the route, specifying the name of the logged application
        /// </summary>
        public string AppName { get; set; }

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
        
        public LogErrorLevelEnum ErrorLevel { get; set; }

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