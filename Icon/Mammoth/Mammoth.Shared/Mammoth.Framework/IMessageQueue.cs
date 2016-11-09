using System;

namespace Mammoth.Framework
{

    public interface IMessageQueue
    {
        int MessageQueueId { get; set; }
        int? MessageHistoryId { get; set; }
        int MessageStatusId { get; set; }
        int? InProcessBy { get; set; }
        DateTime? ProcessedDate { get; set; }
    }
}
