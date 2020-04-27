using System.Collections.Generic;
using BrandUploadProcessor.Common.Models;
using OfficeOpenXml;

namespace BrandUploadProcessor.Service.ExcelParsing.Interfaces
{
    public interface IExcelRowParser
    {
        List<RowObject> Parse(ExcelWorksheet sheet, List<ColumnHeader> headers);
    }
}