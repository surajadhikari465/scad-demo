using Mammoth.Common;
using Mammoth.Common.ControllerApplication;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.Price.Controller
{
    public class PriceControllerApplicationSettings : IControllerApplicationSettings, IRegionalControllerApplicationSettings
    {
        public int Instance { get; set; }
        public int MaxNumberOfRowsToMark { get; set; }
        public List<string> Regions { get; set; }
        public string CurrentRegion { get; set; }
        public string ControllerName { get; set; }
        public List<string> NonAlertErrors { get; set; }
        public static PriceControllerApplicationSettings CreateFromConfig()
        {
            return new PriceControllerApplicationSettings
            {
                ControllerName = "Price",
                Instance = AppSettingsAccessor.GetIntSetting("InstanceID"),
                MaxNumberOfRowsToMark = AppSettingsAccessor.GetIntSetting("MaxNumberOfRowsToMark"),
                Regions = AppSettingsAccessor.GetStringSetting("RegionsToProcess").Split(',').ToList(),
                NonAlertErrors = AppSettingsAccessor.GetStringSetting(nameof(NonAlertErrors)).Split(',').ToList()
            };
        }
    }
}