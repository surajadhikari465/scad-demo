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
using Icon.Dashboard.Mvc.Enums;

namespace Icon.Dashboard.Mvc.Controllers
{
    public class HomeController : BaseDashboardController
    {
        public HomeController() : this(null, null, null, null, null, null) { }

        public HomeController(
            IDashboardAuthorizer dashboardAuthorizer = null,
            IDashboardDataManager dashboardConfigManager = null,
            IIconDatabaseServiceWrapper iconDbService = null,
            IMammothDatabaseServiceWrapper mammothDbService = null,
            IEnvironmentCookieManager cookieManager = null,
            IRemoteWmiServiceWrapper remoteServicesService = null)
            : base (dashboardAuthorizer, dashboardConfigManager, iconDbService, mammothDbService)
        {
            this.CookieManager = cookieManager ??
                new EnvironmentCookieManager(
                    DashboardGlobals.ConfigData.EnvironmentCookieDurationHours,
                    DashboardGlobals.ConfigData.EnvironmentCookieName,
                    DashboardGlobals.ConfigData.EnvironmentAppServersCookieName);
            this.RemoteServicesService = remoteServicesService ??  new RemoteWmiServiceWrapper();
        }

        protected IEnvironmentCookieManager CookieManager { get; private set; }
        protected IRemoteWmiServiceWrapper RemoteServicesService { get; private set; }

