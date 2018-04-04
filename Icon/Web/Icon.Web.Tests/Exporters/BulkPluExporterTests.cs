using Icon.Web.DataAccess.Models;
using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Icon.Web.Tests.Unit.Exporters
{
    [TestClass] [Ignore]
    public class BulkPluExporterTests
    {
        private WorkbookFormat excelFormat;
        private ExcelExportModel exportModel;

        [TestInitialize]
        public void Initialize()
        {
            excelFormat = WorkbookFormat.Excel2007;
            exportModel = new ExcelExportModel(excelFormat);
        }

        [TestMethod]
        public void Export_PluList_ShouldConvertToSpreadsheetObject()
        {
            // Given.
            string pluDescription = "Test";
            string brand = "Test";
            string plu = "1234";

            List<BulkImportPluModel> pluList = new List<BulkImportPluModel>
            {
                new BulkImportPluModel
                {
                    BrandName = brand,
                    ProductDescription = pluDescription,
                    NationalPlu = plu,
                    flPLU = plu,
                    maPLU = plu,
                    mwPLU = plu,
                    naPLU = plu,
                    ncPLU = plu,
                    nePLU = plu,
                    pnPLU = plu,
                    rmPLU = plu,
                    soPLU = plu,
                    spPLU = plu,
                    swPLU = plu,
                    ukPLU = plu
                }           
            };

            var bulkPluExporter = new BulkPluExporter();
            bulkPluExporter.ExportData = pluList;
            bulkPluExporter.ExportModel = exportModel;

            // When.
            bulkPluExporter.Export();
            Worksheet pluWorksheet = exportModel.ExcelWorkbook.Worksheets[0];

            // Then.
            Assert.AreEqual(brand, pluWorksheet.Rows[1].Cells[0].Value);
            Assert.AreEqual(pluDescription, pluWorksheet.Rows[1].Cells[1].Value);
            Assert.AreEqual(plu, pluWorksheet.Rows[1].Cells[2].Value);
            Assert.AreEqual(plu, pluWorksheet.Rows[1].Cells[3].Value);
            Assert.AreEqual(plu, pluWorksheet.Rows[1].Cells[4].Value);
            Assert.AreEqual(plu, pluWorksheet.Rows[1].Cells[5].Value);
            Assert.AreEqual(plu, pluWorksheet.Rows[1].Cells[6].Value);
            Assert.AreEqual(plu, pluWorksheet.Rows[1].Cells[7].Value);
            Assert.AreEqual(plu, pluWorksheet.Rows[1].Cells[8].Value);
            Assert.AreEqual(plu, pluWorksheet.Rows[1].Cells[9].Value);
            Assert.AreEqual(plu, pluWorksheet.Rows[1].Cells[10].Value);
            Assert.AreEqual(plu, pluWorksheet.Rows[1].Cells[11].Value);
            Assert.AreEqual(plu, pluWorksheet.Rows[1].Cells[12].Value);
            Assert.AreEqual(plu, pluWorksheet.Rows[1].Cells[13].Value);
            Assert.AreEqual(plu, pluWorksheet.Rows[1].Cells[14].Value);
        }
    }
}
