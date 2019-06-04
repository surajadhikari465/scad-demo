using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.ViewModels;
using Icon.Dashboard.RemoteServicesAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Xml.Linq;
using System.Data.Common;
using System.Data.EntityClient;
using System.Data.SqlClient;

namespace Icon.Dashboard.Mvc.Services
{
    public class RemoteWmiServiceWrapper : IRemoteWmiServiceWrapper
    {
        public IRemoteWmiAccessService WmiService { get;  set; }

        public IIconDatabaseServiceWrapper IconDbService { get; set; }
        public IMammothDatabaseServiceWrapper MammothDbService { get; set; }
        public IEsbEnvironmentManager EsbEnvironmentManager { get; set; }

        public bool MammothDbEnabled { get; set; }

        public RemoteWmiServiceWrapper(
            bool useMammothDb = false,
            IIconDatabaseServiceWrapper iconDbService = null,
            IMammothDatabaseServiceWrapper mammothDbService = null,
            IEsbEnvironmentManager esbEnvironmentManager = null)
       : this(useMammothDb, null, iconDbService, mammothDbService, esbEnvironmentManager) { }

        public RemoteWmiServiceWrapper(
            bool useMammothDb = false,
            IRemoteWmiAccessService dataService = null,
            IIconDatabaseServiceWrapper iconDbService = null,
            IMammothDatabaseServiceWrapper mammothDbService = null,
            IEsbEnvironmentManager esbEnvironmentManager = null)
        {
            this.MammothDbEnabled = useMammothDb;
            this.WmiService = dataService ?? new RemoteWmiAccessService();
            this.IconDbService = iconDbService ?? new IconDatabaseServiceWrapper();
            this.MammothDbService = this.MammothDbEnabled ? mammothDbService ?? new MammothDatabaseServiceWrapper() : null;
            this.EsbEnvironmentManager = esbEnvironmentManager ?? new EsbEnvironmentManager();
        }

        public IconApplicationViewModel LoadRemoteService(string server, string application, bool commandsEnabled)
        {
            var remoteServiceModel = WmiService.LoadRemoteService(server, application);

            return CreateViewModel(server, remoteServiceModel, commandsEnabled);
        }

