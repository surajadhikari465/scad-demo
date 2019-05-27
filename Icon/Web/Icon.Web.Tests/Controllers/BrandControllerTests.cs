using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Testing.Builders;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Controllers;
using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;

namespace Icon.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class BrandControllerTests
    {
        private BrandController controller;
        private IconContext context;
        private DbContextTransaction transaction;
        private Mock<ILogger> mockLogger;
        private Mock<IQueryHandler<GetBrandsParameters, List<BrandModel>>> mockGetBrandsQuery;
        private Mock<IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass>> mockGetHierarchyClassQuery;
        private Mock<IManagerHandler<BrandManager>> mockBrandManagerHandler;
        private Mock<IExcelExporterService> mockExcelExporterService;
        private HierarchyClass testBrand;
        private string testBrandName;
        private string testBrandAbbreviation;
        private IconWebAppSettings settings;
        private Mock<ControllerContext> mockControllerContext;
        private Mock<IIdentity> mockIdentity;
        private string userName;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            mockLogger = new Mock<ILogger>();
            mockGetBrandsQuery = new Mock<IQueryHandler<GetBrandsParameters, List<BrandModel>>>();
            mockGetHierarchyClassQuery = new Mock<IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass>>();
            mockBrandManagerHandler = new Mock<IManagerHandler<BrandManager>>();
            mockExcelExporterService = new Mock<IExcelExporterService>();
            settings = new IconWebAppSettings();
            mockControllerContext = new Mock<ControllerContext>();
            mockIdentity = new Mock<IIdentity>();

            testBrandName = "Test Brand";
            testBrandAbbreviation = "TSTBRND";

            controller = new BrandController(
                mockLogger.Object,
                mockGetBrandsQuery.Object,
                mockGetHierarchyClassQuery.Object,
                mockBrandManagerHandler.Object,
                mockExcelExporterService.Object,
                settings);

            transaction = context.Database.BeginTransaction();

            userName = "Test User";
            mockIdentity.SetupGet(i => i.Name).Returns(userName);
            mockIdentity.SetupGet(i => i.IsAuthenticated).Returns(true);
            mockControllerContext.SetupGet(m => m.HttpContext.User).Returns(new GenericPrincipal(mockIdentity.Object, null));
            controller.ControllerContext = mockControllerContext.Object;
            mockControllerContext.Setup(c => c.HttpContext.User.IsInRole(It.Is<string>(s => s == userName))).Returns(true);

            // default settings for all tests
            settings.IconInterfaceEnabled = false;
            settings.WriteAccessGroups = "";
            settings.TraitWriteAccessGroups = "";

            // setup for get brands list
            mockGetBrandsQuery.Setup(bq => bq.Search(It.IsAny<GetBrandsParameters>())).Returns(new List<BrandModel>());
        }

        private void StageTestBrand()
        {
            testBrand = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Brands)
                .WithHierarchyLevel(1)
                .WithHierarchyParentClassId(null)
                .WithBrandAbbreviation(testBrandAbbreviation);

            context.HierarchyClass.Add(testBrand);
            context.SaveChanges();
        }

        [TestMethod]
        public void Index_InitialPageLoad_ShouldReturnView()
        {
            // When
            var result = this.controller.Index() as ViewResult;

            // Then
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void BrandControllerIndex_InitialPageLoad_DefaultViewShouldBeReturned()
        {
            // Given
            mockGetBrandsQuery.Setup(bq => bq.Search(It.IsAny<GetBrandsParameters>())).Returns(new List<BrandModel>());

            // When
            var result = this.controller.Index() as ViewResult;

            // Then
            Assert.AreEqual(result.ViewName, String.Empty); // This will be empty if the view returned is not specified and returning the controller action

        }

        [TestMethod]
        public void BrandControllerCreateGet_InitialPageLoad_DefaultViewShouldBeReturned()
        {
            // When.
            var result = controller.Create() as ViewResult;

            // Then.
            Assert.AreEqual(result.ViewName, String.Empty);
        }

        [TestMethod]
        public void BrandControllerCreatePost_InvalidModelState_DefaultViewShouldBeReturnedWithPopulatedViewModel()
        {
            // Given.
            controller.ModelState.AddModelError("test", "test");

            var viewModel = new BrandViewModel
            {
                BrandName = testBrandName,
                BrandAbbreviation = testBrandAbbreviation
            };

            // When.
            var result = controller.Create(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as BrandViewModel;

            Assert.AreEqual(result.ViewName, String.Empty);
            Assert.IsNotNull(returnedViewModel);
            Assert.AreEqual(testBrandName, returnedViewModel.BrandName);
            Assert.AreEqual(testBrandAbbreviation, returnedViewModel.BrandAbbreviation);
        }

        [TestMethod]
        public void BrandControllerCreatePost_ModelStateErrorForHierarchyClassNameKey_ModelStateErrorShouldBeIgnored()
        {
            // Given.
            controller.ModelState.AddModelError("HierarchyClassName", "test");

            var viewModel = new BrandViewModel
            {
                BrandName = testBrandName,
                BrandAbbreviation = testBrandAbbreviation
            };

            // When.
            var result = controller.Create(viewModel) as ViewResult;

            // Then.
            mockBrandManagerHandler.Verify(t => t.Execute(It.IsAny<BrandManager>()), Times.Once);
        }

        [TestMethod]
        public void BrandControllerCreatePost_OnlyBrandIsCreated_DefaultViewShouldBeReturnedWithSuccessMessage()
        {
            // Given.
            var viewModel = new BrandViewModel
            {
                BrandName = testBrandName,
                BrandAbbreviation = null
            };

            // When.
            var result = controller.Create(viewModel) as ViewResult;

            // Then.
            var viewData = result.ViewData;
            string expectedSuccessMessage = String.Format("Successfully added brand {0}.", testBrandName);

            mockBrandManagerHandler.Verify(t => t.Execute(It.IsAny<BrandManager>()), Times.Once);
            Assert.AreEqual(result.ViewName, String.Empty);
            Assert.AreEqual(expectedSuccessMessage, viewData["SuccessMessage"]);
        }

        [TestMethod]
        public void BrandControllerCreatePost_BrandAndAbbreviationAreCreated_DefaultViewShouldBeReturnedWithSuccessMessage()
        {
            // Given.
            var viewModel = new BrandViewModel
            {
                BrandName = testBrandName,
                BrandAbbreviation = testBrandAbbreviation
            };

            // When.
            var result = controller.Create(viewModel) as ViewResult;

            // Then.
            var viewData = result.ViewData;
            string expectedSuccessMessage = String.Format("Successfully added brand {0} with abbreviation {1}.", testBrandName, testBrandAbbreviation);

            mockBrandManagerHandler.Verify(t => t.Execute(It.IsAny<BrandManager>()), Times.Once);
            Assert.AreEqual(result.ViewName, String.Empty);
            Assert.AreEqual(expectedSuccessMessage, viewData["SuccessMessage"]);
        }

        [TestMethod]
        public void BrandControllerCreatePost_BrandUpdateIsNotSuccessful_DefaultViewShouldBeReturnedWithErrorMessage()
        {
            // Given.
            string errorMessage = "An unexpected error occurred.";
            mockBrandManagerHandler.Setup(t => t.Execute(It.IsAny<BrandManager>())).Throws(new CommandException(errorMessage));

            var viewModel = new BrandViewModel
            {
                BrandName = testBrandName,
                BrandAbbreviation = testBrandAbbreviation
            };

            // When.
            var result = controller.Create(viewModel) as ViewResult;

            // Then.
            var viewData = result.ViewData;

            mockBrandManagerHandler.Verify(t => t.Execute(It.IsAny<BrandManager>()), Times.Once);
            mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
            Assert.AreEqual(result.ViewName, String.Empty);
            Assert.AreEqual(errorMessage, viewData["ErrorMessage"]);
        }

        [TestMethod]
        public void BrandControllerEditGet_InitialPageLoad_BrandInformationShouldBeDisplayed()
        {
            // Given.
            testBrand = new HierarchyClass(){ hierarchyClassName = testBrandName, hierarchyID = Hierarchies.Brands, hierarchyLevel = HierarchyLevels.Parent, hierarchyParentClassID = null };
            testBrand.HierarchyClassTrait.Add(new HierarchyClassTrait { traitID = Traits.BrandAbbreviation, traitValue = testBrandAbbreviation });

            testBrand.HierarchyClassTrait.Single().Trait = new Trait { traitCode = TraitCodes.BrandAbbreviation };

            mockGetHierarchyClassQuery.Setup(q => q.Search(It.IsAny<GetHierarchyClassByIdParameters>())).Returns(testBrand);

            // When.
            var result = controller.Edit(testBrand.hierarchyClassID) as ViewResult;

            // Then.
            var viewModel = result.Model as BrandViewModel;

            Assert.AreEqual(result.ViewName, String.Empty);
            Assert.AreEqual(viewModel.HierarchyClassId, testBrand.hierarchyClassID);
            Assert.AreEqual(viewModel.BrandName, testBrand.hierarchyClassName);
            Assert.AreEqual(viewModel.BrandAbbreviation, testBrand.HierarchyClassTrait.Single(hct => hct.traitID == Traits.BrandAbbreviation).traitValue);
        }

        [TestMethod]
        public void BrandControllerEditGet_IconInterfaceNotEnabledAndNoTraitWriteAccess_UserAccessSetToNone()
        {
            // Given.
            settings.IconInterfaceEnabled = false;
            settings.WriteAccessGroups = userName;
            settings.TraitWriteAccessGroups = "";
            testBrand = new HierarchyClass() { hierarchyClassName = testBrandName, hierarchyID = Hierarchies.Brands, hierarchyLevel = HierarchyLevels.Parent, hierarchyParentClassID = null };
            testBrand.HierarchyClassTrait.Add(new HierarchyClassTrait { traitID = Traits.BrandAbbreviation, traitValue = testBrandAbbreviation });
            testBrand.HierarchyClassTrait.Single().Trait = new Trait { traitCode = TraitCodes.BrandAbbreviation };
            mockGetBrandsQuery.Setup(q => q.Search(It.IsAny<GetBrandsParameters>())).Returns(new List<BrandModel>());
            mockGetHierarchyClassQuery.Setup(q => q.Search(It.IsAny<GetHierarchyClassByIdParameters>())).Returns(testBrand);

            // When.
            var result = controller.Edit(testBrand.hierarchyClassID) as ViewResult;

            // Then.
            var viewModel = result.Model as BrandViewModel;
            Assert.AreEqual(viewModel.UserWriteAccess, Enums.WriteAccess.None);
        }

        [TestMethod]
        public void BrandControllerEditGet_IconInterfaceEnabledAndUserInWriteAccess_UserAccessSetToFull()
        {
            // Given.
            settings.IconInterfaceEnabled = true;
            settings.WriteAccessGroups = userName;
            settings.TraitWriteAccessGroups = "";
            testBrand = new HierarchyClass() { hierarchyClassName = testBrandName, hierarchyID = Hierarchies.Brands, hierarchyLevel = HierarchyLevels.Parent, hierarchyParentClassID = null };
            testBrand.HierarchyClassTrait.Add(new HierarchyClassTrait { traitID = Traits.BrandAbbreviation, traitValue = testBrandAbbreviation });
            testBrand.HierarchyClassTrait.Single().Trait = new Trait { traitCode = TraitCodes.BrandAbbreviation };
            mockGetBrandsQuery.Setup(q => q.Search(It.IsAny<GetBrandsParameters>())).Returns(new List<BrandModel>());
            mockGetHierarchyClassQuery.Setup(q => q.Search(It.IsAny<GetHierarchyClassByIdParameters>())).Returns(testBrand);

            // When.
            var result = controller.Edit(testBrand.hierarchyClassID) as ViewResult;

            // Then.
            var viewModel = result.Model as BrandViewModel;
            Assert.AreEqual(viewModel.UserWriteAccess, Enums.WriteAccess.Full);
        }

        [TestMethod]
        public void BrandControllerEditGet_IconInterfaceEnabledAndUserNotInWriteAccessButUserInTraitWriteAccess_UserAccessSetToTraits()
        {
            // Given.
            settings.IconInterfaceEnabled = true;
            settings.WriteAccessGroups = "";
            settings.TraitWriteAccessGroups = userName;
            testBrand = new HierarchyClass() { hierarchyClassName = testBrandName, hierarchyID = Hierarchies.Brands, hierarchyLevel = HierarchyLevels.Parent, hierarchyParentClassID = null };
            testBrand.HierarchyClassTrait.Add(new HierarchyClassTrait { traitID = Traits.BrandAbbreviation, traitValue = testBrandAbbreviation });
            testBrand.HierarchyClassTrait.Single().Trait = new Trait { traitCode = TraitCodes.BrandAbbreviation };
            mockGetBrandsQuery.Setup(q => q.Search(It.IsAny<GetBrandsParameters>())).Returns(new List<BrandModel>());
            mockGetHierarchyClassQuery.Setup(q => q.Search(It.IsAny<GetHierarchyClassByIdParameters>())).Returns(testBrand);

            // When.
            var result = controller.Edit(testBrand.hierarchyClassID) as ViewResult;

            // Then.
            var viewModel = result.Model as BrandViewModel;
            Assert.AreEqual(viewModel.UserWriteAccess, Enums.WriteAccess.Traits);
        }

        [TestMethod]
        public void BrandControllerEditGet_IconInterfaceNotEnabledButUserInTraitWriteAccess_UserAccessSetToTraits()
        {
            // Given.
            settings.IconInterfaceEnabled = false;
            settings.WriteAccessGroups = "";
            settings.TraitWriteAccessGroups = userName;
            testBrand = new HierarchyClass() { hierarchyClassName = testBrandName, hierarchyID = Hierarchies.Brands, hierarchyLevel = HierarchyLevels.Parent, hierarchyParentClassID = null };
            testBrand.HierarchyClassTrait.Add(new HierarchyClassTrait { traitID = Traits.BrandAbbreviation, traitValue = testBrandAbbreviation });
            testBrand.HierarchyClassTrait.Single().Trait = new Trait { traitCode = TraitCodes.BrandAbbreviation };
            mockGetBrandsQuery.Setup(q => q.Search(It.IsAny<GetBrandsParameters>())).Returns(new List<BrandModel>());
            mockGetHierarchyClassQuery.Setup(q => q.Search(It.IsAny<GetHierarchyClassByIdParameters>())).Returns(testBrand);

            // When.
            var result = controller.Edit(testBrand.hierarchyClassID) as ViewResult;

            // Then.
            var viewModel = result.Model as BrandViewModel;
            Assert.AreEqual(viewModel.UserWriteAccess, Enums.WriteAccess.Traits);
        }

        [TestMethod]
        public void BrandControllerEditGet_IconInterfaceNotEnabledAndUserNotInTraitWriteAccess_UserAccessSetToNone()
        {
            // Given.
            settings.IconInterfaceEnabled = false;
            settings.WriteAccessGroups = "";
            settings.TraitWriteAccessGroups = "";
            testBrand = new HierarchyClass() { hierarchyClassName = testBrandName, hierarchyID = Hierarchies.Brands, hierarchyLevel = HierarchyLevels.Parent, hierarchyParentClassID = null };
            testBrand.HierarchyClassTrait.Add(new HierarchyClassTrait { traitID = Traits.BrandAbbreviation, traitValue = testBrandAbbreviation });
            testBrand.HierarchyClassTrait.Single().Trait = new Trait { traitCode = TraitCodes.BrandAbbreviation };
            mockGetBrandsQuery.Setup(q => q.Search(It.IsAny<GetBrandsParameters>())).Returns(new List<BrandModel>());
            mockGetHierarchyClassQuery.Setup(q => q.Search(It.IsAny<GetHierarchyClassByIdParameters>())).Returns(testBrand);
            

            // When.
            var result = controller.Edit(testBrand.hierarchyClassID) as ViewResult;

            // Then.
            var viewModel = result.Model as BrandViewModel;
            Assert.AreEqual(viewModel.UserWriteAccess, Enums.WriteAccess.None);
        }

        [TestMethod]
        public void BrandControllerEditPost_InvalidModelState_DefaultViewShouldBeReturnedWithPopulatedViewModel()
        {
            // Given.
            int testHierarchyClassId = 1;

            controller.ModelState.AddModelError("test", "test");

            var viewModel = new BrandViewModel
            {
                BrandName = testBrandName,
                BrandAbbreviation = testBrandAbbreviation,
                HierarchyClassId = testHierarchyClassId
            };

            // When.
            var result = controller.Edit(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as BrandViewModel;

            Assert.AreEqual(result.ViewName, String.Empty);
            Assert.AreEqual(returnedViewModel.HierarchyClassId, testHierarchyClassId);
            Assert.AreEqual(returnedViewModel.BrandName, testBrandName);
            Assert.AreEqual(returnedViewModel.BrandAbbreviation, testBrandAbbreviation);
        }

        [TestMethod]
        public void BrandControllerEditPost_ModelStateErrorForHierarchyClassNameKey_ModelStateErrorShouldBeIgnored()
        {
            // Given.
            controller.ModelState.AddModelError("HierarchyClassName", "test");

            var viewModel = new BrandViewModel
            {
                BrandName = testBrandName,
                BrandAbbreviation = testBrandAbbreviation
            };

            // When.
            var result = controller.Edit(viewModel) as ViewResult;

            // Then.
            mockBrandManagerHandler.Verify(t => t.Execute(It.IsAny<BrandManager>()), Times.Once);
        }

        [TestMethod]
        public void BrandControllerEditPost_BrandInformationIsUpdatedSuccessfully_DefaultViewShouldBeReturnedWithSuccessMessage()
        {
            // Given.
            string successMessage = "Brand update was successful.";

            var viewModel = new BrandViewModel
            {
                BrandName = testBrandName,
                BrandAbbreviation = testBrandAbbreviation
            };

            // When.
            var result = controller.Edit(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as BrandViewModel;
            var viewData = result.ViewData;

            mockBrandManagerHandler.Verify(t => t.Execute(It.IsAny<BrandManager>()), Times.Once);
            Assert.AreEqual(result.ViewName, String.Empty);
            Assert.AreEqual(successMessage, viewData["SuccessMessage"]);
        }

        [TestMethod]
        public void BrandControllerEditPost_BrandInformationIsNotUpdatedSuccessfully_DefaultViewShouldBeReturnedWithErrorMessage()
        {
            // Given.
            string errorMessage = "An unexpected error occurred.";
            mockBrandManagerHandler.Setup(t => t.Execute(It.IsAny<BrandManager>())).Throws(new CommandException(errorMessage));

            var viewModel = new BrandViewModel
            {
                BrandName = testBrandName,
                BrandAbbreviation = testBrandAbbreviation
            };

            // When.
            var result = controller.Edit(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as BrandViewModel;
            var viewData = result.ViewData;

            mockBrandManagerHandler.Verify(t => t.Execute(It.IsAny<BrandManager>()), Times.Once);
            mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
            Assert.AreEqual(result.ViewName, String.Empty);
            Assert.AreEqual(errorMessage, viewData["ErrorMessage"]);
        }

        [TestMethod]
        public void All_Get_ShouldReturnAllBrands()
        {
            //Given
            mockGetBrandsQuery.Setup(m => m.Search(It.IsAny<GetBrandsParameters>()))
                .Returns(new List<BrandModel>()
                {
                    new BrandModel { HierarchyClassName = "Test 1"},
                    new BrandModel { HierarchyClassName = "Test 2"},
                    new BrandModel { HierarchyClassName = "Test 3"}
                });

            //When
            var result = controller.All() as ViewResult;
            var model = (result.Model as IEnumerable<BrandViewModel>).ToList();

            //Then
            Assert.AreEqual("Test 1", model[0].BrandName);
            Assert.AreEqual("Test 2", model[1].BrandName);
            Assert.AreEqual("Test 3", model[2].BrandName);
        }
    }
}