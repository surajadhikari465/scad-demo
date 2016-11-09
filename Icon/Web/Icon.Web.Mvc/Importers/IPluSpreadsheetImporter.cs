using Icon.Web.Common.Validators;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Models;
using Infragistics.Documents.Excel;
using System.Collections.Generic;

namespace Icon.Web.Mvc.Importers
{
    public interface IPluSpreadsheetImporter
    {
        Workbook Workbook { get; set; }
        List<BulkImportPluModel> ErrorRows { get; set; }
        List<BulkImportPluModel> ParsedRows { get; set; }
        List<BulkImportPluModel> ValidRows { get; set; }
        List<BulkImportPluRemapModel> Remappings { get; set; }
        IObjectValidator<string> Validator { get; set; }
        void ConvertSpreadsheetToModel();
        void ConvertRemappingsToModel();
        bool ValidSpreadsheetType();
        void ValidateSpreadsheetData();
    }
}
