using Icon.Framework;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Icon.Web.Mvc.Exporters
{
    public class NationalClassExporter
    {
        private Worksheet worksheet;

        public Dictionary<string, string>  ExportData { get; set; }
        public ExcelExportModel ExportModel { get; set; }

        public void Export()
        {
            worksheet = ExportModel.ExcelWorkbook.Worksheets.Add("National Classes");

            var headerCell = worksheet.GetCell("A1");
            headerCell.CellFormat.Fill = CellFill.CreateSolidFill(Color.LightGreen);
            headerCell.CellFormat.Font.ColorInfo = new WorkbookColorInfo(Color.Black);
            headerCell.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            headerCell.Value = "National Class Name";

            worksheet.Columns[0].Width = 9000;

            int i = 1;
            foreach (var hierarchyClass in ExportData)
            {
                worksheet.Rows[i].Cells[0].Value = hierarchyClass.Value;
                i++;
            }
        }
    }
}