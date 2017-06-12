using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Monitoring.Common.Settings
{
    public class VimLocaleConJobMonitorSettings : IVimLocaleConJobMonitorSettings
    {
        public bool EnableVimLocaleConJobMonitor
        {
            get
            {
               return AppSettingsAccessor.GetBoolSetting(nameof(EnableVimLocaleConJobMonitor));
            }
        }

        public int MinutesAllowedSinceLastVimLocaleCon
        {
            get
            {
                return AppSettingsAccessor.GetIntSetting(nameof(MinutesAllowedSinceLastVimLocaleCon));
            }
        }
    }
}
