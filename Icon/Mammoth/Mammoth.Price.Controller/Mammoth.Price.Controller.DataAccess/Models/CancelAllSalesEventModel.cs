using System;

namespace Mammoth.Price.Controller.DataAccess.Models
{
    public class CancelAllSalesEventModel
    {
        public int QueueId { get; set; }
        public int EventTypeId { get; set; }
        public string Region { get; set; }
        public string ScanCode { get; set; }
        public int BusinessUnitId { get; set; }
        public DateTime EndDate { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }
        public string ErrorSource { get; set; }
    }
}