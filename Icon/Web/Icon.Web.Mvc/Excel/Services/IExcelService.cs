using System.Collections.Generic;
using Icon.Web.Mvc.Excel.Models;
using Infragistics.Documents.Excel;

namespace Icon.Web.Mvc.Excel.Services
{
    public interface IExcelService<T> where T : ExcelModel, new()
    {
        IReadOnlyCollection<string> Columns { get; }

        ImportResponse<T> Import(Workbook book);
        ExportResponse Export(ExportRequest<T> request);
    }
}