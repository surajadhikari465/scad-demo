using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.DataAccess.Models
{
    public class StagingItemLocaleModel
    {
        public string Region { get; set; }
        public int BusinessUnitID { get; set; }
        public string ScanCode { get; set; }
        public bool Discount_Case { get; set; }
        public bool Discount_TM { get; set; }
        public int? Restriction_Age { get; set; }
        public bool Restriction_Hours { get; set; }
        public bool Authorized { get; set; }
        public bool Discontinued { get; set; }
        public string LabelTypeDesc { get; set; }
        public bool LocalItem { get; set; }
        public string Product_Code { get; set; }
        public string RetailUnit { get; set; }
        public string Sign_Desc { get; set; }
        public string Locality { get; set; }
        public string Sign_RomanceText_Long { get; set; }
        public string Sign_RomanceText_Short { get; set; }
        public decimal Msrp { get; set; }
        public bool OrderedByInfor { get; set; }
        public decimal? AltRetailSize { get; set; }
        public string AltRetailUOM { get; set; }
        public string DefaultScanCode { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid TransactionId { get; set; }
    }
}
