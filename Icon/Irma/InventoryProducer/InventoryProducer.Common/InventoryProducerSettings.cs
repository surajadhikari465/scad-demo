using Icon.Common;
using System.Linq;

namespace InventoryProducer.Common
{
    public class InventoryProducerSettings
    {
        public string Source { get; set; }
        public int Instance { get; set; }
        public int DequeueMaxRecords { get; set; }
        public int DequeueMinuteOffset { get; set; }
        public string InstockDequeueStoredProcedureName { get; set; }
        public string NonReceivingSystemsSpoilage { get; set; }
        public string NonReceivingSystemsTransferOrder { get; set; }
        public string ProducerType { get; set; }
        public string TransactionType { get; set; }
        public string RegionCode { get; set; }
        public string InstanceGUID { get; set; }
        public int DbCommandTimeoutInSeconds { get; set; }
        public int DbErrorRetryCount { get; set; }
        public int DbRetryDelayInMilliseconds { get; set; }
        public int BatchSize { get; set; }
        public int ServiceMaxRetryCount { get; set; }
        public int ServiceMaxRetryDelayInMilliseconds { get; set; }

        public static InventoryProducerSettings CreateFromConfig(string sourceConfigValue, int instanceConfigValue)
        {
            return new InventoryProducerSettings
            {
                Source = sourceConfigValue,
                Instance = instanceConfigValue,
                ProducerType = AppSettingsAccessor.GetStringSetting("ProducerType", string.Empty),
                NonReceivingSystemsSpoilage = AppSettingsAccessor.GetStringSetting("NonReceivingSystemsSpoilage", false),
                NonReceivingSystemsTransferOrder = AppSettingsAccessor.GetStringSetting("NonReceivingSystemsTransferOrder", string.Empty),
                DequeueMaxRecords = AppSettingsAccessor.GetIntSetting("DequeueMaxRecords"),
                DequeueMinuteOffset = AppSettingsAccessor.GetIntSetting("DequeueMinuteOffset"),
                InstockDequeueStoredProcedureName = AppSettingsAccessor.GetStringSetting("InstockDequeueStoredProcedureName"),
                TransactionType = AppSettingsAccessor.GetStringSetting("TransactionType"),
                RegionCode = AppSettingsAccessor.GetStringSetting("RegionCode"),
                InstanceGUID = AppSettingsAccessor.GetStringSetting("InstanceGUID"),
                DbCommandTimeoutInSeconds = AppSettingsAccessor.GetIntSetting("DbCommandTimeoutInSeconds"),
                DbErrorRetryCount = AppSettingsAccessor.GetIntSetting("DbErrorRetryCount"),
                DbRetryDelayInMilliseconds = AppSettingsAccessor.GetIntSetting("DbRetryDelayInMilliseconds"),
                BatchSize = AppSettingsAccessor.GetIntSetting("BatchSize"),
                ServiceMaxRetryCount = AppSettingsAccessor.GetIntSetting("ServiceMaxRetryCount"),
                ServiceMaxRetryDelayInMilliseconds = AppSettingsAccessor.GetIntSetting("ServiceMaxRetryDelayInMilliseconds")
            };
        }
    }
}
