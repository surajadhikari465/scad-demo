using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using WebSupport.Controllers;
using Moq;
using System.Web.Mvc;
using Icon.Common.DataAccess;
using WebSupport.DataAccess.Queries;
using WebSupport.DataAccess.TransferObjects;
using WebSupport.Models;
using System.Web;
using WebSupport.Tests.TestData;
using WebSupport.ViewModels;
using System.Web.Routing;
using Esb.Core.EsbServices;
using System;
using WebSupport.DataAccess.Commands;
using Icon.Common;
using System.Text;

namespace WebSupport.Tests.Controllers
{
    [TestClass]
    public class CheckPointRequestControllerTest : Controller
    {
        protected Mock<ILogger> mockLogger = new Mock<ILogger>();
        protected Mock<IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>>> mockGetStoresQuery =
             new Mock<IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>>>();
        protected Mock<IEsbMultipleMessageService<CheckPointRequestViewModel>> mockCheckPointRequestMessageService =
            new Mock<IEsbMultipleMessageService<CheckPointRequestViewModel>>();
        protected Mock<ICommandHandler<ArchiveCheckpointMessageCommandParameters>> mockArchiveCheckpointMessageCommand =
            new Mock<ICommandHandler<ArchiveCheckpointMessageCommandParameters>>();
        protected Mock<HttpContextBase> mockHttpContext =
            new Mock<HttpContextBase>();
        protected PriceResetTestData testData = new PriceResetTestData();

        protected ILogger testLogger
        {
            get
            {
                return mockLogger.Object;
            }
        }

        protected IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>> getStoresQuery
        {
            get
            {
                return mockGetStoresQuery.Object;
            }
        }

        protected IEsbMultipleMessageService<CheckPointRequestViewModel> checkPointRequestMessageService
        {
            get
            {
                return mockCheckPointRequestMessageService.Object;
            }
        }

