using Infragistics.Documents.Excel;

namespace Icon.Web.Mvc.Excel.WorksheetBuilders
{
    public interface IWorksheetBuilder
    {
        void AppendWorksheet(Workbook workbook);
    }
}
