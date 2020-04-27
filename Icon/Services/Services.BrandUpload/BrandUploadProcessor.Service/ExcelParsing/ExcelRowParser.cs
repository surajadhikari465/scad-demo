using System.Collections.Generic;
using System.Linq;
using BrandUploadProcessor.Common;
using BrandUploadProcessor.Common.Models;
using BrandUploadProcessor.Service.ExcelParsing.Interfaces;
using OfficeOpenXml;

namespace BrandUploadProcessor.Service.ExcelParsing
{
    public class ExcelRowParser : IExcelRowParser
    {
        public List<RowObject> Parse(ExcelWorksheet sheet, List<ColumnHeader> headers)
        {
            var start = sheet.Dimension.Start;
            var end = sheet.Dimension.End;
            var rows = new List<RowObject>();

            // each row
            for (var rowId = start.Row + 1; rowId <= end.Row; rowId++)
            {
                var excelRowData = sheet.GetRowData(rowId);
                if (excelRowData.All(c => string.IsNullOrWhiteSpace(c.Value.CellToString())))
                    continue;
                var rowObject = new RowObject { Row = rowId };

                var cellData = from r in excelRowData
                    join h in headers on r.Start.Column equals h.ColumnIndex
                    select new ParsedCell { Column = h, Address = r.Address, CellValue = r.Value.CellToString() };

                rowObject.Cells = cellData.ToList();

                rows.Add(rowObject);
            }
            return rows;
        }
    }
}