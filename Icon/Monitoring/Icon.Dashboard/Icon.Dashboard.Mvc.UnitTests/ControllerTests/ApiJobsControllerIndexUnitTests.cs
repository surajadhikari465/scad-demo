using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Dashboard.Mvc.Controllers;
using System.Web.Mvc;
using System.Web;
using Icon.Dashboard.Mvc.Services;
using Moq;
using System.Collections.Generic;
using Icon.Dashboard.Mvc.ViewModels;
using System.Threading.Tasks;
using System.Linq;
using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.Mvc.Filters;

namespace Icon.Dashboard.Mvc.UnitTests.ControllerTests
{
    [TestClass]
    public class ApiJobsControllerIndexUnitTests : _MvcControllerUnitTestBase
    {
        public ApiJobsControllerIndexUnitTests() : base() { }

        private const string loggingDataServiceName = "loggingDataService";
        protected override string testControllerName => "ApiJobs";
        protected override string testActionName => "Index";

        protected ApiJobsController ConstructController()
        {
            return ConstructController(
                base.authorizer,
                base.dashboardConfigManager,
                base.iconDbServiceWrapper,
                base.mammothDbServiceWrapper);
        }

        protected ApiJobsController ConstructController(
            IDashboardAuthorizer dashboardAuthorizer = null,
            IDashboardDataManager dashboardConfigManager = null,
            IIconDatabaseServiceWrapper iconDbServiceWrapper = null,
            IMammothDatabaseServiceWrapper mammothDbServiceWrapper = null)
        {
            var controller = new ApiJobsController(
                dashboardAuthorizer,
                dashboardConfigManager,
                iconDbServiceWrapper,
                mammothDbServiceWrapper);
            base.InitializeTestControllerContext(controller);
            return controller;
        }

        protected int SetupTestLoggingServiceForGetAnyMessageType(int page = 1, int pageSize = PagingConstants.DefaultPageSize)
        {
            var fakeData = serviceTestData.ApiJob.ApiMessageJobSummaryViewModelList;
            int expectedCount = fakeData.Count;
            base.MockIconLoggingServiceWrapper
                .Setup(s => s.GetPagedApiJobSummaries(page, pageSize))
                .Returns(fakeData);
            return fakeData.Count;
        }


        protected int SetupTestLoggingServiceForGetSingleMessageType(int page = 1, int pageSize = PagingConstants.DefaultPageSize)
        {
            var fakeApp = serviceTestData.AppLogs.FakeAppB;
            return SetupTestLoggingServiceForGetSingleMessageType(IconApiControllerMessageType.Hierarchy, page, pageSize);
        }

        protected int SetupTestLoggingServiceForGetSingleMessageType(string messageType, int page = 1, int pageSize = PagingConstants.DefaultPageSize)
        {
            var fakeSummaries = serviceTestData.ApiJob.ApiMessageJobSummaryViewModelList.Where(s => s.MessageType == messageType).ToList();
            base.MockIconLoggingServiceWrapper
                .Setup(s => s.GetPagedApiJobSummariesByMessageType(messageType, page, pageSize)) 
                .Returns(fakeSummaries);
            return fakeSummaries.Count;
        }

        [TestMethod]
        public void MvcApiJobsController_IndexGetUnfiltered_ShouldSetViewTitle()
        {
            // Arrange
            var controller = ConstructController();

            // Act
            ActionResult result = controller.Index();

            // Assert
            const string expectedTitle = "Tst0 DB API Controller Message Jobs Summary";
            Assert.IsNotNull(controller.ViewBag.Title);
            Assert.IsInstanceOfType(controller.ViewBag.Title, typeof(string));
            Assert.AreEqual(expectedTitle, controller.ViewBag.Title.ToString());
        }

        [TestMethod]
        public void MvcApiJobsController_IndexGetFiltered_ShouldSetViewTitle()
        {
            // Arrange
            var messageType = IconApiControllerMessageType.Hierarchy;
            int expectedCount = SetupTestLoggingServiceForGetSingleMessageType(messageType);
            var controller = ConstructController();

            // Act
            ActionResult result = controller.Index(messageType);

            // Assert
            string expectedTitle = "Tst0 DB API Controller " + messageType.ToString() + " Message Jobs";
            Assert.IsNotNull(controller.ViewBag.Title);
            Assert.IsInstanceOfType(controller.ViewBag.Title, typeof(string));
            Assert.AreEqual(expectedTitle, controller.ViewBag.Title.ToString());
        }

