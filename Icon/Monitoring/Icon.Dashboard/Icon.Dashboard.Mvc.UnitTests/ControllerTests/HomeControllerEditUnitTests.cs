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
using Icon.Dashboard.Mvc.Models;

namespace Icon.Dashboard.Mvc.UnitTests.ControllerTests
{

    [TestClass]
    public class HomeControllerEditUnitTests : _HomeControllerUnitTestBase
    {
        protected override string testActionName => "Edit";

        [TestMethod]
        public void MvcHomeController_EditGetService_ShouldReturnViewResult()
        {
            // Arrange
            var testService = serviceTestData.Services.GloconViewModel;
            MockRemoteWmiSerivceWrapper
                .Setup(s => s.LoadRemoteService(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<List<LoggedAppViewModel>>(),
                    It.IsAny<List<LoggedAppViewModel>>(),
                    It.IsAny<List<EnvironmentModel>>(),
                    It.IsAny<List<EsbEnvironmentModel>>(),
                    It.IsAny<IExternalConfigXmlManager>()))
                .Returns(testService);
            var controller = ConstructController();

            // Act
            ActionResult result = controller.Edit(testService.Name, testService.Server);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void MvcHomeController_EditGet_ShouldCallReadConfig()
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
        public void MvcHomeController_EditGet_ShouldCallSetPermissionsAndActiveEnvironment()
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
        public void MvcHomeController_EditGetService_ShouldCallGetViewModelMethod()
        {
            // Arrange
            var testService = serviceTestData.Services.GloconViewModel;
            MockRemoteWmiSerivceWrapper
                .Setup(s => s.LoadRemoteService(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<List<LoggedAppViewModel>>(),
                    It.IsAny<List<LoggedAppViewModel>>(),
                    It.IsAny<List<EnvironmentModel>>(),
                    It.IsAny<List<EsbEnvironmentModel>>(),
                    It.IsAny<IExternalConfigXmlManager>()))
                .Returns(testService);
            var controller = ConstructController();

            // Act
            ActionResult result = controller.Edit(testService.Name, testService.Server);

            // Assert
            MockRemoteWmiSerivceWrapper.Verify(s => s.LoadRemoteService(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<List<LoggedAppViewModel>>(),
                    It.IsAny<List<LoggedAppViewModel>>(),
                    It.IsAny<List<EnvironmentModel>>(),
                    It.IsAny<List<EsbEnvironmentModel>>(),
                    It.IsAny<IExternalConfigXmlManager>()),
                Times.Once);
        }

        [TestMethod]
        public void MvcHomeController_EditPostService_ShouldCallSaveAppSettings()
        {
            // Arrange
            var testService = serviceTestData.Services.GloconViewModel;
            var postModel = serviceTestData.Services.GloconViewModel;
            var controller = ConstructController();

            // Act
            ActionResult result = controller.Edit(postModel);

            // Assert
            MockRemoteWmiSerivceWrapper.Verify(s => s.UpdateRemoteServiceConfig( postModel, null), Times.Once);
        }

        [TestMethod]
        public void MvcHomeController_EditPostService_ShouldRedirectToGetAfterExecutingCommand()
        {
            // Arrange
            var testService = serviceTestData.Services.GloconViewModel;
            var postModel = serviceTestData.Services.GloconViewModel;
            var controller = ConstructController();

            // Act
            ActionResult result = controller.Edit(postModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = result as RedirectToRouteResult;
        }

        [TestMethod]
        public void MvcHomeController_EditPostService_ShouldRedirectToGetWithExpectedRouteValues()
        {
            // Arrange
            var testService = serviceTestData.Services.GloconViewModel;
            var postModel = serviceTestData.Services.GloconViewModel;
            var controller = ConstructController();

            // Act
            ActionResult result = controller.Edit(postModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = result as RedirectToRouteResult;
            Assert.AreEqual("Home", routeResult.RouteValues["controller"]);
            Assert.AreEqual("Edit", routeResult.RouteValues["action"]);
            Assert.AreEqual(testService.Server, routeResult.RouteValues["appServer"]);
            Assert.AreEqual(testService.Name, routeResult.RouteValues["serviceName"]);
        }
    }
}
