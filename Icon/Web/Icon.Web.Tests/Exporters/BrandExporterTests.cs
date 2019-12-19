using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Icon.Web.Tests.Unit.Exporters
{
    [TestClass] [Ignore]
    public class BrandExporterTests
    {
        private WorkbookFormat excelFormat;
        private ExcelExportModel exportModel;
        private BrandExporter exporter;

        [TestInitialize]
        public void Initialize()
        {
            excelFormat = WorkbookFormat.Excel2007;
            exportModel = new ExcelExportModel(excelFormat);
        }

        [TestMethod]
        public void BrandExport_Brand()
        {
            // Given
            var testBrand1 = new BrandExportViewModel { BrandId = "1", BrandName = "Test Brand Export1", BrandAbbreviation = "BEVM1", Designation = "TestDesignation1", ParentCompany = "TestParentCompany1", ZipCode = "TestZipCode1", Locality = "TestLocality1" };
            var testBrand2 = new BrandExportViewModel { BrandId = "2", BrandName = "Test Brand Export2", BrandAbbreviation = "BEVM2", Designation = "TestDesignation2", ParentCompany = "TestParentCompany2", ZipCode = "TestZipCode2", Locality = "TestLocality2" };
            var brandExportData = new List<BrandExportViewModel>();

            brandExportData.Add(testBrand1);
            brandExportData.Add(testBrand2);

            this.exporter = new BrandExporter();
            this.exporter.ExportModel = exportModel;
            this.exporter.ExportData = brandExportData;

            // When
            this.exporter.Export();

            // Then
            string formatString = "{0}|{1}";
            Worksheet worksheet = exportModel.ExcelWorkbook.Worksheets[0];
            Assert.AreEqual(String.Format(formatString, testBrand1.BrandName, testBrand1.BrandId), worksheet.Rows[1].Cells[0].Value, "The BrandName on the export of first row did not match expected value");
            Assert.AreEqual(testBrand1.BrandAbbreviation, worksheet.Rows[1].Cells[1].Value, "The BrandAbbreviation on the export of first row did not match expected value");
            Assert.AreEqual(testBrand1.Designation, worksheet.Rows[1].Cells[2].Value, "The Designation on the export of first row did not match expected value");
            Assert.AreEqual(testBrand1.ParentCompany, worksheet.Rows[1].Cells[3].Value, "The ParentCompany on the export of first row did not match expected value");
            Assert.AreEqual(testBrand1.ZipCode, worksheet.Rows[1].Cells[4].Value, "The ZipCode on the export of first row did not match expected value");
            Assert.AreEqual(testBrand1.Locality, worksheet.Rows[1].Cells[5].Value, "The Locality on the export of first row did not match expected value");
            Assert.AreEqual(String.Format(formatString, testBrand2.BrandName, testBrand2.BrandId), worksheet.Rows[2].Cells[0].Value, "The BrandName on the export of second row did not match expected value");
            Assert.AreEqual(testBrand2.BrandAbbreviation, worksheet.Rows[2].Cells[1].Value, "The BrandAbbreviation on the export of second row did not match expected value");
            Assert.AreEqual(testBrand2.Designation, worksheet.Rows[2].Cells[2].Value, "The Designation on the export of first row did not match expected value");
            Assert.AreEqual(testBrand2.ParentCompany, worksheet.Rows[2].Cells[3].Value, "The ParentCompany on the export of first row did not match expected value");
            Assert.AreEqual(testBrand2.ZipCode, worksheet.Rows[2].Cells[4].Value, "The ZipCode on the export of first row did not match expected value");
            Assert.AreEqual(testBrand2.Locality, worksheet.Rows[2].Cells[5].Value, "The Locality on the export of first row did not match expected value");
        }
    }
}