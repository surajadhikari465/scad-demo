using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Filters;
using Icon.Dashboard.Mvc.Helpers;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.Services;
using Icon.Dashboard.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;

namespace Icon.Dashboard.Mvc.Controllers
{
    /// <summary>
    /// Base class for dashboard controllers
    /// </summary>
    public abstract class BaseDashboardController : Controller
    {
        public BaseDashboardController() : this(null, null, null, null) { }

        public BaseDashboardController(
            IDashboardAuthorizer dashboardAuthorizer = null,
            IDashboardDataManager dashboardConfigManager = null,
            IIconDatabaseServiceWrapper iconDbService = null,
            IMammothDatabaseServiceWrapper mammothDbService = null)
        {
            Authorizer = dashboardAuthorizer ?? 
                new DashboardAuthorizer(
                    DashboardGlobals.ConfigData.SecurityGroupsWithReadRights,
                    DashboardGlobals.ConfigData.SecurityGroupsWithEditRights);
            
            DashboardDataService = dashboardConfigManager ?? new DashboardDataManager(DashboardGlobals.ConfigData);
            IconDatabaseService = iconDbService ?? new IconDatabaseServiceWrapper();
            MammothDatabaseService = mammothDbService ?? new MammothDatabaseServiceWrapper();
        }

        protected IDashboardAuthorizer Authorizer { get; private set; }
        protected IDashboardDataManager DashboardDataService { get; private set; }
        protected IIconDatabaseServiceWrapper IconDatabaseService { get; private set; }
        protected IMammothDatabaseServiceWrapper MammothDatabaseService { get; private set; }

        protected IPrincipal UserPrincipal
        {
            get
            {
                return HttpContext.User;
            }
        }

        protected string ControllerName
        {
            get
            {
                return ControllerContext.RouteData.Values["controller"].ToString();
            }
        }

        protected string ActionName
        {
            get
            {
                return ControllerContext.RouteData.Values["action"].ToString();
            }
        }

        public bool UserMayEdit()
        {
            return Authorizer.IsAuthorized(this.UserPrincipal, UserAuthorizationLevelEnum.EditingPrivileges);
        }

        public List<LoggedAppViewModel> GetAppsForLoggingMenu(IconOrMammothEnum system)
        {
            var loggedApps = new List<LoggedAppViewModel>();
            switch (system)
            {
                case IconOrMammothEnum.Icon:
                    loggedApps = IconDatabaseService.GetApps();
                    break;
                case IconOrMammothEnum.Mammoth:
                    loggedApps =  MammothDatabaseService.GetApps();
                    break;
                default:
                    break;
            }
            return loggedApps;
        }

        public GlobalViewData BuildGlobalViewModel(string queryParameter = null)
        {
            var userMayEdit = UserMayEdit();

            return DashboardDataService.BuildGlobalViewModel(
                ControllerName,
                ActionName,
                userMayEdit,
                GetAppsForLoggingMenu(IconOrMammothEnum.Icon),
                GetAppsForLoggingMenu(IconOrMammothEnum.Mammoth),
                queryParameter);
        }
    }
}