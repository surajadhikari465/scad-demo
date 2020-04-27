using BrandUploadProcessor.Common.Models;
using OfficeOpenXml;

namespace BrandUploadProcessor.Service.Validation.Interfaces
{
    public interface IColumnHeadersValidator
    {
        ValidationResponse Validate(BrandUploadInformation brandUploadInformation, ExcelPackage excelPackage);
    }


    public interface DuplicateValuesValidator
    {

    }

}