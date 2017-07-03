using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Icon.Dashboard.Mvc.ViewModels;
using Icon.Dashboard.Mvc.Services;
using System.Configuration;
using Icon.Dashboard.Mvc.Helpers;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.Filters;

namespace Icon.Dashboard.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private HttpServerUtilityBase _serverUtility;

        public HomeController() : this(null, null, null, null)
        {
        }

        public HomeController(string dataFile = null,
            IDataFileServiceWrapper dataServiceWrapper = null,
            IIconDatabaseServiceWrapper loggingServiceWrapper = null,
            HttpServerUtilityBase serverUtility = null)
        {
            XmlDataFile = dataFile ?? Utils.DataFileName;
            DashboardDataFileService = dataServiceWrapper ?? new DataFileServiceWrapper();
            IconDatabaseService = loggingServiceWrapper ?? new IconDatabaseServiceWrapper();

            _serverUtility = serverUtility;
        }

        public IDataFileServiceWrapper DashboardDataFileService { get; private set; }
        public IIconDatabaseServiceWrapper IconDatabaseService { get; private set; }
        public HttpServerUtilityBase ServerUtility
        {
            get
            {
                return _serverUtility ?? Server;
            }
        }

        public string XmlDataFile { get; set; }

        #region GET
        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.ReadOnly)]
        public ActionResult Index()
        {
            HttpContext.Items["loggingDataService"] = IconDatabaseService;
            var viewModels = DashboardDataFileService.GetApplicationListViewModels(ServerUtility, XmlDataFile);
            return View(viewModels);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.ReadOnly)]
        public ActionResult Details(string application, string server)
        {
            HttpContext.Items["loggingDataService"] = IconDatabaseService;
            var viewModel = DashboardDataFileService.GetApplicationViewModel(ServerUtility, XmlDataFile, application, server);
            return View(viewModel);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.EditingPrivileges)]
        public ActionResult Edit(string application, string server)
        {
            HttpContext.Items["loggingDataService"] = IconDatabaseService;
            var viewModel = DashboardDataFileService.GetApplicationViewModel(ServerUtility, XmlDataFile, application, server);
            return View(viewModel);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.ReadOnly)]
        public ActionResult Configure(string application, string server)
        {
            HttpContext.Items["loggingDataService"] = IconDatabaseService;
            var viewModel = DashboardDataFileService.GetApplicationViewModel(ServerUtility, XmlDataFile, application, server);
            return View(viewModel);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.ReadOnly)]
        public ActionResult Create()
        {
            HttpContext.Items["loggingDataService"] = IconDatabaseService;
            var viewModel = new IconApplicationViewModel();
            return View(viewModel);
        }

        [HttpGet]
        [ChildActionOnly]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.ReadOnly)]
        public ActionResult IconApiServicePartial(string application, string server)
        {
            var task = DashboardDataFileService.GetServiceViewModel(ServerUtility, XmlDataFile, application, server);
            return PartialView("_IconApiServicePartial", task);
        }

        [HttpGet]
        [ChildActionOnly]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.ReadOnly)]
        public ActionResult TaskPartial(string application, string server)
        {
            var task = DashboardDataFileService.GetTaskViewModel(ServerUtility, XmlDataFile, application, server);
            return PartialView("_TaskViewModelPartial", task);
        }
        #endregion

        #region POST
        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.EditingPrivileges)]
        public ActionResult Index(string application, string server, string command)
        {
            DashboardDataFileService.ExecuteCommand(ServerUtility, XmlDataFile, application, server, command);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.EditingPrivileges)]
        public ActionResult Details(string application, string server, string command)
        {
            DashboardDataFileService.ExecuteCommand(ServerUtility, XmlDataFile, application, server, command);
            return RedirectToAction("Details", "Home", new { application = application, server = server });
        }

        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.EditingPrivileges)]
        public ActionResult Edit(IconApplicationViewModel appViewModel)
        {
            DashboardDataFileService.UpdateApplication(ServerUtility, appViewModel, XmlDataFile);
            return RedirectToAction("Details", "Home", new { application = appViewModel.Name, server = appViewModel.Server });
        }

        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.EditingPrivileges)]
        public ActionResult Configure(IconApplicationViewModel appViewModel)
        {
            ViewBag.EnvironmentOptions = new EnvironmentSwitcher().GetServersForEnvironments();
            DashboardDataFileService.SaveAppSettings(appViewModel);
            return RedirectToAction("Details", "Home", new { application = appViewModel.Name, server = appViewModel.Server });
        }

        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.EditingPrivileges)]
        public ActionResult Create(IconApplicationViewModel appViewModel)
        {
            DashboardDataFileService.AddApplication(ServerUtility, appViewModel, XmlDataFile);
            return RedirectToAction("Details", "Home", new { application = appViewModel.Name, server = appViewModel.Server });
        }

        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.EditingPrivileges)]
        public ActionResult Delete(string application, string server)
        {
            var app = DashboardDataFileService.GetApplication(ServerUtility, XmlDataFile, application, server);
            DashboardDataFileService.DeleteApplication(ServerUtility, app, XmlDataFile);
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}