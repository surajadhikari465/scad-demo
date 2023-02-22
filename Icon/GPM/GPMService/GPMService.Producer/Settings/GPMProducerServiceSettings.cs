using Icon.Common;

namespace GPMService.Producer.Settings
{
    internal class GPMProducerServiceSettings
    {
        public string ServiceType { get; set; }
        public string GpmConfirmBODBucket { get; set; }
        public string GpmProcessBODBucket { get; set; }
        public string GpmJustInTimeBucket { get; set; }
        public string GpmJustInTimeTopicArn { get; set; }
        public int MaxRedeliveryCount { get; set; }
        public int DbErrorRetryCount { get; set; }
        public int DbRetryDelayInMilliseconds { get; set; }
        public int SendMessageRetryCount { get; set; }
        public int SendMessageRetryDelayInMilliseconds { get; set; }
        public int ActivePriceSubsetSize { get; set; }
        public int ExpiringTprSubsetSize { get; set; }
        public int JdbcQueryTimeoutInSeconds { get; set; }
        public int ActivePriceBatchSize { get; set; }
        public int ExpiringTprBatchSize { get; set; }
        public int TimerProcessRunIntervalInMilliseconds { get; set; }
        public int EmergencyPriceDequeueCount { get; set; }
        public static GPMProducerServiceSettings CreateSettings()
        {
            return new GPMProducerServiceSettings
            {
                ServiceType = AppSettingsAccessor.GetStringSetting("ServiceType"),
                GpmJustInTimeBucket = AppSettingsAccessor.GetStringSetting("GpmJustInTimeBucket"),
                GpmConfirmBODBucket = AppSettingsAccessor.GetStringSetting("GpmConfirmBODBucket"),
                GpmProcessBODBucket = AppSettingsAccessor.GetStringSetting("GpmProcessBODBucket"),
                GpmJustInTimeTopicArn = AppSettingsAccessor.GetStringSetting("GpmJustInTimeTopicArn"),
                MaxRedeliveryCount = AppSettingsAccessor.GetIntSetting("MaxRedeliveryCount", 8),
                DbErrorRetryCount = AppSettingsAccessor.GetIntSetting("DbErrorRetryCount", 5),
                DbRetryDelayInMilliseconds = AppSettingsAccessor.GetIntSetting("DbRetryDelayInMilliseconds", 3000),
                SendMessageRetryCount = AppSettingsAccessor.GetIntSetting("SendMessageRetryCount", 3),
                SendMessageRetryDelayInMilliseconds = AppSettingsAccessor.GetIntSetting("SendMessageRetryDelayInMilliseconds", 3000),
                ActivePriceSubsetSize = AppSettingsAccessor.GetIntSetting("ActivePriceSubsetSize", 5000),
                ExpiringTprSubsetSize = AppSettingsAccessor.GetIntSetting("ExpiringTprSubsetSize", 1000),
                JdbcQueryTimeoutInSeconds = AppSettingsAccessor.GetIntSetting("JdbcQueryTimeoutInSeconds", 120),
                ActivePriceBatchSize = AppSettingsAccessor.GetIntSetting("ActivePriceBatchSize", 100),
                ExpiringTprBatchSize = AppSettingsAccessor.GetIntSetting("ExpiringTprBatchSize", 100),
                TimerProcessRunIntervalInMilliseconds = AppSettingsAccessor.GetIntSetting("TimerProcessRunIntervalInMilliseconds", 120000),
                EmergencyPriceDequeueCount = AppSettingsAccessor.GetIntSetting("EmergencyPriceDequeueCount", 100)
            };
        }
    }
}
