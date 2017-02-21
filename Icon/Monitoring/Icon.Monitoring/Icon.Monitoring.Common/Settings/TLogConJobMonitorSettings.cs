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

        public int ItemMovementMaxRows
        {
            get
            {
                return AppSettingsAccessor.GetIntSetting(nameof(ItemMovementMaxRows));
            }
        }

        public int MaxLastTLogConJobLogTime
        {
            get
            {
                return AppSettingsAccessor.GetIntSetting(nameof(MaxLastTLogConJobLogTime));
            }
        }
    }
}
