using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.DataAccess.Models
{
    public class StagingPriceModel
    {
        public string Region { get; set; }
        public string ScanCode { get; set; }
        public int BusinessUnitId { get; set; }
        public int Multiple { get; set; }
        public decimal Price { get; set; }
        public string PriceType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PriceUom { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid TransactionId { get; set; }
    }
}
