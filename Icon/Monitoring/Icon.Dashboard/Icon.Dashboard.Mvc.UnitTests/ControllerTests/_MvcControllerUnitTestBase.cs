using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Filters;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.Services;
using Icon.Dashboard.Mvc.UnitTests.TestData;
using Icon.Dashboard.Mvc.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Icon.Dashboard.Mvc.UnitTests.ControllerTests
{
    public abstract class _MvcControllerUnitTestBase
    {
        protected Mock<HttpContextBase> MockHttpContext = new Mock<HttpContextBase>();
        protected Mock<IPrincipal> MockUser = new Mock<IPrincipal>();
        protected Mock<IDashboardAuthorizer> MockAuthorizer = new Mock<IDashboardAuthorizer>();
        protected Mock<IDashboardDataManager> MockDashboardConfigManager = new Mock<IDashboardDataManager>();
        protected Mock<IIconDatabaseServiceWrapper> MockIconLoggingServiceWrapper = new Mock<IIconDatabaseServiceWrapper>();
        protected Mock<IMammothDatabaseServiceWrapper> MockMammothLoggingServiceWrapper = new Mock<IMammothDatabaseServiceWrapper>();
        protected Mock<IEnvironmentCookieManager> MockCookieManager = new Mock<IEnvironmentCookieManager>();
        protected Mock<IRemoteWmiServiceWrapper> MockRemoteWmiSerivceWrapper = new Mock<IRemoteWmiServiceWrapper>();

        protected RemoteServiceTestData serviceTestData = new RemoteServiceTestData();
        protected ConfigTestData configTestData = new ConfigTestData();
        protected GlobalViewData fakeGlobalViewData = new GlobalViewData();
        protected abstract string testControllerName { get; }
        protected abstract string testActionName { get; }

        public _MvcControllerUnitTestBase()
        {
            SetupFakeGlobalViewData();
            SetupStandardMockConfigProperties(MockDashboardConfigManager);
        }

        protected void InitializeTestControllerContext(Controller controller)
        {
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(r => r.Url).Returns(new Uri("http://fakeserver/IconDashboard/Home/Index", UriKind.Absolute));
            mockRequest.Setup(r => r.Cookies).Returns(new HttpCookieCollection { });

            var mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(r => r.Cookies).Returns(new HttpCookieCollection { });

            MockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);
            MockHttpContext.Setup(c => c.Response).Returns(mockResponse.Object);

            var fakeRouteData = new RouteData();
            fakeRouteData.Values.Add("controller", testControllerName);
            fakeRouteData.Values.Add("action", testActionName);

            controller.ControllerContext = new ControllerContext(MockHttpContext.Object, fakeRouteData, controller);
        }

        private void SetupFakeGlobalViewData()
        {
            fakeGlobalViewData.ActionName = testControllerName;
            fakeGlobalViewData.ActionName = testActionName;
            fakeGlobalViewData.HostingEnvironment = configTestData.ViewModels.Tst0;
            fakeGlobalViewData.ActiveEnvironment = configTestData.ViewModels.Tst0;
            fakeGlobalViewData.ActiveEnvironmentName = configTestData.ViewModels.Tst0.Name;
            fakeGlobalViewData.ServiceCommandsAreEnabled = true;
            fakeGlobalViewData.ServiceCommandTimeout = 1000;
            fakeGlobalViewData.HoursForRecentErrors = 48;
            fakeGlobalViewData.MillisecondsForRecentErrorsPolling = 10000;
            fakeGlobalViewData.SubMenuForIconLogs = new SubMenuViewModel();
            fakeGlobalViewData.SubMenuForMammothLogs = new SubMenuViewModel();
            fakeGlobalViewData.SubMenuForIconApiJobs = new SubMenuViewModel();
            fakeGlobalViewData.SubMenuForSupportApps = new SubMenuForSupportAppsViewModel();
            fakeGlobalViewData.SubMenuForEnvironments = new SubMenuViewModel();
        }

        private void SetupStandardMockConfigProperties(Mock<IDashboardDataManager> mockDashboardConfigManager)
        {
            mockDashboardConfigManager.SetupGet(c => c.ConfigData)
                .Returns(configTestData.ConfigDataModel);
            mockDashboardConfigManager.SetupGet(c => c.HostingEnvironment)
                .Returns(configTestData.Models.Tst0);
            mockDashboardConfigManager.SetupGet(c => c.ActiveEnvironment)
                .Returns(configTestData.Models.Tst0);
            mockDashboardConfigManager.SetupGet(c => c.AreChangesAllowed)
                .Returns(true);
            mockDashboardConfigManager.Setup(c => c.BuildGlobalViewModel(
                testControllerName,
                testActionName,
                It.IsAny<bool>(),
                It.IsAny<List<LoggedAppViewModel>>(),
                It.IsAny<List<LoggedAppViewModel>>(),
                It.IsAny<string>()))
                .Returns(fakeGlobalViewData);
        }

        protected IPrincipal user => MockUser.Object;

        protected IDashboardAuthorizer authorizer => MockAuthorizer.Object;

        protected IDashboardDataManager dashboardConfigManager => MockDashboardConfigManager.Object;

        protected IIconDatabaseServiceWrapper iconDbServiceWrapper => MockIconLoggingServiceWrapper.Object;

        protected IMammothDatabaseServiceWrapper mammothDbServiceWrapper => MockMammothLoggingServiceWrapper.Object;

        protected IEnvironmentCookieManager environmentCookieManager => MockCookieManager.Object;

        protected IRemoteWmiServiceWrapper wmiServiceWrapper => MockRemoteWmiSerivceWrapper.Object;
    }
}
