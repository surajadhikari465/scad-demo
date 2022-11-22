using Icon.Common;

namespace GPMService.Producer.Settings
{
    internal class GPMProducerServiceSettings
    {
        public int MaxRedeliveryCount { get; set; }
        public int DbErrorRetryCount { get; set; }
        public int DbRetryDelayInMilliseconds { get; set; }
        public int SendMessageRetryCount { get; set; }
        public int SendMessageRetryDelayInMilliseconds { get; set; }
        public int ActivePriceSubsetSize { get; set; }
        public int JdbcQueryTimeoutInSeconds { get; set; }
        public int ActivePriceBatchsize { get; set; }
        public static GPMProducerServiceSettings CreateSettings()
        {
            return new GPMProducerServiceSettings
            {
                MaxRedeliveryCount = AppSettingsAccessor.GetIntSetting("MaxRedeliveryCount", 3),
                DbErrorRetryCount = AppSettingsAccessor.GetIntSetting("DbErrorRetryCount", 5),
                DbRetryDelayInMilliseconds = AppSettingsAccessor.GetIntSetting("DbRetryDelayInMilliseconds", 3000),
                SendMessageRetryCount = AppSettingsAccessor.GetIntSetting("SendMessageRetryCount", 3),
                SendMessageRetryDelayInMilliseconds = AppSettingsAccessor.GetIntSetting("SendMessageRetryDelayInMilliseconds", 3000),
                ActivePriceSubsetSize = AppSettingsAccessor.GetIntSetting("ActivePriceSubsetSize", 5000),
                JdbcQueryTimeoutInSeconds = AppSettingsAccessor.GetIntSetting("JdbcQueryTimeoutInSeconds", 120),
                ActivePriceBatchsize = AppSettingsAccessor.GetIntSetting("ActivePriceBatchsize", 100)
            };
        }
    }
}
