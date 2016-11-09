using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageGenerationWeb.Models
{
    public class ItemPriceMessageModel
    {
        public int ItemId { get; set; }
        public string ItemTypeCode { get; set; }
        public string ItemTypeDesc { get; set; }
        public string LocaleName { get; set; }
        public string ScanCode { get; set; }
        public int ScanCodeId { get; set; }
        public int ScanCodeTypeId { get; set; }
        public string ScanCodeTypeDescription { get; set; }
    }
}