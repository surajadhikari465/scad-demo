using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Monitoring.Common.Settings
{
    public class TLogConJobMonitorSettings : ITLogConJobMonitorSettings
    {
        public bool EnableTLogConJobMonitor
        {
            get
            {
               return AppSettingsAccessor.GetBoolSetting(nameof(EnableTLogConJobMonitor));
            }
        }

        public bool EnableItemMovementTableCheck
        {
            get
            {
                return AppSettingsAccessor.GetBoolSetting(nameof(EnableItemMovementTableCheck));
            }
        }

        public int ItemMovementMaximumRows
        {
            get
            {
                return AppSettingsAccessor.GetIntSetting(nameof(ItemMovementMaximumRows));
            }
        }

        public int MinutesAllowedSinceLastTconLog
        {
            get
            {
                return AppSettingsAccessor.GetIntSetting(nameof(MinutesAllowedSinceLastTconLog));
            }
        }
    }
}
