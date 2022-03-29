using Apache.NMS;
using Icon.Common;


namespace Icon.ActiveMQ
{
    public class ActiveMQConnectionSettings: IActiveMQConnectionSettings
    {
        private string _destinationType;

        public string ServerUrl { get; set; }
        public string JmsUsername { get; set; }
        public string JmsPassword { get; set; }
        public string QueueName { get; set; }
        public int ReconnectDelay { get; set; }
        public AcknowledgementMode SessionMode { get; set; }
        public string DestinationType
        {
            get { return _destinationType ?? "Queue"; }
            set { _destinationType = value; }
        }

        /// <summary>
        /// Fills the class attributes using app.config values
        /// </summary>
        /// <param name="destinationConfigName">Queue/Topic key name in app.config: Default to "ActiveMqQueueName"</param>
        public void LoadFromConfig(string destinationConfigName = "ActiveMqQueueName")
        {
            ServerUrl = AppSettingsAccessor.GetStringSetting("ActiveMqServerUrl", false);
            JmsUsername = AppSettingsAccessor.GetStringSetting("ActiveMqJmsUsername", false);
            JmsPassword = AppSettingsAccessor.GetStringSetting("ActiveMqJmsPassword", false);
            QueueName = AppSettingsAccessor.GetStringSetting(destinationConfigName, false);
            DestinationType = AppSettingsAccessor.GetStringSetting("DestinationType", false);
            ReconnectDelay = AppSettingsAccessor.GetIntSetting("ActiveMqReconnectDelay", false);
            SessionMode = AppSettingsAccessor.GetEnumSetting<AcknowledgementMode>("ActiveMqSessionMode", false);
        }

        public static ActiveMQConnectionSettings CreateSettingsFromConfig()
        {
            ActiveMQConnectionSettings settings = new ActiveMQConnectionSettings();
            settings.LoadFromConfig();
            return settings;
        }

        /// <summary>
        /// Creates a new ActiveMQConnectionSettings object and fills class attributes using app.config
        /// </summary>
        /// <param name="queueConfigName">Queue/Topic key name in app.config</param>
        /// <returns></returns>
        public static ActiveMQConnectionSettings CreateSettingsFromConfig(string queueConfigName)
        {
            ActiveMQConnectionSettings settings = new ActiveMQConnectionSettings();
            settings.LoadFromConfig(queueConfigName);
            return settings;
        }
    }

}
