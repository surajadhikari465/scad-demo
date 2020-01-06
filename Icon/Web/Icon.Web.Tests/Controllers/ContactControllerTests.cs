using Icon.Common.DataAccess;
using Icon.Logging;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Controllers;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web.Mvc;
using Icon.Common.Models;
using Icon.Web.Mvc.Exporters;
using Icon.Framework;
using System.Linq;


namespace Icon.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class ContactControllerTests
    {
        private ILogger logger;
        private Mock<IIdentity> mockIdentity;
        private ContactController controller;
        private Mock<ControllerContext> mockControllerContext;
        private Mock<IQueryHandler<GetContactsParameters, List<ContactModel>>> mockGetContactsQuery;
        private Mock<IQueryHandler<GetContactTypesParameters, List<ContactTypeModel>>>getContactTypesQuery;
        private Mock<IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass>> mockGetHierarchyClassQuery;
        private IManagerHandler<AddUpdateContactManager> contactManagerHandler;
        private IconWebAppSettings settings;
        private IDonutCacheManager cacheManager;

        [TestInitialize]
        public void Initialize()
        {
            this.logger = new Mock<ILogger>().Object;
            this.settings = new IconWebAppSettings();
            this.mockIdentity = new Mock<IIdentity>();
            this.cacheManager = new Mock<IDonutCacheManager>().Object;
            this.mockControllerContext = new Mock<ControllerContext>();
            this.mockGetContactsQuery = new Mock<IQueryHandler<GetContactsParameters, List<ContactModel>>>();
            this.getContactTypesQuery = new Mock<IQueryHandler<GetContactTypesParameters, List<ContactTypeModel>>>();
            this.mockGetHierarchyClassQuery = new Mock<IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass>>();
            this.contactManagerHandler = new Mock<IManagerHandler<AddUpdateContactManager>>().Object;

            this.controller = new ContactController(
                logger: this.logger,
                getContactsQuery: this.mockGetContactsQuery.Object,
                getContactTypesQuery: this.getContactTypesQuery.Object,
                getHierarchyClassQuery: this.mockGetHierarchyClassQuery.Object,
                contactManagerHandler: this.contactManagerHandler,
                settings: this.settings,
                cacheManager: cacheManager);

            this.mockIdentity.SetupGet(i => i.Name).Returns("Test User");
            this.mockIdentity.SetupGet(i => i.IsAuthenticated).Returns(true);
            this.mockControllerContext.SetupGet(m => m.HttpContext.User).Returns(new GenericPrincipal(mockIdentity.Object, null));
            this.controller.ControllerContext = mockControllerContext.Object;

            this.settings.WriteAccessGroups = "Test User";
            this.getContactTypesQuery.Setup(x => x.Search(It.IsAny<GetContactTypesParameters>()))
                .Returns(new List<ContactTypeModel>(){ new ContactTypeModel(){ ContactTypeId = 1, ContactTypeName = "AR/AP"}});
            this.mockGetContactsQuery.Setup(x => x.Search(It.IsAny<GetContactsParameters>()))
                .Returns(new List<ContactModel>(){new ContactModel(){ ContactId = 1, ContactName = "Test Contact", Email = "test@gmail.com", ContactTypeId = 1, HierarchyClassId = 12345 }});
            this.mockGetHierarchyClassQuery.Setup(x => x.Search(It.IsAny<GetHierarchyClassByIdParameters>()))
                .Returns(new HierarchyClass(){ hierarchyClassID = 12345, hierarchyClassName = "Test", hierarchyID = Hierarchies.Brands });
        }

        [TestMethod]
        public void Index_InitialPageLoad_ShouldReturnView()
        {
            //When
            var result = this.controller.Index() as ViewResult;

            //Then
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Contact_ContactViewDisabled_ShouldRedirect()
        {
            //Given
            this.settings.IsContactViewEnabled = false;
            //When
            var result = controller.Contact(1);
            //Then
            Assert.IsNotNull(result);
            Assert.IsTrue(result is RedirectToRouteResult);
        }

        [TestMethod]
        public void Contact_ContactViewEnabled_ShouldReturnView()
        {
            //Given
            this.settings.IsContactViewEnabled = true;
            //When
            var result = controller.Contact(1);
            //Then
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Contact_InvalidHierarchy_ShouldRedirect()
        {
            //Given
            this.settings.IsContactViewEnabled = true;
            this.mockGetHierarchyClassQuery.Setup(x => x.Search(It.IsAny<GetHierarchyClassByIdParameters>()))
                .Returns(new HierarchyClass(){ hierarchyClassID = 12345, hierarchyClassName = "Test", hierarchyID = Hierarchies.Tax });
            //When
            var result = controller.Contact(1);
            //Then
            Assert.IsNotNull(result);
            Assert.IsTrue(result is RedirectToRouteResult);
        }

        [TestMethod]
        public void Contact_MismatchedHierarchyAndContact_ShouldRedirect()
        {
            //Given
            this.settings.IsContactViewEnabled = true;
            this.mockGetContactsQuery.Setup(x => x.Search(It.IsAny<GetContactsParameters>()))
                .Returns(new List<ContactModel>() { new ContactModel() { ContactId = 1, ContactName = "Test Contact", Email = "test@gmail.com", ContactTypeId = 1, HierarchyClassId = 100 } });
            //When
            var result = controller.Manage(12345, 2);
            //Then
            Assert.IsNotNull(result);
            Assert.IsTrue(result is RedirectToRouteResult);
        }

        [TestMethod]
        public void Contact_ContactAll_ShouldReturnOneContact()
        {
            //Given
            this.settings.IsContactViewEnabled = true;
            //When
            var result = controller.ContactAll(1) as ViewResult;
            //Then
            Assert.IsNotNull(result);
            Assert.AreEqual(1, (result.Model as IEnumerable<ContactViewModel>).Count());
        }
    }
}