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
        protected IEsbEnvironmentManager esbEnvironmentManager { get; private set; }
        protected IRemoteWmiServiceWrapper remoteServicesService { get; private set; }

        public HomeController() : this(null, null, null, null, null ) { }

        public HomeController(
            IDashboardEnvironmentManager environmentManager = null,
            IIconDatabaseServiceWrapper iconDbService = null,
            IMammothDatabaseServiceWrapper mammothDbService = null,
            IEsbEnvironmentManager esbEnvironmentManager = null,
            IRemoteWmiServiceWrapper remoteServicesService = null)
            : base (environmentManager, iconDbService, mammothDbService)
        {
            EnvironmentCookieName = "environmentCookie";
            EnvironmentAppServersCookieName = "environmentAppServersCookie";
            EnvironmentCookieDurationHours = GetEnvironmentCookieDurationValue();
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

            var chosenEnvironment = EnvironmentManager.GetEnvironment(Request.Url.Host, environment);
            SetEnvironmentCookies(chosenEnvironment, Response);
            ViewBag.Environment = chosenEnvironment.Name;

            var commandsEnabled = !EnvironmentManager.EnvironmentIsProduction(chosenEnvironment) &&
                DashboardAuthorization.IsAuthorized(HttpContext.User, UserAuthorizationLevelEnum.EditingPrivileges);
            ViewBag.CommandsEnabled = commandsEnabled;

            var appViewModels = remoteServicesService.LoadRemoteServices(chosenEnvironment, commandsEnabled);
            return View(appViewModels);
        }

        [HttpGet]
        [MenuOptionsFilter]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Custom()
        {
            HttpContext.Items["iconLoggingDataService"] = IconDatabaseService;
            HttpContext.Items["mammothLoggingDataService"] = MammothDatabaseService;

            var chosenEnvironment = GetEnvironmentFromCookiesIfPresentOrDefault(Request, EnvironmentManager);
            ViewBag.Environment = chosenEnvironment.Name;
            var environmentCollection = EnvironmentManager.BuildEnvironmentCollection(chosenEnvironment);

            return View(environmentCollection);
        }

        [HttpGet]
        [MenuOptionsFilter]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges)]
        public ActionResult Edit(string server, string application)
        {
            HttpContext.Items["iconLoggingDataService"] = IconDatabaseService;
            HttpContext.Items["mammothLoggingDataService"] = MammothDatabaseService;

            var chosenEnvironment = GetEnvironmentFromCookiesIfPresentOrDefault(Request, EnvironmentManager);
            ViewBag.Environment = chosenEnvironment.Name;
            SetEnvironmentCookies(chosenEnvironment, Response);

            var commandsEnabled =  !EnvironmentManager.EnvironmentIsProduction(chosenEnvironment) &&
                DashboardAuthorization.IsAuthorized(HttpContext.User, UserAuthorizationLevelEnum.EditingPrivileges);
            ViewBag.CommandsEnabled = commandsEnabled;

            var appViewModel = remoteServicesService.LoadRemoteService(server, application, commandsEnabled);


            return View(appViewModel);
        }
        #endregion

        #region POST
        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges)]
        public ActionResult Index(string server, string application, string command)
        {
            var envrionmentForApp = EnvironmentManager.GetEnvironmentEnumFromAppserver(server);
            if (envrionmentForApp != EnvironmentEnum.Prd)
            {
                remoteServicesService.ExecuteServiceCommand(server, application, command);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges)]
        public ActionResult Edit(IconApplicationViewModel appViewModel)
        {
            var environmentForApp = EnvironmentManager.GetEnvironmentEnumFromAppserver(appViewModel.Server);
            if (environmentForApp != EnvironmentEnum.Prd)
            {
                remoteServicesService.SaveRemoteServiceAppSettings(appViewModel);
            }
            return RedirectToAction("Edit", "Home", new { server = appViewModel.Server, application = appViewModel.Name });
        }

        [HttpPost]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Custom(DashboardEnvironmentCollectionViewModel customEnvironment)
        {
            HttpContext.Items["iconLoggingDataService"] = IconDatabaseService;
            HttpContext.Items["mammothLoggingDataService"] = MammothDatabaseService;

            var chosenEnvironment = customEnvironment.Environments[customEnvironment.SelectedEnvIndex];
            ViewBag.Environment = chosenEnvironment.Name;
            SetEnvironmentCookies(chosenEnvironment, Response);
           
            var commandsEnabled = !EnvironmentManager.EnvironmentIsProduction(chosenEnvironment) && 
                DashboardAuthorization.IsAuthorized(HttpContext.User, UserAuthorizationLevelEnum.EditingPrivileges);
            ViewBag.CommandsEnabled = commandsEnabled;

            var appViewModels = remoteServicesService.LoadRemoteServices(chosenEnvironment, commandsEnabled);

            return View("Index", appViewModels);
        }

        #endregion
        protected string EnvironmentCookieName { get; set; }
        protected string EnvironmentAppServersCookieName { get; set; }
        protected int EnvironmentCookieDurationHours { get; set; }

        protected void SetEnvironmentCookies(DashboardEnvironmentViewModel selectedEnvironment, HttpResponseBase response)
        {
            // store the chosen environment in a cookie
            var environmentCookie = new HttpCookie(EnvironmentCookieName, selectedEnvironment.Name);
            environmentCookie.Expires = DateTime.Now.AddHours(EnvironmentCookieDurationHours);
            response.Cookies.Add(environmentCookie);

            var commaSeparatedListOfAppServers = string.Join(",", selectedEnvironment.AppServers.Select(s => s.ServerName));
            var appServersCookie = new HttpCookie(EnvironmentAppServersCookieName, commaSeparatedListOfAppServers);
            appServersCookie.Expires = DateTime.Now.AddHours(EnvironmentCookieDurationHours);
            response.Cookies.Add(appServersCookie);
        }

        protected DashboardEnvironmentViewModel GetEnvironmentFromCookiesIfPresentOrDefault(HttpRequestBase request, IDashboardEnvironmentManager environmentManager)
        {
            // has the client already chosen a particular environment and set a cookie to store the value?
            var environmentNameCookie = request.Cookies[EnvironmentCookieName];
            if (environmentNameCookie == null)
            {
                // if not set by cookie or unexpected environment name, determine the default environment based on the hosting web server
                var defaultEnvironmentForWebhost = environmentManager.BuildEnvironmentViewModelFromWebhost(request.Url.Host);
                return defaultEnvironmentForWebhost;
            }
            else
            {
                // cookie was set previously, so client established environment definition to use & we should get that value
                // get the current environment from the cookie value
                var environmentName = environmentNameCookie.Value;
                var environmentAppServers = new List<string>();

                // were specific app servers for the environment set?
                var appServersCookie = request.Cookies[EnvironmentAppServersCookieName];
                if (appServersCookie == null)
                {
                    // try to get default app servers for environment 
                    if (Enum.TryParse(environmentName, out EnvironmentEnum chosenEnvironmentEnum))
                    {
                        environmentAppServers = environmentManager.GetDefaultAppServersForEnvironment(chosenEnvironmentEnum);
                    }
                }
                else
                {
                    environmentAppServers = appServersCookie.Value.Split(',').ToList();
                }
                var customEnvironmentViewModel = new DashboardEnvironmentViewModel
                {
                    Name = environmentName,
                    AppServers = environmentAppServers
                        .Select(s => new AppServerViewModel { ServerName = s })
                        .ToList()
                };
                return customEnvironmentViewModel;
            }
        }

        private int GetEnvironmentCookieDurationValue()
        {
            const int defaultHours = 1;
            var appSettingForCookieDuration = ConfigurationManager.AppSettings["environmentCookieDurationHours"];
            return int.TryParse(appSettingForCookieDuration ?? string.Empty, out int hours)
                ? hours
                : defaultHours;
        }
    }
}