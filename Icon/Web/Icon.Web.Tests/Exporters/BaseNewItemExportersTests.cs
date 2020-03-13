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
            Assert.AreEqual("Barcode Type", itemTemplateNewExporter.spreadsheetColumns[0].HeaderTitle);
            Assert.AreEqual("Scan Code", itemTemplateNewExporter.spreadsheetColumns[1].HeaderTitle);
            Assert.AreEqual("Brands", itemTemplateNewExporter.spreadsheetColumns[2].HeaderTitle);
            Assert.AreEqual("Merchandise", itemTemplateNewExporter.spreadsheetColumns[3].HeaderTitle);
            Assert.AreEqual("Tax", itemTemplateNewExporter.spreadsheetColumns[4].HeaderTitle);
            Assert.AreEqual("National", itemTemplateNewExporter.spreadsheetColumns[5].HeaderTitle);
        }

        [TestMethod]
        public void GetItemTemplateExporter_NoError_ShouldReturnItemTemplateExporterWithColumnHeader()
        {
            // Given.
            exporterService = new ExcelExporterService();

            // When.
            var itemTemplateNewExporter = exporterService.GetItemTemplateNewExporter(null, true, false);

            // Then.
            Assert.AreEqual("Barcode Type", itemTemplateNewExporter.spreadsheetColumns[0].HeaderTitle);
            Assert.AreEqual("Scan Code", itemTemplateNewExporter.spreadsheetColumns[1].HeaderTitle);
            Assert.AreEqual("Brands", itemTemplateNewExporter.spreadsheetColumns[2].HeaderTitle);
            Assert.AreEqual("Financial", itemTemplateNewExporter.spreadsheetColumns[6].HeaderTitle);
            Assert.AreEqual("ItemId", itemTemplateNewExporter.spreadsheetColumns[8].HeaderTitle);
        }

        [TestMethod]
        public void GetItemTemplateNewExporter_NoError_ShouldReturnItemTemplateNewExporterWithHiddenColumns()
        {
            // Given.
            exporterService = new ExcelExporterService();

            // When.
            var itemTemplateNewExporter = exporterService.GetItemTemplateNewExporter(null, true, true);

            // Then.
            Assert.AreEqual(NewItemExcelHelper.NewExcelExportColumnNames.ItemId, itemTemplateNewExporter.ListHiddenColumnNames[0]);
            Assert.AreEqual(NewItemExcelHelper.NewExcelExportColumnNames.ItemType, itemTemplateNewExporter.ListHiddenColumnNames[1]);
            Assert.AreEqual(NewItemExcelHelper.NewExcelExportColumnNames.Financial, itemTemplateNewExporter.ListHiddenColumnNames[2]);
            Assert.AreEqual(Constants.Attributes.ProhibitDiscount, itemTemplateNewExporter.ListHiddenColumnNames[3]);
            Assert.AreEqual(Constants.Attributes.CreatedBy, itemTemplateNewExporter.ListHiddenColumnNames[4]);
            Assert.AreEqual(Constants.Attributes.CreatedDateTimeUtc, itemTemplateNewExporter.ListHiddenColumnNames[5]);
            Assert.AreEqual(Constants.Attributes.ModifiedBy, itemTemplateNewExporter.ListHiddenColumnNames[6]);
            Assert.AreEqual(Constants.Attributes.ModifiedDateTimeUtc, itemTemplateNewExporter.ListHiddenColumnNames[7]);
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
            Assert.AreEqual(listSelectedColumns.Count, itemTemplateNewExporter.SelectedColumnNames.Count);
        }

        [TestMethod]
        public void GetItemTemplateNewExporter_NoError_ShouldReturnItemTemplateNewExporterWithSelectedColumnsSpreadSheetColumnsInOrder()
        {
            //Given
            List<string> listSelectedColumns = new List<string>()
            {
                NewItemExcelHelper.NewExcelExportColumnNames.Manufacturer,
                NewItemExcelHelper.NewExcelExportColumnNames.Merchandise,
                NewItemExcelHelper.NewExcelExportColumnNames.BarCodeType,
                NewItemExcelHelper.NewExcelExportColumnNames.Brand,
                NewItemExcelHelper.NewExcelExportColumnNames.Financial,
                NewItemExcelHelper.NewExcelExportColumnNames.ItemId,
                NewItemExcelHelper.NewExcelExportColumnNames.ItemType,
                NewItemExcelHelper.NewExcelExportColumnNames.ScanCode,
                NewItemExcelHelper.NewExcelExportColumnNames.Tax,
                NewItemExcelHelper.NewExcelExportColumnNames.NationalClass
            };
            List<string> columnDisplayNames = new List<string>()
            {
                "Manufacturer",
                "Merchandise",
                "Barcode Type",
                "Brands",
                "Financial",
                "ItemId",
                "Item Type Description",
                "Scan Code",
                "Tax",
                "National"
            };
            exporterService = new ExcelExporterService();

            //When
            var itemTemplateNewExporter = exporterService.GetItemTemplateNewExporter(listSelectedColumns, false, false);

            //Then
            Assert.IsNotNull(itemTemplateNewExporter.spreadsheetColumns);
            Assert.AreEqual(listSelectedColumns.Count, itemTemplateNewExporter.spreadsheetColumns.Count);
            Assert.AreEqual(0, itemTemplateNewExporter.spreadsheetColumns.Find(item => item.HeaderTitle == columnDisplayNames[0]).Index);
            Assert.AreEqual(1, itemTemplateNewExporter.spreadsheetColumns.Find(item => item.HeaderTitle == columnDisplayNames[1]).Index);
            Assert.AreEqual(2, itemTemplateNewExporter.spreadsheetColumns.Find(item => item.HeaderTitle == columnDisplayNames[2]).Index);
            Assert.AreEqual(3, itemTemplateNewExporter.spreadsheetColumns.Find(item => item.HeaderTitle == columnDisplayNames[3]).Index);
            Assert.AreEqual(4, itemTemplateNewExporter.spreadsheetColumns.Find(item => item.HeaderTitle == columnDisplayNames[4]).Index);
            Assert.AreEqual(5, itemTemplateNewExporter.spreadsheetColumns.Find(item => item.HeaderTitle == columnDisplayNames[5]).Index);
            Assert.AreEqual(6, itemTemplateNewExporter.spreadsheetColumns.Find(item => item.HeaderTitle == columnDisplayNames[6]).Index);
            Assert.AreEqual(7, itemTemplateNewExporter.spreadsheetColumns.Find(item => item.HeaderTitle == columnDisplayNames[7]).Index);
            Assert.AreEqual(8, itemTemplateNewExporter.spreadsheetColumns.Find(item => item.HeaderTitle == columnDisplayNames[8]).Index);
            Assert.AreEqual(9, itemTemplateNewExporter.spreadsheetColumns.Find(item => item.HeaderTitle == columnDisplayNames[9]).Index);
        }
    }
}