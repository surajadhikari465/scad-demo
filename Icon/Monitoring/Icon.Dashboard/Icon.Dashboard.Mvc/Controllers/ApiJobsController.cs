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
        public ApiJobsController() : this(null, null, null) { }

        public ApiJobsController(
            IDashboardEnvironmentManager environmentManager = null,
            IIconDatabaseServiceWrapper iconDbService = null,
            IMammothDatabaseServiceWrapper mammothDbService = null)
            : base(environmentManager, iconDbService, mammothDbService) { }

        #region GET

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Index(string id = null, int page = 1, int pageSize = PagingConstants.DefaultPageSize)
        {
            //enable filter to use the data service
            HttpContext.Items["iconLoggingDataService"] = IconDatabaseService;
            HttpContext.Items["mammothLoggingDataService"] = MammothDatabaseService;

            var currentEnvironment = EnvironmentManager.GetEnvironment(Request.Url.Host);
            ViewBag.Environment = currentEnvironment.Name;

            var jobSummaries = GetJobSummariesAndSetRelatedViewData(currentEnvironment.Name, id, page, pageSize);

            return View(jobSummaries);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Pending()
        {
            HttpContext.Items["iconLoggingDataService"] = IconDatabaseService;
            HttpContext.Items["mammothLoggingDataService"] = MammothDatabaseService;

            var currentEnvironment = EnvironmentManager.GetEnvironment(Request.Url.Host);
            ViewBag.Environment = currentEnvironment.Name;

            var pendingMessages = IconDatabaseService.GetPendingMessages();
            return View(pendingMessages);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult RedrawPaging(string appName = null, int page = 1, int pageSize = PagingConstants.DefaultPageSize)
        {
            HttpContext.Items["iconLoggingDataService"] = IconDatabaseService;
            HttpContext.Items["mammothLoggingDataService"] = MammothDatabaseService;

            var currentEnvironment = EnvironmentManager.GetEnvironment(Request.Url.Host);
            ViewBag.Environment = currentEnvironment.Name;

            var pagingData = GetPaginationViewModel(appName, page, pageSize);
            return PartialView("_PaginationPartial", pagingData);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Summarize(ApiMessageJobTimedReportViewModel viewModel)
        {
            var currentEnvironment = EnvironmentManager.GetEnvironment(Request.Url.Host);
            ViewBag.Environment = currentEnvironment.Name;

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
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult TableRefresh(string appName = null, int page = 1, int pageSize = PagingConstants.DefaultPageSize, string errorLevel = "Any")
        {
            var currentEnvironment = EnvironmentManager.GetEnvironment(Request.Url.Host);
            ViewBag.Environment = currentEnvironment.Name;

            var jobSummaries = GetJobSummariesAndSetRelatedViewData(currentEnvironment.Name, appName, page, pageSize);
            return PartialView("_ApiJobsTablePartial", jobSummaries);
        }
        #endregion

        protected PaginationPageSetViewModel GetPaginationViewModel(int page, int pageSize)
        {
            var pagingData = new PaginationPageSetViewModel("TableRefresh", "ApiJobs", page, pageSize);
            return pagingData;
        }

        protected PaginationPageSetViewModel GetPaginationViewModel(string jobType, int page, int pageSize)
        {
            var pagingData = String.IsNullOrWhiteSpace(jobType)
               ? GetPaginationViewModel(page, pageSize)
               : new PaginationPageSetViewModel("TableRefresh", "ApiJobs",  page, pageSize, jobType);
            return pagingData;
        }

        private IEnumerable<ApiMessageJobSummaryViewModel> GetJobSummariesAndSetRelatedViewData(string environment, string jobType, int page, int pageSize)
        {
            if (String.IsNullOrWhiteSpace(jobType))
            {
                return GetJobSummariesAndSetRelatedViewData(environment, page, pageSize);
            }
            List<ApiMessageJobSummaryViewModel> jobSummaries = IconDatabaseService.GetPagedApiJobSummariesByMessageType(jobType, page, pageSize);

            ViewBag.JobType = jobType;
            ViewBag.Title = String.Format("{0} DB API Controller {1} Message Jobs", environment, jobType);
            ViewBag.PaginationPageSetViewModel = GetPaginationViewModel(jobType, page, pageSize);

            return jobSummaries;
        }

        private IEnumerable<ApiMessageJobSummaryViewModel> GetJobSummariesAndSetRelatedViewData(string environment, int page, int pageSize)
        {
            List<ApiMessageJobSummaryViewModel> jobSummaries = IconDatabaseService.GetPagedApiJobSummaries(page, pageSize);
            
            ViewBag.Title = environment + " DB API Controller Message Jobs Summary";
            ViewBag.PaginationPageSetViewModel = GetPaginationViewModel(page, pageSize);

            return jobSummaries;
        }
    }
}