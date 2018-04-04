using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.Controllers;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Icon.Web.Tests.Unit.Controllers
{
    [TestClass] [Ignore]
    public class PluMappingControllerTests
    {
        private Mock<ILogger> mockLogger;
        private Mock<IQueryHandler<GetPluMappingParameters, List<Item>>> mockPluQuery;
        private Mock<IQueryHandler<GetItemByIdParameters, Item>> mockGetItemQuery;
        private Mock<ICommandHandler<UpdatePluCommand>> mockUpdatePlu;
        private Mock<IQueryHandler<GetPluRemappingsParameters, List<BulkImportPluRemapModel>>> mockRemapQuery;
        private Mock<HttpSessionStateBase> mockSession;
        private Mock<ControllerContext> mockContext;

        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ILogger>();
            this.mockPluQuery = new Mock<IQueryHandler<GetPluMappingParameters, List<Item>>>();
            this.mockGetItemQuery = new Mock<IQueryHandler<GetItemByIdParameters, Item>>();
            this.mockUpdatePlu = new Mock<ICommandHandler<UpdatePluCommand>>();
            this.mockRemapQuery = new Mock<IQueryHandler<GetPluRemappingsParameters, List<BulkImportPluRemapModel>>>();
            this.mockSession = new Mock<HttpSessionStateBase>();
            this.mockContext = new Mock<ControllerContext>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.mockLogger = null;
            this.mockPluQuery = null;
        }

        [TestMethod]
        public void Index_InitialPageLoad_ReturnsViewNotNull()
        {
            // Given
            var pluController = new PluMappingController(
                mockLogger.Object, mockPluQuery.Object, mockGetItemQuery.Object, mockRemapQuery.Object, mockUpdatePlu.Object);

            // When
            var result = pluController.Index() as ViewResult;

            // Then
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Search_ValidViewModel_ReturnsPartialView()
        {
            // Given
            var pluController = new PluMappingController(
                mockLogger.Object, mockPluQuery.Object, mockGetItemQuery.Object, mockRemapQuery.Object, mockUpdatePlu.Object);

            List<Item> pluItemList = new List<Item> { GetFakeItemPluMap() };
            List<PluMappingViewModel> pluMapList = new List<PluMappingViewModel>();
            PluSearchViewModel searchPlus = new PluSearchViewModel { PluMapping = pluMapList };

            mockSession.SetupSet(s => s["grid_search_results"] = new List<PluMappingViewModel>());
            mockContext.Setup(c => c.HttpContext.Session).Returns(mockSession.Object);
            pluController.ControllerContext = mockContext.Object;

            mockPluQuery.Setup(q => q.Search(It.IsAny<GetPluMappingParameters>())).Returns(pluItemList);

            // When
            var result = pluController.Search(searchPlus) as PartialViewResult;

            // Then
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Edit_ItemId_ReturnViewResultNotNull()
        {
            // Given
            var pluController = new PluMappingController(
                mockLogger.Object, mockPluQuery.Object, mockGetItemQuery.Object, mockRemapQuery.Object, mockUpdatePlu.Object);

            Item item = GetFakeItemPluMap();
            int id = 5;

            mockGetItemQuery.Setup(q => q.Search(It.IsAny<GetItemByIdParameters>())).Returns(item);

            // When
            var result = pluController.Edit(id) as ViewResult;

            // Then
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Edit_ValidPluMapping_UpdateExecutesOnlyOnce()
        {
            // Given
            var pluController = new PluMappingController(
                mockLogger.Object, mockPluQuery.Object, mockGetItemQuery.Object, mockRemapQuery.Object, mockUpdatePlu.Object);
            List<BulkImportPluRemapModel> remapList = new List<BulkImportPluRemapModel>();
            PluMappingViewModel pluModel = new PluMappingViewModel
            {
                ItemId = 5,
                NationalPlu = "5555",
                flPLU = "1212"
            };

            mockRemapQuery.Setup(r => r.Search(It.IsAny<GetPluRemappingsParameters>())).Returns(remapList);

            // When
            var result = pluController.Edit(pluModel) as ViewResult;

            // Then
            mockUpdatePlu.Verify(update => update.Execute(It.IsAny<UpdatePluCommand>()), Times.Once);
        }

        [TestMethod]
        public void Edit_ValidPluMapping_ReturnViewResultNotNull()
        {
            // Given
            var pluController = new PluMappingController(
                mockLogger.Object, mockPluQuery.Object, mockGetItemQuery.Object, mockRemapQuery.Object, mockUpdatePlu.Object);
            List<BulkImportPluRemapModel> remapList = new List<BulkImportPluRemapModel>();
            PluMappingViewModel pluModel = new PluMappingViewModel
            {
                ItemId = 5,
                NationalPlu = "5555",
                flPLU = "1212"
            };

            mockRemapQuery.Setup(r => r.Search(It.IsAny<GetPluRemappingsParameters>())).Returns(remapList);

            // When
            var result = pluController.Edit(pluModel) as ViewResult;

            // Then
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Edit_ValidData_SuccessfulViewDataMessageIsNotNull()
        {
            // Given
            var pluController = new PluMappingController(
                mockLogger.Object, mockPluQuery.Object, mockGetItemQuery.Object, mockRemapQuery.Object, mockUpdatePlu.Object);
            List<BulkImportPluRemapModel> remapList = new List<BulkImportPluRemapModel>();
            PluMappingViewModel pluModel = new PluMappingViewModel
            {
                ItemId = 5,
                NationalPlu = "5555",
                flPLU = "1212"
            };

            mockRemapQuery.Setup(r => r.Search(It.IsAny<GetPluRemappingsParameters>())).Returns(remapList);

            // When
            var result = pluController.Edit(pluModel) as ViewResult;

            // Then
            Assert.IsNotNull(result.ViewData["UpdateSuccess"]);
        }

        [TestMethod]
        public void Edit_UpdateException_FailureViewDataMessageIsNotNull()
        {
            // Given
            var pluController = new PluMappingController(
                mockLogger.Object, mockPluQuery.Object, mockGetItemQuery.Object, mockRemapQuery.Object, mockUpdatePlu.Object);
            
            List<BulkImportPluRemapModel> remapList = new List<BulkImportPluRemapModel>();

            PluMappingViewModel pluModel = new PluMappingViewModel
            {
                ItemId = 5,
                NationalPlu = "5555",
                flPLU = "1212"
            };

            mockRemapQuery.Setup(r => r.Search(It.IsAny<GetPluRemappingsParameters>())).Returns(remapList);
            mockUpdatePlu.Setup(r => r.Execute(It.IsAny<UpdatePluCommand>())).Throws(new CommandException());

            // When
            var result = pluController.Edit(pluModel) as ViewResult;

            // Then
            Assert.IsNotNull(result.ViewData["UpdateFailed"]);
        }

        [TestMethod]
        public void Edit_UpdateException_ReturnViewResultNotNull()
        {
            // Given
            var pluController = new PluMappingController(
                mockLogger.Object, mockPluQuery.Object, mockGetItemQuery.Object, mockRemapQuery.Object, mockUpdatePlu.Object);
            
            List<BulkImportPluRemapModel> remapList = new List<BulkImportPluRemapModel>();
            
            PluMappingViewModel pluModel = new PluMappingViewModel
            {
                ItemId = 5,
                NationalPlu = "5555",
                flPLU = "1212"
            };

            mockRemapQuery.Setup(r => r.Search(It.IsAny<GetPluRemappingsParameters>())).Returns(remapList);
            mockUpdatePlu.Setup(r => r.Execute(It.IsAny<UpdatePluCommand>())).Throws(new CommandException());

            // When
            var result = pluController.Edit(pluModel) as ViewResult;

            // Then
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Edit_MappingAlreadyExists_ReturnViewResultNotNull()
        {
            // Given
            var pluController = new PluMappingController(
                mockLogger.Object, mockPluQuery.Object, mockGetItemQuery.Object, mockRemapQuery.Object, mockUpdatePlu.Object);
            PluMappingViewModel pluModel = new PluMappingViewModel
            {
                ItemId = 5,
                NationalPlu = "5555",
                flPLU = "1212"
            };

            List<BulkImportPluRemapModel> remapList = new List<BulkImportPluRemapModel>
            {
                new BulkImportPluRemapModel
                {
                    NewNationalPlu = "22771100000",
                    NewNationalPluId = 1,
                    CurrentNationalPlu = "22772200000",
                    CurrentNationalPluId = 2,
                    Region = "RM"
                }
            };

            mockRemapQuery.Setup(r => r.Search(It.IsAny<GetPluRemappingsParameters>())).Returns(remapList);

            // When
            var result = pluController.Edit(pluModel) as ViewResult;

            // Then
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Edit_MappingAlreadyExists_FailureViewDataMessageIsNotNull()
        {
            // Given
            var pluController = new PluMappingController(
                mockLogger.Object, mockPluQuery.Object, mockGetItemQuery.Object, mockRemapQuery.Object, mockUpdatePlu.Object);
            PluMappingViewModel pluModel = new PluMappingViewModel
            {
                ItemId = 5,
                NationalPlu = "5555",
                flPLU = "1212"
            };

            List<BulkImportPluRemapModel> remapList = new List<BulkImportPluRemapModel>
            {
                new BulkImportPluRemapModel
                {
                    NewNationalPlu = "22771100000",
                    NewNationalPluId = 1,
                    CurrentNationalPlu = "22772200000",
                    CurrentNationalPluId = 2,
                    Region = "RM"
                }
            };

            mockRemapQuery.Setup(r => r.Search(It.IsAny<GetPluRemappingsParameters>())).Returns(remapList);

            // When
            var result = pluController.Edit(pluModel) as ViewResult;

            // Then
            Assert.IsNotNull(result.ViewData["UpdateFailed"]);
        }

        [TestMethod]
        public void Edit_MappingAlreadyExists_UpdateDidNotExecute()
        {
            // Given
            var pluController = new PluMappingController(
                mockLogger.Object, mockPluQuery.Object, mockGetItemQuery.Object, mockRemapQuery.Object, mockUpdatePlu.Object);
            PluMappingViewModel pluModel = new PluMappingViewModel
            {
                ItemId = 5,
                NationalPlu = "5555",
                flPLU = "1212"
            };

            List<BulkImportPluRemapModel> remapList = new List<BulkImportPluRemapModel>
            {
                new BulkImportPluRemapModel
                {
                    NewNationalPlu = "22771100000",
                    NewNationalPluId = 1,
                    CurrentNationalPlu = "22772200000",
                    CurrentNationalPluId = 2,
                    Region = "RM"
                }
            };

            mockRemapQuery.Setup(r => r.Search(It.IsAny<GetPluRemappingsParameters>())).Returns(remapList);

            // When
            var result = pluController.Edit(pluModel) as ViewResult;

            // Then
            mockUpdatePlu.Verify(update => update.Execute(It.IsAny<UpdatePluCommand>()), Times.Never);
        }


        [TestMethod]
        public void Edit_RegionalPosPluDoesNotMatchNationalScalePluLength_ReturnViewResultNotNull()
        {
            // Given
            var pluController = new PluMappingController(
                mockLogger.Object, mockPluQuery.Object, mockGetItemQuery.Object, mockRemapQuery.Object, mockUpdatePlu.Object);
            List<BulkImportPluRemapModel> remapList = new List<BulkImportPluRemapModel>();
            PluMappingViewModel pluModel = new PluMappingViewModel
            {
                ItemId = 5,
                NationalPlu = "22778810000",
                nePLU = "1212"
            };

            mockRemapQuery.Setup(r => r.Search(It.IsAny<GetPluRemappingsParameters>())).Returns(remapList);

            // When
            var result = pluController.Edit(pluModel) as ViewResult;

            // Then
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Edit_RegionalPosPluDoesNotMatchNationalScalePluLength_UpdateDidNotExecute()
        {
            // Given
            var pluController = new PluMappingController(
                mockLogger.Object, mockPluQuery.Object, mockGetItemQuery.Object, mockRemapQuery.Object, mockUpdatePlu.Object);
            List<BulkImportPluRemapModel> remapList = new List<BulkImportPluRemapModel>();
            PluMappingViewModel pluModel = new PluMappingViewModel
            {
                ItemId = 5,
                NationalPlu = "22778810000",
                nePLU = "1212"
            };

            mockRemapQuery.Setup(r => r.Search(It.IsAny<GetPluRemappingsParameters>())).Returns(remapList);

            // When
            var result = pluController.Edit(pluModel) as ViewResult;

            // Then
            mockUpdatePlu.Verify(update => update.Execute(It.IsAny<UpdatePluCommand>()), Times.Never);
        }

        [TestMethod]
        public void Edit_RegionalPosPluDoesNotMatchNationalScalePluLength_FailureViewDataMessageIsNotNull()
        {
            // Given
            var pluController = new PluMappingController(
                mockLogger.Object, mockPluQuery.Object, mockGetItemQuery.Object, mockRemapQuery.Object, mockUpdatePlu.Object);
            List<BulkImportPluRemapModel> remapList = new List<BulkImportPluRemapModel>();
            PluMappingViewModel pluModel = new PluMappingViewModel
            {
                ItemId = 5,
                NationalPlu = "22778810000",
                nePLU = "1212"
            };

            mockRemapQuery.Setup(r => r.Search(It.IsAny<GetPluRemappingsParameters>())).Returns(remapList);

            // When
            var result = pluController.Edit(pluModel) as ViewResult;

            // Then
            Assert.IsNotNull(result.ViewData["UpdateFailed"]);
        }

        [TestMethod]
        public void Edit_RegionalPluWithLeadingZeros_LeadingZerosShouldBeTrimmed()
        {
            // Given
            var pluController = new PluMappingController(
                mockLogger.Object, mockPluQuery.Object, mockGetItemQuery.Object, mockRemapQuery.Object, mockUpdatePlu.Object);
            
            List<BulkImportPluRemapModel> remapList = new List<BulkImportPluRemapModel>();
            
            PluMappingViewModel pluModel = new PluMappingViewModel
            {
                ItemId = 5,
                NationalPlu = "6000",
                nePLU = "01212"
            };

            mockRemapQuery.Setup(r => r.Search(It.IsAny<GetPluRemappingsParameters>())).Returns(remapList);

            // When
            var result = pluController.Edit(pluModel) as ViewResult;
            var viewModel = result.Model as PluMappingViewModel;

            // Then
            Assert.AreEqual("1212", viewModel.nePLU);
        }

        private Item GetFakeItemPluMap()
        {
            Item item = new Item { itemID = 45612378 };

            ScanCode scanCode = new ScanCode { itemID = item.itemID, scanCodeID = 1, scanCode = "23999900000", scanCodeTypeID = 1 };
            ScanCodeType scanCodeType = new ScanCodeType { scanCodeTypeID = 1, scanCodeTypeDesc = "Scale PLU" };

            scanCode.ScanCodeType = scanCodeType;
            item.ScanCode.Add(scanCode);
            
            item.PLUMap = new PLUMap 
            { 
                itemID = item.itemID,
                flPLU = "23888800000",
                rmPLU = "24777700000",
                spPLU = "21333300000"
            };

            return item;
        }
    }
}
