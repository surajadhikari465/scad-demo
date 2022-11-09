using System;
using Icon.Common;

namespace Icon.Dvs
{
    public class DvsListenerSettings
    {
        private static readonly int DEFAULT_LISTENER_THREADS = 1;
        private static readonly int DEFAULT_SQS_TIMEOUT = 3;
        private static readonly int DEFAULT_POLL_INTERVAL = 10000;

        public string ListenerApplicationName { get; set; }
        public int NumberOfListenerThreads { get; set; } = DEFAULT_LISTENER_THREADS;
        public string SqsQueueUrl { get; set; }
        public string AwsAccessKey { get; set; }
        public string AwsSecretKey { get; set; }
        public string Region { get; set; }
        public int SqsTimeout { get; set; } = DEFAULT_SQS_TIMEOUT;
        public int PollInterval { get; set; } = DEFAULT_POLL_INTERVAL;

        public static DvsListenerSettings CreateSettingsFromConfig()
        {
            DvsListenerSettings settings = new DvsListenerSettings()
            {
                ListenerApplicationName = AppSettingsAccessor.GetStringSetting("ListenerApplicationName"),
                SqsQueueUrl = AppSettingsAccessor.GetStringSetting("SqsQueueUrl"),
                AwsAccessKey = AppSettingsAccessor.GetStringSetting("AwsAccessKey"),
                AwsSecretKey = AppSettingsAccessor.GetStringSetting("AwsSecretKey"),
                Region = AppSettingsAccessor.GetStringSetting("Region")
            };
            int numberOfListenerThreads = AppSettingsAccessor.GetIntSetting("NumberOfListnerThreads", false);
            int sqsTimeout = AppSettingsAccessor.GetIntSetting("SqsTimeout", false);
            int pollInterval = AppSettingsAccessor.GetIntSetting("PollInterval", false);

            settings.NumberOfListenerThreads = numberOfListenerThreads > 0? numberOfListenerThreads : DEFAULT_LISTENER_THREADS;
            settings.SqsTimeout = sqsTimeout > 0? sqsTimeout : DEFAULT_SQS_TIMEOUT;
            settings.PollInterval = pollInterval > 0 ? pollInterval : DEFAULT_POLL_INTERVAL;
            return settings;
        }
    }
}
