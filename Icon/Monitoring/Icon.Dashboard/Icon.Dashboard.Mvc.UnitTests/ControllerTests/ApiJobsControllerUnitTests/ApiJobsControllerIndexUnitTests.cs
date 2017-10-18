using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Dashboard.Mvc.Controllers;
using System.Web.Mvc;
using System.Web;
using Icon.Dashboard.Mvc.Services;
using Moq;
using System.Collections.Generic;
using Icon.Dashboard.Mvc.ViewModels;
using Icon.Dashboard.DataFileAccess.Models;
using System.Threading.Tasks;
using System.Linq;
using Icon.Dashboard.CommonDatabaseAccess;

namespace Icon.Dashboard.Mvc.UnitTests.ControllerTests
{
    [TestClass]
    public class ApiJobsControllerIndexUnitTests : _MvcControllerUnitTestBase
    {
        public ApiJobsControllerIndexUnitTests() : base() { }

        private const string loggingDataServiceName = "loggingDataService";

        protected ApiJobsController ConstructController()
        {
            return ConstructController(base.loggingServiceWrapper, base.serverUtility);
        }

        protected ApiJobsController ConstructController(IIconDatabaseServiceWrapper loggingServiceWrapper = null,
            HttpServerUtilityBase serverUtility = null)
        {
            var controller = new ApiJobsController(base.serverUtility, base.loggingServiceWrapper);
            base.SetupMockHttpContext(controller, loggingDataServiceName, mockIconLoggingServiceWrapper);
            return controller;
        }

        protected int SetupTestLoggingServiceForGetAnyMessageType(int page = 1, int pageSize = PagingConstants.DefaultPageSize)
        {
            var fakeData = base.AllFakeJobSummaryViewModels;
            int expectedCount = fakeData.Count;
            base.mockIconLoggingServiceWrapper
                .Setup(s => s.GetPagedApiJobSummaries(page, pageSize))
                .Returns(fakeData);
            return fakeData.Count;
        }


        protected int SetupTestLoggingServiceForGetSingleMessageType(int page = 1, int pageSize = PagingConstants.DefaultPageSize)
        {
            var fakeApp = base.FakeAppB;
            return SetupTestLoggingServiceForGetSingleMessageType(IconApiControllerMessageType.Hierarchy, page, pageSize);
        }

        protected int SetupTestLoggingServiceForGetSingleMessageType(string messageType, int page = 1, int pageSize = PagingConstants.DefaultPageSize)
        {
            var fakeSummaries = base.AllFakeJobSummaryViewModels.Where(s => s.MessageType == messageType).ToList();
            base.mockIconLoggingServiceWrapper
                .Setup(s => s.GetPagedApiJobSummariesByMessageType(messageType, page, pageSize)) 
                .Returns(fakeSummaries);
            return fakeSummaries.Count;
        }

        [TestMethod]
        public void MvcApiJobsController_Index_GetUnfiltered_Should_Set_ViewBagTitle()
        {
            //Given
            var controller = ConstructController();

            //When
            ActionResult result = controller.Index();

            //Then
            const string expectedTitle = "API Controller Message Jobs Summary";
            Assert.IsNotNull(controller.ViewBag.Title);
            Assert.IsInstanceOfType(controller.ViewBag.Title, typeof(string));
            Assert.AreEqual(expectedTitle, controller.ViewBag.Title.ToString());
        }

        [TestMethod]
        public void MvcApiJobsController_Index_GetFiltered_Should_Set_ViewBagTitle()
        {
            //Given
            var messageType = IconApiControllerMessageType.Hierarchy;
            int expectedCount = SetupTestLoggingServiceForGetSingleMessageType(messageType);
            var controller = ConstructController();

            //When
            ActionResult result = controller.Index(messageType);

            //Then
            string expectedTitle = "API Controller " + messageType.ToString() + " Message Jobs";
            Assert.IsNotNull(controller.ViewBag.Title);
            Assert.IsInstanceOfType(controller.ViewBag.Title, typeof(string));
            Assert.AreEqual(expectedTitle, controller.ViewBag.Title.ToString());
        }

        [TestMethod]
        public void MvcApiJobsController_Index_GetUnfiltered_Should_Set_DataServicePropertyInHttpContextItems()
        {
            //Given
            var controller = ConstructController();

            //When
            ActionResult result = controller.Index();

            //Then
            //should have set a property in the HttpContext.Items with a reference to the data service
            Assert.IsNotNull(controller.HttpContext.Items);
            Assert.IsTrue(controller.HttpContext.Items.Contains(loggingDataServiceName));
            Assert.IsNotNull(controller.HttpContext.Items[loggingDataServiceName]);
            Assert.IsInstanceOfType(controller.HttpContext.Items[loggingDataServiceName], typeof(IIconDatabaseServiceWrapper));
        }

        [TestMethod]
        public void MvcApiJobsController_Index_GetFiltered_Should_Set_DataServicePropertyInHttpContextItems()
        {
            //Given
            var messageType = IconApiControllerMessageType.Hierarchy;
            int expectedCount = SetupTestLoggingServiceForGetSingleMessageType(messageType);
            var controller = ConstructController();

            //When
            ActionResult result = controller.Index(messageType);

            //Then
            //should have set a property in the HttpContext.Items with a reference to the data service
            Assert.IsNotNull(controller.HttpContext.Items);
            Assert.IsTrue(controller.HttpContext.Items.Contains(loggingDataServiceName));
            Assert.IsNotNull(controller.HttpContext.Items[loggingDataServiceName]);
            Assert.IsInstanceOfType(controller.HttpContext.Items[loggingDataServiceName], typeof(IIconDatabaseServiceWrapper));
        }

