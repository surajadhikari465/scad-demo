namespace Icon.Dashboard.DataFileAccess.Services
{
    using System;
    using System.Collections.Generic;
    using Icon.Dashboard.DataFileAccess.Models;

    public interface IIconDashboardDataService
    {
        IEnumerable<ApplicationFactory> ApplicationFactories { get; }
        void VerifyDataFileSchema(string pathToXmlDataFile, string pathToSchema);

        void AddApplication(IApplication application, string pathToXmlDataFile);
        void DeleteApplication(IApplication application, string pathToXmlDataFile);
        IApplication GetApplication(string pathToXmlDataFile, string appName, string server);
        IEnumerable<IApplication> GetApplications(string pathToXmlDataFile, EnvironmentEnum environment);
        void UpdateApplication(IApplication application, string pathToXmlDataFile);
        void SaveAppSettings(IApplication application);
        void Start(IApplication application, params string[] args);
        void Stop(IApplication application);

        IEnumerable<IEsbEnvironment> GetEsbEnvironments(string pathToXmlDataFile);
        IEsbEnvironment GetEsbEnvironment(string pathToXmlDataFile, string name);
        void AddEsbEnvironment(IEsbEnvironment esbEnvironment, string pathToXmlDataFile);
        void UpdateEsbEnvironment(IEsbEnvironment esbEnvironment, string pathToXmlDataFile);
        void DeleteEsbEnvironment(IEsbEnvironment esbEnvironment, string pathToXmlDataFile);
        List<Tuple<bool, string>> UpdateApplicationsToEsbEnvironment(IEsbEnvironment esbEnvironment, string pathToXmlDataFile);
        Tuple<bool, string> UpdateApplicationEsbEnvironmentAndRestart(IconApplicationIdentifier applicationIdentifier, IEsbEnvironment esbEnvironment, string pathToXmlDataFile);

        /// <summary>
        /// Searches the data file to find whether one of the esb envrionments defined in the data 
        ///   has every ESB-using application configured to use the server associated with the 
        ///   esb environment definition
        /// </summary>
        /// <param name="pathToXmlDataFile">ICON Dashboard data file</param>
        /// <returns>Returns an object implementing IEsbEnvrionment. Or null if none of the environments has all its applications
        /// configured for the same esb environment.</returns>
        IEsbEnvironment GetCurrentEsbEnvironment(string pathToXmlDataFile);
    }
}