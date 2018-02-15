using System;
using System.Collections.Generic;
using System.Configuration;

namespace Icon.Monitoring.Common.Settings
{
    public class MammothPrimeAffinityControllerMonitorSettings : IMammothPrimeAffinityControllerMonitorSettings
    {
        public Dictionary<string, bool> PrimeAffinityControllerMonitorEnabledByRegion { get; set; }
        public Dictionary<string, DateTime> PrimeAffinityPsgCompletionUtcTimeByRegion { get; set; }

        public void Load()
        {
            this.PrimeAffinityPsgCompletionUtcTimeByRegion = new Dictionary<string, DateTime>
            {
                { "FL", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["PrimeAffinityPsgCompletionUtcTime_FL"])) },
                { "MA", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["PrimeAffinityPsgCompletionUtcTime_MA"])) },
                { "MW", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["PrimeAffinityPsgCompletionUtcTime_MW"])) },
                { "NA", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["PrimeAffinityPsgCompletionUtcTime_NA"])) },
                { "NC", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["PrimeAffinityPsgCompletionUtcTime_NC"])) },
                { "NE", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["PrimeAffinityPsgCompletionUtcTime_NE"])) },
                { "PN", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["PrimeAffinityPsgCompletionUtcTime_PN"])) },
                { "RM", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["PrimeAffinityPsgCompletionUtcTime_RM"])) },
                { "SO", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["PrimeAffinityPsgCompletionUtcTime_SO"])) },
                { "SP", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["PrimeAffinityPsgCompletionUtcTime_SP"])) },
                { "SW", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["PrimeAffinityPsgCompletionUtcTime_SW"])) },
            };
            this.PrimeAffinityControllerMonitorEnabledByRegion = new Dictionary<string, bool>
            {
                { "FL", bool.Parse(ConfigurationManager.AppSettings["PrimeAffinityControllerMonitorEnabled_FL"]) },
                { "MA", bool.Parse(ConfigurationManager.AppSettings["PrimeAffinityControllerMonitorEnabled_MA"]) },
                { "MW", bool.Parse(ConfigurationManager.AppSettings["PrimeAffinityControllerMonitorEnabled_MW"]) },
                { "NA", bool.Parse(ConfigurationManager.AppSettings["PrimeAffinityControllerMonitorEnabled_NA"]) },
                { "NC", bool.Parse(ConfigurationManager.AppSettings["PrimeAffinityControllerMonitorEnabled_NC"]) },
                { "NE", bool.Parse(ConfigurationManager.AppSettings["PrimeAffinityControllerMonitorEnabled_NE"]) },
                { "PN", bool.Parse(ConfigurationManager.AppSettings["PrimeAffinityControllerMonitorEnabled_PN"]) },
                { "RM", bool.Parse(ConfigurationManager.AppSettings["PrimeAffinityControllerMonitorEnabled_RM"]) },
                { "SO", bool.Parse(ConfigurationManager.AppSettings["PrimeAffinityControllerMonitorEnabled_SO"]) },
                { "SP", bool.Parse(ConfigurationManager.AppSettings["PrimeAffinityControllerMonitorEnabled_SP"]) },
                { "SW", bool.Parse(ConfigurationManager.AppSettings["PrimeAffinityControllerMonitorEnabled_SW"]) },
            };
        }
    }
}
