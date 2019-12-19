using BulkItemUploadProcessor.Common.Models;
using OfficeOpenXml;

namespace BulkItemUploadProcessor.Service.Validation.Validation.Interfaces
{
    public interface IExcelWorkbookValidator
    {
        ValidationResponse Validate(BulkItemUploadInformation bulkItemUploadInformation, ExcelPackage excelPackage);
    }
}