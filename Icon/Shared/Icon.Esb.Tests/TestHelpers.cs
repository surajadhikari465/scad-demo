using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.Tests
{
    public static class TestHelpers
    {
        public static void CopyOfAppSettings(Dictionary<string, string> settingsCopy)
        {
            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                settingsCopy.Add(key, ConfigurationManager.AppSettings[key]);
            }
        }

        public static void ClearAppSettings(Dictionary<string, string> settingsCopy)
        {
            foreach (var keyValuePair in settingsCopy)
            {
                ConfigurationManager.AppSettings.Set(keyValuePair.Key, null);
            }
        }

        public static void SetAppSettings(Dictionary<string, string> settingsCopy)
        {
            foreach (var keyValuePair in settingsCopy)
            {
                ConfigurationManager.AppSettings.Set(keyValuePair.Key, keyValuePair.Value);
            }
        }
    }
}
