using Infragistics.Documents.Excel;
using System;

namespace Icon.Web.Mvc.Exporters
{
    public class SpreadsheetColumn<T>
    {
        public int Index { get; set; }
        public string HeaderTitle { get; set; }
        public CellFill HeaderBackground { get; set; }
        public WorkbookColorInfo HeaderForeground { get; set; }
        public ExcelDefaultableBoolean IsHeaderFontBold { get; set; }
        public int Width { get; set; }
        public HorizontalCellAlignment Alignment { get; set; }
        public Action<WorksheetRow, T> SetValue { get; set; }
        public bool IsWrapText { get; set; }
    }
}