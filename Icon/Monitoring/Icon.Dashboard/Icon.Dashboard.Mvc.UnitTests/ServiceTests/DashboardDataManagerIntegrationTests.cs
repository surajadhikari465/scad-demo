using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.Models.CustomConfigElements;
using Icon.Dashboard.Mvc.Services;
using Icon.Dashboard.Mvc.UnitTests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Icon.Dashboard.Mvc.UnitTests.ServiceTests
{
    [TestClass]
    public class DashboardDataManagerIntegrationTests
    {
        ConfigTestData configTestData = new ConfigTestData();
        Mock<IDashboardConfigDataLoader> mockConfigDataLoader = new Mock<IDashboardConfigDataLoader>();

        [TestMethod]
        public void Constructor_LoadsConfigData()
        {
            // Arrange
            var configData = new DashboardConfigDataModel(mockConfigDataLoader.Object);
            // Act
            var dashboardDataService = new DashboardDataManager(configData);
            // Assert
            Assert.IsNotNull(dashboardDataService.ConfigData);
        }

        [TestMethod]
        public void Constructor_LoadsConfigData_ForHostingEnvironmentSetting()
        {
            // Arrange
            var expectedVal = EnvironmentEnum.Tst0;
            mockConfigDataLoader.Setup(a => a
                .GetTypedSettingValueOrDefault(Constants.DashboardAppSettings.Keys.HostingEnvironment, It.IsAny<EnvironmentEnum>()))
                .Returns(expectedVal);
            var configData = new DashboardConfigDataModel(mockConfigDataLoader.Object);
            // Act
            var dashboardDataService = new DashboardDataManager(configData);
            // Assert
            Assert.AreEqual(expectedVal, dashboardDataService.ConfigData.HostingEnvironmentSetting);
        }

        [TestMethod]
        public void Constructor_SetsHostingEnvironment()
        {
            // Arrange
            var hostingEnvironmentSetting = EnvironmentEnum.Tst0;
            mockConfigDataLoader.Setup(a => a
                .GetTypedSettingValueOrDefault(Constants.DashboardAppSettings.Keys.HostingEnvironment, It.IsAny<EnvironmentEnum>()))
                .Returns(hostingEnvironmentSetting);
            mockConfigDataLoader.Setup(r => r.GetEnvironments())
                .Returns(configTestData.Models.EnvironmentsList);
            var configData = new DashboardConfigDataModel(mockConfigDataLoader.Object);
            // Act
            var dashboardDataService = new DashboardDataManager(configData);
            // Assert
            Assert.IsNotNull(dashboardDataService.HostingEnvironment);
            //Assert.IsTrue(dashboardDataService.HostingEnvironmentIsSet);
            Assert.AreEqual(hostingEnvironmentSetting, dashboardDataService.HostingEnvironment.EnvironmentEnum);
        }

        [TestMethod]
        public void Constructor_SetsHostingEnvironmentToTst0_WithAllExpectedProperties()
        {
            // Arrange
            var hostingEnvironmentSetting = EnvironmentEnum.Tst0;
            mockConfigDataLoader.Setup(a => a
                .GetTypedSettingValueOrDefault(Constants.DashboardAppSettings.Keys.HostingEnvironment, It.IsAny<EnvironmentEnum>()))
                .Returns(hostingEnvironmentSetting);
            mockConfigDataLoader.Setup(r => r.GetEnvironments())
                .Returns(configTestData.Models.EnvironmentsList);
            var configData = new DashboardConfigDataModel(mockConfigDataLoader.Object);
            // Act
            var dashboardDataService = new DashboardDataManager(configData);
            // Assert
            var hostingEnv = dashboardDataService.HostingEnvironment;
            Assert.AreEqual("Tst0", hostingEnv.Name);
            Assert.AreEqual(EnvironmentEnum.Tst0, hostingEnv.EnvironmentEnum);
            Assert.AreEqual(true, hostingEnv.IsEnabled);
            Assert.AreEqual("http://irmatestapp1/IconDashboard", hostingEnv.DashboardUrl);
            Assert.AreEqual("irmatestapp1", hostingEnv.WebServer);
            CustomAsserts.ListsAreEqual(
                new List<string> { "vm-icon-test1", "vm-icon-test2" },
                hostingEnv.AppServers);
            Assert.AreEqual(@"http://icon-test/", hostingEnv.IconWebUrl);
            Assert.AreEqual(@"http://irmatestapp1/MammothWebSupport", hostingEnv.MammothWebSupportUrl);
            CustomAsserts.ListsAreEqual(
                new List<string> { @"https://cerd1669:18090/", @"https://cerd1670:18090/" },
                hostingEnv.TibcoAdminUrls);
            Assert.AreEqual(@"CEWD1815\SQLSHARED2012D", hostingEnv.IconDatabaseServer);
            Assert.AreEqual("iCON", hostingEnv.IconDatabaseName);
            Assert.AreEqual(@"MAMMOTH-DB01-DEV\MAMMOTH", hostingEnv.MammothDatabaseServer);
            Assert.AreEqual("Mammoth", hostingEnv.MammothDatabaseName);
            CustomAsserts.ListsAreEqual(
                new List<string> { @"idt-nc\nct", @"idt-ne\net", @"idt-pn\pnt", @"idt-sp\spt", @"idt-sw\swt", @"idt-uk\ukt" }
                , hostingEnv.IrmaDatabaseServers);
            Assert.AreEqual("ItemCatalog_Test", hostingEnv.IrmaDatabaseName);
        }

        [TestMethod]
        public void Constructor_SetsHostingEnvironmentToPerf_WithAllExpectedProperties()
        {
            // Arrange
            var hostingEnvironmentSetting = EnvironmentEnum.Perf;
            mockConfigDataLoader.Setup(a => a
                .GetTypedSettingValueOrDefault(Constants.DashboardAppSettings.Keys.HostingEnvironment, It.IsAny<EnvironmentEnum>()))
                .Returns(hostingEnvironmentSetting);
            mockConfigDataLoader.Setup(r => r.GetEnvironments())
                .Returns(configTestData.Models.EnvironmentsList);
            var configData = new DashboardConfigDataModel(mockConfigDataLoader.Object);
            // Act
            var dashboardDataService = new DashboardDataManager(configData);
            // Assert
            var hostingEnv = dashboardDataService.HostingEnvironment;
            Assert.AreEqual("Perf", hostingEnv.Name);
            Assert.AreEqual(EnvironmentEnum.Perf, hostingEnv.EnvironmentEnum);
            Assert.AreEqual(true, hostingEnv.IsEnabled);
            Assert.AreEqual("http://irmaqaapp1/IconDashboardPerf", hostingEnv.DashboardUrl);
            Assert.AreEqual("irmaqaapp1", hostingEnv.WebServer);
            CustomAsserts.ListsAreEqual(
                new List<string> { "vm-icon-qa3", "vm-icon-qa4" },
                hostingEnv.AppServers);
            Assert.AreEqual(@"http://icon-perf/", hostingEnv.IconWebUrl);
            Assert.AreEqual(@"http://irmaqaapp1/MammothWebSupportPerf", hostingEnv.MammothWebSupportUrl);
            CustomAsserts.ListsAreEqual(
                new List<string> { @"https://cerd1666:28090/", @"https://cerd1667:28090/" },
                hostingEnv.TibcoAdminUrls);
            Assert.AreEqual(@"CEWD1815\SQLSHARED2012D", hostingEnv.IconDatabaseServer);
            Assert.AreEqual("iCONLoadTest", hostingEnv.IconDatabaseName);
            Assert.AreEqual(@"qa-01-mammoth02\mammoth02", hostingEnv.MammothDatabaseServer);
            Assert.AreEqual("Mammoth", hostingEnv.MammothDatabaseName);
            CustomAsserts.ListsAreEqual(hostingEnv.IrmaDatabaseServers , hostingEnv.IrmaDatabaseServers);
            Assert.AreEqual("ItemCatalog", hostingEnv.IrmaDatabaseName);
        }

        [TestMethod]
        public void Constructor_SetsActiveEnvironment()
        {
            // Arrange
            var hostingEnvironmentSetting = EnvironmentEnum.Tst0;
            mockConfigDataLoader.Setup(a => a
                .GetTypedSettingValueOrDefault(Constants.DashboardAppSettings.Keys.HostingEnvironment, It.IsAny<EnvironmentEnum>()))
                .Returns(hostingEnvironmentSetting);
            mockConfigDataLoader.Setup(r => r.GetEnvironments())
                .Returns(configTestData.Models.EnvironmentsList);
            var configData = new DashboardConfigDataModel(mockConfigDataLoader.Object);
            // Act
            var dashboardDataService = new DashboardDataManager(configData);
            // Assert
            Assert.IsNotNull(dashboardDataService.ActiveEnvironment);
            Assert.AreEqual(hostingEnvironmentSetting, dashboardDataService.ActiveEnvironment.EnvironmentEnum);
            Assert.AreEqual(hostingEnvironmentSetting.ToString(), dashboardDataService.ActiveEnvironment.Name);
        }
    }
}
