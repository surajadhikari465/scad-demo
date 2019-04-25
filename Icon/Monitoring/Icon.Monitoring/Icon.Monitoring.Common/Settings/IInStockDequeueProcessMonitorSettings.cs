using System;
using System.Collections.Generic;

namespace Icon.Monitoring.Common.Settings
{
    public interface IInStockDequeueProcessMonitorSettings
    {
         int NumberOfMaximumMinutesRecordCanBeInUnprocessedStatus { get; }
         List<string> InStockDequeueProcessRegions { get; }
    }
}
