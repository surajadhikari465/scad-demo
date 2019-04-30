using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Icon.Dashboard.Mvc.ViewModels;
using Icon.Dashboard.Mvc.Services;
using Icon.Dashboard.Mvc.Helpers;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.Filters;
using System.Security.Principal;
using System.IO;
using System.Configuration;
using System.Xml.Linq;

namespace Icon.Dashboard.Mvc.Controllers
{
    public class HomeController : BaseDashboardController
    {
        protected IDashboardEnvironmentManager dashboardEnvironmentManager { get; private set; } 
        protected IEsbEnvironmentManager esbEnvironmentManager { get; private set; } 
        protected IRemoteWmiServiceWrapper remoteServicesService { get; private set; } 

        public HomeController() : this(null, null, null, null, null ) { }

        public HomeController(
            IIconDatabaseServiceWrapper iconDbService = null,
            IMammothDatabaseServiceWrapper mammothDbService = null,
            IDashboardEnvironmentManager dashboardEnvironmentManager = null,
            IEsbEnvironmentManager esbEnvironmentManager = null,
            IRemoteWmiServiceWrapper remoteServicesService = null)
            : base (iconDbService, mammothDbService)
        {
            this.dashboardEnvironmentManager = dashboardEnvironmentManager ?? new DashboardEnvironmentManager();
            this.esbEnvironmentManager = esbEnvironmentManager ?? new EsbEnvironmentManager();
            this.remoteServicesService = remoteServicesService ?? new RemoteWmiServiceWrapper(Utils.GetMammothDbEnabledFlag(), iconDbService, mammothDbService, esbEnvironmentManager);
        }

        #region GET
      
        [HttpGet]
        [MenuOptionsFilter]
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

            return View(appViewModels);
        }

        [HttpGet]
        [MenuOptionsFilter]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Custom()
        {
            HttpContext.Items["iconLoggingDataService"] = IconDatabaseService;
            HttpContext.Items["mammothLoggingDataService"] = MammothDatabaseService;

            var environmentManager = new DashboardEnvironmentManager();
            var defaultEnvironment = environmentManager.GetDefaultEnvironmentEnumFromWebhost(Request.Url.Host);
            var environmentCollection = environmentManager.BuildEnvironmentCollection(defaultEnvironment);

            return View(environmentCollection);
        }

        [HttpGet]
        [MenuOptionsFilter]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges)]
        public ActionResult Edit(string server, string application)
        {
            HttpContext.Items["iconLoggingDataService"] = IconDatabaseService;
            HttpContext.Items["mammothLoggingDataService"] = MammothDatabaseService;
            var environmentManager = new DashboardEnvironmentManager();

            var environment = environmentManager.GetEnvironmentFromAppserver(server);
            var commandsEnabled = environment != EnvironmentEnum.Prd &&
                DashboardAuthorization.IsAuthorized(HttpContext.User, UserAuthorizationLevelEnum.EditingPrivileges);

            var appViewModel = remoteServicesService.LoadRemoteService(server, application, commandsEnabled);

            ViewBag.CommandsEnabled = commandsEnabled;

            return View(appViewModel);
        }
        #endregion

        #region POST
        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges)]
        public ActionResult Index(string server, string application, string command)
        {
            remoteServicesService.ExecuteServiceCommand(server, application, command);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges)]
        public ActionResult Edit(IconApplicationViewModel appViewModel)
        {
            var environmentManager = new DashboardEnvironmentManager();
            remoteServicesService.SaveRemoteServiceAppSettings(appViewModel);
            return RedirectToAction("Edit", "Home", new { server = appViewModel.Server, application = appViewModel.Name });
        }

        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Custom(DashboardEnvironmentCollectionViewModel customEnvironment)
        {
            HttpContext.Items["iconLoggingDataService"] = IconDatabaseService;
            HttpContext.Items["mammothLoggingDataService"] = MammothDatabaseService;
            var chosenCustomEnvironment = customEnvironment.Environments[customEnvironment.SelectedEnvIndex];
           
            var environmentManager = new DashboardEnvironmentManager();
            var commandsEnabled = !chosenCustomEnvironment.Name.Equals(EnvironmentEnum.Prd.ToString(), Utils.StrcmpOption) &&
                DashboardAuthorization.IsAuthorized(HttpContext.User, UserAuthorizationLevelEnum.EditingPrivileges);
            
            var iconAppsWithLogging = IconDatabaseService.GetApps();
            var mammothAppsWithLogging = MammothDatabaseService.GetApps();
            var allEsbEnvironments = esbEnvironmentManager.GetEsbEnvironmentDefinitions();

            var appViewModels = remoteServicesService.LoadRemoteServices(chosenCustomEnvironment, commandsEnabled);

            ViewBag.CommandsEnabled = commandsEnabled;

            return View("Index", appViewModels);
        }

        #endregion}
    }
}