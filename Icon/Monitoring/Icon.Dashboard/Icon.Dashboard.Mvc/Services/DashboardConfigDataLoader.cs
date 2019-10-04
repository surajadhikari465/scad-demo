using Icon.Dashboard.Mvc.Helpers;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.Models.CustomConfigElements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Dashboard.Mvc.Services
{
    public class DashboardConfigDataLoader : IDashboardConfigDataLoader
    {
        public DashboardConfigDataLoader() { }

        public DashboardConfigDataLoader(Dictionary<string, string> appSettings,
            EnvironmentsSection environmentsConfigSection,
            EsbEnvironmentsSection esbEnvironmentsConfigSection) : this()
        {
            LoadConfigData(appSettings, environmentsConfigSection, esbEnvironmentsConfigSection);
        }

        public Dictionary<string, string> AppSettingsDictionary { get; private set; }
        public EnvironmentsSection EnvironmentsSection { get; private set; }
        public EsbEnvironmentsSection EsbEnvironmentsSection { get; private set; }

        public void LoadConfigData(Dictionary<string, string> appSettings,
            EnvironmentsSection environmentsConfigSection,
            EsbEnvironmentsSection esbEnvironmentsConfigSection)
        {
            AppSettingsDictionary = appSettings;
            EnvironmentsSection = environmentsConfigSection;
            EsbEnvironmentsSection = esbEnvironmentsConfigSection;
        }

        public List<EnvironmentModel> GetEnvironments()
        {
            return ConfigSectionToEnvironmentModels(EnvironmentsSection);
        }

        public List<EsbEnvironmentModel> GetEsbEnvironments()
        {
            return ConfigSectionToEsbEnvironmentModels(EsbEnvironmentsSection);
        }

        internal string GetAppSettingStringValue(string key)
        {
            string value = null;
            if (!string.IsNullOrEmpty(key))
            {
                if (AppSettingsDictionary.ContainsKey(key))
                {
                    value = AppSettingsDictionary[key];
                }
            }
            return value;
        }

        public KeyValuePair<string, string> GetAppSettingKeyValuePair(string key)
        {
            string value = null;
            if (!string.IsNullOrEmpty(key))
            {
                value = GetAppSettingStringValue(key);
            }

            var kvp = new KeyValuePair<string, string>(key, value);
            return kvp;
        }

        public T GetTypedSettingValueOrDefault<T>(string key, T defaultValue = default)
        {
            if (!string.IsNullOrEmpty(key))
            {
                string value = GetAppSettingStringValue(key);
                if (value != null)
                {
                    try
                    {
                        var type = typeof(T);
                        return type.IsEnum
                            ? (T)Enum.Parse(type, value)
                            : (T)Convert.ChangeType(value, type);
                    }
                    catch { }
                }
            }
            return defaultValue;
        }

        public string[] SplitAppSettingValuesToArray(string key, string[] defaultCommaSeparatedStringValue)
        {
            var commaSeparatedStringValue = GetAppSettingStringValue(key);

            return string.IsNullOrWhiteSpace(commaSeparatedStringValue)
                ? defaultCommaSeparatedStringValue
                : Utils.SplitCommaSeparatedValuesToArray(commaSeparatedStringValue);
        }

        public List<string> SplitAppSettingValuesToList(string key, string defaultCommaSeparatedStringValue)
        {
            var commaSeparatedStringValue = GetTypedSettingValueOrDefault(key, defaultCommaSeparatedStringValue);

            return string.IsNullOrWhiteSpace(commaSeparatedStringValue)
                ? Utils.SplitCommaSeparatedValuesToList(defaultCommaSeparatedStringValue)
                : Utils.SplitCommaSeparatedValuesToList(commaSeparatedStringValue);
        }

        internal List<EnvironmentModel> ConfigSectionToEnvironmentModels(EnvironmentsSection environmentsSection)
        {
            var models = new List<EnvironmentModel>();

            if (environmentsSection != null && environmentsSection.Environments != null)
            {
                models.AddRange(environmentsSection.Environments
                    .Where(el => el.IsEnabled)
                    .Select(el => new EnvironmentModel(el)));
            }

            return models;
        }

        internal List<EsbEnvironmentModel> ConfigSectionToEsbEnvironmentModels(EsbEnvironmentsSection esbEnvironmentsSection)
        {
            var models = new List<EsbEnvironmentModel>();

            if (esbEnvironmentsSection != null && esbEnvironmentsSection.EsbEnvironments != null)
            {
                models.AddRange(esbEnvironmentsSection.EsbEnvironments
                    .Select(el => new EsbEnvironmentModel(el)));
            }

            return models;
        }
    }
}