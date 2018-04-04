﻿using Icon.Common.DataAccess;
using Icon.Logging;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Models;
using Icon.Web.Mvc.Controllers;
using Icon.Web.Mvc.Importers;
using Icon.Web.Mvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Icon.Web.Tests.Unit.Controllers
{
    [TestClass] [Ignore]
    public class BulkBrandImportControllerTests
    {
        private BulkBrandImportController controller;
        private Mock<ILogger> mockLogger;
        private Mock<ISpreadsheetImporter<BulkImportBrandModel>> mockImporter;
        private Mock<ICommandHandler<BulkImportCommand<BulkImportBrandModel>>> mockBulkImportCommand;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger>();
            mockImporter = new Mock<ISpreadsheetImporter<BulkImportBrandModel>>();
            mockBulkImportCommand = new Mock<ICommandHandler<BulkImportCommand<BulkImportBrandModel>>>();

            controller = new BulkBrandImportController(mockLogger.Object, mockImporter.Object, mockBulkImportCommand.Object);
        }

        [TestMethod]
        public void Index_InitialPageLoad_RendersItemImportPageWithNewViewModel()
        {
            // When.
            ViewResult result = controller.Index() as ViewResult;

            // Then.
            Assert.IsInstanceOfType(result.Model, typeof(BulkImportViewModel<BulkImportBrandModel>));
        }

        [TestMethod]
        public void Import_InvalidModelState_RendersIndexView()
        {
            // Given.
            controller.ModelState.AddModelError("test", "test");

            // When.
            ViewResult result = controller.Import(new BulkImportViewModel<BulkImportBrandModel>()) as ViewResult;

            // Then.
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void Import_InvalidSpreadsheetType_RendersIndexWithValidSpreadsheetTypeSetToFalse()
        {
            // Given.
            Mock<HttpPostedFileBase> mockAttachment = new Mock<HttpPostedFileBase>();

            mockAttachment.SetupGet(attachment => attachment.InputStream).Returns(new MemoryStream(Icon.Web.Tests.Unit.Resources.ExcelWorkbook));
            mockImporter.Setup(importer => importer.IsValidSpreadsheetType()).Returns(false);

            // When.
            ViewResult result = controller.Import(new BulkImportViewModel<BulkImportBrandModel>() { ExcelAttachment = mockAttachment.Object }) as ViewResult;
            var viewModel = result.Model as BulkImportViewModel<BulkImportBrandModel>;

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
            mockImporter.Setup(importer => importer.IsValidSpreadsheetType()).Returns(true);
            mockImporter.Setup(importer => importer.ValidRows).Returns(new List<BulkImportBrandModel> { new BulkImportBrandModel() });
            mockContext.Setup(context => context.HttpContext.Session).Returns(mockSession.Object);
            mockContext.SetupGet(p => p.HttpContext.User.Identity.Name).Returns("TestUser");

            controller.ControllerContext = mockContext.Object;

            // When.
            ViewResult result = controller.Import(new BulkImportViewModel<BulkImportBrandModel>() { ExcelAttachment = mockAttachment.Object }) as ViewResult;
            var viewModel = result.Model as BulkImportViewModel<BulkImportBrandModel>;

            // Then.
            mockBulkImportCommand.Verify(command => command.Execute(It.IsAny<BulkImportCommand<BulkImportBrandModel>>()), Times.Once);
        }

        [TestMethod]
        public void Import_SpreadsheetWithNoValidItems_BulkImportCommandShouldNotBeCalled()
        {
            // Given.
            Mock<HttpSessionStateBase> mockSession = new Mock<HttpSessionStateBase>();
            Mock<ControllerContext> mockContext = new Mock<ControllerContext>();
            Mock<HttpPostedFileBase> mockAttachment = new Mock<HttpPostedFileBase>();

            mockAttachment.SetupGet(attachment => attachment.InputStream).Returns(new MemoryStream(Icon.Web.Tests.Unit.Resources.ExcelWorkbook));
            mockImporter.Setup(importer => importer.IsValidSpreadsheetType()).Returns(true);
            mockImporter.Setup(importer => importer.ValidRows).Returns(new List<BulkImportBrandModel>());
            mockContext.Setup(context => context.HttpContext.Session).Returns(mockSession.Object);

            controller.ControllerContext = mockContext.Object;

            // When.
            ViewResult result = controller.Import(new BulkImportViewModel<BulkImportBrandModel>() { ExcelAttachment = mockAttachment.Object }) as ViewResult;
            var viewModel = result.Model as BulkImportViewModel<BulkImportBrandModel>;

            // Then.
            mockBulkImportCommand.Verify(command => command.Execute(It.IsAny<BulkImportCommand<BulkImportBrandModel>>()), Times.Never);
        }
    }
}
