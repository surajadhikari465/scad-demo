using System;

namespace MammothWebApi.DataAccess.Models
{
    public class StagingItemLocaleExtendedModel
    {
        public string Region { get; set; }
        public string ScanCode { get; set; }
        public int BusinessUnitId { get; set; }
        public int AttributeId { get; set; }
        public string AttributeValue { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid TransactionId { get; set; }
    }
}
