using Icon.Common.DataAccess;
using Icon.Logging;
using Icon.Web.Controllers;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.Mvc.Excel;
using Icon.Web.Mvc.Excel.Models;
using Icon.Web.Mvc.Excel.Services;
using Icon.Web.Mvc.Excel.Validators.Factories;
using Icon.Web.Mvc.Importers;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Icon.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class BulkItemImportControllerTests
    {
        private BulkItemImportController controller;
        private Mock<ILogger> mockLogger;
        private Mock<IExcelService<ItemExcelModel>> mockItemExcelService;
        private Mock<ICommandHandler<BulkImportCommand<BulkImportItemModel>>> mockBulkImportCommand;
        

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger>();
            mockItemExcelService = new Mock<IExcelService<ItemExcelModel>>();
            mockBulkImportCommand = new Mock<ICommandHandler<BulkImportCommand<BulkImportItemModel>>>();

            controller = new BulkItemImportController(mockLogger.Object, mockItemExcelService.Object, mockBulkImportCommand.Object);
        }

        [TestMethod]
        public void Index_InitialPageLoad_RendersItemImportPageWithNewViewModel()
        {
            // When.
            ViewResult result = controller.Index() as ViewResult;

            // Then.
            Assert.IsInstanceOfType(result.Model, typeof(BulkImportViewModel<ItemExcelModel>));
        }

        [TestMethod]
        public void Import_InvalidModelState_RendersIndexView()
        {
            // Given.
            controller.ModelState.AddModelError("test", "test");

            // When.
            ViewResult result = controller.Import(new BulkImportViewModel<ItemExcelModel>()) as ViewResult;

            // Then.
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void Import_InvalidSpreadsheetType_RendersIndexWithValidSpreadsheetTypeSetToFalse()
        {
            // Given.
            mockItemExcelService.Setup(m => m.Import(It.IsAny<Workbook>()))
                .Returns(new ImportResponse<ItemExcelModel> { ErrorMessage = "Test" });

            Mock<HttpSessionStateBase> mockSession = new Mock<HttpSessionStateBase>();
            Mock<ControllerContext> mockContext = new Mock<ControllerContext>();
            Mock<HttpPostedFileBase> mockAttachment = new Mock<HttpPostedFileBase>();

            mockAttachment.SetupGet(attachment => attachment.InputStream).Returns(new MemoryStream(Resources.ExcelWorkbook));
            mockContext.Setup(context => context.HttpContext.Session).Returns(mockSession.Object);
            controller.ControllerContext = mockContext.Object;

            // When.
            ViewResult result = controller.Import(new BulkImportViewModel<ItemExcelModel>() { ExcelAttachment = mockAttachment.Object }) as ViewResult;
            var viewModel = result.Model as BulkImportViewModel<ItemExcelModel>;

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

            ItemExcelModel dummyModel = new ItemExcelModel();
            dummyModel.GetType().GetProperties().Where(pi => pi.IsDefined(typeof(ExcelColumnAttribute), false)).ToList().ForEach(
                pi => pi.SetValue(dummyModel, string.Empty));

            mockAttachment.SetupGet(attachment => attachment.InputStream).Returns(new MemoryStream(Icon.Web.Tests.Unit.Resources.ExcelWorkbook));
            mockItemExcelService.Setup(importer => importer.Import(It.IsAny<Workbook>())).Returns(
                new ImportResponse<ItemExcelModel>() { Items = new List<ItemExcelModel> { dummyModel } });
            mockContext.Setup(context => context.HttpContext.Session).Returns(mockSession.Object);
            mockContext.SetupGet(p => p.HttpContext.User.Identity.Name).Returns("TestUser");

            controller.ControllerContext = mockContext.Object;

            // When.
            ViewResult result = controller.Import(new BulkImportViewModel<ItemExcelModel>() { ExcelAttachment = mockAttachment.Object }) as ViewResult;
            var viewModel = result.Model as BulkImportViewModel<BulkImportItemModel>;

            // Then.
            mockBulkImportCommand.Verify(command => command.Execute(It.IsAny<BulkImportCommand<BulkImportItemModel>>()), Times.Once);
        }

        [TestMethod]
        public void Import_SpreadsheetWithNoValidItems_BulkImportCommandShouldNotBeCalled()
        {
            // Given.
            Mock<HttpSessionStateBase> mockSession = new Mock<HttpSessionStateBase>();
            Mock<ControllerContext> mockContext = new Mock<ControllerContext>();
            Mock<HttpPostedFileBase> mockAttachment = new Mock<HttpPostedFileBase>();

            mockAttachment.SetupGet(attachment => attachment.InputStream).Returns(new MemoryStream(Resources.ExcelWorkbook));
            mockContext.Setup(context => context.HttpContext.Session).Returns(mockSession.Object);
            controller.ControllerContext = mockContext.Object;

            mockItemExcelService.Setup(importer => importer.Import(It.IsAny<Workbook>())).Returns(new ImportResponse<ItemExcelModel>());

            // When.
            ViewResult result = controller.Import(new BulkImportViewModel<ItemExcelModel>() { ExcelAttachment = mockAttachment.Object }) as ViewResult;
            var viewModel = result.Model as BulkImportViewModel<BulkImportItemModel>;

            // Then.
            mockBulkImportCommand.Verify(command => command.Execute(It.IsAny<BulkImportCommand<BulkImportItemModel>>()), Times.Never);
        }
    }
}
