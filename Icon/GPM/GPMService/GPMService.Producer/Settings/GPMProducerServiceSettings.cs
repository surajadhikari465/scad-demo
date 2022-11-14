using Icon.Common;

namespace GPMService.Producer.Settings
{
    internal class GPMProducerServiceSettings
    {
        public int ResetFlagTrueValue { get; set; }
        public int MaxRedeliveryCount { get; set; }
        public int DbErrorRetryCount { get; set; }
        public int DbRetryDelayInMilliseconds { get; set; }
        public static GPMProducerServiceSettings CreateSettings()
        {
            return new GPMProducerServiceSettings
            {
                ResetFlagTrueValue = AppSettingsAccessor.GetIntSetting("ResetFlagTrueValue", 1),
                MaxRedeliveryCount = AppSettingsAccessor.GetIntSetting("MaxRedeliveryCount", 3),
                DbErrorRetryCount = AppSettingsAccessor.GetIntSetting("DbErrorRetryCount", 5),
                DbRetryDelayInMilliseconds = AppSettingsAccessor.GetIntSetting("DbRetryDelayInMilliseconds", 3)
            };
        }
    }
}
