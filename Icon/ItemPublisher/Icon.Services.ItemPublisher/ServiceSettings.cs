using Icon.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Services.ItemPublisher.Services
{
    /// <summary>
    /// Settings used by the NewItem service
    /// </summary>
    public class ServiceSettings
    {
        /// <summary>
        /// The number of records that will be pulled from the queue table each call.
        /// </summary>
        public int BatchSize { get; set; }

        /// <summary>
        /// How often the timer that watches for queue records is fired.
        /// </summary>
        public int TimerIntervalInMilliseconds { get; set; }

        /// <summary>
        /// A List of systems that will be included in the ESB header for property NonReceivingSystemsJmsProperty
        /// </summary>
        public List<string> NonReceivingSystemsProduct { get; set; } = new List<string>();

        /// <summary>
        /// A list of all possible receiving systems
        /// </summary>
        public List<string> AllReceivingSystemsProduct { get; set; } = new List<string>();

        public bool IncludeManufacturerHierarchy { get; set; }

        /// <summary>
        /// How often the cache will refresh
        /// </summary>
        public int TimerIntervalCacheRefreshInMilliseconds { get; set; }

        public void LoadSettings()
        {
            this.BatchSize = AppSettingsAccessor.GetIntSetting("BatchSize");
            this.TimerIntervalInMilliseconds = AppSettingsAccessor.GetIntSetting("TimerIntervalInSeconds") * 1000;
            this.TimerIntervalCacheRefreshInMilliseconds = AppSettingsAccessor.GetIntSetting("TimerIntervalCacheRefreshInSeconds") * 1000;
            this.NonReceivingSystemsProduct = AppSettingsAccessor.GetStringSetting("NonReceivingSystemsProductCSV", false).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            this.AllReceivingSystemsProduct = AppSettingsAccessor.GetStringSetting("AllReceivingSystemsProductCSV", false).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            this.IncludeManufacturerHierarchy = AppSettingsAccessor.GetBoolSetting("IncludeManufacturerHierarchy");
        }
    }
}