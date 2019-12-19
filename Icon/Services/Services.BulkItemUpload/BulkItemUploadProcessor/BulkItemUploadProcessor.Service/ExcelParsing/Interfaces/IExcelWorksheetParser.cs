using OfficeOpenXml;

namespace BulkItemUploadProcessor.Service.ExcelParsing.Interfaces
{
    public interface IExcelWorksheetParser
    {
        ExcelPackage Parse(byte[] fileContent);
    }
}