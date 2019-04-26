using Icon.Dashboard.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.Services
{
    public interface IEsbEnvironmentManager
    {
        EsbEnvironmentViewModel GetEsbEnvironment(string name);
        IEnumerable<EsbEnvironmentViewModel> GetEsbEnvironmentDefinitions();
        IEnumerable<EsbEnvironmentViewModel> GetEsbEnvironmentDefinitionsWithAppsPopulated(IEnumerable<IconApplicationViewModel> dashboardApps);
        void AddEsbEnvironmentDefinition(EsbEnvironmentViewModel esbEnvironment);
        void UpdateEsbEnvironmenDefinition(EsbEnvironmentViewModel esbEnvironment);
        void DeleteEsbEnvironmentDefinition(EsbEnvironmentViewModel esbEnvironment);
        void ReconfigureEsbApps(IEnumerable<EsbEnvironmentViewModel> viewModelWithChanges, IEnumerable<IconApplicationViewModel> applications);
    }
}