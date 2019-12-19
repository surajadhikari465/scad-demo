using System;

namespace Icon.Web.Tests.Integration.TestModels
{
    internal class TestMessageQueueItemModel
    {
        public int ItemId { get; set; }
        public DateTime InsertDateUtc { get; set; }
        public DateTime EsbReadyDateTimeUtc { get; set; }
    }
}
