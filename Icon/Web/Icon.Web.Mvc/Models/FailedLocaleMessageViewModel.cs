using System;

namespace Icon.Web.Mvc.Models
{
    public class FailedLocaleMessageViewModel
    {
        public int Id { get; set; }
        public string TerritoryCode { get; set; }
        public string StoreAbbreviation { get; set; }
        public string LocaleName { get; set; }
        public DateTime? InsertDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
    }
}