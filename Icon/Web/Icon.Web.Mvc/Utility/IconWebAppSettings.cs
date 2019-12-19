using Icon.Common;

namespace Icon.Web.Mvc.Utility
{
    public class IconWebAppSettings
    {
        public string WriteAccessGroups { get; set; }
        public string ReadAccessGroups { get; set; }
        public string AdminAccessGroups { get; set; }
        public bool IsManufacturerHierarchyMessage { get; set; }
        
        public static IconWebAppSettings CreateSettingsFromConfig()
        {
            return new IconWebAppSettings
            {
                WriteAccessGroups = AppSettingsAccessor.GetStringSetting("WriteAccess",false),
                ReadAccessGroups = AppSettingsAccessor.GetStringSetting("ReadAccess",false),
                AdminAccessGroups = AppSettingsAccessor.GetStringSetting("AdminAccess",false),
                IsManufacturerHierarchyMessage = AppSettingsAccessor.GetBoolSetting("EnableManufacturerHierarchyMessages", false)
            };
        }
    }
}