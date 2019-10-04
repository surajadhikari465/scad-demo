using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Helpers;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;

namespace Icon.Dashboard.Mvc.Services
{
    public class ExternalConfigXmlManager : IExternalConfigXmlManager
    {
        public ExternalConfigXmlManager() { }
        public ExternalConfigXmlManager(string externalConfigFilePath)
        {
            ExternalConfigFilePath = externalConfigFilePath;
        }

        public string ExternalConfigFilePath { get; set; }

        /// <summary>
        /// Contains all app settings from the config file (including any ESB-related settings0
        /// </summary>
        public Dictionary<string, string> AppSettingsDictionary { get; set; }

        public bool IsFilePathVerified { get; protected set; }

        protected XDocument LoadConfigAsXmlDoc(string externalConfigFilePath,
            string activityForErrorMessage = "open external config file as xml")
        {
            if (Utils.VerifyExternalFilePath(externalConfigFilePath, activityForErrorMessage))
            {
                IsFilePathVerified = true;
                return XDocument.Load(externalConfigFilePath);
            }
            return null;
        }

        protected XElement ReadAppSettingsRootElementFromXmlDoc(XDocument xDocument)
        {
            return xDocument.Root.Element("appSettings");
        }

        protected IEnumerable<XElement> ReadAppSettingsElementsFromXmlDoc(XDocument xDocument)
        {
            var appSettingsElement = ReadAppSettingsRootElementFromXmlDoc(xDocument);
            return appSettingsElement?.Elements() ?? new XElement[0];
        }

        public RemoteServiceConfigDataModel ReadConfigData(List<EnvironmentModel> possibleEnvironments, List<EsbEnvironmentModel> possibleEsbEnvironments)
        {
            return ReadConfigData(this.ExternalConfigFilePath, possibleEnvironments, possibleEsbEnvironments);
        }

        public RemoteServiceConfigDataModel ReadConfigData(string configFilePath,
            List<EnvironmentModel> possibleEnvironments,
            List<EsbEnvironmentModel> possibleEsbEnvironments)
        {
            var configData = new RemoteServiceConfigDataModel();
            configData.ConfigFilePath = configFilePath;

            // load the application's config file as xml
            var configXmlDoc = LoadConfigAsXmlDoc(configFilePath);
            if (configXmlDoc != null)
            {
                configData.IsConfigFilePathValid = IsFilePathVerified;
                // read config app settings as a list of key/value pairs
                var appSettingsElements = ReadAppSettingsElementsFromXmlDoc(configXmlDoc);
                var appSettingsKvpList = AppSettingsElementsToKeyValuePairs(appSettingsElements);
                // read all the app settings to a dictionary
                SetAppSettingsDictionary(appSettingsKvpList);
                // set the config data model's app settings to contain all non-ESB settings
                configData.NonEsbAppSettings = FilterAppSettingsForNonEsbSettings(this.AppSettingsDictionary);

                // read the service's database logging id from the nLog xml section
                configData.LoggingID = ReadNlogAppIdParameter(configXmlDoc);

                // read esb connection data
                // (esb connection settings can either be in conventional app settings
                // key /value pairs or in a custom <esbEnvironment> xml element)
                if (DictionaryContainsEsbServerKey(this.AppSettingsDictionary))
                {
                    // set esb connection configuration from app settings (if any)
                    var esbConnection = BuildEsbConnectionModelFromSettingsDictionary(this.AppSettingsDictionary, possibleEsbEnvironments);
                    if (esbConnection != null)
                    {
                        configData.EsbConnections.Add(esbConnection);
                    }
                }
                else
                {
                    // if no esb app settings were detected, check for a custom esb config section 
                    var esbSettingsFromEsbConfigSection = GetEsbConnectionsDictionariesFromXmlEsbElements(configXmlDoc);
                    if (esbSettingsFromEsbConfigSection != null && esbSettingsFromEsbConfigSection.Count > 0)
                    {
                        var esbConnections = BuildEsbConnectionModelsFromCustomConfigDictionaries(esbSettingsFromEsbConfigSection, possibleEsbEnvironments);
                        if (esbConnections != null && esbConnections.Count > 0)
                        {
                            configData.HasEsbSettingsInCustomConfigSection = true;
                            configData.EsbConnections.AddRange(esbConnections);
                        }
                    }
                }

                // read database configuration from config connection strings
                configData.DatabaseConfiguration = ReadDatabaseConfiguration(configXmlDoc, possibleEnvironments);
            }
            return configData;
        }

