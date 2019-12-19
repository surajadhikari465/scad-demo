using Icon.Monitoring.Common.Enums;
using System;
using System.Collections.Generic;

namespace Icon.Monitoring.Common
{
    public class MessageQueueTypes
    {
        public const string Product = "Product";
        public const string Price = "Price";
        public const string ItemLocale = "ItemLocale";
        public const string Hierarchy = "Hierarchy";
        public const string Locale = "Locale";
        public const string ProductSelectionGroup = "ProductSelectionGroup";
        public const string Attribute = "Attribute";
        public const string Item = "Item";
    }

    public class QueueData
    {
        public int? LastMessageQueueId { get; set; }
        public int NumberOfTimesMatched { get; set; }
    }

    public static class JobStatusCache
    {
        public static Dictionary<string, DateTime> OpsgenieTracker = new Dictionary<string, DateTime>();
    }

    public static class MammothPriceChangeQueueCache
    {
        public static Dictionary<IrmaRegions, QueueData> IrmaRegionMapper = new Dictionary<IrmaRegions, QueueData>
        {
            { IrmaRegions.FL, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
            { IrmaRegions.MA, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
            { IrmaRegions.MW, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
            { IrmaRegions.NA, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
            { IrmaRegions.NC, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
            { IrmaRegions.NE, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
            { IrmaRegions.PN, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
            { IrmaRegions.RM, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
            { IrmaRegions.SO, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
            { IrmaRegions.SP, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
            { IrmaRegions.SW, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
            { IrmaRegions.UK, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } }
        };
    }

    public static class MammothItemLocaleChangeQueueCache
    {
        public static Dictionary<IrmaRegions, QueueData> IrmaRegionMapper = new Dictionary<IrmaRegions, QueueData>
        {
            { IrmaRegions.FL, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
            { IrmaRegions.MA, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
            { IrmaRegions.MW, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
            { IrmaRegions.NA, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
            { IrmaRegions.NC, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
            { IrmaRegions.NE, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
            { IrmaRegions.PN, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
            { IrmaRegions.RM, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
            { IrmaRegions.SO, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
            { IrmaRegions.SP, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
            { IrmaRegions.SW, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } },
            { IrmaRegions.UK, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 } }
        };
    }
}
