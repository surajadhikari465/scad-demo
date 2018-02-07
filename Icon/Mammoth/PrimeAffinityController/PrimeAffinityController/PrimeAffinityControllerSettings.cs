using Icon.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrimeAffinityController
{
    public class PrimeAffinityControllerSettings
    {
        public List<string> Regions { get; set; }
        public List<int> ExcludedPSNumbers { get; set; }
        public List<string> PriceTypes { get; set; }
        public double RunInterval { get; set; }
        public DayOfWeek MaintenanceDay { get; set; }
        public TimeSpan MaintenanceStartTime { get; set; }
        public TimeSpan MaintenanceEndTime { get; set; }
        public string JobName { get; set; }

        public static PrimeAffinityControllerSettings Load()
        {
            return new PrimeAffinityControllerSettings
            {
                Regions = AppSettingsAccessor.GetStringSetting(nameof(Regions)).Split(',').Select(r => r.Trim()).ToList(),
                ExcludedPSNumbers = AppSettingsAccessor.GetStringSetting(nameof(ExcludedPSNumbers)).Split(',').Select(ps => int.Parse(ps)).ToList(),
                PriceTypes = AppSettingsAccessor.GetStringSetting(nameof(PriceTypes)).Split(',').Select(pt => pt.Trim()).ToList(),
                RunInterval = AppSettingsAccessor.GetIntSetting(nameof(RunInterval)),
                MaintenanceDay = AppSettingsAccessor.GetEnumSetting<DayOfWeek>(nameof(MaintenanceDay)),
                MaintenanceStartTime = TimeSpan.Parse(AppSettingsAccessor.GetStringSetting(nameof(MaintenanceStartTime))),
                MaintenanceEndTime = TimeSpan.Parse(AppSettingsAccessor.GetStringSetting(nameof(MaintenanceEndTime))),
                JobName = AppSettingsAccessor.GetStringSetting("ServiceDisplayName"),
            };
        }
    }
}
