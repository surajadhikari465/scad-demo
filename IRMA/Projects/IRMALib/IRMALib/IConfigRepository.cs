using System;
using System.Collections.Generic;
using System.Data;

namespace WholeFoods.Common.IRMALib
{
    public interface IConfigRepository
    {
        AppConfigApp AddAppConfigApp(AppConfigApp _appConfigApp);
        AppConfigEnv AddAppConfigEnv(AppConfigEnv _appConfigEnv);
        AppConfigKey AddAppConfigKey(AppConfigKey _appConfigKey);
        void AddAppConfigValue(AppConfigValue _appConfigValue, bool UpdateExistingKeyValue);
        bool ConfigurationGetValue(string configKey, ref string value);
        bool ConfigurationGetValue(string configXml, string configKey, ref string value);
        string ConfigurationGetValue(string configKey);
        string ConfigurationGetValue_OLD(Guid appId, Guid envId, string configKey);
        IEnumerable<AppConfigKey> GetApplicationKeyList();
        IEnumerable<AppConfigApp> GetApplicationList(Guid _environmentId);
        string GetConfigDocument(Guid _applicationId, Guid _environmentId);
        void GetConfigInfo();
        string GetConfigKeyList(Guid _applicationId, Guid _environmentId);
        IEnumerable<AppConfig_GetConfigListResult> GetConfigList();
        object GetConfigValue(string _configKey);
        string GetConfigXml(Guid _applicationId, Guid _environmentId);
        IEnumerable<AppConfigEnv> GetEnvironmentList();
        AppConfigKey ImportKey(AppConfigKey _appConfigKey);
        bool LoadConfig(Guid _applicationId, Guid _environmentId);
        bool RemoveAppConfigApp(AppConfigApp _appConfigApp);
        bool RemoveAppConfigEnv(AppConfigEnv _appConfigEnv);
        bool RemoveAppConfigValue(AppConfigValue _appConfigValue);
        bool Save(AppConfigApp _appConfigApp);
        bool UpdateConfigInfo(DataSet ds, string TblName);
        bool UpdateKeyValue(AppConfigValue _appConfigValue);
        bool UpdateKeyValue(Guid appId, Guid envId, string name, string value, int userId);
        void WriteConfiguration(Guid _applicationId, Guid _environmentId);
        void WriteConfiguration(Guid _applicationId, Guid _environmentId, string subFolder);
    }
}