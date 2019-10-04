using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.MammothDatabaseAccess;
using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Services;
using Icon.Dashboard.Mvc.UnitTests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.Mvc.UnitTests.ServiceTests
{
    using AppLogFilterType = Expression<Func<IAppLog, bool>>; 

    [TestClass]
    public class MammothDbServiceWrapperUnitTests
    {
        MammothDatabaseServiceWrapper dbWrapperService = null;
        Mock<IMammothDatabaseService> mockDataService = new Mock<IMammothDatabaseService>();
        DbServiceTestData dbTestData = new DbServiceTestData(DateTime.Now);
        int DefaultPage = 1;
        int DefaultPageSize = 20;

        [TestInitialize]
        public void TestInitialize()
        {
            dbWrapperService = new MammothDatabaseServiceWrapper(mockDataService.Object);
        }

        private AppLogFilterType GetDefaultFilterForAppAndErrorLevel(int appID, LogErrorLevelEnum errorLevelEnum)
        {
            return (appLog) =>
                appLog.AppID == appID && appLog.Level == errorLevelEnum.ToString();
        }

        private AppLogFilterType GetDefaultFilterForErrorLevel(LogErrorLevelEnum errorLevelEnum)
        {
            return (appLog) =>
                appLog.Level == errorLevelEnum.ToString();
        }

        [TestMethod]
        public void GetAppByAppName_WhenAppNameNull_DoesNotCallDataService()
        {
            // Arrange
            string appName = null;
            // Act
            dbWrapperService.GetApp(appName);
            // Assert
            mockDataService.Verify(s => s.GetApp(appName), Times.Never);
        }

        [TestMethod]
        public void GetAppByAppName_WhenAppNameEmpty_DoesNotCallDataService()
        {
            // Arrange
            string appName = "";
            // Act
            dbWrapperService.GetApp(appName);
            // Assert
            mockDataService.Verify(s => s.GetApp(appName), Times.Never);
        }

        [TestMethod]
        public void GetAppByAppName_WhenAppNameWhiteSpace_DoesNotCallDataService()
        {
            // Arrange
            string appName = "\t ";
            // Act
            dbWrapperService.GetApp(appName);
            // Assert
            mockDataService.Verify(s => s.GetApp(appName), Times.Never);
        }

        [TestMethod]
        public void GetAppByAppName_WhenAppNameHasValue_CallsDataServiceWithAppName()
        {
            // Arrange
            string appName = "my app1";
            // Act
            dbWrapperService.GetApp(appName);
            // Assert
            mockDataService.Verify(s => s.GetApp(appName), Times.Once);
        }

        [TestMethod]
        public void GetAppByAppId_WhenAppIdIsNegative_DoesNotCallDataService()
        {
            // Arrange
            int appID = -1;
            // Act
            dbWrapperService.GetApp(appID);
            // Assert
            mockDataService.Verify(s => s.GetApp(appID), Times.Never);
        }

        [TestMethod]
        public void GetAppByAppId_WhenAppIdIsZero_DoesNotCallDataService()
        {
            // Arrange
            int appID = 0;
            // Act
            dbWrapperService.GetApp(appID);
            // Assert
            mockDataService.Verify(s => s.GetApp(appID), Times.Never);
        }

        [TestMethod]
        public void GetAppByAppId_WhenAppIdHasValue_CallsDataServiceWithAppID()
        {
            // Arrange
            int appID = 26;
            // Act
            dbWrapperService.GetApp(appID);
            // Assert
            mockDataService.Verify(s => s.GetApp(appID), Times.Once);
        }

        [TestMethod]
        public void GetAppID_WhenAppNameNull_DoesNotCallDataService()
        {
            // Arrange
            string appName = null;
            // Act
            dbWrapperService.GetAppID(appName);
            // Assert
            mockDataService.Verify(s => s.GetApp(appName), Times.Never);
        }

        [TestMethod]
        public void GetAppID_WhenAppNameEmpty_DoesNotCallDataService()
        {
            // Arrange
            string appName = "";
            // Act
            dbWrapperService.GetAppID(appName);
            // Assert
            mockDataService.Verify(s => s.GetApp(appName), Times.Never);
        }

        [TestMethod]
        public void GetAppID_WhenAppNameWhiteSpace_DoesNotCallDataService()
        {
            // Arrange
            string appName = "\t ";
            // Act
            dbWrapperService.GetAppID(appName);
            // Assert
            mockDataService.Verify(s => s.GetApp(appName), Times.Never);
        }

        [TestMethod]
        public void GetAppID_WhenAppNameNull_ReturnsNegative()
        {
            // Arrange
            string appName = null;
            int expectedIdValue = -1;
            // Act
            var actualIdValue = dbWrapperService.GetAppID(appName);
            // Assert
            Assert.AreEqual(expectedIdValue, actualIdValue);
        }

        [TestMethod]
        public void GetAppID_WhenAppNameEmpty_ReturnsNegative()
        {
            // Arrange
            string appName = "";
            int expectedIdValue = -1;
            // Act
            var actualIdValue = dbWrapperService.GetAppID(appName);
            // Assert
            Assert.AreEqual(expectedIdValue, actualIdValue);
        }

        [TestMethod]
        public void GetAppID_WhenAppNameWhiteSpace_ReturnsNegative()
        {
            // Arrange
            string appName = "\t ";
            int expectedIdValue = -1;
            // Act
            var actualIdValue = dbWrapperService.GetAppID(appName);
            // Assert
            Assert.AreEqual(expectedIdValue, actualIdValue);
        }

        [TestMethod]
        public void GetAppID_WhenAppNameHasValue_CallsDataServiceWithAppName()
        {
            // Arrange
            string appName = "TestAppName";
            // Act
            var actualIdValue = dbWrapperService.GetAppID(appName);
            // Assert
            mockDataService.Verify(s => s.GetApp(appName), Times.Once);
        }

        [TestMethod]
        public void GetAppID_WhenAppNotFound_ReturnsNegative()
        {
            // Arrange
            string appName = "TestAppName";
            int expectedIdValue = -1;
            mockDataService.Setup(m => m.GetApp(appName)).Returns((IApp)null);
            // Act
            var actualIdValue = dbWrapperService.GetAppID(appName);
            // Assert
            Assert.AreEqual(expectedIdValue, actualIdValue);
        }

        [TestMethod]
        public void GetAppID_WhenAppFound_ReturnsAppId()
        {
            // Arrange
            var app = new FakeApp(7, "My.Service");
            mockDataService.Setup(m => m.GetApp(app.AppName))
                .Returns(app);
            // Act
            var actualIdValue = dbWrapperService.GetAppID(app.AppName);
            // Assert
            Assert.AreEqual(app.AppID, actualIdValue);
        }

        [TestMethod]
        public void GetApps_CallsDataService()
        {
            // Arrange
            // Act
            dbWrapperService.GetApps();
            // Assert
            mockDataService.Verify(s => s.GetApps(), Times.Once);
        }

        [TestMethod]
        public void GetApps_WhenNoDataFound_ReturnsEmptyList()
        {
            // Arrange
            IEnumerable<IApp> appData = null;
            mockDataService.Setup(m => m.GetApps()).Returns(appData);
            // Act
            var results = dbWrapperService.GetApps();
            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void GetApps_WhenDataFound_ReturnsExpectedList()
        {
            // Arrange
            var expectedCount = dbTestData.MammothApps.Count;
            mockDataService.Setup(m => m.GetApps()).Returns(dbTestData.MammothApps);
            // Act
            var results = dbWrapperService.GetApps();
            // Assert
            Assert.AreEqual(expectedCount, results.Count);
        }

        [TestMethod]
        public void GetPagedAppLogsByApp_WhenAppNameNull_DoesNotCallDataService()
        {
            // Arrange
            string appName = null;
            // Act
            var results = dbWrapperService.GetPagedAppLogsByApp(appName);
            // Assert
            mockDataService.Verify(m => m.GetApp(appName), Times.Never);
        }

        [TestMethod]
        public void GetPagedAppLogsByApp_WhenAppNameWhitespace_DoesNotCallDataService()
        {
            // Arrange
            string appName = "\r\n";
            // Act
            var results = dbWrapperService.GetPagedAppLogsByApp(appName);
            // Assert
            mockDataService.Verify(m => m.GetApp(appName), Times.Never);
        }

        [TestMethod]
        public void GetPagedAppLogsByApp_WhenAppNameWhitespace_ReturnsEmptyList()
        {
            // Arrange
            string appName = "\t ";
            // Act
            var results = dbWrapperService.GetPagedAppLogsByApp(appName);
            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void GetPagedAppLogsByApp_NoFilterNorPagingParams_CallsDataServiceToGetAppId()
        {
            // Arrange
            var app = new FakeApp(7, "My.Service");
            mockDataService.Setup(m => m.GetApp(app.AppName))
                .Returns(app);
            // Act
            var results = dbWrapperService.GetPagedAppLogsByApp(app.AppName);
            // Assert
            mockDataService.Verify(m => m.GetApp(app.AppName), Times.Once);
        }

        [TestMethod]
        public void GetPagedAppLogsByApp_NoFilterNorPagingParams_CallsDataService_WithDescendingSortOrder()
        {
            // Arrange
            var expectedSortOrder = QuerySortOrder.Descending;
            var app = new FakeApp(7, "My.Service");
            mockDataService.Setup(m => m.GetApp(app.AppName))
                .Returns(app);
            // Act
            var results = dbWrapperService.GetPagedAppLogsByApp(app.AppName);
            // Assert
            mockDataService.Verify(m => m
                .GetPagedAppLogsWithFilter(
                    It.IsAny<AppLogFilterType>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    expectedSortOrder),
                Times.Once);
        }

        [TestMethod]
        public void GetPagedAppLogsByApp_NoFilterNorPagingParams_CallsDataService_WithDefaultPagingValues()
        {
            // Arrange
            var app = new FakeApp(7, "My.Service");
            mockDataService.Setup(m => m.GetApp(app.AppName))
                .Returns(app);
            // Act
            var results = dbWrapperService.GetPagedAppLogsByApp(app.AppName);
            // Assert
            mockDataService.Verify(m => m
                .GetPagedAppLogsWithFilter(
                    It.IsAny<AppLogFilterType>(),
                    this.DefaultPage,
                    this.DefaultPageSize,
                    It.IsAny<QuerySortOrder>()),
                Times.Once);
        }

        [TestMethod]
        public void GetPagedAppLogsByApp_NoFilterWithPagingParams_CallsDataService_WithExpectedPagingValues()
        {
            // Arrange
            var app = new FakeApp(7, "My.Service");
            var page = 3;
            var pageSize = 15;
            mockDataService.Setup(m => m.GetApp(app.AppName))
                .Returns(app);
            // Act
            var results = dbWrapperService.GetPagedAppLogsByApp(app.AppName, page, pageSize);
            // Assert
            mockDataService.Verify(m => m
                .GetPagedAppLogsWithFilter(
                    It.IsAny<AppLogFilterType>(),
                    page,
                    pageSize,
                    It.IsAny<QuerySortOrder>()),
                Times.Once);
        }

        [TestMethod]
        public void GetPagedAppLogsByApp_NoFilterNorPagingParams_CallsDataService_WithFilterForAppNameAndDefaultErrorLevel()
        {
            // Arrange
            var app = new FakeApp(7, "My.Service");
            mockDataService.Setup(m => m.GetApp(app.AppName))
                .Returns(app);
            var dataForFilterComparer = dbTestData
                .GetAppLogDataForFilterTest(app.AppID, app.AppName, LogErrorLevelEnum.Error);
            // Act
            var results = dbWrapperService.GetPagedAppLogsByApp(app.AppName);
            // Assert
            mockDataService.Verify(m => m
                .GetPagedAppLogsWithFilter(
                    It.Is<Expression<Func<IAppLog, bool>>>(e =>
                         CustomAsserts.FilterResultComparer(e, dataForFilterComparer.ToArray())),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<QuerySortOrder>()),
                Times.Once);
        }

        [TestMethod]
        public void GetPagedAppLogsByApp_NoFilterNorPagingParams_WhenNullData_ReturnsEmptyList()
        {
            // Arrange
            var app = new FakeApp(7, "My.Service");
            mockDataService.Setup(m => m.GetApp(app.AppName))
                .Returns(app);
            mockDataService.Setup(m => m.GetPagedAppLogsWithFilter(It.IsAny<AppLogFilterType>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<QuerySortOrder>()))
                .Returns((IEnumerable<IAppLog>)null);
            // Act
            var results = dbWrapperService.GetPagedAppLogsByApp(app.AppName);
            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void GetPagedAppLogsByApp_NoFilterNorPagingParams_WhenEmptyData_ReturnsEmptyList()
        {
            // Arrange
            var app = new FakeApp(7, "My.Service");
            mockDataService.Setup(m => m.GetApp(app.AppName))
                .Returns(app);
            mockDataService.Setup(m => m.GetPagedAppLogsWithFilter(It.IsAny<AppLogFilterType>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<QuerySortOrder>()))
                .Returns(new List<FakeAppLog>());
            // Act
            var results = dbWrapperService.GetPagedAppLogsByApp(app.AppName);
            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void GetPagedAppLogsByApp_NoFilterNorPagingParams_WhenDataFound_ReturnsExpectedList()
        {
            // Arrange
            var app = new FakeApp(7, "My.Service");
            var fakeAppLogData = dbTestData.GetFakeAppLogs(this.DefaultPageSize, app.AppName, app.AppID, LogErrorLevelEnum.Any);
            var expectedCount = fakeAppLogData.Count;
            mockDataService.Setup(m => m.GetApp(app.AppName))
                .Returns(app);
            mockDataService.Setup(m => m.GetPagedAppLogsWithFilter(It.IsAny<AppLogFilterType>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<QuerySortOrder>()))
                .Returns(fakeAppLogData);
            // Act
            var results = dbWrapperService.GetPagedAppLogsByApp(app.AppName);
            // Assert
            mockDataService.Verify(m => m
                .GetPagedAppLogsWithFilter(
                    It.IsAny<AppLogFilterType>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<QuerySortOrder>()),
                 Times.Once);
            Assert.AreEqual(expectedCount, results.Count);
        }

        [TestMethod]
        public void GetPagedAppLogsByApp_FilterForError_CallsDataService_WithExpectedErrorLevelFilter()
        {
            // Arrange
            var app = new FakeApp(7, "My.Service");
            var errorLevel = LogErrorLevelEnum.Error;
            mockDataService.Setup(m => m.GetApp(app.AppName))
                .Returns(app);
            var fakeDataForFilterTest = dbTestData
                .GetAppLogDataForFilterTest(app.AppID, app.AppName, errorLevel);
            // Act
            var results = dbWrapperService.GetPagedAppLogsByApp(
                app.AppName, this.DefaultPage, this.DefaultPageSize, errorLevel);
            // Assert
            mockDataService.Verify(m => m
                .GetPagedAppLogsWithFilter(
                    It.Is<Expression<Func<IAppLog, bool>>>(e =>
                         CustomAsserts.FilterResultComparer(e, fakeDataForFilterTest.ToArray())),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<QuerySortOrder>()),
                Times.Once);
        }

        [TestMethod]
        public void GetPagedAppLogsByApp_FilterForInfo_CallsDataService_WithExpectedErrorLevelFilter()
        {
            // Arrange
            var app = new FakeApp(7, "My.Service");
            var errorLevel = LogErrorLevelEnum.Info;
            mockDataService.Setup(m => m.GetApp(app.AppName))
                .Returns(app);
            var dataForFilterTest = dbTestData
                .GetAppLogDataForFilterTest(app.AppID, app.AppName, errorLevel);
            // Act
            var results = dbWrapperService
                .GetPagedAppLogsByApp(app.AppName, this.DefaultPage, this.DefaultPageSize, errorLevel);
            // Assert
            mockDataService.Verify(m => m
                .GetPagedAppLogsWithFilter(
                    It.Is<Expression<Func<IAppLog, bool>>>(e =>
                         CustomAsserts.FilterResultComparer(e, dataForFilterTest.ToArray())),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<QuerySortOrder>()),
                Times.Once);
        }

        [TestMethod]
        public void GetPagedAppLogsByApp_FilterForWarn_CallsDataService_WithExpectedErrorLevelFilter()
        {
            // Arrange
            var app = new FakeApp(7, "My.Service");
            var errorLevel = LogErrorLevelEnum.Warn;
            mockDataService.Setup(m => m.GetApp(app.AppName))
                .Returns(app);
            var dataForFilterTest = dbTestData
                .GetAppLogDataForFilterTest(app.AppID, app.AppName, errorLevel);
            // Act
            var results = dbWrapperService
                .GetPagedAppLogsByApp(app.AppName, this.DefaultPage, this.DefaultPageSize, errorLevel);
            // Assert
            mockDataService.Verify(m => m
                .GetPagedAppLogsWithFilter(
                    It.Is<Expression<Func<IAppLog, bool>>>(e =>
                         CustomAsserts.FilterResultComparer(e, dataForFilterTest.ToArray())),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<QuerySortOrder>()),
                Times.Once);
        }

        [TestMethod]
        public void GetPagedAppLogsByApp_FilterAnyWithPaging_CallsDataService_WithExpectedAppName()
        {
            // Arrange
            var errorLevel = LogErrorLevelEnum.Any;
            var appName = "My.Test.Service";
            // Act
            var results = dbWrapperService
                .GetPagedAppLogsByApp(appName, this.DefaultPage, this.DefaultPageSize, errorLevel);
            // Assert
            mockDataService.Verify(m =>
                m.GetPagedAppLogsByApp(appName, It.IsAny<int>(), It.IsAny<int>()),
                Times.Once);
        }

        [TestMethod]
        public void GetPagedAppLogsByApp_FilterAnyWithPaging_CallsDataService_WithDefaultPagingValues()
        {
            // Arrange
            var errorLevel = LogErrorLevelEnum.Any;
            var appName = "My.Test.Service";
            // Act
            var results = dbWrapperService
                .GetPagedAppLogsByApp(appName, this.DefaultPage, this.DefaultPageSize, errorLevel);
            // Assert
            mockDataService.Verify(m =>
                m.GetPagedAppLogsByApp(It.IsAny<string>(), this.DefaultPage, this.DefaultPageSize),
                Times.Once);
        }

        [TestMethod]
        public void GetPagedAppLogsByApp_FilterAnyWithPaging_CallsDataService_WithExpectedPagingValues()
        {
            // Arrange
            var errorLevel = LogErrorLevelEnum.Any;
            var appName = "My.Test.Service";
            var page = 3;
            var pageSize = 15;
            // Act
            var results = dbWrapperService
                .GetPagedAppLogsByApp(appName, page, pageSize, errorLevel);
            // Assert
            mockDataService.Verify(m =>
                m.GetPagedAppLogsByApp(It.IsAny<string>(), page, pageSize),
                Times.Once);
        }

        [TestMethod]
        public void GetPagedAppLogsByApp_FilterAny_WhenNullData_ReturnsEmptyList()
        {
            // Arrange
            var errorLevel = LogErrorLevelEnum.Any;
            var appName = "My.Test.Service";
            mockDataService.Setup(m => m.GetPagedAppLogsByApp(appName, It.IsAny<int>(), It.IsAny<int>()))
                .Returns((IEnumerable<IAppLog>)null);
            // Act
            var results = dbWrapperService
                .GetPagedAppLogsByApp(appName, this.DefaultPage, this.DefaultPageSize, errorLevel);
            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void GetPagedAppLogsByApp_FilterAny_WhenEmptyData_ReturnsEmptyList()
        {
            // Arrange
            var errorLevel = LogErrorLevelEnum.Any;
            var appName = "My.Test.Service";
            mockDataService.Setup(m => m.GetPagedAppLogsByApp(appName, It.IsAny<int>(), It.IsAny<int>()))
                .Returns(new List<FakeAppLog>());
            // Act
            var results = dbWrapperService
                .GetPagedAppLogsByApp(appName, this.DefaultPage, this.DefaultPageSize, errorLevel);
            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void GetPagedAppLogsByApp_FilterAny_WhenDataFound_ReturnsExpectedList()
        {
            // Arrange
            var errorLevel = LogErrorLevelEnum.Any;
            var appName = "My.Test.Service";
            var fakeAppLogData = dbTestData.GetFakeAppLogs(this.DefaultPageSize, appName, 7, LogErrorLevelEnum.Any);
            var expectedCount = fakeAppLogData.Count;
            mockDataService.Setup(m => m.GetPagedAppLogsByApp(appName, It.IsAny<int>(), It.IsAny<int>()))
                .Returns(fakeAppLogData);
            // Act
            var results = dbWrapperService
                .GetPagedAppLogsByApp(appName, this.DefaultPage, this.DefaultPageSize, errorLevel);
            // Assert
            mockDataService.Verify(m => 
                m.GetPagedAppLogsByApp(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()),
                Times.Once);
            Assert.AreEqual(expectedCount, results.Count);
        }

        [TestMethod]
        public void GetPagedAppLogs_DefaultParameters_CallsDataService_WithDefaultPagingValues()
        {
            // Arrange
            // Act
            var results = dbWrapperService.GetPagedAppLogs();
            // Assert
            mockDataService.Verify(m => m
                .GetPagedAppLogsWithFilter(
                    It.IsAny<AppLogFilterType>(),
                    this.DefaultPage,
                    this.DefaultPageSize,
                    It.IsAny<QuerySortOrder>()),
                Times.Once);
        }

        [TestMethod]
        public void GetPagedAppLogs_DefaultParameters_CallsDataService_WithDescendingSortOrder()
        {
            // Arrange
            var expectedSortOrder = QuerySortOrder.Descending;
            // Act
            var results = dbWrapperService.GetPagedAppLogs();
            // Assert
            mockDataService.Verify(m => m
                .GetPagedAppLogsWithFilter(
                    It.IsAny<AppLogFilterType>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    expectedSortOrder),
                Times.Once);
        }

        [TestMethod]
        public void GetPagedAppLogs_DefaultParameters_CallsDataService_WithExpectedFilter()
        {
            // Arrange
            var errorLevel = LogErrorLevelEnum.Error;
            var dataForFilterTest = dbTestData.GetAppLogDataForFilterTest(errorLevel);
            // Act
            var results = dbWrapperService.GetPagedAppLogs();
            // Assert
            mockDataService.Verify(m => m
                .GetPagedAppLogsWithFilter(
                    It.Is<Expression<Func<IAppLog, bool>>>(e =>
                         CustomAsserts.FilterResultComparer(e, dataForFilterTest.ToArray())),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<QuerySortOrder>()),
                Times.Once);
        }

        [TestMethod]
        public void GetPagedAppLogs_SpecifiedPagingParameters_CallsDataService_WithExpectedValues()
        {
            // Arrange
            var page = 3;
            var pageSize = 15;
            var errorLevel = LogErrorLevelEnum.Error;
            // Act
            var results = dbWrapperService.GetPagedAppLogs(page, pageSize, errorLevel);
            // Assert
            mockDataService.Verify(m => m
                .GetPagedAppLogsWithFilter(
                    It.IsAny<AppLogFilterType>(),
                    page,
                    pageSize,
                    It.IsAny<QuerySortOrder>()),
                Times.Once);
        }

        [TestMethod]
        public void GetPagedAppLogs_FilterForAny_CallsDataService_WithExpectedFilter()
        {
            // Arrange
            var errorLevel = LogErrorLevelEnum.Any;
            // Act
            var results = dbWrapperService
                .GetPagedAppLogs(this.DefaultPage, this.DefaultPageSize, errorLevel);
            // Assert
            mockDataService.Verify(m => m
                .GetPagedAppLogs(It.IsAny<int>(), It.IsAny<int>()),
                Times.Once);
        }

        [TestMethod]
        public void GetPagedAppLogs_FilterForError_CallsDataService_WithExpectedFilter()
        {
            // Arrange
            var errorLevel = LogErrorLevelEnum.Error;
            var dataForFilterTest = dbTestData.GetAppLogDataForFilterTest(errorLevel);
            // Act
            var results = dbWrapperService
                .GetPagedAppLogs(this.DefaultPage, this.DefaultPageSize, errorLevel);
            // Assert
            mockDataService.Verify(m => m
                .GetPagedAppLogsWithFilter(
                    It.Is<Expression<Func<IAppLog, bool>>>(e =>
                         CustomAsserts.FilterResultComparer(e, dataForFilterTest.ToArray())),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<QuerySortOrder>()),
                Times.Once);
        }

        [TestMethod]
        public void GetPagedAppLogs_FilterForInfo_CallsDataService_WithExpectedFilter()
        {
            // Arrange
            var errorLevel = LogErrorLevelEnum.Info;
            var dataForFilterTest = dbTestData.GetAppLogDataForFilterTest(errorLevel);
            // Act
            var results = dbWrapperService
                .GetPagedAppLogs(this.DefaultPage, this.DefaultPageSize, errorLevel);
            // Assert
            mockDataService.Verify(m => m
                .GetPagedAppLogsWithFilter(
                    It.Is<Expression<Func<IAppLog, bool>>>(e =>
                         CustomAsserts.FilterResultComparer(e, dataForFilterTest.ToArray())),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<QuerySortOrder>()),
                Times.Once);
        }

        [TestMethod]
        public void GetPagedAppLogs_FilterForWarn_CallsDataService_WithExpectedFilter()
        {
            // Arrange
            var errorLevel = LogErrorLevelEnum.Warn;
            var dataForFilterTest = dbTestData.GetAppLogDataForFilterTest(errorLevel);
            // Act
            var results = dbWrapperService
                .GetPagedAppLogs(this.DefaultPage, this.DefaultPageSize, errorLevel);
            // Assert
            mockDataService.Verify(m => m
                .GetPagedAppLogsWithFilter(
                    It.Is<Expression<Func<IAppLog, bool>>>(e =>
                         CustomAsserts.FilterResultComparer(e, dataForFilterTest.ToArray())),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<QuerySortOrder>()),
                Times.Once);
        }

        [TestMethod]
        public void GetPagedAppLogs_DefaultParameters_WhenNullData_ReturnsEmptyList()
        {
            // Arrange
            mockDataService.Setup(m => m.GetPagedAppLogsWithFilter(It.IsAny<AppLogFilterType>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<QuerySortOrder>()))
                .Returns((IEnumerable<IAppLog>)null);
            // Act
            var results = dbWrapperService.GetPagedAppLogs();
            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void GetPagedAppLogs_DefaultParameters_WhenEmptyData_ReturnsEmptyList()
        {
            // Arrange
            mockDataService.Setup(m => m.GetPagedAppLogsWithFilter(It.IsAny<AppLogFilterType>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<QuerySortOrder>()))
                .Returns(new List<FakeAppLog>());
            // Act
            var results = dbWrapperService.GetPagedAppLogs();
            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void GetPagedAppLogs_DefaultParameters_WhenDataFound_ReturnsExpectedList()
        {
            // Arrange
            var errorLevel = LogErrorLevelEnum.Error;
            var fakeAppLogData = dbTestData.GetFakeAppLogs(this.DefaultPageSize, errorLevel);
            var expectedCount = fakeAppLogData.Count;
            mockDataService.Setup(m => m.GetPagedAppLogsWithFilter(It.IsAny<AppLogFilterType>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<QuerySortOrder>()))
                .Returns(fakeAppLogData);
            // Act
            var results = dbWrapperService.GetPagedAppLogs();
            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(expectedCount, results.Count);
        }


        [TestMethod]
        public void GetPagedFilteredAppLogs_DefaultParameters_CallsDataService_WithExpectedValues()
        {
            // Arrange
            var errorLevel = LogErrorLevelEnum.Error;
            var expectedSortOrder = QuerySortOrder.Unspecified;
            var filter = GetDefaultFilterForErrorLevel(errorLevel);
            // Act
            var results = dbWrapperService.GetPagedFilteredAppLogs(filter);
            // Assert
            mockDataService.Verify(m => m
                .GetPagedAppLogsWithFilter(
                    It.IsAny<AppLogFilterType>(),
                    this.DefaultPage,
                    this.DefaultPageSize,
                    expectedSortOrder),
                Times.Once);
        }

        [TestMethod]
        public void GetPagedFilteredAppLogs_SepecifiedParameters_CallsDataService_WithExpectedValues()
        {
            // Arrange
            var errorLevel = LogErrorLevelEnum.Error;
            var page = 2;
            var pageSize = 40;
            var sortOrder = QuerySortOrder.Ascending;
            var filter = GetDefaultFilterForErrorLevel(errorLevel);
            // Act
            var results = dbWrapperService
                .GetPagedFilteredAppLogs(filter, page, pageSize, sortOrder);
            // Assert
            mockDataService.Verify(m => m
                .GetPagedAppLogsWithFilter(
                    It.IsAny<AppLogFilterType>(),
                    page,
                    pageSize,
                    sortOrder),
                Times.Once);
        }

        [TestMethod]
        public void GetPagedFilteredAppLogs_FilterAny_CallsDataService_WithExpectedFilter()
        {
            // Arrange
            var errorLevel = LogErrorLevelEnum.Any;
            var filter = GetDefaultFilterForErrorLevel(errorLevel);
            // note... since the filter type is Any, we cannot use the FilterResultComparer.FilterResultComparer
            //  the filter comparer would check for a literal type of "Any"... 
            // Act
            var results = dbWrapperService.GetPagedFilteredAppLogs(filter);
            // Assert
            mockDataService.Verify(m => m
               .GetPagedAppLogsWithFilter(
                   It.IsAny<AppLogFilterType>(),
                   It.IsAny<int>(),
                   It.IsAny<int>(),
                   It.IsAny<QuerySortOrder>()),
               Times.Once);
        }

        [TestMethod]
        public void GetPagedFilteredAppLogs_DefaultParameters_WhenNullData_ReturnsEmptyList()
        {
            // Arrange
            var errorLevel = LogErrorLevelEnum.Any;
            var filter = GetDefaultFilterForErrorLevel(errorLevel);
            mockDataService.Setup(m => m
            .GetPagedAppLogsWithFilter(It.IsAny<AppLogFilterType>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<QuerySortOrder>()))
                .Returns((IEnumerable<IAppLog>)null);
            // Act
            var results = dbWrapperService.GetPagedFilteredAppLogs(filter);
            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void GetPagedFilteredAppLogs_DefaultParameters_WhenEmptyData_ReturnsEmptyList()
        {
            // Arrange
            var errorLevel = LogErrorLevelEnum.Any;
            var filter = GetDefaultFilterForErrorLevel(errorLevel);
            mockDataService.Setup(m => m
                .GetPagedAppLogsWithFilter(It.IsAny<AppLogFilterType>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<QuerySortOrder>()))
                .Returns(new List<FakeAppLog>());
            // Act
            var results = dbWrapperService.GetPagedFilteredAppLogs(filter);
            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void GetPagedFilteredAppLogs_DefaultParameters_WhenDataFound_ReturnsExpectedList()
        {
            var errorLevel = LogErrorLevelEnum.Error;
            var fakeAppLogData = dbTestData.GetFakeAppLogs(this.DefaultPageSize, errorLevel);
            var expectedCount = fakeAppLogData.Count;
            var filter = GetDefaultFilterForErrorLevel(errorLevel);
            mockDataService.Setup(m => m
                .GetPagedAppLogsWithFilter(It.IsAny<AppLogFilterType>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<QuerySortOrder>()))
                .Returns(fakeAppLogData);
            // Act
            var results = dbWrapperService.GetPagedFilteredAppLogs(filter);
            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(expectedCount, results.Count);
        }

        [Ignore]
        [TestMethod]
        public void GetSingleAppLog_tests()
        {
            throw new NotImplementedException();
        }

        [Ignore]
        [TestMethod]
        public void GetRecentLogEntriesReportForAppByAppName_tests()
        {
            throw new NotImplementedException();
        }

        [Ignore]
        [TestMethod]
        public void GetRecentLogEntriesReportForAppByAppID_tests()
        {
            throw new NotImplementedException();
        }

        [Ignore]
        [TestMethod]
        public void BuildEmptyRecentLogReport_tests()
        {
            throw new NotImplementedException();
        }
    }
}
