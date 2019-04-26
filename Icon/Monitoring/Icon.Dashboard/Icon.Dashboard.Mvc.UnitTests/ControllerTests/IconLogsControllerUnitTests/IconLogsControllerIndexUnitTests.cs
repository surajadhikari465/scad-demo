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
using Icon.Dashboard.Mvc.Models;

namespace Icon.Dashboard.Mvc.UnitTests.ControllerTests
{
    [TestClass]
    public class LogsControllerIndexUnitTests : _MvcControllerUnitTestBase
    {
        public LogsControllerIndexUnitTests() : base() { }

        private const string loggingDataServiceName = "loggingDataService";

        protected IconLogsController ConstructController()
        {
            return ConstructController(base.iconDbServiceWrapper, base.mammothDbServiceWrapper);
        }

        protected IconLogsController ConstructController(IIconDatabaseServiceWrapper iconDbServiceWrapper = null,
            IMammothDatabaseServiceWrapper mammothDbServiceWrapper = null)
        {
            var controller = new IconLogsController(base.iconDbServiceWrapper);
            base.SetupMockHttpContext(controller, loggingDataServiceName, mockIconLoggingServiceWrapper);
            return controller;
        }

        protected int SetupTestLoggingServiceForGetAnyApp(int page = 1, int pageSize = PagingConstants.DefaultPageSize)
        {
            var fakeData = base.AllFakeIconAppLogViewModels;
            int expectedCount = fakeData.Count;
            base.mockIconLoggingServiceWrapper
                .Setup(s => s.GetPagedAppLogs(page, pageSize, LogErrorLevelEnum.Error))
                .Returns(fakeData);
            return fakeData.Count;
        }        

        protected int SetupTestLoggingServiceForGetSingleApp(int page=1, int pageSize = PagingConstants.DefaultPageSize)
        {
            var fakeApp = base.FakeAppB;
            return SetupTestLoggingServiceForGetSingleApp(fakeApp, page, pageSize);
        }

        protected int SetupTestLoggingServiceForGetSingleApp(IApp fakeApp, int page = 1, int pageSize = PagingConstants.DefaultPageSize)
        {
            string name = fakeApp.AppName;
            base.mockIconLoggingServiceWrapper
                .Setup(s => s.GetApp(name))
                .Returns(fakeApp);
            var fakeLogs = base.AllFakeIconAppLogViewModels.Where(al => al.AppName == name).ToList();
            base.mockIconLoggingServiceWrapper
                .Setup(s => s.GetPagedAppLogsByApp(name, page, pageSize, LogErrorLevelEnum.Error))
                .Returns(fakeLogs);
            return fakeLogs.Count;
        }

        [TestMethod]
        public void MvcLogsController_Index_GetUnfiltered_Should_Set_ViewBagTitle()
        {
            //Given
            const string expectedTitle = "ICON Dashboard Log Viewer (All Icon Apps)";
            var controller = ConstructController();

            //When
            ActionResult result = controller.Index();

            //Then
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelResult = viewResult.Model as IconLogEntryCollectionViewModel;;
            Assert.AreEqual(expectedTitle, modelResult.Title);
        }

        [TestMethod]
        public void MvcLogsController_Index_GetFiltered_Should_Set_ViewBagTitle()
        {
            //Given
            var fakeApp = base.FakeAppB;
            string name = fakeApp.AppName;
            string expectedTitle = fakeApp.AppName + " Log Viewer";
            var controller = ConstructController();

            //When
            ActionResult result = controller.Index(name);

            //Then
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelResult = viewResult.Model as IconLogEntryCollectionViewModel;
            Assert.AreEqual(expectedTitle, modelResult.Title);
        }

        [TestMethod]
        public void MvcLogsController_Index_GetUnfiltered_Should_Set_DataServicePropertyInHttpContextItems()
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
            Assert.IsInstanceOfType(controller.HttpContext.Items[loggingDataServiceName], typeof(Mock<IIconDatabaseServiceWrapper>));
        }

        [TestMethod]
        public void MvcLogsController_Index_GetFiltered_Should_Set_DataServicePropertyInHttpContextItems()
        {
            //Given
            var fakeApp = base.FakeAppB;
            string name = fakeApp.AppName;
            int expectedCount = SetupTestLoggingServiceForGetSingleApp(fakeApp);
            var controller = ConstructController();

            //When
            ActionResult result = controller.Index(name);

            //Then
            //should have set a property in the HttpContext.Items with a reference to the data service
            Assert.IsNotNull(controller.HttpContext.Items);
            Assert.IsTrue(controller.HttpContext.Items.Contains(loggingDataServiceName));
            Assert.IsNotNull(controller.HttpContext.Items[loggingDataServiceName]);
            Assert.IsInstanceOfType(controller.HttpContext.Items[loggingDataServiceName], typeof(Mock<IIconDatabaseServiceWrapper>));
        }

