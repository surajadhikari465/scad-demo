using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel;
using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using Icon.Common.Models;

namespace Icon.Web.Tests.Unit.Exporters
{
    [TestClass]
    public class ItemNewTemplateExporterTests
    {
        private WorkbookFormat excelFormat;
        private ExcelExportModel exportModel;
        private Mock<IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>>> mockGetHierarchyClassesQueryHandler;
        private Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>>> mockGetAttributesQueryHandler;
        private Mock<IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>>> mockGetBarcodeTypeQueryHandler;

        [TestInitialize]
        public void Initialize()
        {
            excelFormat = WorkbookFormat.Excel2007;
            exportModel = new ExcelExportModel(excelFormat);
           
            mockGetHierarchyClassesQueryHandler = new Mock<IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>>>();
            mockGetAttributesQueryHandler = new Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>>>();
            mockGetBarcodeTypeQueryHandler = new Mock<IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>>>();
        }

        [TestMethod]
        public void Export_CellValuesInSpreadsheet_AllCellsShouldBeBlank()
        {
            // Given.
            
            List<Hierarchy> fakeHierarchyList = new List<Hierarchy>
            {
                TestHelpers.GetFakeHierarchy()
            };
           
            mockGetHierarchyClassesQueryHandler.Setup(r => r.Search(It.IsAny<GetHierarchyClassesParameters>())).Returns(new List<HierarchyClassModel>());
            mockGetAttributesQueryHandler.Setup(q => q.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>())).Returns(new List<AttributeModel>());
            mockGetBarcodeTypeQueryHandler.Setup(q => q.Search(It.IsAny<GetBarcodeTypeParameters>())).Returns(new List<BarcodeTypeModel>());

            var exporter = new ItemNewTemplateExporter(
                mockGetHierarchyClassesQueryHandler.Object,
                mockGetAttributesQueryHandler.Object,
                mockGetBarcodeTypeQueryHandler.Object);

            exporter.ExportModel = exportModel;
            exporter.ExportNewItemTemplate = false;
            exporter.SelectedColumnNames = new List<string>();
            exporter.ExportAllAttributes = false;

            // When.
            exporter.Export();

            // Then.
            Worksheet firstWorksheet = exportModel.ExcelWorkbook.Worksheets[0];

