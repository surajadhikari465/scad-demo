using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using WholeFoods.Utility.DataAccess;


using log4net;

namespace WholeFoods.Common.IRMALib
{
    public class ConfigRepository : IConfigRepository
    {
        #region Members (the jokes write themselves)

        private IRMALibDataClassesDataContext db;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private XDocument config = null;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.  Needs connection string for Linq.
        /// </summary>
        /// <param name="connectionString">Connection string for the DB connection.</param>
        public ConfigRepository(string connectionString)
        {
            db = new IRMALibDataClassesDataContext(connectionString);
        }


        #endregion

        #region Methods

        public bool UpdateKeyValue(AppConfigValue _appConfigValue)
        {
            // the 0 as last argument here is the system userId.  Per advice from Tom Lux.  This way we're not mucking about with finding out
            // if the executing user is a user on the database we're messing with, etc.
            return UpdateKeyValue(_appConfigValue.ApplicationID, _appConfigValue.EnvironmentID, _appConfigValue.AppConfigKey.Name, _appConfigValue.Value, 0);
        }

        public bool UpdateKeyValue(Guid appId, Guid envId, string name, string value, int userId)
        {
            int? keyId = FindAppKeyByName(name);
            if (!keyId.HasValue)
            {
                db.AppConfig_AddKey(ref keyId, name, userId);
            }
            try
            {
                // call stored proc to update
                db.AppConfig_UpdateKeyValue(appId, envId, keyId, value, userId);
            }
            catch (Exception ex)
            {
                // OOPS_HEH
                logger.Error("Unable to update AppConfig key/value: " + ex.Message);
                return false;
            }
            return true;
        }

        private int? FindAppKeyByName(string name)
        {
            var key = db.AppConfigKeys.FirstOrDefault(ack => ack.Name.ToLower() == name.ToLower());
            if (key != null)
                return key.KeyID;

            return null;
        }

        /// <summary>
        /// Loads a config file and retains in memory, where it can be queried.
        /// </summary>
        /// <param name="_applicationId">The appId for the config</param>
        /// <param name="_environmentId">The envId for the config</param>
        /// <returns>true on success, false otherwise.</returns>
        public bool LoadConfig(Guid _applicationId, Guid _environmentId)
        {
            string xml = "<configuration><appSettings>" + GetConfigKeyList(_applicationId, _environmentId) + "</appSettings></configuration>";
            if (xml == "<configuration><appSettings></appSettings></configuration>")
            {
                // no keys found for this config, so return false;
                return false;
            }
            config = new XDocument();
            try
            {
                config = XDocument.Parse(xml);
            }
            catch (Exception ex)
            {
                logger.Error("LoadConfig() error: " + ex.Message);
                config = null;
                return false;
            }
            return true;
        }

        // gets all the keys associated with a certain config.  
        // wrap in a <configuration><appSettings> ... </appSettings></configuration>
        // for go-go gadget use.
        public string GetConfigKeyList(Guid _applicationId, Guid _environmentId)
        {
            var keys = db.AppConfig_GetConfigKeyList(_applicationId, _environmentId);
            StringBuilder sb = new StringBuilder();
            foreach (AppConfig_GetConfigKeyListResult result in keys)
            {
                sb.Append(result.ConfigKey);
            }
            return sb.ToString();
        }

        public string ConfigurationGetValue(string configKey)
        {
            if (config != null)
            {
                var elem = from key in config.Root.Descendants().Descendants()
                           where key.Attribute("key").Value.ToLower() == configKey.ToLower()
                           select key.Attribute("value").Value;

                return elem.FirstOrDefault();
            }
            return null;
        }

        public bool ConfigurationGetValue(string configKey, ref string value)
        {
            if (config != null)
            {
                var elem = from key in config.Root.Descendants().Descendants()
                           where key.Attribute("key").Value.ToLower() == configKey.ToLower()
                           select key.Attribute("value").Value;

                if (elem != null)
                {
                    value = elem.First().ToString();
                    return true;
                }
            }
            return false;
        }

