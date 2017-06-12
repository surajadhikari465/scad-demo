using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Monitoring.Common.Settings
{
    public interface IVimLocaleConJobMonitorSettings
    {
        bool EnableVimLocaleConJobMonitor { get; }
        int MinutesAllowedSinceLastVimLocaleCon { get; }
     
    }
}
