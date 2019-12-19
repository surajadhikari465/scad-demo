using System.Collections.Generic;
using BulkItemUploadProcessor.Common;
using BulkItemUploadProcessor.Common.Models;
using OfficeOpenXml;

namespace BulkItemUploadProcessor.Service.ExcelParsing.Interfaces
{
    public interface IExcelRowParser
    {
        List<RowObject> Parse(ExcelWorksheet sheet, List<ColumnHeader> headers);
    }
}