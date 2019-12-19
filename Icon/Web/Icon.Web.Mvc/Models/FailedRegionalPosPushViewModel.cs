using System;

namespace Icon.Web.Mvc.Models
{
    public class FailedRegionalPosPushViewModel
    {
        public string RegionAbbreviation { get; set; }
        public int Id { get; set; }
        public string ScanCode { get; set; }
        public int BusinessUnitId { get; set; }
        public string ChangeType { get; set; }
        public DateTime ProcessFailedDate { get; set; }
    }
}