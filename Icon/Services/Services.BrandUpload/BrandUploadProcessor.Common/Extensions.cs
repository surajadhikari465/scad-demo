using System;
using System.Collections.Generic;
using OfficeOpenXml;

namespace BrandUploadProcessor.Common
{
    public static class Extensions
    {
        public static ExcelRange GetRowData(this ExcelWorksheet sheet, int rowId)
        {
            return sheet.Cells[$"{rowId}:{rowId}"];
        }

        public static string CellToString(this object cellValue)
        {
            return cellValue == null ? string.Empty : cellValue.ToString().Trim();
        }

        public static string ToFormattedDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ");
        }

        public  static string GetCellValue(int index, Dictionary<int, string> cells, bool allowRemoveValue, string removeValue)
        {
            if (!cells.ContainsKey(index)) return null;

            if (allowRemoveValue) return cells[index];
            
            return string.Equals(cells[index], removeValue, StringComparison.CurrentCultureIgnoreCase) ? null : cells[index];
            
            
        }
    }
}