using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.Mvc.Filters;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.Services;
using Icon.Dashboard.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Icon.Dashboard.Mvc.Controllers
{
    public class ApiJobsController : BaseDashboardController
    {
        public ApiJobsController() : this(null, null) { }

        public ApiJobsController(
            HttpServerUtilityBase serverUtility = null,
            IIconDatabaseServiceWrapper loggingServiceWrapper = null)
            : base (serverUtility, loggingServiceWrapper ) { }

        #region GET

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.ReadOnly)]
        public ActionResult Index(string id = null, int page = 1, int pageSize = PagingConstants.DefaultPageSize)
        {
            //enable filter to use the data service
            HttpContext.Items["loggingDataService"] = IconDatabaseService;
            var jobSummaries = GetJobSummariesAndSetRelatedViewData(id, page, pageSize);

            return View(jobSummaries);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.ReadOnly)]
        public ActionResult Pending()
        {
            var pendingMessages = IconDatabaseService.GetPendingMessages();
            return View(pendingMessages);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.ReadOnly)]
        public ActionResult RedrawPaging(string routeParameter = null, int page = 1, int pageSize = PagingConstants.DefaultPageSize)
        {
            var pagingData = GetPaginationViewModel(routeParameter, page, pageSize);
            return PartialView("_PaginationPartial", pagingData);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.ReadOnly)]
        public ActionResult Summarize(ApiMessageJobTimedReportViewModel viewModel)
        {
            if (!viewModel.StartTime.HasValue)
            {
                ModelState.AddModelError(nameof(viewModel.StartTime), "Start time for query must be set");
            }
            if (!viewModel.EndTime.HasValue)
            {
                ModelState.AddModelError(nameof(viewModel.EndTime), "Start time for query must be set");
            }

            if (ModelState.IsValid)
            {
                ModelState.Clear();
                try
                {
                    viewModel = IconDatabaseService.GetApiJobSummaryReport(viewModel.MessageType, viewModel.StartTime.Value, viewModel.EndTime.Value);
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null) ex = ex.InnerException;
                    viewModel.Errors = "Server " + ex.Message;
                }
            }
            else
            {
                var modelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                viewModel.Errors = string.Join(System.Environment.NewLine, modelErrors);
            }
            return PartialView("_ApiMessageJobTimedReportResultPartial", viewModel);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.ReadOnly)]
        public ActionResult TableRefresh(string routeParameter = null, int page = 1, int pageSize = PagingConstants.DefaultPageSize, string errorLevel = "Any")
        {
            var jobSummaries = GetJobSummariesAndSetRelatedViewData(routeParameter, page, pageSize);
            return PartialView("_ApiJobsTablePartial", jobSummaries);
        }
        #endregion

        protected PaginationPageSetViewModel GetPaginationViewModel(int page, int pageSize)
        {
            var pagingData = new PaginationPageSetViewModel("TableRefresh", "ApiJobs", PagingConstants.NumberOfQuickLinks, page, pageSize);
            return pagingData;
        }

        protected PaginationPageSetViewModel GetPaginationViewModel(string jobType, int page, int pageSize)
        {
            var pagingData = String.IsNullOrWhiteSpace(jobType)
               ? GetPaginationViewModel(page, pageSize)
               : new PaginationPageSetViewModel("TableRefresh", "ApiJobs", PagingConstants.NumberOfQuickLinks, page, pageSize, jobType);
            return pagingData;
        }

        private IEnumerable<ApiMessageJobSummaryViewModel> GetJobSummariesAndSetRelatedViewData(string jobType, int page, int pageSize)
        {
            if (String.IsNullOrWhiteSpace(jobType))
            {
                return GetJobSummariesAndSetRelatedViewData(page, pageSize);
            }
            List<ApiMessageJobSummaryViewModel> jobSummaries = IconDatabaseService.GetPagedApiJobSummariesByMessageType(jobType, page, pageSize);

            ViewBag.JobType = jobType;
            ViewBag.Title = String.Format("API Controller {0} Message Jobs", jobType);
            ViewBag.PaginationPageSetViewModel = GetPaginationViewModel(jobType, page, pageSize);

            return jobSummaries;
        }

        private IEnumerable<ApiMessageJobSummaryViewModel> GetJobSummariesAndSetRelatedViewData(int page, int pageSize)
        {
            List<ApiMessageJobSummaryViewModel> jobSummaries = IconDatabaseService.GetPagedApiJobSummaries(page, pageSize);
            
            ViewBag.Title = "API Controller Message Jobs Summary";
            ViewBag.PaginationPageSetViewModel = GetPaginationViewModel(page, pageSize);

            return jobSummaries;
        }
    }
}