        public void SetAppSettingsDictionary(List<KeyValuePair<string, string>> appSettingsKvpList)
        {
            this.AppSettingsDictionary = appSettingsKvpList?.GroupBy(x => x.Key).Select(g => g.First())?
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ?? new Dictionary<string, string>();
        }

        public List<KeyValuePair<string, string>> AppSettingsElementsToKeyValuePairs(IEnumerable<XElement> appSettingsElements)
        {
            return appSettingsElements?
                  .Select(e => new KeyValuePair<string, string>(
                      e.Attribute("key").Value,
                      e.Attribute("value").Value))?.ToList()
                      ?? new List<KeyValuePair<string, string>>();
        }

        public List<EsbConnectionViewModel> BuildEsbConnectionModelsFromCustomConfigDictionaries(List<Dictionary<string, string>> esbConfigSections, List<EsbEnvironmentModel> possibleEsbEnvironments)
        {
            var esbConnections = new List<EsbConnectionViewModel>();
            if (esbConfigSections != null && esbConfigSections.Count > 0)
            {
                foreach (var esbDictionary in esbConfigSections)
                {
                    var esbConnectionModel = BuildEsbConnectionModelFromSettingsDictionary(esbDictionary, possibleEsbEnvironments);
                    if (esbConnectionModel != null)
                    {
                        esbConnections.Add(esbConnectionModel);
                    }
                }
            }
            return esbConnections;
        }

        public EsbConnectionViewModel BuildEsbConnectionModelFromSettingsDictionary(Dictionary<string, string> appSettings, List<EsbEnvironmentModel> possibleEsbEnvironments)
        {
            if (appSettings != null)
            {
                var esbSettingsDictionary = FilterAppSettingsForOnlyEsbSettings(appSettings);
                if (esbSettingsDictionary != null && esbSettingsDictionary.Count > 0)
                {
                    var esbConnection = new EsbConnectionViewModel(esbSettingsDictionary);
                    esbConnection.EnvironmentEnum = DetermineEsbEnvironmentFromServerUrlSetting(possibleEsbEnvironments, esbConnection.ServerUrl);
                    return esbConnection;
                }
            }
            return null;
        }

        public static Dictionary<string, string> FilterAppSettingsForNonEsbSettings(
            Dictionary<string, string> allAppSettings)
        {
            return allAppSettings?
                .Where(s => !Constants.EsbSettingKeys.KeyList.Contains(s.Key, StringComparer.OrdinalIgnoreCase))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                ?? new Dictionary<string, string>();
        }

        public static Dictionary<string, string> FilterAppSettingsForOnlyEsbSettings(
          Dictionary<string, string> allAppSettings)
        {
            return allAppSettings?
                .Where(s => Constants.EsbSettingKeys.KeyList.Contains(s.Key, StringComparer.OrdinalIgnoreCase))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                ?? new Dictionary<string, string>();
        }

        public static bool DictionaryContainsEsbServerKey(Dictionary<string, string> dictionary)
        {
            if (dictionary != null)
            {
                var caseInsensitiveDictionary = new Dictionary<string, string>(dictionary, StringComparer.CurrentCultureIgnoreCase);
                return caseInsensitiveDictionary.ContainsKey(Constants.EsbSettingKeys.ServerUrlKey);
            }

            return false;
        }

        public int ReadNlogAppIdParameter(XDocument configXmlDoc)
        {
            int loggingId = 0;

            try
            {
                var nlogElement = configXmlDoc.Root.Elements()
                        .FirstOrDefault(rootEl => rootEl.Name.ToString().Contains("nlog"));
                if (nlogElement != null)
                {
                    var appIdParameter = nlogElement.Descendants()
                        .FirstOrDefault(d => d.Name.ToString().Contains("parameter") && d.Attribute("name").Value == "@AppId");
                    if (appIdParameter != null)
                    {
                        int.TryParse(appIdParameter.Attribute("layout").Value, out loggingId);
                    }
                }
            }
            catch (Exception)
            {
                loggingId = -1;
            }
            return loggingId;
        }

        internal IEnumerable<XElement> GetEsbConnectionXmlElements(XDocument configXmlDoc)
        {
            var esbConnectionsRootElement = configXmlDoc.Root.Elements()
                .FirstOrDefault(rootEl => rootEl.Name.ToString().Contains("esbConnections"));
            if (esbConnectionsRootElement != null)
            {
                return esbConnectionsRootElement.Descendants()
                    .Where(d => d.Name.ToString().Equals("esbConnection"));
            }
            return new XElement[0];
        }

