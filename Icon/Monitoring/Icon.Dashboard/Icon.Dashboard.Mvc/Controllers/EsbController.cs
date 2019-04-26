using Icon.Dashboard.Mvc.Filters;
using Icon.Dashboard.Mvc.Helpers;
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
    public class EsbController : BaseDashboardController
    {
        protected IDashboardEnvironmentManager dashboardEnvironmentManager { get; private set; } 
        protected IEsbEnvironmentManager esbEnvironmentManager { get; private set; } 
        protected IRemoteWmiServiceWrapper remoteServicesService { get; private set; } 

        public EsbController() : this(null, null, null, null, null) { }

        public EsbController(
            IIconDatabaseServiceWrapper iconDbService = null,
            IMammothDatabaseServiceWrapper mammothDbService = null,
            IDashboardEnvironmentManager dashboardEnvironmentManager = null,
            IEsbEnvironmentManager esbEnvironmentMgmtSvc = null,
            IRemoteWmiServiceWrapper remoteServicesService = null)
            : base (iconDbService, mammothDbService)
        {
            this.dashboardEnvironmentManager = dashboardEnvironmentManager ?? new DashboardEnvironmentManager();
            this.esbEnvironmentManager = esbEnvironmentMgmtSvc ?? new EsbEnvironmentManager();
            this.remoteServicesService = remoteServicesService ?? new RemoteWmiServiceWrapper(iconDbService, mammothDbService, esbEnvironmentManager);
        }

        #region GET

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Index(string environment = null)
        {
            HttpContext.Items["iconLoggingDataService"] = IconDatabaseService;
            HttpContext.Items["mammothLoggingDataService"] = MammothDatabaseService;
            var environmentManager = new DashboardEnvironmentManager();

            var chosenEnvironmentEnum = EnvironmentEnum.Undefined;
            if (string.IsNullOrWhiteSpace(environment) || !Enum.TryParse(environment, out chosenEnvironmentEnum))
            {
                // determine the default environment based on the hosting web server
                chosenEnvironmentEnum = environmentManager.GetDefaultEnvironmentEnumFromWebhost(Request.Url.Host);
            }
            var chosenEnvironmentViewModel = environmentManager.BuildEnvironmentViewModel(chosenEnvironmentEnum);

            var commandsEnabled = chosenEnvironmentEnum != EnvironmentEnum.Prd &&
                DashboardAuthorization.IsAuthorized(HttpContext.User, UserAuthorizationLevelEnum.EditingPrivileges);
            
            var appViewModels = remoteServicesService.LoadRemoteServices(chosenEnvironmentViewModel, commandsEnabled);

            ViewBag.CommandsEnabled = commandsEnabled;

            ViewBag.Action = nameof(Index);
            ViewBag.Title = "Configure ESB Apps";

            var viewData = esbEnvironmentManager.GetEsbEnvironmentDefinitionsWithAppsPopulated(appViewModels);
            return View(viewData);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Details(string name)
        {
            //enable filter to use the logging service for building menus
            HttpContext.Items["iconLoggingDataService"] = IconDatabaseService;
            HttpContext.Items["mammothLoggingDataService"] = MammothDatabaseService;

            if (String.IsNullOrWhiteSpace(name))
            {
                return RedirectToAction("Index");
            }
            var esbEnvironment = esbEnvironmentManager.GetEsbEnvironment(name);
            ViewBag.Action = "Details";
            ViewBag.Title = String.Format("View ESB Environment \"{0}\" Configuration", esbEnvironment.Name);
            return View(esbEnvironment);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Edit(string name)
        {
            //enable filter to use the logging service for building menus
            HttpContext.Items["iconLoggingDataService"] = IconDatabaseService;
            HttpContext.Items["mammothLoggingDataService"] = MammothDatabaseService;

            if (String.IsNullOrWhiteSpace(name))
            {
                return RedirectToAction("Index");
            }
            var esbEnvironment = esbEnvironmentManager.GetEsbEnvironment(name);;

            ViewBag.Title = String.Format("Edit ESB Environment \"{0}\" Configuration", esbEnvironment.Name);
            return View(esbEnvironment);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Create()
        {
            //enable filter to use the logging service for building menus
            HttpContext.Items["iconLoggingDataService"] = IconDatabaseService;
            HttpContext.Items["mammothLoggingDataService"] = MammothDatabaseService;

            var viewModel = new EsbEnvironmentViewModel();
            ViewBag.Title = "Create New ESB Environment Configuration";
            return View(viewModel);
        }

        #endregion

        #region POST
        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges)]
        public ActionResult Index(IEnumerable<EsbEnvironmentViewModel> esbEnvironments, string environment = null)
        {
            var environmentManager = new DashboardEnvironmentManager();

            var chosenEnvironmentEnum = EnvironmentEnum.Undefined;
            if (string.IsNullOrWhiteSpace(environment) || !Enum.TryParse(environment, out chosenEnvironmentEnum))
            {
                // determine the default environment based on the hosting web server
                chosenEnvironmentEnum = environmentManager.GetDefaultEnvironmentEnumFromWebhost(Request.Url.Host);
            }
            var chosenEnvironmentViewModel = environmentManager.BuildEnvironmentViewModel(chosenEnvironmentEnum);

            var commandsEnabled = chosenEnvironmentEnum != EnvironmentEnum.Prd &&
                DashboardAuthorization.IsAuthorized(HttpContext.User, UserAuthorizationLevelEnum.EditingPrivileges);;

            var existingServiceDefinitions = remoteServicesService.LoadRemoteServices(chosenEnvironmentViewModel, commandsEnabled);
            
            esbEnvironmentManager.ReconfigureEsbApps(esbEnvironments, existingServiceDefinitions);

            //TODO figure out if we have to restart _all_ services or not...
            //RemoteServicesService.RestartServices(esbEnvironments);

            return RedirectToAction(nameof(Index), "Esb");
        }

        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges)]
        public ActionResult Edit(EsbEnvironmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                esbEnvironmentManager.UpdateEsbEnvironmenDefinition( model);
                return RedirectToAction("Details", "Esb", new { name = model.Name });
            }

            return View("Edit", model);
        }

        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges)]
        public ActionResult Create(EsbEnvironmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                esbEnvironmentManager.AddEsbEnvironmentDefinition( model);
                return RedirectToAction("Index");
            }

            return View("Create", model);
        }

        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges)]
        public ActionResult Delete(string name)
        {
            var esbEnvironment = esbEnvironmentManager.GetEsbEnvironment(name);
            if (esbEnvironment != null)
            {
                esbEnvironmentManager.DeleteEsbEnvironmentDefinition(esbEnvironment);
            }
            return RedirectToAction("Index");
        }
        #endregion
    }
}