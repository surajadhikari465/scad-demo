using Infragistics.Documents.Excel;
using System.Collections.Generic;
using Icon.Common.Validators;

namespace Icon.Web.Mvc.Importers
{
    public interface ISpreadsheetImporter<T>
    {
        Workbook Workbook { get; set; }
        List<T> ErrorRows { get; set; }
        List<T> ParsedRows { get; set; }
        List<T> ValidRows { get; set; }
        IObjectValidator<string> Validator { get; set; }
        void ConvertSpreadsheetToModel();
        bool IsValidSpreadsheetType();
        void ValidateSpreadsheetData();
    }
}
