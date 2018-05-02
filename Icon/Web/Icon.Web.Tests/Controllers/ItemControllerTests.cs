using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Testing.Builders;
using Icon.Web.Common;
using Icon.Web.Common.Validators;
using Icon.Web.Controllers;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.App_Start;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Utility;
using Icon.Web.Tests.Common.Builders;
using Infragistics.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Icon.Web.Tests.Unit.Controllers
{
    [TestClass] [Ignore]
    public class ItemControllerTests
    {
        private Mock<ILogger> mockLogger;
        private Mock<IManagerHandler<UpdateItemManager>> mockUpdateItemManagerHandler;
        private Mock<IManagerHandler<ValidateItemManager>> mockValidateItemManagerHandler;
        private Mock<IManagerHandler<AddItemManager>> mockAddItemManagerHandler;
        private Mock<IObjectValidator<ItemViewModel>> mockItemViewModelValidator;
        private Mock<HttpSessionStateBase> mockSession;
        private Mock<HttpResponseBase> mockResponse;
        private Mock<ControllerContext> mockContext;
        private ItemController controller;
        private List<ItemViewModel> selectedRows;
        private string testUser = "TestUser";
        private Mock<IQueryHandler<GetItemsBySearchParameters, ItemsBySearchResultsModel>> mockGetItemsBySearchQueryHandler;
        private Mock<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>> mockGetHierarchyLineageQueryHandler;
        private Mock<ICommandHandler<AddProductMessageCommand>> mockAddProductMessageCommandHandler;     
        private Mock<IInfragisticsHelper> mockInfragisticsHelper;
        private Mock<UrlHelper> mockUrlHelper;
        private Mock<IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>>> mockGetItemsByBulkScanCodeSearcParameters;

        [TestInitialize]
        public void InitializeTestData()
        {
            this.mockLogger = new Mock<ILogger>();
            this.mockUpdateItemManagerHandler = new Mock<IManagerHandler<UpdateItemManager>>();
            this.mockValidateItemManagerHandler = new Mock<IManagerHandler<ValidateItemManager>>();
            this.mockAddItemManagerHandler = new Mock<IManagerHandler<AddItemManager>>();
            this.mockItemViewModelValidator = new Mock<IObjectValidator<ItemViewModel>>();
            this.mockGetItemsBySearchQueryHandler = new Mock<IQueryHandler<GetItemsBySearchParameters, ItemsBySearchResultsModel>>();
            this.mockGetHierarchyLineageQueryHandler = new Mock<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>>();         
            this.mockAddProductMessageCommandHandler = new Mock<ICommandHandler<AddProductMessageCommand>>();
            this.mockInfragisticsHelper = new Mock<IInfragisticsHelper>();
            this.mockUrlHelper = new Mock<UrlHelper>();
            mockGetItemsByBulkScanCodeSearcParameters = new Mock<IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>>>();

            this.mockSession = new Mock<HttpSessionStateBase>();
            this.mockResponse = new Mock<HttpResponseBase>();
            this.mockContext = new Mock<ControllerContext>();

            this.controller = new ItemController(mockLogger.Object,
                mockUpdateItemManagerHandler.Object,
                mockValidateItemManagerHandler.Object,
                mockAddItemManagerHandler.Object,
                mockItemViewModelValidator.Object,
                mockGetItemsBySearchQueryHandler.Object,
                mockGetHierarchyLineageQueryHandler.Object,
                mockAddProductMessageCommandHandler.Object,
                mockInfragisticsHelper.Object, 
                mockGetItemsByBulkScanCodeSearcParameters.Object);
      
            mockContext.SetupGet(c => c.HttpContext.User.Identity.Name).Returns(testUser);
            controller.ControllerContext = mockContext.Object;

            this.selectedRows = new List<ItemViewModel>
			{
				new ItemViewModel
				{
					ItemId = 1,
					ScanCode = "12345123456"
				},
				new ItemViewModel
				{
					ItemId = 1,
					ScanCode = "12345123457"
				},
				new ItemViewModel
				{
					ItemId = 1,
					ScanCode = "12345123458"
				}
			};

            AutoMapperWebConfiguration.Configure();
            controller.Url = mockUrlHelper.Object;
        }

        [TestCleanup]
        public void Cleanup()
        {
            Mapper.Reset();
        }

        [TestCategory("Controller"), TestCategory("Item Search")]
        [TestMethod]
        public void Index_IndexAction_ReturnsNewItemSearchViewModel()
        {
            // When.
            var result = controller.Index() as ViewResult;

            // Then.
            result.Model.Equals(new ItemSearchViewModel());
        }

        [TestCategory("Controller"), TestCategory("Item Search")]
        [TestMethod]
        public void Search_InvalidModelState_ReturnsSearchOptionsPartialView()
        {
            // Given.
            mockResponse.SetupGet(r => r.StatusCode).Returns(500);
            mockSession.SetupSet(s => s["grid_search_results"] = new List<ItemViewModel>());
            mockContext.Setup(c => c.HttpContext.Session).Returns(mockSession.Object);
            mockContext.Setup(c => c.HttpContext.Response).Returns(mockResponse.Object);
            controller.ControllerContext = mockContext.Object;

            // When.
            controller.ModelState.AddModelError("test", "test");
            var result = controller.Search(new ItemSearchViewModel()) as PartialViewResult;

            // Then.
            result.ViewName = "_ItemSearchOptionsPartial";
        }

        [TestCategory("Controller"), TestCategory("Item Search")]
        [TestMethod]
        public void Search_ValidSearchParameters_ReturnsNoItems()
        {
            // Given.
            ItemSearchViewModel searchItem = new ItemSearchViewModel();
            List<HierarchyClass> hierarchyClasses = new List<HierarchyClass>
                {
                    new HierarchyClass { hierarchyClassName = "Test HierarchyClass", hierarchyClassID = 123, hierarchyLevel = 1 }
                };
            Hierarchy hierarchy = new Hierarchy
            {
                hierarchyID = 1,
                hierarchyName = "Test Hierarchy",
                HierarchyPrototype = null
            };

            controller.ControllerContext = mockContext.Object;

            mockGetItemsBySearchQueryHandler.Setup(r => r.Search(It.IsAny<GetItemsBySearchParameters>())).Returns(new ItemsBySearchResultsModel { ItemsCount = 1 });
            mockGetHierarchyLineageQueryHandler.Setup(r => r.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(GetFakeHierarchy());

            // When.
            PartialViewResult result = controller.Search(searchItem) as PartialViewResult;
            ItemSearchResultsViewModel resultModel = result.Model as ItemSearchResultsViewModel;

            // Then.
            int expectedCount = 0;
            int actualCount = resultModel.Items.Count;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void Search_FullSearchParameters_ShouldSendParametersToCommandHandler()
        {
            //Given
            var viewModel = new ItemSearchViewModel
                {
                    BrandName = "Test Brand",
                    ProductDescription = "Test Desc",
                    MerchandiseHierarchy = "Test Merch",
                    NationalHierarchy = "Test National",
                    PackageUnit = "Test PackageUnit",
                    PartialBrandName = true,
                    PartialScanCode = true,
                    PosDescription = "Test POS Desc",
                    PosScaleTare = "Test PosScaleTare",
                    RetailSize = "Test Retail Size",
                    ScanCode = "Test Scan Code",
                    SelectedDepartmentSaleId = "Test Dept",
                    SelectedFoodStampId = "Test FoodStamp",
                    SelectedRetailUom = "Test UOM",
                    SelectedDeliverySystem = "Test Delivery System",
                    TaxHierarchy = "Test Tax",
                    ItemSignAttributes = new ItemSignAttributesSearchViewModel
                    {
                        SelectedAnimalWelfareRatingId = AnimalWelfareRatings.Step3,
                        SelectedBiodynamicOption = "Test Biodynamic",
                        SelectedCheeseRawOption = "Test CheeseRaw",
                        GlutenFreeAgency = "Test Gluten",
                        KosherAgency = "Test Kosher",
                        NonGmoAgency = "Test NonGmo",
                        OrganicAgency = "Test Organic",
                        SelectedPremiumBodyCareOption = "Test PremiumBodyCare",
                        SelectedCheeseMilkTypeId = MilkTypes.GoatMilk,
                        SelectedEcoScaleRatingId = EcoScaleRatings.PremiumYellow,
                        SelectedSeafoodCatchTypeId = SeafoodCatchTypes.Wild,
                        SelectedSeafoodFreshOrFrozenId = SeafoodFreshOrFrozenTypes.PreviouslyFrozen,
                        VeganAgency = "Test Vegan",
                        SelectedVegetarianOption = "Test Vegetarian",
                        SelectedWholeTradeOption = "Test WholeTrade"
                    }
                };
            viewModel.SelectedStatusId = int.Parse(viewModel.Status.First(s => s.Text == "Loaded").Value);
            viewModel.SelectedHiddenItemStatusId = int.Parse(viewModel.HiddenStatus.First(s => s.Text == "Hidden").Value);

            mockGetItemsBySearchQueryHandler.Setup(m => m.Search(It.Is<GetItemsBySearchParameters>(p =>
                    p.BrandName == viewModel.BrandName
                    && p.ProductDescription == viewModel.ProductDescription
                    && p.PosDescription == viewModel.PosDescription
                    && p.MerchandiseHierarchy == viewModel.MerchandiseHierarchy
                    && p.NationalClass == viewModel.NationalHierarchy
                    && p.PackageUnit == viewModel.PackageUnit
                    && p.PartialBrandName == viewModel.PartialBrandName
                    && p.PartialScanCode == viewModel.PartialScanCode
                    && p.PosScaleTare == viewModel.PosScaleTare
                    && p.RetailSize == viewModel.RetailSize
                    && p.ScanCode == viewModel.ScanCode
                    && p.DepartmentSale == viewModel.SelectedDepartmentSaleId
                    && p.FoodStampEligible == viewModel.SelectedFoodStampId
                    && p.HiddenItemStatus == HiddenStatus.Hidden
                    && p.RetailUom == viewModel.SelectedRetailUom
                    && p.DeliverySystem == viewModel.SelectedDeliverySystem
                    && p.SearchStatus == SearchStatus.Loaded
                    && p.TaxRomance == viewModel.TaxHierarchy
                    && p.AnimalWelfareRatingId == AnimalWelfareRatings.Step3
                    && p.Biodynamic == viewModel.ItemSignAttributes.SelectedBiodynamicOption
                    && p.MilkTypeId == MilkTypes.GoatMilk
                    && p.CheeseRaw == viewModel.ItemSignAttributes.SelectedCheeseRawOption
                    && p.EcoScaleRatingId == EcoScaleRatings.PremiumYellow
                    && p.GlutenFreeAgency == viewModel.ItemSignAttributes.GlutenFreeAgency
                    && p.KosherAgency == viewModel.ItemSignAttributes.KosherAgency
                    && p.NonGmoAgency == viewModel.ItemSignAttributes.NonGmoAgency
                    && p.OrganicAgency == viewModel.ItemSignAttributes.OrganicAgency
                    && p.PremiumBodyCare == viewModel.ItemSignAttributes.SelectedPremiumBodyCareOption
                    && p.SeafoodFreshOrFrozenId == SeafoodFreshOrFrozenTypes.PreviouslyFrozen
                    && p.SeafoodCatchTypeId == SeafoodCatchTypes.Wild
                    && p.VeganAgency == viewModel.ItemSignAttributes.VeganAgency
                    && p.Vegetarian == viewModel.ItemSignAttributes.SelectedVegetarianOption
                    && p.WholeTrade == viewModel.ItemSignAttributes.SelectedWholeTradeOption)))
                .Returns(new ItemsBySearchResultsModel { Items = new List<ItemSearchModel>() });

            mockGetHierarchyLineageQueryHandler.Setup(r => r.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(GetFakeHierarchy());

            //When
            controller.Search(viewModel);

            //Then
            mockGetItemsBySearchQueryHandler.Verify();
        }

        [TestCategory("Controller"), TestCategory("Item Edit")]
        [TestMethod]
        public void Edit_RequestToEditValidItemId_ReturnsItemEditViewModel()
        {
            // Given.
            List<Hierarchy> hierList = new List<Hierarchy>() { 
				new Hierarchy { 
					HierarchyClass = new List<HierarchyClass>() {
						new HierarchyClass { hierarchyClassID = 1, hierarchyClassName = "fake hier class" }
					}, 
					hierarchyID = 1, 
					hierarchyName = "fake hier", 
					HierarchyPrototype = null 
				} 
			};
                        
            mockGetHierarchyLineageQueryHandler.Setup(r => r.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(GetFakeHierarchy());
            mockGetItemsByBulkScanCodeSearcParameters.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>()))
                .Returns(new List<ItemSearchModel> { GetFakeItemNavigationData() });

            // When.
            var result = controller.Edit("1234") as ViewResult;

            // Then.
            string modelName = result.Model.GetType().ToString();
            Assert.IsTrue(modelName.ToLower().EndsWith("ItemEditViewModel".ToLower()));
        }

        [TestCategory("Controller"), TestCategory("Item Edit")]
        [TestMethod]
        public void Edit_DataInViewForUpdatesCausesError_ResultHasUpdateFailedViewData()
        {
            // Given.
            List<Hierarchy> hierList = new List<Hierarchy>() { 
				new Hierarchy { 
					HierarchyClass = new List<HierarchyClass>() {
						new HierarchyClass { hierarchyClassID = 1, hierarchyClassName = "fake hier class" }
					}, 
					hierarchyID = 1, 
					hierarchyName = "fake hier", 
					HierarchyPrototype = null 
				} 
			};

            mockGetHierarchyLineageQueryHandler.Setup(r => r.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(GetFakeHierarchy());
            mockGetItemsByBulkScanCodeSearcParameters.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>()))
                .Returns(new List<ItemSearchModel> { GetFakeItemNavigationData() });
            mockUpdateItemManagerHandler.Setup(uic => uic.Execute(It.IsAny<UpdateItemManager>())).Throws(new ItemTraitUpdateException("Fake Item Update Error", string.Empty, string.Empty, string.Empty, new Exception()));


            // When.
            var viewModel = new ItemEditViewModel(GetFakeItemNavigationData());
            var result = controller.Edit(viewModel) as ViewResult;

            // Then.
            Assert.IsTrue(result.ViewData["UpdateFailed"] != null);
        }

        [TestMethod]
        public void Edit_TaxClassHasNoTaxAbbreviation_ResultHasUpdateFailedViewDataTaxSpecificErrorMessage()
        {
            // Given
            List<Hierarchy> hierList = new List<Hierarchy>() { 
				new Hierarchy { 
					HierarchyClass = new List<HierarchyClass>() {
						new HierarchyClass { hierarchyClassID = 1, hierarchyClassName = "fake hier class" }
					}, 
					hierarchyID = 1, 
					hierarchyName = "fake hier", 
					HierarchyPrototype = null 
				} 
			};

            mockGetHierarchyLineageQueryHandler.Setup(r => r.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(GetFakeHierarchy());
            mockUpdateItemManagerHandler.Setup(uic => uic.Execute(It.IsAny<UpdateItemManager>()))
                .Throws(new CommandException("The tax class needs a tax abbreviation before it can be assigned to any items."));
            mockGetItemsByBulkScanCodeSearcParameters.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>()))
                .Returns(new List<ItemSearchModel> { GetFakeItemNavigationData() });


            var expectedMessage = "The tax class needs a tax abbreviation before it can be assigned to any items.";

            // When
            var viewModel = new ItemEditViewModel(GetFakeItemNavigationData());
            var result = controller.Edit(viewModel) as ViewResult;
            var actualMessage = result.ViewData["UpdateFailed"];

            // Then
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void Edit_BrandNameDoesNotExist_ShouldReturnError()
        {
            //Given
            mockGetHierarchyLineageQueryHandler.Setup(m => m.Search(It.IsAny<GetHierarchyLineageParameters>()))
                .Returns(GetFakeHierarchy());
            mockGetItemsByBulkScanCodeSearcParameters.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>()))
                .Returns(new List<ItemSearchModel> { GetFakeItemNavigationData() });

            //When
            var result = controller.Edit(new ItemEditViewModel() { BrandName = "Not Exists" }) as ViewResult;

            //Then
            Assert.AreEqual("Brand Not Exists does not exist.", result.ViewData["UpdateFailed"]);
        }

        [TestCategory("Controller"), TestCategory("Item Message")]
        [TestMethod]
        public void SendProductMessage_CallsEventHandlerAndRedirectsToEdit()
        {
            // Given.
            Item item = GetFakeItem();
            List<Hierarchy> hierList = new List<Hierarchy>() {
                new Hierarchy {
                    HierarchyClass = new List<HierarchyClass>() {
                        new HierarchyClass { hierarchyClassID = 1, hierarchyClassName = "fake hier class" }
                    },
                    hierarchyID = 1,
                    hierarchyName = "fake hier",
                    HierarchyPrototype = null
                }
            };

            mockGetHierarchyLineageQueryHandler.Setup(r => r.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(GetFakeHierarchy());
            mockAddProductMessageCommandHandler.Setup(e => e.Execute(It.IsAny<AddProductMessageCommand>()));

            // When.
            var result = controller.SendProductMessage(1, "1234") as RedirectToRouteResult;

            //Then
            Assert.IsNotNull(result);
            Assert.AreEqual("Edit", result.RouteValues["action"]);
            Assert.AreEqual("1234", result.RouteValues["scanCode"]);
            mockAddProductMessageCommandHandler.Verify(e => e.Execute(It.IsAny<AddProductMessageCommand>()), Times.Once);
        }

        [TestMethod]
        public void ValidateSelected_SuccessValidation_ReturnsTrueJsonResult()
        {
            // Given
            var expectedSuccess = true;
            var expectedMessage = "Successfully validated all selected items.";

            mockContext.SetupGet(c => c.HttpContext.User.Identity.Name).Returns(testUser);
            controller.ControllerContext = mockContext.Object;

            // When
            var result = controller.ValidateSelected(selectedRows) as JsonResult;
            var actualSuccess = result.GetDataProperty("Success");
            var actualMessage = result.GetDataProperty("Message");

            // Then
            Assert.AreEqual(expectedSuccess, actualSuccess);
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void ValidateSelected_ErrorLoading_ReturnsFalseJsonResult()
        {
            // Given
            var expectedSuccess = false;
            var expectedMessage = "Validation failed.";

            var exception = new CommandException(expectedMessage);
            mockValidateItemManagerHandler.Setup(c => c.Execute(It.IsAny<ValidateItemManager>())).Throws(exception);

            // When
            var result = controller.ValidateSelected(selectedRows) as JsonResult;
            var actualSuccess = result.GetDataProperty("Success");
            var actualMessage = result.GetDataProperty("Message");

            // Then
            Assert.AreEqual(expectedSuccess, actualSuccess);
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void ValidateSelected_SelectedIsEmpty_ShouldReturnJsonResultWithFalseSuccessAndFailureMessage()
        {
            //Given
            var expectedSuccess = false;
            var expectedMessage = "No items were selected to validate.";

            //When
            var actual = controller.ValidateSelected(new List<ItemViewModel>()) as JsonResult;

            //Then
            Assert.AreEqual(expectedSuccess, actual.GetDataProperty("Success"));
            Assert.AreEqual(expectedMessage, actual.GetDataProperty("Message"));
        }

        [TestMethod]
        public void ValidateSelected_SelectedIsNull_ShouldReturnJsonResultWithFalseSuccessAndFailureMessage()
        {
            //Given
            var expectedSuccess = false;
            var expectedMessage = "No items were selected to validate.";

            //When
            var actual = controller.ValidateSelected(null) as JsonResult;

            //Then
            Assert.AreEqual(expectedSuccess, actual.GetDataProperty("Success"));
            Assert.AreEqual(expectedMessage, actual.GetDataProperty("Message"));
        }

        [TestMethod]
        public void Create_SuccessfullyPopulatesViewModel_ShouldReturnViewModelWithHierarchyClassesPopulated()
        {
            // Given.
            HierarchyClassListModel hierarchyClassViewModel = new HierarchyClassListModel();

            var merchHierarchyClasses = new List<HierarchyClassModel>
            {
                new TestHierarchyClassModelBuilder().WithHierarchyClassName("MerchHierarchyClass1").WithHierarchyClassId(1).WithHierarchyClassLineage("MerchHierarchyClass1")
            };
            
            var taxHierarchyClasses = new List<HierarchyClassModel>
            {
                new TestHierarchyClassModelBuilder().WithHierarchyClassName("TaxHierarchyClass1").WithHierarchyClassId(1).WithHierarchyClassLineage("TaxHierarchyClass1"),
                new TestHierarchyClassModelBuilder().WithHierarchyClassName("TaxHierarchyClass2").WithHierarchyClassId(2).WithHierarchyClassLineage("TaxHierarchyClass2")
            };
            
            var browsingHierarchyClasses = new List<HierarchyClassModel>
            {
                new TestHierarchyClassModelBuilder().WithHierarchyClassName("BrowsingHierarchyClass1").WithHierarchyClassId(1).WithHierarchyClassLineage("BrowsingHierarchyClass1"),
                new TestHierarchyClassModelBuilder().WithHierarchyClassName("BrowsingHierarchyClass2").WithHierarchyClassId(2).WithHierarchyClassLineage("BrowsingHierarchyClass2"),
                new TestHierarchyClassModelBuilder().WithHierarchyClassName("BrowsingHierarchyClass3").WithHierarchyClassId(3).WithHierarchyClassLineage("BrowsingHierarchyClass3")
            };

            var nationalHierarchyClasses = new List<HierarchyClassModel>
            {
                new TestHierarchyClassModelBuilder().WithHierarchyClassName("NationalHierarchyClass1").WithHierarchyClassId(1).WithHierarchyClassLineage("NationalHierarchyClass1"),
                new TestHierarchyClassModelBuilder().WithHierarchyClassName("NationalHierarchyClass2").WithHierarchyClassId(2).WithHierarchyClassLineage("NationalHierarchyClass2")
            };

            hierarchyClassViewModel.MerchandiseHierarchyList = merchHierarchyClasses;
            hierarchyClassViewModel.TaxHierarchyList = taxHierarchyClasses;
            hierarchyClassViewModel.BrowsingHierarchyList = browsingHierarchyClasses;
            hierarchyClassViewModel.NationalHierarchyList = nationalHierarchyClasses;

            mockGetHierarchyLineageQueryHandler.Setup(r => r.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(hierarchyClassViewModel);

            // When.
            var view = controller.Create() as ViewResult;
            
            // Then.
            var viewModel = view.Model as ItemCreateViewModel;
            var actualMerch = viewModel.MerchandiseHierarchyClasses.ToList();
            var actualTax = viewModel.TaxHierarchyClasses.ToList();
            var actualBrowse = viewModel.BrowsingHierarchyClasses.ToList();
            var actualUoms = viewModel.RetailUoms.ToList();
            var actualDeliverySystems = viewModel.DeliverySystems.ToList();

            Assert.AreEqual(1, actualMerch.Count);
            Assert.AreEqual(2, actualTax.Count);
            Assert.AreEqual(3, actualBrowse.Count);
            Assert.AreEqual(string.Empty, actualUoms[0].Text);
            Assert.AreEqual("EA", actualUoms[1].Text);
            Assert.AreEqual("LB", actualUoms[2].Text);
            Assert.AreEqual("CT", actualUoms[3].Text);
            Assert.AreEqual("CAP", actualDeliverySystems[1].Text);
            Assert.AreEqual("CHW", actualDeliverySystems[2].Text);
            Assert.AreEqual("LZ", actualDeliverySystems[3].Text);
        }

        [TestMethod]
        public void Create_SuccessfullyPopulatesViewModel_ShouldReturnViewModelWithAgencyListPopulated()
        {
            // Given.
            HierarchyClassListModel hierarchyClassViewModel = new HierarchyClassListModel();

            var merchHierarchyClasses = new List<HierarchyClassModel>
            {
                new TestHierarchyClassModelBuilder().WithHierarchyClassName("MerchHierarchyClass1").WithHierarchyClassId(1).WithHierarchyClassLineage("MerchHierarchyClass1")
            };

            var taxHierarchyClasses = new List<HierarchyClassModel>
            {
                new TestHierarchyClassModelBuilder().WithHierarchyClassName("TaxHierarchyClass1").WithHierarchyClassId(1).WithHierarchyClassLineage("TaxHierarchyClass1"),
                new TestHierarchyClassModelBuilder().WithHierarchyClassName("TaxHierarchyClass2").WithHierarchyClassId(2).WithHierarchyClassLineage("TaxHierarchyClass2")
            };

            var browsingHierarchyClasses = new List<HierarchyClassModel>
            {
                new TestHierarchyClassModelBuilder().WithHierarchyClassName("BrowsingHierarchyClass1").WithHierarchyClassId(1).WithHierarchyClassLineage("BrowsingHierarchyClass1"),
                new TestHierarchyClassModelBuilder().WithHierarchyClassName("BrowsingHierarchyClass2").WithHierarchyClassId(2).WithHierarchyClassLineage("BrowsingHierarchyClass2"),
                new TestHierarchyClassModelBuilder().WithHierarchyClassName("BrowsingHierarchyClass3").WithHierarchyClassId(3).WithHierarchyClassLineage("BrowsingHierarchyClass3")
            };

            var nationalHierarchyClasses = new List<HierarchyClassModel>
            {
                new TestHierarchyClassModelBuilder().WithHierarchyClassName("NationalHierarchyClass1").WithHierarchyClassId(1).WithHierarchyClassLineage("NationalHierarchyClass1"),
                new TestHierarchyClassModelBuilder().WithHierarchyClassName("NationalHierarchyClass2").WithHierarchyClassId(2).WithHierarchyClassLineage("NationalHierarchyClass2")
            };

            hierarchyClassViewModel.MerchandiseHierarchyList = merchHierarchyClasses;
            hierarchyClassViewModel.TaxHierarchyList = taxHierarchyClasses;
            hierarchyClassViewModel.BrowsingHierarchyList = browsingHierarchyClasses;
            hierarchyClassViewModel.NationalHierarchyList = nationalHierarchyClasses;

            mockGetHierarchyLineageQueryHandler.Setup(r => r.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(hierarchyClassViewModel);

            // When.
            var view = controller.Create() as ViewResult;

            // Then.
            var viewModel = view.Model as ItemCreateViewModel;
            var actualGlutenFreeAgencies = viewModel.GlutenFreeAgencies.ToList();
            var actualKosherAgencies = viewModel.KosherAgencies.ToList();
            var actualNonGmoAgencies = viewModel.NonGmoAgencies.ToList();
            var actualOrganicAgencies = viewModel.OrganicAgencies.ToList();
            var actualVeganAgencies = viewModel.VeganAgencies.ToList();

            Assert.AreEqual(1, actualGlutenFreeAgencies.Count);
            Assert.AreEqual(1, actualKosherAgencies.Count);
            Assert.AreEqual(1, actualNonGmoAgencies.Count);
            Assert.AreEqual(1, actualOrganicAgencies.Count);
            Assert.AreEqual(1, actualVeganAgencies.Count);            
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Test Exception")]
        public void Create_QueryHandlersThrowAnException_ShouldThrowException()
        {
            //Given
            mockGetHierarchyLineageQueryHandler.Setup(qh => qh.Search(It.IsAny<GetHierarchyLineageParameters>()))
                .Throws(new Exception("Test Exception"));

            //When
            controller.Create();
        }

        [TestMethod]
        public void Create_ItemCreateIsSuccessful_ShouldReturnRedirectToRouteResultAndCreationMessage()
        {
            //Given
            mockAddItemManagerHandler.Setup(mh => mh.Execute(It.IsAny<AddItemManager>())).Verifiable();

            //When
            var result = controller.Create(new ItemCreateViewModel
            {
                ScanCode = "1234",
                ProductDescription = "Test",
                PosDescription = "Test"
            }) as RedirectToRouteResult;

            //Then
            mockAddItemManagerHandler.Verify();
            Assert.IsNotNull(result);
            Assert.AreEqual("Created item successfully.", controller.TempData["CreateItemMessage"]);
        }

        [TestMethod]
        public void Create_ItemCreateNotSuccessful_ShouldReturnFailureMessageToUserAndFillSelectLists()
        {
            // Given.
            HierarchyClassListModel hierarchyClassViewModel = new HierarchyClassListModel();
            mockAddItemManagerHandler.Setup(mh => mh.Execute(It.IsAny<AddItemManager>())).Throws(new Exception("Test Exception"));

            var merchHierarchyClasses = new List<HierarchyClassModel>
            {
                new TestHierarchyClassModelBuilder().WithHierarchyClassName("MerchHierarchyClass1").WithHierarchyClassId(1).WithHierarchyClassLineage("MerchHierarchyClass1")
            };
          

            var taxHierarchyClasses = new List<HierarchyClassModel>
            {
                new TestHierarchyClassModelBuilder().WithHierarchyClassName("TaxHierarchyClass1").WithHierarchyClassId(1).WithHierarchyClassLineage("TaxHierarchyClass1"),
                new TestHierarchyClassModelBuilder().WithHierarchyClassName("TaxHierarchyClass2").WithHierarchyClassId(2).WithHierarchyClassLineage("TaxHierarchyClass2")
            };
           
            var browsingHierarchyClasses = new List<HierarchyClassModel>
            {
                new TestHierarchyClassModelBuilder().WithHierarchyClassName("BrowsingHierarchyClass1").WithHierarchyClassId(1).WithHierarchyClassLineage("BrowsingHierarchyClass1"),
                new TestHierarchyClassModelBuilder().WithHierarchyClassName("BrowsingHierarchyClass1").WithHierarchyClassId(2).WithHierarchyClassLineage("BrowsingHierarchyClass1"),
                new TestHierarchyClassModelBuilder().WithHierarchyClassName("BrowsingHierarchyClass1").WithHierarchyClassId(3).WithHierarchyClassLineage("BrowsingHierarchyClass1")
            };
            var nationalHierarchyClasses = new List<HierarchyClassModel>
            {
                new TestHierarchyClassModelBuilder().WithHierarchyClassName("NationalHierarchyClass1").WithHierarchyClassId(1).WithHierarchyClassLineage("NationalHierarchyClass1"),
                new TestHierarchyClassModelBuilder().WithHierarchyClassName("NationalHierarchyClass1").WithHierarchyClassId(2).WithHierarchyClassLineage("NationalHierarchyClass1")
            };

            hierarchyClassViewModel.MerchandiseHierarchyList = merchHierarchyClasses;
            hierarchyClassViewModel.TaxHierarchyList = taxHierarchyClasses;
            hierarchyClassViewModel.BrowsingHierarchyList = browsingHierarchyClasses;
            hierarchyClassViewModel.NationalHierarchyList = nationalHierarchyClasses;
            mockGetHierarchyLineageQueryHandler.Setup(r => r.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(hierarchyClassViewModel);

            // When.
            var result = controller.Create(new ItemCreateViewModel()) as ViewResult;

            // Then.
            var viewModel = result.Model as ItemCreateViewModel;
            var actualMerchandise = viewModel.MerchandiseHierarchyClasses.ToList();
            var actualTax = viewModel.TaxHierarchyClasses.ToList();
            var actualBrowsing = viewModel.BrowsingHierarchyClasses.ToList();
            var actualUoms = viewModel.RetailUoms.ToList();
            var actualDeliverySystems = viewModel.DeliverySystems.ToList();

            Assert.AreEqual(1, actualMerchandise.Count);
            Assert.AreEqual(2, actualTax.Count);
            Assert.AreEqual(3, actualBrowsing.Count);
            Assert.AreEqual(string.Empty, actualUoms[0].Text);
            Assert.AreEqual("EA", actualUoms[1].Text);
            Assert.AreEqual("LB", actualUoms[2].Text);
            Assert.AreEqual("CT", actualUoms[3].Text);
            Assert.AreEqual("CAP", actualDeliverySystems[1].Text);
            Assert.AreEqual("CHW", actualDeliverySystems[2].Text);
            Assert.AreEqual("LZ", actualDeliverySystems[3].Text);
        }

        [TestMethod]
        public void Create_ModelStateIsInvalid_ShouldReturnViewWithValidationErrors()
        {
            //Given
            controller.ModelState.AddModelError("test", "test");
            mockGetHierarchyLineageQueryHandler.Setup(r => r.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(GetFakeHierarchy());


            //When
            var result = controller.Create(new ItemCreateViewModel()) as ViewResult;

            //Then
            Assert.IsNotNull(result);
            Assert.AreEqual(1, controller.ModelState.Count);
            Assert.AreEqual("test", controller.ModelState["test"].Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void SaveChangesInGrid_SuccessfullySaveChanges_ShouldReturnSuccessTrueJsonResult()
        {
            // Given
            List<Transaction<ItemViewModel>> itemViewModels = new List<Transaction<ItemViewModel>>()
            {
                new Transaction<ItemViewModel>
                {
                    row = new ItemViewModel()
                }
            };
            mockItemViewModelValidator.Setup(v => v.Validate(It.IsAny<ItemViewModel>()))
                .Returns(ObjectValidationResult.ValidResult());
            mockUpdateItemManagerHandler.Setup(mh => mh.Execute(It.IsAny<UpdateItemManager>()))
                .Verifiable();
            mockContext.SetupGet(mc => mc.HttpContext.Request.Form)
                .Returns(new NameValueCollection 
                {
                    { "ig_transactions", new JavaScriptSerializer().Serialize(itemViewModels) }
                });

            controller.ControllerContext = mockContext.Object;

            // When
            JsonResult result = controller.SaveChangesInGrid() as JsonResult;

            // Then
            Assert.IsNotNull(result);
            Assert.IsTrue(Convert.ToBoolean(result.GetDataProperty("Success")));
            mockUpdateItemManagerHandler.Verify();
        }

        [TestMethod]
        public void SaveChangesInGrid_NoTransactionsGiven_ShouldSuccessFalseAndErrorJsonResult()
        {
            // Given.
            mockContext.SetupGet(mc => mc.HttpContext.Request.Form)
                .Returns(new NameValueCollection 
                {
                    { "ig_transactions", "[]" }
                });

            // When.
            JsonResult result = controller.SaveChangesInGrid() as JsonResult;

            // Then.
            Assert.IsNotNull(result);
            Assert.IsFalse(Convert.ToBoolean(result.GetDataProperty("Success")));
            Assert.AreEqual("No new values were specified for the item.", result.GetDataProperty("Error"));
        }

        [TestMethod]
        public void SaveChangesInGrid_ViewModelIsInvalid_ShouldReturnFailure()
        {
            // Given.
            var itemViewModels = new List<Transaction<ItemViewModel>>()
            {
                new Transaction<ItemViewModel>
                {
                    row = new ItemViewModel()
                }
            };

            mockUpdateItemManagerHandler.Setup(m => m.Execute(It.IsAny<UpdateItemManager>())).Throws(new ArgumentException("Invalid Result"));
            mockContext.SetupGet(mc => mc.HttpContext.Request.Form).Returns(new NameValueCollection 
                {
                    { "ig_transactions", new JavaScriptSerializer().Serialize(itemViewModels) }
                });

            controller.ControllerContext = mockContext.Object;

            // When.
            var result = controller.SaveChangesInGrid() as JsonResult;

            // Then.
            Assert.IsNotNull(result);
            Assert.IsFalse(Convert.ToBoolean(result.GetDataProperty("Success")));
            Assert.AreEqual("Invalid Result", result.GetDataProperty("Error"));
        }

        [TestMethod]
        public void Search_ViewModel_MerchandiseHierarchyClassesExcludeAffinitySubBricks()
        {
            // Given.
            var hierarchies = new List<Hierarchy>();
            hierarchies.Add(new Hierarchy { hierarchyID = Hierarchies.Merchandise, hierarchyName = HierarchyNames.Merchandise });
            
            hierarchies[0].HierarchyClass = new List<HierarchyClass>();
            hierarchies[0].HierarchyClass.Add(new TestHierarchyClassBuilder().WithHierarchyClassId(1).WithAffinityTrait("1").WithHierarchyLevel(5).Build());
            hierarchies[0].HierarchyClass.Add(new TestHierarchyClassBuilder().WithHierarchyClassId(2).WithHierarchyLevel(5).Build());
            HierarchyClassListModel HierarchyListModal = new HierarchyClassListModel();
            HierarchyListModal.MerchandiseHierarchyList = new List<HierarchyClassModel>();
            HierarchyClass nonAffinityHierarchy = hierarchies[0].HierarchyClass.First(hc => !hc.HierarchyClassTrait.Any(hct => hct.traitID == Traits.Affinity));
            HierarchyClassModel merchLineage = new HierarchyClassModel() { HierarchyClassId = nonAffinityHierarchy.hierarchyClassID, HierarchyClassName = nonAffinityHierarchy.hierarchyClassName };
            HierarchyListModal.MerchandiseHierarchyList.Add(merchLineage);
            HierarchyListModal.BrandHierarchyList = new List<HierarchyClassModel>();
            HierarchyListModal.TaxHierarchyList = new List<HierarchyClassModel>();
            HierarchyListModal.NationalHierarchyList = new List<HierarchyClassModel>();
            mockGetItemsBySearchQueryHandler.Setup(q => q.Search(It.IsAny<GetItemsBySearchParameters>())).Returns(new ItemsBySearchResultsModel { Items = new List<ItemSearchModel> { GetFakeItemNavigationData() } });
            mockGetHierarchyLineageQueryHandler.Setup(r => r.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(HierarchyListModal);

            // When.
            var result = controller.Search(new ItemSearchViewModel { ScanCode = "12345" }) as PartialViewResult;
            var model = result.Model as ItemSearchResultsViewModel;

            // Then.
            HierarchyClass expected = hierarchies[0].HierarchyClass.First(hc => !hc.HierarchyClassTrait.Any(hct => hct.traitID == Traits.Affinity));
            Assert.AreEqual(expected.hierarchyClassID, model.MerchandiseHierarchyClasses[0].HierarchyClassId);
            Assert.AreEqual(expected.hierarchyClassName, model.MerchandiseHierarchyClasses[0].HierarchyClassName);
        }
        
        private Item GetFakeItem()
        {
            Item item = new Item() { itemID = 1 };

            // ScanCode
            item.ScanCode = new List<ScanCode>();
            item.ScanCode.Add(new ScanCode { itemID = 1, scanCode = "1234567890" });

            // BrandName
            item.ItemHierarchyClass = new List<ItemHierarchyClass>();
            item.ItemHierarchyClass.Add(new ItemHierarchyClass { itemID = 1, hierarchyClassID = 1 });

            item.ItemHierarchyClass.First().HierarchyClass = new HierarchyClass();
            item.ItemHierarchyClass.First().HierarchyClass.hierarchyClassID = 1;
            item.ItemHierarchyClass.First().HierarchyClass.hierarchyClassName = "Test";
            item.ItemHierarchyClass.First().HierarchyClass.hierarchyID = 2;

            item.ItemHierarchyClass.First().HierarchyClass.Hierarchy = new Hierarchy();
            item.ItemHierarchyClass.First().HierarchyClass.Hierarchy.hierarchyID = 2;
            item.ItemHierarchyClass.First().HierarchyClass.Hierarchy.hierarchyName = "Brand";

            // ItemTraits
            item.ItemTrait = new List<ItemTrait>();

            // Product Description
            item.ItemTrait.Add(new ItemTrait
            {
                itemID = 1,
                traitValue = null,
                traitID = Traits.ProductDescription,
                Trait = new Trait { traitCode = TraitCodes.ProductDescription, traitID = Traits.ProductDescription }
            });

            // POS Description
            item.ItemTrait.Add(new ItemTrait
            {
                itemID = 1,
                traitValue = null,
                traitID = Traits.PosDescription,
                Trait = new Trait { traitCode = TraitCodes.PosDescription, traitID = Traits.PosDescription }
            });

            // Package Unit
            item.ItemTrait.Add(new ItemTrait
            {
                itemID = 1,
                traitValue = null,
                traitID = Traits.PackageUnit,
                Trait = new Trait { traitCode = TraitCodes.RetailSize, traitID = Traits.PackageUnit }
            });

            // Retail Size
            item.ItemTrait.Add(new ItemTrait
            {
                itemID = 1,
                traitValue = null,
                traitID = Traits.RetailSize,
                Trait = new Trait { traitCode = TraitCodes.RetailSize, traitID = Traits.RetailSize }
            });

            //Retail UOM
            item.ItemTrait.Add(new ItemTrait
            {
                itemID = 1,
                traitValue = null,
                traitID = Traits.RetailUom,
                Trait = new Trait { traitCode = TraitCodes.RetailUom, traitID = Traits.RetailUom }
            });

            //Delivery System
            item.ItemTrait.Add(new ItemTrait
            {
                itemID = 1,
                traitValue = null,
                traitID = Traits.DeliverySystem,
                Trait = new Trait { traitCode = TraitCodes.DeliverySystem, traitID = Traits.DeliverySystem }
            });

            item.ItemTrait.Add(new ItemTrait
            {
                itemID = 1,
                traitValue = null,
                traitID = Traits.InsertDate,
                Trait = new Trait { traitCode = TraitCodes.InsertDate, traitID = Traits.InsertDate }
            });

            return item;
        }
        
        private ItemSearchModel GetFakeItemNavigationData()
        {
            ItemSearchModel itemData = new ItemSearchModel() { ItemId = 1 };

            // ScanCode
            itemData.ScanCode = "1234567890";
            itemData.BrandHierarchyClassId = 2;
            itemData.BrandName = "Brand";
            itemData.MerchandiseHierarchyClassId = 1;
            itemData.MerchandiseHierarchyName =  "Test";
            itemData.ProductDescription = "TestProduct";

            itemData.PosDescription = "";
            itemData.PackageUnit = "1";
            itemData.FoodStampEligible = "1";
            itemData.PosScaleTare = "05";
            itemData.RetailSize ="ea";
            itemData.RetailUom = "pound";
            itemData.DeliverySystem = "cap";           

            itemData.TaxHierarchyName = "TestTax";
            itemData.TaxHierarchyClassId = 3;

            itemData.BrowsingHierarchyName = "TestBrowsing";
            itemData.BrowsingHierarchyClassId = 4;
            itemData.DepartmentSale = "1";

            itemData.CreatedDate = DateTime.Now.ToString();

            return itemData;
        }

        private HierarchyClassListModel GetFakeHierarchy()
        {
            HierarchyClassListModel hierarchyListModal  = new HierarchyClassListModel();
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

            hierarchyListModal.BrandHierarchyList = new List<HierarchyClassModel>{hierarchyModel};
            hierarchyListModal.TaxHierarchyList = new List<HierarchyClassModel>{hierarchyModelTax};
            hierarchyListModal.MerchandiseHierarchyList = new List<HierarchyClassModel> { hierarchyModelMerch };


            HierarchyClassModel hierarchyModelBrowsing = new HierarchyClassModel();
            hierarchyModelBrowsing.HierarchyClassId = 5;
            hierarchyModelBrowsing.HierarchyClassName = "Browsing";
            hierarchyModelBrowsing.HierarchyParentClassId = null;

            HierarchyClassModel hierarchyModelNational = new HierarchyClassModel();
            hierarchyModelNational.HierarchyClassId = 6;
            hierarchyModelNational.HierarchyClassName = "National";
            hierarchyModelNational.HierarchyParentClassId = null;

            hierarchyListModal.BrandHierarchyList = new List<HierarchyClassModel> { hierarchyModel };
            hierarchyListModal.TaxHierarchyList = new List<HierarchyClassModel> { hierarchyModelTax };
            hierarchyListModal.MerchandiseHierarchyList = new List<HierarchyClassModel> { hierarchyModelMerch };
            hierarchyListModal.BrowsingHierarchyList = new List<HierarchyClassModel> { hierarchyModelBrowsing };
            hierarchyListModal.NationalHierarchyList = new List<HierarchyClassModel> { hierarchyModelNational };


            return hierarchyListModal;
        }

        private List<CertificationAgencyModel> BuildFakeAgencies()
        {
            var agencyClasses = new List<CertificationAgencyModel>
            {
                new TestCertificationAgencyModelBuilder().WithHierarchyClassName("GlutenFree").WithHierarchyClassId(9).WithGlutenFree("1"),
                new TestCertificationAgencyModelBuilder().WithHierarchyClassName("WithKosher").WithHierarchyClassId(2).WithKosher("1"),
                new TestCertificationAgencyModelBuilder().WithHierarchyClassName("WithNonGMO").WithHierarchyClassId(3).WithNonGMO("1"),
                new TestCertificationAgencyModelBuilder().WithHierarchyClassName("WithOrganic").WithHierarchyClassId(4).WithOrganic("1"),
                new TestCertificationAgencyModelBuilder().WithHierarchyClassName("WithVegan").WithHierarchyClassId(5).WithVegan("1"),
            };

            return agencyClasses;
        }
    }
}