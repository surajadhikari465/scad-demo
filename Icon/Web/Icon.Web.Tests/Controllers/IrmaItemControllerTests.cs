using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Testing.Builders;
using Icon.Web.Common;
using Icon.Web.Controllers;
using Icon.Web.DataAccess.Commands;
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
    public class IrmaItemControllerTests
    {
        private Mock<ILogger> logger;
        private Mock<IQueryHandler<GetIrmaItemsParameters, List<IRMAItem>>> mockIrmaQuery;
        private Mock<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>> mockHierarchyClassQuery;
        private Mock<ICommandHandler<UpdateIrmaItemCommand>> mockUpdateCommand;
        private Mock<IManagerHandler<AddItemManager>> mockAddItemManager;
        private IrmaItemController controller;
        private List<IrmaItemViewModel> selectedRows;
        private Mock<HttpSessionStateBase> mockSession;
        private Mock<ControllerContext> mockContext;
        private IrmaItemSearchViewModel viewModel;
        private List<IRMAItem> irmaItems;
        private IRMAItem item;
        private List<Hierarchy> hierarchies;
        private string testUser = "TestUser";
        private Mock<IQueryHandler<GetIrmaItemsParameters, List<IRMAItem>>> mockIrmaStoreProcQuery;
        private Mock<ICommandHandler<DeleteIrmaItemCommand>> mockDeleteIrmaItemHandler;
        private Mock<IExcelExporterService> mockExcelExporterService;

        [TestInitialize]
        public void InitializeData()
        {
            logger = new Mock<ILogger>();
            mockIrmaQuery = new Mock<IQueryHandler<GetIrmaItemsParameters, List<IRMAItem>>>();
            mockHierarchyClassQuery = new Mock<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>>();
            mockUpdateCommand = new Mock<ICommandHandler<UpdateIrmaItemCommand>>();
            mockAddItemManager = new Mock<IManagerHandler<AddItemManager>>();
            mockSession = new Mock<HttpSessionStateBase>();
            mockContext = new Mock<ControllerContext>();
            mockDeleteIrmaItemHandler = new Mock<ICommandHandler<DeleteIrmaItemCommand>>();
            mockIrmaStoreProcQuery = new Mock<IQueryHandler<GetIrmaItemsParameters, List<IRMAItem>>>();
            mockExcelExporterService = new Mock<IExcelExporterService>();
           
            controller = new IrmaItemController(logger.Object, 
                mockHierarchyClassQuery.Object,
                mockUpdateCommand.Object, 
                mockAddItemManager.Object, 
                mockIrmaStoreProcQuery.Object,          
                mockDeleteIrmaItemHandler.Object,
                mockExcelExporterService.Object);

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
            viewModel = new IrmaItemSearchViewModel { BrandName = "New Chapter" };
            irmaItems = new List<IRMAItem>();

            this.item = new TestIrmaItemBuilder().WithIrmaItemId(1).WithRegionCode("PN").WithIdentifier("45234532451")
                .WithDefaultIdentifier(true).WithBrandName("New Chapter").WithItemDescription("Search Test Desc")
                .WithPosDescription("Search Pos Desc").WithFoodStamp(true).WithPackageUnit(6).WithRetailSize(33.8M)
                .WithRetailUom("OZ").WithPosScaleTare(2).WithTaxClassId(15).WithMerchandiseClassId(14).WithNationalClassId(20).Build();

            irmaItems.Add(item);
            hierarchies = GetHierarchies();
            var hierarchyClassLists = GetHierarchyClassLists();

            mockIrmaQuery.Setup(iq => iq.Search(It.IsAny<GetIrmaItemsParameters>())).Returns(irmaItems);
            mockIrmaStoreProcQuery.Setup(iq => iq.Search(It.IsAny<GetIrmaItemsParameters>())).Returns(irmaItems);
            mockHierarchyClassQuery.Setup(hq => hq.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(hierarchyClassLists);       
        }

        [TestMethod]
        public void Index_IndexAction_ReturnsNewIrmaItemViewModel()
        {
            // Given
            var expected = new IrmaItemSearchViewModel();

            // When
            var result = controller.Index() as ViewResult;

            // Then
            result.Model.Equals(expected);
        }

        [TestMethod]
        public void Search_SearchViewModel_ReturnsPartialViewResultWithViewModelItemListPopulated()
        {
            // Given
            var expected = item;

            // When
            var result = controller.Search(viewModel) as PartialViewResult;
            var model = result.Model as IrmaItemSearchViewModel;
            var actual = model.Items;

            // Then
            Assert.AreEqual(expected.irmaItemID, actual.Select(i => i.IrmaItemId).FirstOrDefault(), "irmaItemID mismatch");
            Assert.AreEqual(expected.identifier, actual.Select(i => i.Identifier).FirstOrDefault(), "identifier mismatch");
            Assert.AreEqual(expected.defaultIdentifier, actual.Select(i => i.DefaultIdentifier).FirstOrDefault(), "defaultIdentifier mismatch");
            Assert.AreEqual(expected.brandName, actual.Select(i => i.BrandName).FirstOrDefault(), "brandName mismatch");
            Assert.AreEqual(expected.itemDescription, actual.Select(i => i.ItemDescription).FirstOrDefault(), "itemDescription mismatch");
            Assert.AreEqual(expected.posDescription, actual.Select(i => i.PosDescription).FirstOrDefault(), "posDescription mismatch");
            Assert.AreEqual(expected.packageUnit, actual.Select(i => i.PackageUnit).FirstOrDefault(), "packageUnit mismatch");
            Assert.AreEqual(expected.retailSize, actual.Select(i => i.RetailSize).FirstOrDefault(), "retailSize mismatch");
            Assert.AreEqual(expected.retailUom, actual.Select(i => i.RetailUom).FirstOrDefault(), "retailUom mismatch");
            Assert.AreEqual(expected.DeliverySystem, actual.Select(i => i.DeliverySystem).FirstOrDefault(), "DeliverySystem mismatch");
            Assert.AreEqual(expected.foodStamp, actual.Select(i => i.FoodStamp).FirstOrDefault(), "foodStamp mismatch");
            Assert.AreEqual(expected.posScaleTare, actual.Select(i => i.PosScaleTare).FirstOrDefault(), "posScaleTare mismatch");
            Assert.AreEqual(expected.taxClassID, actual.Select(i => i.TaxHierarchyClassId).FirstOrDefault(), "taxClassID mismatch");
            Assert.AreEqual(expected.merchandiseClassID, actual.Select(i => i.MerchandiseHierarchyClassId).FirstOrDefault(), "merchandiseClassID mismatch");
            Assert.AreEqual(expected.nationalClassID, actual.Select(i => i.NationalHierarchyClassId).FirstOrDefault(), "NationaClassID mismatch");
        }

        [TestMethod]
        public void Search_SearchViewModel_ReturnsPartialViewResultWithBrandComboList()
        {
            // Given
            var expected = item.brandName;

            // When
            var result = controller.Search(viewModel) as PartialViewResult;
            var model = result.Model as IrmaItemSearchViewModel;
            var actual = model.AllBrands;

            // Then
            Assert.IsTrue(actual.Any(brand => brand.HierarchyClassName == expected));
        }

        [TestMethod]
        public void Search_SearchViewModel_BrandComboListIncludesExistingAndNewBrands()
        {
            // Given
            var expected = item.brandName;

            // When
            var result = controller.Search(viewModel) as PartialViewResult;
            var model = result.Model as IrmaItemSearchViewModel;
            var actual = model.AllBrands;

            // Then
            Assert.IsTrue(actual.Any(brand => brand.HierarchyClassName == expected));
            Assert.IsTrue(actual.Any(brand => brand.HierarchyClassName == "Mock Brand Class 3"));
        }

        [TestMethod]
        public void Search_SearchViewModel_ReturnsPartialViewResultWithTaxComboList()
        {
            // Given
            var expected = item.taxClassID;

            // When
            var result = controller.Search(viewModel) as PartialViewResult;
            var model = result.Model as IrmaItemSearchViewModel;
            var actual = model.TaxHierarchyClasses;

            // Then
            Assert.IsTrue(actual.Any(tax => tax.HierarchyClassId == expected));
        }

        [TestMethod]
        public void Search_SearchViewModel_ReturnsPartialViewResultWithNationalComboList()
        {
            // Given
            var expected = item.nationalClassID;

            // When
            var result = controller.Search(viewModel) as PartialViewResult;
            var model = result.Model as IrmaItemSearchViewModel;
            var actual = model.NationalHierarchyClasses;

            // Then
            Assert.IsTrue(actual.Any(nat => nat.HierarchyClassId == expected));
        }

        [TestMethod]
        public void Search_SearchViewModel_ReturnsPartialViewResultWithMerchandiseComboList()
        {
            // Given
            var expected = item.merchandiseClassID;

            // When
            var result = controller.Search(viewModel) as PartialViewResult;
            var model = result.Model as IrmaItemSearchViewModel;
            var actual = model.MerchandiseHierarchyClasses;

            // Then
            Assert.IsTrue(actual.Any(merch => merch.HierarchyClassId == expected));
        }

        [TestMethod]
        public void Search_SearchViewModel_ReturnsMerchandiseComboListWithoutAffinitySubBricks()
        {
            // Given           
            HierarchyClassListModel hierarchyClassList = GetHierarchyClassLists();
            List<HierarchyClass> merch = new List<HierarchyClass>();
            merch = hierarchies.First(h => h.hierarchyID == Hierarchies.Merchandise).HierarchyClass.ToList();
            merch.Add(new TestHierarchyClassBuilder().WithHierarchyClassId(1).WithAffinityTrait("1").Build());
            merch.Add(new TestHierarchyClassBuilder().WithHierarchyClassId(2).WithAffinityTrait("0").Build());


            mockHierarchyClassQuery = new Mock<IQueryHandler<GetHierarchyLineageParameters,HierarchyClassListModel>>();
            mockHierarchyClassQuery.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(hierarchyClassList);

            mockIrmaQuery = new Mock<IQueryHandler<GetIrmaItemsParameters, List<IRMAItem>>>();
            mockIrmaQuery.Setup(iq => iq.Search(It.IsAny<GetIrmaItemsParameters>())).Returns(irmaItems);

            mockIrmaStoreProcQuery = new Mock<IQueryHandler<GetIrmaItemsParameters, List<IRMAItem>>>();
            mockIrmaStoreProcQuery.Setup(iq => iq.Search(It.IsAny<GetIrmaItemsParameters>())).Returns(irmaItems);

            controller = new IrmaItemController(this.logger.Object,
                this.mockHierarchyClassQuery.Object, this.mockUpdateCommand.Object, this.mockAddItemManager.Object, mockIrmaStoreProcQuery.Object, mockDeleteIrmaItemHandler.Object, mockExcelExporterService.Object);

            // When
            var result = controller.Search(viewModel) as PartialViewResult;
            var model = result.Model as IrmaItemSearchViewModel;
            var actual = model.MerchandiseHierarchyClasses;

            // Then
            var withoutAffinity = hierarchies[0].HierarchyClass.Where(hc => hc.HierarchyClassTrait.Any(hct => hct.traitID == Traits.Affinity));

            Assert.IsTrue(actual.Count == 1);
        }

        [TestMethod]
        public void LoadSelected_SuccessfulLoading_ReturnsJsonTrueResult()
        {
            // Given
            var expectedSuccess = true;
            var expectedMessage = "Successfully loaded all selected items.";

            // When
            var result = controller.LoadSelected(selectedRows) as JsonResult;
            var actualSuccess = result.GetDataProperty("Success");
            var actualMessage = result.GetDataProperty("Message");

            // Then
            Assert.AreEqual(expectedSuccess, actualSuccess);
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void LoadSelected_ErrorLoading_ReturnsFalseJsonResult()
        {
            // Given
            var expectedSuccess = false;
            var expectedMessage = String.Format("There was an error adding scan code {0}.  Error: {1}", selectedRows[1].Identifier, "Sequence Contains More Than One Element.");

            int count = 0;
            mockAddItemManager.Setup(c => c.Execute(It.IsAny<AddItemManager>())).Callback(() =>
                {
                    count++;
                    if (count == 1)
                    {
                        throw new CommandException(String.Format("There was an error adding scan code {0}.  Error: {1}", selectedRows[count].Identifier, "Sequence Contains More Than One Element."));
                    }
                });

            // When
            var result = controller.LoadSelected(selectedRows) as JsonResult;
            var actualSuccess = result.GetDataProperty("Success");
            var actualMessage = result.GetDataProperty("Message");

            // Then
            Assert.AreEqual(expectedSuccess, actualSuccess);
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void LoadSelected_SelectedIsNull_ShouldReturnJsonResultWithFalseSuccessAndFailureMessage()
        {
            //Given
            var expectedSuccess = false;
            var expectedMessage = "No items were selected to load into Icon.";

            //When
            var actual = controller.LoadSelected(null) as JsonResult;

            //Then
            Assert.AreEqual(expectedSuccess, actual.GetDataProperty("Success"));
            Assert.AreEqual(expectedMessage, actual.GetDataProperty("Message"));
        }

        [TestMethod]
        public void LoadSelected_SelectedIsEmpty_ShouldReturnJsonResultWithFalseSuccessAndFailureMessage()
        {
            //Given
            var expectedSuccess = false;
            var expectedMessage = "No items were selected to load into Icon.";

            //When
            var actual = controller.LoadSelected(new List<IrmaItemViewModel>()) as JsonResult;

            //Then
            Assert.AreEqual(expectedSuccess, actual.GetDataProperty("Success"));
            Assert.AreEqual(expectedMessage, actual.GetDataProperty("Message"));
        }

        [TestMethod]
        public void LoadSelected_TaxClassHasNoAbbreviationForTwoItems_ShouldReturnJsonResultWithFailureMessagesForEachFailedItem()
        {
            // Given
            bool expectedSuccess = false;
            string expectedMessage = String.Format("The tax class for Identifier {0} needs a tax abbreviation before being assigned to any item. " +
                "The tax class for Identifier {1} needs a tax abbreviation before being assigned to any item.", selectedRows[0].Identifier, selectedRows[1].Identifier);

            int count = -1;
            mockAddItemManager.Setup(c => c.Execute(It.IsAny<AddItemManager>())).Callback(() =>
                {
                    count++;
                    if (count <= 1)
                    {
                        throw new CommandException(String.Format("The tax class for Identifier {0} needs a tax abbreviation before being assigned to any item.", selectedRows[count].Identifier));
                    }
                });

            // When
            var result = controller.LoadSelected(selectedRows) as JsonResult;
            var actualSuccess = result.GetDataProperty("Success");
            var actualMessage = result.GetDataProperty("Message");

            // Then
            Assert.AreEqual(expectedSuccess, actualSuccess, "The expected Success JsonResult did not match the actual Success JsonResult");
            Assert.AreEqual(expectedMessage, actualMessage, "The expected Message JsonResult did not match the actual Message JsonResult");
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
        public void ValidateSelected_SelectedIsEmpty_ShouldReturnJsonResultWithFalseSuccessAndFailureMessage()
        {
            //Given
            var expectedSuccess = false;
            var expectedMessage = "No items were selected to validate.";

            //When
            var actual = controller.ValidateSelected(new List<IrmaItemViewModel>()) as JsonResult;

            //Then
            Assert.AreEqual(expectedSuccess, actual.GetDataProperty("Success"));
            Assert.AreEqual(expectedMessage, actual.GetDataProperty("Message"));
        }

        [TestMethod]
        public void ValidateSelected_SuccessValidation_ReturnsTrueJsonResult()
        {
            // Given
            var expectedSuccess = true;
            var expectedMessage = "Successfully validated all selected items.";

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
            var expectedMessage = String.Format("There was an error adding scan code {0}.  Error: {1}", selectedRows[1].Identifier, "Sequence Contains More Than One Element.");

            int count = 0;
            mockAddItemManager.Setup(c => c.Execute(It.IsAny<AddItemManager>())).Callback(() =>
            {
                count++;
                if (count == 1)
                {
                    throw new CommandException(String.Format("There was an error adding scan code {0}.  Error: {1}", selectedRows[count].Identifier, "Sequence Contains More Than One Element."));
                }
            });

            // When
            var result = controller.ValidateSelected(selectedRows) as JsonResult;
            var actualSuccess = result.GetDataProperty("Success");
            var actualMessage = result.GetDataProperty("Message");

            // Then
            Assert.AreEqual(expectedSuccess, actualSuccess);
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void ValidateSelected_TaxClassHasNoAbbreviation_ShouldReturnJsonResultWithFailureMessage()
        {
            // Given
            bool expectedSuccess = false;
            string expectedMessage = String.Format("The tax class for Identifier {0} needs a tax abbreviation before being assigned to any item. " +
                "The tax class for Identifier {1} needs a tax abbreviation before being assigned to any item.", selectedRows[0].Identifier, selectedRows[1].Identifier);

            int count = -1;
            mockAddItemManager.Setup(c => c.Execute(It.IsAny<AddItemManager>())).Callback(() =>
            {
                count++;
                if (count <= 1)
                {
                    throw new CommandException(String.Format("The tax class for Identifier {0} needs a tax abbreviation before being assigned to any item.", selectedRows[count].Identifier));
                }
            });

            // When
            var result = controller.ValidateSelected(selectedRows) as JsonResult;
            var actualSuccess = result.GetDataProperty("Success");
            var actualMessage = result.GetDataProperty("Message");

            // Then
            Assert.AreEqual(expectedSuccess, actualSuccess);
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void SaveChangesInGrid_HttpContextTransaction_ReturnsTrueSuccessJsonResult()
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
            mockContext.Setup(mc => mc.HttpContext).Returns(mockHttpContext.Object);
            controller.ControllerContext = mockContext.Object;
            
            // When
            var result = controller.SaveChangesInGrid() as JsonResult;

            // Then
            Assert.AreEqual(true, result.GetDataProperty("Success"));
        }

        [TestMethod]
        public void SaveChangesInGrid_HttpContextTransaction_ReturnsFalseSuccessAndErrorJsonResult()
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
            mockContext.Setup(mc => mc.HttpContext).Returns(mockHttpContext.Object);
            controller.ControllerContext = mockContext.Object;

            mockUpdateCommand.Setup(u => u.Execute(It.IsAny<UpdateIrmaItemCommand>())).Throws(new CommandException("Test Exception"));
            
            // When
            var result = controller.SaveChangesInGrid() as JsonResult;

            // Then
            Assert.AreEqual(false, result.GetDataProperty("Success"));
            Assert.AreEqual("Test Exception", result.GetDataProperty("Error"));
        }

        [TestMethod]
        public void DeleteSelected_SuccessfulDeleting_ReturnsJsonTrueResult()
        {
            // Given
            var expectedSuccess = true;
            var expectedMessage = "Successfully deleted all selected items.";

            // When
            var result = controller.DeleteSelected(selectedRows) as JsonResult;
            var actualSuccess = result.GetDataProperty("Success");
            var actualMessage = result.GetDataProperty("Message");

            // Then
            Assert.AreEqual(expectedSuccess, actualSuccess);
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void DeleteSelected_ErrorDeleting_ReturnsFalseJsonResult()
        {
            // Given
            var expectedSuccess = false;
            var expectedMessage = String.Format("There was an error deleting scan code {0}.  Error: {1}", selectedRows[1].Identifier, "Item does not exist.");

            int count = 0;
            mockDeleteIrmaItemHandler.Setup(c => c.Execute(It.IsAny<DeleteIrmaItemCommand>())).Callback(() =>
            {
                count++;
                if (count == 1)
                {
                    throw new CommandException(String.Format("There was an error deleting scan code {0}.  Error: {1}", selectedRows[count].Identifier, "Item does not exist."));
                }
            });

            // When
            var result = controller.DeleteSelected(selectedRows) as JsonResult;
            var actualSuccess = result.GetDataProperty("Success");
            var actualMessage = result.GetDataProperty("Message");

            // Then
            Assert.AreEqual(expectedSuccess, actualSuccess);
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod]
        public void DeleteSelected_NoRowsSelected_ReturnsFalseJsonResult()
        {
            // Given
            var expectedSuccess = false;
            var expectedMessage = String.Format("No items were selected to delete.");


            // When
            var result = controller.DeleteSelected(new List<IrmaItemViewModel> ()) as JsonResult;
            var actualSuccess = result.GetDataProperty("Success");
            var actualMessage = result.GetDataProperty("Message");

            // Then
            Assert.AreEqual(expectedSuccess, actualSuccess);
            Assert.AreEqual(expectedMessage, actualMessage);
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
                    .WithHierarchyLevel(4).WithHierarchyClassId(20).WithHierarchyClassParentId(null).Build()
            };

            return hierarchyClassList;
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
