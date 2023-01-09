using Icon.Common;

namespace MammothR10Price
{
    public class MammothR10PriceServiceSettings
    {
        public string ApplicationName { get; set; }
        public int SendMessageRetryCount { get; set; }
        public int SendMessageRetryDelayInMilliseconds { get; set; }
        public string NonReceivingSystems { get; set; }

        public static MammothR10PriceServiceSettings CreateSettingsFromConfig()
        {
            return new MammothR10PriceServiceSettings
            {
                ApplicationName = AppSettingsAccessor.GetStringSetting("ApplicationName"),
                SendMessageRetryCount = AppSettingsAccessor.GetIntSetting("SendMessageRetryCount", 3),
                SendMessageRetryDelayInMilliseconds = AppSettingsAccessor.GetIntSetting("SendMessageRetryDelayInMilliseconds", 3000),
                NonReceivingSystems = AppSettingsAccessor.GetStringSetting("NonReceivingSystems"),
            };
        }
    }
}
