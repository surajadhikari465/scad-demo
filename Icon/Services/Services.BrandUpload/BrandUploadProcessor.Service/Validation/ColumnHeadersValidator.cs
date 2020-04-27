using System.Collections.Generic;
using System.Linq;
using BrandUploadProcessor.Common;
using BrandUploadProcessor.Common.Models;
using BrandUploadProcessor.Service.Validation.Interfaces;
using Icon.Common.DataAccess;
using OfficeOpenXml;

namespace BrandUploadProcessor.Service.Validation
{
    public class ColumnHeadersValidator : IColumnHeadersValidator
    {
        private IQueryHandler<EmptyQueryParameters<IEnumerable<BrandAttributeModel>>, IEnumerable<BrandAttributeModel>> getBrandAttributeQueryHandler;

        public ColumnHeadersValidator(IQueryHandler<EmptyQueryParameters<IEnumerable<BrandAttributeModel>>, IEnumerable<BrandAttributeModel>> getBrandAttributeQueryHandler)
        {
            this.getBrandAttributeQueryHandler = getBrandAttributeQueryHandler;
        }

        public ValidationResponse Validate(BrandUploadInformation bulkItemUploadInformation, ExcelPackage excelPackage)
        {
            ExcelWorksheet sheet = excelPackage.Workbook.Worksheets.FirstOrDefault(w => w.Name == Constants.BrandWorksheetName);

            var attributes = getBrandAttributeQueryHandler.Search(new EmptyQueryParameters<IEnumerable<BrandAttributeModel>>());

            var headerColumns = sheet.Cells[sheet.Dimension.Start.Row, sheet.Dimension.Start.Column, 1, sheet.Dimension.End.Column];
            var headers = headerColumns
                .Where(r => r.Value != null)
                .Select(r => r.Value.ToString())
                .ToHashSet();

            foreach (var attribute in attributes.Where(a => a.IsRequired && !a.IsReadOnly))
            {
                if (!headers.Contains(attribute.TraitDesc))
                    return new ValidationResponse { IsValid = false, Error = $"Missing '{attribute.TraitDesc}' column." };
            }

            return new ValidationResponse { IsValid = true };
        }
    }
}