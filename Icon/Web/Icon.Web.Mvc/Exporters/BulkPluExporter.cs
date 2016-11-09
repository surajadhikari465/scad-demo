using Icon.Web.DataAccess.Models;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using System.Collections.Generic;
using System.Drawing;

namespace Icon.Web.Mvc.Exporters
{
    public class BulkPluExporter
    {
        public List<BulkImportPluModel> ExportData { get; set; }
        public ExcelExportModel ExportModel { get; set; }

        private Worksheet pluWorksheet;

        public void Export()
        {
            pluWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add("PLU Mappings");

            foreach (var cell in pluWorksheet.GetRegion("A1:O1"))
            {
                cell.CellFormat.Fill = CellFill.CreateSolidFill(Color.LightGreen);
                cell.CellFormat.Font.ColorInfo = new WorkbookColorInfo(Color.Black);
                cell.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            }

            pluWorksheet.Rows[0].Cells[0].Value = "Brand";
            pluWorksheet.Rows[0].Cells[1].Value = "Product Description";
            pluWorksheet.Rows[0].Cells[2].Value = "National PLU";
            pluWorksheet.Rows[0].Cells[3].Value = "FL PLU";
            pluWorksheet.Rows[0].Cells[4].Value = "MA PLU";
            pluWorksheet.Rows[0].Cells[5].Value = "MW PLU";
            pluWorksheet.Rows[0].Cells[6].Value = "NA PLU";
            pluWorksheet.Rows[0].Cells[7].Value = "NC PLU";
            pluWorksheet.Rows[0].Cells[8].Value = "NE PLU";
            pluWorksheet.Rows[0].Cells[9].Value = "PN PLU";
            pluWorksheet.Rows[0].Cells[10].Value = "RM PLU";
            pluWorksheet.Rows[0].Cells[11].Value = "SO PLU";
            pluWorksheet.Rows[0].Cells[12].Value = "SP PLU";
            pluWorksheet.Rows[0].Cells[13].Value = "SW PLU";
            pluWorksheet.Rows[0].Cells[14].Value = "UK PLU";

            pluWorksheet.Columns[0].Width = 5000;
            pluWorksheet.Columns[0].CellFormat.Alignment = HorizontalCellAlignment.Left;
            pluWorksheet.Columns[1].Width = 9000;
            pluWorksheet.Columns[1].CellFormat.Alignment = HorizontalCellAlignment.Left;
            pluWorksheet.Columns[2].Width = 3500;
            pluWorksheet.Columns[2].CellFormat.Alignment = HorizontalCellAlignment.Left;
            pluWorksheet.Columns[3].Width = 3500;
            pluWorksheet.Columns[3].CellFormat.Alignment = HorizontalCellAlignment.Left;
            pluWorksheet.Columns[4].Width = 3500;
            pluWorksheet.Columns[4].CellFormat.Alignment = HorizontalCellAlignment.Left;
            pluWorksheet.Columns[5].Width = 3500;
            pluWorksheet.Columns[5].CellFormat.Alignment = HorizontalCellAlignment.Left;
            pluWorksheet.Columns[6].Width = 3500;
            pluWorksheet.Columns[6].CellFormat.Alignment = HorizontalCellAlignment.Left;
            pluWorksheet.Columns[7].Width = 3500;
            pluWorksheet.Columns[7].CellFormat.Alignment = HorizontalCellAlignment.Left;
            pluWorksheet.Columns[8].Width = 3500;
            pluWorksheet.Columns[8].CellFormat.Alignment = HorizontalCellAlignment.Left;
            pluWorksheet.Columns[9].Width = 3500;
            pluWorksheet.Columns[9].CellFormat.Alignment = HorizontalCellAlignment.Left;
            pluWorksheet.Columns[10].Width = 3500;
            pluWorksheet.Columns[10].CellFormat.Alignment = HorizontalCellAlignment.Left;
            pluWorksheet.Columns[11].Width = 3500;
            pluWorksheet.Columns[11].CellFormat.Alignment = HorizontalCellAlignment.Left;
            pluWorksheet.Columns[12].Width = 3500;
            pluWorksheet.Columns[12].CellFormat.Alignment = HorizontalCellAlignment.Left;
            pluWorksheet.Columns[13].Width = 3500;
            pluWorksheet.Columns[13].CellFormat.Alignment = HorizontalCellAlignment.Left;
            pluWorksheet.Columns[14].Width = 3500;
            pluWorksheet.Columns[14].CellFormat.Alignment = HorizontalCellAlignment.Left;

            int i = 1;
            foreach (BulkImportPluModel plu in ExportData)
            {
                pluWorksheet.Rows[i].Cells[0].Value = plu.BrandName;
                pluWorksheet.Rows[i].Cells[1].Value = plu.ProductDescription;
                pluWorksheet.Rows[i].Cells[2].Value = plu.NationalPlu;
                pluWorksheet.Rows[i].Cells[3].Value = plu.flPLU;
                pluWorksheet.Rows[i].Cells[4].Value = plu.maPLU;
                pluWorksheet.Rows[i].Cells[5].Value = plu.mwPLU;
                pluWorksheet.Rows[i].Cells[6].Value = plu.naPLU;
                pluWorksheet.Rows[i].Cells[7].Value = plu.ncPLU;
                pluWorksheet.Rows[i].Cells[8].Value = plu.nePLU;
                pluWorksheet.Rows[i].Cells[9].Value = plu.pnPLU;
                pluWorksheet.Rows[i].Cells[10].Value = plu.rmPLU;
                pluWorksheet.Rows[i].Cells[11].Value = plu.soPLU;
                pluWorksheet.Rows[i].Cells[12].Value = plu.spPLU;
                pluWorksheet.Rows[i].Cells[13].Value = plu.swPLU;
                pluWorksheet.Rows[i].Cells[14].Value = plu.ukPLU;
                i++;
            }
        }
    }
}