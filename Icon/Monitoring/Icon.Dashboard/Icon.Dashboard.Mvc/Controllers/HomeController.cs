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

        public HomeController(string pathToXmlDataFile = null,
            IDataFileServiceWrapper dataServiceWrapper = null,
            IIconDatabaseServiceWrapper loggingServiceWrapper = null,
            HttpServerUtilityBase serverUtility = null)
        {
            DataFileName = pathToXmlDataFile ?? Utils.DataFileName;
            _serverUtility = serverUtility;
            DashboardDataFileService = dataServiceWrapper ?? new DataFileServiceWrapper();
            IconDatabaseService = loggingServiceWrapper ?? new IconDatabaseServiceWrapper();

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

        public string DataFileName { get; set; }

        #region GET
        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.ReadOnly)]
        public ActionResult Index()
        {
            HttpContext.Items["loggingDataService"] = IconDatabaseService;
            var dataFileWebServerPath = Utils.GetPathForDataFile(ServerUtility, DataFileName);
            var viewModels = DashboardDataFileService.GetApplications(dataFileWebServerPath);
            return View(viewModels);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.ReadOnly)]
        public ActionResult Details(string application, string server)
        {
            HttpContext.Items["loggingDataService"] = IconDatabaseService;
            var dataFileWebServerPath = Utils.GetPathForDataFile(ServerUtility, DataFileName);
            var viewModel = DashboardDataFileService.GetApplication(dataFileWebServerPath, application, server);
            return View(viewModel);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.EditingPrivileges)]
        public ActionResult Edit(string application, string server)
        {
            HttpContext.Items["loggingDataService"] = IconDatabaseService;
            var dataFileWebServerPath = Utils.GetPathForDataFile(ServerUtility, DataFileName);
            var viewModel = DashboardDataFileService.GetApplication(dataFileWebServerPath, application, server);
            return View(viewModel);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.ReadOnly)]
        public ActionResult Configure(string application, string server)
        {
            HttpContext.Items["loggingDataService"] = IconDatabaseService;
            var dataFileWebServerPath = Utils.GetPathForDataFile(ServerUtility, DataFileName);
            var viewModel = DashboardDataFileService.GetApplication(dataFileWebServerPath, application, server);
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
        #endregion

        #region POST
        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.EditingPrivileges)]
        public ActionResult Index(string application, string server, string command)
        {
            DashboardDataFileService.ExecuteServiceCommand(DataFileName, application, server, command);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.EditingPrivileges)]
        public ActionResult Details(string application, string server, string command)
        {
            var dataFileWebServerPath = Utils.GetPathForDataFile(ServerUtility, DataFileName);
            DashboardDataFileService.ExecuteServiceCommand( dataFileWebServerPath, application, server, command);
            return RedirectToAction("Details", "Home", new { application = application, server = server });
        }

        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.EditingPrivileges)]
        public ActionResult Edit(IconApplicationViewModel appViewModel)
        {
            DashboardDataFileService.UpdateApplication(DataFileName, appViewModel);
            return RedirectToAction("Details", "Home", new { application = appViewModel.Name, server = appViewModel.Server });
        }

        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.EditingPrivileges)]
        public ActionResult Configure(IconApplicationViewModel appViewModel)
        {
            ViewBag.EnvironmentOptions = new EnvironmentSwitcher().GetServersForEnvironments();
            DashboardDataFileService.SaveAppSettings(appViewModel);
            return RedirectToAction("Details", "Home", new
            {
                application = appViewModel.Name,
                server = appViewModel.Server
            });
        }

        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.EditingPrivileges)]
        public ActionResult Create(IconApplicationViewModel appViewModel)
        {
            var dataFileWebServerPath = Utils.GetPathForDataFile(ServerUtility, DataFileName);
            DashboardDataFileService.AddApplication(dataFileWebServerPath, appViewModel);
            return RedirectToAction("Details", "Home", new
            {
                application = appViewModel.Name,
                server = appViewModel.Server
            });
        }

        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.EditingPrivileges)]
        public ActionResult Delete(string application, string server)
        {
            var dataFileWebServerPath = Utils.GetPathForDataFile(ServerUtility, DataFileName);
            var app = DashboardDataFileService.GetApplication(dataFileWebServerPath, application, server);
            DashboardDataFileService.DeleteApplication(dataFileWebServerPath, app);
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}