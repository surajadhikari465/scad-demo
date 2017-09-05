using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class PriceResetControllerTest
    {
        protected Mock<ILogger> mockLogger = new Mock<ILogger>();
        protected Mock<IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>>> mockGetStoresQuery =
             new Mock<IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>>>();
        protected Mock<IEsbService<PriceResetRequestViewModel>> mockMessageService =
            new Mock<IEsbService<PriceResetRequestViewModel>>();
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

        protected IEsbService<PriceResetRequestViewModel> messageService
        {
            get
            {
                return mockMessageService.Object;
            }
        }

        protected void SetMockHttpContext(Controller controller)
        {
            controller.ControllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), controller);
        }

        [TestMethod]
        public void PriceResetController_Index_Get_ShouldReturnViewResult()
        {
            // Arrange
            var controller = new PriceResetController(testLogger, getStoresQuery, messageService);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void PriceResetController_Index_Get_ShouldReturnViewResult_WithRegionOptions()
        {
            //Arrange
            var expectedRegionsOptions = StaticData.WholeFoodsRegions.ToList();
            var controller = new PriceResetController(testLogger, getStoresQuery, messageService);

            // Act
            var result = controller.Index();

            // Assert
            var viewModelResult = (PriceResetRequestViewModel)((ViewResult)result).Model;
            Assert.AreEqual(expectedRegionsOptions.Count,
                viewModelResult.OptionsForRegion.Count());
        }

        [TestMethod]
        public void PriceResetController_Index_Get_ShouldReturnViewResult_WithSystemOptions()
        {
            //Arrange
            var expectedDownstreamSystemsOptions = StaticData.DownstreamSystems.ToList();
            var controller = new PriceResetController(testLogger, getStoresQuery, messageService);

            // Act
            var result = controller.Index();

            // Assert
            var viewModelResult = (PriceResetRequestViewModel)((ViewResult)result).Model;
            Assert.AreEqual(expectedDownstreamSystemsOptions.Count,
                viewModelResult.OptionsForDestinationSystem.Count());
        }

        [TestMethod]
        public void PriceResetController_Index_Post_WithValidData_ShouldAttemptToSendMessage()
        {
            // Arrange
            var controller = new PriceResetController(testLogger, getStoresQuery, messageService);
            var postData = new PriceResetRequestViewModel()
            {
                RegionIndex = 0,
                DownstreamSystems = new int[] { 0, 1 },
                Stores = new string[] { "10001", "10010", "10100", "11000" },
                Items = PriceResetTestData.NewlineSeparatedScanCodes
            };
            this.mockMessageService.Setup(m => m.Send(It.IsAny<PriceResetRequestViewModel>()))
                .Returns(new EsbServiceResponse { Status = EsbServiceResponseStatus.Sent });

            // Act
            var result = controller.Index(postData);
            // Assert
            mockMessageService.Verify(s => s.Send(postData), Times.Once);
        }

        [TestMethod]
        public void PriceResetController_Index_Post_WithValidationError_ShouldNotAttemptToSendMessage()
        {
            // Arrange
            var controller = new PriceResetController(testLogger, getStoresQuery, messageService);
            var postData = new PriceResetRequestViewModel();
            //add a simulated model error
            controller.ViewData.ModelState.AddModelError("testKey", "my validation error");
            // Act
            var result = controller.Index(postData);
            // Assert
            mockMessageService.Verify(s => s.Send(postData), Times.Never);
        }

        [TestMethod]
        public void PriceResetController_Index_Post_WithValidationError_ShouldReturnDataAsChosenAtClient()
        {
            // Arrange
            var controller = new PriceResetController(testLogger, getStoresQuery, messageService);
            var expectedRegionIndex = 2;
            var expectedDownstreamSystems = new int[] { 7, 10 };
            var expectedStores = new string[] { "10001", "10010", "10100", "11000" };
            var expectedItems = PriceResetTestData.NewlineSeparatedScanCodes;
            var expectedStoreQueryResults = new List<StoreTransferObject>
            {
                new StoreTransferObject { BusinessUnit="10001", Name="test 1", Abbreviation="T1A"},
                new StoreTransferObject { BusinessUnit="10010", Name="test 2", Abbreviation="T2B"},
                new StoreTransferObject { BusinessUnit="10100", Name="test 3", Abbreviation="T3C"},
                new StoreTransferObject { BusinessUnit="11000", Name="test 4", Abbreviation="T4D"},
                new StoreTransferObject { BusinessUnit="11111", Name="test 5", Abbreviation="T5E"},
                new StoreTransferObject { BusinessUnit="10101", Name="test 6", Abbreviation="T6F"}
            };
            mockGetStoresQuery.Setup(q => q.Search(It.IsAny<GetStoresForRegionParameters>()))
                .Returns(expectedStoreQueryResults);
            var postData = new PriceResetRequestViewModel()
            {
                RegionIndex = expectedRegionIndex,
                DownstreamSystems = expectedDownstreamSystems,
                Stores = expectedStores,
                Items = expectedItems
            };
            controller.ViewData.ModelState.AddModelError("testKey", "my validation error");
            // Act
            var result = controller.Index(postData);
            // Assert
            var viewModelResult = (PriceResetRequestViewModel)((ViewResult)result).Model;
            Assert.IsNotNull(viewModelResult);
            Assert.AreEqual(expectedRegionIndex, viewModelResult.RegionIndex);
            Assert.AreEqual(expectedDownstreamSystems, viewModelResult.DownstreamSystems);
            Assert.AreEqual(expectedStores, viewModelResult.Stores);
            Assert.AreEqual(expectedItems, viewModelResult.Items);
        }

        [TestMethod]
        public void PriceResetController_Index_Post_WithInvalidData_ShouldRedirectToAvoidFormResubmitWarning()
        {
            // Arrange
            var controller = new PriceResetController(testLogger, getStoresQuery, messageService);
            var postData = new PriceResetRequestViewModel();
            controller.ModelState.AddModelError("testError", "testError");
            // Act
            var result = controller.Index(postData);
            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult));
            mockMessageService.Verify(m => m.Send(It.IsAny<PriceResetRequestViewModel>()), Times.Never);
        }

        [TestMethod]
        public void PriceResetController_Stores_Get_ShouldGetStoresBasedOnRegion()
        {
            // Arrange
            var testRegionIndex = 7;
            var controller = new PriceResetController(testLogger, getStoresQuery, messageService);
            var expectedStoreQueryResults = new List<StoreTransferObject>
            {
                new StoreTransferObject { BusinessUnit="10001", Name="test 1", Abbreviation="T1A"},
                new StoreTransferObject { BusinessUnit="10010", Name="test 2", Abbreviation="T2B"},
                new StoreTransferObject { BusinessUnit="10100", Name="test 3", Abbreviation="T3C"},
                new StoreTransferObject { BusinessUnit="11000", Name="test 4", Abbreviation="T4D"},
                new StoreTransferObject { BusinessUnit="11111", Name="test 5", Abbreviation="T5E"},
                new StoreTransferObject { BusinessUnit="10101", Name="test 6", Abbreviation="T6F"}
            };
            mockGetStoresQuery.Setup(q => q.Search(It.IsAny<GetStoresForRegionParameters>()))
                .Returns(expectedStoreQueryResults);
            // Act
            var result = controller.Stores(testRegionIndex);
            var jsonData = ((JsonResult)result).Data;
            // Assert
            Assert.IsInstanceOfType(jsonData, typeof(IEnumerable<StoreTransferObject>));
            Assert.AreEqual(expectedStoreQueryResults, (jsonData as IEnumerable<StoreTransferObject>));
            Assert.AreEqual(expectedStoreQueryResults.Count, (jsonData as IEnumerable<StoreTransferObject>).Count());
        }

        //private bool TestCallback(GetStoresForRegionParameters queryParam)
        //{
        //    return false;
        //}

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PriceResetController_Stores_Get_WhenRegionCodeIsInvalid_ShouldThrowArgumentException()
        {
            // Arrange
            var regionCode = 88;
            var controller = new PriceResetController(testLogger, getStoresQuery, messageService);
            // Act
            var result = controller.Stores(regionCode);
            // Assert
            //should have thrown expected exception
        }
    }
}
