using Icon.Web.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Icon.Web.Mvc.Models
{
    public class BulkScanCodeSearchFileUploadViewModel
    {
        [TextFileAttachment]
        [Display(Name = "File Path")]
        public HttpPostedFileBase TextFileAttachment { get; set; }
    }
}