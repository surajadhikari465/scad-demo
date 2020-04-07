using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace Icon.Web.Tests.Unit.Exporters
{
    [TestClass]
    public class BrandTemplateExporterTests
    {
        private WorkbookFormat excelFormat;
        private ExcelExportModel exportModel;
        private Mock<IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>>> mockGetHierarchyClassesQueryHandler;

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
            mockGetHierarchyClassesQueryHandler = new Mock<IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>>>();
            var exporter = new BrandTemplateExporter(mockGetHierarchyClassesQueryHandler.Object);
            exporter.ExportModel = exportModel;

            // When.
            exporter.Export();

            // Then.
            Worksheet brandWorksheet = exportModel.ExcelWorkbook.Worksheets[0];

            Assert.AreEqual("Exported", brandWorksheet.Name);
            Assert.AreEqual("Brand Name", brandWorksheet.Rows[0].Cells[0].Value, "Column header on the export did not match expected value");
            Assert.AreEqual("Brand ID", brandWorksheet.Rows[0].Cells[1].Value, "Column header on the export did not match expected value");
            Assert.AreEqual("Brand Abbreviation", brandWorksheet.Rows[0].Cells[2].Value, "Column header on the export of first row did not match expected value");
            Assert.AreEqual("Designation", brandWorksheet.Rows[0].Cells[3].Value, "Column header on the export did not match expected value");
            Assert.AreEqual("Parent Company", brandWorksheet.Rows[0].Cells[4].Value, "Column header on the export did not match expected value");
            Assert.AreEqual("Zip Code", brandWorksheet.Rows[0].Cells[5].Value, "Column header on the export did not match expected value");
            Assert.AreEqual("Locality", brandWorksheet.Rows[0].Cells[6].Value, "Column header on the export did not match expected value");
            
            Assert.AreEqual("Exported", brandWorksheet.Name);
            Assert.AreEqual("Reference Brands", exportModel.ExcelWorkbook.Worksheets[1].Name);
            Assert.AreEqual("Designation", exportModel.ExcelWorkbook.Worksheets[2].Name);
            Assert.AreEqual("ParentCompany", exportModel.ExcelWorkbook.Worksheets[3].Name);
        }
    }
}
