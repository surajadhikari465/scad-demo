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
        bool EnableItemMovementTableCheck { get; }
        int MinutesAllowedSinceLastTLogCon { get; }
        int ItemMovementMaximumRows { get; }
     
    }
}