        [HttpGet]
        [DashboardAuthorizer(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Index()
        {
            var altEnvironment = CookieManager.GetEnvironmentCookieIfPresent(Request);
            DashboardDataService.SetPermissionsForActiveEnvironment(base.UserMayEdit(), altEnvironment);

            var iconApps = base.IconDatabaseService.GetApps();
            var mammothApps = base.MammothDatabaseService.GetApps();

            var serviceViewModels = RemoteServicesService.LoadRemoteServices(
                DashboardDataService.ActiveEnvironment.AppServers,
                DashboardDataService.AreChangesAllowed,
                iconApps,
                mammothApps,
                DashboardDataService.ConfigData.EnvironmentDefinitions,
                DashboardDataService.ConfigData.EsbEnvironmentDefinitions);

            var globalViewModel = base.BuildGlobalViewModel();
            globalViewModel.ViewTitle = "Icon Dashboard";
            ViewBag.GlobalViewData = globalViewModel;

            //TODO change in views to use GlobalViewData
            ViewBag.CommandsEnabled = DashboardDataService.AreChangesAllowed;
            ViewBag.Environment = DashboardDataService.ActiveEnvironment.Name;

            return View(serviceViewModels);
        }

        [HttpGet]
        [DashboardAuthorizer(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult SetAlternateEnvironment(string environment)
        {
            //environment parameter is used to allow selecting an alternate environment (other than the native hosting one)
            if (!string.IsNullOrWhiteSpace(environment)
                && Enum.TryParse(environment, out EnvironmentEnum altEnvironmentEnum))
            {
                if (altEnvironmentEnum == DashboardDataService.ConfigData.HostingEnvironmentSetting)
                {
                    CookieManager.ClearEnvironmentCookies(Request, Response);
                }
                else if (altEnvironmentEnum != EnvironmentEnum.Undefined
                    && altEnvironmentEnum != EnvironmentEnum.Custom )
                {
                    var altEnvironmentCookieModel = DashboardDataService.GetEnvironmentCookieModelFromEnum(altEnvironmentEnum);

                    // set the environment cookie with the selected alternate environment
                    CookieManager.SetEnvironmentCookies(Request, Response, altEnvironmentCookieModel);
                }
            }
            
            return RedirectToAction(Constants.MvcNames.HomeIndexActionName, Constants.MvcNames.HomeControllerName);
        }

        [HttpGet]
        [DashboardAuthorizer(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Custom()
        {
            var altEnvironment = CookieManager.GetEnvironmentCookieIfPresent(Request);
            DashboardDataService.SetPermissionsForActiveEnvironment(base.UserMayEdit(), altEnvironment);

            var environmentCollection = DashboardDataService.GetEnvironmentViewModels();

            //var globalViewModel = base.BuildGlobalViewModel();
            var globalViewModel = base.BuildGlobalViewModel();
            //TODO change in views to use GlobalViewData
            ViewBag.GlobalViewData = globalViewModel;
            return View(environmentCollection);
        }

        [HttpGet]
        [DashboardAuthorizer(RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges)]
        public ActionResult Edit(string appServer, string serviceName)
        {
            if (string.IsNullOrWhiteSpace(appServer) || string.IsNullOrWhiteSpace(serviceName))
            {
                return RedirectToAction("Index");
            }
            // get the environment for the server running the service
            DashboardDataService.SetPermissionsForRemoteEnvironment(base.UserMayEdit(), appServer);

            var iconApps = base.IconDatabaseService.GetApps();
            var mammothApps = base.MammothDatabaseService.GetApps();

            // load the remote service information
            var serviceViewModel = RemoteServicesService.LoadRemoteService(
                appServer,
                serviceName,
                DashboardDataService.AreChangesAllowed,
                iconApps,
                mammothApps,
                DashboardDataService.ConfigData.EnvironmentDefinitions,
                DashboardDataService.ConfigData.EsbEnvironmentDefinitions);

            //do not pass query params here
            var globalViewModel = base.BuildGlobalViewModel();
            //TODO set title her or in view?
            globalViewModel.ViewTitle = $"View\\Edit Configuration for \"{serviceViewModel.DisplayName}\"";

            ViewBag.CommandsEnabled = DashboardDataService.AreChangesAllowed;
            ViewBag.GlobalViewData = globalViewModel;
            return View(serviceViewModel);
        }

        [HttpPost]
        [DashboardAuthorizer(RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges)]
        public ActionResult Index(string appServer, string serviceName, string command)
        {
            // get the environment for the server running the service
            DashboardDataService.SetPermissionsForRemoteEnvironment(base.UserMayEdit(), appServer);
  
            if (DashboardDataService.AreChangesAllowed)
            {
                RemoteServicesService.ExecuteServiceCommand(appServer, serviceName, command);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [DashboardAuthorizer(RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges)]
        public ActionResult Edit(ServiceViewModel appViewModel)
        {
            // get the environment for the server running the service
            DashboardDataService.SetPermissionsForRemoteEnvironment(base.UserMayEdit(), appViewModel.Server);

            if (DashboardDataService.AreChangesAllowed)
            {
                RemoteServicesService.UpdateRemoteServiceConfig(appViewModel);
            }
            return RedirectToAction("Edit", "Home", new { appServer = appViewModel.Server, serviceName = appViewModel.Name });
        }

        [HttpPost]
        [DashboardAuthorizer(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Custom(DashboardEnvironmentCollectionViewModel customEnvironment)
        {
            var chosenEnvironment = customEnvironment.Environments[customEnvironment.SelectedEnvIndex];
            // only set environment cookie if non-hosting environment selected; otherwise clear cookie
            if (chosenEnvironment.EnvironmentEnum == DashboardDataService.ActiveEnvironment.EnvironmentEnum)
            {
                CookieManager.ClearEnvironmentCookies(Request, Response);
                DashboardDataService.SetPermissionsForActiveEnvironment(base.UserMayEdit());
            }
            else
            {
                var selectedEnvironment = new EnvironmentCookieModel(chosenEnvironment.Name, chosenEnvironment.EnvironmentEnum);
                if (chosenEnvironment.AppServers!=null && chosenEnvironment.AppServers.Count>0)
                {
                    foreach(var appServer in chosenEnvironment.AppServers)
                    {
                        selectedEnvironment.AppServers.Add(appServer.ServerName);
                    }
                }
                CookieManager.SetEnvironmentCookies(Request, Response, selectedEnvironment);
                DashboardDataService.SetPermissionsForActiveEnvironment(base.UserMayEdit(), selectedEnvironment);
            }

            var iconApps = base.IconDatabaseService.GetApps();
            var mammothApps = base.MammothDatabaseService.GetApps();

            var appViewModels = RemoteServicesService.LoadRemoteServices(
                DashboardDataService.ActiveEnvironment.AppServers,
                DashboardDataService.AreChangesAllowed,
                iconApps,
                mammothApps,
                DashboardDataService.ConfigData.EnvironmentDefinitions,
                DashboardDataService.ConfigData.EsbEnvironmentDefinitions);

            var globalViewModel = base.BuildGlobalViewModel();
            globalViewModel.ViewTitle = $"Icon Dashboard ({DashboardDataService.ActiveEnvironment.Name} Services)";
            //ViewBag.Environment = chosenEnvironment.Name;
            ViewBag.CommandsEnabled = DashboardDataService.AreChangesAllowed;
            ViewBag.GlobalViewData = globalViewModel;
            return View("Index", appViewModels);
        }
    }
}