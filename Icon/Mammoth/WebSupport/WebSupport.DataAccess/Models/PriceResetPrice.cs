using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSupport.DataAccess.Models
{
    public class PriceResetPrice
    {
        public int ItemId { get; set; }
        public string ScanCode { get; set; }
        public Guid GpmId { get; set; }
        public string ItemTypeCode { get; set; }
        public string ItemTypeDesc { get; set; }
        public int BusinessUnitId { get; set; }
        public string StoreName { get; set; }
        public string PriceType { get; set; }
        public string PriceTypeAttribute { get; set; }
        public string SellableUom { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Price { get; set; }
        public byte Multiple { get; set; }
        public DateTime? NewTagExpiration { get; set; }
        public string SequenceId { get; set; }
        public string PatchFamilyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
