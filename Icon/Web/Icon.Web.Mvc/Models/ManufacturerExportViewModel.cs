using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.Models
{
    public class ManufacturerExportViewModel
    {
        public string ManufacturerId { get; set; }
        public string ManufacturerName { get; set; }
        public string ZipCode { get; set; }
        public string ArCustomerId { get; set; }
    }
}