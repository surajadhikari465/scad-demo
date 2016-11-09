using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using System.Collections.Generic;
using System.Drawing;

namespace Icon.Web.Mvc.Exporters
{
    public class PluCategoryExporter
    {
        private Worksheet pluWorksheet;

        public List<PluCategoryViewModel> ExportData { get; set; }
        public ExcelExportModel ExportModel { get; set; }

        public void Export()
        {
            pluWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add("PLU Categories");

            foreach (var cell in pluWorksheet.GetRegion("A1:C1"))
            {
                cell.CellFormat.Fill = CellFill.CreateSolidFill(Color.LightGreen);
                cell.CellFormat.Font.ColorInfo = new WorkbookColorInfo(Color.Black);
                cell.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            }

            pluWorksheet.Rows[0].Cells[0].Value = "PLU Category";
            pluWorksheet.Rows[0].Cells[1].Value = "Start";
            pluWorksheet.Rows[0].Cells[2].Value = "End";

            pluWorksheet.Columns[0].Width = 5000;
            pluWorksheet.Columns[0].CellFormat.Alignment = HorizontalCellAlignment.Left;
            pluWorksheet.Columns[1].Width = 5000;
            pluWorksheet.Columns[1].CellFormat.Alignment = HorizontalCellAlignment.Right;
            pluWorksheet.Columns[1].CellFormat.FormatString = "@";
            pluWorksheet.Columns[2].Width = 5000;
            pluWorksheet.Columns[2].CellFormat.Alignment = HorizontalCellAlignment.Right;
            pluWorksheet.Columns[2].CellFormat.FormatString = "@";

            int i = 1;
            foreach (PluCategoryViewModel pluCategory in ExportData)
            {
                pluWorksheet.Rows[i].Cells[0].Value = pluCategory.PluCategoryName;
                pluWorksheet.Rows[i].Cells[1].Value = pluCategory.BeginRange;
                pluWorksheet.Rows[i].Cells[2].Value = pluCategory.EndRange;
                i++;
            }
        }
    }
}