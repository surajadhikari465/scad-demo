using Icon.Common;

namespace ErrorMessagesMonitor.Settings
{
    internal class ErrorMessagesMonitorServiceSettings
    {
        public int TimerProcessRunIntervalInMilliseconds { get; set; }
        public string SendErrorAlerts;
        public string ErrorAlertIntegrationKey;
        public string ErrorAlertUri;

        public static ErrorMessagesMonitorServiceSettings CreateSettingsFromConfig()
        {
            return new ErrorMessagesMonitorServiceSettings
            {
                TimerProcessRunIntervalInMilliseconds = AppSettingsAccessor.GetIntSetting("TimerProcessRunIntervalInMilliseconds"),
                SendErrorAlerts = AppSettingsAccessor.GetStringSetting("SendErrorAlerts"),
                ErrorAlertIntegrationKey = AppSettingsAccessor.GetStringSetting("ErrorAlertIntegrationKey"),
                ErrorAlertUri = AppSettingsAccessor.GetStringSetting("ErrorAlertUri")
            };
        }
    }
}
