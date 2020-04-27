using OfficeOpenXml;

namespace BrandUploadProcessor.Service.ExcelParsing.Interfaces
{
    public interface IExcelWorksheetParser
    {
        ExcelPackage Parse(byte[] fileContent);
    }
}