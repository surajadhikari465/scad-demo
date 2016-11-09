using Icon.Web.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Icon.Web.Mvc.Models
{
    public class BulkImportViewModel<T>
    {
        [ExcelAttachment]
        [Display(Name = "File Path")]
        public HttpPostedFileBase ExcelAttachment { get; set; }

        public bool? ValidSpreadsheetType { get; set; }

        public List<T> ErrorItems { get; set; }

        public int ValidItemsCount { get; set; }
    }
}