using Icon.Common.DataAccess;
using Icon.Common.Models;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Controllers;
using Icon.Web.Mvc.Models.RefreshData;
using Icon.Web.Mvc.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data.Entity;
using System.Web.Mvc;

namespace Icon.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class RefreshDataControllerTests
    {
        private RefreshDataController controller;
        private IconContext context;
        private DbContextTransaction transaction;
        private Mock<ILogger> mockLogger;
        private Mock<ICommandHandler<PublishItemUpdatesCommand>> mockProductMessagesCommandHandler;
        private Mock<ICommandHandler<RefreshLocalesCommand>> mockLocaleMessagesCommandHandler;
        private Mock<ICommandHandler<RefreshHierarchiesCommand>> mockHierarchiesCommandHandler;
        private Mock<ICommandHandler<RefreshAttributesCommand>> mockAttributesCommandHandler;
        private Mock<IQueryHandler<GetAttributeByAttributeIdParameters, AttributeModel>> mockGetAttributeByAttributeIdQuery;
        private IconWebAppSettings settings;

        [TestInitialize]
        public void Initialize()
        {
            this.context = new IconContext();
            this.mockLogger = new Mock<ILogger>();
            this.mockProductMessagesCommandHandler = new Mock<ICommandHandler<PublishItemUpdatesCommand>>();
            this.mockLocaleMessagesCommandHandler = new Mock<ICommandHandler<RefreshLocalesCommand>>();
            this.mockHierarchiesCommandHandler = new Mock<ICommandHandler<RefreshHierarchiesCommand>>();
            this.mockAttributesCommandHandler = new Mock<ICommandHandler<RefreshAttributesCommand>>();
            this.mockGetAttributeByAttributeIdQuery = new Mock<IQueryHandler<GetAttributeByAttributeIdParameters, AttributeModel>>();
            settings = new IconWebAppSettings();
            this.mockGetAttributeByAttributeIdQuery.Setup(x => x.Search(It.IsAny<GetAttributeByAttributeIdParameters>())).Returns(new AttributeModel());

            this.controller = new RefreshDataController(
                mockProductMessagesCommandHandler.Object,
                mockLocaleMessagesCommandHandler.Object,
                mockHierarchiesCommandHandler.Object,
                mockAttributesCommandHandler.Object,
                mockGetAttributeByAttributeIdQuery.Object,
                                mockLogger.Object,
                new IconWebAppSettings()
                {
                    IsManufacturerHierarchyMessage = true
                });

            this.transaction = context.Database.BeginTransaction();
        }

        [TestMethod]
        public void Items_GetWithValidParameters_ViewResultShouldNotBeNull()
        {
            // Given.
            this.mockProductMessagesCommandHandler.Setup(x => x.Execute(It.IsAny<PublishItemUpdatesCommand>()));

            // When.
            ActionResult result = this.controller.Items(new Mvc.Models.RefreshData.RefreshDataViewModel()
            {
                Identifiers = @"123"
            })
            as ActionResult;

            // Then.
            Assert.IsNotNull(result);
            this.mockProductMessagesCommandHandler.Verify(x => x.Execute(It.IsAny<PublishItemUpdatesCommand>()));
        }


        [TestMethod]
        public void Items_GetWithEmptyParameters_ViewResultShouldNotBeNull()
        {
            // Given.
            this.mockProductMessagesCommandHandler.Setup(x => x.Execute(It.IsAny<PublishItemUpdatesCommand>()));

            // When.
            ActionResult result = this.controller.Items(new Mvc.Models.RefreshData.RefreshDataViewModel()
            {
                Identifiers = @""
            })
            as ActionResult;

            // Then.
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Items_GetWithNullParameters_ViewResultShouldNotBeNull()
        {
            // Given.
            this.mockProductMessagesCommandHandler.Setup(x => x.Execute(It.IsAny<PublishItemUpdatesCommand>()));

            // When.
            ActionResult result = this.controller.Items(new Mvc.Models.RefreshData.RefreshDataViewModel()
            {
                Identifiers = null
            })
            as ActionResult;

            // Then.
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Attributes_GetWithValidParameters_ViewResultShouldNotBeNull()
        {
            // Given.
            this.mockAttributesCommandHandler.Setup(x => x.Execute(It.IsAny<RefreshAttributesCommand>()));

            // When.
            ActionResult result = this.controller.Attributes(new Mvc.Models.RefreshData.RefreshAttributesViewModel()
            {
                AttributeIds = @"123"
            })
            as ActionResult;

            // Then.
            Assert.IsNotNull(result);
            this.mockAttributesCommandHandler.Verify(x => x.Execute(It.IsAny<RefreshAttributesCommand>()));
        }


        [TestMethod]
        public void Attributes_GetWithEmptyParameters_ViewResultShouldNotBeNull()
        {
            // Given.
            this.mockAttributesCommandHandler.Setup(x => x.Execute(It.IsAny<RefreshAttributesCommand>()));

            // When.
            ActionResult result = this.controller.Attributes(new Mvc.Models.RefreshData.RefreshAttributesViewModel
            {
                AttributeIds = @""
            })
            as ActionResult;

            // Then.
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Attributes_GetWithNullParameters_ViewResultShouldNotBeNull()
        {
            // Given.
            this.mockAttributesCommandHandler.Setup(x => x.Execute(It.IsAny<RefreshAttributesCommand>()));

            // When.
            ActionResult result = this.controller.Attributes(new Mvc.Models.RefreshData.RefreshAttributesViewModel
            {
                AttributeIds = null
            })
            as ActionResult;

            // Then.
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Hierarchy_Get_ShouldLoadPage()
        {
            // Given.

            // When.
            ViewResult result = controller.Hierarchy() as ViewResult;

            // Then.
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Hierarchy_PostWithValidParameters_ShouldHaveSuccessMessage()
        {
            // Given.
            RefreshDataViewModel viewModel = new RefreshDataViewModel()
            {
                Identifiers = "1"
            };

            // When.
            ViewResult result = controller.Hierarchy(viewModel) as ViewResult;

            // Then.
            Assert.AreEqual(result.ViewBag.Message, "Refreshed Hierarchy successfully.");
        }
    }
}
