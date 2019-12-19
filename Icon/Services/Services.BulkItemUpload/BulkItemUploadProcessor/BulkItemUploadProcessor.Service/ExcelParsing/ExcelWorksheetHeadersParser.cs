using BulkItemUploadProcessor.Common.Models;
using BulkItemUploadProcessor.Service.ExcelParsing.Interfaces;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Linq;

namespace BulkItemUploadProcessor.Service.ExcelParsing
{
    public class ExcelWorksheetHeadersParser : IExcelWorksheetHeadersParser
    {
        private const int FirstRowIndex = 1;

        /// <summary>
        /// Returns the headers of a worksheet's columns.
        /// </summary>
        /// <param name="sheet">The worksheet to parse the headers from.</param>
        /// <returns>Dictionary of column headers with the ColumnIndex as the key.</returns>
        public List<ColumnHeader> Parse(ExcelWorksheet sheet)
        {
            return sheet
                .Cells[sheet.Dimension.Start.Row,
                       sheet.Dimension.Start.Column,
                       FirstRowIndex,
                       sheet.Dimension.End.Column]
                .Select(c => new ColumnHeader { Address = c.Address, Name = c.Text, ColumnIndex = c.Start.Column })
                .Where(c => !string.IsNullOrWhiteSpace(c.Name.Trim()))
                .ToList();
        }
    }
}
