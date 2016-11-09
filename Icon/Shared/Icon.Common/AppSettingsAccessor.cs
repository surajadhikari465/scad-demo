using System;
using System.Configuration;

namespace Icon.Common
{
    public class AppSettingsAccessor
    {
        public static string GetStringSetting(string settingName, bool required = true)
        {
            string setting = ConfigurationManager.AppSettings[settingName];
            ValidateSetting(settingName, setting, required);

            return setting;
        }

        public static string GetStringSetting(string settingName, string defaultValue)
        {
            string setting = ConfigurationManager.AppSettings[settingName];
            if (String.IsNullOrWhiteSpace(setting))
            {
                return defaultValue;
            }

            return setting;
        }

        public static int GetIntSetting(string settingName, bool required = true)
        {
            string setting = ConfigurationManager.AppSettings[settingName];
            ValidateSetting(settingName, setting, required);

            int intSetting = 0;
            if (int.TryParse(setting, out intSetting) || !required)
            {
                return intSetting;
            }
            else
            {
                throw new InvalidOperationException(String.Format("[{0}] in the settings file is not a valid integer.  Its value is set to [{1}].", settingName, setting));
            }
        }

        public static int GetIntSetting(string settingName, int defaultValue)
        {
            string setting = ConfigurationManager.AppSettings[settingName];

            int intSetting = 0;
            if (!int.TryParse(setting, out intSetting))
            {
                intSetting = defaultValue;
            }

            return intSetting;
        }

        public static T GetEnumSetting<T>(string settingName, bool required = true) where T : struct, IComparable, IFormattable, IConvertible
        {
            string setting = ConfigurationManager.AppSettings[settingName];
            ValidateSetting(settingName, setting, required);

            T enumSetting = default(T);
            if (Enum.TryParse<T>(setting, out enumSetting) || !required)
            {
                return enumSetting;
            }
            else
            {
                throw new InvalidOperationException(
                    String.Format("[{0}] is not a valid value for the [{1}] setting.  [{1}] is a [{2}].  Valid values for [{2}] are : [{3}].",
                        setting,
                        settingName,
                        typeof(T),
                        String.Join(", ", Enum.GetNames(typeof(T)
                ))));
            }
        }

        public static bool GetBoolSetting(string settingName, bool required = true)
        {
            string setting = ConfigurationManager.AppSettings[settingName];
            ValidateSetting(settingName, setting, required);

            bool boolSetting = default(bool);
            if (Boolean.TryParse(setting, out boolSetting) || !required)
            {
                return boolSetting;
            }
            else
            {
                throw new InvalidOperationException(String.Format("[{0}] is not a valid value for the [{1}] setting. [{1}] must be either True or False. Case is irrelevant", setting, settingName));
            }
        }

        private static void ValidateSetting(string settingName, string setting, bool required)
        {
            if (required)
            {
                if (String.IsNullOrWhiteSpace(setting))
                {
                    throw new InvalidOperationException(String.Format("[{0}] is a required setting but could not be found or had no value in the application's settings.", settingName));
                }
            }
        }
    }
}
