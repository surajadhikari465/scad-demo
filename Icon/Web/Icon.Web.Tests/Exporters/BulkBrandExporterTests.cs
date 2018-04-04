using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Infragistics.Documents.Excel;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Exporters;
using System.Collections.Generic;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.Tests.Unit.Exporters
{
    [TestClass] [Ignore]
    public class BulkBrandExporterTests
    {
        private WorkbookFormat excelFormat;
        private ExcelExportModel exportModel;
        private BulkBrandExporter exporter;

        [TestInitialize]
        public void Initialize()
        {
            excelFormat = WorkbookFormat.Excel2007;
            exportModel = new ExcelExportModel(excelFormat);
        }

        [TestMethod]
        public void BrandExport_BrandsExist_ShouldExportBrands()
        {
            // Given
            var testBrand1 = new BulkImportBrandModel { BrandId = "1", BrandName = "Test Brand Export1", BrandAbbreviation = "BEVM1" };
            var testBrand2 = new BulkImportBrandModel { BrandId = "2", BrandName = "Test Brand Export2", BrandAbbreviation = "BEVM2" };
            var brandExportData = new List<BulkImportBrandModel>();
            brandExportData.Add(testBrand1);
            brandExportData.Add(testBrand2);

            this.exporter = new BulkBrandExporter();
            this.exporter.ExportModel = exportModel;
            this.exporter.ExportData = brandExportData;
            
            // When
            this.exporter.Export();

            // Then
            string formatString = "{0}|{1}";
            Worksheet worksheet = exportModel.ExcelWorkbook.Worksheets[0];
            Assert.AreEqual(String.Format(formatString, testBrand1.BrandName, testBrand1.BrandId), worksheet.Rows[1].Cells[0].Value, "The Brand on the export of first row did not match expected value");
            Assert.AreEqual(testBrand1.BrandAbbreviation, worksheet.Rows[1].Cells[1].Value, "The BrandAbbreviation on the export of first row did not match expected value");
            Assert.AreEqual(String.Format(formatString, testBrand2.BrandName, testBrand2.BrandId), worksheet.Rows[2].Cells[0].Value, "The Brand on the export of second row did not match expected value");
            Assert.AreEqual(testBrand2.BrandAbbreviation, worksheet.Rows[2].Cells[1].Value, "The BrandAbbreviation on the export of second row did not match expected value");
        }

        [TestMethod]
        public void BrandExport_BrandsHaveErrors_ShouldHaveErrorColumnInSpreadsheet()
        {
            // Given
            var testBrand1 = new BulkImportBrandModel { BrandId = "1", BrandName = "Test Brand Export1", BrandAbbreviation = "BEVM1", Error = "Test Error" };
            var testBrand2 = new BulkImportBrandModel { BrandId = "2", BrandName = "Test Brand Export2", BrandAbbreviation = "BEVM2", Error = "Test Error" };
            var brandExportData = new List<BulkImportBrandModel>();
            brandExportData.Add(testBrand1);
            brandExportData.Add(testBrand2);

            this.exporter = new BulkBrandExporter();
            this.exporter.ExportModel = exportModel;
            this.exporter.ExportData = brandExportData;

            // When
            this.exporter.Export();

            // Then
            string formatString = "{0}|{1}";
            Worksheet worksheet = exportModel.ExcelWorkbook.Worksheets[0];
            Assert.AreEqual(String.Format(formatString, testBrand1.BrandName, testBrand1.BrandId), worksheet.Rows[1].Cells[0].Value, "The Brand on the export of first row did not match expected value");
            Assert.AreEqual(testBrand1.BrandAbbreviation, worksheet.Rows[1].Cells[1].Value, "The BrandAbbreviation on the export of first row did not match expected value");
            Assert.AreEqual(testBrand1.Error, worksheet.Rows[1].Cells[2].Value, "The BrandAbbreviation on the export of first row did not match expected value");

            Assert.AreEqual(String.Format(formatString, testBrand2.BrandName, testBrand2.BrandId), worksheet.Rows[2].Cells[0].Value, "The Brand on the export of second row did not match expected value");
            Assert.AreEqual(testBrand2.BrandAbbreviation, worksheet.Rows[2].Cells[1].Value, "The BrandAbbreviation on the export of second row did not match expected value");
            Assert.AreEqual(testBrand2.Error, worksheet.Rows[2].Cells[2].Value, "The BrandAbbreviation on the export of second row did not match expected value");
        }
    }
}
