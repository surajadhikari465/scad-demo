using System;
using System.Collections.Generic;

namespace Icon.Monitoring.Common.Settings
{
    public interface IMammothActivePriceServiceMonitorSettings
    {
        Dictionary<string, bool> ActivePriceServiceMonitorEnabledByRegion { get; set; }
        Dictionary<string, DateTime> ActivePriceServiceCompletionUtcTimeByRegion { get; set; }
        void Load();
    }
}
