using System.Configuration;

namespace BulkItemUploadProcessor.Common
{
    public class BulkItemUploadProcessorSettings
    {
        private const int DefaultRetryCount = 3;
        private const int DefaultRetryAddUpdateMillisecondDelay = 3000;

        public int RetryAddUpdateCount { get; set; }
        public int RetryAddUpdateMillisecondDelay { get; set; }

        public static BulkItemUploadProcessorSettings Load()
        {
            if (!int.TryParse(ConfigurationManager.AppSettings[nameof(RetryAddUpdateCount)], out int retryCount))
            {
                retryCount = DefaultRetryCount;
            }
            if (!int.TryParse(ConfigurationManager.AppSettings[nameof(RetryAddUpdateMillisecondDelay)], out int retryAddUpdateMillisecondDelay))
            {
                retryAddUpdateMillisecondDelay = DefaultRetryAddUpdateMillisecondDelay;
            }
            return new BulkItemUploadProcessorSettings
            {
                RetryAddUpdateCount = retryCount,
                RetryAddUpdateMillisecondDelay = retryAddUpdateMillisecondDelay
            };
        }
    }
}