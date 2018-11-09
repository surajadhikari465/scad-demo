using Icon.Common;
using System.Collections.Generic;
using System.Linq;

namespace Icon.ApiController.Common
{
    public class ApiControllerSettings
    {
        public string Source { get; set; }
        public int Instance { get; set; }
        public int QueueLookAhead { get; set; }
        public int MiniBulkLimitItemLocale { get; set; }
        public int MiniBulkLimitPrice { get; set; }
        public List<string> ConfiguredBusinessUnits { get; set; }
        public string NonReceivingSystemsAll { get; set; }
        public string NonReceivingSystemsItemLocale { get; set; }
        public string NonReceivingSystemsPrice { get; set; }
        public string NonReceivingSystemsProduct { get; set; }
        public string NonReceivingSystemsHierarchy { get; set; }
        public bool EnableNationalHierarchy { get; set; }

        public static ApiControllerSettings CreateFromConfig(string source, int instance)
        {
            return new ApiControllerSettings
            {
                Source = source,
                Instance = instance,
                QueueLookAhead = AppSettingsAccessor.GetIntSetting("QueueLookAhead", 1000),
                MiniBulkLimitItemLocale = AppSettingsAccessor.GetIntSetting("MiniBulkLimitItemLocale", 100),
                MiniBulkLimitPrice = AppSettingsAccessor.GetIntSetting("MiniBulkLimitPrice", 100),
                ConfiguredBusinessUnits = AppSettingsAccessor.GetStringSetting("ConfiguredBusinessUnits", string.Empty).Split(',').ToList(),
                NonReceivingSystemsAll = AppSettingsAccessor.GetStringSetting("NonReceivingSystemsAll", false),
                NonReceivingSystemsItemLocale = AppSettingsAccessor.GetStringSetting("NonReceivingSystemsItemLocale", false),
                NonReceivingSystemsPrice = AppSettingsAccessor.GetStringSetting("NonReceivingSystemsPrice", false),
                NonReceivingSystemsProduct = AppSettingsAccessor.GetStringSetting("NonReceivingSystemsProduct", false),
                NonReceivingSystemsHierarchy = AppSettingsAccessor.GetStringSetting("NonReceivingSystemsHierarchy", false),
                EnableNationalHierarchy = AppSettingsAccessor.GetBoolSetting(nameof(EnableNationalHierarchy), false)
            };
        }
    }
}