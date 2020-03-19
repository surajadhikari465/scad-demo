using BulkItemUploadProcessor.Common;
using BulkItemUploadProcessor.Common.Models;
using BulkItemUploadProcessor.Service.Validation.Validation.Interfaces;
using Icon.Common.DataAccess;
using Icon.Common.Models;
using Icon.Framework;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Linq;

namespace BulkItemUploadProcessor.Service.Validation
{
    public class ColumnHeadersValidator : IColumnHeadersValidator
    {
        private IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>> getAttributeModelQueryHandler;

        public ColumnHeadersValidator(IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>> getAttributeModelQueryHandler)
        {
            this.getAttributeModelQueryHandler = getAttributeModelQueryHandler;
        }

        public ValidationResponse Validate(BulkItemUploadInformation bulkItemUploadInformation, ExcelPackage excelPackage)
        {
            ExcelWorksheet sheet = excelPackage.Workbook.Worksheets.FirstOrDefault(w => w.Name == Constants.ItemWorksheetName);

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

                if (!headers.Contains(HierarchyNames.Merchandise))
                    return new ValidationResponse { IsValid = false, Error = $"Missing '{HierarchyNames.Merchandise}' column." };

                if (!headers.Contains(HierarchyNames.Brands))
                    return new ValidationResponse { IsValid = false, Error = $"Missing '{HierarchyNames.Brands}' column." };

                if (!headers.Contains(HierarchyNames.Tax))
                    return new ValidationResponse { IsValid = false, Error = $"Missing '{HierarchyNames.Tax}' column." };

                if (!headers.Contains(HierarchyNames.National))
                    return new ValidationResponse { IsValid = false, Error = $"Missing '{HierarchyNames.National}' column." };

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
