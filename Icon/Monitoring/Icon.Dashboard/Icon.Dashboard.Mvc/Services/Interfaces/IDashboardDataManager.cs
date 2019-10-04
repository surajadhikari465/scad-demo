using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.ViewModels;
using System;
using System.Collections.Generic;

namespace Icon.Dashboard.Mvc.Services
{
    public interface IDashboardDataManager
    {
        DashboardConfigDataModel ConfigData { get; set; }
        void SetPermissionsForActiveEnvironment(bool userMayEdit, EnvironmentCookieModel possibleAlternateEnvironment = null);
        void SetPermissionsForRemoteEnvironment(bool userMayEdit, string appServer);
        bool AreChangesAllowed { get; }
        EnvironmentModel HostingEnvironment { get; }
        EnvironmentModel AlternateEnvironment { get; }
        EnvironmentModel ActiveEnvironment { get; }
        bool HostingEnvironmentIsActive { get; }
        DashboardEnvironmentCollectionViewModel GetEnvironmentViewModels();
        EnvironmentCookieModel GetEnvironmentCookieModelFromEnum(EnvironmentEnum environmentEnum);
        EsbEnvironmentViewModel GetEsbEnvironmentViewModel(string name);
        List<EsbEnvironmentViewModel> GetAllEsbEnvironmentViewModels();
        List<EsbEnvironmentViewModel> GetEsbEnvironmentViewModelsWithAssignedServices(
            List<ServiceViewModel> serviceViewModels);
        GlobalViewData BuildGlobalViewModel(
            string controllerName,
            string actionName,
            bool userHasEditRights,
            List<LoggedAppViewModel> loggedIconServices,
            List<LoggedAppViewModel> loggedMammothServices,
            string queryParameter = null);
    }
}