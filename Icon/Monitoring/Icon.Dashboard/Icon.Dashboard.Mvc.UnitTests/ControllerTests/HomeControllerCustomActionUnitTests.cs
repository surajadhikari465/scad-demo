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
    public class HomeControllerCustomActionUnitTests : _HomeControllerUnitTestBase
    {
        public HomeControllerCustomActionUnitTests() : base() { }

        protected override string testActionName => "Custom";

        //[TestMethod]
        //public void MvcHomeController_CustomGet_ShouldCallReadConfig()
        //{
        //    // Arrange
        //    var controller = ConstructController();
        //    // Act
        //    var result = controller.Custom();
        //    // Assert
        //    MockDashboardConfigManager.Verify(m => m.ReadConfigAndSetHostingEnvironment(), Times.Once);
        //}

        [TestMethod]
        public void MvcHomeController_CustomGet_ShouldCallGetEnvironmentCookie()
        {
            // Arrange
            var controller = ConstructController();
            // Act
            var result = controller.Custom();
            // Assert
            MockCookieManager.Verify(m =>
                m.GetEnvironmentCookieIfPresent(It.IsAny<HttpRequestBase>()), Times.Once);
        }

        [TestMethod]
        public void MvcHomeController_CustomGet_ShouldCallSetPermissionsAndActiveEnvironment()
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
        public void MvcHomeController_CustomGet__ShouldCheckEnvironmentCookie()
        {
            // Arrange
            var controller = ConstructController();
            // Act
            var result = controller.Custom();
            // Assert
            MockCookieManager.Verify(m =>
                m.GetEnvironmentCookieIfPresent(It.IsAny<HttpRequestBase>()),
                Times.Once);
        }

        [TestMethod]
        public void MvcHomeController_CustomGet_ShouldCallBuildEnvironmentViewModels()
        {
            // Arrange
            var controller = ConstructController();
            // Act
            var result = controller.Custom();
            // Assert
            MockDashboardConfigManager.Verify(m => m.GetEnvironmentViewModels(), Times.Once);
        }


        [TestMethod]
        public void MvcHomeController_CustomGet_ShouldReturnViewResultWithData()
        {
            // Arrange
            var controller = ConstructController();
            MockDashboardConfigManager.Setup(m => m.GetEnvironmentViewModels())
                .Returns(base.configTestData.ViewModels.EnvironmentCollectionViewModel);
            // Act
            var result = controller.Custom();
            // Assert
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelResult = viewResult.Model as DashboardEnvironmentCollectionViewModel;
            Assert.IsNotNull(modelResult);
        }
    }
}