        public IconApplicationViewModel CreateViewModel(string serverName,
           RemoteServiceModel remoteService,
           bool commandsEnabled)
        {
            var iconApps = IconDbService.GetApps();
            var mammothApps = this.MammothDbEnabled ? MammothDbService.GetApps() : new List<IconLoggedAppViewModel>();
            var esbEnvironments = EsbEnvironmentManager.GetEsbEnvironmentDefinitions();

            var appViewModel = new IconApplicationViewModel()
            {
                Name = remoteService.FullName,
                DisplayName = remoteService.DisplayName,
                Server = serverName,
                Description = remoteService.Description,
                Family = remoteService.FullName.Contains("Mammoth") ? "Mammoth" : "Icon",
                ConfigFilePath = remoteService.ConfigFilePath,
                CommandsEnabled = commandsEnabled,
                Status = remoteService.State,
                ValidCommands = SetValidCommands(remoteService.State),
                StatusIsGreen = IsStatusGreen(remoteService.State),
                LoggingID = 0,
                LoggingName = "",
                AppSettings = new Dictionary<string, string>(),
                EsbConnectionSettings = new Dictionary<string, string>(),
                HostName = remoteService.SystemName,
                AccountName = remoteService.RunningAs.Replace(@"wfm\", "").Replace(@"@wfm.pvt", "")
            };

            try
            {
                ;
                if (IsRunningAsUnitTest())
                {
                    // testing or loading service from local machine, just use filename without path
                    appViewModel.ConfigFilePath = Path.GetFileName(appViewModel.ConfigFilePath);
                }
                else
                {
                    appViewModel.ConfigFilePath = GetConfigUncPath(appViewModel.Server, remoteService.ConfigFilePath);
                }

                if (System.IO.File.Exists(appViewModel.ConfigFilePath))
                {
                    appViewModel.ConfigFilePathIsValid = true;
                    // load the application's config file
                    var appConfig = XDocument.Load(appViewModel.ConfigFilePath);

                    //populate app and esb settings from config file
                    var allAppSettings = appConfig.Root.Element("appSettings").Elements()
                        .Select(e => new
                        {
                            Key = e.Attribute("key").Value,
                            Value = e.Attribute("value").Value
                        });

                    var nonEsbSettings = allAppSettings
                        .Where(s => !EsbAppSettings.EsbAppSettingsNames.Contains(s.Key))
                        .ToList();
                    nonEsbSettings.ForEach(e => appViewModel.AppSettings.Add(e.Key, e.Value));

                    var esbEnvironmentSettings = allAppSettings
                        .Where(s => EsbAppSettings.EsbAppSettingsNames.Contains(s.Key))
                        .ToList();
                    esbEnvironmentSettings.ForEach(e => appViewModel.EsbConnectionSettings.Add(e.Key, e.Value));

                    appViewModel.CurrentEsbEnvironment = FindEsbEnvironmentForApp(esbEnvironments, appViewModel.EsbConnectionSettings);

                    // populate logging id and name from config file and database App data 
                    if (appViewModel.Family.Contains("Mammoth"))
                    {
                        appViewModel.LoggingID = GetLoggingIdFromConfig(appConfig);
                        if (appViewModel.LoggingID.GetValueOrDefault(0) > 0)
                        {
                            appViewModel.LoggingName = GetLoggingNameFromId(mammothApps, appViewModel.LoggingID.Value);
                        }
                    }
                    else
                    {
                        appViewModel.LoggingID = GetLoggingIdFromConfig(appConfig);
                        if (appViewModel.LoggingID.GetValueOrDefault(0) > 0)
                        {
                            appViewModel.LoggingName = GetLoggingNameFromId(iconApps, appViewModel.LoggingID.Value);
                        }
                    }

                    // load database configuration
                    var dbInfo = ReadDatabaseConfiguration(appViewModel.ConfigFilePath);
                    appViewModel.DatabaseConfiguration = new AppDatabaseConfigurationViewModel(dbInfo);
                }
                else
                {
                    appViewModel.ConfigFilePathIsValid = false;
                }
            }
            catch (Exception ex)
            {
                // eat error for now
                string errMsg = ex.Message;
            }
            return appViewModel;
        }

        /// <summary>
        /// Converts PathName from a WMI ManagementObject into a UNC path to a configuration file
        /// </summary>
        /// <param name="serverName">The remote server name for the start of the UNC path</param>
        /// <param name="pathName">PathName from a WMI Management Object representing a remote service, for example
        ///     "E:\Icon\API Controller Phase 2\Hierarchy\Icon.ApiController.Controller.exe"  -displayname "Icon API Controller - Hierarchy" -servicename "IconAPIController-Hierarchy"</param>
        /// <returns>UNC path to the config file, for example
        ///     "\\vm-icon-test1\E$\Icon\API Controller Phase 2\Hierarchy\Icon.ApiController.Controller.exe.config" </returns>
        public string GetConfigUncPath(string serverName, string pathName)
        {
            // remove quotation marks
            string configPathOnHost = pathName.Replace("\"", "");
            if (configPathOnHost.Contains("-"))
            {
                // remove any parameters (we only want the path to the executable)
                configPathOnHost = configPathOnHost.Substring(0, configPathOnHost.IndexOf('-'));
            }
            // replace local drive letter from remote server with an administrative share path ($ instead of :)
            configPathOnHost = configPathOnHost.Replace(":", "$");
            // trim and add .config suffix
            configPathOnHost = configPathOnHost.Trim();
            configPathOnHost += ".config";

            // build the unc path using the server and converted path
            configPathOnHost = $"\\\\{serverName}\\{configPathOnHost}";

            return configPathOnHost;
        }

        public bool IsRunningAsUnitTest()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Any(a => a.FullName.StartsWith("Microsoft.VisualStudio.QualityTools.UnitTestFramework"));
        }

        public List<IconApplicationViewModel> LoadRemoteServices(DashboardEnvironmentViewModel customEnvironment, bool commandsEnabled)
        {
            var appViewModels = new List<IconApplicationViewModel>();

            foreach (var serverName in customEnvironment.AppServers.Select(s => s.ServerName))
            {
                var remoteServiceModels = WmiService.LoadRemoteServices(serverName);

                appViewModels.AddRange(remoteServiceModels.Select(s =>
                    CreateViewModel(serverName, s, commandsEnabled)));
            }

            return appViewModels;
        }

        public bool IsStatusGreen(string state)
        {
            switch (state)
            {
                case "Running":
                    return true;
                case "Stopped":
                case "StartPending":
                case "StopPending":
                case "ContinuePending":
                case "PausePending":
                case "Paused":
                default:
                    break;
            }
            return false;
        }

        public List<string> SetValidCommands(string state)
        {
            var validCommands = new List<string>();
            switch (state)
            {
                case "Running":
                    validCommands.Add("Stop");
                    break;
                case "Stopped":
                    validCommands.Add("Start");
                    break;
                case "StartPending":
                case "StopPending":
                case "ContinuePending":
                case "PausePending":
                case "Paused":
                    validCommands.Add("Start");
                    validCommands.Add("Stop");
                    break;
                case "Undefined":
                default:
                    break;
            }
            return validCommands;
        }

