using System.Collections.Generic;
using BulkItemUploadProcessor.Common.Models;
using OfficeOpenXml;

namespace BulkItemUploadProcessor.Service.ExcelParsing.Interfaces
{
    public interface IExcelWorksheetHeadersParser
    {
        List<ColumnHeader> Parse(ExcelWorksheet sheet);
    }
}