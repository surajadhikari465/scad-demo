using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Helpers;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.Models.CustomConfigElements;
using Icon.Dashboard.Mvc.Services;
using Icon.Dashboard.Mvc.UnitTests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.Mvc.UnitTests.ModelTests
{
    [TestClass]
    public class DashboardConfigDataModelIntegrationTests
    {
        ConfigTestData configTestData = new ConfigTestData();
        string webConfigWithStandardData = $"TestData\\SampleWebConfigWithStandardValues.config";
        string webConfigWithNoData = $"TestData\\SampleWebConfigWithNoValues.config";

        private DashboardConfigDataLoader CreateConfigReaderForExternalFile(string path)
        {
            var configuration = ConfigAccess.OpenExternalAppConfiguration(path);
            var appSettings = ConfigAccess.GetAppSettingsAsDictionary(configuration);
            var environmentsConfigSection = ConfigAccess.GetCustomConfigSection<EnvironmentsSection>(configuration, Constants.CustomConfigSectionGroupName);
            var esbEnvironmentsConfigSection = ConfigAccess.GetCustomConfigSection<EsbEnvironmentsSection>(configuration, Constants.CustomConfigSectionGroupName);
            var configReader = new DashboardConfigDataLoader(appSettings, environmentsConfigSection, esbEnvironmentsConfigSection);
            return configReader;
        }

        private DashboardConfigDataLoader CreateConfigReaderForInternalTestProjectAppConfig()
        {
            var configuration = ConfigAccess.OpenInternalAppConfiguration();
            var appSettings = ConfigAccess.GetAppSettingsAsDictionary(configuration);
            var environmentsConfigSection = ConfigAccess.GetCustomConfigSection<EnvironmentsSection>(configuration, Constants.CustomConfigSectionGroupName);
            var esbEnvironmentsConfigSection = ConfigAccess.GetCustomConfigSection<EsbEnvironmentsSection>(configuration, Constants.CustomConfigSectionGroupName);
            var configReader = new DashboardConfigDataLoader(appSettings, environmentsConfigSection, esbEnvironmentsConfigSection);
            return configReader;
        }

        [TestMethod]
        public void LoadConfigData_WhenHostingEnvironmentValueIsSetInExternalFile_SetsExpectedValue()
        {
            // Arrange
            // value in sample config file: <add key="hostingEnvironment" value="Tst0" />
            var expectedValue = EnvironmentEnum.Tst0;
            var configReader = CreateConfigReaderForExternalFile(webConfigWithStandardData);
            var configDataModel = new DashboardConfigDataModel();
            // Act
            configDataModel.LoadConfigData(configReader);
            // Assert
            Assert.AreEqual(expectedValue, configDataModel.HostingEnvironmentSetting);
        }

        [TestMethod]
        public void LoadConfigData_WhenHostingEnvironmentValueNotSetInExternalFile_SetsDefaultValue()
        {
            var expectedValue = Constants.DashboardAppSettings.DefaultValues.HostingEnvironmentEnum;
            var configReader = CreateConfigReaderForExternalFile(webConfigWithNoData);
            var configDataModel = new DashboardConfigDataModel();
            // Act
            configDataModel.LoadConfigData(configReader);
            // Assert
            Assert.AreEqual(expectedValue, configDataModel.HostingEnvironmentSetting);
        }

        [TestMethod]
        public void LoadConfigData_WhenHostingEnvironmentValueIsSetInTestProjectAppConfig_SetsExpectedValue()
        {
            // Arrange
            // value in test project app.config  <add key="hostingEnvironment" value="Dev0" />
            var expectedValue = EnvironmentEnum.Dev0;
            var configReader = CreateConfigReaderForInternalTestProjectAppConfig();
            var configDataModel = new DashboardConfigDataModel();
            // Act
            configDataModel.LoadConfigData(configReader);
            // Assert
            Assert.AreEqual(expectedValue, configDataModel.HostingEnvironmentSetting);
        }

        [TestMethod]
        public void LoadConfigData_WhenSecondsForRecentErrorsPollingValueIsSetInExternalFile_SetsExpectedValue()
        {
            // Arrange
            // value in sample config file: <add key="intervalForRecentErrorsPollingInSeconds" value="2" />
            var expectedValue = 2000;
            var configReader = CreateConfigReaderForExternalFile(webConfigWithStandardData);
            var configDataModel = new DashboardConfigDataModel();
            // Act
            configDataModel.LoadConfigData(configReader);
            // Assert
            Assert.AreEqual(expectedValue, configDataModel.MillisecondsForRecentErrorsPolling);
        }

        [TestMethod]
        public void LoadConfigData_WhenSecondsForRecentErrorsPollingValueNotSetInExternalFile_SetsDefaultValue()
        {
            // default value is in seconds, should be converted to milliseconds:
            var expectedValue = Constants.DashboardAppSettings.DefaultValues.SecondsForRecentErrorsPolling * 1000;
            var configReader = CreateConfigReaderForExternalFile(webConfigWithNoData);
            var configDataModel = new DashboardConfigDataModel();
            // Act
            configDataModel.LoadConfigData(configReader);
            // Assert
            Assert.AreEqual(expectedValue, configDataModel.MillisecondsForRecentErrorsPolling);
        }

        [TestMethod]
        public void LoadConfigData_WhenSecondsForRecentErrorsPollingValueIsSetInTestProjectAppConfig_SetsExpectedValue()
        {
            // Arrange
            // value in test project app.config  <add key="intervalForRecentErrorsPollingInSeconds" value="3" />
            var expectedValue = 3000;
            var configReader = CreateConfigReaderForInternalTestProjectAppConfig();
            var configDataModel = new DashboardConfigDataModel();
            // Act
            configDataModel.LoadConfigData(configReader);
            // Assert
            Assert.AreEqual(expectedValue, configDataModel.MillisecondsForRecentErrorsPolling);
        }

        [TestMethod]
        public void LoadConfigData_WhenSecurityGroupsWithReadRightsValueIsSetInExternalFile_SetsExpectedValue()
        {
            // Arrange
            // value in sample config file:  <add key="securityGroupsForReadOnly" value="IRMA.Applications,IRMA.Support" />
            var expectedValue = new List<string> { "IRMA.Applications", "IRMA.Support" };
            var configReader = CreateConfigReaderForExternalFile(webConfigWithStandardData);
            var configDataModel = new DashboardConfigDataModel();
            // Act
            configDataModel.LoadConfigData(configReader);
            // Assert
            CustomAsserts.ListsAreEqual(expectedValue, configDataModel.SecurityGroupsWithReadRights);
        }

        [TestMethod]
        public void LoadConfigData_WhenSecurityGroupsWithReadRightsValueNotSetInExternalFile_SetsDefaultValue()
        {
            var expectedValue = Constants.DashboardAppSettings.DefaultValues.SecurityGroupsWithReadOnly.Split(',').ToList();
            var configReader = CreateConfigReaderForExternalFile(webConfigWithNoData);
            var configDataModel = new DashboardConfigDataModel();
            // Act
            configDataModel.LoadConfigData(configReader);
            // Assert
            CustomAsserts.ListsAreEqual(expectedValue, configDataModel.SecurityGroupsWithReadRights);
        }

        [TestMethod]
        public void LoadConfigData_WhenSecurityGroupsWithReadRightsValueIsSetInTestProjectAppConfig_SetsExpectedValue()
        {
            // Arrange
            // value in test project app.config <add key="securityGroupForReadOnly" value="IRMA.Applications" />
            var expectedValue = new List<string> { "IRMA.Applications" };
            var configReader = CreateConfigReaderForInternalTestProjectAppConfig();
            var configDataModel = new DashboardConfigDataModel();
            // Act
            configDataModel.LoadConfigData(configReader);
            // Assert
            CustomAsserts.ListsAreEqual(expectedValue, configDataModel.SecurityGroupsWithReadRights);
        }

        [TestMethod]
        public void LoadConfigData_WhenEnvironmentDefinitionsValueIsSetInExternalFile_SetsExpectedValue()
        {
            // Arrange
            var expectedValue = configTestData.Models.EnvironmentsList.Where(e => e.IsEnabled).ToList();
            var configReader = CreateConfigReaderForExternalFile(webConfigWithStandardData);
            var configDataModel = new DashboardConfigDataModel();
            // Act
            configDataModel.LoadConfigData(configReader);
            // Assert
            CustomAsserts.ListsAreEqual(expectedValue, configDataModel.EnvironmentDefinitions);
        }

        [TestMethod]
        public void LoadConfigData_WhenEnvironmentDefinitionsValueNotSetInExternalFile_SetsDefaultValue()
        {
            var expectedValue = new List<EnvironmentModel> { };
            var configReader = CreateConfigReaderForExternalFile(webConfigWithNoData);
            var configDataModel = new DashboardConfigDataModel();
            // Act
            configDataModel.LoadConfigData(configReader);
            // Assert
            CustomAsserts.ListsAreEqual(expectedValue, configDataModel.EnvironmentDefinitions);
        }

        [TestMethod]
        public void LoadConfigData_WhenEnvironmentDefinitionsIsSetInTestProjectAppConfig_SetsExpectedValue()
        {
            // Arrange
            var expectedValue = configTestData.Models.EnvironmentsList.Where(e => e.IsEnabled).ToList();
            var configReader = CreateConfigReaderForInternalTestProjectAppConfig();
            var configDataModel = new DashboardConfigDataModel();
            // Act
            configDataModel.LoadConfigData(configReader);
            // Assert
            CustomAsserts.ListsAreEqual(expectedValue, configDataModel.EnvironmentDefinitions);
        }

        [TestMethod]
        public void LoadConfigData_WhenEsbEnvironmentDefinitionsValueIsSetInExternalFile_SetsExpectedValue()
        {
            // Arrange
            var expectedValue = configTestData.Models.EsbEnvironmentsList;
            var configReader = CreateConfigReaderForExternalFile(webConfigWithStandardData);
            var configDataModel = new DashboardConfigDataModel();
            // Act
            configDataModel.LoadConfigData(configReader);
            // Assert
            CustomAsserts.ListsAreEqual(expectedValue, configDataModel.EsbEnvironmentDefinitions);
        }

        [TestMethod]
        public void LoadConfigData_WhenEsbEnvironmentDefinitionsValueNotSetInExternalFile_SetsDefaultValue()
        {
            var expectedValue = new List<EsbEnvironmentModel> { };
            var configReader = CreateConfigReaderForExternalFile(webConfigWithNoData);
            var configDataModel = new DashboardConfigDataModel();
            // Act
            configDataModel.LoadConfigData(configReader);
            // Assert
            CustomAsserts.ListsAreEqual(expectedValue, configDataModel.EsbEnvironmentDefinitions);
        }

        [TestMethod]
        public void LoadConfigData_WhenEsbEnvironmentDefinitionsIsSetInTestProjectAppConfig_SetsExpectedValue()
        {
            // Arrange
            var expectedValue = configTestData.Models.EsbEnvironmentsList;
            var configReader = CreateConfigReaderForInternalTestProjectAppConfig();
            var configDataModel = new DashboardConfigDataModel();
            // Act
            configDataModel.LoadConfigData(configReader);
            // Assert
            CustomAsserts.ListsAreEqual(expectedValue, configDataModel.EsbEnvironmentDefinitions);
        }
    }
}
