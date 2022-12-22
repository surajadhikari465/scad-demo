using Icon.Common;

namespace JobScheduler.Service.Settings
{
    internal class JobSchedulerServiceSettings
    {
        public int TimerProcessRunIntervalInMilliseconds { get; set; }

        public static JobSchedulerServiceSettings CreateSettings()
        {
            return new JobSchedulerServiceSettings
            {
                TimerProcessRunIntervalInMilliseconds = AppSettingsAccessor.GetIntSetting("TimerProcessRunIntervalInMilliseconds", 300000)
            };
        }
    }
}
