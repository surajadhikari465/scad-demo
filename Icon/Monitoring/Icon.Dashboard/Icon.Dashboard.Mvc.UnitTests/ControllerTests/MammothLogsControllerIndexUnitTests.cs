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
using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Filters;

namespace Icon.Dashboard.Mvc.UnitTests.ControllerTests
{
    [TestClass]
    public class MammothLogsControllerIndexUnitTests : _MvcControllerUnitTestBase
    {
        public MammothLogsControllerIndexUnitTests() : base() { }

        protected override string testControllerName => "MammothLogs";
        protected override string testActionName => "Index";

        protected MammothLogsController ConstructController()
        {
            return ConstructController(
                base.authorizer,
                base.dashboardConfigManager,
                base.iconDbServiceWrapper,
                base.mammothDbServiceWrapper);
        }

        protected MammothLogsController ConstructController(
            IDashboardAuthorizer dashboardAuthorizer = null,
            IDashboardDataManager dashboardConfigManager = null,
            IIconDatabaseServiceWrapper iconDbServiceWrapper = null,
            IMammothDatabaseServiceWrapper mammothDbServiceWrapper = null)
        {
            var controller = new MammothLogsController(
                dashboardAuthorizer,
                dashboardConfigManager,
                iconDbServiceWrapper,
                mammothDbServiceWrapper);
            base.InitializeTestControllerContext(controller);
            return controller;
        }

        protected int SetupTestLoggingServiceForGetAnyApp(int page = 1, int pageSize = PagingConstants.DefaultPageSize)
        {
            var fakeData = serviceTestData.AppLogs.AllFakeMammothAppLogViewModels;
            int expectedCount = fakeData.Count;
            base.MockMammothLoggingServiceWrapper
                .Setup(s => s.GetPagedAppLogs(page, pageSize, LogErrorLevelEnum.Error))
                .Returns(fakeData);
            return fakeData.Count;
        }

        protected int SetupTestLoggingServiceForGetSingleApp(int page = 1, int pageSize = PagingConstants.DefaultPageSize)
        {
            var fakeApp = serviceTestData.AppLogs.FakeAppB;
            return SetupTestLoggingServiceForGetSingleApp(fakeApp, page, pageSize);
        }

        protected int SetupTestLoggingServiceForGetSingleApp(IApp fakeApp, int page = 1, int pageSize = PagingConstants.DefaultPageSize)
        {
            string name = fakeApp.AppName;
            base.MockMammothLoggingServiceWrapper
                .Setup(s => s.GetApp(name))
                .Returns(fakeApp);
            var fakeLogs = serviceTestData.AppLogs.AllFakeMammothAppLogViewModels
                .Where(al => al.AppName == name).ToList();
            base.MockMammothLoggingServiceWrapper
                .Setup(s => s.GetPagedAppLogsByApp(name, page, pageSize, LogErrorLevelEnum.Error))
                .Returns(fakeLogs);
            return fakeLogs.Count;
        }

        [TestMethod]
        public void MvcLogsController_IndexGetUnfiltered_ShouldSetModelPartialViewTitle()
        {
            // Arrange
            const string expectedTitle = "Mammoth Tst0 DB Log Viewer (All Mammoth Apps)";
            var controller = ConstructController();

            // Act
            ActionResult result = controller.Index();

            // Assert
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelResult = viewResult.Model as AppLogReportViewModel; ;
            Assert.AreEqual(expectedTitle, modelResult.PartialViewTitle);
        }

        [TestMethod]
        public void MvcLogsController_IndexGetFiltered_ShouldSetModelPartialViewTitle()
        {
            // Arrange
            var fakeApp = serviceTestData.AppLogs.FakeAppB;
            string name = fakeApp.AppName;
            string expectedTitle = "Mammoth Tst0 DB \"" + fakeApp.AppName + "\" Log Viewer";
            var controller = ConstructController();

            // Act
            ActionResult result = controller.Index(name);

            // Assert
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelResult = viewResult.Model as AppLogReportViewModel;
            Assert.AreEqual(expectedTitle, modelResult.PartialViewTitle);
        }

