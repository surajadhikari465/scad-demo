using Icon.Web.Attributes;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Icon.Web.Mvc.Models
{
    public class MerchTaxMappingIndexViewModel
    {
        [ExcelAttachment]
        [Display(Name = "File Path")]
        public HttpPostedFileBase ExcelAttachment { get; set; }

        public bool? ValidSpreadsheetType { get; set; }

        public List<BulkImportItemModel> ErrorItems { get; set; }

        public int ValidItemsCount { get; set; }

        public MerchTaxMappingGridViewModel GridViewModel { get; set; }
    }
}
