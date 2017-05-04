using NodaTime;
using System;
using System.Collections.Generic;

namespace Icon.Monitoring.Common.Settings
{
    public interface IMonitorSettings
    {
        Dictionary<string, TimeSpan> MonitorTimers { get; set; }
        string IntegrationKey { get; set; }

        List<string>  ApiControllerMonitorRegions { get; set; }
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

        LocalTime StoreOpenCentralTime_FL { get; set; }
        LocalTime StoreOpenCentralTime_MA { get; set; }
        LocalTime StoreOpenCentralTime_MW { get; set; }
        LocalTime StoreOpenCentralTime_NA { get; set; }
        LocalTime StoreOpenCentralTime_NC { get; set; }
        LocalTime StoreOpenCentralTime_NE { get; set; }
        LocalTime StoreOpenCentralTime_PN { get; set; }
        LocalTime StoreOpenCentralTime_RM { get; set; }
        LocalTime StoreOpenCentralTime_SO { get; set; }
        LocalTime StoreOpenCentralTime_SP { get; set; }
        LocalTime StoreOpenCentralTime_SW { get; set; }
        LocalTime StoreOpenCentralTime_UK { get; set; }
        int NumberOfMinutesBeforeStoreOpens { get; set; }
        LocalTime ApiControllerMonitorBlackoutStart { get; set; }
        LocalTime ApiControllerMonitorBlackoutEnd { get; set; }
        string ApiControllerMonitorBlackoutDay { get; set; }

    }
}