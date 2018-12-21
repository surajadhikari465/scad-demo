using Icon.Dashboard.DataFileAccess.Models;
using Icon.Dashboard.DataFileGenerator.Configuration;
using Icon.Dashboard.DataFileGenerator.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.DataFileGenerator.Services
{
    public class ConfigReader : IConfigReader
    {
        public ConfigDataModel ReadAppConfig()
        {
            var environmentFromConfig = ConfigurationManager.AppSettings["Environment"];

            var serversFromConfig = ConfigurationManager.AppSettings["RemoteServers"];
            var serverList = serversFromConfig.Split(new char[] { ',' }).ToList();

            var remoteAppNamesList = ConfigurationManager.AppSettings["RemoteServiceSearchTerms"];
            var remoteServicesSearchTerms = remoteAppNamesList.Split(new char[] { ',' }).ToList();

            EsbSection esbConfigSettings = EsbSection.GetConfig();

            List<IEsbEnvironmentDefinition> esbEnvironments = new List<IEsbEnvironmentDefinition>(esbConfigSettings.EsbEnvironments.Count);
            for (int i=0; i< esbConfigSettings.EsbEnvironments.Count; i++)
            {
                esbEnvironments.Add(esbConfigSettings.EsbEnvironments[i]);
            }

            var configData = new ConfigDataModel(environmentFromConfig, serverList, remoteServicesSearchTerms, esbEnvironments);
            return configData;
        }
    }
}