        [TestMethod]
        public void MvcLogsController_IndexGetUnfiltered_ShouldSetViewBagIdPaginationParameter()
        {
            // Arrange
            var controller = ConstructController();

            // Act
            ActionResult result = controller.Index();

            // Assert
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelResult = viewResult.Model as AppLogReportViewModel;
            Assert.IsNotNull(modelResult.PaginationModel);
        }

        [TestMethod]
        public void MvcLogsController_IndexGetUnfiltered_ShouldSetViewBagIdPaginationParameter_WithDefaultPagingValue()
        {
            // Arrange
            var controller = ConstructController();

            // Act
            ActionResult result = controller.Index();

            // Assert
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelResult = viewResult.Model as AppLogReportViewModel;
            Assert.IsNotNull(modelResult.PaginationModel);
            Assert.AreEqual(1, modelResult.PaginationModel.CurrentPage);
            Assert.AreEqual(PagingConstants.DefaultPageSize, modelResult.PaginationModel.PageSize);
        }

        [TestMethod]
        public void MvcLogsController_IndexGetFiltered_ShouldSetViewBagIdPaginationParameter_WithId()
        {
            // Arrange
            var fakeApp = serviceTestData.AppLogs.FakeAppA;
            string name = fakeApp.AppName;
            int expectedCount = SetupTestLoggingServiceForGetSingleApp(fakeApp);
            var controller = ConstructController();

            // Act
            ActionResult result = controller.Index(name);

            // Assert
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelResult = viewResult.Model as AppLogReportViewModel;
            Assert.AreEqual(expectedCount, modelResult.LogEntries.Count());
            Assert.IsNotNull(modelResult.PaginationModel);
            Assert.AreEqual(name, modelResult.PaginationModel.AppName);
        }

        [TestMethod]
        public void MvcLogsController_IndexGetFiltered_ShouldSetViewBagIdPaginationParameter_WithIdAndPagingValues()
        {
            // Arrange
            var fakeApp = serviceTestData.AppLogs.FakeAppB;
            string name = fakeApp.AppName;
            int page = 4;
            int pageSize = 100;
            int expectedCount = SetupTestLoggingServiceForGetSingleApp(page, pageSize);
            var controller = ConstructController();

            // Act
            ActionResult result = controller.Index(name, page, pageSize);

            // Assert
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelResult = viewResult.Model as AppLogReportViewModel;
            Assert.IsNotNull(modelResult.PaginationModel);
            Assert.AreEqual(name, modelResult.PaginationModel.AppName);
            Assert.AreEqual(page, modelResult.PaginationModel.CurrentPage);
            Assert.AreEqual(pageSize, modelResult.PaginationModel.PageSize);
        }

        [TestMethod]
        public void MvcLogsController_IndexGetUnfiltered_ShouldReturnViewResult()
        {
            // Arrange
            SetupTestLoggingServiceForGetAnyApp();
            var controller = ConstructController();

            // Act
            ActionResult result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void MvcLogsController_IndexGetFiltered_ShouldReturnViewResult()
        {
            // Arrange
            int expectedCount = SetupTestLoggingServiceForGetSingleApp();
            var controller = ConstructController();

            // Act
            ActionResult result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void MvcLogsController_IndexGetUnfiltered_ShouldReturnViewResultWithData()
        {
            // Arrange
            int expectedCount = SetupTestLoggingServiceForGetAnyApp();
            var controller = ConstructController();

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelResult = viewResult.Model as AppLogReportViewModel;
            Assert.AreEqual(expectedCount, modelResult.LogEntries.Count());
        }

        [TestMethod]
        public void MvcLogsController_IndexGetFiltered_ShouldReturnViewResultWithData()
        {
            // Arrange
            var fakeApp = serviceTestData.AppLogs.FakeAppB;
            string name = fakeApp.AppName;
            int expectedCount = SetupTestLoggingServiceForGetSingleApp(fakeApp);
            var controller = ConstructController();

            // Act
            var result = controller.Index(name);

            // Assert
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelResult = viewResult.Model as AppLogReportViewModel;
            Assert.AreEqual(expectedCount, modelResult.LogEntries.Count());
        }
    }
}
