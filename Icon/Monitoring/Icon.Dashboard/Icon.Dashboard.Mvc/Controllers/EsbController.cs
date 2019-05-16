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
        protected IEsbEnvironmentManager esbEnvironmentManager { get; private set; } 
        protected IRemoteWmiServiceWrapper remoteServicesService { get; private set; } 

        public EsbController() : this(null, null, null, null, null) { }

        public EsbController(
            IDashboardEnvironmentManager environmentManager = null,
            IIconDatabaseServiceWrapper iconDbService = null,
            IMammothDatabaseServiceWrapper mammothDbService = null,
            IEsbEnvironmentManager esbEnvironmentMgmtSvc = null,
            IRemoteWmiServiceWrapper remoteServicesService = null)
            : base (environmentManager, iconDbService, mammothDbService)
        {
            this.esbEnvironmentManager = esbEnvironmentMgmtSvc ?? new EsbEnvironmentManager();
            this.remoteServicesService = remoteServicesService ?? new RemoteWmiServiceWrapper(Utils.GetMammothDbEnabledFlag(), iconDbService, mammothDbService, esbEnvironmentManager);
        }

        #region GET

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Index()
        {
            HttpContext.Items["iconLoggingDataService"] = IconDatabaseService;
            HttpContext.Items["mammothLoggingDataService"] = MammothDatabaseService;

            var currentEnvironment = EnvironmentManager.GetEnvironment(Request.Url.Host);
            ViewBag.Environment = currentEnvironment.Name;
            
            var commandsEnabled = !EnvironmentManager.EnvironmentIsProduction(currentEnvironment) &&
                DashboardAuthorization.IsAuthorized(HttpContext.User, UserAuthorizationLevelEnum.EditingPrivileges);
            
            var appViewModels = remoteServicesService.LoadRemoteServices(currentEnvironment, commandsEnabled);

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

            var currentEnvironment = EnvironmentManager.GetEnvironment(Request.Url.Host);
            ViewBag.Environment = currentEnvironment.Name;

            if (String.IsNullOrWhiteSpace(name))
            {
                return RedirectToAction("Index");
            }
            var esbEnvironment = esbEnvironmentManager.GetEsbEnvironment(name);
            ViewBag.Action = "Details";
            ViewBag.Title = String.Format("{0} Dashboard: View ESB Environment \"{1}\" Configuration", currentEnvironment.Name, esbEnvironment.Name);
            return View(esbEnvironment);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Edit(string name)
        {
            //enable filter to use the logging service for building menus
            HttpContext.Items["iconLoggingDataService"] = IconDatabaseService;
            HttpContext.Items["mammothLoggingDataService"] = MammothDatabaseService;

            var currentEnvironment = EnvironmentManager.GetEnvironment(Request.Url.Host);
            ViewBag.Environment = currentEnvironment.Name;

            if (String.IsNullOrWhiteSpace(name))
            {
                return RedirectToAction("Index");
            }
            var esbEnvironment = esbEnvironmentManager.GetEsbEnvironment(name);;

            ViewBag.Title = String.Format("{0} Dashoard: Edit ESB Environment \"{1}\" Configuration", currentEnvironment.Name, esbEnvironment.Name);
            return View(esbEnvironment);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Create()
        {
            //enable filter to use the logging service for building menus
            HttpContext.Items["iconLoggingDataService"] = IconDatabaseService;
            HttpContext.Items["mammothLoggingDataService"] = MammothDatabaseService;

            var currentEnvironment = EnvironmentManager.GetEnvironment(Request.Url.Host);
            ViewBag.Environment = currentEnvironment.Name;

            var viewModel = new EsbEnvironmentViewModel();
            ViewBag.Title = String.Format("{0} Dashboard: Create New ESB Environment Configuration", currentEnvironment.Name);
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