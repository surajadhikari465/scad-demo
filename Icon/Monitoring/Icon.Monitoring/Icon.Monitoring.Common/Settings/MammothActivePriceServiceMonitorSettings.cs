using System;
using System.Collections.Generic;
using System.Configuration;

namespace Icon.Monitoring.Common.Settings
{
    public class MammothActivePriceServiceMonitorSettings : IMammothActivePriceServiceMonitorSettings
    {
        public Dictionary<string, bool> ActivePriceServiceMonitorEnabledByRegion { get; set; }
        public Dictionary<string, DateTime> ActivePriceServiceCompletionUtcTimeByRegion { get; set; }

        public void Load()
        {
            this.ActivePriceServiceCompletionUtcTimeByRegion = new Dictionary<string, DateTime>
            {
                { "FL", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["ActivePriceServiceCompletionUtcTime_FL"])) },
                { "MA", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["ActivePriceServiceCompletionUtcTime_MA"])) },
                { "MW", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["ActivePriceServiceCompletionUtcTime_MW"])) },
                { "NA", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["ActivePriceServiceCompletionUtcTime_NA"])) },
                { "NC", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["ActivePriceServiceCompletionUtcTime_NC"])) },
                { "NE", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["ActivePriceServiceCompletionUtcTime_NE"])) },
                { "PN", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["ActivePriceServiceCompletionUtcTime_PN"])) },
                { "RM", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["ActivePriceServiceCompletionUtcTime_RM"])) },
                { "SO", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["ActivePriceServiceCompletionUtcTime_SO"])) },
                { "SP", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["ActivePriceServiceCompletionUtcTime_SP"])) },
                { "SW", DateTime.UtcNow.Date.Add(TimeSpan.Parse(ConfigurationManager.AppSettings["ActivePriceServiceCompletionUtcTime_SW"])) },
            };
            this.ActivePriceServiceMonitorEnabledByRegion = new Dictionary<string, bool>
            {
                { "FL", bool.Parse(ConfigurationManager.AppSettings["ActivePriceServiceMonitorEnabled_FL"]) },
                { "MA", bool.Parse(ConfigurationManager.AppSettings["ActivePriceServiceMonitorEnabled_MA"]) },
                { "MW", bool.Parse(ConfigurationManager.AppSettings["ActivePriceServiceMonitorEnabled_MW"]) },
                { "NA", bool.Parse(ConfigurationManager.AppSettings["ActivePriceServiceMonitorEnabled_NA"]) },
                { "NC", bool.Parse(ConfigurationManager.AppSettings["ActivePriceServiceMonitorEnabled_NC"]) },
                { "NE", bool.Parse(ConfigurationManager.AppSettings["ActivePriceServiceMonitorEnabled_NE"]) },
                { "PN", bool.Parse(ConfigurationManager.AppSettings["ActivePriceServiceMonitorEnabled_PN"]) },
                { "RM", bool.Parse(ConfigurationManager.AppSettings["ActivePriceServiceMonitorEnabled_RM"]) },
                { "SO", bool.Parse(ConfigurationManager.AppSettings["ActivePriceServiceMonitorEnabled_SO"]) },
                { "SP", bool.Parse(ConfigurationManager.AppSettings["ActivePriceServiceMonitorEnabled_SP"]) },
                { "SW", bool.Parse(ConfigurationManager.AppSettings["ActivePriceServiceMonitorEnabled_SW"]) },
            };
        }
    }
}
