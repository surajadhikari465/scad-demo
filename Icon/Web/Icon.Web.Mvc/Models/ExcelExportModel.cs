using Infragistics.Documents.Excel;

namespace Icon.Web.Mvc.Models
{
    public class ExcelExportModel
    {
        private Workbook excelWorkbook;

        public ExcelExportModel()
        {
            excelWorkbook = new Workbook();
        }

        public ExcelExportModel(WorkbookFormat exportFormat)
        {
            excelWorkbook = new Workbook(exportFormat);
        }

        public Workbook ExcelWorkbook
        {
            get
            {
                return excelWorkbook;
            }
        }
    }
}