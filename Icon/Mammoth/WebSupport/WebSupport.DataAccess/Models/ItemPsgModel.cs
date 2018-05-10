using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSupport.DataAccess.Models
{
    public class ItemPsgModel
    {
        public int BusinessUnitId { get; set; }
        public int ItemId { get; set; }
        public string ItemTypeCode { get; set; }
        public string MessageAction { get; set; }
        public string Region { get; set; }
        public string ScanCode { get; set; }
        public string StoreName { get; set; }
        public object SourceData { get; set; }
    }
}