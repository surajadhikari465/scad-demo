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
    public class ManufacturerControllerTests
    {
        private ManufacturerController controller;
        private IconContext context;
        private DbContextTransaction transaction;
        private Mock<ILogger> mockLogger;
        private Mock<IQueryHandler<GetManufacturersParameters, List<ManufacturerModel>>> mockGetManufacturersQuery;
        private Mock<IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass>> mockGetHierarchyClassQuery;
        private Mock<IManagerHandler<ManufacturerManager>> mockManufacturerManagerHandler;
        private Mock<IExcelExporterService> mockExcelExporterService;
        private Mock<IDonutCacheManager> mockCacheManager;
        private HierarchyClass testManufacturer;
        private string testManufacturerName;
        private string testZipCode;
        private string testArCustomerId;
        private IconWebAppSettings settings;
        private Mock<ControllerContext> mockControllerContext;
        private Mock<IIdentity> mockIdentity;
        private string userName;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            mockLogger = new Mock<ILogger>();
            mockGetManufacturersQuery = new Mock<IQueryHandler<GetManufacturersParameters, List<ManufacturerModel>>>();
            mockGetHierarchyClassQuery = new Mock<IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass>>();
            mockManufacturerManagerHandler = new Mock<IManagerHandler<ManufacturerManager>>();
            mockExcelExporterService = new Mock<IExcelExporterService>();
            settings = new IconWebAppSettings();
            mockControllerContext = new Mock<ControllerContext>();
            mockIdentity = new Mock<IIdentity>();
            mockCacheManager = new Mock<IDonutCacheManager>();
            testManufacturerName = "Test Manufacturer";
            testZipCode = "78704";
            testArCustomerId = "1234";

            controller = new ManufacturerController(
                mockLogger.Object,
                mockGetManufacturersQuery.Object,
                mockGetHierarchyClassQuery.Object,
                mockManufacturerManagerHandler.Object,
                mockExcelExporterService.Object,
                mockCacheManager.Object,
                new IconWebAppSettings()
                {
                    WriteAccessGroups = "none"
                });

            transaction = context.Database.BeginTransaction();

            userName = "Test User";
            mockIdentity.SetupGet(i => i.Name).Returns(userName);
            mockIdentity.SetupGet(i => i.IsAuthenticated).Returns(true);
            mockControllerContext.SetupGet(m => m.HttpContext.User).Returns(new GenericPrincipal(mockIdentity.Object, null));
            controller.ControllerContext = mockControllerContext.Object;
            mockControllerContext.Setup(c => c.HttpContext.User.IsInRole(It.Is<string>(s => s == userName))).Returns(true);

            // default settings for all tests
            settings.WriteAccessGroups = "";

            // setup for get manufacturers list
            mockGetManufacturersQuery.Setup(bq => bq.Search(It.IsAny<GetManufacturersParameters>())).Returns(new List<ManufacturerModel>());
        }

        private void StageTestManufacturer()
        {
            testManufacturer = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Manufacturer)
                .WithHierarchyLevel(1)
                .WithHierarchyParentClassId(null);

            context.HierarchyClass.Add(testManufacturer);
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
        public void ManufacturerControllerIndex_InitialPageLoad_DefaultViewShouldBeReturned()
        {
            // Given
            mockGetManufacturersQuery.Setup(bq => bq.Search(It.IsAny<GetManufacturersParameters>())).Returns(new List<ManufacturerModel>());

            // When
            var result = this.controller.Index() as ViewResult;

            // Then
            Assert.AreEqual(result.ViewName, String.Empty); // This will be empty if the view returned is not specified and returning the controller action

        }

        [TestMethod]
        public void ManufacturerControllerCreateGet_InitialPageLoad_DefaultViewShouldBeReturned()
        {
            // When.
            var result = controller.Create() as ViewResult;

            // Then.
            Assert.AreEqual(result.ViewName, String.Empty);
        }

        [TestMethod]
        public void ManufacturerControllerCreatePost_InvalidModelState_DefaultViewShouldBeReturnedWithPopulatedViewModel()
        {
            // Given.
            controller.ModelState.AddModelError("test", "test");

            var viewModel = new ManufacturerViewModel
            {
                ManufacturerName = testManufacturerName,
                ZipCode = testZipCode,
                ArCustomerId = testArCustomerId
            };

            // When.
            var result = controller.Create(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as ManufacturerViewModel;

            Assert.AreEqual(result.ViewName, String.Empty);
            Assert.IsNotNull(returnedViewModel);
            Assert.AreEqual(testManufacturerName, returnedViewModel.ManufacturerName);
        }

        [TestMethod]
        public void ManufacturerControllerCreatePost_ModelStateErrorForHierarchyClassNameKey_ModelStateErrorShouldBeIgnored()
        {
            // Given.
            controller.ModelState.AddModelError("HierarchyClassName", "test");

            var viewModel = new ManufacturerViewModel
            {
                ManufacturerName = testManufacturerName,
                ZipCode = testZipCode,
                ArCustomerId = testArCustomerId
            };

            // When.
            var result = controller.Create(viewModel) as ViewResult;

            // Then.
            mockManufacturerManagerHandler.Verify(t => t.Execute(It.IsAny<ManufacturerManager>()), Times.Once);
        }

        [TestMethod]
        public void ManufacturerControllerEditPost_ModelStateErrorForHierarchyClassNameKey_ModelStateErrorShouldBeIgnored()
        {
            // Given.
            controller.ModelState.AddModelError("HierarchyClassName", "test");

            var viewModel = new ManufacturerViewModel
            {
                ManufacturerName = testManufacturerName,
                ZipCode = testZipCode,
                ArCustomerId = testArCustomerId
            };

            // When.
            var result = controller.Edit(viewModel) as ViewResult;

            // Then.
            mockManufacturerManagerHandler.Verify(t => t.Execute(It.IsAny<ManufacturerManager>()), Times.Once);
        }

        [TestMethod]
        public void ManufacturerControllerCreatePost_OnlyManufacturerIsCreated_DefaultViewShouldBeReturnedWithSuccessMessage()
        {
            // Given.
            var viewModel = new ManufacturerViewModel
            {
                ManufacturerName = testManufacturerName,
                ZipCode = testZipCode,
                ArCustomerId = testArCustomerId
            };

            // When.
            var result = controller.Create(viewModel) as ViewResult;

            // Then.
            var viewData = result.ViewData;
            string expectedSuccessMessage = String.Format("Successfully added manufacturer {0}.", testManufacturerName);

            mockManufacturerManagerHandler.Verify(t => t.Execute(It.IsAny<ManufacturerManager>()), Times.Once);
            Assert.AreEqual(result.ViewName, String.Empty);
            Assert.AreEqual(expectedSuccessMessage, viewData["SuccessMessage"]);
        }

        [TestMethod]
        public void ManufacturerControllerEditPost_OnlyManufacturerIsUpdated_DefaultViewShouldBeReturnedWithSuccessMessage()
        {
            // Given.
            var viewModel = new ManufacturerViewModel
            {
                ManufacturerName = testManufacturerName,
                ZipCode = testZipCode,
                ArCustomerId = testArCustomerId,
                HierarchyClassId = 1
            };

            // When.
            var result = controller.Edit(viewModel) as ViewResult;

            // Then.
            var viewData = result.ViewData;
            string expectedSuccessMessage = $"Successfully updated manufacturer {testManufacturerName}.";

            mockManufacturerManagerHandler.Verify(t => t.Execute(It.IsAny<ManufacturerManager>()), Times.Once);
            Assert.AreEqual(result.ViewName, String.Empty);
            Assert.AreEqual(expectedSuccessMessage, viewData["SuccessMessage"]);
        }

        [TestMethod]
        public void All_Get_ShouldReturnAllManufacturers()
        {
            //Given
            mockGetManufacturersQuery.Setup(m => m.Search(It.IsAny<GetManufacturersParameters>()))
                .Returns(new List<ManufacturerModel>()
                {
                    new ManufacturerModel { HierarchyClassName = "Test 1"},
                    new ManufacturerModel { HierarchyClassName = "Test 2"},
                    new ManufacturerModel { HierarchyClassName = "Test 3"}
                });

            //When
            var result = controller.All() as ViewResult;
            var model = (result.Model as IEnumerable<ManufacturerViewModel>).ToList();

            //Then
            Assert.AreEqual("Test 1", model[0].ManufacturerName);
            Assert.AreEqual("Test 2", model[1].ManufacturerName);
            Assert.AreEqual("Test 3", model[2].ManufacturerName);
        }

        [TestMethod]
        public void ManufacturerControllerCreatePost_ManufacturerIsCreatedWithValidZipCode_ShouldBeMatchWithPattern()
        {
            // Given.
            var viewModel = new ManufacturerViewModel
            {
                ManufacturerName = testManufacturerName,
                ZipCode = "Ab 12-",
                ArCustomerId = testArCustomerId
            };
            string pattern = @"^[A-Za-z0-9\s-]+$";
            // When.
            var result = controller.Create(viewModel) as ViewResult;

            // Then.
            var viewData = result.ViewData;

            mockManufacturerManagerHandler.Verify(t => t.Execute(It.IsAny<ManufacturerManager>()), Times.Once);
            Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(viewModel.ZipCode, pattern));
        }

        [TestMethod]
        public void ManufacturerControllerCreatePost_ManufacturerIsCreatedWithInValidZipCode_ShouldBeMatchWithPattern()
        {
            // Given.
            var viewModel = new ManufacturerViewModel
            {
                ManufacturerName = testManufacturerName,
                ZipCode = "###$$%5%",
                ArCustomerId = testArCustomerId
            };
            string pattern = @"^[A-Za-z0-9\s-]+$";
            // When.
            var result = controller.Create(viewModel) as ViewResult;

            // Then.
            var viewData = result.ViewData;

            mockManufacturerManagerHandler.Verify(t => t.Execute(It.IsAny<ManufacturerManager>()), Times.Once);
            Assert.IsFalse(System.Text.RegularExpressions.Regex.IsMatch(viewModel.ZipCode, pattern));
        }
    }
}