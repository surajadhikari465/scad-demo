using Icon.Common;

namespace ErrorMessagesMonitor.Settings
{
    internal class ErrorMessagesMonitorServiceSettings
    {
        public string ServiceType { get; set; }
        public int TimerProcessRunIntervalInMilliseconds { get; set; }
        // TODO - Add more attributes

        public static ErrorMessagesMonitorServiceSettings CreateSettingsFromConfig()
        {
            return new ErrorMessagesMonitorServiceSettings
            {
                TimerProcessRunIntervalInMilliseconds = AppSettingsAccessor.GetIntSetting("TimerProcessRunIntervalInMilliseconds", 900000)
            };
        }
    }
}
