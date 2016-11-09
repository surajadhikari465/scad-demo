using System;

namespace Vim.Common.DataAcess.Tests
{
    public class EventQueueModel
    {
        public int QueueId { get; set; }
        public int EventTypeId { get; set; }
        public int EventReferenceId { get; set; }
        public string EventMessage { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? ProcessedFailedDate { get; set; }
        public int? InProcessBy { get; set; }
        public int? NumberOfRetry { get; set; }
    }
}
