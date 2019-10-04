using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Services;
using Icon.Dashboard.Mvc.UnitTests.TestData;
using Icon.Dashboard.RemoteServicesAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Icon.Dashboard.Mvc.UnitTests.ServiceTests
{
    [TestClass]
    public class ExternalConfigXmlManagerIntegrationTests
    {
        RemoteServiceTestData serviceTestData = new RemoteServiceTestData();
        DbWrapperTestData dbWrapperTestData = new DbWrapperTestData();
        ConfigTestData configTestData = new ConfigTestData();
        Mock<IRemoteWmiAccessService> mockWMiSvc = new Mock<IRemoteWmiAccessService>();
        Mock<IIconDatabaseServiceWrapper> mockIconDbService = new Mock<IIconDatabaseServiceWrapper>();
        Mock<IMammothDatabaseServiceWrapper> mockMammothDbService = new Mock<IMammothDatabaseServiceWrapper>();
        string testDataFolder = @"TestData";

        private Dictionary<string, string> ReadAppSettingsFromFile(string path)
        {
            var configXmlDoc = XDocument.Load(path);
            if (configXmlDoc != null)
            {
                var appSettingsElement = configXmlDoc.Root.Element("appSettings");
                if (appSettingsElement != null && appSettingsElement.Elements() != null)
                {
                    return appSettingsElement.Elements().Select(e =>
                            new KeyValuePair<string, string>(e.Attribute("key").Value, e.Attribute("value").Value))
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                }
            }
            return null;
        }

        [TestMethod]
        public void ReadConfigData_WhenReadingConfigForIconGlocon_ReadsExpectedLoggingId()
        {
            // Arrange
            var configFilePath = $"{testDataFolder}\\SampleAppConfig_NoEsb_GloCon.exe.config";
            var configReader = new ExternalConfigXmlManager(configFilePath);
            var expectedLoggingId = 7;
            // Act
            var configData = configReader.ReadConfigData(configFilePath, configTestData.Models.EnvironmentsList, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.AreEqual(expectedLoggingId, configData.LoggingID);
        }

        [TestMethod]
        public void ReadConfigData_WhenReadingConfigForIconGlocon_ReadsExpectedAppSettings()
        {
            // Arrange
            var configFilePath = $"{testDataFolder}\\SampleAppConfig_NoEsb_GloCon.exe.config";
            var configReader = new ExternalConfigXmlManager(configFilePath);
            var expectedAppSettingsDictionary = serviceTestData.Services.AppSettings.FakeAppSettings_IconGloCon_NoEsb;
            // Act
            var configData = configReader.ReadConfigData(configFilePath, configTestData.Models.EnvironmentsList, configTestData.Models.EsbEnvironmentsList);
            // Assert
            CustomAsserts.AssertDictionariesEqual(expectedAppSettingsDictionary, configData.NonEsbAppSettings);
        }

        [TestMethod]
        public void ReadConfigData_WhenReadingConfigForMammothItemLocaleController__ReadsExpectedLoggingId()
        {
            // Arrange
            var configFilePath = $"{testDataFolder}\\SampleAppConfig_NoEsb_MammothItemLocale.exe.config";
            var configReader = new ExternalConfigXmlManager(configFilePath);
            var expectedLoggingId = 2;
            // Act
            var configData = configReader.ReadConfigData(configFilePath, configTestData.Models.EnvironmentsList, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.AreEqual(expectedLoggingId, configData.LoggingID);
        }

        [TestMethod]
        public void ReadConfigData_WhenReadingConfigForMammothItemLocaleController_ReadsExpectedAppSettings()
        {
            // Arrange
            var configFilePath = $"{testDataFolder}\\SampleAppConfig_NoEsb_MammothItemLocale.exe.config";
            var configReader = new ExternalConfigXmlManager(configFilePath);
            var expectedAppSettingsDictionary = serviceTestData.Services.AppSettings.FakeAppSettings_MammothItemLocaleController_NoEsb;
            // Act
            var configData = configReader.ReadConfigData(configFilePath, configTestData.Models.EnvironmentsList, configTestData.Models.EsbEnvironmentsList);
            // Assert
            CustomAsserts.AssertDictionariesEqual(expectedAppSettingsDictionary, configData.NonEsbAppSettings);
        }

        [TestMethod]
        public void ReadConfigData_WhenReadingConfigForIconEwicListener_ReadsExpectedLoggingId()
        {
            // Arrange
            var configFilePath = $"{testDataFolder}\\SampleAppConfig_MultiEsbTst_EwicAplListener.exe.config";
            var configReader = new ExternalConfigXmlManager(configFilePath);
            var expectedLoggingId = 3;
            // Act
            var configData = configReader.ReadConfigData(configFilePath, configTestData.Models.EnvironmentsList, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.AreEqual(expectedLoggingId, configData.LoggingID);
        }

        [TestMethod]
        public void ReadConfigData_WhenReadingConfigForIconEwicListener_ReadsExpectedAppSettings()
        {
            // Arrange
            var configFilePath = $"{testDataFolder}\\SampleAppConfig_MultiEsbTst_EwicAplListener.exe.config";
            var configReader = new ExternalConfigXmlManager(configFilePath);
            var expectedAppSettingsDictionary = serviceTestData.Services.AppSettings.FakeAppSettings_IconEwicAplListener_ESB_Tst;
            // Act
            var configData = configReader.ReadConfigData(configFilePath, configTestData.Models.EnvironmentsList, configTestData.Models.EsbEnvironmentsList);
            // Assert
            CustomAsserts.AssertDictionariesEqual(expectedAppSettingsDictionary, configData.NonEsbAppSettings);
        }

        [TestMethod]
        public void ReadConfigData_WhenReadingConfigForMammothProductListener_ReadsExpectedLoggingId()
        {
            // Arrange
            var configFilePath = $"{testDataFolder}\\SampleAppConfig_MultiEsbPerf_MammothProductListener.exe.config";
            var configReader = new ExternalConfigXmlManager(configFilePath);
            var expectedLoggingId = 5;
            // Act
            var configData = configReader.ReadConfigData(configFilePath, configTestData.Models.EnvironmentsList, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.AreEqual(expectedLoggingId, configData.LoggingID);
        }

        [TestMethod]
        public void ReadConfigData_WhenReadingConfigForMammothProductListener_ReadsExpectedNonEsbAppSettings()
        {
            // Arrange
            var configFilePath = $"{testDataFolder}\\SampleAppConfig_MultiEsbPerf_MammothProductListener.exe.config";
            var configReader = new ExternalConfigXmlManager(configFilePath);
            var expectedAppSettingsDictionary = serviceTestData.Services.AppSettings.FakeAppSettings_MammothProductListener_ESB_Perf;
            // Act
            var configData = configReader.ReadConfigData(configFilePath, configTestData.Models.EnvironmentsList, configTestData.Models.EsbEnvironmentsList);
            // Assert
            CustomAsserts.AssertDictionariesEqual(expectedAppSettingsDictionary, configData.NonEsbAppSettings);
        }

        [TestMethod]
        public void ReadConfigData_WhenReadingConfigForMammothProductListener_ReadsEsbSettingsFromCustomConfigSection()
        {
            // Arrange
            var configFilePath = $"{testDataFolder}\\SampleAppConfig_MultiEsbPerf_MammothProductListener.exe.config";
            var configReader = new ExternalConfigXmlManager(configFilePath);
            var expectedAppSettingsDictionary = serviceTestData.Services.AppSettings.FakeAppSettings_MammothProductListener_ESB_Perf;
            // Act
            var configData = configReader.ReadConfigData(configFilePath, configTestData.Models.EnvironmentsList, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.IsTrue(configData.HasEsbSettingsInCustomConfigSection);
        }

        [TestMethod]
        public void ReadConfigData_WhenReadingConfigForMammothProductListener_ReadsExpectedEsbConnectionsCount()
        {
            // Arrange
            var configFilePath = $"{testDataFolder}\\SampleAppConfig_MultiEsbPerf_MammothProductListener.exe.config";
            var configReader = new ExternalConfigXmlManager(configFilePath);
            // Act
            var configData = configReader.ReadConfigData(configFilePath, configTestData.Models.EnvironmentsList, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.IsNotNull(configData.EsbConnections);
            Assert.AreEqual(2, configData.EsbConnections.Count);
        }

        [TestMethod]
        public void ReadConfigData_WhenReadingConfigForMammothProductListener_ReadsExpectedEsbConnectionsWithNames()
        {
            // Arrange
            var configFilePath = $"{testDataFolder}\\SampleAppConfig_MultiEsbPerf_MammothProductListener.exe.config";
            var configReader = new ExternalConfigXmlManager(configFilePath);
            var expectedAppSettingsDictionary = serviceTestData.Services.AppSettings.FakeAppSettings_MammothProductListener_ESB_Perf;
            // Act
            var configData = configReader.ReadConfigData(configFilePath, configTestData.Models.EnvironmentsList, configTestData.Models.EsbEnvironmentsList);
            // Assert
            var esbConnection = configData.EsbConnections.Single(c => c.ConnectionName == "ESB");
            var r10Connection = configData.EsbConnections.Single(c => c.ConnectionName == "R10");
        }

        [TestMethod]
        public void ReadConfigData_WhenReadingConfigForMammothProductListener_ReadsExpectedEsbConnectionsWithAllData()
        {
            // Arrange
            var configFilePath = $"{testDataFolder}\\SampleAppConfig_MultiEsbPerf_MammothProductListener.exe.config";
            var configReader = new ExternalConfigXmlManager(configFilePath);
            // Act
            var configData = configReader.ReadConfigData(configFilePath, configTestData.Models.EnvironmentsList, configTestData.Models.EsbEnvironmentsList);
            // Assert
            var esbConnection = configData.EsbConnections.Single(c => c.ConnectionName == "ESB");
            Assert.AreEqual("ESB", esbConnection.ConnectionName);
            Assert.AreEqual(EsbConnectionTypeEnum.Mammoth, esbConnection.ConnectionType);
            Assert.AreEqual("ssl://PERF-ESB-EMS-1.wfm.pvt:27293,ssl://PERF-ESB-EMS-2.wfm.pvt:27293", esbConnection.ServerUrl);
            Assert.AreEqual("mammothUser", esbConnection.JmsUsername);
            Assert.AreEqual("jmsM@mm24;PERF", esbConnection.JmsPassword);
            Assert.AreEqual("jndiMammothUser", esbConnection.JndiUsername);
            Assert.AreEqual("jndiM@mm$$$PERF", esbConnection.JndiPassword);
            Assert.AreEqual("ItemQueueConnectionFactory", esbConnection.ConnectionFactoryName);
            Assert.AreEqual("esb", esbConnection.SslPassword);
            Assert.AreEqual("ClientAcknowledge", esbConnection.SessionMode);
            Assert.AreEqual("E=uday.bhaskar@wholefoods.com, CN=uday.bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=Austin, S=TX, C=US", esbConnection.CertificateName);
            Assert.AreEqual("Root", esbConnection.CertificateStoreName);
            Assert.AreEqual("LocalMachine", esbConnection.CertificateStoreLocation);
            Assert.AreEqual("PERF-ESB-EMS-1.wfm.pvt", esbConnection.TargetHostName);
            Assert.AreEqual("WFMESB.Enterprise.Item.ItemGateway.Queue.V2", esbConnection.QueueName);
            Assert.AreEqual("30000", esbConnection.ReconnectDelay);

            var r10Connection = configData.EsbConnections.Single(c => c.ConnectionName == "R10");
            Assert.AreEqual(EsbConnectionTypeEnum.Icon, r10Connection.ConnectionType);
            Assert.AreEqual("R10", r10Connection.ConnectionName);
            Assert.AreEqual("ssl://PERF-ESB-EMS-1.wfm.pvt:27293,ssl://PERF-ESB-EMS-2.wfm.pvt:27293", r10Connection.ServerUrl);
            Assert.AreEqual("iconUser", r10Connection.JmsUsername);
            Assert.AreEqual("jms!C0nPERF", r10Connection.JmsPassword);
            Assert.AreEqual("jndiIconUser", r10Connection.JndiUsername);
            Assert.AreEqual("jndiIconUserPERF", r10Connection.JndiPassword);
            Assert.AreEqual("ItemQueueConnectionFactory", r10Connection.ConnectionFactoryName);
            Assert.AreEqual("esb", r10Connection.SslPassword);
            Assert.AreEqual("ClientAcknowledge", r10Connection.SessionMode);
            Assert.AreEqual("E=uday.bhaskar@wholefoods.com, CN=uday.bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=Austin, S=TX, C=US", r10Connection.CertificateName);
            Assert.AreEqual("Root", r10Connection.CertificateStoreName);
            Assert.AreEqual("LocalMachine", r10Connection.CertificateStoreLocation);
            Assert.AreEqual("PERF-ESB-EMS-1.wfm.pvt", r10Connection.TargetHostName);
            Assert.AreEqual("WFMESB.Commerce.Retail.ItemMgmt.Item.Queue.V2", r10Connection.QueueName);
        }

        [TestMethod]
        public void ReadConfigData_WhenReadingConfigForIconInforListener_ReadsExpectedLoggingId()
        {
            // Arrange
            var configFilePath = $"{testDataFolder}\\SampleAppConfig_MultiEsbDup_IconInforListener.exe.config";
            var configReader = new ExternalConfigXmlManager(configFilePath);
            var expectedLoggingId = 16;
            // Act
            var configData = configReader.ReadConfigData(configFilePath, configTestData.Models.EnvironmentsList, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.AreEqual(expectedLoggingId, configData.LoggingID);
        }

        [TestMethod]
        public void ReadConfigData_WhenReadingConfigForIconInforListener_ReadsExpectedAppSettings()
        {
            // Arrange
            var configFilePath = $"{testDataFolder}\\SampleAppConfig_MultiEsbDup_IconInforListener.exe.config";
            var configReader = new ExternalConfigXmlManager(configFilePath);
            var expectedAppSettingsDictionary = serviceTestData.Services.AppSettings.FakeAppSettings_IconInforListener_ESB_Dup;
            // Act
            var configData = configReader.ReadConfigData(configFilePath, configTestData.Models.EnvironmentsList, configTestData.Models.EsbEnvironmentsList);
            // Assert
            CustomAsserts.AssertDictionariesEqual(expectedAppSettingsDictionary, configData.NonEsbAppSettings);
        }

        [TestMethod]
        public void ReadConfigData_WhenReadingConfigForMammothApiController_ReadsExpectedLoggingId()
        {
            // Arrange
            var configFilePath = $"{testDataFolder}\\SampleAppConfig_EsbTst_MammothApiController.exe.config";
            var configReader = new ExternalConfigXmlManager(configFilePath);
            var expectedLoggingId = 4;
            // Act
            var configData = configReader.ReadConfigData(configFilePath, configTestData.Models.EnvironmentsList, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.AreEqual(expectedLoggingId, configData.LoggingID);
        }

        [TestMethod]
        public void ReadConfigData_WhenReadingConfigForMammothApiController_ReadsExpectedNonEsbAppSettings()
        {
            // Arrange
            var configFilePath = $"{testDataFolder}\\SampleAppConfig_EsbTst_MammothApiController.exe.config";
            var expectedAppSettingsDictionary = serviceTestData.Services.AppSettings.FakeAppSettings_MammothApiController_ESB_Tst_NonEsbSettingsOnly;
            var configReader = new ExternalConfigXmlManager();
            // Act
            var configData = configReader.ReadConfigData(configFilePath, configTestData.Models.EnvironmentsList, configTestData.Models.EsbEnvironmentsList);
            // Assert
            CustomAsserts.AssertDictionariesEqual(expectedAppSettingsDictionary, configData.NonEsbAppSettings);
        }

        [TestMethod]
        public void RewriteAppSettings_WhenValidPath_UpdatesProvidedAppSettings()
        {
            // Arrange
            var configUpdater = new ExternalConfigXmlManager();
            var testConfigFileToCopy = $"{testDataFolder}\\SampleAppConfig_EsbTst_MammothApiController.exe.config";
            var configFilePath = testConfigFileToCopy.Replace(".exe.config", "2.exe.config");
            File.Copy(testConfigFileToCopy, configFilePath, true);
            var appSettingsToUpdate = new Dictionary<string, string>
            {
                {"ServerUrl", "ssl://PERF-ESB-EMS-1.wfm.pvt:27293,ssl://PERF-ESB-EMS-2.wfm.pvt:27293"},
                {"JmsPassword", "jms!C0nPERF"},
                {"JndiPassword", "jndiIconUserPERF"},
                {"CertificateName", "E=uday.bhaskar@wholefoods.com, CN=uday.bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=Austin, S=TX, C=US"},
                {"TargetHostName", "PERF-ESB-EMS-1.wfm.pvt"},
            };
            // Act
            configUpdater.UpdateExternalAppSettings(configFilePath, appSettingsToUpdate);
            // Assert
            var appSettingsAfterUpdate = ReadAppSettingsFromFile(configFilePath);
            CustomAsserts.AssertAllExpectedEntriesInDictionary(appSettingsToUpdate, appSettingsAfterUpdate);
            File.Delete(configFilePath);
        }

        [TestMethod]
        public void RewriteAppSettings_WhenValidPath_DoesNotOverwriteOtherSettings()
        {
            // Arrange
            var configUpdater = new ExternalConfigXmlManager();
            var testConfigFileToCopy = $"{testDataFolder}\\SampleAppConfig_EsbTst_MammothApiController.exe.config";
            var expectedAppSettingsDictionary = serviceTestData.Services.AppSettings.FakeAppSettings_MammothApiController_ESB_Tst_NonEsbSettingsOnly;
            var configFilePath = testConfigFileToCopy.Replace(".exe.config", "2.exe.config");
            File.Copy(testConfigFileToCopy, configFilePath, true);
            var appSettingsToUpdate = new Dictionary<string, string>
            {
                {"ServerUrl", "ssl://PERF-ESB-EMS-1.wfm.pvt:27293,ssl://PERF-ESB-EMS-2.wfm.pvt:27293"},
                {"JmsPassword", "jms!C0nPERF"},
                {"JndiPassword", "jndiIconUserPERF"},
                {"CertificateName", "E=uday.bhaskar@wholefoods.com, CN=uday.bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=Austin, S=TX, C=US"},
                {"TargetHostName", "PERF-ESB-EMS-1.wfm.pvt"},
            };
            // Act
            configUpdater.UpdateExternalAppSettings(configFilePath, appSettingsToUpdate);
            // Assert
            var appSettingsAfterUpdate = ReadAppSettingsFromFile(configFilePath);
            CustomAsserts.AssertAllExpectedEntriesInDictionary(expectedAppSettingsDictionary, appSettingsAfterUpdate);
            File.Delete(configFilePath);
        }
    }
}
