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
using Icon.Dashboard.DataFileAccess.Models;
using Icon.Dashboard.RemoteServicesAccess;
using System.IO;
using System.Configuration;
using System.Xml.Linq;

namespace Icon.Dashboard.Mvc.Controllers
{
    public class HomeController : BaseDashboardController
    {
        public HomeController() : this(null, null, null, null, null, null) { }

        public HomeController(
            HttpServerUtilityBase serverUtility = null,
            IIconDatabaseServiceWrapper iconDataBaseService = null,
            string pathToXmlDataFile = null,
            IDataFileServiceWrapper dataServiceWrapper = null,
            IMammothDatabaseServiceWrapper mammothDbService = null,
            IRemoteWmiServiceWrapper remoteServicesService = null)
            : base(serverUtility,
                  iconDataBaseService,
                  pathToXmlDataFile,
                  dataServiceWrapper,
                  mammothDbService,
                  remoteServicesService) { }

        #region GET
      
        [HttpGet]
        [MenuOptionsFilter]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Index(DashboardEnvironmentCollectionViewModel customEnvironment = null)
        {
            var chosenEnvironment = new DashboardEnvironmentViewModel();

            if (customEnvironment == null  || customEnvironment.Environments.Count<1)
            {
                // determine the default environment based on the hosting web server
                var envSwitcher = new EnvironmentSwitcher();
                chosenEnvironment = envSwitcher.GetDefaultEnvironment(Request.Url.Host);
            }
            else
            { 
                // use the selected environment in the provided model
                chosenEnvironment = customEnvironment.Environments[customEnvironment.SelectedEnvIndex];
            }

            var commandsEnabled = DashboardAuthorization.IsAuthorized(HttpContext.User, UserAuthorizationLevelEnum.EditingPrivileges);
            var dataFileWebServerPath = Utils.GetPathForDataFile(ServerUtility, DataFileName);

            RemoteServicesService.IconApps = IconDatabaseService.GetApps();
            RemoteServicesService.MammothApps = MammothDatabaseService.GetApps();
            RemoteServicesService.EsbEnvironments = DashboardDataFileService.GetEsbEnvironmentsWithoutApplications(dataFileWebServerPath);

            var appViewModels = RemoteServicesService.LoadRemoteServices(chosenEnvironment, commandsEnabled);

            ViewBag.CommandsEnabled = commandsEnabled;

            return View(appViewModels);
        }

        [HttpGet]
        [MenuOptionsFilter]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Custom()
        {
            HttpContext.Items["loggingDataService"] = IconDatabaseService;

            var envSwitcher = new EnvironmentSwitcher();
            var defaultEnvironment = EnvironmentEnum.Undefined;
            var environmentCollection = new DashboardEnvironmentCollectionViewModel();

            var defaultEnvironmentName = envSwitcher.GetWebServersForEnvironments()
                    .FirstOrDefault(e => e.Value.Equals(Request.Url.Host, StringComparison.InvariantCultureIgnoreCase)).Key;
            if (string.IsNullOrWhiteSpace(defaultEnvironmentName))
            {
                // when debugging on local system
                defaultEnvironment = EnvironmentEnum.Dev;
            }
            else
            {
                Enum.TryParse(defaultEnvironmentName, out defaultEnvironment);
            }

            foreach (var environment in Enum.GetValues(typeof(EnvironmentEnum)).Cast<EnvironmentEnum>())
            {
                if (environment == EnvironmentEnum.Undefined) continue;

                var defaultAppServersForEnvironment = envSwitcher.GetDefaultAppServersForEnvironment(environment);
                var environmentViewModel = new DashboardEnvironmentViewModel()
                {
                    Name = environment.ToString(),
                    AppServers = defaultAppServersForEnvironment
                       .Select(s => new AppServerViewModel { ServerName = s })
                       .ToList()
                };
                environmentCollection.Environments.Add(environmentViewModel);
                if (environment == defaultEnvironment)
                {
                    environmentCollection.SelectedEnvIndex = environmentCollection.Environments.IndexOf(environmentViewModel);
                }
            }
            return View(environmentCollection);
        }

        [HttpGet]
        [MenuOptionsFilter]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges)]
        public ActionResult Edit(string server, string application)
        {
            HttpContext.Items["loggingDataService"] = IconDatabaseService;
            
            var commandsEnabled = DashboardAuthorization.IsAuthorized(HttpContext.User, UserAuthorizationLevelEnum.EditingPrivileges);
            var dataFileWebServerPath = Utils.GetPathForDataFile(ServerUtility, DataFileName);

            RemoteServicesService.IconApps = IconDatabaseService.GetApps();
            RemoteServicesService.MammothApps = MammothDatabaseService.GetApps();
            RemoteServicesService.EsbEnvironments = DashboardDataFileService.GetEsbEnvironmentsWithoutApplications(dataFileWebServerPath);

            var appViewModel = RemoteServicesService.LoadRemoteService(server, application, commandsEnabled);
            ViewBag.CommandsEnabled = commandsEnabled;

            return View(appViewModel);
        }
        #endregion

        #region POST
        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges)]
        public ActionResult Index(string server, string application, string command)
        {
            var dataFileWebServerPath = Utils.GetPathForDataFile(ServerUtility, DataFileName);
            DashboardDataFileService.ExecuteServiceCommand(dataFileWebServerPath, application, server, command);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges)]
        public ActionResult Edit(IconApplicationViewModel appViewModel)
        {
            var dataFileWebServerPath = Utils.GetPathForDataFile(ServerUtility, DataFileName);
            DashboardDataFileService.SaveAppSettings(appViewModel);
            return RedirectToAction("Edit", "Home", new { server = appViewModel.Server, application = appViewModel.Name });
        }

        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Custom(DashboardEnvironmentCollectionViewModel customEnvironment)
        {
            var chosenCustomEnvironment = customEnvironment.Environments[customEnvironment.SelectedEnvIndex];
            
            var commandsEnabled = DashboardAuthorization.IsAuthorized(HttpContext.User, UserAuthorizationLevelEnum.EditingPrivileges);
            
            var iconAppsWithLogging = IconDatabaseService.GetApps();
            var mammothAppsWithLogging = MammothDatabaseService.GetApps();
            var dataFileWebServerPath = Utils.GetPathForDataFile(ServerUtility, DataFileName);
            var allEsbEnvironments = DashboardDataFileService.GetEsbEnvironmentsWithoutApplications(dataFileWebServerPath);

            var appViewModels = RemoteServicesService.LoadRemoteServices(chosenCustomEnvironment,
                commandsEnabled,
                iconAppsWithLogging,
                mammothAppsWithLogging,
                allEsbEnvironments);

            ViewBag.CommandsEnabled = commandsEnabled;

            return View("Index", appViewModels);
        }
      
        #endregion}
    }
}