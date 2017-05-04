using Icon.Common;
using System.Collections.Generic;
using System.Linq;

namespace Infor.Services.NewItem.Notifiers
{
    public class NewItemNotifierSettings
    {
        private static string RegionalNotificationEnabledKeyPrefix = "RegionalNotificationEnabled_";

        public Dictionary<string, bool> RegionalNotificationEnabled { get; set; }

        public static NewItemNotifierSettings CreateFromConfig()
        {
            return new NewItemNotifierSettings
            {
                RegionalNotificationEnabled = AppSettingsAccessor.GetStringSetting("Regions")
                    .Split(',')
                    .ToDictionary(
                        region => region,
                        region => AppSettingsAccessor.GetBoolSetting(RegionalNotificationEnabledKeyPrefix + region))
            };
        }
    }
}