        protected void SetMockHttpContext(Controller controller)
        {
            controller.ControllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), controller);
        }

        [TestMethod]
        public void CheckPointRequestController_GetRequest_ShouldReturnViewResult()
        {
            // Arrange
            var controller = new CheckPointRequestController(testLogger, getStoresQuery, checkPointRequestMessageService, mockArchiveCheckpointMessageCommand.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void CheckPointRequestController_GetRequest_ShouldReturnViewResultWithRegionOptions()
        {
            //Arrange
            var expectedRegionsOptions = StaticData.WholeFoodsRegions.ToList();
            var controller = new CheckPointRequestController(testLogger, getStoresQuery, checkPointRequestMessageService, mockArchiveCheckpointMessageCommand.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewModelResult = (CheckPointRequestViewModel)((ViewResult)result).Model;
            Assert.AreEqual(expectedRegionsOptions.Count,
                viewModelResult.OptionsForRegion.Count());
        }

        [TestMethod]
        public void CheckPointRequestController_PostWithValidDataSingleStoreSingleItem_ShouldAttemptToSendMessage()
        {
            // Arrange
            var controller = new CheckPointRequestController(testLogger, getStoresQuery, checkPointRequestMessageService, mockArchiveCheckpointMessageCommand.Object);
            var postData = new CheckPointRequestViewModel()
            {
                RegionIndex = 0,
                Stores = new string[] { "10001" },
                ScanCodes = "4282342774"
            };
            this.mockCheckPointRequestMessageService.Setup(m => m.Send(It.IsAny<CheckPointRequestViewModel>()))
                .Returns(
                new List<EsbServiceResponse>
                {
                    new EsbServiceResponse { Status = EsbServiceResponseStatus.Sent }
                });

            // Act
            var result = controller.Index(postData);

            // Assert
            mockCheckPointRequestMessageService.Verify(s => s.Send(postData), Times.Once);
        }

        [TestMethod]
        public void CheckPointRequestController_PostWithValidDataMultipleStoresMultipleItems_ShouldAttemptToSendMessage()
        {
            // Arrange
            var controller = new CheckPointRequestController(testLogger, getStoresQuery, checkPointRequestMessageService, mockArchiveCheckpointMessageCommand.Object);
            var postData = new CheckPointRequestViewModel()
            {
                RegionIndex = 0,
                Stores = new string[] { "10001", "10010", "10100" },
                ScanCodes = string.Join(Environment.NewLine, "4282342774", "4282342775", "4282342776", "4282342777")
            };
            this.mockCheckPointRequestMessageService.Setup(m => m.Send(It.IsAny<CheckPointRequestViewModel>()))
                .Returns(
                new List<EsbServiceResponse>
                {
                    new EsbServiceResponse { Status = EsbServiceResponseStatus.Sent },
                    new EsbServiceResponse { Status = EsbServiceResponseStatus.Sent },
                    new EsbServiceResponse { Status = EsbServiceResponseStatus.Sent },
                    new EsbServiceResponse { Status = EsbServiceResponseStatus.Sent },
                    new EsbServiceResponse { Status = EsbServiceResponseStatus.Sent },
                    new EsbServiceResponse { Status = EsbServiceResponseStatus.Sent },
                    new EsbServiceResponse { Status = EsbServiceResponseStatus.Sent },
                    new EsbServiceResponse { Status = EsbServiceResponseStatus.Sent },
                    new EsbServiceResponse { Status = EsbServiceResponseStatus.Sent },
                    new EsbServiceResponse { Status = EsbServiceResponseStatus.Sent },
                    new EsbServiceResponse { Status = EsbServiceResponseStatus.Sent },
                    new EsbServiceResponse { Status = EsbServiceResponseStatus.Sent }
                });

            // Act
            var result = controller.Index(postData);

            // Assert
            mockCheckPointRequestMessageService.Verify(s => s.Send(postData), Times.Once);
        }

        [TestMethod]
        public void CheckPointRequestController_PostWithValidationError_ShouldNotAttemptToSendMessage()
        {
            // Arrange
            var controller = new CheckPointRequestController(testLogger, getStoresQuery, checkPointRequestMessageService, mockArchiveCheckpointMessageCommand.Object);
            var postData = new CheckPointRequestViewModel();
            controller.ViewData.ModelState.AddModelError("testKey", "my validation error");

            // Act
            var result = controller.Index(postData);

            // Assert
            mockCheckPointRequestMessageService.Verify(s => s.Send(postData), Times.Never);
        }

        [TestMethod]
        public void CheckPointRequestController_PostWithInvalidModel_SetsTempDataError()
        {
            // Arrange
            var controller = new CheckPointRequestController(testLogger, getStoresQuery, checkPointRequestMessageService, mockArchiveCheckpointMessageCommand.Object);
            var postData = new CheckPointRequestViewModel()
            {
                Stores = new string[] { "10001", "10010", "10100" },
                ScanCodes = string.Join(Environment.NewLine, "4282342774", "4282342775", "4282342776", "4282342777")
            };
            controller.ViewData.ModelState.AddModelError("testKey", "my validation error");
            var expectedErrorMsg = "Invalid ViewModel submitted for GPM Checkpoint Request. " +
                "Stores: 10001,10010,10100 ScanCodes: 4282342774,4282342775,4282342776,4282342777. " +
                "Model State Errors: my validation error";

            // Act
            var result = controller.Index(postData);

            // Assert
            var actualErrorMsg = controller.TempData[CheckPointRequestController.tempDataErrors];
            Assert.AreEqual(expectedErrorMsg, actualErrorMsg);
        }

        [TestMethod]
        public void CheckPointRequestController_PostWithInvalidModelWithNoStores_SetsTempDataError()
        {
            // Arrange
            var controller = new CheckPointRequestController(testLogger, getStoresQuery, checkPointRequestMessageService, mockArchiveCheckpointMessageCommand.Object);
            var postData = new CheckPointRequestViewModel()
            {
                Stores = null,
                ScanCodes = string.Join(Environment.NewLine, "4282342774", "4282342775", "4282342776", "4282342777")
            };
            controller.ViewData.ModelState.AddModelError("testKey", "my validation error");
            var expectedErrorMsg = "Invalid ViewModel submitted for GPM Checkpoint Request. " +
                "Stores: none ScanCodes: 4282342774,4282342775,4282342776,4282342777. " +
                "Model State Errors: my validation error";

            // Act
            var result = controller.Index(postData);

            // Assert
            var actualErrorMsg = controller.TempData[CheckPointRequestController.tempDataErrors];
            Assert.AreEqual(expectedErrorMsg, actualErrorMsg);
        }

        [TestMethod]
        public void CheckPointRequestController_PostWithInvalidModelWithNoScanCodes_SetsTempDataError()
        {
            // Arrange
            var controller = new CheckPointRequestController(testLogger, getStoresQuery, checkPointRequestMessageService, mockArchiveCheckpointMessageCommand.Object);
            var postData = new CheckPointRequestViewModel()
            {
                Stores = new string[] { "10001", "10010", "10100" },
                ScanCodes = null
            };
            controller.ViewData.ModelState.AddModelError("testKey", "my validation error");
            var expectedErrorMsg = "Invalid ViewModel submitted for GPM Checkpoint Request. " +
                "Stores: 10001,10010,10100 ScanCodes: none. " +
                "Model State Errors: my validation error";

            // Act
            var result = controller.Index(postData);

            // Assert
            var actualErrorMsg = controller.TempData[CheckPointRequestController.tempDataErrors];
            Assert.AreEqual(expectedErrorMsg, actualErrorMsg);
        }

        [TestMethod]
        public void CheckPointRequestController_PostWithTooMuchData_SetsTempDataError()
        {
            // Arrange
            var limit = AppSettingsAccessor.GetIntSetting(EsbConstants.CheckPointRequestMaxItemsKey, 100);
            var controller = new CheckPointRequestController(testLogger, getStoresQuery, checkPointRequestMessageService, mockArchiveCheckpointMessageCommand.Object);
            var storesForPost = new string[] { "10001", "10010", "10100" };
            var scanCodesForPost = new StringBuilder();
            for (int i = 0; i < limit / storesForPost.Length + 1; i++)
            {
                scanCodesForPost.Append($"{i + 1}{i + 1}{i + 1}{i + 1}{i + 1}");
                if (i < limit / 2)
                {
                    scanCodesForPost.Append(Environment.NewLine);
                }
            }
            var postData = new CheckPointRequestViewModel()
            {
                Stores = storesForPost,
                ScanCodes = scanCodesForPost.ToString()
            };
            var expectedErrorMsg = $"A maximum of {limit} GPM Checkpoint item-store requests can be made at once. " +
                $"The submitted data contained {postData.Stores.Length} Stores and {postData.ScanCodesList.Count} ScanCodes, " +
                $"which would result in {postData.Stores.Length * postData.ScanCodesList.Count} request messages if attempted. Please de-select some stores and/or items and then re-submit.";

            // Act
            var result = controller.Index(postData);

            // Assert
            var actualErrorMsg = controller.TempData[CheckPointRequestController.tempDataErrors];
            Assert.AreEqual(expectedErrorMsg, actualErrorMsg);
        }
    }
}
