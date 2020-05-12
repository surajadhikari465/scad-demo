using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Controllers;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.InfragisticsHelpers;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Utility;
using Infragistics.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Icon.Common;
using Icon.Common.Models;
using Icon.Common.Validators.ItemAttributes;
using static Icon.Framework.ItemTypes;
using ScanCodeTypesDescription = Icon.Framework.ScanCodeTypes.Descriptions;
using Icon.Web.Mvc.Utility.ItemHistory;
using Icon.Web.DataAccess.Infrastructure.ItemSearch;
using Icon.Web.Mvc.Domain.BulkImport;
using Icon.Web.Tests.Unit.Models;

namespace Icon.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class ItemControllerTests
    {
        private Mock<IManagerHandler<UpdateItemManager>> mockUpdateItemManagerHandler;
        private Mock<IInfragisticsHelper> mockInfragisticsHelper = new Mock<IInfragisticsHelper>();
        private const string GET_ITEMS_PARAMETERS_VIEW_MODEL = "GetItemsParametersViewModel";
        private const string SELECTED_COLUMN_NAMES = "SelectedColumnNames";
        private const string JSON_CONTENT_TYPE = "application/json";
        private ItemController controller;
        private Mock<ILogger> mockLogger;
        private Mock<IQueryHandler<GetItemsParameters, GetItemsResult>> mockGetItemsQueryHandler;
        private Mock<IQueryHandler<GetItemParameters, ItemDbModel>> mockGetItemQueryHandler;
        private Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>>> mockGetAttributesQueryHandler;
        private Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeInforModel>>, IEnumerable<AttributeInforModel>>> mockGetInforAttributesQueryHandler;
        private Mock<IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>>> mockGetHierarchyClassesQueryHandler;
        private Mock<ControllerContext> context = new Mock<ControllerContext>();
        private MockHttpSessionStateBase session = new MockHttpSessionStateBase();
        private NameValueCollection queryString = new NameValueCollection();
        private GetItemsResult getItemsResult = new GetItemsResult();
        private Mock<IQueryHandler<GetItemPropertiesFromMerchHierarchyParameters, MerchDependentItemPropertiesModel>> mockGetItemPropertiesFromMerchQueryHandler;
        private Mock<IManagerHandler<AddItemManager>> mockAddItemManagerHandler;
        private Mock<IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>>> mockGetBarcodeTypeQueryHandler;
        private Mock<IExcelExporterService> mockExcelExporterService;

        private Mock<IQueryHandler<GetItemHistoryParameters, IEnumerable<ItemHistoryDbModel>>> mockGetItemHistoryQueryHandler;
        private Mock<IQueryHandler<GetItemInforHistoryParameters, IEnumerable<ItemInforHistoryDbModel>>> mockGetInforItemHistoryQueryHandler;
        private Mock<IItemHistoryBuilder> mockItemHistoryBuilder;
        private Mock<IQueryHandler<GetItemHierarchyClassHistoryParameters, ItemHierarchyClassHistoryAllModel>> mockItemHierarchyHistoryQueryHandler;
        private Mock<IHistoryModelTransformer> mockHistoryModelTransformer;
        private string top = "20";
        private string skip = "10";
        private Mock<IItemAttributesValidatorFactory> mockItemAttributesValidatorFactory;
        private Mock<HttpContextBase> fakeHttpContext;
        private GenericIdentity fakeIdentity;
        private GenericPrincipal principal;
        private Mock<ControllerContext> controllerContext;
        private Mock<IQueryHandler<GetItemsByIdSearchParameters, GetItemsResult>> mockGetItemsByIdHandler;
        private Mock<IQueryHandler<EmptyQueryParameters<List<ItemColumnOrderModel>>, List<ItemColumnOrderModel>>> mockGetItemColumnOrderQueryHandler;
        private Mock<IQueryHandler<GetScanCodesParameters, List<string>>> mockGetScanCodeQueryHandler;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger>();
            mockGetItemsQueryHandler = new Mock<IQueryHandler<GetItemsParameters, GetItemsResult>>();
            mockGetItemQueryHandler = new Mock<IQueryHandler<GetItemParameters, ItemDbModel>>();
            mockGetItemHistoryQueryHandler = new Mock<IQueryHandler<GetItemHistoryParameters, IEnumerable<ItemHistoryDbModel>>>();
            mockGetInforItemHistoryQueryHandler = new Mock<IQueryHandler<GetItemInforHistoryParameters, IEnumerable<ItemInforHistoryDbModel>>>();
            mockGetAttributesQueryHandler = new Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>>>();
            mockGetInforAttributesQueryHandler = new Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeInforModel>>, IEnumerable<AttributeInforModel>>>();
            mockGetHierarchyClassesQueryHandler = new Mock<IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>>>();
            mockUpdateItemManagerHandler = new Mock<IManagerHandler<UpdateItemManager>>();
            mockInfragisticsHelper = new Mock<IInfragisticsHelper>();
            mockGetItemPropertiesFromMerchQueryHandler = new Mock<IQueryHandler<GetItemPropertiesFromMerchHierarchyParameters, MerchDependentItemPropertiesModel>>();
            mockAddItemManagerHandler = new Mock<IManagerHandler<AddItemManager>>();
            mockGetBarcodeTypeQueryHandler = new Mock<IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>>>();
            mockExcelExporterService = new Mock<IExcelExporterService>();
            mockItemAttributesValidatorFactory = new Mock<IItemAttributesValidatorFactory>();
            mockItemHistoryBuilder = new Mock<IItemHistoryBuilder>();
            mockHistoryModelTransformer = new Mock<IHistoryModelTransformer>();
            mockItemHierarchyHistoryQueryHandler = new Mock<IQueryHandler<GetItemHierarchyClassHistoryParameters, ItemHierarchyClassHistoryAllModel>>();
            context = new Mock<ControllerContext>();
            session = new MockHttpSessionStateBase();
            fakeHttpContext = new Mock<HttpContextBase>();
            fakeIdentity = new GenericIdentity("User");
            principal = new GenericPrincipal(fakeIdentity, null);
            controllerContext = new Mock<ControllerContext>();
            mockGetItemsByIdHandler = new Mock<IQueryHandler<GetItemsByIdSearchParameters, GetItemsResult>>();
            mockGetItemColumnOrderQueryHandler = new Mock<IQueryHandler<EmptyQueryParameters<List<ItemColumnOrderModel>>, List<ItemColumnOrderModel>>>();
            mockGetScanCodeQueryHandler = new Mock<IQueryHandler<GetScanCodesParameters, List<string>>>();


            controller = new ItemController(
                mockLogger.Object,
                mockGetItemsQueryHandler.Object,
                mockGetItemHistoryQueryHandler.Object,
                mockItemHierarchyHistoryQueryHandler.Object,
                mockGetItemQueryHandler.Object,
                mockGetAttributesQueryHandler.Object,
                mockGetInforAttributesQueryHandler.Object,
                mockGetHierarchyClassesQueryHandler.Object,
                mockUpdateItemManagerHandler.Object,
                mockInfragisticsHelper.Object,
                mockAddItemManagerHandler.Object,
                mockGetItemPropertiesFromMerchQueryHandler.Object,
                mockGetBarcodeTypeQueryHandler.Object,
                mockExcelExporterService.Object,
                mockItemAttributesValidatorFactory.Object,
                mockGetInforItemHistoryQueryHandler.Object,
                new IconWebAppSettings()
                {
                    WriteAccessGroups = "none",
                    ReadAccessGroups = "none",
                    AdminAccessGroups = "none"
                },
                mockItemHistoryBuilder.Object,
                mockHistoryModelTransformer.Object,
                mockGetItemsByIdHandler.Object,
                mockGetItemColumnOrderQueryHandler.Object,
                mockGetScanCodeQueryHandler.Object
                );

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            fakeHttpContext.Setup(t => t.Session).Returns(session);
            fakeHttpContext.Setup(t => t.Request.QueryString).Returns(queryString);
            controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);
            controller.ControllerContext = controllerContext.Object;
            mockGetItemsQueryHandler.Setup(m => m.Search(It.IsAny<GetItemsParameters>())).Returns(getItemsResult);
            getItemsResult.TotalRecordsCount = 10;

            mockGetItemColumnOrderQueryHandler.Setup(M => M.Search(It.IsAny<EmptyQueryParameters<List<ItemColumnOrderModel>>>())).Returns(new List<ItemColumnOrderModel>(){
                { new ItemColumnOrderModel(){ReferenceName = "ItemId", ReferenceNameWithoutSpecialCharacters = "ItemId", ColumnType = "Other" }},
                { new ItemColumnOrderModel(){ReferenceName = "RequestNumber", ReferenceNameWithoutSpecialCharacters = "RequestNumber", ColumnType = "Attribute" }},
                { new ItemColumnOrderModel(){ReferenceName ="BarcodeType", ReferenceNameWithoutSpecialCharacters = "BarcodeType", ColumnType ="Other" }},
                { new ItemColumnOrderModel(){ReferenceName ="Inactive", ReferenceNameWithoutSpecialCharacters = "Inactive", ColumnType ="Attribute" }},
                { new ItemColumnOrderModel(){ReferenceName ="ItemType", ReferenceNameWithoutSpecialCharacters = "ItemType", ColumnType ="Other" }},
                { new ItemColumnOrderModel(){ReferenceName ="ScanCode", ReferenceNameWithoutSpecialCharacters = "ScanCode", ColumnType ="Other" }},
                { new ItemColumnOrderModel(){ReferenceName ="Brand", ReferenceNameWithoutSpecialCharacters = "Brand", ColumnType ="Other" }},
                { new ItemColumnOrderModel(){ReferenceName ="ProductDescription", ReferenceNameWithoutSpecialCharacters = "ProductDescription", ColumnType = "Attribute" }},
                { new ItemColumnOrderModel(){ReferenceName ="POSDescription", ReferenceNameWithoutSpecialCharacters = "POSDescription", ColumnType ="Attribute" }},
                { new ItemColumnOrderModel(){ReferenceName ="CustomerFriendlyDescription", ReferenceNameWithoutSpecialCharacters = "CustomerFriendlyDescription", ColumnType ="Attribute" }},
                { new ItemColumnOrderModel(){ReferenceName ="ItemPack", ReferenceNameWithoutSpecialCharacters = "ItemPack", ColumnType ="Attribute" }},
                { new ItemColumnOrderModel(){ReferenceName ="RetailSize", ReferenceNameWithoutSpecialCharacters = "RetailSize", ColumnType = "Attribute" }},
                { new ItemColumnOrderModel(){ReferenceName ="UOM", ReferenceNameWithoutSpecialCharacters = "UOM", ColumnType ="Attribute" }},
                { new ItemColumnOrderModel(){ReferenceName ="Financial", ReferenceNameWithoutSpecialCharacters = "Financial", ColumnType ="Other" }},
                { new ItemColumnOrderModel(){ReferenceName ="Merchandise", ReferenceNameWithoutSpecialCharacters = "Merchandise", ColumnType ="Other" }},
                { new ItemColumnOrderModel(){ReferenceName ="National", ReferenceNameWithoutSpecialCharacters = "National", ColumnType = "Other" }},
                { new ItemColumnOrderModel(){ReferenceName ="Tax", ReferenceNameWithoutSpecialCharacters = "Tax", ColumnType ="Other" }},
                { new ItemColumnOrderModel(){ReferenceName ="FoodStampEligible", ReferenceNameWithoutSpecialCharacters = "FoodStampEligible", ColumnType ="Attribute" }},
                { new ItemColumnOrderModel(){ReferenceName = "Notes", ReferenceNameWithoutSpecialCharacters = "Notes", ColumnType ="Attribute" }},
                { new ItemColumnOrderModel() { ReferenceName = "DataSource", ReferenceNameWithoutSpecialCharacters = "DataSource", ColumnType = "Attribute" }},
                { new ItemColumnOrderModel() { ReferenceName = "Manufacturer", ReferenceNameWithoutSpecialCharacters = "Manufacturer", ColumnType = "Other" }}
                }
               );

            getItemsResult.Items = new List<ItemDbModel>
            {
                new ItemDbModel
                {
                    BrandsHierarchyClassId = 1,
                    FinancialHierarchyClassId = 2,
                    ItemAttributesJson = "{ProductDescription: 'Test'}",
                    ItemId = 3,
                    ItemTypeId = 4,
                    ItemTypeDescription = Descriptions.RetailSale,
                    MerchandiseHierarchyClassId = 5,
                    NationalHierarchyClassId = 6,
                    ScanCode = "1111",
                    BarcodeTypeId = 1,
                    BarcodeType = "5 Digit POS PLU (10000-82999)",
                    TaxHierarchyClassId = 8
                }
            };

            session.SessionID = Guid.NewGuid().ToString();
            session[GET_ITEMS_PARAMETERS_VIEW_MODEL] = new GetItemsParametersViewModel
            {
                GetItemsAttributesParameters = new List<GetItemsAttributesParameters>()
            };
        }

        [TestMethod]
        public void Index_Get_ReturnsView()
        {
            //When
            var result = controller.Index() as ViewResult;

            //Then
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void SaveGetItemsParameters_PassedInAGetItemsParametersViewModel_ShouldSaveParametersToSession()
        {
            //Given
            GetItemsParametersViewModel viewModel = new GetItemsParametersViewModel
            {
                GetItemsAttributesParameters = new List<GetItemsAttributesParameters>
                {
                    new GetItemsAttributesParameters { AttributeName = "Test1", AttributeValue = "Value1" },
                    new GetItemsAttributesParameters { AttributeName = "Test2", AttributeValue = "Value2" },
                }
            };
            mockLogger.Setup(m => m.Debug(It.IsAny<string>()));

            //When
            controller.SaveGetItemsParameters(viewModel);

            //Then
            Assert.AreEqual(viewModel, session[GET_ITEMS_PARAMETERS_VIEW_MODEL]);
        }

        [TestMethod]
        public void ItemSearchExport_SelectedColumnNamesSent_SessionIsSet()
        {
            //Given
            string[] values = new string[] { "A", "B" };
            //When
            controller.ItemSearchExport(values);

            //Then
            Assert.AreEqual(values, session[SELECTED_COLUMN_NAMES]);
        }

        [TestMethod]
        public void GridDataSource_GetItemsParametersContainsItemId_ShouldSetItemIdOnQueryHandlerParameters()
        {
            //Given
            AssertBasicGridDataSourceTest(new GetItemsAttributesParameters { AttributeName = "ItemId", AttributeValue = "1" });

            //Then
            mockGetItemsQueryHandler.Verify(m => m.Search(It.Is<GetItemsParameters>(
                    p => p.ItemAttributeJsonParameters.First(x => x.AttributeName == "ItemId").Values.First() == "1")),
                    Times.Once);
        }

        [TestMethod]
        public void GridDataSource_GetItemsParametersContainsScanCode_ShouldSetScanCodeOnQueryHandlerParameters()
        {
            //Given
            AssertBasicGridDataSourceTest(new GetItemsAttributesParameters { AttributeName = "ScanCode", AttributeValue = "1010" });

            //Then
            mockGetItemsQueryHandler.Verify(m => m.Search(It.Is<GetItemsParameters>(
                p => p.ItemAttributeJsonParameters.First(x => x.AttributeName == "ScanCode").Values.First() == "1010")),
                Times.Once);
        }

        [TestMethod]
        public void GridDataSource_GetItemsParametersContainsMerchandiseHierarchyClassId_ShouldSetMerchandiseHierarchyClassIdOnQueryHandlerParameters()
        {
            //Given
            AssertBasicGridDataSourceTest(new GetItemsAttributesParameters { AttributeName = "MerchandiseHierarchyClassId", AttributeValue = "1" });

            //Then
            mockGetItemsQueryHandler.Verify(m => m.Search(It.Is<GetItemsParameters>(
                p => p.ItemAttributeJsonParameters.First(x => x.AttributeName == "MerchandiseHierarchyClassId").Values.First() == "1")),
                Times.Once);
        }

        [TestMethod]
        public void GridDataSource_GetItemsParametersContainsBrandsHierarchyClassId_ShouldSetBrandsHierarchyClassIdOnQueryHandlerParameters()
        {
            //Given
            AssertBasicGridDataSourceTest(new GetItemsAttributesParameters { AttributeName = "BrandsHierarchyClassId", AttributeValue = "1" });

            //Then
            mockGetItemsQueryHandler.Verify(m => m.Search(It.Is<GetItemsParameters>(
                p => p.ItemAttributeJsonParameters.First(x => x.AttributeName == "BrandsHierarchyClassId").Values.First() == "1")),
                Times.Once);
        }

        [TestMethod]
        public void GridDataSource_GetItemsParametersContainsTaxHierarchyClassId_ShouldSetTaxHierarchyClassIdOnQueryHandlerParameters()
        {
            //Given
            AssertBasicGridDataSourceTest(new GetItemsAttributesParameters { AttributeName = "TaxHierarchyClassId", AttributeValue = "1" });

            //Then
            mockGetItemsQueryHandler.Verify(m => m.Search(It.Is<GetItemsParameters>(
                p => p.ItemAttributeJsonParameters.First(x => x.AttributeName == "TaxHierarchyClassId").Values.First() == "1")),
                Times.Once);
        }

        [TestMethod]
        public void GridDataSource_GetItemsParametersContainsNationalHierarchyClassId_ShouldSetNationalHierarchyClassIdOnQueryHandlerParameters()
        {
            //Given
            AssertBasicGridDataSourceTest(new GetItemsAttributesParameters { AttributeName = "NationalHierarchyClassId", AttributeValue = "1" });

            //Then
            mockGetItemsQueryHandler.Verify(m => m.Search(It.Is<GetItemsParameters>(
                p => p.ItemAttributeJsonParameters.First(x => x.AttributeName == "NationalHierarchyClassId").Values.First() == "1")),
                Times.Once);
        }

        [TestMethod]
        public void GridDataSource_GetItemsParametersContainsFinancialHierarchyClassId_ShouldSetFinancialHierarchyClassIdOnQueryHandlerParameters()
        {
            //Given
            AssertBasicGridDataSourceTest(new GetItemsAttributesParameters { AttributeName = "FinancialHierarchyClassId", AttributeValue = "1" });

            //Then
            mockGetItemsQueryHandler.Verify(m => m.Search(It.Is<GetItemsParameters>(
                p => p.ItemAttributeJsonParameters.First(x => x.AttributeName == "FinancialHierarchyClassId").Values.First() == "1")),
                Times.Once);
        }

        [TestMethod]
        public void GridDataSource_GetItemsParametersContainsManufacturerHierarchyClassId_ShouldSetManufacturerHierarchyClassIdOnQueryHandlerParameters()
        {
            //Given
            AssertBasicGridDataSourceTest(new GetItemsAttributesParameters { AttributeName = "ManufacturerHierarchyClassId", AttributeValue = "1" });

            //Then
            mockGetItemsQueryHandler.Verify(m => m.Search(It.Is<GetItemsParameters>(
                p => p.ItemAttributeJsonParameters.First(x => x.AttributeName == "ManufacturerHierarchyClassId").Values.First() == "1")),
                Times.Once);
        }

        [TestMethod]
        public void GridDataSource_GetItemsParametersContainsJsonAttributes_ShouldSetItemAttributesJsonOnQueryHandlerParameters()
        {
            //Given
            var parameters = new[]
            {
                new GetItemsAttributesParameters { AttributeName = "Json1", AttributeValue = "1", SearchOperator=AttributeSearchOperator.ContainsAll },
                new GetItemsAttributesParameters { AttributeName = "Json2", AttributeValue = "2", SearchOperator=AttributeSearchOperator.ContainsAll },
                new GetItemsAttributesParameters { AttributeName = "Json3", AttributeValue = "3", SearchOperator=AttributeSearchOperator.ContainsAll }
            };
            AssertBasicGridDataSourceTest(parameters);

            //Then
            mockGetItemsQueryHandler.Verify(m => m.Search(It.Is<GetItemsParameters>(
                    p => JsonConvert.SerializeObject(p.ItemAttributeJsonParameters) == JsonConvert.SerializeObject(parameters))),
                Times.Once);
        }

        [TestMethod]
        public void GridDataSource_GetItemsParametersRequestContainsOrderByValueAndOrderByOrder_ShouldSetOrderByValueAndOrderByOrder()
        {
            //Given
            var parameters = new[] { new GetItemsAttributesParameters { AttributeName = "Json3", AttributeValue = "3", SearchOperator = AttributeSearchOperator.ContainsAll } }.ToList();

            //When
            session[GET_ITEMS_PARAMETERS_VIEW_MODEL] = new GetItemsParametersViewModel
            {
                GetItemsAttributesParameters = parameters
            };
            queryString["$top"] = top;
            queryString["$skip"] = skip;
            queryString["$orderby"] = "ScanCode ASC";

            //When
            var result = controller.GridDataSource() as ContentResult;

            //Then
            Assert.IsNotNull(result);
            Assert.AreEqual(JSON_CONTENT_TYPE, result.ContentType);
            var content = JsonConvert.DeserializeObject<dynamic>(result.Content);
            Assert.AreEqual(10, content.TotalRecordsCount.Value);
            Assert.AreEqual(3, content.Records[0].ItemId.Value);
            mockGetItemsQueryHandler.Verify(m => m.Search(It.Is<GetItemsParameters>(
                    p => p.Top == 20
                         && p.Skip == 10
                         && p.OrderByOrder == "ASC"
                         && p.OrderByValue == "ScanCode"
                         && JsonConvert.SerializeObject(p.ItemAttributeJsonParameters) == JsonConvert.SerializeObject(parameters))),
                     Times.Once);
        }

        [TestMethod]
        public void Detail_ItemIdIsSet_ShouldReturnViewWithItem()
        {
            //Given
            string testScanCode = "testScanCode";
            var testItemModel = new ItemDbModel
            {
                ScanCode = testScanCode,
                BrandsHierarchyClassId = 1,
                FinancialHierarchyClassId = 2,
                ItemAttributesJson = "{TestProp:'TestValue'}",
                ItemTypeId = ItemTypes.RetailSale,
                ItemTypeDescription = Descriptions.RetailSale,
                MerchandiseHierarchyClassId = 3,
                NationalHierarchyClassId = 4,
                BarcodeTypeId = 1,
                BarcodeType = "5 Digit POS PLU (10000-82999)",
                TaxHierarchyClassId = 5,
                ManufacturerHierarchyClassId = 6
            };
            mockGetItemQueryHandler.Setup(m => m.Search(It.Is<GetItemParameters>(p => p.ScanCode == testScanCode)))
                .Returns(testItemModel);
            mockGetAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(new List<AttributeModel> { new AttributeModel { AttributeName = "Test" } });
            mockGetHierarchyClassesQueryHandler.Setup(m => m.Search(It.Is<GetHierarchyClassesParameters>(
                    p => p.HierarchyId == Hierarchies.Merchandise
                         && p.HierarchyClassId == testItemModel.MerchandiseHierarchyClassId)))
                .Returns(new List<HierarchyClassModel> { new HierarchyClassModel { HierarchyLineage = "Merchandise" } });
            mockGetHierarchyClassesQueryHandler.Setup(m => m.Search(It.Is<GetHierarchyClassesParameters>(
                    p => p.HierarchyId == Hierarchies.Brands
                         && p.HierarchyClassId == testItemModel.BrandsHierarchyClassId)))
                .Returns(new List<HierarchyClassModel> { new HierarchyClassModel { HierarchyLineage = "Brands" } });
            mockGetHierarchyClassesQueryHandler.Setup(m => m.Search(It.Is<GetHierarchyClassesParameters>(
                    p => p.HierarchyId == Hierarchies.Tax
                         && p.HierarchyClassId == testItemModel.TaxHierarchyClassId)))
                .Returns(new List<HierarchyClassModel> { new HierarchyClassModel { HierarchyLineage = "Tax" } });
            mockGetHierarchyClassesQueryHandler.Setup(m => m.Search(It.Is<GetHierarchyClassesParameters>(
                    p => p.HierarchyId == Hierarchies.Financial
                         && p.HierarchyClassId == testItemModel.FinancialHierarchyClassId)))
                .Returns(new List<HierarchyClassModel> { new HierarchyClassModel { HierarchyLineage = "Financial" } });
            mockGetHierarchyClassesQueryHandler.Setup(m => m.Search(It.Is<GetHierarchyClassesParameters>(
                    p => p.HierarchyId == Hierarchies.National
                         && p.HierarchyClassId == testItemModel.NationalHierarchyClassId)))
                .Returns(new List<HierarchyClassModel> { new HierarchyClassModel { HierarchyLineage = "National" } });
            mockGetHierarchyClassesQueryHandler.Setup(m => m.Search(It.Is<GetHierarchyClassesParameters>(
                    p => p.HierarchyId == Hierarchies.Manufacturer
                         && p.HierarchyClassId == testItemModel.ManufacturerHierarchyClassId)))
                .Returns(new List<HierarchyClassModel> { new HierarchyClassModel { HierarchyLineage = "Manufacturer" } });


            //When
            var result = controller.Detail(testScanCode) as ViewResult;

            //Then
            Assert.IsNotNull(result);
            var model = result.Model as ItemDetailViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(testItemModel.ItemId, model.ItemViewModel.ItemId);
            Assert.AreEqual("Merchandise", model.ItemViewModel.MerchandiseHierarchyLineage);
            Assert.AreEqual("Brands", model.ItemViewModel.BrandsHierarchyLineage);
            Assert.AreEqual("Tax", model.ItemViewModel.TaxHierarchyLineage);
            Assert.AreEqual("Financial", model.ItemViewModel.FinancialHierarchyLineage);
            Assert.AreEqual("National", model.ItemViewModel.NationalHierarchyLineage);
            Assert.AreEqual(21, model.ItemColumnOrderModelList.Count);
            Assert.AreEqual(true, model.ItemColumnOrderModelList.Any(x => x.ReferenceNameWithoutSpecialCharacters == "ItemId"));
            Assert.AreEqual(true, model.ItemColumnOrderModelList.Any(x => x.ReferenceNameWithoutSpecialCharacters == "Notes"));
            Assert.AreEqual(false, model.ItemColumnOrderModelList.Any(x => x.ReferenceNameWithoutSpecialCharacters == "Accessory"));
        }

        [TestMethod]
        public void Edit_UpdateSubmitted_ShouldUpdate()
        {
            //Given
            string testScanCode = "testScanCode";
            var testItemModel = new ItemDbModel
            {
                ScanCode = testScanCode,
                BrandsHierarchyClassId = 1,
                FinancialHierarchyClassId = 2,
                ItemAttributesJson = JsonConvert.SerializeObject(new Dictionary<string, string>()
                {
                    { Constants.Attributes.CreatedBy, "TestCreate" },
                    { Constants.Attributes.CreatedDateTimeUtc, "2001-01-01 12:12:12" },
                    { Constants.Attributes.ModifiedBy, "Test" },
                    { Constants.Attributes.ModifiedDateTimeUtc, "2002-01-01 00:00:00" }
                }),
                ItemTypeId = ItemTypes.RetailSale,
                ItemTypeDescription = Descriptions.RetailSale,
                MerchandiseHierarchyClassId = 3,
                NationalHierarchyClassId = 4,
                BarcodeTypeId = 1,
                BarcodeType = "5 Digit POS PLU (10000-82999)",
                TaxHierarchyClassId = 5,
                ManufacturerHierarchyClassId = 6
            };

            mockUpdateItemManagerHandler.Setup(x => x.Execute(It.IsAny<UpdateItemManager>())).Callback<UpdateItemManager>((item) =>
            {
                Assert.AreEqual("2001-01-01 12:12:12", item.ItemAttributes[Constants.Attributes.CreatedDateTimeUtc], "CreatedDateTimeUtc should not change regardless of what the client sends");
                Assert.IsTrue(item.ItemAttributes[Constants.Attributes.ModifiedBy] != null, "ModifiedBy should be set when updating records");
                Assert.IsTrue(item.ItemAttributes[Constants.Attributes.ModifiedDateTimeUtc] != null, "ModifiedDateTime should be set when updating records");
                Assert.AreEqual("TestCreate", item.ItemAttributes[Constants.Attributes.CreatedBy]);
                Assert.AreEqual("2001-01-01 12:12:12", item.ItemAttributes[Constants.Attributes.CreatedDateTimeUtc]);
            });

            mockGetItemQueryHandler.Setup(m => m.Search(It.Is<GetItemParameters>(p => p.ScanCode == testScanCode)))
                .Returns(testItemModel);

            mockGetAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(new List<AttributeModel> { new AttributeModel { AttributeName = "Test" } });

            mockGetHierarchyClassesQueryHandler.Setup(m => m.Search(It.Is<GetHierarchyClassesParameters>(
                    p => p.HierarchyId == Hierarchies.Merchandise
                         && p.HierarchyClassId == testItemModel.MerchandiseHierarchyClassId)))
                .Returns(new List<HierarchyClassModel> { new HierarchyClassModel { HierarchyLineage = "Merchandise" } });

            mockGetHierarchyClassesQueryHandler.Setup(m => m.Search(It.Is<GetHierarchyClassesParameters>(
                    p => p.HierarchyId == Hierarchies.Brands
                         && p.HierarchyClassId == testItemModel.BrandsHierarchyClassId)))
                .Returns(new List<HierarchyClassModel> { new HierarchyClassModel { HierarchyLineage = "Brands" } });

            mockGetHierarchyClassesQueryHandler.Setup(m => m.Search(It.Is<GetHierarchyClassesParameters>(
                    p => p.HierarchyId == Hierarchies.Tax
                         && p.HierarchyClassId == testItemModel.TaxHierarchyClassId)))
                .Returns(new List<HierarchyClassModel> { new HierarchyClassModel { HierarchyLineage = "Tax" } });

            mockGetHierarchyClassesQueryHandler.Setup(m => m.Search(It.Is<GetHierarchyClassesParameters>(
                    p => p.HierarchyId == Hierarchies.Financial
                         && p.HierarchyClassId == testItemModel.FinancialHierarchyClassId)))
                .Returns(new List<HierarchyClassModel> { new HierarchyClassModel { HierarchyLineage = "Financial" } });

            mockGetHierarchyClassesQueryHandler.Setup(m => m.Search(It.Is<GetHierarchyClassesParameters>(
                    p => p.HierarchyId == Hierarchies.National
                         && p.HierarchyClassId == testItemModel.NationalHierarchyClassId)))
                .Returns(new List<HierarchyClassModel> { new HierarchyClassModel { HierarchyLineage = "National" } });

            mockGetHierarchyClassesQueryHandler.Setup(m => m.Search(It.Is<GetHierarchyClassesParameters>(
                   p => p.HierarchyId == Hierarchies.Manufacturer
                        && p.HierarchyClassId == testItemModel.ManufacturerHierarchyClassId)))
               .Returns(new List<HierarchyClassModel> { new HierarchyClassModel { HierarchyLineage = "Manufacturer" } });

            mockGetItemPropertiesFromMerchQueryHandler.Setup(m => m.Search(It.IsAny<GetItemPropertiesFromMerchHierarchyParameters>()))
                .Returns(new MerchDependentItemPropertiesModel { FinancialHierarcyClassId = 1, ProhibitDiscount = false, NonMerchandiseTraitValue = "test" });

            ItemEditViewModel model = new ItemEditViewModel
            {
                ItemViewModel = new ItemViewModel()
                {
                    ItemId = 1,
                    ScanCode = "testScanCode",
                    MerchandiseHierarchyClassId = 1,
                    BrandsHierarchyClassId = 2,
                    TaxHierarchyClassId = 3,
                    NationalHierarchyClassId = 5,
                    ManufacturerHierarchyClassId = 6,
                    ItemAttributes = new Dictionary<string, string>()
                    {
                        { Constants.Attributes.ProhibitDiscount, "false" },
                        { Constants.Attributes.CreatedDateTimeUtc, DateTime.Parse("2002-01-01").ToString() },
                    }
                }
            };
            // When
            var result = controller.Edit(model) as ViewResult;

            // Then
            mockUpdateItemManagerHandler.Verify(x => x.Execute(It.IsAny<UpdateItemManager>()), Times.Once);
            Assert.IsNotNull(result);
            var modelResult = result.Model as ItemEditViewModel;
            Assert.IsNotNull(modelResult);
            Assert.AreEqual("false", model.ItemViewModel.ItemAttributes[Constants.Attributes.ProhibitDiscount]);
            Assert.AreEqual(2, modelResult.ItemViewModel.FinancialHierarchyClassId);
            Assert.AreEqual(1, modelResult.ItemViewModel.ItemTypeId);
            Assert.IsTrue(modelResult.Success);
            Assert.AreEqual(21, modelResult.ItemColumnOrderModelList.Count);
            Assert.AreEqual(true, modelResult.ItemColumnOrderModelList.Any(x => x.ReferenceNameWithoutSpecialCharacters == "ItemId"));
            Assert.AreEqual(true, modelResult.ItemColumnOrderModelList.Any(x => x.ReferenceNameWithoutSpecialCharacters == "Notes"));
            Assert.AreEqual(true, modelResult.ItemColumnOrderModelList.Any(x => x.ReferenceNameWithoutSpecialCharacters == "Tax"));
            Assert.AreEqual(false, modelResult.ItemColumnOrderModelList.Any(x => x.ReferenceNameWithoutSpecialCharacters == "EStoreEligible"));
        }

        [TestMethod]
        public void Add_AddItemSubmitted_ShouldAddItem()
        {
            //Given
            string testScanCode = "testScanCode";
            var testItemModel = new ItemCreateViewModel
            {
                ScanCode = testScanCode,
                BrandHierarchyClassId = 1,
                ItemAttributes = new Dictionary<string, string>() {
                    { "TestProp", "TestValue" }
                },
                BarcodeTypeId = 2,
                MerchandiseHierarchyClassId = 3,
                NationalHierarchyClassId = 4,
                TaxHierarchyClassId = 5,
                ManufacturerHierarchyClassId = 6,
                ScanCodeType = "Plu"
            };

            mockGetItemPropertiesFromMerchQueryHandler.Setup(m => m.Search(It.IsAny<GetItemPropertiesFromMerchHierarchyParameters>()))
                .Returns(new MerchDependentItemPropertiesModel { FinancialHierarcyClassId = 1, ProhibitDiscount = false, NonMerchandiseTraitValue = "test" });

            mockAddItemManagerHandler.Setup(x => x.Execute(It.IsAny<AddItemManager>())).Callback<AddItemManager>((item) =>
            {
                Assert.IsTrue(item.ItemAttributes[Constants.Attributes.ModifiedBy] != null, "ModifiedBy should be set when creating records");
                Assert.IsTrue(item.ItemAttributes[Constants.Attributes.ModifiedDateTimeUtc] != null, "ModifiedDateTimeUTC should be set when creating records");
                Assert.IsTrue(item.ItemAttributes[Constants.Attributes.CreatedBy] != null, "CreatedBy should be set when creating records");
                Assert.IsTrue(item.ItemAttributes[Constants.Attributes.CreatedDateTimeUtc] != null, "CreatedDateTimeUTC should be set when creating records");
            });
            //When
            var result = controller.Create(testItemModel) as RedirectToRouteResult;

            //Then
            Assert.IsNotNull(result);
            Assert.AreEqual("Detail", result.RouteValues["action"]);
            mockAddItemManagerHandler.Verify(x => x.Execute(It.Is<AddItemManager>(m => m.ItemTypeId == ItemTypes.Ids["RTL"])));
            mockAddItemManagerHandler.Verify(x => x.Execute(It.Is<AddItemManager>(m => m.FinancialHierarchyClassId == 1)));
            mockAddItemManagerHandler.Verify(x => x.Execute(It.Is<AddItemManager>(m => m.ItemAttributes.ContainsKey(Constants.Attributes.ProhibitDiscount))));
            mockAddItemManagerHandler.Verify(x => x.Execute(It.Is<AddItemManager>(m => m.MerchandiseHierarchyClassId == testItemModel.MerchandiseHierarchyClassId)));
            mockAddItemManagerHandler.Verify(x => x.Execute(It.Is<AddItemManager>(m => m.BrandsHierarchyClassId == testItemModel.BrandHierarchyClassId)));
            mockAddItemManagerHandler.Verify(x => x.Execute(It.Is<AddItemManager>(m => m.TaxHierarchyClassId == testItemModel.TaxHierarchyClassId)));
            mockAddItemManagerHandler.Verify(x => x.Execute(It.Is<AddItemManager>(m => m.NationalHierarchyClassId == testItemModel.NationalHierarchyClassId)));
            mockAddItemManagerHandler.Verify(x => x.Execute(It.Is<AddItemManager>(m => m.ManufacturerHierarchyClassId == testItemModel.ManufacturerHierarchyClassId)));
        }

        [TestMethod]
        public void Edit_ItemIdIsSet_ShouldReturnViewWithItem()
        {
            //Given
            string testScanCode = "testScanCode";
            var testItemModel = new ItemDbModel
            {
                ScanCode = testScanCode,
                BrandsHierarchyClassId = 1,
                FinancialHierarchyClassId = 2,
                ItemAttributesJson = "{TestProp:'TestValue'}",
                ItemTypeId = ItemTypes.RetailSale,
                ItemTypeDescription = Descriptions.RetailSale,
                MerchandiseHierarchyClassId = 3,
                NationalHierarchyClassId = 4,
                BarcodeTypeId = 1,
                BarcodeType = "5 Digit POS PLU (10000-82999)",
                TaxHierarchyClassId = 5,
                ManufacturerHierarchyClassId = 6
            };
            mockGetItemQueryHandler.Setup(m => m.Search(It.Is<GetItemParameters>(p => p.ScanCode == testScanCode)))
                .Returns(testItemModel);
            mockGetAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(new List<AttributeModel> { new AttributeModel { AttributeName = "Test" } });

            mockGetHierarchyClassesQueryHandler.Setup(m => m.Search(It.Is<GetHierarchyClassesParameters>(
                    p => p.HierarchyId == Hierarchies.Merchandise
                         && p.HierarchyClassId == testItemModel.MerchandiseHierarchyClassId)))
                .Returns(new List<HierarchyClassModel> { new HierarchyClassModel { HierarchyLineage = "Merchandise" } });

            mockGetHierarchyClassesQueryHandler.Setup(m => m.Search(It.Is<GetHierarchyClassesParameters>(
                    p => p.HierarchyId == Hierarchies.Brands
                         && p.HierarchyClassId == testItemModel.BrandsHierarchyClassId)))
                .Returns(new List<HierarchyClassModel> { new HierarchyClassModel { HierarchyLineage = "Brands" } });

            mockGetHierarchyClassesQueryHandler.Setup(m => m.Search(It.Is<GetHierarchyClassesParameters>(
                    p => p.HierarchyId == Hierarchies.Tax
                         && p.HierarchyClassId == testItemModel.TaxHierarchyClassId)))
                .Returns(new List<HierarchyClassModel> { new HierarchyClassModel { HierarchyLineage = "Tax" } });

            mockGetHierarchyClassesQueryHandler.Setup(m => m.Search(It.Is<GetHierarchyClassesParameters>(
                    p => p.HierarchyId == Hierarchies.Financial
                         && p.HierarchyClassId == testItemModel.FinancialHierarchyClassId)))
                .Returns(new List<HierarchyClassModel> { new HierarchyClassModel { HierarchyLineage = "Financial" } });

            mockGetHierarchyClassesQueryHandler.Setup(m => m.Search(It.Is<GetHierarchyClassesParameters>(
                    p => p.HierarchyId == Hierarchies.National
                         && p.HierarchyClassId == testItemModel.NationalHierarchyClassId)))
                .Returns(new List<HierarchyClassModel> { new HierarchyClassModel { HierarchyLineage = "National" } });

            mockGetHierarchyClassesQueryHandler.Setup(m => m.Search(It.Is<GetHierarchyClassesParameters>(
                p => p.HierarchyId == Hierarchies.Manufacturer
                     && p.HierarchyClassId == testItemModel.ManufacturerHierarchyClassId)))
            .Returns(new List<HierarchyClassModel> { new HierarchyClassModel { HierarchyLineage = "Manufacturer" } });

            //When
            var result = controller.Edit(testScanCode) as ViewResult;

            //Then
            Assert.IsNotNull(result);
            var model = result.Model as ItemEditViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(testItemModel.ItemId, model.ItemViewModel.ItemId);
            Assert.AreEqual("Merchandise", model.ItemViewModel.MerchandiseHierarchyLineage);
            Assert.AreEqual("Brands", model.ItemViewModel.BrandsHierarchyLineage);
            Assert.AreEqual("Tax", model.ItemViewModel.TaxHierarchyLineage);
            Assert.AreEqual("Financial", model.ItemViewModel.FinancialHierarchyLineage);
            Assert.AreEqual("National", model.ItemViewModel.NationalHierarchyLineage);
            Assert.AreEqual("Manufacturer", model.ItemViewModel.ManufacturerHierarchyLineage);
            Assert.AreEqual(21, model.ItemColumnOrderModelList.Count);
            Assert.AreEqual(true, model.ItemColumnOrderModelList.Any(x => x.ReferenceNameWithoutSpecialCharacters == "ItemId"));
            Assert.AreEqual(true, model.ItemColumnOrderModelList.Any(x => x.ReferenceNameWithoutSpecialCharacters == "Notes"));
            Assert.AreEqual(true, model.ItemColumnOrderModelList.Any(x => x.ReferenceNameWithoutSpecialCharacters == "Tax"));
            Assert.AreEqual(false, model.ItemColumnOrderModelList.Any(x => x.ReferenceNameWithoutSpecialCharacters == "EStoreEligible"));
        }

        [TestMethod]
        public void GridUpdate_GivenAnItem_ShouldAssignStaticAttributesAsPropertiesOnManager()
        {
            //Given
            DateTime now = DateTime.UtcNow;
            string expectedCreatedDateTime = DateTime.UtcNow.AddDays(-10).ToString();
            dynamic jObject = new JObject();
            jObject.ItemId = 1;
            jObject.MerchandiseHierarchyClassId = 2;
            jObject.BrandsHierarchyClassId = 3;
            jObject.TaxHierarchyClassId = 4;
            jObject.FinancialHierarchyClassId = 5;
            jObject.NationalHierarchyClassId = 6;
            jObject.ManufacturerHierarchyClassId = 6;
            jObject.ScanCode = "4011";
            jObject.ScanCodeTypeId = 0;
            jObject.ItemTypeId = 0;
            jObject.TestAttribute1 = "Test1";
            jObject.TestAttribute2 = "Test2";

            mockInfragisticsHelper.Setup(m => m.LoadTransactions<JObject>(It.IsAny<NameValueCollection>()))
                .Returns(new List<Transaction<JObject>>
                {
                    new Transaction<JObject>
                    {
                        row = jObject
                    }
                });

            mockItemAttributesValidatorFactory
                .Setup(v => v.CreateItemAttributesJsonValidator(It.IsAny<string>()).Validate(It.IsAny<string>()))
                .Returns(new ItemAttributesValidationResult { ErrorMessages = null, IsValid = true });

            mockGetItemQueryHandler
                .Setup(q => q.Search(It.IsAny<GetItemParameters>()))
                .Returns(new ItemDbModel
                {
                    ItemAttributesJson = JsonConvert.SerializeObject(new Dictionary<string, string>()
                    {
                        {Constants.Attributes.CreatedDateTimeUtc,expectedCreatedDateTime},
                        {Constants.Attributes.CreatedBy,"CreatedUser"}
                    }),
                    ItemId = 1,
                    ItemTypeId = ItemTypes.RetailSale,
                    BarcodeTypeId = 1
                });

            mockGetItemPropertiesFromMerchQueryHandler
                .Setup(q => q.Search(It.IsAny<GetItemPropertiesFromMerchHierarchyParameters>()))
                .Returns(new MerchDependentItemPropertiesModel
                {
                    FinancialHierarcyClassId = 5,
                    NonMerchandiseTraitValue = ItemTypeCodes.RetailSale,
                    ProhibitDiscount = false
                });

            //When
            controller.GridUpdate();

            //Then
            mockUpdateItemManagerHandler.Verify(m => m.Execute(It.Is<UpdateItemManager>(
                uim => uim.ItemId == 1
                       && uim.MerchandiseHierarchyClassId == 2
                       && uim.BrandsHierarchyClassId == 3
                       && uim.TaxHierarchyClassId == 4
                       && uim.FinancialHierarchyClassId == 5
                       && uim.NationalHierarchyClassId == 6
                       && uim.ItemAttributes.Keys.Count == 7
                       && uim.ItemAttributes["TestAttribute1"].ToString() == "Test1"
                       && uim.ItemAttributes["TestAttribute2"].ToString() == "Test2"
                       && uim.ItemAttributes[Constants.Attributes.ProhibitDiscount] == "false"
                       && uim.ItemAttributes[Constants.Attributes.ModifiedBy] == "User"
                       && uim.ItemAttributes[Constants.Attributes.CreatedBy] == "CreatedUser"
                       && uim.ItemAttributes[Constants.Attributes.CreatedDateTimeUtc] == expectedCreatedDateTime
                       && DateTime.Parse(uim.ItemAttributes[Constants.Attributes.ModifiedDateTimeUtc]).Date == now.Date)));
        }

        private void AssertBasicGridDataSourceTest(params GetItemsAttributesParameters[] getItemAttributesParameters)
        {
            session[GET_ITEMS_PARAMETERS_VIEW_MODEL] = new GetItemsParametersViewModel
            {
                GetItemsAttributesParameters = getItemAttributesParameters.ToList()
            };
            queryString["$top"] = top;
            queryString["$skip"] = skip;

            //When
            var result = controller.GridDataSource() as ContentResult;

            //Then
            Assert.IsNotNull(result);
            Assert.AreEqual(JSON_CONTENT_TYPE, result.ContentType);
            var content = JsonConvert.DeserializeObject<dynamic>(result.Content);
            Assert.AreEqual(10, content.TotalRecordsCount.Value);
            Assert.AreEqual(3, content.Records[0].ItemId.Value);
            mockGetItemsQueryHandler.Verify(m => m.Search(It.Is<GetItemsParameters>(
                    p => p.Top == 20
                         && p.Skip == 10
                         && p.OrderByOrder == "ASC"
                         && p.OrderByValue == "ItemId")),
                   Times.Once);
        }

        [TestMethod]
        public void GetMissingScanCodes_WhenScanCodeExactyMatchAny_ShouldReturnWithMissingScanCode()
        {
            //Given
            var parameters = new[] { new GetItemsAttributesParameters { AttributeName = "ScanCode", AttributeValue = "4011 10", SearchOperator = AttributeSearchOperator.ExactlyMatchesAny } }.ToList();

            session[GET_ITEMS_PARAMETERS_VIEW_MODEL] = new GetItemsParametersViewModel
            {
                GetItemsAttributesParameters = parameters
            };

            //When
            mockGetScanCodeQueryHandler.Setup(m => m.Search(It.IsAny<GetScanCodesParameters>()))
              .Returns(new List<string> { "4011" });
            var result = controller.GetMissingScanCodes() as JsonResult;

            //Then
            Assert.IsNotNull(result.Data);
            var json = JsonConvert.SerializeObject(result.Data);
            var missingScanCode = JsonConvert.DeserializeObject<MissingScanCodeModelTests>(json);
            Assert.IsTrue(missingScanCode.MissingScanCodes.Any());
            Assert.AreEqual("10", missingScanCode.MissingScanCodes[0]);
        }
    }

    internal abstract class MockHttpSessionState : HttpSessionStateBase
    {
        // session id was not getting set causing tests to fail
        public sealed override string SessionID
        {
            get { return SessionValue; }
        }

        protected abstract string SessionValue { get; }
    }
    internal class MockHttpSessionStateBase : MockHttpSessionState
    {
        private Dictionary<string, object> dictionary = new Dictionary<string, object>();

        public override object this[string name] { get => dictionary[name]; set => dictionary[name] = value; }

        public new string SessionID { get; set; }

        protected override string SessionValue
        {
            get { return SessionID; }
        }
    }
}