        [TestMethod]
        public void MvcApiJobsController_Index_GetUnfiltered_Should_Set_ViewBagIdPaginationParameter()
        {
            //Given
            var controller = ConstructController();

            //When
            ActionResult result = controller.Index();

            //Then
            Assert.IsNotNull(controller.ViewBag.PaginationPageSetViewModel);
            Assert.IsInstanceOfType(controller.ViewBag.PaginationPageSetViewModel, typeof(PaginationPageSetViewModel));
        }

        [TestMethod]
        public void MvcApiJobsController_Index_GetUnfiltered_Should_Set_ViewBagIdPaginationParameter_WithDefaultPagingValue()
        {
            //Given
            var controller = ConstructController();

            //When
            ActionResult result = controller.Index();

            //Then
            Assert.IsNotNull(controller.ViewBag.PaginationPageSetViewModel);
            Assert.IsInstanceOfType(controller.ViewBag.PaginationPageSetViewModel, typeof(PaginationPageSetViewModel));
            var paginationModel = (PaginationPageSetViewModel)controller.ViewBag.PaginationPageSetViewModel;
            Assert.AreEqual(1, paginationModel.CurrentPage);
            Assert.AreEqual(PagingConstants.DefaultPageSize, paginationModel.PageSize);
        }

        [TestMethod]
        public void MvcApiJobsController_Index_GetFiltered_Should_Set_ViewBagIdPaginationParameter_WithId()
        {
            //Given
            var messageType = IconApiControllerMessageType.Hierarchy;
            int expectedCount = SetupTestLoggingServiceForGetSingleMessageType(messageType);
            var controller = ConstructController();

            //When
            ActionResult result = controller.Index(messageType);

            //Then
            Assert.IsNotNull(controller.ViewBag.PaginationPageSetViewModel);
            Assert.IsInstanceOfType(controller.ViewBag.PaginationPageSetViewModel, typeof(PaginationPageSetViewModel));
            Assert.AreEqual(messageType.ToString(), ((PaginationPageSetViewModel)controller.ViewBag.PaginationPageSetViewModel).RouteParameter);
        }

        [TestMethod]
        public void MvcApiJobsController_Index_GetFiltered_Should_Set_ViewBagIdPaginationParameter_WithIdAndPagingValues()
        {
            //Given
            var messageType = IconApiControllerMessageType.Price;
            int page = 4;
            int pageSize = 100;
            int expectedCount = SetupTestLoggingServiceForGetSingleMessageType(page, pageSize);
            var controller = ConstructController();

            //When
            ActionResult result = controller.Index(messageType, page, pageSize);

            //Then
            Assert.IsNotNull(controller.ViewBag.PaginationPageSetViewModel);
            Assert.IsInstanceOfType(controller.ViewBag.PaginationPageSetViewModel, typeof(PaginationPageSetViewModel));
            Assert.AreEqual(messageType.ToString(), ((PaginationPageSetViewModel)controller.ViewBag.PaginationPageSetViewModel).RouteParameter);
            Assert.AreEqual(page, ((PaginationPageSetViewModel)controller.ViewBag.PaginationPageSetViewModel).CurrentPage);
            Assert.AreEqual(pageSize, ((PaginationPageSetViewModel)controller.ViewBag.PaginationPageSetViewModel).PageSize);
        }

        [TestMethod]
        public void MvcApiJobsController_Index_GetUnfiltered_Should_ReturnViewResult()
        {
            //Given
            SetupTestLoggingServiceForGetAnyMessageType();
            var controller = ConstructController();

            //When
            ActionResult result = controller.Index();

            //Then
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void MvcApiJobsController_Index_GetFiltered_Should_ReturnViewResult()
        {
            //Given
            var messageType = IconApiControllerMessageType.Price;
            int expectedCount = SetupTestLoggingServiceForGetSingleMessageType(messageType);
            var controller = ConstructController();

            //When
            ActionResult result = controller.Index(messageType);

            //Then
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void MvcApiJobsController_Index_GetUnfiltered_Should_ReturnViewResultWithData()
        {
            //Given
            int expectedCount = SetupTestLoggingServiceForGetAnyMessageType();
            var controller = ConstructController();

            //When
            var result = controller.Index();

            //Then
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelResult = viewResult.Model as List<ApiMessageJobSummaryViewModel>;
            Assert.AreEqual(expectedCount, modelResult.Count);
        }

        [TestMethod]
        public void MvcApiJobsController_Index_GetFiltered_Should_ReturnViewResultWithData()
        {
            //Given
            var messageType = IconApiControllerMessageType.Price;
            int expectedCount = SetupTestLoggingServiceForGetSingleMessageType(messageType);
            var controller = ConstructController();

            //When
            var result = controller.Index(messageType);

            //Then
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelResult = viewResult.Model as List<ApiMessageJobSummaryViewModel>;
            Assert.AreEqual(expectedCount, modelResult.Count);
        }
    }
}
