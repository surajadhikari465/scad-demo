using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.Models
{
    public class FailedGlobalPosPushViewModel
    {
        public string RegionAbbreviation { get; set; }
        public int Id { get; set; }
        public string ScanCode { get; set; }
        public int BusinessUnitId { get; set; }
        public string ChangeType { get; set; }
        public DateTime? EsbProcessFailedDate { get; set; }
        public DateTime? UdmProcessFailedDate { get; set; }
    }
}