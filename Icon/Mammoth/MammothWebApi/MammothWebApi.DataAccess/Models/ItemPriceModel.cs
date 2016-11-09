using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.DataAccess.Models
{
    public class ItemPriceModel
    {
        public string Region { get; set; }
        public string ScanCode { get; set; }
        public int BusinessUnitId { get; set; }
        public int Multiple { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PriceType { get; set; }
        public string PriceUom { get; set; }
        public string CurrencyCode { get; set; }
    }
}
