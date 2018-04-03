using System;

namespace MammothWebApi.Service.Models
{
    public class CancelAllSalesServiceModel
    {
        public string Region { get; set; }
        public string ScanCode { get; set; }
        public int BusinessUnitId { get; set; }
        public DateTime EndDate { get; set; }
    }
}