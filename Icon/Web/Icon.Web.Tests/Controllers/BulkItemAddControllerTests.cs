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
    [TestClass] [Ignore]
    public class BulkItemAddControllerTests
    {
        private Mock<ILogger> mockLogger;
        private Mock<ISpreadsheetImporter<BulkImportNewItemModel>> mockNewItemSpreadsheetImporter;
        private Mock<ICommandHandler<BulkImportCommand<BulkImportNewItemModel>>> mockBulkImportCommand;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger>();
            mockNewItemSpreadsheetImporter = new Mock<ISpreadsheetImporter<BulkImportNewItemModel>>();
            mockBulkImportCommand = new Mock<ICommandHandler<BulkImportCommand<BulkImportNewItemModel>>>();
        }

        [TestMethod]
        public void Index_InitialPageLoad_RendersItemImportPageWithNewViewModel()
        {
            // Given.
            var controller = new BulkItemAddController(mockLogger.Object, mockNewItemSpreadsheetImporter.Object, mockBulkImportCommand.Object);

            // When.
            ViewResult result = controller.Index() as ViewResult;

            // Then.
            Assert.IsInstanceOfType(result.Model, typeof(BulkImportViewModel<BulkImportNewItemModel>));
        }

        [TestMethod]
        public void Import_InvalidModelState_RendersIndexView()
        {
            // Given.
            var controller = new BulkItemAddController(mockLogger.Object, mockNewItemSpreadsheetImporter.Object, mockBulkImportCommand.Object);
            controller.ModelState.AddModelError("test", "test");

            // When.
            ViewResult result = controller.Import(new BulkImportViewModel<BulkImportNewItemModel>()) as ViewResult;

            // Then.
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void Import_InvalidSpreadsheetType_RendersIndexWithValidSpreadsheetTypeSetToFalse()
        {
            // Given.
            Mock<HttpPostedFileBase> mockAttachment = new Mock<HttpPostedFileBase>();
            var controller = new BulkItemAddController(mockLogger.Object, mockNewItemSpreadsheetImporter.Object, mockBulkImportCommand.Object);

            mockAttachment.SetupGet(attachment => attachment.InputStream).Returns(new MemoryStream(Icon.Web.Tests.Unit.Resources.ExcelWorkbook));
            mockNewItemSpreadsheetImporter.Setup(importer => importer.IsValidSpreadsheetType()).Returns(false);
            
            // When.
            ViewResult result = controller.Import(new BulkImportViewModel<BulkImportNewItemModel>() { ExcelAttachment = mockAttachment.Object }) as ViewResult;
            var viewModel = result.Model as BulkImportViewModel<BulkImportNewItemModel>;

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
            mockNewItemSpreadsheetImporter.Setup(importer => importer.IsValidSpreadsheetType()).Returns(true);
            mockNewItemSpreadsheetImporter.Setup(importer => importer.ValidRows).Returns(new List<BulkImportNewItemModel> { new BulkImportNewItemModel() });
            mockContext.Setup(context => context.HttpContext.Session).Returns(mockSession.Object);
            mockContext.SetupGet(p => p.HttpContext.User.Identity.Name).Returns("TestUser");

            var controller = new BulkItemAddController(mockLogger.Object, mockNewItemSpreadsheetImporter.Object, mockBulkImportCommand.Object);
            controller.ControllerContext = mockContext.Object;

            // When.
            ViewResult result = controller.Import(new BulkImportViewModel<BulkImportNewItemModel>() { ExcelAttachment = mockAttachment.Object }) as ViewResult;
            var viewModel = result.Model as BulkImportViewModel<BulkImportNewItemModel>;

            // Then.
            mockBulkImportCommand.Verify(command => command.Execute(It.IsAny<BulkImportCommand<BulkImportNewItemModel>>()), Times.Once);
        }

        [TestMethod]
        public void Import_SpreadsheetWithNoValidItems_BulkImportCommandShouldNotBeCalled()
        {
            // Given.
            Mock<HttpSessionStateBase> mockSession = new Mock<HttpSessionStateBase>();
            Mock<ControllerContext> mockContext = new Mock<ControllerContext>();
            Mock<HttpPostedFileBase> mockAttachment = new Mock<HttpPostedFileBase>();

            mockAttachment.SetupGet(attachment => attachment.InputStream).Returns(new MemoryStream(Icon.Web.Tests.Unit.Resources.ExcelWorkbook));
            mockNewItemSpreadsheetImporter.Setup(importer => importer.IsValidSpreadsheetType()).Returns(true);
            mockNewItemSpreadsheetImporter.Setup(importer => importer.ValidRows).Returns(new List<BulkImportNewItemModel>());
            mockContext.Setup(context => context.HttpContext.Session).Returns(mockSession.Object);

            var controller = new BulkItemAddController(mockLogger.Object, mockNewItemSpreadsheetImporter.Object, mockBulkImportCommand.Object);
            controller.ControllerContext = mockContext.Object;

            // When.
            ViewResult result = controller.Import(new BulkImportViewModel<BulkImportNewItemModel>() { ExcelAttachment = mockAttachment.Object }) as ViewResult;
            var viewModel = result.Model as BulkImportViewModel<BulkImportNewItemModel>;

            // Then.
            mockBulkImportCommand.Verify(command => command.Execute(It.IsAny<BulkImportCommand<BulkImportNewItemModel>>()), Times.Never);
        }
    }
}
