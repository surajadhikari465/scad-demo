using System;
using System.Collections.Generic;

namespace Icon.Monitoring.Common.Settings
{
    public interface IMammothPrimeAffinityControllerMonitorSettings
    {
        Dictionary<string, bool> PrimeAffinityControllerMonitorEnabledByRegion { get; set; }
        Dictionary<string, DateTime> PrimeAffinityPsgCompletionUtcTimeByRegion { get; set; }
        void Load();
    }
}
