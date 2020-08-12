using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

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

            Assert.IsTrue(brandWorksheet.Rows[0].Cells.Any(c => ((string)c.Value) == "Brand Name"));
            Assert.IsTrue(brandWorksheet.Rows[0].Cells.Any(c => ((string)c.Value) == "Brand ID"));
            Assert.IsTrue(brandWorksheet.Rows[0].Cells.Any(c => ((string)c.Value) == "Brand Abbreviation"));
            Assert.IsTrue(brandWorksheet.Rows[0].Cells.Any(c => ((string)c.Value) == "Designation"));
            Assert.IsTrue(brandWorksheet.Rows[0].Cells.Any(c => ((string)c.Value) == "Parent Company"));
            Assert.IsTrue(brandWorksheet.Rows[0].Cells.Any(c => ((string)c.Value) == "Zip Code"));
            Assert.IsTrue(brandWorksheet.Rows[0].Cells.Any(c => ((string)c.Value) == "Locality"));

            Assert.AreEqual("Brands", brandWorksheet.Name);
            Assert.AreEqual("Reference Brands", exportModel.ExcelWorkbook.Worksheets[1].Name);
            Assert.AreEqual("Designation", exportModel.ExcelWorkbook.Worksheets[2].Name);
            Assert.AreEqual("ParentCompany", exportModel.ExcelWorkbook.Worksheets[3].Name);
        }
    }
}
