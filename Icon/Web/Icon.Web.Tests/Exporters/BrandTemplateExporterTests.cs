using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Icon.Web.Tests.Unit.Exporters
{
    [TestClass]
    public class BrandTemplateExporterTests
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
        public void BrandTemplateExporter_Export_PreGeneratedWorksheets_AllColumnsShouldBeGenerated()
        {
            // Given.           
            var exporter = new BrandTemplateExporter();
            exporter.ExportModel = exportModel;

            // When.
            exporter.Export();

            // Then.
            Worksheet brandWorksheet = exportModel.ExcelWorkbook.Worksheets[0];

            Assert.AreEqual("Exported", brandWorksheet.Name);
            Assert.AreEqual("Brand", brandWorksheet.Rows[0].Cells[0].Value, "First column header on the export did not match expected value");
            Assert.AreEqual("Brand Abbreviation", brandWorksheet.Rows[0].Cells[1].Value, "Second column header on the export of first row did not match expected value");
        }
    }
}
