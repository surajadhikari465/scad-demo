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

namespace Icon.Dashboard.Mvc.UnitTests.ServiceTests
{
    [TestClass]
    public class ConfigReaderIntegrationTests
    {
        ConfigTestData configTestData = new ConfigTestData();
        string webConfigWithStandardData = $"TestData\\SampleWebConfigWithStandardValues.config";
        string webConfigWithNoData = $"TestData\\SampleWebConfigWithNoValues.config";

        [TestMethod]
        public void GetEnvironments_WhenEnvironmentDataExits_GetsEnvironmentsList()
        {
            // Arrange
            var configuration = ConfigAccess.OpenExternalAppConfiguration(webConfigWithStandardData);
            var appSettings = ConfigAccess.GetAppSettingsAsDictionary(configuration);
            var environmentsConfigSection = ConfigAccess.GetCustomConfigSection<EnvironmentsSection>(configuration, Constants.CustomConfigSectionGroupName);
            var esbEnvironmentsConfigSection = ConfigAccess.GetCustomConfigSection<EsbEnvironmentsSection>(configuration, Constants.CustomConfigSectionGroupName);
            var configDataReader = new DashboardConfigDataLoader(appSettings, environmentsConfigSection, esbEnvironmentsConfigSection);
            // Act
            var environments = configDataReader.GetEnvironments();
            // Assert
            Assert.IsNotNull(environments);
        }

        [TestMethod]
        public void GetEnvironments_WhenEnvironmentDataExits_GetsExpectedEnvironmentsList()
        {
            // Arrange
            var configuration = ConfigAccess.OpenExternalAppConfiguration(webConfigWithStandardData);
            var appSettings = ConfigAccess.GetAppSettingsAsDictionary(configuration);
            var environmentsConfigSection = ConfigAccess.GetCustomConfigSection<EnvironmentsSection>(configuration, Constants.CustomConfigSectionGroupName);
            var esbEnvironmentsConfigSection = ConfigAccess.GetCustomConfigSection<EsbEnvironmentsSection>(configuration, Constants.CustomConfigSectionGroupName);
            var configDataReader = new DashboardConfigDataLoader(appSettings, environmentsConfigSection, esbEnvironmentsConfigSection);
            // Act
            var environments = configDataReader.GetEnvironments();
            // Assert
            var dev0Env = environments.Single(e => e.Name == EnvironmentEnum.Dev0.ToString());
            Assert.IsNotNull(dev0Env);
            Assert.AreEqual("Dev0", dev0Env.Name);
            //Assert.AreEqual(EnvironmentEnum.Dev0, env.EnvironmentEnum);
            Assert.AreEqual(true, dev0Env.IsEnabled);
            Assert.AreEqual("http://irmadevapp1/IconDashboard", dev0Env.DashboardUrl);
            Assert.AreEqual("irmadevapp1", dev0Env.WebServer);
            CustomAsserts.ListsAreEqual(new List<string> { "vm-icon-dev1" }, dev0Env.AppServers);
            Assert.AreEqual(@"http://icon-dev/", dev0Env.IconWebUrl);
            CustomAsserts.ListsAreEqual(new List<string> { @"https://cerd1668:8090/" }, dev0Env.TibcoAdminUrls);
            Assert.AreEqual(@"cewd1815\sqlshared2012d", dev0Env.IconDatabaseServer);
            Assert.AreEqual("iCONDev", dev0Env.IconDatabaseName);
            Assert.AreEqual(@"MAMMOTH-DB01-DEV\MAMMOTH", dev0Env.MammothDatabaseServer);
            Assert.AreEqual("Mammoth_Dev", dev0Env.MammothDatabaseName);
            CustomAsserts.ListsAreEqual(new List<string> { @"https://cerd1668:8090/" }, dev0Env.TibcoAdminUrls);
            var expectedIrmaDbList = new List<string>
            {
                @"idd-fl\fld", @"idd-ma\mad", @"idd-mw\mwd", @"idd-na\nad", @"idd-rm\rmd" ,@"idd-so\sod"
            };
            CustomAsserts.ListsAreEqual(expectedIrmaDbList, dev0Env.IrmaDatabaseServers);
            Assert.AreEqual("ItemCatalog_Test", dev0Env.IrmaDatabaseName);
        }

        [TestMethod]
        public void GetEsbEnvironments_WhenEsbEnvironmentDataExits_GetsEsbEnvironmentsList()
        {
            // Arrange
            var configuration = ConfigAccess.OpenExternalAppConfiguration(webConfigWithStandardData);
            var appSettings = ConfigAccess.GetAppSettingsAsDictionary(configuration);
            var environmentsConfigSection = ConfigAccess.GetCustomConfigSection<EnvironmentsSection>(configuration, Constants.CustomConfigSectionGroupName);
            var esbEnvironmentsConfigSection = ConfigAccess.GetCustomConfigSection<EsbEnvironmentsSection>(configuration, Constants.CustomConfigSectionGroupName);
            var configDataReader = new DashboardConfigDataLoader(appSettings, environmentsConfigSection, esbEnvironmentsConfigSection);
            // Act
            var section = configDataReader.GetEsbEnvironments();
            // Assert
            Assert.IsNotNull(section);
        }
    }
}
