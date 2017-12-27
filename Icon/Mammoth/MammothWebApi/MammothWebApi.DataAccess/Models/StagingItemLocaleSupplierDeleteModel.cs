using System;

namespace MammothWebApi.DataAccess.Models
{
    public class StagingItemLocaleSupplierDeleteModel
    {
        public string Region { get; set; }
        public int BusinessUnitID { get; set; }
        public string ScanCode { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid TransactionId { get; set; }
    }
}
