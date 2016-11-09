using Icon.Web.Attributes;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Icon.Web.Mvc.Models
{
    public class BulkPluMappingViewModel
    {
        [ExcelAttachment]
        [Display(Name = "File Path")]
        public HttpPostedFileBase ExcelAttachment { get; set; }

        public bool? ValidSpreadsheetType { get; set; }

        public List<BulkImportPluModel> ErrorItems { get; set; }

        public List<BulkImportPluRemapModel> RemapRows { get; set; }

        public int ValidItemsCount { get; set; }
    }
}
