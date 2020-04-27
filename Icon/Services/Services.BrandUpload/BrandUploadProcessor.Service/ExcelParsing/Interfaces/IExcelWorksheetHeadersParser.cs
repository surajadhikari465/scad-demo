using System.Collections.Generic;
using BrandUploadProcessor.Common.Models;
using OfficeOpenXml;

namespace BrandUploadProcessor.Service.ExcelParsing.Interfaces
{
    public interface IExcelWorksheetHeadersParser
    {
        List<ColumnHeader> Parse(ExcelWorksheet sheet);
    }
}