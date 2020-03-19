using BulkItemUploadProcessor.Common.Models;
using OfficeOpenXml;

namespace BulkItemUploadProcessor.Service.Validation.Validation.Interfaces
{
    public interface IColumnHeadersValidator
    {
        ValidationResponse Validate(BulkItemUploadInformation bulkItemUploadInformation, ExcelPackage excelPackage);
    }
}