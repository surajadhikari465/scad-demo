using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
using Icon.Web.Tests.Common.Builders;
using Infragistics.Documents.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Icon.Web.Tests.Unit.Exporters
{
    [TestClass]
    public class PluCategoryExporterTests
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
        public void PluCategoryExporter_Export_CategoryList_ShouldBeConvertedToWorkbookObject()
        {
            //Given
            PluCategoryViewModel testPluCategory = new TestPluCategoryBuilder()
                .WithPluCategoryName("TestPLUCategory")
                .WithBeginRange("5")
                .WithEndRange("1");

            List<PluCategoryViewModel> pluCategoryViewList = new List<PluCategoryViewModel>();
            pluCategoryViewList.Add(testPluCategory);

            var exporter = new PluCategoryExporter();
            exporter.ExportData = pluCategoryViewList;
            exporter.ExportModel = exportModel;

            // When.
            exporter.Export();

            // Then.
            Worksheet firstWorksheet = exportModel.ExcelWorkbook.Worksheets[0];

            Assert.AreEqual(testPluCategory.PluCategoryName, firstWorksheet.Rows[1].Cells[0].Value);
            Assert.AreEqual(testPluCategory.BeginRange, firstWorksheet.Rows[1].Cells[1].Value);
            Assert.AreEqual(testPluCategory.EndRange, firstWorksheet.Rows[1].Cells[2].Value);
        }
    }
}
