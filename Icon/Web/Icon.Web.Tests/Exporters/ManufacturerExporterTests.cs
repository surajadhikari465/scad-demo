using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Icon.Web.Tests.Unit.Exporters
{
    [TestClass]
    public class ManufacturerExporterTests
    {
        private WorkbookFormat excelFormat;
        private ExcelExportModel exportModel;
        private ManufacturerExporter exporter;

        [TestInitialize]
        public void Initialize()
        {
            excelFormat = WorkbookFormat.Excel2007;
            exportModel = new ExcelExportModel(excelFormat);
        }

        [TestMethod]
        public void ManufacturerExport_Brand()
        {
            // Given
            var testManufacturer1 = new ManufacturerExportViewModel { ManufacturerId = "1", ManufacturerName = "Test Manufacturer Export1", ZipCode = "78704", ArCustomerId = "1234" };
            var testManufacturer2 = new ManufacturerExportViewModel { ManufacturerId = "2", ManufacturerName = "Test Manufacturer Export2", ZipCode = "78749", ArCustomerId = "4321" };
            var manufacturerExportData = new List<ManufacturerExportViewModel>();

            manufacturerExportData.Add(testManufacturer1);
            manufacturerExportData.Add(testManufacturer2);

            this.exporter = new ManufacturerExporter
            {
                ExportModel = exportModel,
                ExportData = manufacturerExportData
            };

            // When
            this.exporter.Export();

            // Then
            string formatString = "{0}|{1}";
            Worksheet worksheet = exportModel.ExcelWorkbook.Worksheets[0];
            Assert.AreEqual(String.Format(formatString, testManufacturer1.ManufacturerName, testManufacturer1.ManufacturerId), worksheet.Rows[1].Cells[0].Value, "The ManufacturerName on the export of first row did not match expected value");
            Assert.AreEqual(testManufacturer1.ZipCode, worksheet.Rows[1].Cells[1].Value, "The Zip Code on the export of first row did not match expected value");
            Assert.AreEqual(testManufacturer1.ArCustomerId, worksheet.Rows[1].Cells[2].Value, "The AR Customer ID on the export of first row did not match expected value");
            Assert.AreEqual(String.Format(formatString, testManufacturer2.ManufacturerName, testManufacturer2.ManufacturerId), worksheet.Rows[2].Cells[0].Value, "The ManufacturerName on the export of second row did not match expected value");
            Assert.AreEqual(testManufacturer2.ZipCode, worksheet.Rows[2].Cells[1].Value, "The Zip Code on the export of first row did not match expected value");
            Assert.AreEqual(testManufacturer2.ArCustomerId, worksheet.Rows[2].Cells[2].Value, "The AR Customer ID on the export of first row did not match expected value");
        }
    }
}
