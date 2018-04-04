using Icon.Common.DataAccess;
using Icon.Logging;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Controllers;
using Icon.Web.Mvc.Importers;
using Icon.Web.Mvc.Models;
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
    [TestClass] [Ignore]
    public class MerchTaxMappingControllerTests
    {
        private Mock<ILogger> mockLogger;
        private Mock<IQueryHandler<GetMerchTaxMappingsParameters, List<MerchTaxMappingModel>>> mockGetMerchTaxMappingQuery;
        private MerchTaxMappingController controller;
        private Mock<IManagerHandler<UpdateMerchTaxAssociationManager>> mockUpdateMerchTaxAssociationManagerHandler;
        private Mock<IManagerHandler<AddMerchTaxAssociationManager>> mockAddMerchTaxAssociationManagerHandler;
        private Mock<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>> mockGetHierarchyLineageQueryHandler;
        private Mock<ICommandHandler<DeleteMerchTaxMappingByIdCommand>> mockDeleteMerchTaxMappingByIdHandler;
        private Mock<ISpreadsheetImporter<BulkImportItemModel>> mockItemSpreadsheetImporter;
        private Mock<ICommandHandler<BulkImportCommand<BulkImportItemModel>>> mockBulkImportCommand;
        private MerchTaxMappingModel viewModel;

        [TestInitialize]
        public void InitializeData()
        {
            mockLogger = new Mock<ILogger>();
            mockGetMerchTaxMappingQuery = new Mock<IQueryHandler<GetMerchTaxMappingsParameters, List<MerchTaxMappingModel>>>();
            mockAddMerchTaxAssociationManagerHandler = new Mock<IManagerHandler<AddMerchTaxAssociationManager>>();
            mockUpdateMerchTaxAssociationManagerHandler = new Mock<IManagerHandler<UpdateMerchTaxAssociationManager>>();
            mockGetHierarchyLineageQueryHandler = new Mock<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>>();
            mockDeleteMerchTaxMappingByIdHandler = new Mock<ICommandHandler<DeleteMerchTaxMappingByIdCommand>>();
            mockItemSpreadsheetImporter = new Mock<ISpreadsheetImporter<BulkImportItemModel>>();
            mockBulkImportCommand = new Mock<ICommandHandler<BulkImportCommand<BulkImportItemModel>>>();

            viewModel = new MerchTaxMappingModel();

            controller = new MerchTaxMappingController(
                mockLogger.Object,
                mockGetMerchTaxMappingQuery.Object,
                mockAddMerchTaxAssociationManagerHandler.Object,
                mockUpdateMerchTaxAssociationManagerHandler.Object,
                mockGetHierarchyLineageQueryHandler.Object,
                mockDeleteMerchTaxMappingByIdHandler.Object,
                mockItemSpreadsheetImporter.Object,
                mockBulkImportCommand.Object);
        }

        [TestMethod]
        public void Index_MerchTaxMappingViewModel_ResultViewModelPopulatedWithMappings()
        {
            // Given.
            List<MerchTaxMappingModel> mappingList = new List<MerchTaxMappingModel>();

            mappingList.Add(new MerchTaxMappingModel()
            {
                MerchandiseHierarchyClassId = 1,
                MerchandiseHierarchyClassLineage = "Test Merch lineage",
                MerchandiseHierarchyClassName = "Test Merch Name",
                TaxHierarchyClassId = 2,
                TaxHierarchyClassLineage = "Test Tax Lineage",
                TaxHierarchyClassName = "Test Tax Name"
            });

            mockGetMerchTaxMappingQuery.Setup(q => q.Search(It.IsAny<GetMerchTaxMappingsParameters>())).Returns(mappingList);

            // When.
            ViewResult result = controller.Index() as ViewResult;
            MerchTaxMappingIndexViewModel model = result.Model as MerchTaxMappingIndexViewModel;

            // Then.
            Assert.AreEqual(mappingList.Count(), model.GridViewModel.MerchTaxMappingList.Count());
        }

        [TestMethod]
        public void Create_Get_ViewModelShouldBeReturned()
        {
            // Given.
            mockGetHierarchyLineageQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>()))
                .Returns(new HierarchyClassListModel { TaxHierarchyList = new List<HierarchyClassModel>(), MerchandiseHierarchyList = new List<HierarchyClassModel>() });

            // When.
            var result = controller.Create() as ViewResult;
            var viewModel = result.Model as MerchTaxMappingCreateModel;

            // Then.
            Assert.IsNotNull(viewModel.MerchandiseHierarchyClasses);
            Assert.IsNotNull(viewModel.TaxHierarchyClasses);
        }

        [TestMethod]
        public void Create_PostInvalidModelState_NoActionShouldBePerformed()
        {
            // Given.
            int testMerchandiseClassId = 1;
            int testTaxClassId = 2;

            mockGetHierarchyLineageQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>()))
                .Returns(new HierarchyClassListModel { TaxHierarchyList = new List<HierarchyClassModel>(), MerchandiseHierarchyList = new List<HierarchyClassModel>() });

            controller.ModelState.AddModelError("Test", "Error");

            var postedViewModel = new MerchTaxMappingCreateModel
            {
                MerchandiseHierarchyClassId = testMerchandiseClassId,
                TaxHierarchyClassId = testTaxClassId
            };

            // When.
            var result = controller.Create(postedViewModel) as ViewResult;
            var returnedViewModel = result.Model as MerchTaxMappingCreateModel;

            // Then.
            mockAddMerchTaxAssociationManagerHandler.Verify(h => h.Execute(It.IsAny<AddMerchTaxAssociationManager>()), Times.Never);

            Assert.AreEqual(testMerchandiseClassId, returnedViewModel.MerchandiseHierarchyClassId);
            Assert.AreEqual(testTaxClassId, returnedViewModel.TaxHierarchyClassId);
        }

        [TestMethod]
        public void Create_PostSuccessfulCreation_IndexViewShouldBeReturned()
        {
            // Given.
            int testMerchandiseClassId = 1;
            int testTaxClassId = 2;

            var postedViewModel = new MerchTaxMappingCreateModel
            {
                MerchandiseHierarchyClassId = testMerchandiseClassId,
                TaxHierarchyClassId = testTaxClassId
            };

            mockGetHierarchyLineageQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>()))
                .Returns(new HierarchyClassListModel { TaxHierarchyList = new List<HierarchyClassModel>(), MerchandiseHierarchyList = new List<HierarchyClassModel>() });

            // When.
            var result = controller.Create(postedViewModel) as RedirectToRouteResult;
            var routeValues = result.RouteValues;

            // Then.
            mockAddMerchTaxAssociationManagerHandler.Verify(h => h.Execute(It.IsAny<AddMerchTaxAssociationManager>()), Times.Once);

            Assert.AreEqual(routeValues["action"], "Index");
        }

        [TestMethod]
        public void Create_PostUnsuccessfulCreation_IndexViewShouldBeReturned()
        {
            // Given.
            int testMerchandiseClassId = 1;
            int testTaxClassId = 2;
            string errorMessage = "error";

            var postedViewModel = new MerchTaxMappingCreateModel
            {
                MerchandiseHierarchyClassId = testMerchandiseClassId,
                TaxHierarchyClassId = testTaxClassId
            };

            mockGetHierarchyLineageQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>()))
                .Returns(new HierarchyClassListModel { TaxHierarchyList = new List<HierarchyClassModel>(), MerchandiseHierarchyList = new List<HierarchyClassModel>() });

            mockAddMerchTaxAssociationManagerHandler.Setup(m => m.Execute(It.IsAny<AddMerchTaxAssociationManager>())).Throws(new Exception(errorMessage));

            // When.
            var result = controller.Create(postedViewModel) as ViewResult;
            var returnedViewModel = result.Model as MerchTaxMappingCreateModel;

            // Then.
            mockAddMerchTaxAssociationManagerHandler.Verify(h => h.Execute(It.IsAny<AddMerchTaxAssociationManager>()), Times.Once);

            Assert.AreEqual(testMerchandiseClassId, returnedViewModel.MerchandiseHierarchyClassId);
            Assert.AreEqual(testTaxClassId, returnedViewModel.TaxHierarchyClassId);
        }

        [TestMethod]
        public void Edit_HierarchyClassIdGet_ViewModelShouldBeReturned()
        {
            // Given.
            int testMerchandiseClassId = 1;
            int testTaxClassId = 2;
            string testMerchandiseClassName = "Test Merchandise";
            string testTaxClassName = "Test Tax";
            string testMerchandiseLineage = "Merch|1";
            string testTaxLineage = "Tax|2";

            var mapping = new MerchTaxMappingModel
            {
                MerchandiseHierarchyClassId = testMerchandiseClassId,
                MerchandiseHierarchyClassName = testMerchandiseClassName,
                TaxHierarchyClassId = testTaxClassId,
                TaxHierarchyClassName = testTaxClassName,
                TaxHierarchyClassLineage = testTaxLineage,
                MerchandiseHierarchyClassLineage = testMerchandiseLineage
            };

            var mappings = new List<MerchTaxMappingModel> { mapping };

            mockGetMerchTaxMappingQuery.Setup(q => q.Search(It.IsAny<GetMerchTaxMappingsParameters>())).Returns(mappings);
            mockGetHierarchyLineageQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(new HierarchyClassListModel { TaxHierarchyList = new List<HierarchyClassModel>() });

            // When.
            var result = controller.Edit(testMerchandiseClassId) as ViewResult;
            var viewModel = result.Model as MerchTaxMappingEditModel;
            
            // Then.
            Assert.AreEqual(testMerchandiseClassId, viewModel.MerchandiseHierarchyClassId);
            Assert.AreEqual(testMerchandiseClassName, viewModel.MerchandiseHierarchyClassName);
            Assert.AreEqual(testTaxClassId, viewModel.TaxHierarchyClassId);
            Assert.AreEqual(testTaxClassName, viewModel.TaxHierarchyClassName);
            Assert.AreEqual(testMerchandiseLineage, viewModel.MerchandiseHierarchyClassLineage);
            Assert.AreEqual(testTaxLineage, viewModel.TaxHierarchyClassLineage);
        }

        [TestMethod]
        public void Edit_PostInvalidModelState_NoActionShouldBePerformed()
        {
            // Given.
            int testMerchandiseClassId = 1;
            int testTaxClassId = 2;
            string testMerchandiseClassName = "Test Merchandise";
            string testTaxClassName = "Test Tax";
            string testMerchandiseLineage = "Merch|1";
            string testTaxLineage = "Tax|2";

            var postedViewModel = new MerchTaxMappingEditModel
            {
                MerchandiseHierarchyClassId = testMerchandiseClassId,
                MerchandiseHierarchyClassName = testMerchandiseClassName,
                MerchandiseHierarchyClassLineage = testMerchandiseLineage,
                TaxHierarchyClassId = testTaxClassId,
                TaxHierarchyClassName = testTaxClassName,
                TaxHierarchyClassLineage = testTaxLineage,
                TaxHierarchyClasses = new List<SelectListItem>()
            };

            mockGetHierarchyLineageQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(new HierarchyClassListModel { TaxHierarchyList = new List<HierarchyClassModel>() });

            controller.ModelState.AddModelError("Test", "Error");

            // When.
            var result = controller.Edit(postedViewModel) as ViewResult;
            var returnedViewModel = result.Model as MerchTaxMappingEditModel;

            // Then.
            mockUpdateMerchTaxAssociationManagerHandler.Verify(h => h.Execute(It.IsAny<UpdateMerchTaxAssociationManager>()), Times.Never);

            Assert.AreEqual(testMerchandiseClassId, returnedViewModel.MerchandiseHierarchyClassId);
            Assert.AreEqual(testMerchandiseClassName, returnedViewModel.MerchandiseHierarchyClassName);
            Assert.AreEqual(testTaxClassId, returnedViewModel.TaxHierarchyClassId);
            Assert.AreEqual(testTaxClassName, returnedViewModel.TaxHierarchyClassName);
            Assert.AreEqual(testMerchandiseLineage, returnedViewModel.MerchandiseHierarchyClassLineage);
            Assert.AreEqual(testTaxLineage, returnedViewModel.TaxHierarchyClassLineage);
        }

        [TestMethod]
        public void Edit_PostSuccessfulUpdate_SuccessMessageShouldBeReturned()
        {
            // Given.
            int testMerchandiseClassId = 1;
            int testTaxClassId = 2;
            string testMerchandiseClassName = "Test Merchandise";
            string testTaxClassName = "Test Tax";
            string testMerchandiseLineage = "Merch|1";
            string testTaxLineage = "Tax|2";

            var postedViewModel = new MerchTaxMappingEditModel
            {
                MerchandiseHierarchyClassId = testMerchandiseClassId,
                MerchandiseHierarchyClassName = testMerchandiseClassName,
                MerchandiseHierarchyClassLineage = testMerchandiseLineage,
                TaxHierarchyClassId = testTaxClassId,
                TaxHierarchyClassName = testTaxClassName,
                TaxHierarchyClassLineage = testTaxLineage,
                TaxHierarchyClasses = new List<SelectListItem>()
            };

            mockGetHierarchyLineageQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(new HierarchyClassListModel { TaxHierarchyList = new List<HierarchyClassModel>() });

            // When.
            var result = controller.Edit(postedViewModel) as ViewResult;
            var returnedViewModel = result.Model as MerchTaxMappingEditModel;

            // Then.
            mockUpdateMerchTaxAssociationManagerHandler.Verify(h => h.Execute(It.IsAny<UpdateMerchTaxAssociationManager>()), Times.Once);

            Assert.AreEqual(result.ViewData["SuccessMessage"], "Merchandise/Tax mapping update was successful.");
            Assert.IsNull(result.ViewData["ErrorMessage"]);
            Assert.AreEqual(testMerchandiseClassId, returnedViewModel.MerchandiseHierarchyClassId);
            Assert.AreEqual(testMerchandiseClassName, returnedViewModel.MerchandiseHierarchyClassName);
            Assert.AreEqual(testTaxClassId, returnedViewModel.TaxHierarchyClassId);
            Assert.AreEqual(testTaxClassName, returnedViewModel.TaxHierarchyClassName);
            Assert.AreEqual(testMerchandiseLineage, returnedViewModel.MerchandiseHierarchyClassLineage);
            Assert.AreEqual(testTaxLineage, returnedViewModel.TaxHierarchyClassLineage);
        }

        [TestMethod]
        public void Edit_PostUnsuccessfulUpdate_ErrorMessageShouldBeReturned()
        {
            // Given.
            int testMerchandiseClassId = 1;
            int testTaxClassId = 2;
            string testMerchandiseClassName = "Test Merchandise";
            string testTaxClassName = "Test Tax";
            string testMerchandiseLineage = "Merch|1";
            string testTaxLineage = "Tax|2";
            string errorMessage = "error";

            var postedViewModel = new MerchTaxMappingEditModel
            {
                MerchandiseHierarchyClassId = testMerchandiseClassId,
                MerchandiseHierarchyClassName = testMerchandiseClassName,
                MerchandiseHierarchyClassLineage = testMerchandiseLineage,
                TaxHierarchyClassId = testTaxClassId,
                TaxHierarchyClassName = testTaxClassName,
                TaxHierarchyClassLineage = testTaxLineage,
                TaxHierarchyClasses = new List<SelectListItem>()
            };

            mockGetHierarchyLineageQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(new HierarchyClassListModel { TaxHierarchyList = new List<HierarchyClassModel>() });
            mockUpdateMerchTaxAssociationManagerHandler.Setup(h => h.Execute(It.IsAny<UpdateMerchTaxAssociationManager>())).Throws(new Exception(errorMessage));

            // When.
            var result = controller.Edit(postedViewModel) as ViewResult;
            var returnedViewModel = result.Model as MerchTaxMappingEditModel;

            // Then.
            mockUpdateMerchTaxAssociationManagerHandler.Verify(h => h.Execute(It.IsAny<UpdateMerchTaxAssociationManager>()), Times.Once);

            Assert.AreEqual(result.ViewData["ErrorMessage"], errorMessage);
            Assert.IsNull(result.ViewData["SuccessMessage"]);
            Assert.AreEqual(testMerchandiseClassId, returnedViewModel.MerchandiseHierarchyClassId);
            Assert.AreEqual(testMerchandiseClassName, returnedViewModel.MerchandiseHierarchyClassName);
            Assert.AreEqual(testTaxClassId, returnedViewModel.TaxHierarchyClassId);
            Assert.AreEqual(testTaxClassName, returnedViewModel.TaxHierarchyClassName);
            Assert.AreEqual(testMerchandiseLineage, returnedViewModel.MerchandiseHierarchyClassLineage);
            Assert.AreEqual(testTaxLineage, returnedViewModel.TaxHierarchyClassLineage);
        }

        [TestMethod]
        public void Import_InvalidModelState_IndexViewShouldBeRendered()
        {
            // Given.
            mockGetMerchTaxMappingQuery.Setup(q => q.Search(It.IsAny<GetMerchTaxMappingsParameters>())).Returns(new List<MerchTaxMappingModel>());
            
            controller.ModelState.AddModelError("test", "test");

            // When.
            ViewResult result = controller.Import(new MerchTaxMappingIndexViewModel()) as ViewResult;

            // Then.
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void Import_InvalidSpreadsheetType_IndexViewShouldBeRendered()
        {
            // Given.
            mockGetMerchTaxMappingQuery.Setup(q => q.Search(It.IsAny<GetMerchTaxMappingsParameters>())).Returns(new List<MerchTaxMappingModel>());

            Mock<HttpPostedFileBase> mockAttachment = new Mock<HttpPostedFileBase>();
            mockAttachment.SetupGet(attachment => attachment.InputStream).Returns(new MemoryStream(Icon.Web.Tests.Unit.Resources.ExcelWorkbook));
            mockItemSpreadsheetImporter.Setup(importer => importer.IsValidSpreadsheetType()).Returns(false);

            // When.
            ViewResult result = controller.Import(new MerchTaxMappingIndexViewModel { ExcelAttachment = mockAttachment.Object }) as ViewResult;
            var viewModel = result.Model as BulkImportViewModel<BulkImportItemModel>;

            // Then.
            Assert.AreEqual("Index", result.ViewName);
            Assert.AreEqual("Invalid spreadsheet type detected.  Please check column headers.", result.ViewData["ErrorMessage"]);
        }

        [TestMethod]
        public void Import_SpreadsheetWithValidItems_BulkImportCommandShouldBeCalled()
        {
            // Given.
            mockGetMerchTaxMappingQuery.Setup(q => q.Search(It.IsAny<GetMerchTaxMappingsParameters>())).Returns(new List<MerchTaxMappingModel>());

            Mock<HttpSessionStateBase> mockSession = new Mock<HttpSessionStateBase>();
            Mock<ControllerContext> mockContext = new Mock<ControllerContext>();
            Mock<HttpPostedFileBase> mockAttachment = new Mock<HttpPostedFileBase>();

            mockAttachment.SetupGet(attachment => attachment.InputStream).Returns(new MemoryStream(Icon.Web.Tests.Unit.Resources.ExcelWorkbook));
            mockItemSpreadsheetImporter.Setup(importer => importer.IsValidSpreadsheetType()).Returns(true);
            mockItemSpreadsheetImporter.Setup(importer => importer.ValidRows).Returns(new List<BulkImportItemModel> { new BulkImportItemModel() });
            mockItemSpreadsheetImporter.Setup(importer => importer.ErrorRows).Returns(new List<BulkImportItemModel>());
            mockContext.Setup(context => context.HttpContext.Session).Returns(mockSession.Object);
            mockContext.SetupGet(p => p.HttpContext.User.Identity.Name).Returns("TestUser");

            controller.ControllerContext = mockContext.Object;

            // When.
            ViewResult result = controller.Import(new MerchTaxMappingIndexViewModel { ExcelAttachment = mockAttachment.Object }) as ViewResult;
            var viewModel = result.Model as BulkImportViewModel<BulkImportItemModel>;

            // Then.
            mockBulkImportCommand.Verify(command => command.Execute(It.IsAny<BulkImportCommand<BulkImportItemModel>>()), Times.Once);
        }

        [TestMethod]
        public void Import_SpreadsheetWithNoValidItems_BulkImportCommandShouldNotBeCalled()
        {
            // Given.
            mockGetMerchTaxMappingQuery.Setup(q => q.Search(It.IsAny<GetMerchTaxMappingsParameters>())).Returns(new List<MerchTaxMappingModel>());

            Mock<HttpSessionStateBase> mockSession = new Mock<HttpSessionStateBase>();
            Mock<ControllerContext> mockContext = new Mock<ControllerContext>();
            Mock<HttpPostedFileBase> mockAttachment = new Mock<HttpPostedFileBase>();

            mockAttachment.SetupGet(attachment => attachment.InputStream).Returns(new MemoryStream(Icon.Web.Tests.Unit.Resources.ExcelWorkbook));
            mockItemSpreadsheetImporter.Setup(importer => importer.IsValidSpreadsheetType()).Returns(true);
            mockItemSpreadsheetImporter.Setup(importer => importer.ValidRows).Returns(new List<BulkImportItemModel>());
            mockItemSpreadsheetImporter.Setup(importer => importer.ErrorRows).Returns(new List<BulkImportItemModel>());
            mockContext.Setup(context => context.HttpContext.Session).Returns(mockSession.Object);

            controller.ControllerContext = mockContext.Object;

            // When.
            ViewResult result = controller.Import(new MerchTaxMappingIndexViewModel { ExcelAttachment = mockAttachment.Object }) as ViewResult;
            var viewModel = result.Model as BulkImportViewModel<BulkImportItemModel>;

            // Then.
            mockBulkImportCommand.Verify(command => command.Execute(It.IsAny<BulkImportCommand<BulkImportItemModel>>()), Times.Never);
        }

        [TestMethod]
        public void Import_SpreadsheetWithErrorItems_ImportErrorsViewShouldBeReturned()
        {
            // Given.
            mockGetMerchTaxMappingQuery.Setup(q => q.Search(It.IsAny<GetMerchTaxMappingsParameters>())).Returns(new List<MerchTaxMappingModel>());

            Mock<HttpSessionStateBase> mockSession = new Mock<HttpSessionStateBase>();
            Mock<ControllerContext> mockContext = new Mock<ControllerContext>();
            Mock<HttpPostedFileBase> mockAttachment = new Mock<HttpPostedFileBase>();

            mockAttachment.SetupGet(attachment => attachment.InputStream).Returns(new MemoryStream(Icon.Web.Tests.Unit.Resources.ExcelWorkbook));
            mockItemSpreadsheetImporter.Setup(importer => importer.IsValidSpreadsheetType()).Returns(true);
            mockItemSpreadsheetImporter.Setup(importer => importer.ValidRows).Returns(new List<BulkImportItemModel>());
            mockItemSpreadsheetImporter.Setup(importer => importer.ErrorRows).Returns(new List<BulkImportItemModel> { new BulkImportItemModel() });
            mockContext.Setup(context => context.HttpContext.Session).Returns(mockSession.Object);

            controller.ControllerContext = mockContext.Object;

            // When.
            ViewResult result = controller.Import(new MerchTaxMappingIndexViewModel { ExcelAttachment = mockAttachment.Object }) as ViewResult;
            var viewModel = result.Model as List<DefaultTaxClassMismatchExportModel>;

            // Then.
            Assert.AreEqual(1, viewModel.Count);
            Assert.AreEqual("ImportErrors", result.ViewName);
        }
    }
}
