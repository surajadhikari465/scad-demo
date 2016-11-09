using NodaTime;
using System;
using System.Collections.Generic;

namespace Icon.Monitoring.Common.Settings
{
    public interface IMonitorSettings
    {
        Dictionary<string, TimeSpan> MonitorTimers { get; set; }
        string IntegrationKey { get; set; }
        bool SendPagerDutyNotifications { get; set; }
        string PagerDutyUri { get; set; }
        string Environment { get; set; }
        double MonitorServiceTimer { get; set; }
        LocalTime PosPushStartTime_FL { get; set; }
        LocalTime PosPushStartTime_MA { get; set; }
        LocalTime PosPushStartTime_MW { get; set; }
        LocalTime PosPushStartTime_NA { get; set; }
        LocalTime PosPushStartTime_NC { get; set; }
        LocalTime PosPushStartTime_NE { get; set; }
        LocalTime PosPushStartTime_PN { get; set; }
        LocalTime PosPushStartTime_RM { get; set; }
        LocalTime PosPushStartTime_SO { get; set; }
        LocalTime PosPushStartTime_SP { get; set; }
        LocalTime PosPushStartTime_SW { get; set; }
        LocalTime PosPushStartTime_UK { get; set; }
    }
}