            Assert.IsNull(firstWorksheet.Rows[1].Cells[NewItemExcelHelper.ConsolidatedNewItemColumnIndexes.ScanCodeColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[NewItemExcelHelper.ConsolidatedNewItemColumnIndexes.BrandColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[NewItemExcelHelper.ConsolidatedNewItemColumnIndexes.MerchandiseColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[NewItemExcelHelper.ConsolidatedNewItemColumnIndexes.NationalColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[NewItemExcelHelper.ConsolidatedNewItemColumnIndexes.TaxColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[NewItemExcelHelper.ConsolidatedNewItemColumnIndexes.BarCodeTypeColumnIndex].Value);
        }

        [TestMethod]
        public void Export_PreGeneratedWorksheets_AllWorksheetsShouldBeGenerated()
        {
            // Given.

            List<Hierarchy> fakeHierarchyList = new List<Hierarchy>
            {
                TestHelpers.GetFakeHierarchy()
            };
           
            mockGetHierarchyClassesQueryHandler.Setup(r => r.Search(It.IsAny<GetHierarchyClassesParameters>())).Returns(new List<HierarchyClassModel> ());
            mockGetAttributesQueryHandler.Setup(q => q.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>())).Returns(new List<AttributeModel>());
            mockGetBarcodeTypeQueryHandler.Setup(q => q.Search(It.IsAny<GetBarcodeTypeParameters>())).Returns(new List<BarcodeTypeModel>());

            var exporter = new ItemNewTemplateExporter(
                mockGetHierarchyClassesQueryHandler.Object,
                mockGetAttributesQueryHandler.Object,
                mockGetBarcodeTypeQueryHandler.Object);

            exporter.ExportModel = exportModel;
            exporter.ExportNewItemTemplate = false;
            exporter.SelectedColumnNames = new List<string>();
            exporter.ExportAllAttributes = false;

            // When.
            exporter.Export();

            // Then.
            Worksheet firstWorksheet = exportModel.ExcelWorkbook.Worksheets[0];
            Worksheet brandWorksheet = exportModel.ExcelWorkbook.Worksheets[1];
            Worksheet merchandiseWorksheet = exportModel.ExcelWorkbook.Worksheets[2];
            Worksheet taxWorksheet = exportModel.ExcelWorkbook.Worksheets[3];
            Worksheet nationalWorksheet = exportModel.ExcelWorkbook.Worksheets[4];

            Assert.AreEqual(HierarchyNames.Brands, brandWorksheet.Name);
            Assert.AreEqual(HierarchyNames.Merchandise, merchandiseWorksheet.Name);
            Assert.AreEqual(HierarchyNames.Tax, taxWorksheet.Name);
            Assert.AreEqual(HierarchyNames.National, nationalWorksheet.Name);
        }

        [TestMethod]
        public void Export_HierarchyCellsExceptScanCode_ShouldHaveValidationRules()
        {
            // Given.
            List<Hierarchy> fakeHierarchyList = new List<Hierarchy>
            {
                TestHelpers.GetFakeHierarchy()
            };
           
            mockGetHierarchyClassesQueryHandler.Setup(r => r.Search(It.IsAny<GetHierarchyClassesParameters>())).Returns(new List<HierarchyClassModel> { new HierarchyClassModel { HierarchyLineage = "Brands" } });
            mockGetHierarchyClassesQueryHandler.Setup(r => r.Search(It.IsAny<GetHierarchyClassesParameters>())).Returns(new List<HierarchyClassModel> { new HierarchyClassModel { HierarchyLineage = "Merchandise" } });
            mockGetHierarchyClassesQueryHandler.Setup(r => r.Search(It.IsAny<GetHierarchyClassesParameters>())).Returns(new List<HierarchyClassModel> { new HierarchyClassModel { HierarchyLineage = "Tax" } });
            mockGetHierarchyClassesQueryHandler.Setup(r => r.Search(It.IsAny<GetHierarchyClassesParameters>())).Returns(new List<HierarchyClassModel> { new HierarchyClassModel { HierarchyLineage = "National" } });
            mockGetAttributesQueryHandler.Setup(q => q.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>())).Returns(new List<AttributeModel>());
            mockGetBarcodeTypeQueryHandler.Setup(q => q.Search(It.IsAny<GetBarcodeTypeParameters>())).Returns(new List<BarcodeTypeModel>());

            var exporter = new ItemNewTemplateExporter(
                mockGetHierarchyClassesQueryHandler.Object,
                mockGetAttributesQueryHandler.Object,
                mockGetBarcodeTypeQueryHandler.Object);

            exporter.ExportModel = exportModel;
            exporter.ExportNewItemTemplate = false;
            exporter.SelectedColumnNames = new List<string>();
            exporter.ExportAllAttributes = false;
            
            // When.
            exporter.Export();

            // Then.
            Worksheet firstWorksheet = exportModel.ExcelWorkbook.Worksheets[0];
            Assert.IsNull(firstWorksheet.Rows[1].Cells[1].DataValidationRule);
        }
        private HierarchyClassModel GetFakeHierarchy()
        {
            HierarchyClassModel hierarchyModel = new HierarchyClassModel();

            hierarchyModel.HierarchyClassId = 2;
            hierarchyModel.HierarchyClassName = "Brand";
            hierarchyModel.HierarchyParentClassId = null;

            HierarchyClassModel hierarchyModelTax = new HierarchyClassModel();
            hierarchyModelTax.HierarchyClassId = 3;
            hierarchyModelTax.HierarchyClassName = "Tax";
            hierarchyModelTax.HierarchyParentClassId = null;

            HierarchyClassModel hierarchyModelMerch = new HierarchyClassModel();
            hierarchyModelMerch.HierarchyClassId = 4;
            hierarchyModelMerch.HierarchyClassName = "Merch";
            hierarchyModelMerch.HierarchyParentClassId = null;

            HierarchyClassModel hierarchyModelNational = new HierarchyClassModel();
            hierarchyModelNational.HierarchyClassId = 5;
            hierarchyModelNational.HierarchyClassName = "National";
            hierarchyModelNational.HierarchyParentClassId = null;

            return hierarchyModel;
        }
    }
}