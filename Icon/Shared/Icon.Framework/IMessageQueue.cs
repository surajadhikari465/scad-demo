using System;

namespace Icon.Framework
{
    public interface IMessageQueue
    {
        int MessageQueueId { get; set; }
        int? MessageHistoryId { get; set; }
        int MessageStatusId { get; set; }
        int? InProcessBy { get; set; }
        DateTime? ProcessedDate { get; set; }
    }

    public interface IIrmaPushData
    {
        int IRMAPushID { get; set; }
        string ScanCode { get; set; }
        string RegionCode { get; set; }
    }

    public interface IItemMessageQueue : IMessageQueue
    {
        int ItemId { get; set; }
    }

    public partial class MessageQueueProduct : IMessageQueue, IItemMessageQueue { }
    public partial class MessageQueueItemLocale : IMessageQueue, IItemMessageQueue, IIrmaPushData { }
    public partial class MessageQueueHierarchy : IMessageQueue { }
    public partial class MessageQueuePrice : IMessageQueue, IItemMessageQueue, IIrmaPushData { }
    public partial class MessageQueueLocale : IMessageQueue { }
    public partial class MessageQueueProductSelectionGroup : IMessageQueue { }
}