        public bool ConfigurationGetValue(string configXml, string configKey, ref string value)
        {
            XDocument _config = XDocument.Parse(configXml);

            if (_config != null)
            {
                var elem = from key in _config.Root.Descendants().Descendants()
                           where key.Attribute("key").Value.ToLower() == configKey.ToLower()
                           select key.Attribute("value").Value;

                if (elem != null)
                {
                    value = elem.First().ToString();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the specified config value. (OBSOLETE)
        /// </summary>
        /// <param name="appId">Application ID</param>
        /// <param name="envId">Environment ID</param>
        /// <param name="configKey">The key for the config item you want.</param>
        /// <returns>A string.  Convert as needed.</returns>
        public string ConfigurationGetValue_OLD(Guid appId, Guid envId, string configKey)
        {
            return (db.ApplicationConfigurations
                .FirstOrDefault(ac => ac.ApplicationId.ToLower() == appId.ToString().ToLower() && ac.EnvironmentId.ToLower() == envId.ToString().ToLower() && ac.Key == configKey)).Value;
        }

        
        public string GetConfigXml(Guid _applicationId, Guid _environmentId)
        {
            return (db.AppConfigApps.Where(ac => ac.ApplicationID == _applicationId && ac.EnvironmentID == _environmentId && ac.Deleted == false).SingleOrDefault().Configuration).ToString();
        }

        public string GetConfigDocument(Guid _applicationId, Guid _environmentId)
        {
            return (db.AppConfigApps.Where(ac => ac.ApplicationID == _applicationId && ac.EnvironmentID == _environmentId && ac.Deleted == false).SingleOrDefault().Configuration).ToString();
        }

        

        /// <summary>
        /// Write a config to the specified sub folder underneath the executable working directory
        /// </summary>
        /// <param name="_applicationId">the app id for the config</param>
        /// <param name="_environmentId">the environment id for the config</param>
        /// <param name="subFolder">the subfolder where you wish to write the config</param>
        public void WriteConfiguration(Guid _applicationId, Guid _environmentId, string subFolder)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(GetConfigDocument(_applicationId, _environmentId));

            string fPath = "";
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), subFolder)))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), subFolder));
            }

            fPath = Path.Combine(Directory.GetCurrentDirectory(), subFolder, "appSettings.config");

            doc.Save(fPath);
        }

        public void WriteConfiguration(Guid _applicationId, Guid _environmentId)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(GetConfigDocument(_applicationId, _environmentId));

            string fPath = "";

            // this code is from the existing version.  I'm thinking that this library should remain agnostic about
            // the existence of command line arguments.  included here for completeness, but use the 3 argument version
            // otherwise.
            if (Environment.GetCommandLineArgs().Length > 1)
            {
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), Environment.GetCommandLineArgs().GetValue(1).ToString())))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), Environment.GetCommandLineArgs().GetValue(1).ToString()));
                }

                // SingleSource job execution - regional working folders are located under the current app executable directory
                fPath = Path.Combine(Directory.GetCurrentDirectory(), Environment.GetCommandLineArgs().GetValue(1).ToString(), "appSettings.config");
            }
            else
            {
                // client/user initiated
                fPath = Path.Combine(Environment.SpecialFolder.ApplicationData.ToString(), "appSettings.config");
            }

            doc.Save(fPath);
        }
        /// <summary>
        /// Returns a single record from the ConfigurationData table. (Probably Obsolete)
        /// </summary>
        /// <param name="_configKey">the key for which value you want</param>
        /// <returns>An object.  Cast as needed.</returns>
        public object GetConfigValue(string _configKey)
        {
            return db.ConfigurationDatas.FirstOrDefault(cd => cd.ConfigKey == _configKey).ConfigValue;
        }

        public IEnumerable<AppConfigApp> GetApplicationList(Guid _environmentId)
        {
            return db.AppConfigApps.Where(aca => aca.EnvironmentID == _environmentId);
        }

        public IEnumerable<AppConfig_GetConfigListResult> GetConfigList()
        {
            return db.AppConfig_GetConfigList();
        }

        public IEnumerable<AppConfigEnv> GetEnvironmentList()
        {
            return db.AppConfigEnvs.Where(ace => ace.Deleted == false);
        }

        public IEnumerable<AppConfigKey> GetApplicationKeyList()
        {
            return db.AppConfigKeys.Where(ack => ack.Deleted == false);
        }

        #endregion

        #region As Yet Unimplemented and Some Likely Obsolete

        public bool Save(AppConfigApp _appConfigApp)
        {
            throw new NotImplementedException();
        }

        public bool RemoveAppConfigValue(AppConfigValue _appConfigValue)
        {
            throw new NotImplementedException();
        }

        public bool RemoveAppConfigApp(AppConfigApp _appConfigApp)
        {
            throw new NotImplementedException();
        }

        public bool RemoveAppConfigEnv(AppConfigEnv _appConfigEnv)
        {
            throw new NotImplementedException();
        }

        public void AddAppConfigValue(AppConfigValue _appConfigValue, bool UpdateExistingKeyValue)
        {
            throw new NotImplementedException();
        }

        public AppConfigKey ImportKey(AppConfigKey _appConfigKey)
        {
            throw new NotImplementedException();
        }

        public AppConfigKey AddAppConfigKey(AppConfigKey _appConfigKey)
        {
            throw new NotImplementedException();
        }

        public AppConfigEnv AddAppConfigEnv(AppConfigEnv _appConfigEnv)
        {
            throw new NotImplementedException();
        }

        public AppConfigApp AddAppConfigApp(AppConfigApp _appConfigApp)
        {
            throw new NotImplementedException();
        }

        public bool UpdateConfigInfo(DataSet ds, string TblName)
        {
            throw new NotImplementedException();
        }

        public void GetConfigInfo()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
