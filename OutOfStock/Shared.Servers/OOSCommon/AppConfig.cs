using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace OOSCommon
{
    public class AppConfig
    {
        public const string suffixAppSetting = "Suffix";

        public static AppSettingAccessor AppSettings = new AppSettingAccessor();
        public static ConnectionStringAccessor ConnectionStrings = new ConnectionStringAccessor();

        public static string suffix
        {
            get
            {
                if (_suffix == null)
                    _suffix = ConfigurationManager.AppSettings[suffixAppSetting];
                return _suffix;
            }
        } static string _suffix = null;

        public static string GetAppSetting(string key)
        {
            string result = string.Empty;
            if (!string.IsNullOrWhiteSpace(suffix) && ConfigurationManager.AppSettings.AllKeys.Contains(key + suffix))
                result = ConfigurationManager.AppSettings[key + suffix];
            else
                result = ConfigurationManager.AppSettings[key];
            return result;
        }

        public static ConnectionStringSettings GetConnectionString(string key)
        {
            ConnectionStringSettings result = null;
            string keyWithSuffix = key + suffix;
            if (!string.IsNullOrWhiteSpace(suffix))
                result = ConfigurationManager.ConnectionStrings[key + suffix];
            if (result == null)
                result = ConfigurationManager.ConnectionStrings[key];
            return result;
        }

        public class AppSettingAccessor
        {
            public string this[string key]
            {
                get { return AppConfig.GetAppSetting(key); }
            }
        }

        public class ConnectionStringAccessor
        {
            public ConnectionStringSettings this[string key]
            {
                get { return AppConfig.GetConnectionString(key); }
            }
        }

    }
}
