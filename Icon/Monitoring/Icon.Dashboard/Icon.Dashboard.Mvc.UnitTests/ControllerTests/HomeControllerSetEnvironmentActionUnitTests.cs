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
using System.Security.Principal;
using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Models;

namespace Icon.Dashboard.Mvc.UnitTests.ControllerTests
{
    [TestClass]
    public class HomeControllerSetEnvironmentActionUnitTests : _HomeControllerUnitTestBase
    {
        public HomeControllerSetEnvironmentActionUnitTests() : base() { }

        protected override string testActionName => "SetAlternateEnvironment";

        [TestMethod]
        public void MvcHomeController_SetAlternateEnvironmentGet_WhenNoEnvironmentParam_ShouldRedirectToIndex()
        {
            // Arrange
            var controller = ConstructController();
            // Act
            var result = controller.SetAlternateEnvironment(null);
            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = result as RedirectToRouteResult;
            Assert.AreEqual(routeResult.RouteValues["action"], "Index");
            Assert.AreEqual(routeResult.RouteValues["controller"], "Home");
        }

        [TestMethod]
        public void MvcHomeController_SetAlternateEnvironmentGet_WhenEnvironmentParamNotParsable_ShouldRedirectToIndex()
        {
            // Arrange
            var controller = ConstructController();
            // Act
            var result = controller.SetAlternateEnvironment("xxx");
            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = result as RedirectToRouteResult;
            Assert.AreEqual(routeResult.RouteValues["action"], "Index");
            Assert.AreEqual(routeResult.RouteValues["controller"], "Home");
        }

        [TestMethod]
        public void MvcHomeController_SetAlternateEnvironmentGet_WhenEnvironmentParamIsUndefined_ShouldRedirectToIndex()
        {
            // Arrange
            var controller = ConstructController();
            // Act
            var result = controller.SetAlternateEnvironment(EnvironmentEnum.Undefined.ToString());
            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = result as RedirectToRouteResult;
            Assert.AreEqual(routeResult.RouteValues["action"], "Index");
            Assert.AreEqual(routeResult.RouteValues["controller"], "Home");
        }

        [TestMethod]
        public void MvcHomeController_SetAlternateEnvironmentGet_WhenEnvironmentParamIsCustom_ShouldRedirectToIndex()
        {
            // Arrange
            var controller = ConstructController();
            // Act
            var result = controller.SetAlternateEnvironment(EnvironmentEnum.Custom.ToString());
            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = result as RedirectToRouteResult;
            Assert.AreEqual(routeResult.RouteValues["action"], "Index");
            Assert.AreEqual(routeResult.RouteValues["controller"], "Home");
        }

        [TestMethod]
        public void MvcHomeController_SetAlternateEnvironmentGet_WhenEnvironmentParamIsHosting_ShouldClearEnvironmentCookie()
        {
            // Arrange
            var environment = configTestData.ConfigDataModel.HostingEnvironmentSetting.ToString();
            var controller = ConstructController();
            // Act
            var result = controller.SetAlternateEnvironment(environment);
            // Assert
            MockCookieManager.Verify(m =>
                m.ClearEnvironmentCookies(It.IsAny<HttpRequestBase>(), It.IsAny<HttpResponseBase>()),
                Times.Once);
        }

        [TestMethod]
        public void MvcHomeController_SetAlternateEnvironmentGet_WhenEnvironmentParamIsValid_ShouldBuildEnvironmentCookie()
        {
            // Arrange
            var environment = EnvironmentEnum.Perf;
            var controller = ConstructController();
            // Act
            var result = controller.SetAlternateEnvironment(environment.ToString());
            // Assert
            MockDashboardConfigManager.Verify(m => m.GetEnvironmentCookieModelFromEnum(environment), Times.Once);
        }

        [TestMethod]
        public void MvcHomeController_SetAlternateEnvironmentGet_WhenEnvironmentParamIsValid_ShouldSetEnvironmentCookie()
        {
            // Arrange
            var environment = EnvironmentEnum.Perf.ToString();
            var controller = ConstructController();
            // Act
            var result = controller.SetAlternateEnvironment(environment);
            // Assert
            MockCookieManager.Verify(m =>
                m.SetEnvironmentCookies(
                    It.IsAny<HttpRequestBase>(),
                    It.IsAny<HttpResponseBase>(),
                    It.IsAny<EnvironmentCookieModel>()),
                Times.Once);
        }
    }
}
