using Icon.Common;

namespace ErrorMessagesMonitor.Settings
{
    internal class ErrorMessagesMonitorServiceSettings
    {
        public string ServiceType { get; set; }
        public int TimerProcessRunIntervalInMilliseconds { get; set; }
        public string SendErrorAlerts;
        public string ErrorAlertIntegrationKey;
        public string ErrorAlertUri;
        // TODO - Add more attributes

        public static ErrorMessagesMonitorServiceSettings CreateSettingsFromConfig()
        {
            return new ErrorMessagesMonitorServiceSettings
            {
                TimerProcessRunIntervalInMilliseconds = AppSettingsAccessor.GetIntSetting("TimerProcessRunIntervalInMilliseconds", 900000),
                SendErrorAlerts = AppSettingsAccessor.GetStringSetting("SendErrorAlerts"),
                ErrorAlertIntegrationKey = AppSettingsAccessor.GetStringSetting("ErrorAlertIntegrationKey"),
                ErrorAlertUri = AppSettingsAccessor.GetStringSetting("ErrorAlertUri")
            };
        }
    }
}
