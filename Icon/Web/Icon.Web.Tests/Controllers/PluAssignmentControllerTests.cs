using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Testing.Builders;
using Icon.Web.Controllers;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
using Icon.Web.Tests.Common.Builders;
using Infragistics.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Icon.Web.Tests.Unit.Controllers
{
    [TestClass] [Ignore]
    public class PluAssignmentControllerTests
    {
        private Mock<ILogger> logger;
        private Mock<IManagerHandler<AddItemManager>> mockAddItemManagerHandler;
        private Mock<IQueryHandler<GetAvailablePlusByCategoryParameters, List<IRMAItem>>> mockGetAvailablePlusByCategoryQueryHandler;
        private Mock<IQueryHandler<GetAvailableScanCodesByCategoryParameters, List<IRMAItem>>> mockGetAvailableScanCodesByCategoryQueryHandler;
        private Mock<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>> mockGetHierarchyLineageQueryHandler;
        private Mock<IQueryHandler<GetPluCategoriesParameters, List<PLUCategory>>> mockGetPluCategoriesQuery;

        private PluAssignmentSearchViewModel viewModel;
        private List<IrmaItemViewModel> selectedRows;
        private Mock<HttpSessionStateBase> mockSession;
        private Mock<ControllerContext> mockContext;
        private PluAssignmentController controller;
        private Mock<IExcelExporterService> mockExcelExporterService;
        private Mock<IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass>> hieararchyQueryHandler;

        private List<IRMAItem> irmaItems;
        private IRMAItem item;
        private List<Hierarchy> hierarchies;
        private string testUser = "TestUser";

        [TestInitialize]
        public void Initialize()
        {
            logger = new Mock<ILogger>();
            mockAddItemManagerHandler = new Mock<IManagerHandler<AddItemManager>>();
            mockGetAvailablePlusByCategoryQueryHandler = new Mock<IQueryHandler<GetAvailablePlusByCategoryParameters, List<IRMAItem>>>();
            mockGetAvailableScanCodesByCategoryQueryHandler = new Mock<IQueryHandler<GetAvailableScanCodesByCategoryParameters, List<IRMAItem>>>();
            mockGetHierarchyLineageQueryHandler = new Mock<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>>();
            mockGetPluCategoriesQuery = new Mock<IQueryHandler<GetPluCategoriesParameters, List<PLUCategory>>>();
            mockSession = new Mock<HttpSessionStateBase>();
            mockContext = new Mock<ControllerContext>();
            hieararchyQueryHandler = new Mock<IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass>>();
            mockExcelExporterService = new Mock<IExcelExporterService>();
            controller = new PluAssignmentController(logger.Object,
                mockAddItemManagerHandler.Object,
                mockGetAvailablePlusByCategoryQueryHandler.Object,
                mockGetAvailableScanCodesByCategoryQueryHandler.Object,
                mockGetHierarchyLineageQueryHandler.Object,
                mockGetPluCategoriesQuery.Object,
                mockExcelExporterService.Object,
                hieararchyQueryHandler.Object);

            //Setup up Username
            mockContext.SetupGet(c => c.HttpContext.User.Identity.Name).Returns(testUser);
            controller.ControllerContext = mockContext.Object;

            // Setup Selected Row Data used for controller actions returning Json results
            selectedRows = new List<IrmaItemViewModel>
            {
                new TestIrmaItemViewModelBuilder().WithIrmaItemId(1).WithIdentifier("12345123456"),
                new TestIrmaItemViewModelBuilder().WithIrmaItemId(1).WithIdentifier("12345123457"),
                new TestIrmaItemViewModelBuilder().WithIrmaItemId(1).WithIdentifier("12345123458")
            };

            // Setup Mock Session
            mockSession.SetupSet(s => s["grid_search_results"] = new List<ItemViewModel>());
            mockContext.Setup(c => c.HttpContext.Session).Returns(mockSession.Object);
            controller.ControllerContext = mockContext.Object;

            // Setup Data for Search Method
            viewModel = new PluAssignmentSearchViewModel() { SelectedPluCategoryId = 1 };
            irmaItems = new List<IRMAItem>();

            this.item = new TestIrmaItemBuilder().WithIrmaItemId(1).WithRegionCode("PN").WithIdentifier("45234532451")
                .WithDefaultIdentifier(true).WithBrandName("New Chapter").WithItemDescription("Search Test Desc")
                .WithPosDescription("Search Pos Desc").WithFoodStamp(true).WithPackageUnit(6).WithRetailSize(33.8M)
                .WithRetailUom("OZ").WithPosScaleTare(2).WithTaxClassId(15).WithMerchandiseClassId(14).Build();

            irmaItems.Add(item);
            hierarchies = GetHierarchies();
            var hierarchyClassLists = GetHierarchyClassLists();

            mockGetAvailablePlusByCategoryQueryHandler.Setup(iq => iq.Search(It.IsAny<GetAvailablePlusByCategoryParameters>())).Returns(irmaItems);
            mockGetAvailableScanCodesByCategoryQueryHandler.Setup(iq => iq.Search(It.IsAny<GetAvailableScanCodesByCategoryParameters>())).Returns(irmaItems);
            mockGetHierarchyLineageQueryHandler.Setup(hq => hq.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(hierarchyClassLists);
            mockGetPluCategoriesQuery.Setup(gp => gp.Search(It.IsAny<GetPluCategoriesParameters>())).Returns(GetAllPluCategories());
        }

        [TestMethod]
        public void Index_IndexAction_ReturnsPluAssignmentSearchViewModel()
        {
            // Given
            var expected = new PluAssignmentSearchViewModel();

            // When
            var result = controller.Index() as ViewResult;

            // Then
            result.Model.Equals(expected);
        }

        [TestMethod]
        public void Search_IndexViewModel_ReturnsPartialViewResultWithViewModelListPopulated()
        {
            //Given
            var expected = item;

            // When
            var result = controller.Search(viewModel) as PartialViewResult;
            var model = result.Model as PluAssignmentSearchViewModel;
            var actual = model.Items;

            // Then           
            Assert.AreEqual(expected.identifier, actual.Select(i => i.Identifier).FirstOrDefault(), "identifier mismatch");
        }

        [TestMethod]
        public void Search_RangeIsPlu_ShouldCallGetPlusQueryHandler()
        {
            //Given
            var expected = item;
            viewModel.SelectedPluCategoryId = 4;

            // When
            var result = controller.Search(viewModel) as PartialViewResult;
            var model = result.Model as PluAssignmentSearchViewModel;
            var actual = model.Items;

            // Then
            mockGetAvailablePlusByCategoryQueryHandler.Verify(m => m.Search(It.IsAny<GetAvailablePlusByCategoryParameters>()), Times.Once);
            mockGetAvailableScanCodesByCategoryQueryHandler.Verify(m => m.Search(It.IsAny<GetAvailableScanCodesByCategoryParameters>()), Times.Never);
            Assert.AreEqual(expected.identifier, actual.Select(i => i.Identifier).FirstOrDefault(), "identifier mismatch");
        }

        [TestMethod]
        public void Search_RangeIsNotPlu_ShouldCallGetScanCodesQueryHandler()
        {
            //Given
            var expected = item;

            // When
            var result = controller.Search(viewModel) as PartialViewResult;
            var model = result.Model as PluAssignmentSearchViewModel;
            var actual = model.Items;

            // Then
            mockGetAvailableScanCodesByCategoryQueryHandler.Verify(m => m.Search(It.IsAny<GetAvailableScanCodesByCategoryParameters>()), Times.Once);
            mockGetAvailablePlusByCategoryQueryHandler.Verify(m => m.Search(It.IsAny<GetAvailablePlusByCategoryParameters>()), Times.Never);
            Assert.AreEqual(expected.identifier, actual.Select(i => i.Identifier).FirstOrDefault(), "identifier mismatch");
        }

        [TestMethod]
        public void SaveChangesInGrid_SuccessValidation_ReturnsTrueJsonResult()
        {
            // Given
            Transaction<IrmaItemViewModel> item = new Transaction<IrmaItemViewModel>
            {
                rowId = "1",
                row = new IrmaItemViewModel
                {
                    IrmaItemId = 1,
                    Identifier = "12345123456",
                    BrandName = "IrmaItem ControllerBrand",
                    ItemDescription = "IrmaItem Controller Test Desc",
                    PosDescription = "IrmaItem Controller Test PosDesc",
                    PackageUnit = 1,
                    FoodStamp = true,
                    PosScaleTare = 0,
                    TaxHierarchyClassId = 11,
                    MerchandiseHierarchyClassId = 11
                }
            };
            var transaction = new List<Transaction<IrmaItemViewModel>>
            {
                item
            };

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string data = serializer.Serialize(transaction);
            var form = new NameValueCollection();
            form.Add("ig_transactions", data);

            var mockHttpContext = new Mock<HttpContextBase>();
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(r => r.Form).Returns(form);
            mockHttpContext.Setup(hc => hc.Request).Returns(mockRequest.Object);
            mockHttpContext.Setup(hc => hc.User.Identity.Name).Returns(testUser);
            mockContext.Setup(mc => mc.HttpContext).Returns(mockHttpContext.Object);
            controller.ControllerContext = mockContext.Object;

            // When
            var result = controller.SaveChangesInGrid() as JsonResult;

            // Then
            Assert.AreEqual(true, result.GetDataProperty("Success"));
        }

        private static List<Hierarchy> GetHierarchies()
        {
            List<HierarchyClass> merchHierarchyClasses = new List<HierarchyClass>
            { 
                new HierarchyClass 
                { 
                    hierarchyID = 1, hierarchyClassID = 14, hierarchyLevel = 1, hierarchyClassName = "Mock Merch Class 1", hierarchyParentClassID = null,
                    Hierarchy = new Hierarchy { hierarchyID = 1, hierarchyName = HierarchyNames.Merchandise }
                }
            };
            List<HierarchyClass> taxHierarchyClasses = new List<HierarchyClass>
            {
                new HierarchyClass
                {
                    hierarchyID = 2, hierarchyClassID = 15, hierarchyLevel = 1, hierarchyClassName = "Mock Tax Class 2", hierarchyParentClassID = null,
                    Hierarchy = new Hierarchy { hierarchyID = 2, hierarchyName = HierarchyNames.Tax }
                }
            };
            List<HierarchyClass> brandHierarchyClasses = new List<HierarchyClass>
            {
                new HierarchyClass
                {
                    hierarchyID = 3, hierarchyClassID = 16, hierarchyLevel = 1, hierarchyClassName = "Mock Brand Class 3", hierarchyParentClassID = null,
                    Hierarchy = new Hierarchy { hierarchyID = 3, hierarchyName = HierarchyNames.Brands }
                }
            };
            List<Hierarchy> hierarchies = new List<Hierarchy> 
            { 
                new Hierarchy 
                { 
                    hierarchyID = 1, hierarchyName = HierarchyNames.Merchandise, HierarchyClass = merchHierarchyClasses 
                },
                new Hierarchy
                {
                    hierarchyID = 2, hierarchyName = HierarchyNames.Tax, HierarchyClass = taxHierarchyClasses
                },
                new Hierarchy
                {
                    hierarchyID = 3, hierarchyName = HierarchyNames.Brands, HierarchyClass = brandHierarchyClasses
                }
            };

            return hierarchies;
        }

        private static HierarchyClassListModel GetHierarchyClassLists()
        {
            var hierarchyClassList = new HierarchyClassListModel();
            hierarchyClassList.BrandHierarchyList = new List<HierarchyClassModel>
            {
                new TestHierarchyClassModelBuilder().WithHierarchyId(Hierarchies.Brands).WithHierarchyClassName("Mock Brand Class 3")
                    .WithHierarchyLevel(1).WithHierarchyClassId(16).WithHierarchyClassParentId(null).Build(),
            };

            hierarchyClassList.MerchandiseHierarchyList = new List<HierarchyClassModel>
            {
                new TestHierarchyClassModelBuilder().WithHierarchyId(Hierarchies.Tax).WithHierarchyClassName("Mock Tax Class 2")
                    .WithHierarchyLevel(1).WithHierarchyClassId(14).WithHierarchyClassParentId(null).Build()
            };

            hierarchyClassList.TaxHierarchyList = new List<HierarchyClassModel>
            {
                new TestHierarchyClassModelBuilder().WithHierarchyId(Hierarchies.Tax).WithHierarchyClassName("Mock Brand Class 1")
                    .WithHierarchyLevel(1).WithHierarchyClassId(15).WithHierarchyClassParentId(null).Build()
            };

            hierarchyClassList.NationalHierarchyList = new List<HierarchyClassModel>
            {
                new TestHierarchyClassModelBuilder().WithHierarchyId(Hierarchies.National).WithHierarchyClassName("Mock National Class 1")
                    .WithHierarchyLevel(4).WithHierarchyClassId(40).WithHierarchyClassParentId(null).Build()
            };

            return hierarchyClassList;
        }

        private List<PLUCategory> GetAllPluCategories()
        {
            List<PLUCategory> allPluList = new List<PLUCategory>();
            allPluList.Add(new PLUCategory() { PluCategoryID = 1, PluCategoryName = "Cat1", BeginRange = "2222222" });
            allPluList.Add(new PLUCategory() { PluCategoryID = 2, PluCategoryName = "Cat2" });
            allPluList.Add(new PLUCategory() { PluCategoryID = 3, PluCategoryName = "NotPlu", BeginRange = "4600000000000" });
            allPluList.Add(new PLUCategory() { PluCategoryID = 4, PluCategoryName = "Plu", BeginRange = "111" });
            return allPluList;
        }

    }
}
