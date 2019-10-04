using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.Mvc.Enums;
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
        public ApiJobsController() : this(null, null, null, null) { }

        public ApiJobsController(
            IDashboardAuthorizer dashboardAuthorizer = null,
            IDashboardDataManager dashboardConfigManager = null,
            IIconDatabaseServiceWrapper iconDbService = null,
            IMammothDatabaseServiceWrapper mammothDbService = null)
            : base(dashboardAuthorizer, dashboardConfigManager, iconDbService, mammothDbService) { }

        [HttpGet]
        [DashboardAuthorizer(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Index(string jobType = null, int page = 1, int pageSize = PagingConstants.DefaultPageSize)
        {
            var jobSummaries = GetJobSummariesAndSetRelatedViewData(page, pageSize, jobType);
            //TODO clean up
            ViewBag.Title = jobSummaries.ViewTitle;
            ViewBag.PaginationPageSetViewModel = jobSummaries.PaginationModel;

            ViewBag.GlobalViewData = base.BuildGlobalViewModel();
            return View(jobSummaries);
        }

        [HttpGet]
        [DashboardAuthorizer(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Pending()
        {
            var pendingMessages = IconDatabaseService.GetPendingMessages();

            ViewBag.GlobalViewData = base.BuildGlobalViewModel();
            return View(pendingMessages);
        }

        [HttpGet]
        [DashboardAuthorizer(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult RedrawPaging(string jobType = null, int page = 1, int pageSize = PagingConstants.DefaultPageSize)
        {
            var pagingData = GetPaginationViewModel(page, pageSize, jobType);
            //ViewBag.GlobalViewData = base.BuildGlobalViewModel(base.UserPrincipal);
            return PartialView("_PaginationPartial", pagingData);
        }

        [HttpGet]
        [DashboardAuthorizer(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
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
                    viewModel = IconDatabaseService.GetApiJobSummaryReport(
                        viewModel.MessageType, viewModel.StartTime.Value, viewModel.EndTime.Value);
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null) ex = ex.InnerException;
                    viewModel.Errors = "Server " + ex.Message;
                }
            }
            else
            {
                var modelErrors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();
                viewModel.Errors = string.Join(System.Environment.NewLine, modelErrors);
            }
            //ViewBag.GlobalViewData = base.BuildGlobalViewModel(base.UserPrincipal);
            return PartialView("_ApiMessageJobTimedReportResultPartial", viewModel);
        }

        [HttpGet]
        [DashboardAuthorizer(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult TableRefresh(string jobType = null, int page = 1, int pageSize = PagingConstants.DefaultPageSize, string errorLevel = "Any")
        {
            var jobSummaries = GetJobSummariesAndSetRelatedViewData(page, pageSize, jobType);

            //ViewBag.GlobalViewData = base.BuildGlobalViewModel(base.UserPrincipal);
            return PartialView("_ApiJobsReportPartial", jobSummaries);
        }

        protected PaginationPageSetViewModel GetPaginationViewModel(int page, int pageSize, string jobType = null)
        {
            var pagingViewModel = String.IsNullOrWhiteSpace(jobType)
               ? new PaginationPageSetViewModel(Constants.MvcNames.ApiJobsTableRefreshActionName, Constants.MvcNames.ApiJobControllerName, page, pageSize)
               : new PaginationPageSetViewModel(Constants.MvcNames.ApiJobsTableRefreshActionName, Constants.MvcNames.ApiJobControllerName, page, pageSize, jobType);
            return pagingViewModel;
        }

        private ApiMessageJobReportViewModel GetJobSummariesAndSetRelatedViewData(
            int page, int pageSize, string jobType = null)
        {
            var viewModel = new ApiMessageJobReportViewModel(jobType);

            //var jobSummaries = new List<ApiMessageJobSummaryViewModel>();
            if (String.IsNullOrWhiteSpace(jobType))
            {
                viewModel.JobSummaries = IconDatabaseService.GetPagedApiJobSummaries(page, pageSize);
                viewModel.ViewTitle = $"{DashboardDataService.ActiveEnvironment.Name} DB API Controller Message Jobs Summary";
                viewModel.PaginationModel = GetPaginationViewModel(page, pageSize);
            }
            else
            {
                viewModel.JobSummaries = IconDatabaseService.GetPagedApiJobSummariesByMessageType(jobType, page, pageSize);
                viewModel.JobType = jobType;
                viewModel.ViewTitle = $"{DashboardDataService.ActiveEnvironment.Name} DB API Controller {jobType} Message Jobs";
                viewModel.PaginationModel = GetPaginationViewModel(page, pageSize, jobType);
            }

            return viewModel;
        }
    }
}