using System;

namespace MammothWebApi.DataAccess.Models
{
    public class CancelAllSalesModel
    {
        public string ScanCode { get; set; }
        public int BusinessUnitID { get; set; }
        public DateTime EndDate { get; set; }
    }
}