using Icon.Common.DataAccess;
using Icon.Logging;
using Icon.Web.Controllers;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.Mvc.Importers;
using Icon.Web.Mvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Icon.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class BulkPluMappingControllerTests
    {
        private Mock<ILogger> mockLogger;
        private Mock<IPluSpreadsheetImporter> mockImporter;
        private Mock<ICommandHandler<BulkImportCommand<BulkImportPluModel>>> mockCommand;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger>();
            mockImporter = new Mock<IPluSpreadsheetImporter>();
            mockCommand = new Mock<ICommandHandler<BulkImportCommand<BulkImportPluModel>>>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            mockLogger = null;
            mockImporter = null;
            mockCommand = null;
        }

        [TestMethod]
        public void Index_InitialPageLoad_BulkPluMappingViewModelShouldNotBeNull()
        {
            // Given.
            BulkPluMappingController controller = new BulkPluMappingController(mockLogger.Object, mockImporter.Object, mockCommand.Object);

            // When.
            ViewResult result = controller.Index() as ViewResult;

            // Then.
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Import_InvalidModelState_RendersIndexView()
        {
            // Given.
            BulkPluMappingController controller = new BulkPluMappingController(mockLogger.Object, mockImporter.Object, mockCommand.Object);
            controller.ModelState.AddModelError("test", "test");

            //// When.
            ViewResult result = controller.Import(new BulkPluMappingViewModel()) as ViewResult;

            //// Then.
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void Import_InvalidSpreadsheetType_RendersIndexWithValidSpreadsheetTypeSetToFalse()
        {
            // Given.
            Mock<HttpPostedFileBase> mockAttachment = new Mock<HttpPostedFileBase>();
            BulkPluMappingController controller = new BulkPluMappingController(mockLogger.Object, mockImporter.Object, mockCommand.Object);

            mockAttachment.SetupGet(attachment => attachment.InputStream).Returns(new MemoryStream(Icon.Web.Tests.Unit.Resources.ExcelWorkbook));
            mockImporter.Setup(importer => importer.ValidSpreadsheetType()).Returns(false);

            // When.
            ViewResult result = controller.Import(new BulkPluMappingViewModel() { ExcelAttachment = mockAttachment.Object }) as ViewResult;
            var viewModel = result.Model as BulkPluMappingViewModel;

            // Then.
            Assert.AreEqual("Index", result.ViewName);
            Assert.AreEqual(false, viewModel.ValidSpreadsheetType);
        }

        [TestMethod]
        public void Import_SpreadsheetWithValidItems_BulkImportCommandShouldBeCalled()
        {
            // Given.
            Mock<HttpSessionStateBase> mockSession = new Mock<HttpSessionStateBase>();
            Mock<ControllerContext> mockContext = new Mock<ControllerContext>();
            Mock<HttpPostedFileBase> mockAttachment = new Mock<HttpPostedFileBase>();

            mockAttachment.SetupGet(attachment => attachment.InputStream).Returns(new MemoryStream(Icon.Web.Tests.Unit.Resources.ExcelWorkbook));
            mockImporter.Setup(importer => importer.ValidSpreadsheetType()).Returns(true);
            mockImporter.Setup(importer => importer.ValidRows).Returns(new List<BulkImportPluModel> { new BulkImportPluModel() });
            mockContext.Setup(context => context.HttpContext.Session).Returns(mockSession.Object);
            mockContext.SetupGet(p => p.HttpContext.User.Identity.Name).Returns("TestUser");

            BulkPluMappingController controller = new BulkPluMappingController(mockLogger.Object, mockImporter.Object, mockCommand.Object);
            controller.ControllerContext = mockContext.Object;

            // When.
            ViewResult result = controller.Import(new BulkPluMappingViewModel() { ExcelAttachment = mockAttachment.Object }) as ViewResult;
            var viewModel = result.Model as BulkPluMappingViewModel;

            // Then.
            mockCommand.Verify(command => command.Execute(It.IsAny<BulkImportCommand<BulkImportPluModel>>()), Times.Once);
        }

        [TestMethod]
        public void Import_SpreadsheetWithNoValidItems_BulkImportCommandShouldNotBeCalled()
        {
            // Given.
            Mock<HttpSessionStateBase> mockSession = new Mock<HttpSessionStateBase>();
            Mock<ControllerContext> mockContext = new Mock<ControllerContext>();
            Mock<HttpPostedFileBase> mockAttachment = new Mock<HttpPostedFileBase>();

            mockAttachment.SetupGet(attachment => attachment.InputStream).Returns(new MemoryStream(Icon.Web.Tests.Unit.Resources.ExcelWorkbook));
            mockImporter.Setup(importer => importer.ValidSpreadsheetType()).Returns(true);
            mockImporter.Setup(importer => importer.ValidRows).Returns(new List<BulkImportPluModel>());
            mockContext.Setup(context => context.HttpContext.Session).Returns(mockSession.Object);

            BulkPluMappingController controller = new BulkPluMappingController(mockLogger.Object, mockImporter.Object, mockCommand.Object);
            controller.ControllerContext = mockContext.Object;

            // When.
            ViewResult result = controller.Import(new BulkPluMappingViewModel() { ExcelAttachment = mockAttachment.Object }) as ViewResult;
            var viewModel = result.Model as BulkPluMappingViewModel;

            // Then.
            mockCommand.Verify(command => command.Execute(It.IsAny<BulkImportCommand<BulkImportPluModel>>()), Times.Never);
        }
    }
}
