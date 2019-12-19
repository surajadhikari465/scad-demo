using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Icon.Web.Tests.Unit.Exporters
{
    [TestClass]
    public class AttributeExporterTests
    {
        private WorkbookFormat excelFormat;
        private ExcelExportModel exportModel;
        private AttributeExporter exporter;

        [TestInitialize]
        public void Initialize()
        {
            excelFormat = WorkbookFormat.Excel2007;
            exportModel = new ExcelExportModel(excelFormat);
        }

        [TestMethod]
        public void AttributeExport_Attribute()
        {
            // Given
            var testAttribute1 = new AttributeViewModel
                {DisplayName = "Test1", AttributeName = "Test1", Description = "TEST1"};
            var testAttribute2 = new AttributeViewModel
                {DisplayName = "Test2", AttributeName = "Test2", Description = "TEST2"};
            var attributeExportData = new List<AttributeViewModel>();

            attributeExportData.Add(testAttribute1);
            attributeExportData.Add(testAttribute2);

            this.exporter = new AttributeExporter();
            this.exporter.ExportModel = exportModel;
            this.exporter.ExportData = attributeExportData;

            // When
            this.exporter.Export();

            // Then
            Worksheet worksheet = exportModel.ExcelWorkbook.Worksheets[0];
            Assert.AreEqual(testAttribute1.DisplayName, worksheet.Rows[1].Cells[0].Value);
            Assert.AreEqual(testAttribute1.AttributeName, worksheet.Rows[1].Cells[1].Value);
            Assert.AreEqual(testAttribute2.DisplayName, worksheet.Rows[2].Cells[0].Value);
            Assert.AreEqual(testAttribute2.AttributeName, worksheet.Rows[2].Cells[1].Value);
        }
    }
}