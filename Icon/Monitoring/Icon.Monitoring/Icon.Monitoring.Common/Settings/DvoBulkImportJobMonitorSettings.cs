using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Monitoring.Common.Settings
{
    public class DvoBulkImportJobMonitorSettings : IDvoBulkImportJobMonitorSettings
    {
        public bool EnableDvoBulkImportJobMonitor => AppSettingsAccessor.GetBoolSetting(nameof(EnableDvoBulkImportJobMonitor));
        public int DvoBulkImportFileMaxMinuteThreshold => AppSettingsAccessor.GetIntSetting(nameof(DvoBulkImportFileMaxMinuteThreshold));
        public DateTime DvoBulkImportJobMonitorBlackoutEnd => DateTime.Today.Add(TimeSpan.Parse(AppSettingsAccessor.GetStringSetting(nameof(DvoBulkImportJobMonitorBlackoutEnd))));
        public DateTime DvoBulkImportJobMonitorBlackoutStart => DateTime.Today.Add(TimeSpan.Parse(AppSettingsAccessor.GetStringSetting(nameof(DvoBulkImportJobMonitorBlackoutStart))));
        public List<string> DvoBulkImportJobMonitorRegions => AppSettingsAccessor.GetStringSetting(nameof(DvoBulkImportJobMonitorRegions)).Split(',').ToList();
        public string DvoDirectoryPath => AppSettingsAccessor.GetStringSetting(nameof(DvoDirectoryPath));
    }
}
