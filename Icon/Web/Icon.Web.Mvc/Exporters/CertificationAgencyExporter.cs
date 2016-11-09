using Icon.Web.DataAccess.Models;
using Icon.Web.Mvc.Excel;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Icon.Web.Mvc.Exporters
{
    public class CertificationAgencyExporter
    {
        public ExcelExportModel ExportModel { get; set; }
        public List<BulkImportCertificationAgencyModel> ExportData { get; set; }

        public CertificationAgencyExporter()
        {
            ExportData = new List<BulkImportCertificationAgencyModel>();
        }

        public void Export()
        {
            var worksheet = ExportModel.ExcelWorkbook.Worksheets.Add("Certification Agencies");

            var displayErrorColumn = ExportData.Any(ca => !String.IsNullOrWhiteSpace(ca.Error));
            SetUpHeaderRow(worksheet, displayErrorColumn);

            int i = 1;
            foreach (var item in ExportData)
            {
                worksheet.Rows[i].Cells[0].Value = item.CertificationAgencyNameAndId;
                worksheet.Rows[i].Cells[1].Value = item.GlutenFree.ToSpreadsheetBoolean();
                worksheet.Rows[i].Cells[2].Value = item.Kosher.ToSpreadsheetBoolean();
                worksheet.Rows[i].Cells[3].Value = item.NonGmo.ToSpreadsheetBoolean();
                worksheet.Rows[i].Cells[4].Value = item.Organic.ToSpreadsheetBoolean();
                worksheet.Rows[i].Cells[5].Value = item.DefaultOrganic.ToSpreadsheetBoolean();
                worksheet.Rows[i].Cells[6].Value = item.Vegan.ToSpreadsheetBoolean();
                if(displayErrorColumn)
                {
                    worksheet.Rows[i].Cells[7].Value = item.Error;
                }

                i++;
            }
        }

        private void SetUpHeaderRow(Worksheet worksheet, bool displayErrorColumn)
        {
            SetUpColumn(worksheet, 0, "Agency", 15000);
            SetUpColumn(worksheet, 1, "Gluten Free");
            SetUpColumn(worksheet, 2, "Kosher");
            SetUpColumn(worksheet, 3, "Non-GMO");
            SetUpColumn(worksheet, 4, "Organic");
            SetUpColumn(worksheet, 5, "Default Organic Agency");
            SetUpColumn(worksheet, 6, "Vegan");
            if(displayErrorColumn)
            {
                SetUpColumn(worksheet, 7, "Error", 15000);
            }
        }

        private void SetUpColumn(Worksheet worksheet, int columnIndex, string headerTitle, int columnWidth = 4000)
        {
            worksheet.Columns[columnIndex].Width = columnWidth;
            worksheet.Columns[columnIndex].CellFormat.Alignment = HorizontalCellAlignment.Left;

            var headerCell = worksheet.Rows[0].Cells[columnIndex];

            headerCell.Value = headerTitle;
            headerCell.CellFormat.Fill = CellFill.CreateSolidFill(Color.LightGreen);
            headerCell.CellFormat.Font.ColorInfo = new WorkbookColorInfo(Color.Black);
            headerCell.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        }
    }
}