        internal string GetAttributeValue(IEnumerable<XAttribute> xAttributes, string attributeName)
        {
            return xAttributes?.FirstOrDefault(a => a.Name.ToString().Equals(attributeName, Utils.StrcmpOption))?.Value;
        }

        public List<Dictionary<string, string>> GetEsbConnectionsDictionariesFromXmlEsbElements(XDocument configXmlDoc)
        {
            var esbConnectionDictionaries = new List<Dictionary<string, string>>();

            var esbConnectionElements = GetEsbConnectionXmlElements(configXmlDoc);
            if (esbConnectionElements != null)
            {
                foreach (var esbConnection in esbConnectionElements)
                {
                    var connectionDictionary = new Dictionary<string, string>();
                    foreach (var attribute in esbConnection.Attributes())
                    {
                        if (!string.IsNullOrWhiteSpace(attribute.Name.ToString()))
                        {
                            // capitalize first letter
                            var lowercaseName = attribute.Name.ToString();
                            var name = $"{char.ToUpper(lowercaseName[0])}{lowercaseName.Substring(1)}";
                            var value = attribute.Value.ToString();

                            connectionDictionary.Add(name, value);
                        }
                    }
                    esbConnectionDictionaries.Add(connectionDictionary);
                }
            }
            return esbConnectionDictionaries;
        }

        public static EsbEnvironmentEnum DetermineEsbEnvironmentFromServerUrlSetting(
          List<EsbEnvironmentModel> esbEnvironments,
          string serverUrlKeyValue)
        {
            if (esbEnvironments != null && !string.IsNullOrWhiteSpace(serverUrlKeyValue))
            {
                var hostsForApp = Utils.SplitHostsFromServerUrlSetting(serverUrlKeyValue);
                if (hostsForApp != null)
                {
                    foreach (var host in hostsForApp)
                    {
                        var esbEnvWithServer = esbEnvironments.SingleOrDefault(e =>
                                e.ServerUrls.Any(u => u.ContainsCaseInsensitve(host)));
                        if (esbEnvWithServer != null)
                        {
                            return esbEnvWithServer.EsbEnvironment;
                        }
                    }
                }
            }
            return EsbEnvironmentEnum.None;
        }

        public DbConfigurationModel ReadDatabaseConfiguration(XDocument configXmlDoc, List<EnvironmentModel> possibleEnvironments)
        {
            var dbConfig = new DbConfigurationModel();

            if (configXmlDoc?.Root?.Element("connectionStrings")?.Elements()?
                .FirstOrDefault(e => e.Name.ToString().EndsWith("EncryptedData")) != null)
            {
                // encrypted connection string
                var encryptedDb = new DatabaseModel
                {
                    ServerName = "{Encrypted}",
                    DatabaseName = "{Encrypted}",
                    ConnectionStringName = "{Encrypted}",
                    Environment = EnvironmentEnum.Undefined,
                    Category = DatabaseCategoryEnum.Encrypted
                };
                dbConfig.Connections.Add(encryptedDb);
            }
            else
            {
                var connectionStringElements = configXmlDoc?.Root?.Element("connectionStrings")?.Elements()?
                        .Select(e => new ConnectionStringModel(
                            name: e.Attribute("name")?.Value,
                            providerName: e.Attribute("providerName")?.Value,
                            connectionString: e.Attribute("connectionString")?.Value));
                if (connectionStringElements != null)
                {
                    foreach (var csElement in connectionStringElements)
                    {
                        var db = BuildDatabaseDefinitionFromConfigElement(csElement, possibleEnvironments);
                        dbConfig.Connections.Add(db);
                    }
                }
            }

            var nlogElement = configXmlDoc.Root.Elements()
                .FirstOrDefault(rootEl => rootEl.Name.ToString().Contains("nlog"));
            if (nlogElement != null)
            {
                // element name may have namespace preceding like "xsi:type" or "{{http://www.w3.org/2001/04/xmlenc#}Name}"
                var targetElement = nlogElement.Descendants().FirstOrDefault(e => e.Name.ToString().EndsWith("target"));
                if (targetElement != null)
                {
                    if (targetElement.Attributes().Any(a => a.Name.ToString().EndsWith("type"))
                        && targetElement.Attributes().FirstOrDefault(a => a.Name.ToString().EndsWith("type")).Value == "Database"
                        && targetElement.Attributes().Any(a => a.Name == "name")
                        && targetElement.Attributes().Any(a => a.Name == "connectionString"))
                    {
                        var loggingCsElement = new ConnectionStringModel(
                            name: targetElement.Attribute("name").Value,
                            providerName: "SqlClient",
                            connectionString: targetElement.Attribute("connectionString").Value
                        );
                        var loggingDb = BuildDatabaseDefinitionFromConfigElement(loggingCsElement, possibleEnvironments);
                        loggingDb.IsUsedForLogging = true;
                        dbConfig.Connections.Add(loggingDb);
                    }
                }
            }
            return dbConfig;
        }

