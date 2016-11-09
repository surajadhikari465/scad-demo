using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Icon.Dashboard.Mvc.ViewModels;
using Icon.Dashboard.Mvc.Services;
using System.Configuration;
using Icon.Dashboard.Mvc.Helpers;

namespace Icon.Dashboard.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private const string defaultDataFile = "DashboardApplications.xml";

        public IDataFileServiceWrapper DashboardDataFileService { get; private set; }
        public IIconDatabaseServiceWrapper IconDatabaseService { get; private set; }

        public string XmlDataFile { get; set; }

        private HttpServerUtilityBase _serverUtility;
        public HttpServerUtilityBase ServerUtility
        {
            get
            {
                return _serverUtility ?? Server;
            }
        }

        public HomeController() : this(null, null, null, null)
        {
        }

        public HomeController(string dataFile = null,
            IDataFileServiceWrapper dataServiceWrapper = null,
            IIconDatabaseServiceWrapper loggingServiceWrapper = null,
            HttpServerUtilityBase serverUtility = null)
        {
            XmlDataFile = dataFile ?? defaultDataFile;
            DashboardDataFileService = dataServiceWrapper ?? new DataFileServiceWrapper();
            IconDatabaseService = loggingServiceWrapper ?? new IconDatabaseServiceWrapper();
            
            _serverUtility = serverUtility;
        }

        [HttpGet]
        [SetViewBagMenuOptionsFilter]
        public ActionResult Index()
        {
            HttpContext.Items["loggingDataService"] = IconDatabaseService;
            var environment = ConfigurationManager.AppSettings["activeEnvironment"];
            var viewModels = DashboardDataFileService.GetApplicationListViewModels(ServerUtility, XmlDataFile, environment);
            return View(viewModels);
        }

        [HttpPost]
        public ActionResult Index(string application, string server, string command)
        {
            DashboardDataFileService.ExecuteCommand(ServerUtility, XmlDataFile, application, server, command);
            return RedirectToAction("Index", "Home" );
        }

        [HttpGet]
        [SetViewBagMenuOptionsFilter]
        public ActionResult Details(string application, string server)
        {
            HttpContext.Items["loggingDataService"] = IconDatabaseService;
            var viewModel = DashboardDataFileService.GetApplicationViewModel(ServerUtility, XmlDataFile, application, server);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Details(string application, string server, string command)
        {
            DashboardDataFileService.ExecuteCommand(ServerUtility, XmlDataFile, application, server, command);
            return RedirectToAction("Details", "Home", new { application = application, server = server });
        }

        [HttpGet]
        [SetViewBagMenuOptionsFilter]
        public ActionResult Edit(string application, string server)
        {
            HttpContext.Items["loggingDataService"] = IconDatabaseService;
            var viewModel = DashboardDataFileService.GetApplicationViewModel(ServerUtility, XmlDataFile, application, server);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(IconApplicationViewModel appViewModel)
        {
            DashboardDataFileService.UpdateApplication(ServerUtility, appViewModel, XmlDataFile);
            return RedirectToAction("Details", "Home", new { application = appViewModel.Name, server = appViewModel.Server });
        }

        [HttpGet]
        [SetViewBagMenuOptionsFilter]
        public ActionResult Configure(string application, string server)
        {
            HttpContext.Items["loggingDataService"] = IconDatabaseService;
            var viewModel = DashboardDataFileService.GetApplicationViewModel(ServerUtility, XmlDataFile, application, server);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Configure(IconApplicationViewModel appViewModel)
        {
            ViewBag.EnvironmentOptions = new EnvironmentSwitcher().GetServersForEnvironments();
            ViewBag.DataFlowSystemOptions = Utils.GetDataFlowSystemSelections();
            DashboardDataFileService.SaveAppSettings(appViewModel);
            return RedirectToAction("Details", "Home", new { application = appViewModel.Name, server = appViewModel.Server });
        }

        [HttpGet]
        [SetViewBagMenuOptionsFilter]
        public ActionResult Create()
        {
            HttpContext.Items["loggingDataService"] = IconDatabaseService;
            var viewModel = new IconApplicationViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(IconApplicationViewModel appViewModel)
        {
            DashboardDataFileService.AddApplication(ServerUtility, appViewModel, XmlDataFile);
            return RedirectToAction("Details", "Home", new { application = appViewModel.Name, server = appViewModel.Server });
        }

        [HttpPost]
        public ActionResult Delete(string application, string server)
        {
            var app = DashboardDataFileService.GetApplication(ServerUtility, XmlDataFile, application, server);
            DashboardDataFileService.DeleteApplication(ServerUtility, app, XmlDataFile);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult TaskPartial(string application, string server)
        {
            var task = DashboardDataFileService.GetTaskViewModel(ServerUtility, XmlDataFile, application, server);
            return PartialView("_TaskViewModelPartial", task);
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult IconApiServicePartial(string application, string server)
        {
            var task = DashboardDataFileService.GetServiceViewModel(ServerUtility, XmlDataFile, application, server);
            return PartialView("_IconApiServicePartial", task);
        }
    }
}