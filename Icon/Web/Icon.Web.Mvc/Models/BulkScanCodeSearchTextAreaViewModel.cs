using Icon.Web.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Icon.Web.Mvc.Models
{
    public class BulkScanCodeSearchTextAreaViewModel
    {
        [ScanCodeTextArea]
        public string ScanCodes { get; set; }
    }
}