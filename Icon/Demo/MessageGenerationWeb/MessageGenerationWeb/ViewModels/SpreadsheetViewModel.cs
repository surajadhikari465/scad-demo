using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MessageGenerationWeb.ViewModels
{
    public class SpreadsheetViewModel
    {
        [DisplayName("File Path:")]
        public HttpPostedFileBase ExcelAttachment { get; set; }
    }
}