        public DatabaseModel BuildDatabaseDefinitionFromConfigElement(
            ConnectionStringModel csElement,
            List<EnvironmentModel> possibleEnvironments)
        {
            var db = new DatabaseModel();

            var sqlStringBuilder = new SqlConnectionStringBuilder();
            if (!string.IsNullOrWhiteSpace(csElement.ProviderName) && csElement.ProviderName.Contains("EntityClient"))
            {
                db.IsEntityFramework = true;
                var efStringBuilder = new EntityConnectionStringBuilder();
                try
                {
                    efStringBuilder.ConnectionString = csElement.ConnectionString;
                    sqlStringBuilder.ConnectionString = efStringBuilder.ProviderConnectionString;
                }
                catch (ArgumentException)
                {
                    // the connection string may say it is using EntityClient but not have a valid EF connection string,
                    //  in which case just use the conection string as-is
                    sqlStringBuilder.ConnectionString = csElement.ConnectionString;
                }
            }
            else
            {
                db.IsEntityFramework = false;
                sqlStringBuilder.ConnectionString = csElement.ConnectionString;
            }

            db.ServerName = sqlStringBuilder.DataSource;
            db.DatabaseName = sqlStringBuilder.InitialCatalog;
            db.ConnectionStringName = csElement.Name;

            if (csElement.Name.IndexOf("Icon", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                db.Category = DatabaseCategoryEnum.Icon;
            }
            else if (csElement.Name.IndexOf("Mammoth", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                db.Category = DatabaseCategoryEnum.Mammoth;
            }
            else if (csElement.Name.IndexOf("ItemCatalog", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                db.Category = DatabaseCategoryEnum.IRMA;
            }
            else if (csElement.Name.Equals("Vim", StringComparison.InvariantCultureIgnoreCase))
            {
                db.Category = DatabaseCategoryEnum.Vim;
            }
            else if (csElement.Name.StartsWith("dbLog"))
            {
                if (db.DatabaseName.IndexOf("Icon", StringComparison.InvariantCultureIgnoreCase) > -1)
                {
                    db.Category = DatabaseCategoryEnum.Icon;
                }
                else if (db.DatabaseName.IndexOf("Mammoth", StringComparison.InvariantCultureIgnoreCase) > -1)
                {
                    db.Category = DatabaseCategoryEnum.Mammoth;
                }
                else if (db.DatabaseName.IndexOf("ItemCatalog", StringComparison.InvariantCultureIgnoreCase) > -1)
                {
                    db.Category = DatabaseCategoryEnum.IRMA;
                }
            }
            else
            {
                db.Category = DatabaseCategoryEnum.Other;
            }

            db.Environment = DetermineDatabaseEnvironment(db.Category, db.ServerName, db.DatabaseName, possibleEnvironments);

            return db;
        }

        public EnvironmentEnum DetermineDatabaseEnvironment(DatabaseCategoryEnum category,
            string serverName,
            string databaseName,
            List<EnvironmentModel> possibleEnvironments)
        {
            var env = EnvironmentEnum.Undefined;
            EnvironmentModel matchingEnvironment = null;

            switch (category)
            {
                case DatabaseCategoryEnum.Icon:
                    matchingEnvironment = possibleEnvironments
                        .FirstOrDefault(e =>
                            e.IconDatabaseServer.Equals(serverName, Utils.StrcmpOption)
                            && e.IconDatabaseName.Equals(databaseName, Utils.StrcmpOption));
                    break;
                case DatabaseCategoryEnum.Mammoth:
                    matchingEnvironment = possibleEnvironments
                        .FirstOrDefault(e =>
                            e.MammothDatabaseServer.Equals(serverName, Utils.StrcmpOption)
                            && e.MammothDatabaseName.Equals(databaseName, Utils.StrcmpOption));
                    break;
                case DatabaseCategoryEnum.IRMA:
                    matchingEnvironment = possibleEnvironments
                        .FirstOrDefault(e =>
                            e.IrmaDatabaseServers.FindIndex(s=>s.Equals(serverName, Utils.StrcmpOption))!=-1
                            && e.IrmaDatabaseName.Equals(databaseName, Utils.StrcmpOption));
                    break;
                case DatabaseCategoryEnum.Vim:
                case DatabaseCategoryEnum.Encrypted:
                case DatabaseCategoryEnum.Other:
                case DatabaseCategoryEnum.Unknown:
                default:
                    break;
            }

            if (matchingEnvironment!=null)
            {
                return matchingEnvironment.EnvironmentEnum;
            }

            return env;
        }

        public void UpdateExternalAppSettings(Dictionary<string, string> updatedSettingsDictionary)
        {
            UpdateExternalAppSettings(ExternalConfigFilePath, updatedSettingsDictionary);
        }

        public void UpdateExternalAppSettings(string externalConfigFilePath, Dictionary<string, string> updatedSettingsDictionary)
        {
            if (Utils.VerifyExternalFilePath(externalConfigFilePath, "update external app settings as xml", false))
            {
                // create XML elements for each setting
                var updatedElements = updatedSettingsDictionary.Select(i =>
                        new XElement("add",
                            new XAttribute("key", i.Key),
                            new XAttribute("value", i.Value ?? String.Empty)));

                // read the file to an xml document
                var configXmlDoc = LoadConfigAsXmlDoc(externalConfigFilePath, "update external app settings as xml");
                var appSettingsElement = ReadAppSettingsRootElementFromXmlDoc(configXmlDoc);

                foreach(var updatedSetting in updatedSettingsDictionary)
                {
                    var existingElement = appSettingsElement.Elements()
                        .FirstOrDefault(el => el.Attribute("key").Value.Equals(updatedSetting.Key, Utils.StrcmpOption));
                    if (existingElement != null)
                    {
                        if (!existingElement.Attribute("value").Value.Equals(updatedSetting.Value, Utils.StrcmpOption))
                        {
                            existingElement.Attribute("value").SetValue(updatedSetting.Value);
                        }
                    }
                }

                configXmlDoc.Save(externalConfigFilePath);
            }
        }

        public bool UpdateEsbElementAttributes(string externalConfigFilePath, XDocument configXmlDoc, XElement esbConnectionElement, Dictionary<string, string> updatedSettingsDictionary)
        {
            if (configXmlDoc != null && esbConnectionElement != null)
            {
                foreach (var connectionAttribute in esbConnectionElement.Attributes())
                {
                    var attributeName = connectionAttribute.Name.ToString();
                    if (updatedSettingsDictionary.ContainsKey(attributeName))
                    {
                        esbConnectionElement.Attribute(attributeName).SetValue(updatedSettingsDictionary[attributeName]);
                    }
                }
                configXmlDoc.Save(externalConfigFilePath);
                return true;
            }
            return false;
        }

        public bool ReconfigureEsbEnvironmentCustomConfigSection(
            string remoteServiceConfigPath,
            string connectionName,
            Dictionary<string, string> updatedSettingsDictionary)
        {
            var configXmlDoc = LoadConfigAsXmlDoc(remoteServiceConfigPath);
            if (configXmlDoc != null)
            {
                var currentEsbData = GetEsbConnectionsDictionariesFromXmlEsbElements(configXmlDoc);
                if (currentEsbData!=null)
                {
                    foreach(var currentEsbSettingsDictionary in currentEsbData)
                    {
                        if (!currentEsbSettingsDictionary.TryGetValue("name", out string name))
                        {
                            currentEsbSettingsDictionary.TryGetValue("Name", out name);
                        }
                        if (!string.IsNullOrWhiteSpace(name) && name.Equals(connectionName, Utils.StrcmpOption))
                        {
                            var esbConnectionElements = GetEsbConnectionXmlElements(configXmlDoc);
                            if (esbConnectionElements != null)
                            {
                                foreach (var esbConnectionElement in esbConnectionElements)
                                {
                                    var connectionElementName = GetAttributeValue(esbConnectionElement.Attributes(), "name");
                                    if (!string.IsNullOrWhiteSpace(connectionElementName) && name.Equals(connectionName, Utils.StrcmpOption))
                                    {
                                        var saved = UpdateEsbElementAttributes(remoteServiceConfigPath, configXmlDoc, esbConnectionElement, updatedSettingsDictionary);
                                        if (saved) return true;
                                    }                        
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}