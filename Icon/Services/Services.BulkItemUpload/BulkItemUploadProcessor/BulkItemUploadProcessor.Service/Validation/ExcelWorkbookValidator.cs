using BulkItemUploadProcessor.Common;
using BulkItemUploadProcessor.Common.Models;
using BulkItemUploadProcessor.Service.Validation.Validation.Interfaces;
using Icon.Common.DataAccess;
using Icon.Common.Models;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Linq;

namespace BulkItemUploadProcessor.Service.Validation
{
    public class ExcelWorkbookValidator : IExcelWorkbookValidator
    {
        private IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>> getAttributeModelQueryHandler;

        public ExcelWorkbookValidator(IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>> getAttributeModelQueryHandler)
        {
            this.getAttributeModelQueryHandler = getAttributeModelQueryHandler;
        }

        public ValidationResponse Validate(BulkItemUploadInformation bulkItemUploadInformation, ExcelPackage excelPackage)
        {
            ExcelWorksheet sheet = excelPackage.Workbook.Worksheets.FirstOrDefault(w => w.Name == Constants.ItemWorksheetName);
            if (sheet == null)
                return new ValidationResponse { IsValid = false, Error = $"Missing '{Constants.ItemWorksheetName}' worksheet." };

            if (sheet.Dimension == null || sheet.Cells.All(c => c.Value == null))
                return new ValidationResponse { IsValid = false, Error = $"'{Constants.ItemWorksheetName}' worksheet is empty." };

            if (sheet.Cells[2, sheet.Dimension.Start.Column, sheet.Dimension.End.Row, sheet.Dimension.End.Column].All(c => c.Value == null))
                return new ValidationResponse { IsValid = false, Error = $"'{Constants.ItemWorksheetName}' worksheet only contains header row." };

            var attributes = getAttributeModelQueryHandler.Search(new EmptyQueryParameters<IEnumerable<AttributeModel>>());

            var headerColumns = sheet.Cells[sheet.Dimension.Start.Row, sheet.Dimension.Start.Column, 1, sheet.Dimension.End.Column];
            var headers = headerColumns
                .Where(r => r.Value != null)
                .Select(r => r.Value.ToString())
                .ToHashSet();
            if (bulkItemUploadInformation.FileModeType == Enums.FileModeTypeEnum.CreateNew)
            {
                if (!headers.Contains(Constants.BarcodeTypeColumnHeader))
                    return new ValidationResponse { IsValid = false, Error = $"Missing '{Constants.BarcodeTypeColumnHeader}' column." };

                foreach (var attribute in attributes.Where(a => a.IsRequired && !a.IsReadOnly))
                {
                    if (!headers.Contains(attribute.DisplayName))
                        return new ValidationResponse { IsValid = false, Error = $"Missing '{attribute.DisplayName}' column." };
                }
            }
            else
            {
                if (!headers.Contains(Constants.ScanCodeColumnHeader))
                    return new ValidationResponse { IsValid = false, Error = $"Missing '{Constants.ScanCodeColumnHeader}' column." };
                if (!headers.Any(h => Constants.HierarchyColumnNames.Contains(h)) && !attributes.Where(a => !a.IsReadOnly).Any(a => headers.Contains(a.DisplayName)))
                    return new ValidationResponse { IsValid = false, Error = $"Missing additional columns. Must specify a valid hierarchy or non-readonly attribute to update." };
            }

            return new ValidationResponse { IsValid = true };
        }
    }
}