        [TestMethod]
        public void MvcApiJobsController_IndexGetUnfiltered_ShouldSetViewBagIdPaginationParameter()
        {
            // Arrange
            var controller = ConstructController();

            // Act
            ActionResult result = controller.Index();

            // Assert
            Assert.IsNotNull(controller.ViewBag.PaginationPageSetViewModel);
            Assert.IsInstanceOfType(controller.ViewBag.PaginationPageSetViewModel, typeof(PaginationPageSetViewModel));
        }

        [TestMethod]
        public void MvcApiJobsController_IndexGetUnfiltered_ShouldSetViewBagIdPaginationParameter_WithDefaultPagingValue()
        {
            // Arrange
            var controller = ConstructController();

            // Act
            ActionResult result = controller.Index();

            // Assert
            Assert.IsNotNull(controller.ViewBag.PaginationPageSetViewModel);
            Assert.IsInstanceOfType(controller.ViewBag.PaginationPageSetViewModel, typeof(PaginationPageSetViewModel));
            var paginationModel = (PaginationPageSetViewModel)controller.ViewBag.PaginationPageSetViewModel;
            Assert.AreEqual(1, paginationModel.CurrentPage);
            Assert.AreEqual(PagingConstants.DefaultPageSize, paginationModel.PageSize);
        }

        [TestMethod]
        public void MvcApiJobsController_IndexGetFiltered_ShouldSetViewBagIdPaginationParameter_WithId()
        {
            // Arrange
            var messageType = IconApiControllerMessageType.Hierarchy;
            int expectedCount = SetupTestLoggingServiceForGetSingleMessageType(messageType);
            var controller = ConstructController();

            // Act
            ActionResult result = controller.Index(messageType);

            // Assert
            Assert.IsNotNull(controller.ViewBag.PaginationPageSetViewModel);
            Assert.IsInstanceOfType(controller.ViewBag.PaginationPageSetViewModel, typeof(PaginationPageSetViewModel));
            Assert.AreEqual(messageType.ToString(), ((PaginationPageSetViewModel)controller.ViewBag.PaginationPageSetViewModel).AppName);
        }

        [TestMethod]
        public void MvcApiJobsController_IndexGetFiltered_ShouldSetViewBagIdPaginationParameter_WithIdAndPagingValues()
        {
            // Arrange
            var messageType = IconApiControllerMessageType.Price;
            int page = 4;
            int pageSize = 100;
            int expectedCount = SetupTestLoggingServiceForGetSingleMessageType(page, pageSize);
            var controller = ConstructController();

            // Act
            ActionResult result = controller.Index(messageType, page, pageSize);

            // Assert
            Assert.IsNotNull(controller.ViewBag.PaginationPageSetViewModel);
            Assert.IsInstanceOfType(controller.ViewBag.PaginationPageSetViewModel, typeof(PaginationPageSetViewModel));
            Assert.AreEqual(messageType.ToString(), ((PaginationPageSetViewModel)controller.ViewBag.PaginationPageSetViewModel).AppName);
            Assert.AreEqual(page, ((PaginationPageSetViewModel)controller.ViewBag.PaginationPageSetViewModel).CurrentPage);
            Assert.AreEqual(pageSize, ((PaginationPageSetViewModel)controller.ViewBag.PaginationPageSetViewModel).PageSize);
        }

        [TestMethod]
        public void MvcApiJobsController_IndexGetUnfiltered_ShouldReturnViewResult()
        {
            // Arrange
            SetupTestLoggingServiceForGetAnyMessageType();
            var controller = ConstructController();

            // Act
            ActionResult result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void MvcApiJobsController_IndexGetFiltered_ShouldReturnViewResult()
        {
            // Arrange
            var messageType = IconApiControllerMessageType.Price;
            int expectedCount = SetupTestLoggingServiceForGetSingleMessageType(messageType);
            var controller = ConstructController();

            // Act
            ActionResult result = controller.Index(messageType);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void MvcApiJobsController_IndexGetUnfiltered_ShouldReturnViewResultWithData()
        {
            // Arrange
            int expectedCount = SetupTestLoggingServiceForGetAnyMessageType();
            var controller = ConstructController();

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelResult = viewResult.Model as ApiMessageJobReportViewModel;
            Assert.AreEqual(expectedCount, modelResult.JobSummaries.Count);
        }

        [TestMethod]
        public void MvcApiJobsController_IndexGetFiltered_ShouldReturnViewResultWithData()
        {
            // Arrange
            var messageType = IconApiControllerMessageType.Price;
            int expectedCount = SetupTestLoggingServiceForGetSingleMessageType(messageType);
            var controller = ConstructController();

            // Act
            var result = controller.Index(messageType);

            // Assert
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelResult = viewResult.Model as ApiMessageJobReportViewModel;
            Assert.AreEqual(expectedCount, modelResult.JobSummaries.Count);
        }
    }
}
