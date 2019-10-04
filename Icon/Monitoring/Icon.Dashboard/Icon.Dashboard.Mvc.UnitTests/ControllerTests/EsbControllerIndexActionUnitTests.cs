using Icon.Dashboard.Mvc.Models;
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
    public class EsbControllerIndexActionUnitTests : _EsbControllerUnitTestBase
    {
        protected override string testControllerName => "Esb";
        protected override string testActionName => "Index";

        [TestMethod]
        public void MvcEsbController_IndexGet__ShouldCheckEnvironmentCookie()
        {
            // Arrange
            var controller = ConstructController();
            // Act
            var result = controller.Index();
            // Assert
            MockCookieManager.Verify(m =>
                m.GetEnvironmentCookieIfPresent(It.IsAny<HttpRequestBase>()),
                Times.Once);
        }

        [TestMethod]
        public void MvcEsbController_IndexGet_ShouldCallSetPermissionsAndActiveEnvironment()
        {
            // Arrange
            var controller = ConstructController();
            // Act
            var result = controller.Index();
            // Assert
            MockDashboardConfigManager.Verify(m =>
                m.SetPermissionsForActiveEnvironment(
                    It.IsAny<bool>(),
                    It.IsAny<EnvironmentCookieModel>()),
                Times.Once);
        }

        [TestMethod]
        public void MvcEsbController_IndexGet_ShouldReturnViewResultWithData()
        {
            // Arrange
            var expectedCount = configTestData.ViewModels.EsbEnvironmentsList.Count;
            base.MockRemoteWmiSerivceWrapper
                .Setup(s => s.LoadRemoteServices(
                    It.IsAny<List<string>>(),
                    It.IsAny<bool>(),
                    It.IsAny<List<LoggedAppViewModel>>(),
                    It.IsAny<List<LoggedAppViewModel>>(),
                    It.IsAny<List<EnvironmentModel>>(),
                    It.IsAny<List<EsbEnvironmentModel>>()))
                .Returns(serviceTestData.Services.ServiceViewModelList);
            base.MockDashboardConfigManager
                .Setup(m => m.GetEsbEnvironmentViewModelsWithAssignedServices(
                    It.IsAny<List<ServiceViewModel>>()))
                .Returns(configTestData.ViewModels.EsbEnvironmentsList);
            var controller = ConstructController();

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult.Model);
            var modelResult = viewResult.Model as List<EsbEnvironmentViewModel>;
            Assert.IsNotNull(modelResult);
            Assert.AreEqual(expectedCount, modelResult.Count);

        }
    }
}
