using BrandUploadProcessor.Common.Models;
using OfficeOpenXml;

namespace BrandUploadProcessor.Service.Validation.Interfaces
{
    public interface IExcelWorkbookValidator
    {
        ValidationResponse Validate(BrandUploadInformation brandUploadInformation, ExcelPackage excelPackage);
    }
}