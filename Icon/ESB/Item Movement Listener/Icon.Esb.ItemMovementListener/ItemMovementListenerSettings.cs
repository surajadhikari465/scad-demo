using Icon.Common;

namespace Icon.Esb.ItemMovement
{
    public class ItemMovementListenerSettings
    {
        public bool PerformanceLoggingEnabled { get; set; }
        public int MessageQueueSize { get; set; }

        public static ItemMovementListenerSettings CreateSettings()
        {
            var settings = new ItemMovementListenerSettings
            {
                MessageQueueSize = AppSettingsAccessor.GetIntSetting("MessageQueueSize"),
                PerformanceLoggingEnabled = AppSettingsAccessor.GetBoolSetting("PerformanceLoggingEnabled", required: true)
            };

            return settings;
        }
    }
}
