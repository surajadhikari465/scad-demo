using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Configuration;

namespace Icon.Dashboard.Mvc.Helpers
{
    public static class ConfigAccess
    {
        public static Configuration OpenInternalWebConfiguration()
        {
            return WebConfigurationManager.OpenWebConfiguration("~");
        }

        internal static Configuration OpenInternalAppConfiguration()
        {
            return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        public static Configuration OpenExternalAppConfiguration(string externalConfigFilePath)
        {
            Utils.VerifyExternalFilePath(externalConfigFilePath, "open external app config file", false);
            var configFileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = externalConfigFilePath
            };
            return ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
        }

        public static Dictionary<string, string> GetAppSettingsAsDictionary(Configuration config)
        {
            return config?.AppSettings?.Settings?.Cast<KeyValueConfigurationElement>()?
               .ToDictionary(s => s.Key, s => s.Value)
               ?? new Dictionary<string, string>();
        }

        public static TConfigSection GetCustomConfigSection<TConfigSection>(Configuration config, string customSectionGroupName)
            where TConfigSection : ConfigurationSection
        {
            var customSectionCollection = config?.SectionGroups[customSectionGroupName]?.Sections;
            return (customSectionCollection?.Cast<ConfigurationSection>()?
                .FirstOrDefault(s => s.GetType() == typeof(TConfigSection))
                ?? null) as TConfigSection;

        }
    }
}