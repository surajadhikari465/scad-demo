using System.Collections.Generic;
using Icon.Dashboard.DataFileAccess.Models;
using Icon.Dashboard.DataFileAccess.Services;
using Icon.Dashboard.Mvc.ViewModels;
using System.Web;
using System.Threading.Tasks;
using System;

namespace Icon.Dashboard.Mvc.Services
{
    public interface IDataFileServiceWrapper
    {
        IIconDashboardDataService IconMonitoringService { get; }
        IEnumerable<IconApplicationViewModel> GetApplicationListViewModels(HttpServerUtilityBase serverUtility, string configFile);
        IEnumerable<IconApplicationViewModel> ToViewModels(IEnumerable<IApplication> applications,
            HttpServerUtilityBase serverUtility, string dataFileName);

        IApplication GetApplication(HttpServerUtilityBase serverUtility, string configFile, string application, string server);
        IconApplicationViewModel GetApplicationViewModel(HttpServerUtilityBase serverUtility, string configFile, string application, string server);
        TaskViewModel GetTaskViewModel(HttpServerUtilityBase serverUtility, string configFile, string application, string server);
        ServiceViewModel GetServiceViewModel(HttpServerUtilityBase serverUtility, string configFile, string application, string server);
        void ExecuteCommand(HttpServerUtilityBase serverUtility, string dataFileName, string application, string server, string command);
        void UpdateApplication(HttpServerUtilityBase serverUtility, IconApplicationViewModel application, string pathToXmlDataFile);
        void AddApplication(HttpServerUtilityBase serverUtility, IconApplicationViewModel application, string dataFileName);
        void DeleteApplication(HttpServerUtilityBase serverUtility, IApplication application, string dataFileName);
        void SaveAppSettings(IconApplicationViewModel application);
        IApplication FromViewModel(IconApplicationViewModel viewModel);

        IEnumerable<EsbEnvironmentViewModel> GetEsbEnvironments(HttpServerUtilityBase serverUtility, string pathToXmlDataFile);
        IEnumerable<string> GetEsbEnvironmentNames(HttpServerUtilityBase serverUtility, string pathToXmlDataFile);
        EsbEnvironmentViewModel GetEsbEnvironment(HttpServerUtilityBase serverUtility, string pathToXmlDataFile, string name);
        void AddEsbEnvironment(HttpServerUtilityBase serverUtility, EsbEnvironmentViewModel esbEnvironment, string pathToXmlDataFile);
        void UpdateEsbEnvironment(HttpServerUtilityBase serverUtility, EsbEnvironmentViewModel esbEnvironment, string pathToXmlDataFile);
        void DeleteEsbEnvironment(HttpServerUtilityBase serverUtility, EsbEnvironmentViewModel esbEnvironment, string pathToXmlDataFile);
        List<Tuple<bool,string>> SetEsbEnvironment(HttpServerUtilityBase serverUtility, EsbEnvironmentViewModel esbEnvironment, string pathToXmlDataFile);
        EsbEnvironmentViewModel GetCurrentEsbEnvironment(HttpServerUtilityBase serverUtility, string pathToXmlDataFile);
        IEnumerable<IApplication> GetApplications(HttpServerUtilityBase serverUtility, string dataFileName);
    }
}