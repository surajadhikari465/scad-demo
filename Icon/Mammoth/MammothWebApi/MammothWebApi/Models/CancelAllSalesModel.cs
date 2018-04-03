using System;

namespace MammothWebApi.Models
{
    public class CancelAllSalesModel
    {
        public string Region { get; set; }
        public string ScanCode { get; set; }
        public int BusinessUnitId { get; set; }
        public DateTime EndDate { get; set; }
    }
}