using Icon.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.Utility
{
    public class IconWebAppSettings
    {
        public bool IconInterfaceEnabled { get; set; }
        public string WriteAccessGroups { get; set; }
        public string TraitWriteAccessGroups { get; set; }
        public string AdminAccessGroups { get; set; }

        public static IconWebAppSettings CreateSettingsFromConfig()
        {
            return new IconWebAppSettings
            {
                IconInterfaceEnabled = AppSettingsAccessor.GetIntSetting("InforDisableIconInterface", 1) == 0 ? true : false,
                WriteAccessGroups = AppSettingsAccessor.GetStringSetting("WriteAccess",false),
                TraitWriteAccessGroups = AppSettingsAccessor.GetStringSetting("WriteTraitAccess",false),
                AdminAccessGroups = AppSettingsAccessor.GetStringSetting("AdminAccess",false)
            };
        }
    }
}