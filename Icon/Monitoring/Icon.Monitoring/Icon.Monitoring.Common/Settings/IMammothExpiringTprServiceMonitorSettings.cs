using System;
using System.Collections.Generic;

namespace Icon.Monitoring.Common.Settings
{
    public interface IMammothExpiringTprServiceMonitorSettings
    {
        Dictionary<string, bool> ExpiringTprServiceMonitorEnabledByRegion { get; set; }
        Dictionary<string, DateTime> ExpiringTprServiceCompletionUtcTimeByRegion { get; set; }
        void Load();
    }
}
