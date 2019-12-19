using BulkItemUploadProcessor.Common;
using FluentValidation;
using OfficeOpenXml;
using System.Linq;

namespace BulkItemUploadProcessor.Service.Validation
{
    public class ExcelPackageValidator : AbstractValidator<ExcelPackage>
    {
        public ExcelPackageValidator()
        {
            RuleFor(x => x.Workbook.Worksheets.FirstOrDefault(w => w.Name == Constants.ItemWorksheetName))
                .NotNull()
                .WithMessage($"Missing '{Constants.ItemWorksheetName}' worksheet.");
        }
    }
}
