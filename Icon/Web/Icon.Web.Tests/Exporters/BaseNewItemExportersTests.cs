using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Common.Models;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel;
using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Utility;
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
        private Mock<IOrderFieldsHelper> mockOrderFieldsHelper;

        [TestInitialize]
        public void Initialize()
        {
            excelFormat = WorkbookFormat.Excel2007;
            exportModel = new ExcelExportModel(excelFormat);
            mockOrderFieldsHelper = new Mock<IOrderFieldsHelper>();
            mockOrderFieldsHelper.Setup(M => M.OrderAllFields(It.IsAny<List<AttributeViewModel>>())).Returns(new Dictionary<string, string>(){
                { "ItemId", "F" },
                { "RequestNumber", "A" },
                {"BarcodeType","F" },
                {"Inactive","A" },
                {"ItemType", "F" },
                {"ScanCode","F" },
                {"Brand","F" },
                {"ProductDescription", "A" },
                {"POSDescription","A" },
                {"CustomerFriendlyDescription", "A" },
                {"ItemPack", "A" },
                {"RetailSize", "A" },
                {"UOM","A" },
                {"Financial", "F" },
                {"Merchandise", "F" },
                {"National", "F" },
                {"Tax","F" },
                {"FoodStampEligible","A" },
                { "Notes","A" },
                {"DataSource","A" },
                {"Manufacturer", "F" }
                }
              );

            exporterService = new ExcelExporterService(mockOrderFieldsHelper.Object);
            mockGetHierarchyClassesQueryHandler = new Mock<IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>>>();
            mockGetAttributesQueryHandler = new Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>>>();
            mockGetBarcodeTypeQueryHandler = new Mock<IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>>>();
            mockOrderFieldsHelper = new Mock<IOrderFieldsHelper>();
            exporterService1 = new ItemNewTemplateExporter(mockGetHierarchyClassesQueryHandler.Object, mockGetAttributesQueryHandler.Object, mockGetBarcodeTypeQueryHandler.Object, mockOrderFieldsHelper.Object);
            spreadsheetColumns = new List<SpreadsheetColumn<ExportItemModel>>();
        }


        [TestMethod]
        public void GetItemTemplateNewExporter_NoError_ShouldReturnItemTemplateNewExporterWithColumnHeader()
        {
            // When.
            var itemTemplateNewExporter = exporterService.GetItemTemplateNewExporter(null, true, true);

            // Then.
            Assert.AreEqual("Request Number", itemTemplateNewExporter.spreadsheetColumns[0].HeaderTitle);
            Assert.AreEqual("Barcode Type", itemTemplateNewExporter.spreadsheetColumns[1].HeaderTitle);
            Assert.AreEqual("Inactive", itemTemplateNewExporter.spreadsheetColumns[2].HeaderTitle);
            Assert.AreEqual("Scan Code", itemTemplateNewExporter.spreadsheetColumns[3].HeaderTitle);
            Assert.AreEqual("Brands", itemTemplateNewExporter.spreadsheetColumns[4].HeaderTitle);
            Assert.AreEqual("Product Description", itemTemplateNewExporter.spreadsheetColumns[5].HeaderTitle);
        }

        [TestMethod]
        public void GetItemTemplateExporter_NoError_ShouldReturnItemTemplateExporterWithColumnHeader()
        {

            // When.
            var itemTemplateNewExporter = exporterService.GetItemTemplateNewExporter(null, true, false);

            // Then.
            Assert.AreEqual("ItemId", itemTemplateNewExporter.spreadsheetColumns[0].HeaderTitle);
            Assert.AreEqual("Request Number", itemTemplateNewExporter.spreadsheetColumns[1].HeaderTitle);
            Assert.AreEqual("Barcode Type", itemTemplateNewExporter.spreadsheetColumns[2].HeaderTitle);
            Assert.AreEqual("Inactive", itemTemplateNewExporter.spreadsheetColumns[3].HeaderTitle);
            Assert.AreEqual("Item Type Description", itemTemplateNewExporter.spreadsheetColumns[4].HeaderTitle);         
        }

        [TestMethod]
        public void GetItemTemplateNewExporter_NoError_ShouldReturnItemTemplateNewExporterWithHiddenColumns()
        {
   
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

            // When.
            var itemTemplateNewExporter = exporterService.GetItemTemplateNewExporter(null, true, true);

            // Then.
            Assert.IsNull(itemTemplateNewExporter.SelectedColumnNames);
        }

        [TestMethod]
        public void GetItemTemplateNewExporter_NoError_ShouldReturnItemTemplateNewExporterWithSelectedColumnsNotNull()
        {
            // Given.
            List<string> listSelectedColumns = new List<string>() { "ItemId", "BarcodeType" };

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
                NewItemExcelHelper.NewExcelExportColumnNames.BarCodeType,
                NewItemExcelHelper.NewExcelExportColumnNames.ItemId,
                NewItemExcelHelper.NewExcelExportColumnNames.Tax,
                NewItemExcelHelper.NewExcelExportColumnNames.NationalClass
            };

            List<string> columnDisplayNames = new List<string>()
            {
                "Barcode Type",
                "ItemId",
                "Tax",
                "National"
            };
        
            //When
            var itemTemplateNewExporter = exporterService.GetItemTemplateNewExporter(listSelectedColumns, false, false);

            //Then
            Assert.IsNotNull(itemTemplateNewExporter.spreadsheetColumns);
            Assert.AreEqual(listSelectedColumns.Count, itemTemplateNewExporter.spreadsheetColumns.Count);
            Assert.AreEqual(0, itemTemplateNewExporter.spreadsheetColumns.Find(item => item.HeaderTitle == columnDisplayNames[0]).Index);
            Assert.AreEqual(1, itemTemplateNewExporter.spreadsheetColumns.Find(item => item.HeaderTitle == columnDisplayNames[1]).Index);
            Assert.AreEqual(2, itemTemplateNewExporter.spreadsheetColumns.Find(item => item.HeaderTitle == columnDisplayNames[2]).Index);
            Assert.AreEqual(3, itemTemplateNewExporter.spreadsheetColumns.Find(item => item.HeaderTitle == columnDisplayNames[3]).Index);
         
        }
    }
}