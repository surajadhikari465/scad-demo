using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Icon.Dashboard.Mvc.Services
{
    public interface IExternalConfigXmlManager
    {
        string ExternalConfigFilePath { get; set; }
        bool IsFilePathVerified { get; }
        RemoteServiceConfigDataModel ReadConfigData(
            List<EnvironmentModel> possibleEnvironments,
            List<EsbEnvironmentModel> possibleEsbEnvironments);
        RemoteServiceConfigDataModel ReadConfigData(
            string configFilePath,
            List<EnvironmentModel> possibleEnvironments,
            List<EsbEnvironmentModel> possibleEsbEnvironments);
        List<KeyValuePair<string,string>> AppSettingsElementsToKeyValuePairs(
            IEnumerable<XElement> appSettingElements);
        int ReadNlogAppIdParameter(XDocument appConfig);
        DatabaseModel BuildDatabaseDefinitionFromConfigElement(
            ConnectionStringModel csElement,
            List<EnvironmentModel> possibleEnvironments);
        DbConfigurationModel ReadDatabaseConfiguration(
            XDocument configXmlDoc,
            List<EnvironmentModel> possibleEnvironments);
        EnvironmentEnum DetermineDatabaseEnvironment(
            DatabaseCategoryEnum category,
            string serverName,
            string databaseName,
            List<EnvironmentModel> possibleEnvironments);
        void UpdateExternalAppSettings(
            string externalConfigFilePath,
            Dictionary<string, string> updatedSettingsDictionary);
        void UpdateExternalAppSettings(Dictionary<string, string> updatedSettingsDictionary);
        bool ReconfigureEsbEnvironmentCustomConfigSection(
            string remoteServiceConfigPath,
            string connectionName,
            Dictionary<string, string> updatedSettingsDictionary);
    }
}