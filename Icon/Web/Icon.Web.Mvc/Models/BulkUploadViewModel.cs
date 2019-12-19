using Icon.Web.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Icon.Web.Mvc.Models
{
    public class BulkUploadViewModel
    {
        [ExcelAttachment]
        [Display(Name = "File Path")]
        [Required(ErrorMessage = "Please Upload File")]
        public HttpPostedFileBase ExcelAttachment { get; set; }
        public string NewOrExistSetSelected { get; set; }
    }
}