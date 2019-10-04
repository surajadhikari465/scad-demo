using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Models.CustomConfigElements;
using Icon.Dashboard.Mvc.Services;
using Icon.Dashboard.Mvc.UnitTests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.Mvc.UnitTests.ServiceTests
{
    [TestClass]
    public class ConfigDataLoaderUnitTests
    {
        ConfigTestData configTestData = new ConfigTestData();
        Dictionary<string,string> testAppSettings = new Dictionary<string,string>();
        EnvironmentsSection environmentsConfigSection = null;
        EsbEnvironmentsSection esbEnvironmentsConfigSection = null;

        const string testDataFolder = "TestData";
        string webConfigWithStandardData = $"{testDataFolder}\\SampleWebConfigWithStandardValues.config";
        string webConfigWithNoData = $"{testDataFolder}\\SampleWebConfigWithNoValues.config";
        string serviceAppConfigWithEsbSection = $"{testDataFolder}\\SampleAppConfig_MultiEsbPerf_MammothProductListener.exe.config";

        [TestInitialize]
        public void TestInitialize()
        {
            environmentsConfigSection = configTestData.Elements.EnvironmentsSection;
            esbEnvironmentsConfigSection = configTestData.Elements.EsbEnvironmentsSection;
        }

        [TestMethod]
        public void GetTypedSettingValueOrDefault_ForIntValue_CallsAccessorWithParametersAndReturnsValue()
        {
            // Arrange
            var key = "myIntSetting";
            var expectedValue = 728;
            var defaultValue = 44;
            testAppSettings.Add(key, expectedValue.ToString());
            var configDataLoader = new DashboardConfigDataLoader(testAppSettings, environmentsConfigSection, esbEnvironmentsConfigSection);

            // Act
            var actualValue = configDataLoader.GetTypedSettingValueOrDefault(key, defaultValue);

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void GetTypedSettingValueOrDefault_ForStringValue_CallsAccessorWithParametersAndReturnsValue()
        {
            // Arrange
            var key = "myStringSetting";
            var expectedValue = "Atlantic";
            var defaultValue = "Pacific";
            testAppSettings.Add(key, expectedValue.ToString());
            var configDataLoader = new DashboardConfigDataLoader(testAppSettings, environmentsConfigSection, esbEnvironmentsConfigSection);

            // Act
            var actualValue = configDataLoader.GetTypedSettingValueOrDefault(key, defaultValue);

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void GetTypedSettingValueOrDefault_ForEnumValue_CallsAccessorWithParametersAndReturnsValue()
        {
            // Arrange
            var key = "myEnumSetting";
            var expectedValue = EsbEnvironmentEnum.QA_DUP;
            var defaultValue = EsbEnvironmentEnum.TEST_DUP;
            testAppSettings.Add(key, expectedValue.ToString());
            var configDataLoader = new DashboardConfigDataLoader(testAppSettings, environmentsConfigSection, esbEnvironmentsConfigSection);

            // Act
            var actualValue = configDataLoader.GetTypedSettingValueOrDefault(key, defaultValue);

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void SplitAppSettingValuesToList_WhenDelimitedValueFound_ShouldReturnValueSplitAsList()
        {
            // Arrange
            var key = "myCommaSeparatedSetting";
            var expectedValue = new List<string> { "Atlantic", "Pacific", "Arctic" };
            var defaultValue = "Mediterranean,Black";
            testAppSettings.Add(key, "Atlantic,Pacific,Arctic");
            var configDataLoader = new DashboardConfigDataLoader(testAppSettings, environmentsConfigSection, esbEnvironmentsConfigSection);

            // Act
            var actualValue = configDataLoader.SplitAppSettingValuesToList(key, defaultValue);

            // Assert
            CustomAsserts.ListsAreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void SplitAppSettingValuesToList_WhenNonDelimitedValueFound_ShouldReturnValueAsSingleItemList()
        {
            // Arrange
            var key = "myCommaSeparatedSetting";
            var expectedValue = new List<string> { "Pacific" };
            var defaultValue = "Mediterranean,Black";
            testAppSettings.Add(key, "Pacific");
            var configDataLoader = new DashboardConfigDataLoader(testAppSettings, environmentsConfigSection, esbEnvironmentsConfigSection);

            // Act
            var actualValue = configDataLoader.SplitAppSettingValuesToList(key, defaultValue);

            // Assert
            CustomAsserts.ListsAreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void SplitAppSettingValuesToList_WhenDefaultDelimitedValueFound_ShouldReturnDefaultValueAsList()
        {
            // Arrange
            var key = "myCommaSeparatedSetting";
            var expectedValue = new List<string> { "Atlantic", "Pacific", "Arctic" };
            var defaultValue = "Atlantic,Pacific,Arctic";
            testAppSettings.Add(key, defaultValue);
            var configDataLoader = new DashboardConfigDataLoader(testAppSettings, environmentsConfigSection, esbEnvironmentsConfigSection);

            // Act
            var actualValue = configDataLoader.SplitAppSettingValuesToList(key, defaultValue);

            // Assert
            CustomAsserts.ListsAreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void SplitAppSettingValuesToList_WhenDefaultNonDelimitedValueFound_ShouldReturnDefaultValueAsSingleItemList()
        {
            // Arrange
            var key = "myCommaSeparatedSetting";
            var expectedValue = new List<string> { "Pacific" };
            var defaultValue = "Pacific";
            testAppSettings.Add(key, defaultValue);
            var configDataLoader = new DashboardConfigDataLoader(testAppSettings, environmentsConfigSection, esbEnvironmentsConfigSection);

            // Act
            var actualValue = configDataLoader.SplitAppSettingValuesToList(key, defaultValue);

            // Assert
            CustomAsserts.ListsAreEqual(expectedValue, actualValue);
        }


        [TestMethod]
        public void ConfigSectionToEnvironmentModels_SetsExpectedProperties_ForDev0Environment()
        {
            // Arrange
            var configDataLoader = new DashboardConfigDataLoader(testAppSettings, environmentsConfigSection, esbEnvironmentsConfigSection);
            // Act
            var enviromentModels = configDataLoader.ConfigSectionToEnvironmentModels(configTestData.Elements.EnvironmentsSection);
            // Assert
            Assert.IsNotNull(enviromentModels);
            var env = enviromentModels.Single(e => e.EnvironmentEnum == EnvironmentEnum.Dev0);
            Assert.IsNotNull(env);
            Assert.AreEqual("Dev0", env.Name);
            Assert.AreEqual(EnvironmentEnum.Dev0, env.EnvironmentEnum);
            Assert.AreEqual(true, env.IsEnabled);
            Assert.AreEqual("http://irmadevapp1/IconDashboard", env.DashboardUrl);
            Assert.AreEqual("irmadevapp1", env.WebServer);
            CustomAsserts.ListsAreEqual(
                new List<string> { "vm-icon-dev1" },
                env.AppServers);
            Assert.AreEqual(@"http://icon-dev/", env.IconWebUrl);
            CustomAsserts.ListsAreEqual(
                new List<string> { @"https://cerd1668:8090/" },
                env.TibcoAdminUrls);
            Assert.AreEqual(@"cewd1815\sqlshared2012d", env.IconDatabaseServer);
            Assert.AreEqual("iCONDev", env.IconDatabaseName);
            Assert.AreEqual(@"MAMMOTH-DB01-DEV\MAMMOTH", env.MammothDatabaseServer);
            Assert.AreEqual("Mammoth_Dev", env.MammothDatabaseName);
            CustomAsserts.ListsAreEqual(
                new List<string> { @"idd-fl\fld", @"idd-ma\mad", @"idd-mw\mwd", @"idd-na\nad", @"idd-rm\rmd", @"idd-so\sod" }
                , env.IrmaDatabaseServers);
            Assert.AreEqual("ItemCatalog_Test", env.IrmaDatabaseName);
        }
    } 
}
