using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using System.Collections.Generic;
using System.Drawing;

namespace Icon.Web.Mvc.Exporters
{
    public class HierarchyClassExporter
    {
        private Worksheet worksheet;
        public List<HierarchyClassExportViewModel> ExportData { get; set; }
        public ExcelExportModel ExportModel { get; set; }

        public void Export()
        {
            worksheet = ExportModel.ExcelWorkbook.Worksheets.Add("Hierarchy Classes");

            var headerCell = worksheet.GetCell("A1");
            headerCell.CellFormat.Fill = CellFill.CreateSolidFill(Color.LightGreen);
            headerCell.CellFormat.Font.ColorInfo = new WorkbookColorInfo(Color.Black);
            headerCell.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            headerCell.Value = "Hierarchy Class Name";

            worksheet.Columns[0].Width = 9000;

            int i = 1;
            foreach (var hierarchyClass in ExportData)
            {
                worksheet.Rows[i].Cells[0].Value = hierarchyClass.HierarchyClassName;
                i++;
            }
        }
    }
}
