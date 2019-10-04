using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.Models.CustomConfigElements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Dashboard.Mvc.Services
{
    public interface IDashboardConfigDataLoader
    {
        void LoadConfigData(Dictionary<string,string> appSettings,
            EnvironmentsSection environmentsConfigSection,
            EsbEnvironmentsSection esbEnvironmentsConfigSection);
        Dictionary<string, string> AppSettingsDictionary { get; }
        EnvironmentsSection EnvironmentsSection { get; }
        EsbEnvironmentsSection EsbEnvironmentsSection { get;  }
        KeyValuePair<string, string> GetAppSettingKeyValuePair(string key);
        T GetTypedSettingValueOrDefault<T>(string key, T defaultValue = default(T));
        List<string> SplitAppSettingValuesToList(string key, string defaultCommaSeparatedStringValue);
        string[] SplitAppSettingValuesToArray(string key, string[] defaultCommaSeparatedStringValue);
        List<EnvironmentModel> GetEnvironments();
        List<EsbEnvironmentModel> GetEsbEnvironments();
    }
}