using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageGenerationWeb.Models
{
    public class ItemPriceDeleteModel
    {
        public string ScanCode { get; set; }
        public string BusinessUnit { get; set; }
        public decimal Price { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Uom { get; set; }
    }
}