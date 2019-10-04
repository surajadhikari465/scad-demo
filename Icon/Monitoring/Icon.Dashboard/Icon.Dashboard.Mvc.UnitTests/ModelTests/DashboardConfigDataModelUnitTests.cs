using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.Services;
using Icon.Dashboard.Mvc.UnitTests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.Mvc.UnitTests.ModelTests
{
    [TestClass]
    public class DashboardConfigDataModelUnitTests
    {
        ConfigTestData configTestData = new ConfigTestData();
        Mock<IDashboardConfigDataLoader> mockConfigDataLoader = new Mock<IDashboardConfigDataLoader>();

        [TestMethod]
        public void DefaultConstructor_InitializesCollections()
        {
            // Arrange
            // Act
            var configDataModel = new DashboardConfigDataModel();
            // Assert
            Assert.IsNotNull(configDataModel.SecurityGroupsWithReadRights);
            Assert.IsNotNull(configDataModel.SecurityGroupsWithEditRights);
            Assert.IsNotNull(configDataModel.EnvironmentDefinitions);
            Assert.IsNotNull(configDataModel.EsbEnvironmentDefinitions);
        }

        [TestMethod]
        public void DefaultConstructor_SetsCookieNames()
        {
            // Arrange
            var expectedEnvironmentNameCookieName = "environmentName";
            var expectedEnvironmentAppServersCookieName = "environmentAppServers";
            // Act
            var configDataModel = new DashboardConfigDataModel(mockConfigDataLoader.Object);
            // Assert
            Assert.AreEqual(expectedEnvironmentNameCookieName, configDataModel.EnvironmentCookieName);
            Assert.AreEqual(expectedEnvironmentAppServersCookieName, configDataModel.EnvironmentAppServersCookieName);
        }

        [TestMethod]
        public void LoadConfigData_ShouldConvertAllEnvironmentElementsToModels()
        {
            // Arrange
            var expectedModels = configTestData.Models.EnvironmentsList
                .Where(e => e.IsEnabled).ToList();
            mockConfigDataLoader.Setup(r => r.GetEnvironments())
                .Returns(configTestData.Models.EnvironmentsList.Where(e=>e.IsEnabled).ToList());
            var configDataModel = new DashboardConfigDataModel();
            // Act
            configDataModel.LoadConfigData(mockConfigDataLoader.Object);
            // Assert
            CustomAsserts.ListsAreEqual(expectedModels, configDataModel.EnvironmentDefinitions);
        }

        [TestMethod]
        public void LoadConfigData_ShouldConvertAllEsbElementsToModels()
        {
            // Arrange
            var expectedModels = configTestData.Models.EsbEnvironmentsList;
            mockConfigDataLoader.Setup(r => r.GetEsbEnvironments())
                .Returns(configTestData.Models.EsbEnvironmentsList);
            var configDataModel = new DashboardConfigDataModel();
            // Act
            configDataModel.LoadConfigData(mockConfigDataLoader.Object);
            // Assert
            CustomAsserts.ListsAreEqual(configTestData.Models.EsbEnvironmentsList, configDataModel.EsbEnvironmentDefinitions);
        }

        [TestMethod]
        public void LoadConfigData_ConfigValueForHostingEnvironment_IsSetInProperty()
        {
            // Arrange
            var expectedValue = EnvironmentEnum.Tst0;
            mockConfigDataLoader.Setup(m => m
                .GetTypedSettingValueOrDefault(Constants.DashboardAppSettings.Keys.HostingEnvironment, It.IsAny<EnvironmentEnum>()))
                .Returns(expectedValue);
            var configDataModel = new DashboardConfigDataModel();
            // Act
            configDataModel.LoadConfigData(mockConfigDataLoader.Object);
            // Assert
            Assert.AreEqual(expectedValue, configDataModel.HostingEnvironmentSetting);
        }

        [TestMethod]
        public void LoadConfigData_ConfigValueForMillisecondsForRecentErrorsPolling_IsSetInProperty()
        {
            // Arrange
            // setting value is in seconds, accessor should get value and convert to milliseconds
            var secondsValue = 10;
            var expectedMillisecondsValue = secondsValue * 1000;
            mockConfigDataLoader.Setup(m => m
                .GetTypedSettingValueOrDefault(Constants.DashboardAppSettings.Keys.SecondsForRecentErrorsPolling, It.IsAny<int>()))
                .Returns(secondsValue);
            var configDataModel = new DashboardConfigDataModel();
            // Act
            configDataModel.LoadConfigData(mockConfigDataLoader.Object);
            // Assert
            Assert.AreEqual(expectedMillisecondsValue, configDataModel.MillisecondsForRecentErrorsPolling);
        }

        [TestMethod]
        public void LoadConfigData_ConfigValueForEnvironments_IsSetInProperty()
        {
            // Arrange
            var expectedValues = configTestData.Models.EnvironmentsList.Where(e => e.IsEnabled).ToList();
            mockConfigDataLoader.Setup(r => r.GetEnvironments())
                .Returns(configTestData.Models.EnvironmentsList.Where(e=>e.IsEnabled).ToList());
            var configDataModel = new DashboardConfigDataModel();
            // Act
            configDataModel.LoadConfigData(mockConfigDataLoader.Object);
            // Assert
            CustomAsserts.ListsAreEqual(expectedValues, configDataModel.EnvironmentDefinitions);
        }

        [TestMethod]
        public void LoadConfigData_ConfigValueForEsbEnvironments_IsSetInProperty()
        {
            // Arrange
            var expectedValues = configTestData.Models.EsbEnvironmentsList;
            mockConfigDataLoader.Setup(r => r.GetEsbEnvironments())
                .Returns(configTestData.Models.EsbEnvironmentsList);
            var configDataModel = new DashboardConfigDataModel();
            // Act
            configDataModel.LoadConfigData(mockConfigDataLoader.Object);
            // Assert
            CustomAsserts.ListsAreEqual(expectedValues, configDataModel.EsbEnvironmentDefinitions);
        }
    }
}
