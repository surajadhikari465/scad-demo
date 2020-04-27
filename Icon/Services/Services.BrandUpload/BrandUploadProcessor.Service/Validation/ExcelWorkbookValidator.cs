using System.Linq;
using BrandUploadProcessor.Common;
using BrandUploadProcessor.Common.Models;
using BrandUploadProcessor.Service.Validation.Interfaces;
using OfficeOpenXml;

namespace BrandUploadProcessor.Service.Validation
{
    public class ExcelWorkbookValidator : IExcelWorkbookValidator
    {
        public ValidationResponse Validate(BrandUploadInformation brandUploadInformation, ExcelPackage excelPackage)
        {
            ExcelWorksheet sheet = excelPackage.Workbook.Worksheets.FirstOrDefault(w => w.Name == Constants.BrandWorksheetName);
            if (sheet == null)
                return new ValidationResponse { IsValid = false, Error = $"Missing '{Constants.BrandWorksheetName}' worksheet." };

            if (sheet.Dimension == null || sheet.Cells.All(c => c.Value == null))
                return new ValidationResponse { IsValid = false, Error = $"'{Constants.BrandWorksheetName}' worksheet is empty." };

            if (sheet.Cells[2, sheet.Dimension.Start.Column, sheet.Dimension.End.Row, sheet.Dimension.End.Column].All(c => c.Value == null))
                return new ValidationResponse { IsValid = false, Error = $"'{Constants.BrandWorksheetName}' worksheet only contains header row." };

            return new ValidationResponse { IsValid = true };
        }
    }
}