        public string GetLoggingNameFromId(IEnumerable<IconLoggedAppViewModel> apps, int loggingId)
        {
            var loggingName = string.Empty;
            var matchingApp = apps.FirstOrDefault(a => a.AppID == loggingId);
            if (matchingApp != null)
            {
                loggingName = matchingApp.AppName;
            }
            return loggingName;
        }

        public int GetLoggingIdFromConfig(XDocument appConfig)
        {
            int loggingId = 0;

            try
            {
                var nlogElement = appConfig.Root.Elements()
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

        public void ExecuteServiceCommand(string server, string application, string command)
        {
            switch (command)
            {
                case "Start":
                case "start":
                    WmiService.StartRemoteService(server, application, new string[] { });
                    break;
                case "Stop":
                case "stop":
                    WmiService.StopRemoteService(server, application, new string[] { });
                    break;
                default:
                    throw new ArgumentException($"Unexpected command '{command}' for ExecuteServiceCommand");
            }
        }

        public void RestartServices(IEnumerable<EsbEnvironmentViewModel> esbEnvironments)
        {
            foreach (var esbEnvironment in esbEnvironments)
            {
                RestartServices(esbEnvironment.AppsInEnvironment);
            }
        }

        public void RestartServices(IEnumerable<IconApplicationViewModel> applications)
        {
            foreach (var appToRestart in applications)
            {
                WmiService.StopRemoteService(appToRestart.Server, appToRestart.Name, new string[] { });
                //TODO need pause/threading here?
                WmiService.StartRemoteService(appToRestart.Server, appToRestart.Name, new string[] { });
            }
        }

        public string FindEsbEnvironmentForApp(IEnumerable<EsbEnvironmentViewModel> allEsbEnvironments, Dictionary<string,string> esbConnectionSettings)
        {
            var serverUrlKey = nameof(EsbEnvironmentViewModel.ServerUrl);
            if (allEsbEnvironments == null) throw new ArgumentNullException(nameof(allEsbEnvironments));

            if (esbConnectionSettings.Any() && esbConnectionSettings.ContainsKey(serverUrlKey))
            {
                var hostsForApp = Services.EsbEnvironmentManager.GetHostsFromServerUrl(esbConnectionSettings[serverUrlKey]);
                if (hostsForApp != null)
                {
                    foreach (var env in allEsbEnvironments)
                    {
                        var hostsInEsbEnvironment = Services.EsbEnvironmentManager.GetHostsFromServerUrl(env.ServerUrl);
                        if (hostsInEsbEnvironment != null)
                        {
                            foreach (var appHost in hostsForApp)
                            {
                                if (hostsInEsbEnvironment.Contains(appHost))
                                {
                                    return env.Name;
                                }
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }

        public void SaveRemoteServiceAppSettings(IconApplicationViewModel appViewModel)
        {
            try
            {
                var appConfig = XDocument.Load(appViewModel.ConfigFilePath);

                // combine the AppSettings and the ESB-related subset of AppSettings into 1 collection
                var combinedDictionary = AppConfigAssistant.CombineDictionariesIgnoreDuplicates(
                    appViewModel.AppSettings, appViewModel.EsbConnectionSettings);

                // create XML elements for each setting
                var updatedElements = combinedDictionary.Select(i =>
                        new XElement("add",
                            new XAttribute("key", i.Key),
                            new XAttribute("value", i.Value ?? String.Empty)));

                //replace the existing appSettings node in the XML file with the new element collection
                var configAppSettingsElement = appConfig.Root.Element("appSettings");
                configAppSettingsElement.ReplaceNodes(updatedElements);
                appConfig.Save(appViewModel.ConfigFilePath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DatabaseDefinition CreateDatabaseDefinitionFromConfigElement(ConnectionStringConfigElement csElement)
        {
            var db = new DatabaseDefinition();

            var sqlStringBuilder = new SqlConnectionStringBuilder();
            if ( !string.IsNullOrWhiteSpace(csElement.ProviderName) && csElement.ProviderName.Contains("EntityClient"))
            {
                db.IsEntityFramework = true;
                var efStringBuilder = new EntityConnectionStringBuilder();
                try
                {
                    efStringBuilder.ConnectionString = csElement.ConnectionString;
                    sqlStringBuilder.ConnectionString = efStringBuilder.ProviderConnectionString;
                }
                catch (ArgumentException argEx)
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

            db.Environment = DetermineDatabaseEnvironment(db.Category, db.ServerName, db.DatabaseName);

            return db;
        }

        public ApplicationDatabaseConfiguration ReadDatabaseConfiguration(string appConfigPath)
        {
            var dbConfig = new ApplicationDatabaseConfiguration();

            if (System.IO.File.Exists(appConfigPath))
            {
                var appConfig = XDocument.Load(appConfigPath);
                if (appConfig.Root.Element("connectionStrings").Elements()
                    .FirstOrDefault(e=>e.Name.ToString().EndsWith("EncryptedData")) != null)
                {
                    // encrypted connection string
                    var encryptedDb = new DatabaseDefinition
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
                    var connectionStringElements = appConfig.Root.Element("connectionStrings").Elements()
                            .Select(e => new ConnectionStringConfigElement(
                                name: e.Attribute("name")?.Value,
                                providerName: e.Attribute("providerName")?.Value,
                                connectionString: e.Attribute("connectionString")?.Value))
                            .ToList();

                    foreach (var csElement in connectionStringElements)
                    {
                        var db = CreateDatabaseDefinitionFromConfigElement(csElement);
                        dbConfig.Connections.Add(db);
                    }
                }

                var nlogElement = appConfig.Root.Elements()
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
                            var loggingCsElement = new ConnectionStringConfigElement(
                                name: targetElement.Attribute("name").Value,
                                providerName: "SqlClient",
                                connectionString: targetElement.Attribute("connectionString").Value
                            );
                            var loggingDb = CreateDatabaseDefinitionFromConfigElement(loggingCsElement);
                            loggingDb.IsUsedForLogging = true;
                            dbConfig.Connections.Add(loggingDb);
                        }
                    }
                }
            }
            return dbConfig;
        }

        public EnvironmentEnum DetermineDatabaseEnvironment(DatabaseCategoryEnum category, string serverName, string databaseName)
        {
            var env = EnvironmentEnum.Undefined;

            switch (category)
            {
                case DatabaseCategoryEnum.Icon:
                    switch (serverName.ToUpper())
                    {
                        case @"CEWD1815\SQLSHARED2012D":
                        case @"SQL-ICON16-TST":
                            if (databaseName.ToUpper() == "ICON")
                            {
                                env = EnvironmentEnum.Test;
                            }
                            else if (databaseName.ToUpper() == "ICONDEV" || databaseName.ToUpper() == "ICON2016")
                            {
                                env = EnvironmentEnum.Dev;
                            }
                            else if (databaseName.ToUpper() == "ICONLOADTEST")
                            {
                                env = EnvironmentEnum.Perf;
                            }
                            break;
                        case @"ICON-DB01-TST01":
                            env = EnvironmentEnum.Tst1;
                            break;
                        case @"IDQ-ICON\SQLSHARED3Q":
                            env = EnvironmentEnum.QA;
                            break;
                        case @"IDP-ICON\SHARED3P":
                            env = EnvironmentEnum.Prd;
                            break;
                        default:
                            break;
                    }
                    break;
                case DatabaseCategoryEnum.Mammoth:
                    switch (serverName.ToUpper())
                    {
                        case @"MAMMOTH-DB01-DEV\MAMMOTH":
                            if (databaseName.ToUpper() == "MAMMOTH")
                            {
                                env = EnvironmentEnum.Test;
                            }
                            else if (databaseName.ToUpper() == "MAMMOTH_DEV")
                            {
                                env = EnvironmentEnum.Dev;
                            }
                            break;
                        case @"MAMMOTH-DB01-TST01":
                            env = EnvironmentEnum.Tst1;
                            break;
                        case @"MAMMOTH-DB01-QA\MAMMOTH":
                            env = EnvironmentEnum.QA;
                            break;
                        case @"QA-01-MAMMOTH02\MAMMOTH02":
                            env = EnvironmentEnum.Perf;
                            break;
                        case @"MAMMOTH-DB01-PRD\MAMMOTH":
                            env = EnvironmentEnum.Prd;
                            break;
                        default:
                            break;
                    }
                    break;
                case DatabaseCategoryEnum.IRMA:
                    if (serverName.ToUpper().Contains("IDD"))
                    {
                        env = EnvironmentEnum.Dev;
                    }
                    else if (serverName.ToUpper().Contains("IDT"))
                    {
                        env = EnvironmentEnum.Test;
                    }
                    else if (serverName.ToUpper().Contains("TST01"))
                    {
                        env = EnvironmentEnum.Tst1;
                    }
                    else if (serverName.ToUpper().Contains("IDQ"))
                    {
                        env = EnvironmentEnum.QA;
                    }
                    else if (serverName.ToUpper().Contains("IDP"))
                    {
                        env = EnvironmentEnum.Prd;
                    }
                    break;
                case DatabaseCategoryEnum.Vim:
                case DatabaseCategoryEnum.Other:
                case DatabaseCategoryEnum.Unknown:
                default:
                    env = EnvironmentEnum.Undefined;
                    break;
            }

            return env;
        }
    }
}