using System.Collections.Generic;
using System.Xml;
using Icon.Dashboard.DataFileAccess.Models;
using Icon.Dashboard.DataFileGenerator.Models;
using Icon.Dashboard.RemoteServicesAccess;

namespace Icon.Dashboard.DataFileGenerator.Services
{
    public interface IDashboardXmlGenerator
    {
        List<AppModel> AppsDictionary { get; set; }
        string Environment { get; set; }
        List<RemoteServiceModel> Services { get; set; }
        List<IEsbEnvironmentDefinition> EsbEnvironmenets { get; set; }
        XmlDocument GenerateDashboardDataFile();
        XmlDocument GenerateDashboardDataFile(string environment, List<RemoteServiceModel> services, List<AppModel> appsDictionary, List<IEsbEnvironmentDefinition> esbEnvironments);
        void GenerateDashboardDataFileApplications(XmlDocument xmlDoc, XmlElement applicationsElement, string environment, List<RemoteServiceModel> services, List<AppModel> appsDictionary);
        void GenerateDataFileEsbEnviromnment(XmlDocument xmlDoc, XmlElement esbEnvironmentsElement, IEsbEnvironmentDefinition esbEnvironmentModel);
        void GenerateDataFileEsbEnviromnments(XmlDocument xmlDoc, XmlElement esbEnvironmentsElement, List<IEsbEnvironmentDefinition> esbEnvironmentModels);
        string GetLoggingNameForService(RemoteServiceModel service, List<AppModel> appsDictionary);
    }
}