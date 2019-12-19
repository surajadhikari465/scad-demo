using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;

namespace Icon.Web.Mvc.Exporters
{
    public class BarcodeTypeExporter
    {
        private Worksheet barcodeTypeWorksheet;
        public List<BarcodeTypeViewModel> ExportData { get; set; }

        public ExcelExportModel ExportModel { get; set; }

        public void Export() 
        {
            barcodeTypeWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add("Barcode Types");

            foreach (var cell in barcodeTypeWorksheet.GetRegion("A1:D1"))
            {
                cell.CellFormat.Fill = CellFill.CreateSolidFill(Color.LightGreen);
                cell.CellFormat.Font.ColorInfo = new WorkbookColorInfo(Color.Black);
                cell.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            }

            barcodeTypeWorksheet.Rows[0].Cells[0].Value = "Barcode Type";
            barcodeTypeWorksheet.Rows[0].Cells[1].Value = "Start";
            barcodeTypeWorksheet.Rows[0].Cells[2].Value = "End";
            barcodeTypeWorksheet.Rows[0].Cells[3].Value = "Scale PLU";

            barcodeTypeWorksheet.Columns[0].Width = 15000;
            barcodeTypeWorksheet.Columns[0].CellFormat.Alignment = HorizontalCellAlignment.Left;
            barcodeTypeWorksheet.Columns[1].Width = 5000;
            barcodeTypeWorksheet.Columns[1].CellFormat.Alignment = HorizontalCellAlignment.Right;
            barcodeTypeWorksheet.Columns[1].CellFormat.FormatString = "@";
            barcodeTypeWorksheet.Columns[2].Width = 5000;
            barcodeTypeWorksheet.Columns[2].CellFormat.Alignment = HorizontalCellAlignment.Right;
            barcodeTypeWorksheet.Columns[2].CellFormat.FormatString = "@";
            barcodeTypeWorksheet.Columns[3].Width = 3000;
            barcodeTypeWorksheet.Columns[3].CellFormat.Alignment = HorizontalCellAlignment.Right;

            int i = 1;
            foreach (BarcodeTypeViewModel barcodeType in ExportData)
            {
                barcodeTypeWorksheet.Rows[i].Cells[0].Value = barcodeType.BarcodeType;
                barcodeTypeWorksheet.Rows[i].Cells[1].Value = barcodeType.BeginRange;
                barcodeTypeWorksheet.Rows[i].Cells[2].Value = barcodeType.EndRange;
                barcodeTypeWorksheet.Rows[i].Cells[3].Value = barcodeType.ScalePlu ? "Yes" : "No";
                i++;
            }
        }
    }
}