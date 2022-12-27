using Icon.Common;

namespace JobScheduler.Service.Settings
{
    internal class JobSchedulerServiceSettings
    {
        public int TimerProcessRunIntervalInMilliseconds { get; set; }
        public int SendMessageRetryCount { get; set; }
        public int SendMessageRetryDelayInMilliseconds { get; set; }
        public string AwsAccessKey { get; set; }
        public string AwsSecretKey { get; set; }
        public string AwsAccountId { get; set; }

        public static JobSchedulerServiceSettings CreateSettings()
        {
            return new JobSchedulerServiceSettings
            {
                TimerProcessRunIntervalInMilliseconds = AppSettingsAccessor.GetIntSetting("TimerProcessRunIntervalInMilliseconds", 300000),
                SendMessageRetryCount = AppSettingsAccessor.GetIntSetting("SendMessageRetryCount", 3),
                SendMessageRetryDelayInMilliseconds = AppSettingsAccessor.GetIntSetting("SendMessageRetryDelayInMilliseconds", 3000),
                AwsAccessKey = AppSettingsAccessor.GetStringSetting("AwsAccessKey"),
                AwsSecretKey = AppSettingsAccessor.GetStringSetting("AwsSecretKey"),
                AwsAccountId = AppSettingsAccessor.GetStringSetting("AwsAccountId")
            };
        }
    }
}
