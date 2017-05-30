using Mammoth.Common;
using Mammoth.Common.ControllerApplication;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.ItemLocale.Controller
{
    public class ItemLocaleControllerApplicationSettings : IControllerApplicationSettings, IRegionalControllerApplicationSettings
    {
        public int Instance { get; set; }
        public int MaxNumberOfRowsToMark { get; set; }
        public List<string> Regions { get; set; }
        public string CurrentRegion { get; set; }
        public string ControllerName { get; set; }
        public string UriBaseAddress { get; set; }
        public int ApiRowLimit { get; set; }
        public List<string> NonAlertErrors { get; set; }

        public static ItemLocaleControllerApplicationSettings CreateFromConfig()
        {
            return new ItemLocaleControllerApplicationSettings
            {
                ControllerName = "ItemLocale",
                Instance = AppSettingsAccessor.GetIntSetting("InstanceID"),
                MaxNumberOfRowsToMark = AppSettingsAccessor.GetIntSetting(nameof(MaxNumberOfRowsToMark)),
                Regions = AppSettingsAccessor.GetStringSetting("RegionsToProcess").Split(',').ToList(),
                UriBaseAddress = AppSettingsAccessor.GetStringSetting("BaseAddress"),
                ApiRowLimit = AppSettingsAccessor.GetIntSetting(nameof(ApiRowLimit)),
                NonAlertErrors = AppSettingsAccessor.GetStringSetting(nameof(NonAlertErrors)).Split(',').ToList()
            };
        }
    }
}
