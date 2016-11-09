using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.Models
{
    public class FailedProductMessageViewModel
    {
        public int Id { get; set; }
        public string ScanCode { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
    }
}