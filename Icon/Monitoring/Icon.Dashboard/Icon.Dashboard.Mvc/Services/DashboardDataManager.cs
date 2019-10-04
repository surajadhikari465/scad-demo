using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Helpers;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.Models.CustomConfigElements;
using Icon.Dashboard.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Dashboard.Mvc.Services
{
    public class DashboardDataManager : IDashboardDataManager
    {
        public DashboardDataManager(DashboardConfigDataModel configData)
        {
            this.ConfigData = configData;
        }

        public DashboardConfigDataModel ConfigData { get; set; }

        public bool AreChangesAllowed { get; private set; }

        public EnvironmentModel HostingEnvironment
        {
            get
            {
                if (ConfigData == null)
                {
                    throw new ArgumentNullException(
                        $"{nameof(ConfigData)} model must be populated before accessing {nameof(HostingEnvironment)} property");
                }
                if (ConfigData.HostingEnvironmentSetting == EnvironmentEnum.Undefined)
                {
                    throw new ArgumentException(
                        $"{Constants.DashboardAppSettings.Keys.HostingEnvironment} value must be set in web.config and read before accessing {nameof(HostingEnvironment)} property");
                }
                if (ConfigData.EnvironmentDefinitions == null)
                {
                    throw new ArgumentNullException(
                        $"{nameof(EnvironmentsSection)} must must be set in web config before accessing {nameof(HostingEnvironment)}");
                }

                this.ConfigData.EnvironmentDefinitions
                      .Single(e => e.EnvironmentEnum == ConfigData.HostingEnvironmentSetting)
                      .IsHostingEnvironment = true;

                return ConfigData.EnvironmentDefinitions
                      .Single(e => e.EnvironmentEnum == ConfigData.HostingEnvironmentSetting);
            }
        }

        public EnvironmentModel AlternateEnvironment { get; private set; }

        public EnvironmentModel ActiveEnvironment

        {
            get
            {
                return AlternateEnvironment ?? HostingEnvironment;
            }
        }

        public bool HostingEnvironmentIsActive
        {
            get
            {
                return ActiveEnvironment.EnvironmentEnum == HostingEnvironment.EnvironmentEnum;
            }
        }

        public void SetPermissionsForActiveEnvironment(bool userMayEdit,
            EnvironmentCookieModel possibleAlternateEnvironment = null)
        {
            // the Active environment will normally just be the Hosting environment, unless 
            // an alternate environment has been chosen & passed in the provided model
            // (when the alternate environment is null, the Hosting environment will remain Active)
            var environmentFromCookie = GetEnvironmentDefinitonBasedOnCookie(possibleAlternateEnvironment);
            if (environmentFromCookie != null
                && environmentFromCookie.EnvironmentEnum != this.HostingEnvironment.EnvironmentEnum)
            {
                // establish the alternate environment, which will make this the Active environment
                this.AlternateEnvironment = environmentFromCookie;
            }

            this.AreChangesAllowed = DeterminePermissionsForUserInEnvironment(userMayEdit, this.ActiveEnvironment);
        } 

        internal bool DeterminePermissionsForUserInEnvironment(bool userMayEdit, EnvironmentModel activeEnvironment)
        {
            var inProductionEnvironment = EnvironmentHasProductionServers(this.ActiveEnvironment);

            return userMayEdit && !inProductionEnvironment;
        }

        public void SetPermissionsForRemoteEnvironment(bool userMayEdit, string appServer)
        {
            // determine the environment based on the provided remote server- establishing the 
            // alternate environment will make it Active (if the server is not found or unknown, 
            // the Alternate environment will remain null leaving the Hosting environment active)
            var altEnvironment = GetEnvironmentForRemoteServer(appServer);
            if (altEnvironment != null &&
                altEnvironment.EnvironmentEnum != this.HostingEnvironment.EnvironmentEnum)
            {
                this.AlternateEnvironment = altEnvironment;
            }

            var inProductionEnvironment = EnvironmentHasProductionServers(this.ActiveEnvironment);

            this.AreChangesAllowed = userMayEdit && !inProductionEnvironment;
        } 

        public EnvironmentCookieModel GetEnvironmentCookieModelFromEnum(EnvironmentEnum environmentEnum)
        {
            var environmentMatchingChoice = this.ConfigData.EnvironmentDefinitions
                .SingleOrDefault(e => e.EnvironmentEnum == environmentEnum);
            if (environmentMatchingChoice != null)
            {
                var alternateEnvironment = new EnvironmentCookieModel(environmentMatchingChoice.Name, environmentMatchingChoice.EnvironmentEnum);
                alternateEnvironment.AppServers = new List<string>(environmentMatchingChoice.AppServers.Count);
                foreach (var appServer in environmentMatchingChoice.AppServers)
                {
                    alternateEnvironment.AppServers.Add(appServer);
                }
                return alternateEnvironment;
            }
            return null;
        }

        public DashboardEnvironmentCollectionViewModel GetEnvironmentViewModels()
        {
            var viewModelCollection = new DashboardEnvironmentCollectionViewModel();
            var environmentDefinitions = this.ConfigData.EnvironmentDefinitions;

            // iterate list of standard environment definitions (loaded from config)
            foreach (var environmentModel in environmentDefinitions)
            {
                var isHosting = environmentModel.EnvironmentEnum == HostingEnvironment.EnvironmentEnum;
                var isProduction = EnvironmentHasProductionServers(environmentModel);
                var viewModel = environmentModel.ToViewModel(isHosting, isProduction);
                viewModelCollection.Environments.Add(viewModel);
            }

            // add a custom element - either an already-defined custom environment, or an empty custom template
            var customEnvironmentViewModel = new EnvironmentViewModel();
            customEnvironmentViewModel.EnvironmentEnum = EnvironmentEnum.Custom;
            if (ActiveEnvironment.EnvironmentEnum == EnvironmentEnum.Custom)
            {
                customEnvironmentViewModel.Name = ActiveEnvironment.Name;
                customEnvironmentViewModel.AppServers = new List<AppServerViewModel>(ActiveEnvironment.AppServers.Count);
                foreach (var appServer in ActiveEnvironment.AppServers)
                {
                    customEnvironmentViewModel.AppServers.Add(new AppServerViewModel(appServer));
                }
            }
            else
            {
                customEnvironmentViewModel.Name = EnvironmentEnum.Custom.ToString();
                customEnvironmentViewModel.AppServers = new List<AppServerViewModel>();
            }
            viewModelCollection.Environments.Add(customEnvironmentViewModel);

            // set the currently active environment
            var selectedEnvironmentElement = viewModelCollection.Environments
                .SingleOrDefault(e => e.Name.Equals(ActiveEnvironment.Name, Utils.StrcmpOption));
            viewModelCollection.SelectedEnvIndex = viewModelCollection.Environments
                .IndexOf(selectedEnvironmentElement);

            return viewModelCollection;
        }

        public GlobalViewData BuildGlobalViewModel(
            string controllerName,
            string actionName,
            bool userHasEditRights,
            List<LoggedAppViewModel> loggedIconServices,
            List<LoggedAppViewModel> loggedMammothServices,
            string queryParameter = null)
        //string viewTitle = null)
        {
            var viewModel = new GlobalViewData();

            viewModel.ControllerName = controllerName;
            viewModel.ActionName = actionName;

            viewModel.ServiceCommandTimeout = this.ConfigData.ServiceCommandTimeout;
            viewModel.HoursForRecentErrors = this.ConfigData.HoursForRecentErrors;
            viewModel.MillisecondsForRecentErrorsPolling = this.ConfigData.MillisecondsForRecentErrorsPolling;

            viewModel.HostingEnvironment = this.HostingEnvironment.ToViewModel(
                this.HostingEnvironmentIsActive,
                this.EnvironmentHasProductionServers(this.HostingEnvironment));

            viewModel.ActiveEnvironmentName = this.ActiveEnvironment.Name;
            viewModel.ActiveEnvironment = this.ActiveEnvironment.ToViewModel(
                this.ActiveEnvironment.EnvironmentEnum == this.HostingEnvironment.EnvironmentEnum,
                this.EnvironmentHasProductionServers(this.ActiveEnvironment));

            this.AreChangesAllowed = userHasEditRights
               && !EnvironmentHasProductionServers(this.ActiveEnvironment);
            viewModel.ServiceCommandsAreEnabled = this.AreChangesAllowed;

            // build menus for log viewers
            viewModel.SubMenuForIconLogs = BuildSubMenuForAppLogs(
                controllerName,
                actionName,
                IconOrMammothEnum.Icon,
                loggedIconServices,
                queryParameter);

            viewModel.SubMenuForMammothLogs = BuildSubMenuForAppLogs(
                controllerName,
                actionName,
                IconOrMammothEnum.Mammoth,
                loggedMammothServices,
                queryParameter);

            // build menus for api job viewer
            viewModel.SubMenuForIconApiJobs = BuildSubMenuForApiJobs(
                controllerName,
                actionName,
                queryParameter);

            //build support menus
            viewModel.SubMenuForSupportApps = BuildSubMenuForSupportApps(
                this.HostingEnvironment.EnvironmentEnum);

            //build environment selector menu
            viewModel.SubMenuForEnvironments = BuildSubMenuForEnvironments(
                controllerName,
                actionName,
                queryParameter);

            return viewModel;
        }

        internal EnvironmentModel GetEnvironmentForRemoteServer(string appServer)
        {
            return this.ConfigData.EnvironmentDefinitions
                .SingleOrDefault(e => e.AppServers.Contains(appServer));
        }

        internal bool EnvironmentHasProductionServers(EnvironmentModel environment)
        {
            if (environment != null)
            {
                if (environment.EnvironmentEnum == EnvironmentEnum.Prd)
                {
                    return true;
                }

                var defaultPrdEnvironment = this.ConfigData.EnvironmentDefinitions
                    .SingleOrDefault(e => e.EnvironmentEnum == EnvironmentEnum.Prd);
                if (defaultPrdEnvironment != null)
                {
                    foreach (var prdAppServer in defaultPrdEnvironment.AppServers)
                    {
                        if (environment.AppServers
                            .Any(s => s.Equals(prdAppServer, Utils.StrcmpOption)))
                        {
                            return true;
                        }
                    }
                }

                // just in case, check for app servers containing "prd" in them
                foreach (var activeAppServer in environment.AppServers)
                {
                    if (activeAppServer.IndexOf("prd", Utils.StrcmpOption) != -1)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        internal EnvironmentModel GetEnvironmentDefinitonBasedOnCookie(EnvironmentCookieModel environmentFromCoookie = null)
        {
            if (this.HostingEnvironment == null)
            {
                throw new ArgumentException("Hosting Environment must be set before setting active environment");
            }

            // check whether an environment to use is set in the cookie
            if (environmentFromCoookie != null)
            {
                if
                    (environmentFromCoookie.EnvironmentEnum != EnvironmentEnum.Undefined
                        || environmentFromCoookie.EnvironmentEnum != this.HostingEnvironment.EnvironmentEnum)
                {
                    // is the environment set in the cookie a custom or standard environment?
                    if (environmentFromCoookie.EnvironmentEnum == EnvironmentEnum.Custom)
                    {
                        return new EnvironmentModel(environmentFromCoookie);
                    }
                    else
                    {
                        // match the chosen environment to one of the standard definitions (different from hosted)
                        var environmentDefinitionMatchingChoice = this.ConfigData.EnvironmentDefinitions
                            .SingleOrDefault(e => e.EnvironmentEnum == environmentFromCoookie.EnvironmentEnum);
                        // if we don't have a definition for the selected environment, use the default hosting environment
                        if (environmentDefinitionMatchingChoice != null)
                        {
                            return environmentDefinitionMatchingChoice;
                        }
                    }
                }
            }
            return null;
        }

        public EsbEnvironmentViewModel GetEsbEnvironmentViewModel(string name)
        {
            return this.ConfigData.EsbEnvironmentDefinitions
                .SingleOrDefault(e => e.Name.Equals(name, Utils.StrcmpOption))
                .ToViewModel();
        }

        public List<EsbEnvironmentViewModel> GetAllEsbEnvironmentViewModels()
        {
            return this.ConfigData?.EsbEnvironmentDefinitions?
                .ConvertAll(m => m.ToViewModel())
                .OrderBy(m => m.EsbEnvironment)
                .ToList()
                ?? new List<EsbEnvironmentViewModel>();
        }

        public List<EsbEnvironmentViewModel> GetEsbEnvironmentViewModelsWithAssignedServices(
            List<ServiceViewModel> serviceViewModels)
        {
            var esbEnvironmentViewModels = GetAllEsbEnvironmentViewModels();
            if (serviceViewModels != null)
            {
                foreach (var esbEnvironment in esbEnvironmentViewModels)
                {
                    // serviceViewModels should already have ESB environment set when built by remote service wrapper
                    esbEnvironment.AppsInEnvironment = serviceViewModels
                        .Where(s =>
                            s.HasEsbConnections &&
                            s.EsbEnvironmentEnum != EsbEnvironmentEnum.None &&
                            s.EsbEnvironmentEnum == esbEnvironment.EsbEnvironment)
                        .ToList();
                }
            }
            return esbEnvironmentViewModels
                .OrderBy(e => e.EsbEnvironment)
                .ToList();
        }

        internal SubMenuViewModel BuildSubMenuForAppLogs(
            string activeController,
            string activeAction,
            IconOrMammothEnum system,
            List<LoggedAppViewModel> knownServices,
            string queryParameter)
        {
            var logsControllerName = system == IconOrMammothEnum.Mammoth
                ? Constants.MvcNames.MammothLogsControllerName
                : Constants.MvcNames.IconLogsControllerName;
            var logsActionName = system == IconOrMammothEnum.Mammoth
                ? Constants.MvcNames.MammothLogsIndexActionName
                : Constants.MvcNames.IconLogsIndexActionName;

            var subMenuForAppLogs = new SubMenuViewModel();
            subMenuForAppLogs.Header = $"{system} Log Viewer";
            subMenuForAppLogs.TextForRootItem = "All";
            subMenuForAppLogs.ControllerForRootItem = logsControllerName;
            subMenuForAppLogs.ActionForRootItem = logsActionName;
            // if the active view is the log viewer for all apps, then the root menu item (link for all apps) should be active
            subMenuForAppLogs.RootItemIsActive = activeController.Equals(logsControllerName)
                && activeAction.Equals(logsActionName)
                && string.IsNullOrWhiteSpace(queryParameter);

            foreach (var eachKnownApp in knownServices)
            {
                var menuItem = new SubMenuItemViewModel(logsControllerName, logsActionName);
                //menuItem.Environment = hostingEnvironment;
                menuItem.VisibleText = eachKnownApp.AppName;
                menuItem.RouteValuesForItemLink = new { appName = eachKnownApp.AppName };
                // if the log viewer for a single app is the active view, make the menu item for that app active
                menuItem.IsActiveListItem = (activeController.Equals(logsControllerName)
                    && activeAction.Equals(logsActionName)
                    && !string.IsNullOrWhiteSpace(queryParameter)
                    && queryParameter.Equals(eachKnownApp.AppName, Utils.StrcmpOption));
                subMenuForAppLogs.Items.Add(menuItem);
            }
            return subMenuForAppLogs;
        }

        internal SubMenuViewModel BuildSubMenuForApiJobs(
            string activeController,
            string activeAction,
            string queryParameter)
        {
            var apiJobsControllerName = Constants.MvcNames.ApiJobControllerName;
            var apiJobsIndexActionName = Constants.MvcNames.ApiJobsIndexActionName;
            var apiJobsPendingActionName = Constants.MvcNames.ApiJobsPendingActionName;

            var subMenuForApiJobs = new SubMenuViewModel();
            subMenuForApiJobs.Header = $"API Job Monitor";
            subMenuForApiJobs.TextForRootItem = "All";
            subMenuForApiJobs.ControllerForRootItem = apiJobsControllerName;
            subMenuForApiJobs.ActionForRootItem = apiJobsIndexActionName;
            // if the active view is the main view for api jobs, then the root menu item should be active
            subMenuForApiJobs.RootItemIsActive = activeController.Equals(apiJobsControllerName)
                && activeAction.Equals(apiJobsIndexActionName)
                && string.IsNullOrWhiteSpace(queryParameter);

            foreach (var apiJobType in Enum.GetValues(typeof(ApiJobMessageTypeEnum)).Cast<ApiJobMessageTypeEnum>())
            {
                if (apiJobType == ApiJobMessageTypeEnum.All)
                {
                    // add a menu item for pending jobs
                    var menuItemForPendingJobs = new SubMenuItemViewModel(apiJobsControllerName, apiJobsPendingActionName);
                    //menuItemForPendingJobs.Environment = hostingEnvironment;
                    menuItemForPendingJobs.VisibleText = "Pending";
                    menuItemForPendingJobs.RouteValuesForItemLink = new { jobType = "Pending" };
                    //if the pending api jobs view is currently active, make this menu item active
                    menuItemForPendingJobs.IsActiveListItem = activeController.Equals(apiJobsControllerName)
                        && activeAction.Equals(apiJobsPendingActionName);
                    subMenuForApiJobs.Items.Add(menuItemForPendingJobs);
                }
                else
                {
                    // add a menu item for each type of api job
                    var menuItemForJobType = new SubMenuItemViewModel(apiJobsControllerName, apiJobsIndexActionName);
                    // menuItemForJobType.Environment = hostingEnvironment;
                    menuItemForJobType.VisibleText = $"{apiJobType}";
                    menuItemForJobType.RouteValuesForItemLink = new { jobType = apiJobType.ToString() };
                    // if the api job viewer for a single job type is the active view, make the menu item for that job type active
                    menuItemForJobType.IsActiveListItem = (activeController.Equals(apiJobsControllerName)
                        && activeAction.Equals(apiJobsIndexActionName)
                        && !string.IsNullOrWhiteSpace(queryParameter)
                        && queryParameter.Equals(apiJobType.ToString(), Utils.StrcmpOption));
                    subMenuForApiJobs.Items.Add(menuItemForJobType);
                }
            }
            return subMenuForApiJobs;
        }

        internal SubMenuForSupportAppsViewModel BuildSubMenuForSupportApps(
            EnvironmentEnum hostingEnvironment)
        {
            var subMenuForSupportApps = new SubMenuForSupportAppsViewModel();
            subMenuForSupportApps.Header = $"Support Apps";
            subMenuForSupportApps.EnvironmentSubMenus = new List<SubSubMenuForSupportAppsViewModel>();

            foreach (var environmentModel in this.ConfigData.EnvironmentDefinitions.Where(e => e.IsEnabled))
            {
                var subSubMenuForEnviroment = new SubSubMenuForSupportAppsViewModel();
                subSubMenuForEnviroment.EnvironmentEnum = environmentModel.EnvironmentEnum;
                subSubMenuForEnviroment.SubMenuItems = new List<SubSubMenuItemForSupportAppsViewModel>();
                var bootstrapClass = Utils.GetBootstrapClassForEnvironment(environmentModel.EnvironmentEnum);
                subSubMenuForEnviroment.BootstrapClass = bootstrapClass;

                // add link for dashboard (make active if this dashboard?)
                if (!string.IsNullOrWhiteSpace(environmentModel.DashboardUrl))
                {
                    var menuItemForDashboard = new SubSubMenuItemForSupportAppsViewModel();
                    menuItemForDashboard.VisibleText = $"Icon Dashboard {environmentModel.EnvironmentEnum}";
                    menuItemForDashboard.Link = environmentModel.DashboardUrl;
                    menuItemForDashboard.BootstrapClass = bootstrapClass;
                    subSubMenuForEnviroment.SubMenuItems.Add(menuItemForDashboard);
                }
                // add link for mammoth web support
                if (!string.IsNullOrWhiteSpace(environmentModel.MammothWebSupportUrl))
                {
                    var menuItemForMws = new SubSubMenuItemForSupportAppsViewModel();
                    menuItemForMws.VisibleText = $"Mammoth Web Support {environmentModel.EnvironmentEnum}";
                    menuItemForMws.Link = environmentModel.MammothWebSupportUrl;
                    menuItemForMws.BootstrapClass = bootstrapClass;
                    subSubMenuForEnviroment.SubMenuItems.Add(menuItemForMws);
                }
                // add link for icon web
                if (!string.IsNullOrWhiteSpace(environmentModel.IconWebUrl))
                {
                    var menuItemForIconWeb = new SubSubMenuItemForSupportAppsViewModel();
                    menuItemForIconWeb.VisibleText = $"Icon Web {environmentModel.EnvironmentEnum}";
                    menuItemForIconWeb.Link = environmentModel.IconWebUrl;
                    menuItemForIconWeb.BootstrapClass = bootstrapClass;
                    subSubMenuForEnviroment.SubMenuItems.Add(menuItemForIconWeb);
                }
                // add link(s) for tibco admin website
                if (environmentModel.TibcoAdminUrls != null && environmentModel.TibcoAdminUrls.Count > 0)
                {
                    for (int i = 0; i < environmentModel.TibcoAdminUrls.Count; i++)
                    {
                        var menuItemForTibcoAdmin = new SubSubMenuItemForSupportAppsViewModel();
                        var numberSuffix = environmentModel.TibcoAdminUrls.Count == 1
                            ? ""
                            : $" {i + 1}";

                        menuItemForTibcoAdmin.VisibleText = $"TIBCO Admin {environmentModel.EnvironmentEnum}{numberSuffix}";
                        menuItemForTibcoAdmin.Link = environmentModel.TibcoAdminUrls[i];
                        menuItemForTibcoAdmin.BootstrapClass = bootstrapClass;
                        subSubMenuForEnviroment.SubMenuItems.Add(menuItemForTibcoAdmin);
                    }
                }
                subMenuForSupportApps.EnvironmentSubMenus.Add(subSubMenuForEnviroment);
            }
            return subMenuForSupportApps;
        }

        internal SubMenuViewModel BuildSubMenuForEnvironments(
          string activeController,
          string activeAction,
          string queryParameter)
        {
            var homeControllerName = Constants.MvcNames.HomeControllerName;
            var homeIndexActionName = Constants.MvcNames.HomeIndexActionName;
            var homeSetAltEnvironmentActionName = Constants.MvcNames.HomeSetAltEnvironmentActionName;
            var homeCustomActionName = Constants.MvcNames.HomeCustomActionName;

            var subMenuForEnvironments = new SubMenuViewModel();
            subMenuForEnvironments.Header = $"Environment";
            subMenuForEnvironments.TextForRootItem = $"Hosting Environment: {this.HostingEnvironment.Name}";
            subMenuForEnvironments.ControllerForRootItem = homeControllerName;
            subMenuForEnvironments.ActionForRootItem = homeSetAltEnvironmentActionName;
            subMenuForEnvironments.RootItemRouteValues = new { environment = this.HostingEnvironment.EnvironmentEnum.ToString() };
            subMenuForEnvironments.RootItemTextBootstrapClass = Utils
                .GetBootstrapClassForEnvironment(this.HostingEnvironment.EnvironmentEnum);
            // if the active view is the main view for api jobs, then the root menu item should be active
            subMenuForEnvironments.RootItemIsActive = activeController.Equals(homeControllerName)
                && activeAction.Equals(homeIndexActionName)
                && this.HostingEnvironmentIsActive
                && string.IsNullOrWhiteSpace(queryParameter);

            foreach (var environment in this.ConfigData.EnvironmentDefinitions.Where(e => e.IsEnabled))
            {
                // skip the hosting environment, since it is already set as the root item
                if (environment.EnvironmentEnum != this.HostingEnvironment.EnvironmentEnum &&
                    environment.EnvironmentEnum != EnvironmentEnum.Undefined &&
                    environment.EnvironmentEnum != EnvironmentEnum.Custom)
                {
                    // add a menu item for each remaining environment
                    var menuItemForEnv = new SubMenuItemViewModel(homeControllerName, homeSetAltEnvironmentActionName);
                    menuItemForEnv.VisibleText = environment.Name;
                    menuItemForEnv.BoostrapTextClass = Utils.GetBootstrapClassForEnvironment(environment.EnvironmentEnum);
                    // only make the menu item for the environment active if the environment is active
                    menuItemForEnv.IsActiveListItem = this.ActiveEnvironment.EnvironmentEnum != this.HostingEnvironment.EnvironmentEnum
                        && this.ActiveEnvironment.EnvironmentEnum == environment.EnvironmentEnum;
                    menuItemForEnv.RouteValuesForItemLink = new { environment = environment.EnvironmentEnum.ToString() };
                    subMenuForEnvironments.Items.Add(menuItemForEnv);
                }
            }

            // add a menu item for definining a custom environment
            var menuItemForCustomEnv = new SubMenuItemViewModel(homeControllerName, homeCustomActionName);
            menuItemForCustomEnv.VisibleText = "Define Custom";
            menuItemForCustomEnv.BoostrapTextClass = Utils.GetBootstrapClassForEnvironment(EnvironmentEnum.Custom);
            // only make this item active if a custom environment is currently active
            menuItemForCustomEnv.IsActiveListItem = this.ActiveEnvironment.EnvironmentEnum == EnvironmentEnum.Custom;
            //menuItemForCustomEnv.RouteValuesForItemLink = null;
            subMenuForEnvironments.Items.Add(menuItemForCustomEnv);

            return subMenuForEnvironments;
        }
    }
}