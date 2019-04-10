using System.Collections.Generic;
using Icon.Dashboard.DataFileAccess.Models;
using Icon.Dashboard.DataFileAccess.Services;
using Icon.Dashboard.Mvc.ViewModels;
using System;

namespace Icon.Dashboard.Mvc.Services
{
    public interface IDataFileServiceWrapper
    {
        IIconDashboardDataService IconMonitoringService { get; }

        void UpdateApplication(string pathToXmlDataFile, IconApplicationViewModel application);

        void AddApplication(string pathToXmlDataFile, IconApplicationViewModel application);

        void DeleteApplication(string pathToXmlDataFile, IconApplicationViewModel application);

        void ExecuteServiceCommand(string pathToXmlDataFile, string application, string server, string command);

        void SaveAppSettings(IconApplicationViewModel application);

        IconApplicationViewModel GetApplication(string pathToXmlDataFile,
           string application, string server);

        IconApplicationViewModel GetApplication(string pathToXmlDataFile,
             string application, string server, IEnumerable<EsbEnvironmentViewModel> esbEnvironmentNamess);

        IEnumerable<IconApplicationViewModel> GetApplications(string pathToXmlDataFile);

        IEnumerable<EsbEnvironmentViewModel> GetEsbEnvironments(string pathToXmlDataFile, bool includeUnassignedApps = false);
        
        IEnumerable<EsbEnvironmentViewModel> GetEsbEnvironmentsWithoutApplications(string pathToXmlDataFile);

        Dictionary<string, string> ReconfigureEsbApps(
            string pathToXmlDataFile, IEnumerable<EsbEnvironmentViewModel> esbEnvironments);

        EsbEnvironmentViewModel GetEsbEnvironment(string pathToXmlDataFile, string name);

        void AddEsbEnvironment(string pathToXmlDataFile, EsbEnvironmentViewModel esbEnvironment);

        void UpdateEsbEnvironment(string pathToXmlDataFile, EsbEnvironmentViewModel esbEnvironment);

        void DeleteEsbEnvironment(string pathToXmlDataFile, EsbEnvironmentViewModel esbEnvironment);

        EsbEnvironmentViewModel AssignAppsForEsbEnvironment(
          IEsbEnvironmentDefinition esbEnvironment, IEnumerable<IIconApplication> allIconApplications);

        EsbEnvironmentViewModel AssignAppsForEsbEnvironment(
           IEsbEnvironmentDefinition esbEnvironment, IEnumerable<IconApplicationViewModel> allApplicationViewModels);

        IconApplicationViewModel FindEsbEnvironmentForApp(
            IIconApplication iconApplication, IEnumerable<EsbEnvironmentViewModel> allEsbEnvironments);
    }
}