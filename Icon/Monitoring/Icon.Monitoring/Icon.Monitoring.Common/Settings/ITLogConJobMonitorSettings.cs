using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Monitoring.Common.Settings
{
    public interface ITLogConJobMonitorSettings
    {
        bool EnableTLogConJobMonitor { get; }
        int MaxLastTLogConJobLogTime { get; }
        int ItemMovementMaxRows { get; }
        bool EnableItemMovementTableCheck { get; }
    }
}
