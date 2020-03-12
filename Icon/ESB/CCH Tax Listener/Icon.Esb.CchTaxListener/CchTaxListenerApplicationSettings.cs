using Icon.Common;
using Icon.Esb.ListenerApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.CchTax
{
    public class CchTaxListenerApplicationSettings : ListenerApplicationSettings
    {
        public bool UpdateMammoth { get; set; }
        public bool GenerateGlobalEvents { get; set; }

        protected override void LoadSubSettings()
        {
            UpdateMammoth = AppSettingsAccessor.GetBoolSetting("UpdateMammoth", false);
            GenerateGlobalEvents = AppSettingsAccessor.GetBoolSetting("GenerateGlobalEvents", false);
            ListenerApplicationName = "CCHTaxListener";
        }
    }
}
