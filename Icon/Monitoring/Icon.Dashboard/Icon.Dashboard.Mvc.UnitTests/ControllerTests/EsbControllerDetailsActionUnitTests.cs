using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.Services;
using Icon.Dashboard.Mvc.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Icon.Dashboard.Mvc.UnitTests.ControllerTests
{
    [TestClass]
    public class EsbControllerDetailsActionUnitTests : _EsbControllerUnitTestBase
    {
        protected override string testControllerName => "Esb";
        protected override string testActionName => "Details";

        [TestMethod]
        public void MvcEsbController_DetailsGet_ShouldCallSetPermissionsForActiveEnvironment()
        {
            // Arrange
            var esbEnv = EsbEnvironmentEnum.QA_DUP;
            var controller = ConstructController();
            // Act
            var result = controller.Details(esbEnv.ToString());
            // Assert
            MockDashboardConfigManager.Verify(m => 
                m.SetPermissionsForActiveEnvironment(It.IsAny<bool>(), It.IsAny<EnvironmentCookieModel>()),
                Times.Once);
        }

        [TestMethod]
        public void MvcEsbController_DetailsGet_ShouldCheckEnvironmentCookie()
        {
            // Arrange
            var esbEnv = EsbEnvironmentEnum.QA_DUP;
            var controller = ConstructController();
            // Act
            var result = controller.Details(esbEnv.ToString());
            // Assert
            MockCookieManager.Verify(m =>
                m.GetEnvironmentCookieIfPresent(It.IsAny<HttpRequestBase>()),
                Times.Once);
        }

        [TestMethod]
        public void MvcEsbController_DetailsGet_ShouldCallSetPermissionsAndActiveEnvironment()
        {
            // Arrange
            var esbEnv = EsbEnvironmentEnum.TEST;
            var controller = ConstructController();
            // Act
            var result = controller.Details(esbEnv.ToString());
            // Assert
            MockDashboardConfigManager.Verify(m =>
                m.SetPermissionsForActiveEnvironment(
                    It.IsAny<bool>(),
                    It.IsAny<EnvironmentCookieModel>()),
                Times.Once);
        }

        [TestMethod]
        public void MvcEsbController_DetailsGet_IfNameIsNull_ShouldRedirectToIndex()
        {
            // Arrange
            string esbEnvName = null;
            var controller = ConstructController();

            // Act
            var result = controller.Details(esbEnvName);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = result as RedirectToRouteResult;
            Assert.AreEqual(routeResult.RouteValues["action"], "Index");
            Assert.AreEqual(routeResult.RouteValues["controller"], "Esb");
        }

        [TestMethod]
        public void MvcEsbController_DetailsGet_IfNameIsBlank_ShouldRedirectToIndex()
        {
            // Arrange
            var esbEnvName = "";
            var controller = ConstructController();
            // Act
            var result = controller.Details(esbEnvName);
            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = result as RedirectToRouteResult;
            Assert.AreEqual(routeResult.RouteValues["action"], "Index");
            Assert.AreEqual(routeResult.RouteValues["controller"], "Esb");
        }

        [TestMethod]
        public void MvcEsbController_DetailsGet_IfNameNotParsable_ShouldRedirectToIndex()
        {
            // Arrange
            var esbEnvName = "TESTY MCTEST";
            //var fakeData = ServiceTestData.AllFakeAppViewModels;
            //int expectedCount = fakeData.Count;
            //base.MockRemoteWmiSerivceWrapper
            //    .Setup(s => s.LoadRemoteServices(It.IsAny<List<string>>(), It.IsAny<bool>()))
            //    .Returns(fakeData);
            //base.MockDashboardConfigManager
            //    .Setup(m => m.BuildEsbEnvironmentViewModelsWithAppsPopulated(
            //        It.IsAny<List<ServiceViewModel>>()))
            //    .Returns(ConfigTestData.ViewModels.EsbEnvironmentsList);
            var controller = ConstructController();

            // Act
            var result = controller.Details(esbEnvName);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = result as RedirectToRouteResult;
            Assert.AreEqual(routeResult.RouteValues["action"], "Index");
            Assert.AreEqual(routeResult.RouteValues["controller"], "Esb");
        }

        [TestMethod]
        public void MvcEsbController_DetailsGet_IfNameIsParsable_ShouldCallBuildViewModel()
        {
            // Arrange
            var esbEnv = EsbEnvironmentEnum.QA_DUP;
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
            base.MockDashboardConfigManager
                .Setup(m => m.GetEsbEnvironmentViewModelsWithAssignedServices(
                    It.IsAny<List<ServiceViewModel>>()))
                .Returns(configTestData.ViewModels.EsbEnvironmentsList);
            var controller = ConstructController();

            // Act
            var result = controller.Details(esbEnv.ToString());

            // Assert
            MockDashboardConfigManager.Verify(m => m.
                GetEsbEnvironmentViewModel( It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void MvcEsbController_DetailsGet_ShouldReturnViewResultWithData()
        {
            // Arrange
            var esbEnv = EsbEnvironmentEnum.QA_DUP;
            base.MockDashboardConfigManager
                .Setup(m => m.GetEsbEnvironmentViewModel(
                    It.IsAny<string>()))
                .Returns(configTestData.ViewModels.EsbQaDup);
            var controller = ConstructController();

            // Act
            var result = controller.Details(esbEnv.ToString());

            // Assert
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelResult = viewResult.Model as EsbEnvironmentViewModel;
            Assert.IsNotNull(modelResult);
        }
    }
}
