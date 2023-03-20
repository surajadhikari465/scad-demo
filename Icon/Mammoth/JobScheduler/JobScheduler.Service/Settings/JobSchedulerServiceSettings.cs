using Icon.Common;
using System;

namespace JobScheduler.Service.Settings
{
    internal class JobSchedulerServiceSettings
    {
        public int ServiceRunIntervalInMinutes { get; set; }
        public int SendMessageRetryCount { get; set; }
        public int SendMessageRetryDelayInMilliseconds { get; set; }
        public int InstanceId { get; set; }
        public string AwsAccountId { get; set; }

        public static JobSchedulerServiceSettings CreateSettings()
        {
            int serviceRunIntervalInMinutes = AppSettingsAccessor.GetIntSetting("ServiceRunIntervalInMinutes", 5);
            if (serviceRunIntervalInMinutes < 1)
            {
                throw new ArgumentException("Value of ServiceRunIntervalInMinutes must be greater than 0.");
            }
            return new JobSchedulerServiceSettings
            {
                ServiceRunIntervalInMinutes = serviceRunIntervalInMinutes,
                SendMessageRetryCount = AppSettingsAccessor.GetIntSetting("SendMessageRetryCount", 3),
                SendMessageRetryDelayInMilliseconds = AppSettingsAccessor.GetIntSetting("SendMessageRetryDelayInMilliseconds", 3000),
                InstanceId = AppSettingsAccessor.GetIntSetting("InstanceId"),
                AwsAccountId = AppSettingsAccessor.GetStringSetting("AwsAccountId")
            };
        }
    }
}
