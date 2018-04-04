using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Icon.Web.Tests.Unit.Exporters
{
    [TestClass] [Ignore]
    public class HierarchiesExporterTests
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
        public void HierarchiesExporter_HierarchyList_ShouldBeConvertedToWorkbookObject()
        {
            string hierarchyClassName = "Merchandise";

            List<HierarchyClassExportViewModel> hierarchies = new List<HierarchyClassExportViewModel>
            {
                new HierarchyClassExportViewModel 
                {
                    HierarchyClassName = hierarchyClassName
                }
            };

            var exporter = new HierarchyClassExporter();
            exporter.ExportData = hierarchies;
            exporter.ExportModel = exportModel;

            // When
            exporter.Export();

            // Then
            Worksheet firstWorksheet = exportModel.ExcelWorkbook.Worksheets[0];

            Assert.AreEqual(hierarchyClassName, firstWorksheet.Rows[1].Cells[0].Value);

        }

    }
}
