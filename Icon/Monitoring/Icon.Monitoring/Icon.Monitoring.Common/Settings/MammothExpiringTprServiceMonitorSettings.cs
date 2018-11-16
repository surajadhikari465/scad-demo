using System;
using System.Collections.Generic;
using System.Configuration;

namespace Icon.Monitoring.Common.Settings
{
    public class MammothExpiringTprServiceMonitorSettings : IMammothExpiringTprServiceMonitorSettings
    {
        public Dictionary<string, bool> ExpiringTprServiceMonitorEnabledByRegion { get; set; }
        public Dictionary<string, DateTime> ExpiringTprServiceCompletionUtcTimeByRegion { get; set; }

        public void Load()
        {
            this.ExpiringTprServiceCompletionUtcTimeByRegion = new Dictionary<string, DateTime>
            {
                { "FL", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["ExpiringTprServiceCompletionUtcTime_FL"])) },
                { "MA", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["ExpiringTprServiceCompletionUtcTime_MA"])) },
                { "MW", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["ExpiringTprServiceCompletionUtcTime_MW"])) },
                { "NA", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["ExpiringTprServiceCompletionUtcTime_NA"])) },
                { "NC", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["ExpiringTprServiceCompletionUtcTime_NC"])) },
                { "NE", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["ExpiringTprServiceCompletionUtcTime_NE"])) },
                { "PN", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["ExpiringTprServiceCompletionUtcTime_PN"])) },
                { "RM", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["ExpiringTprServiceCompletionUtcTime_RM"])) },
                { "SO", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["ExpiringTprServiceCompletionUtcTime_SO"])) },
                { "SP", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["ExpiringTprServiceCompletionUtcTime_SP"])) },
                { "SW", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["ExpiringTprServiceCompletionUtcTime_SW"])) },
            };
            this.ExpiringTprServiceMonitorEnabledByRegion = new Dictionary<string, bool>
            {
                { "FL", bool.Parse(ConfigurationManager.AppSettings["ExpiringTprServiceMonitorEnabled_FL"]) },
                { "MA", bool.Parse(ConfigurationManager.AppSettings["ExpiringTprServiceMonitorEnabled_MA"]) },
                { "MW", bool.Parse(ConfigurationManager.AppSettings["ExpiringTprServiceMonitorEnabled_MW"]) },
                { "NA", bool.Parse(ConfigurationManager.AppSettings["ExpiringTprServiceMonitorEnabled_NA"]) },
                { "NC", bool.Parse(ConfigurationManager.AppSettings["ExpiringTprServiceMonitorEnabled_NC"]) },
                { "NE", bool.Parse(ConfigurationManager.AppSettings["ExpiringTprServiceMonitorEnabled_NE"]) },
                { "PN", bool.Parse(ConfigurationManager.AppSettings["ExpiringTprServiceMonitorEnabled_PN"]) },
                { "RM", bool.Parse(ConfigurationManager.AppSettings["ExpiringTprServiceMonitorEnabled_RM"]) },
                { "SO", bool.Parse(ConfigurationManager.AppSettings["ExpiringTprServiceMonitorEnabled_SO"]) },
                { "SP", bool.Parse(ConfigurationManager.AppSettings["ExpiringTprServiceMonitorEnabled_SP"]) },
                { "SW", bool.Parse(ConfigurationManager.AppSettings["ExpiringTprServiceMonitorEnabled_SW"]) },
            };
        }
    }
}
