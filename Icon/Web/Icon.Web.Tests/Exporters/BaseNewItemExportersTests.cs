using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Common.Models;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel;
using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Icon.Web.Tests.Unit.Exporters
{

    [TestClass]
    public class BaseNewItemExportersTests
    {
        private IExcelExporterService exporterService;
        private WorkbookFormat excelFormat;
        private ExcelExportModel exportModel;
        private Mock<IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>>> mockGetHierarchyClassesQueryHandler;
        private Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>>> mockGetAttributesQueryHandler;
        private Mock<IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>>> mockGetBarcodeTypeQueryHandler;
        private ItemNewTemplateExporter exporterService1;
        private List<SpreadsheetColumn<ExportItemModel>> spreadsheetColumns;


        [TestInitialize]
        public void Initialize()
        {
            excelFormat = WorkbookFormat.Excel2007;
            exportModel = new ExcelExportModel(excelFormat);
            exporterService = new ExcelExporterService();
            mockGetHierarchyClassesQueryHandler = new Mock<IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>>>();
            mockGetAttributesQueryHandler = new Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>>>();
            mockGetBarcodeTypeQueryHandler = new Mock<IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>>>();
            exporterService1 = new ItemNewTemplateExporter(mockGetHierarchyClassesQueryHandler.Object, mockGetAttributesQueryHandler.Object, mockGetBarcodeTypeQueryHandler.Object);
            spreadsheetColumns = new List<SpreadsheetColumn<ExportItemModel>>();
        }


        [TestMethod]
        public void GetItemTemplateNewExporter_NoError_ShouldReturnItemTemplateNewExporterWithColumnHeader()
        {
            // Given.
            exporterService = new ExcelExporterService();

            // When.
            var itemTemplateNewExporter = exporterService.GetItemTemplateNewExporter(null, true, true);

            // Then.
            Assert.AreEqual(itemTemplateNewExporter.spreadsheetColumns[0].HeaderTitle, "Barcode Type");
            Assert.AreEqual(itemTemplateNewExporter.spreadsheetColumns[1].HeaderTitle, "Scan Code");
            Assert.AreEqual(itemTemplateNewExporter.spreadsheetColumns[2].HeaderTitle, "Brands");
            Assert.AreEqual(itemTemplateNewExporter.spreadsheetColumns[3].HeaderTitle, "Merchandise");
            Assert.AreEqual(itemTemplateNewExporter.spreadsheetColumns[4].HeaderTitle, "Tax");
            Assert.AreEqual(itemTemplateNewExporter.spreadsheetColumns[5].HeaderTitle, "Manufacturer");
        }

        [TestMethod]
        public void GetItemTemplateExporter_NoError_ShouldReturnItemTemplateExporterWithColumnHeader()
        {
            // Given.
            exporterService = new ExcelExporterService();

            // When.
            var itemTemplateNewExporter = exporterService.GetItemTemplateNewExporter(null, true, false);

            // Then.
            Assert.AreEqual(itemTemplateNewExporter.spreadsheetColumns[0].HeaderTitle, "Barcode Type");
            Assert.AreEqual(itemTemplateNewExporter.spreadsheetColumns[1].HeaderTitle, "Scan Code");
            Assert.AreEqual(itemTemplateNewExporter.spreadsheetColumns[2].HeaderTitle, "Scan Code Type Description");
            Assert.AreEqual(itemTemplateNewExporter.spreadsheetColumns[7].HeaderTitle, "Financial");
            Assert.AreEqual(itemTemplateNewExporter.spreadsheetColumns[8].HeaderTitle, "ItemId");
        }

        [TestMethod]
        public void GetItemTemplateNewExporter_NoError_ShouldReturnItemTemplateNewExporterWithHiddenColumns()
        {
            // Given.
            exporterService = new ExcelExporterService();

            // When.
            var itemTemplateNewExporter = exporterService.GetItemTemplateNewExporter(null, true, true);

            // Then.
            Assert.AreEqual(itemTemplateNewExporter.ListHiddenColumnNames[0], NewItemExcelHelper.NewExcelExportColumnNames.ItemId);
            Assert.AreEqual(itemTemplateNewExporter.ListHiddenColumnNames[1], NewItemExcelHelper.NewExcelExportColumnNames.ItemType);
            Assert.AreEqual(itemTemplateNewExporter.ListHiddenColumnNames[2], NewItemExcelHelper.NewExcelExportColumnNames.ScanCodeType);
            Assert.AreEqual(itemTemplateNewExporter.ListHiddenColumnNames[3], NewItemExcelHelper.NewExcelExportColumnNames.Financial);
            Assert.AreEqual(itemTemplateNewExporter.ListHiddenColumnNames[4], Constants.Attributes.ProhibitDiscount);
            Assert.AreEqual(itemTemplateNewExporter.ListHiddenColumnNames[5], NewItemExcelHelper.NewExcelExportColumnNames.Manufacturer);

        }

        [TestMethod]
        public void GetItemTemplateNewExporter_NoError_ShouldReturnItemTemplateNewExporterWithSelectedColumnsNull()
        {
            // Given.
            exporterService = new ExcelExporterService();

            // When.
            var itemTemplateNewExporter = exporterService.GetItemTemplateNewExporter(null, true, true);

            // Then.
            Assert.IsNull(itemTemplateNewExporter.SelectedColumnNames);
        }

        [TestMethod]
        public void GetItemTemplateNewExporter_NoError_ShouldReturnItemTemplateNewExporterWithSelectedColumnsNotNull()
        {
            // Given.
            List<string> listSelectedColumns = new List<string>() { "Test1", "Test2" };
            exporterService = new ExcelExporterService();

            // When.
            var itemTemplateNewExporter = exporterService.GetItemTemplateNewExporter(listSelectedColumns, true, true);

            // Then.
            Assert.IsNotNull(itemTemplateNewExporter.SelectedColumnNames);
            Assert.AreEqual(itemTemplateNewExporter.SelectedColumnNames.Count, listSelectedColumns.Count);
        }
    }
}