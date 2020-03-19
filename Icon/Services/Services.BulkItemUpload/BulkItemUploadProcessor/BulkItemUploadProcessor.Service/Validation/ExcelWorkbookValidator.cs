using BulkItemUploadProcessor.Common;
using BulkItemUploadProcessor.Common.Models;
using BulkItemUploadProcessor.Service.Validation.Validation.Interfaces;
using OfficeOpenXml;
using System.Linq;

namespace BulkItemUploadProcessor.Service.Validation
{
    public class ExcelWorkbookValidator : IExcelWorkbookValidator
    {
        public ValidationResponse Validate(BulkItemUploadInformation bulkItemUploadInformation, ExcelPackage excelPackage)
        {
            ExcelWorksheet sheet = excelPackage.Workbook.Worksheets.FirstOrDefault(w => w.Name == Constants.ItemWorksheetName);
            if (sheet == null)
                return new ValidationResponse { IsValid = false, Error = $"Missing '{Constants.ItemWorksheetName}' worksheet." };

            if (sheet.Dimension == null || sheet.Cells.All(c => c.Value == null))
                return new ValidationResponse { IsValid = false, Error = $"'{Constants.ItemWorksheetName}' worksheet is empty." };

            if (sheet.Cells[2, sheet.Dimension.Start.Column, sheet.Dimension.End.Row, sheet.Dimension.End.Column].All(c => c.Value == null))
                return new ValidationResponse { IsValid = false, Error = $"'{Constants.ItemWorksheetName}' worksheet only contains header row." };

            return new ValidationResponse { IsValid = true };
        }
    }
}
