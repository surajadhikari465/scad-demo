using Icon.Common;

namespace GPMService.Producer.Settings
{
    internal class GPMProducerServiceSettings
    {
        public int ResetFlagTrueValue { get; set; }
        public static GPMProducerServiceSettings CreateSettings()
        {
            return new GPMProducerServiceSettings
            {
                ResetFlagTrueValue = AppSettingsAccessor.GetIntSetting("ResetFlagTrueValue", 1)
            };
        }
    }
}
