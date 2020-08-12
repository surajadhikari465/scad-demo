using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Tests.Unit.Exporters
{
    [TestClass]
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
            Assert.IsTrue(worksheet.Rows[1].Cells.Any(c => ((string)c.Value) == testBrand1.BrandId));
            Assert.IsTrue(worksheet.Rows[1].Cells.Any(c => ((string)c.Value) == testBrand1.BrandAbbreviation));
            Assert.IsTrue(worksheet.Rows[1].Cells.Any(c => ((string)c.Value) == testBrand1.Designation));
            Assert.IsTrue(worksheet.Rows[1].Cells.Any(c => ((string)c.Value) == testBrand1.ParentCompany));
            Assert.IsTrue(worksheet.Rows[1].Cells.Any(c => ((string)c.Value) == testBrand1.ZipCode));
            Assert.IsTrue(worksheet.Rows[1].Cells.Any(c => ((string)c.Value) == testBrand1.Locality));
            Assert.IsTrue(worksheet.Rows[2].Cells.Any(c => ((string)c.Value) == testBrand2.BrandId));
            Assert.IsTrue(worksheet.Rows[2].Cells.Any(c => ((string)c.Value) == testBrand2.BrandAbbreviation));
            Assert.IsTrue(worksheet.Rows[2].Cells.Any(c => ((string)c.Value) == testBrand2.Designation));
            Assert.IsTrue(worksheet.Rows[2].Cells.Any(c => ((string)c.Value) == testBrand2.ParentCompany));
            Assert.IsTrue(worksheet.Rows[2].Cells.Any(c => ((string)c.Value) == testBrand2.ZipCode));
            Assert.IsTrue(worksheet.Rows[2].Cells.Any(c => ((string)c.Value) == testBrand2.Locality));
        }
    }
}