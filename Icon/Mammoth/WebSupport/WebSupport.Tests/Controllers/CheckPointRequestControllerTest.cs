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

namespace WebSupport.Tests.Controllers
{
    [TestClass]
    public class CheckPointRequestControllerTest : Controller
    {
        protected Mock<ILogger> mockLogger = new Mock<ILogger>();
        protected Mock<IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>>> mockGetStoresQuery =
             new Mock<IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>>>();
        protected Mock<IEsbService<CheckPointRequestViewModel>> mockCheckPointRequestMessageService =
            new Mock<IEsbService<CheckPointRequestViewModel>>();
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

        protected IEsbService<CheckPointRequestViewModel> checkPointRequestMessageService
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
            var controller = new CheckPointRequestController(testLogger, getStoresQuery, checkPointRequestMessageService);

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
            var controller = new CheckPointRequestController(testLogger, getStoresQuery, checkPointRequestMessageService);

            // Act
            var result = controller.Index();

            // Assert
            var viewModelResult = (CheckPointRequestViewModel)((ViewResult)result).Model;
            Assert.AreEqual(expectedRegionsOptions.Count,
                viewModelResult.OptionsForRegion.Count());
        }

        [TestMethod]
        public void CheckPointRequestController_PostWithValidData_ShouldAttemptToSendMessage()
        {
            // Arrange
            var controller = new CheckPointRequestController(testLogger, getStoresQuery, checkPointRequestMessageService);
            var postData = new CheckPointRequestViewModel()
            {
                RegionIndex = 0,
                Store = "10001",
                ScanCode = "4282342774"
            };
            this.mockCheckPointRequestMessageService.Setup(m => m.Send(It.IsAny<CheckPointRequestViewModel>()))
                .Returns(new EsbServiceResponse { Status = EsbServiceResponseStatus.Sent });

            // Act
            var result = controller.Index(postData);

            // Assert
            mockCheckPointRequestMessageService.Verify(s => s.Send(postData), Times.Once);
        }

        [TestMethod]
        public void CheckPointRequestController_PostWithValidationError_ShouldNotAttemptToSendMessage()
        {
            // Arrange
            var controller = new CheckPointRequestController(testLogger, getStoresQuery, checkPointRequestMessageService);
            var postData = new CheckPointRequestViewModel();
            controller.ViewData.ModelState.AddModelError("testKey", "my validation error");

            // Act
            var result = controller.Index(postData);

            // Assert
            mockCheckPointRequestMessageService.Verify(s => s.Send(postData), Times.Never);
        }
    }
}
