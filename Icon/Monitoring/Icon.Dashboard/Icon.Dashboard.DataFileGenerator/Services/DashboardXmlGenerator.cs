using Icon.Dashboard.DataFileAccess.Models;
using Icon.Dashboard.DataFileGenerator.Models;
using Icon.Dashboard.RemoteServicesAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Icon.Dashboard.DataFileGenerator.Services
{
    public class DashboardXmlGenerator : IDashboardXmlGenerator
    {
        public DashboardXmlGenerator() { }

        public DashboardXmlGenerator(string environment,
            List<RemoteServiceModel> remoteServices,
            List<AppModel> appsDictionary,
            List<IEsbEnvironmentDefinition> esbEnvironments) 
            : this()
        {
            Environment = string.Copy(environment);
            Services = new List<RemoteServiceModel>(remoteServices);
            AppsDictionary = new List<AppModel>(appsDictionary);
            EsbEnvironmenets = new List<IEsbEnvironmentDefinition>(esbEnvironments);
        }

        public string Environment { get; set; }
        public List<RemoteServiceModel> Services { get; set; }
        public List<AppModel> AppsDictionary { get; set; }
        public List<IEsbEnvironmentDefinition> EsbEnvironmenets { get; set; }

        public XmlDocument GenerateDashboardDataFile()
        {
            return GenerateDashboardDataFile(Environment, Services, AppsDictionary, EsbEnvironmenets);
        }

        public XmlDocument GenerateDashboardDataFile(string environment,
            List<RemoteServiceModel> services,
            List<AppModel> appsDictionary,
            List<IEsbEnvironmentDefinition> esbEnvironments)
        {
            var dashboardDataFile = new XmlDocument();

            XmlDeclaration xmlDeclaration = dashboardDataFile.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = dashboardDataFile.DocumentElement;
            dashboardDataFile.InsertBefore(xmlDeclaration, root);

            XmlElement iconApplicationDataElement = dashboardDataFile.CreateElement("IconApplicationData");
            dashboardDataFile.AppendChild(iconApplicationDataElement);

            XmlElement applicationsElement = dashboardDataFile.CreateElement("Applications");
            iconApplicationDataElement.AppendChild(applicationsElement);

            GenerateDashboardDataFileApplications(dashboardDataFile, applicationsElement, environment, services, appsDictionary);

            XmlElement esbEnvironmentsElement = dashboardDataFile.CreateElement("EsbEnvironments");
            iconApplicationDataElement.AppendChild(esbEnvironmentsElement);

            GenerateDataFileEsbEnviromnments(dashboardDataFile, esbEnvironmentsElement, esbEnvironments);

            return dashboardDataFile;
        }

        public void GenerateDashboardDataFileApplications(XmlDocument xmlDoc, XmlElement applicationsElement,
            string environment, List<RemoteServiceModel> services, List<AppModel> appsDictionary)
        {
            foreach (var service in services)
            {
                XmlElement appElement = xmlDoc.CreateElement("Application");

                appElement.SetAttribute("Name", service.FullName);
                appElement.SetAttribute("DisplayName", service.DisplayName);
                appElement.SetAttribute("TypeOfApplication", service.TypeOfApplication);
                appElement.SetAttribute("Server", service.Server);
                appElement.SetAttribute("LoggingName", GetLoggingNameForService(service, appsDictionary));
                appElement.SetAttribute("Environnment", environment);
                appElement.SetAttribute("ConfigFilePath", service.ConfigFilePath);

                applicationsElement.AppendChild(appElement);
            }
        }

        public string GetLoggingNameForService(RemoteServiceModel service, List<AppModel> appsDictionary)
        {
            // look up the icon or mammoth "appName" which corresponds with the service information
            if (service.FullName.Contains("Mammoth"))
            {
                if (service.FullName.Contains("")) { return "Web Api"; }
                else if (service.FullName.Contains("ItemLocale")) { return "ItemLocale Controller"; }
                else if (service.FullName.Contains("PriceController")
                    || service.FullName.Contains("Price.Controller"))
                    { return "Price Controller"; }
                else if (service.FullName.Contains("ApiController")) { return "API Controller"; }
                else if (service.FullName.Contains("ProductListener")) { return "Product Listener"; }
                else if (service.FullName.Contains("LocaleListener")) { return "Locale Listener"; }
                else if (service.FullName.Contains("Hierarchy")) { return "Hierarchy Class Listener"; }
                else if (service.FullName.Contains("PrimeAffinityController")) { return "Prime Affinity Controller"; }
            }
            else // Icon or Infor-named service
            {
                if (service.FullName.Contains("Web")) { return "Web App"; }
                else if (service.FullName.Contains("")) { return "Interface Controller"; }
                else if (service.FullName.Contains("CchTaxListener")
                    || service.FullName.Contains("eWic")
                    || service.FullName.Contains("ItemMovement")
                    || service.FullName.Contains("R10Listener"))
                { return "ESB Subscriber"; }
                else if (service.FullName.Contains("APIController")) { return "API Controller"; }
                else if (service.FullName.Contains("POS Push")) { return "POS Push Controller"; }
                else if (service.FullName.Contains("GlobalEvent")) { return "Global Controller"; }
                else if (service.FullName.Contains("RegionalEvent")) { return "Regional Controller"; }
                else if (service.FullName.Contains("Purge")) { return "Icon Data Purge"; }
                else if (service.FullName.Contains("TLog")) { return "TLog Controller"; }
                else if (service.FullName.Contains("Nutricon")) { return "Nutrition Web API"; }
                else if (service.FullName.Contains("Vim")) { return "Vim Locale Controller"; }
                else if (service.FullName.Contains("Monitor")) { return "Icon Monitoring"; }
                else if (service.FullName.Contains("InforNewItemEvent")) { return "Infor New Item Service"; }
                else if (service.FullName.Contains("InforHierarchyClass")) { return "Infor Hierarchy Class Listener"; }
                else if (service.FullName.Contains("InforItem")) { return "Infor Item Listener"; }
            }

            return string.Empty;
        }

        public void GenerateDataFileEsbEnviromnment(XmlDocument xmlDoc,
            XmlElement esbEnvironmentsElement,
            IEsbEnvironmentDefinition esbEnvironmentModel)
        {
            XmlElement esbEnvironmentElement = xmlDoc.CreateElement("EsbEnvironment");
            esbEnvironmentElement.SetAttribute("Name", esbEnvironmentModel.Name);
            esbEnvironmentElement.SetAttribute("ServerUrl", esbEnvironmentModel.ServerUrl);
            esbEnvironmentElement.SetAttribute("TargetHostName", esbEnvironmentModel.TargetHostName);
            esbEnvironmentElement.SetAttribute("JmsUsername", esbEnvironmentModel.JmsUsername);
            esbEnvironmentElement.SetAttribute("JmsPassword", esbEnvironmentModel.JmsPassword);
            esbEnvironmentElement.SetAttribute("JndiUsername", esbEnvironmentModel.JndiUsername);
            esbEnvironmentElement.SetAttribute("JndiPassword", esbEnvironmentModel.JndiPassword);
            XmlElement applicationsInEsbEnvironmentElement = xmlDoc.CreateElement("ApplicationsInEnvironment");
            esbEnvironmentElement.AppendChild(applicationsInEsbEnvironmentElement);

            esbEnvironmentsElement.AppendChild(esbEnvironmentElement);
        }

        public void GenerateDataFileEsbEnviromnments(XmlDocument xmlDoc,
            XmlElement esbEnvironmentsElement,
            List<IEsbEnvironmentDefinition> esbEnvironments)
        {
            if (esbEnvironments != null && esbEnvironments.Count > 0)
            {
                foreach (var esbEnvironment in esbEnvironments)
                {
                    GenerateDataFileEsbEnviromnment(xmlDoc, esbEnvironmentsElement, esbEnvironment);
                }
            }
        }
    }
}
