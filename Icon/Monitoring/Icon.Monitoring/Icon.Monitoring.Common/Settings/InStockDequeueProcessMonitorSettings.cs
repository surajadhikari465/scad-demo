using System;
using System.Collections.Generic;
using System.Linq;


namespace Icon.Monitoring.Common.Settings
{
    public class InStockDequeueProcessMonitorSettings: IInStockDequeueProcessMonitorSettings
    {
        public int NumberOfMaximumMinutesRecordCanBeInUnprocessedStatus => AppSettingsAccessor.GetIntSetting(nameof(NumberOfMaximumMinutesRecordCanBeInUnprocessedStatus));
        public List<string> InStockDequeueProcessRegions => AppSettingsAccessor.GetStringSetting(nameof(InStockDequeueProcessRegions)).Split(',').ToList();

    }
}
