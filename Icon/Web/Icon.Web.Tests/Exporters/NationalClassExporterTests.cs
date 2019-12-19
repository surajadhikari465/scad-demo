using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Icon.Web.Tests.Unit.Exporters
{
    [TestClass]
    public class NationalClassExporterTests
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
        public void NationalClassExporter_NationalClassList_ShouldBeConvertedToWorkbookObject()
        {
            Dictionary<string, string> national = new Dictionary<string,string>();
            national.Add("1", "NationalClass");
            var exporter = new NationalClassExporter();
            exporter.ExportData = national;
            exporter.ExportModel = exportModel;

            // When
            exporter.Export();

            // Then
            Worksheet firstWorksheet = exportModel.ExcelWorkbook.Worksheets[0];

            Assert.AreEqual("NationalClass", firstWorksheet.Rows[1].Cells[0].Value);
        }

    }
}