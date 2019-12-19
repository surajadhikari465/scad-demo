using Icon.Web.Attributes;

namespace Icon.Web.Mvc.Models
{
    public class BulkScanCodeSearchTextAreaViewModel
    {
        [ScanCodeTextArea]
        public string ScanCodes { get; set; }
    }
}