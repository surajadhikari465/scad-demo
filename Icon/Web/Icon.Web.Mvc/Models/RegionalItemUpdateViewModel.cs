using System;

namespace Icon.Web.Mvc.Models
{
    public class FailedRegionalEventViewModel
    {
        public int Id { get; set; }
        public string ScanCode { get; set; }
        public string ChangeType { get; set; }
        public DateTime ProcessFailedDate { get; set; }
        public string RegionAbbreviation { get; set; }
    }
}