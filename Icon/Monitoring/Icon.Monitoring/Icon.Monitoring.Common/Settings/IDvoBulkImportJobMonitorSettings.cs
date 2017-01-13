using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Monitoring.Common.Settings
{
    public interface IDvoBulkImportJobMonitorSettings
    {
        bool EnableDvoBulkImportJobMonitor { get; }
        string DvoDirectoryPath { get; }
        int DvoBulkImportFileMaxMinuteThreshold { get; }
        List<string> DvoBulkImportJobMonitorRegions { get; }
        DateTime DvoBulkImportJobMonitorBlackoutStart { get; }
        DateTime DvoBulkImportJobMonitorBlackoutEnd { get; }
    }
}
