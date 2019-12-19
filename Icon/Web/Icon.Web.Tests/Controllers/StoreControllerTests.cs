using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.Controllers;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Icon.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class StoreControllerTests
    {
        StoreController controller;
        private Mock<ILogger> mockLogger;
        private Mock<IQueryHandler<GetLocaleParameters, List<Locale>>> mockGetLocalesQuery;
        private Mock<IQueryHandler<GetLocalesByChainParameters, List<Locale>>> mockGetLocalesByChainQuery;
        private Mock<IGenericQuery> mockGenericQuery;
        private Mock<IManagerHandler<UpdateLocaleManager>> mockUpdateLocaleManager;
        private Mock<IManagerHandler<UpdateVenueManager>> mockUpdatVenueManager;
        private Mock<IManagerHandler<AddLocaleManager>> mockAddLocaleManager;
        private Mock<ControllerContext> mockContext;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger>();
            mockGetLocalesQuery = new Mock<IQueryHandler<GetLocaleParameters, List<Locale>>>();
            mockGetLocalesByChainQuery = new Mock<IQueryHandler<GetLocalesByChainParameters, List<Locale>>>();
            mockGenericQuery = new Mock<IGenericQuery>();
            mockUpdateLocaleManager = new Mock<IManagerHandler<UpdateLocaleManager>>();
            mockUpdatVenueManager = new Mock<IManagerHandler<UpdateVenueManager>>();
            mockAddLocaleManager = new Mock<IManagerHandler<AddLocaleManager>>();
            mockContext = new Mock<ControllerContext>();

            mockGenericQuery.Setup(q => q.GetAll<Country>()).Returns(new List<Country> { new Country { countryID = 1, countryCode = "CC", countryName = "Country" } });
            mockGenericQuery.Setup(q => q.GetAll<Territory>()).Returns(new List<Territory> { new Territory { countryID = 1, territoryID = 1, territoryCode = "TR", territoryName = "Territory" } });
            mockGenericQuery.Setup(q => q.GetAll<Timezone>()).Returns(new List<Timezone> { new Timezone { timezoneID = 1, timezoneCode = "TZ", timezoneName = "Time Zone" } });
            mockGenericQuery.Setup(q => q.GetAll<Agency>()).Returns(new List<Agency>());
            mockGenericQuery.Setup(q => q.GetAll<Currency>()).Returns(new List<Currency>());
            mockGenericQuery.Setup(r => r.GetAll<LocaleSubType>()).Returns(new List<LocaleSubType>
            {
                new LocaleSubType
                    {localeSubTypeID = 1, localeTypeID = 5, localSubTypeCode = "HS", localeSubTypeDesc = "Hospitality"}
            }); 

            controller = new StoreController(mockLogger.Object, mockGetLocalesQuery.Object, mockGetLocalesByChainQuery.Object, mockGenericQuery.Object, mockUpdateLocaleManager.Object, mockUpdatVenueManager.Object, mockAddLocaleManager.Object);
            controller.ControllerContext = mockContext.Object;
            mockContext.SetupGet(m => m.HttpContext.User.Identity.Name)
                .Returns("Test User");
        }

        [TestMethod]
        public void Index_InitialPageLoad_ViewResultShouldNotBeNull()
        {
            // Given.

            List<Locale> fakeLocale = TestHelpers.GetFakeLocale();
            mockGetLocalesByChainQuery.Setup(r => r.Search(It.IsAny<GetLocalesByChainParameters>()))
                .Returns(fakeLocale.ToList());
            mockGetLocalesQuery.Setup(r => r.Search(It.IsAny<GetLocaleParameters>())).Returns(fakeLocale.ToList());
            
            
            // When.
            ViewResult result = controller.Index() as ViewResult;

            // Then.
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateStore_GetWithValidParameters_ViewResultShouldNotBeNull()
        {
            // Given.
            mockGetLocalesQuery.Setup(q => q.Search(It.IsAny<GetLocaleParameters>())).Returns(TestHelpers.GetFakeLocaleList());

            // When.
            ViewResult result = controller.CreateStore(1, "test") as ViewResult;
            
            // Then.
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateStore_NewStoreLocaleTypeId_ShouldBeOneGreaterThanMetroLocaleTypeId()
        {
            // Given.
            mockGetLocalesQuery.Setup(q => q.Search(It.IsAny<GetLocaleParameters>())).Returns(TestHelpers.GetFakeLocaleList());

            // When.
            ViewResult result = controller.CreateStore(1, "test") as ViewResult;
            var model = result.Model as LocaleManagementViewModel;

            // Then.
            Assert.AreEqual(LocaleTypes.Store, model.LocaleTypeId);
        }

        [TestMethod]
        public void CreateStore_GetWithInvalidParameters_ShouldRedirectToIndex()
        {
            // Given.
            mockGetLocalesQuery.Setup(q => q.Search(It.IsAny<GetLocaleParameters>())).Returns(TestHelpers.GetFakeLocaleList());

            // When.
            var nullParentId = controller.CreateStore(null, "test") as RedirectToRouteResult;
            var zeroParentId = controller.CreateStore(0, "test") as RedirectToRouteResult;
            var nullParentName = controller.CreateStore(1, null) as RedirectToRouteResult;
            var emptyParentName = controller.CreateStore(1, String.Empty) as RedirectToRouteResult;

            // Then.
            string routeName = "Index";

            Assert.AreEqual(routeName, nullParentId.RouteValues["action"]);
            Assert.AreEqual(routeName, zeroParentId.RouteValues["action"]);
            Assert.AreEqual(routeName, nullParentName.RouteValues["action"]);
            Assert.AreEqual(routeName, emptyParentName.RouteValues["action"]);
        }

        [TestMethod]
        public void CreateStore_PostWithValidModelState_CommandShouldBeExecuted()
        {
            // Given.
            LocaleManagementViewModel viewModel = new LocaleManagementViewModel()
            {
                ParentLocaleId = 1,
                LocaleName = "test",
                OpenDate = DateTime.Now,
                OwnerOrgPartyId = 1,
                LocaleTypeId = 1
            };

            // When.
            ViewResult result = controller.CreateStore(viewModel) as ViewResult;

            // Then.
            mockAddLocaleManager.Verify(command => command.Execute(It.IsAny<AddLocaleManager>()), Times.Once);
        }

        [TestMethod]
        public void CreateStore_PostWithInvalidModelState_ShouldReturnViewModelObject()
        {
            // Given.
            controller.ModelState.AddModelError("test", "test");

            // When.
            ViewResult result = controller.CreateStore(new LocaleManagementViewModel()) as ViewResult;

            // Then.
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateStore_ExceptionOccurred_ViewDataShouldContainErrorMessage()
        {
            // Given.
            mockAddLocaleManager.Setup(c => c.Execute(It.IsAny<AddLocaleManager>()))
                .Throws(new CommandException("error"));

            LocaleManagementViewModel viewModel = new LocaleManagementViewModel()
            {
                ParentLocaleId = 1,
                LocaleName = "test",
                OpenDate = DateTime.Now,
                OwnerOrgPartyId = 1,
                LocaleTypeId = 1
            };

            // When.
            ViewResult result = controller.CreateStore(viewModel) as ViewResult;
            string message = result.ViewData["ErrorMessage"].ToString();

            // Then.
            Assert.AreEqual("error", message);
        }

    }
}
