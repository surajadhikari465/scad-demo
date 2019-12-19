using System;

namespace Icon.Web.DataAccess.Models
{
    public class MessageQueueItemModel
    {
        public int MessageQueueItemId { get; set; }
        public DateTime EsbReadyDateTimeUtc { get; set; }
        public int ItemId { get; set; }
        public DateTime InsertDateUtc { get; set; }
    }
}
