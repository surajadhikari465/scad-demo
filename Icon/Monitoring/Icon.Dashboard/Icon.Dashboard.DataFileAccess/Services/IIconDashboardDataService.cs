namespace Icon.Dashboard.DataFileAccess.Services
{
    using System;
    using System.Collections.Generic;
    using Icon.Dashboard.DataFileAccess.Models;
    using Icon.Dashboard.DataFileAccess.Constants;

    public interface IIconDashboardDataService
    {
        IEnumerable<ApplicationFactory> ApplicationFactories { get; }

        void VerifyDataFileSchema(string pathToXmlDataFile, string pathToSchema);

        void AddApplication(IIconApplication application, string pathToXmlDataFile);

        void DeleteApplication(IIconApplication application, string pathToXmlDataFile);

        IIconApplication GetApplication(string pathToXmlDataFile, string appName, string server);

        IEnumerable<IIconApplication> GetApplications(string pathToXmlDataFile);

        void UpdateApplication(IIconApplication application, string pathToXmlDataFile);

        void SaveAppSettings(IIconApplication application);

        IEnumerable<IEsbEnvironmentDefinition> GetEsbEnvironments(string pathToXmlDataFile);

        IEsbEnvironmentDefinition GetEsbEnvironment(string pathToXmlDataFile, string name);

        void AddEsbEnvironment(IEsbEnvironmentDefinition esbEnvironment, string pathToXmlDataFile);

        void UpdateEsbEnvironment(IEsbEnvironmentDefinition esbEnvironment, string pathToXmlDataFile);

        void DeleteEsbEnvironment(IEsbEnvironmentDefinition esbEnvironment, string pathToXmlDataFile);

        string StartService(IIconApplication application, TimeSpan waitTime, params string[] args);

        string StopService(IIconApplication application, TimeSpan waitTime);

        string RestartService(IIconApplication app,
            int waitTimeoutMilliseconds = ServiceConstants.DefaultServiceTimeoutMilliseconds);

        KeyValuePair<string, string> ReconfigureEsbSettingsForSingleApp(string pathToXmlDataFile,
            IIconApplication application, IEsbEnvironmentDefinition chosenEsbEnvironment);

        Dictionary<string, string> ReconfigureEsbSettings(string pathToXmlDataFile,
            IEnumerable<IIconApplication> apps, string esbEnvironmentName);

        Dictionary<string, string> ReconfigureEsbSettingsAndRestartServices(string pathToXmlDataFile,
            IEnumerable<IIconApplication> apps,
            string esbEnvironmentName,
            int waitTimeoutMilliseconds = ServiceConstants.DefaultServiceTimeoutMilliseconds);

        Dictionary<string, string> RestartServices(string pathToXmlDataFile,
            IEnumerable<IIconApplication> apps,
            Dictionary<string, string> reconfigResults,
            int waitTimeoutMilliseconds = ServiceConstants.DefaultServiceTimeoutMilliseconds);

        bool HasEsbEnvrionmentChangedForApp(IIconApplication app, string intendedEsbEnvrionmentName);
    }
}