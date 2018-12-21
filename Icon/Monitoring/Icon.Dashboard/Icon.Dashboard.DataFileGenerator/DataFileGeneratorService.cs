using Icon.Dashboard.DataFileAccess.Models;
using Icon.Dashboard.DataFileGenerator.Configuration;
using Icon.Dashboard.DataFileGenerator.Models;
using Icon.Dashboard.DataFileGenerator.Services;
using Icon.Dashboard.IconDatabaseAccess;
using Icon.Dashboard.MammothDatabaseAccess;
using Icon.Dashboard.RemoteServicesAccess;
using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Icon.Dashboard.DataFileGenerator
{
    public class DataFileGeneratorService
    {
        public DataFileGeneratorService() { }

        public DataFileGeneratorService(string xmlOutputPath) 
            : this(xmlOutputPath, null, null, null, null, null, null) { }

        public DataFileGeneratorService(
            string xmlOutputPath,
            IConfigReader configReader,
            IIconDbServiceWrapper iconDbService,
            IMammothDbServiceWrapper mammothDbService,
            IAppDictionaryBuilder appDictionaryBuilder,
            IRemoteServicesAccessService wmiRemoteManager,
            IDashboardXmlGenerator dashboardXmlGenerator) : this()
        {
            XmlOutputPath = xmlOutputPath;
            ConfigReader = configReader ?? new ConfigReader();
            IconDb = iconDbService ?? new IconDbServiceWrapper();
            MammothDb = mammothDbService ?? new MammothDbServiceWrapper();
            AppDictionaryBuilder = appDictionaryBuilder ?? new AppDictionaryBuilder(IconDb, MammothDb);
            WmiRemoteManager = wmiRemoteManager ?? new RemoteServicesAccessService();
            DashboardXmlGenerator = dashboardXmlGenerator ?? new DashboardXmlGenerator();
        }

        public string XmlOutputPath { get; set; }
        public IConfigReader ConfigReader { get; set; }
        public IIconDbServiceWrapper IconDb {get; set;}
        public IMammothDbServiceWrapper MammothDb {get; set;}
        public IAppDictionaryBuilder AppDictionaryBuilder {get; set;}
        public IRemoteServicesAccessService WmiRemoteManager {get; set;}
        public IDashboardXmlGenerator DashboardXmlGenerator {get; set;}

        public XmlDocument GenerateDashboardXmlDataFile()
        {
            // 1. Read config data
            ConfigDataModel configData = ConfigReader.ReadAppConfig();

            // 2. query databases for apps logging info
            List<AppModel> appModels = AppDictionaryBuilder.LoadApps();

            // 3. query remote servers for current services
            List<RemoteServiceModel> remoteServices = WmiRemoteManager.LoadRemoteServicesData(configData.Servers, configData.RemoteServiceSearchTerms);

            // 4. use the xml generator to produce a file
            DashboardXmlGenerator.Environment = string.Copy(configData.Environment);
            DashboardXmlGenerator.AppsDictionary = new List<AppModel>(appModels);
            DashboardXmlGenerator.Services = new List<RemoteServiceModel>(remoteServices);
            DashboardXmlGenerator.EsbEnvironmenets = new List<IEsbEnvironmentDefinition>(configData.EsbEnvironments);
            var xmlDoc = DashboardXmlGenerator.GenerateDashboardDataFile();

            // 5. if valid path for output, write xml output
            if (!string.IsNullOrWhiteSpace(XmlOutputPath))
            {
                //TODO validate path
                xmlDoc.Save(XmlOutputPath);
            }

            return xmlDoc;
        }
    }
}
