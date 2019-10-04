using Icon.Dashboard.Mvc.Enums;
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
        public EsbController() : this(null, null, null, null, null, null) { }

        public EsbController(
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
            this.RemoteServicesService = remoteServicesService ?? new RemoteWmiServiceWrapper();
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
            var esbEnvironmentViewModels = DashboardDataService.
                GetEsbEnvironmentViewModelsWithAssignedServices(serviceViewModels);

            var globalViewModel = base.BuildGlobalViewModel();
            ViewBag.GlobalViewData = globalViewModel;
            //TODO change in views to use GlobalViewData
            ViewBag.Action = nameof(Index);
            ViewBag.Title = "Configure ESB Apps";
            ViewBag.CommandsEnabled = DashboardDataService.AreChangesAllowed;
            ViewBag.Environment = DashboardDataService.ActiveEnvironment.Name;
            return View(esbEnvironmentViewModels);
        }

        [HttpGet]
        [DashboardAuthorizer(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Details(string name)
        {
            var altEnvironment = CookieManager.GetEnvironmentCookieIfPresent(Request);
            DashboardDataService.SetPermissionsForActiveEnvironment(base.UserMayEdit(), altEnvironment);

            if (string.IsNullOrWhiteSpace(name) 
                || !Enum.TryParse<EsbEnvironmentEnum>(name, out EsbEnvironmentEnum esbEnum))
            {
                return RedirectToAction("Index");
            }
            var esbEnvironment = DashboardDataService.GetEsbEnvironmentViewModel(name);

            var globalViewModel = base.BuildGlobalViewModel(name);
            ViewBag.GlobalViewData = globalViewModel;
            ViewBag.Environment = DashboardDataService.ActiveEnvironment.Name;
            ViewBag.Action = "Details";
            ViewBag.Title = $"{0} Dashboard: View ESB Environment \"{DashboardDataService.ActiveEnvironment.Name}\" Configuration";
            return View(esbEnvironment);
        }

        #region POST
        [HttpPost]
        [DashboardAuthorizer(RequiredRole = UserAuthorizationLevelEnum.EditingPrivileges)]
        public ActionResult Index(IEnumerable<EsbEnvironmentViewModel> esbEnvironments)
        {
            var altEnvironment = CookieManager.GetEnvironmentCookieIfPresent(Request);
            DashboardDataService.SetPermissionsForActiveEnvironment(base.UserMayEdit(), altEnvironment);

            var iconApps = base.IconDatabaseService.GetApps();
            var mammothApps = base.MammothDatabaseService.GetApps();

            if (DashboardDataService.AreChangesAllowed)
            {
                var serviceViewModels = RemoteServicesService.LoadRemoteServices(
                    DashboardDataService.ActiveEnvironment.AppServers,
                    DashboardDataService.AreChangesAllowed,
                    iconApps,
                    mammothApps,
                    DashboardDataService.ConfigData.EnvironmentDefinitions,
                    DashboardDataService.ConfigData.EsbEnvironmentDefinitions);

                RemoteServicesService.ReconfigureServiceEsbEnvironmentSettings(
                    esbEnvironments.ToList(),
                    serviceViewModels);
            }

            return RedirectToAction(nameof(Index), "Esb");
        }
        #endregion
    }
}