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
using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Models;

namespace Icon.Dashboard.Mvc.UnitTests.ControllerTests
{
    [TestClass]
    public class HomeControllerIndexUnitTests : _HomeControllerUnitTestBase
    {
        public HomeControllerIndexUnitTests() : base() { }

        protected override string testActionName => "Index";

        [TestMethod]
        public void MvcHomeController_IndexGet_ShouldReturnViewResult()
        {
            // Arrange
            var fakeData = serviceTestData.Services.ServiceViewModelList;
            base.MockRemoteWmiSerivceWrapper
                .Setup(s => s.LoadRemoteServices(
                    It.IsAny<List<string>>(),
                    It.IsAny<bool>(),
                    It.IsAny<List<LoggedAppViewModel>>(),
                    It.IsAny<List<LoggedAppViewModel>>(),
                    It.IsAny<List<EnvironmentModel>>(),
                    It.IsAny<List<EsbEnvironmentModel>>()))
                .Returns(fakeData);
            var controller = ConstructController();

            // Act
            ActionResult result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void MvcHomeController_IndexGet_ShouldReturnViewResultWithData()
        {
            // Arrange
            var fakeData = serviceTestData.Services.ServiceViewModelList;
            int expectedCount = fakeData.Count;
            base.MockRemoteWmiSerivceWrapper
                .Setup(s => s.LoadRemoteServices(
                    It.IsAny<List<string>>(),
                    It.IsAny<bool>(),
                    It.IsAny<List<LoggedAppViewModel>>(),
                    It.IsAny<List<LoggedAppViewModel>>(),
                    It.IsAny<List<EnvironmentModel>>(),
                    It.IsAny<List<EsbEnvironmentModel>>()))
                .Returns(fakeData);
            var controller = ConstructController();

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelResult = viewResult.Model as List<ServiceViewModel>;
            Assert.AreEqual(expectedCount, modelResult.Count);
        }

        [TestMethod]
        public void MvcHomeController_IndexGet_ShouldCallSetPermissionsForActiveEnvironment()
        {
            // Arrange
            var controller = ConstructController();
            // Act
            var result = controller.Index();
            // Assert
            MockDashboardConfigManager.Verify(m => 
                m.SetPermissionsForActiveEnvironment(It.IsAny<bool>(), It.IsAny<EnvironmentCookieModel>()),
                Times.Once);
        }

        [TestMethod]
        public void MvcHomeController_IndexGet_ShouldCheckEnvironmentCookie()
        {
            // Arrange
            var environment = configTestData.ConfigDataModel.HostingEnvironmentSetting.ToString();
            var controller = ConstructController();
            // Act
            var result = controller.Index();
            // Assert
            MockCookieManager.Verify(m =>
                m.GetEnvironmentCookieIfPresent(It.IsAny<HttpRequestBase>()),
                Times.Once);
        }

        [TestMethod]
        public void MvcHomeController_IndexGet_ShouldCallSetPermissionsAndActiveEnvironment()
        {
            // Arrange
            var controller = ConstructController();
            // Act
            var result = controller.Custom();
            // Assert
            MockDashboardConfigManager.Verify(m =>
                m.SetPermissionsForActiveEnvironment(
                    It.IsAny<bool>(),
                    It.IsAny<EnvironmentCookieModel>()),
                Times.Once);
        }

        [TestMethod]
        public void MvcHomeController_IndexPost_ShouldIssueStartCommand()
        {
            // Arrange
            var controller = ConstructController();
            const string cmd = "Start";

            // Act
            var result = controller.Index(serviceTestData.Services.GloconViewModel.Name,
                serviceTestData.Services.GloconViewModel.Server, cmd);

            // Assert
            MockRemoteWmiSerivceWrapper.Verify(s => s
                .ExecuteServiceCommand( It.IsAny<string>(), It.IsAny<string>(), cmd),
                Times.Once);
        }

        [TestMethod]
        public void MvcHomeController_IndexPost_ShouldIssueStopCommand()
        {
            // Arrange
            var controller = ConstructController();
            const string cmd = "Stop";

            // Act
            var result = controller.Index(serviceTestData.Services.GloconViewModel.Name,
                serviceTestData.Services.GloconViewModel.Server, cmd);

            // Assert
            MockRemoteWmiSerivceWrapper.Verify(s => s
                .ExecuteServiceCommand( It.IsAny<string>(),  It.IsAny<string>(), cmd ),
                Times.Once);
        }

        [TestMethod]
        public void MvcHomeController_IndexPost_ShouldIssueAnyCommand()
        {
            // Arrange
            var controller = ConstructController();
            const string cmd = "DoIt";

            // Act
            var result = controller.Index(serviceTestData.Services.GloconViewModel.Name,
                serviceTestData.Services.GloconViewModel.Server, cmd);

            // Assert
            MockRemoteWmiSerivceWrapper.Verify(s => s
                .ExecuteServiceCommand( It.IsAny<string>(), It.IsAny<string>(),  cmd),
                Times.Once);
        }

        [TestMethod]
        public void MvcHomeController_IndexPost_ShouldRedirectToGetAfterExecutingCommand()
        {
            // Arrange
            var controller = ConstructController();
            const string cmd = "Start";

            // Act
            var result = controller.Index(serviceTestData.Services.GloconViewModel.Name,
                serviceTestData.Services.GloconViewModel.Server, cmd);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = result as RedirectToRouteResult;
        }

        [TestMethod]
        public void MvcHomeController_IndexPost_ShouldRedirectToGetWithExpectedRouteValues()
        {
            // Arrange
            var controller = ConstructController();
            const string cmd = "Start";

            // Act
            var result = controller.Index(serviceTestData.Services.GloconViewModel.Name,
                serviceTestData.Services.GloconViewModel.Server, cmd);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = result as RedirectToRouteResult;
            Assert.AreEqual(routeResult.RouteValues["action"], "Index");
            Assert.AreEqual(routeResult.RouteValues["controller"], "Home");
        }
    }
}