        [TestMethod]
        public void MvcLogsController_Index_GetUnfiltered_Should_Set_ViewBagIdPaginationParameter()
        {
            //Given
            var controller = ConstructController();

            //When
            ActionResult result = controller.Index();

            //Then
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelResult = viewResult.Model as IconLogEntryCollectionViewModel;
            Assert.IsNotNull(modelResult.PaginationModel);
        }

        [TestMethod]
        public void MvcLogsController_Index_GetUnfiltered_Should_Set_ViewBagIdPaginationParameter_WithDefaultPagingValue()
        {
            //Given
            var controller = ConstructController();

            //When
            ActionResult result = controller.Index();

            //Then
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelResult = viewResult.Model as IconLogEntryCollectionViewModel;
            Assert.IsNotNull(modelResult.PaginationModel);
            Assert.AreEqual(1, modelResult.PaginationModel.CurrentPage);
            Assert.AreEqual(PagingConstants.DefaultPageSize, modelResult.PaginationModel.PageSize);
        }

        [TestMethod]
        public void MvcLogsController_Index_GetFiltered_Should_Set_ViewBagIdPaginationParameter_WithId()
        {
            //Given
            var fakeApp = base.FakeAppA;
            string name = fakeApp.AppName;
            int expectedCount = SetupTestLoggingServiceForGetSingleApp(fakeApp);
            var controller = ConstructController();

            //When
            ActionResult result = controller.Index(name);

            //Then
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelResult = viewResult.Model as IconLogEntryCollectionViewModel;
            Assert.AreEqual(expectedCount, modelResult.LogEntries.Count());
            Assert.IsNotNull(modelResult.PaginationModel);
            Assert.AreEqual(name, modelResult.PaginationModel.AppName);
        }

        [TestMethod]
        public void MvcLogsController_Index_GetFiltered_Should_Set_ViewBagIdPaginationParameter_WithIdAndPagingValues()
        {
            //Given
            var fakeApp = base.FakeAppB;
            string name = fakeApp.AppName;
            int page = 4;
            int pageSize = 100;
            int expectedCount = SetupTestLoggingServiceForGetSingleApp(page, pageSize);
            var controller = ConstructController();

            //When
            ActionResult result = controller.Index(name, page, pageSize);

            //Then
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelResult = viewResult.Model as IconLogEntryCollectionViewModel;
            Assert.IsNotNull(modelResult.PaginationModel);
            Assert.AreEqual(name, modelResult.PaginationModel.AppName);
            Assert.AreEqual(page, modelResult.PaginationModel.CurrentPage);
            Assert.AreEqual(pageSize, modelResult.PaginationModel.PageSize);
        }

        [TestMethod]
        public void MvcLogsController_Index_GetUnfiltered_Should_ReturnViewResult()
        {
            //Given
            SetupTestLoggingServiceForGetAnyApp();
            var controller = ConstructController();

            //When
            ActionResult result = controller.Index();

            //Then
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void MvcLogsController_Index_GetFiltered_Should_ReturnViewResult()
        {
            //Given
            int expectedCount = SetupTestLoggingServiceForGetSingleApp();
            var controller = ConstructController();

            //When
            ActionResult result = controller.Index();

            //Then
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void MvcLogsController_Index_GetUnfiltered_Should_ReturnViewResultWithData()
        {
            //Given
            int expectedCount = SetupTestLoggingServiceForGetAnyApp();
            var controller = ConstructController();

            //When
            var result = controller.Index();

            //Then
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelResult = viewResult.Model as IconLogEntryCollectionViewModel;
            Assert.AreEqual(expectedCount, modelResult.LogEntries.Count());
        }

        [TestMethod]
        public void MvcLogsController_Index_GetFiltered_Should_ReturnViewResultWithData()
        {
            //Given
            var fakeApp = base.FakeAppB;
            string name = fakeApp.AppName;
            int expectedCount = SetupTestLoggingServiceForGetSingleApp(fakeApp);
            var controller = ConstructController();

            //When
            var result = controller.Index(name);

            //Then
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelResult = viewResult.Model as IconLogEntryCollectionViewModel;
            Assert.AreEqual(expectedCount, modelResult.LogEntries.Count());
        }
    }
}
