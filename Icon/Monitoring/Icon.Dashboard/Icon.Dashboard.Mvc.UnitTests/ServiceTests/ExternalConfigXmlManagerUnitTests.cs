using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Services;
using Icon.Dashboard.Mvc.UnitTests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Icon.Dashboard.Mvc.UnitTests.ServiceTests
{
    [TestClass]
    public class ExternalConfigXmlManagerUnitTests
    {
        private RemoteServiceTestData serviceTestData = new RemoteServiceTestData();
        ConfigTestData configTestData = new ConfigTestData();
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
        public void SetAppSettingsDictionary_WhenPassedNull_DoesNotThrow()
        {
            // Arrange
            List<KeyValuePair<string, string>> keyValuePairs = null;
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            externalConfigManager.SetAppSettingsDictionary(keyValuePairs);
            // Assert
            // ... if no exception, test is good
        }

        [TestMethod]
        public void NonEsbAppSettings_WhenAppSettingsDictionaryHasOnlyNonEsbSettings_ReturnsNonEsbSettings()
        {
            // Arrange
            var nonEsbSettingsDictionary = serviceTestData.Services.AppSettings.FakeAppSettings_NoEsb_2;
            var expectedCount = nonEsbSettingsDictionary.Count;
            // Act
            var nonEsbAppSettings = ExternalConfigXmlManager
                .FilterAppSettingsForNonEsbSettings(nonEsbSettingsDictionary);
            // Asssert
            Assert.IsNotNull(nonEsbAppSettings);
            Assert.AreEqual(expectedCount, nonEsbAppSettings.Count);
        }

        [TestMethod]
        public void NonEsbAppSettings_WhenAppSettingsDictionaryHasOnlyEsbSettings_ReturnsEmptyDictionary()
        {
            // Arrange
            var esbSettingsDictionary = serviceTestData.Services.AppSettings.FakeEsbSettings_TEST_Mammoth;
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            var nonEsbAppSettings = ExternalConfigXmlManager
                .FilterAppSettingsForNonEsbSettings(esbSettingsDictionary);
            // Asssert
            Assert.IsNotNull(nonEsbAppSettings);
            Assert.AreEqual(0, nonEsbAppSettings.Count);
        }

        [TestMethod]
        public void NonEsbAppSettings_WhenAppSettingsDictionaryHasMixedEsbSettings_ReturnsNonEsbSettings()
        {
            // Arrange
            var mixedSettingsDictionary = serviceTestData.Services.AppSettings.FakeAppSettings_UndisinguishedWithEsb;
            var expectedCount = 9;
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            var nonEsbAppSettings = ExternalConfigXmlManager
                .FilterAppSettingsForNonEsbSettings(mixedSettingsDictionary);
            // Asssert
            Assert.IsNotNull(nonEsbAppSettings);
            Assert.AreEqual(expectedCount, nonEsbAppSettings.Count);
        }

        [TestMethod]
        public void BuildEsbConnectionModelFromSettingsDictionary_WhenNoAppSettings_ReturnesNull()
        {
            // Arrange
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            var esbConnection = externalConfigManager
                .BuildEsbConnectionModelFromSettingsDictionary(new Dictionary<string, string>(), configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.IsNull(esbConnection);
        }

        [TestMethod]
        public void BuildEsbConnectionModelFromSettingsDictionary_WhenAppSettingsNonEsb_ReturnsNull()
        {
            // Arrange
            var appSettingsDictionary = serviceTestData.Services.AppSettings.FakeAppSettings_NoEsb_2;
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            var esbConnection = externalConfigManager
                .BuildEsbConnectionModelFromSettingsDictionary(appSettingsDictionary, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.IsNull(esbConnection);
        }

        [TestMethod]
        public void BuildEsbConnectionModelFromSettingsDictionary_WhenAppSettingsForEsb_ReturnsEsbConnection()
        {
            // Arrange
            var appSettingsDictionary = serviceTestData.Services.AppSettings.FakeEsbSettings_TEST_Icon;
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            var esbConnection = externalConfigManager
                .BuildEsbConnectionModelFromSettingsDictionary(appSettingsDictionary, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.IsNotNull(esbConnection);
        }

        [TestMethod]
        public void BuildEsbConnectionModelFromSettingsDictionary_WhenAppSettingsForEsb_AddsEsbConnection_WithExpectedProperties()
        {
            // Arrange
            var appSettingsDictionary = serviceTestData.Services.AppSettings.FakeEsbSettings_TEST_Icon;
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            var esbConnection = externalConfigManager
                .BuildEsbConnectionModelFromSettingsDictionary(appSettingsDictionary, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.AreEqual("ESB", esbConnection.ConnectionName);
            Assert.AreEqual(EsbConnectionTypeEnum.Icon, esbConnection.ConnectionType);
            Assert.AreEqual("ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293", esbConnection.ServerUrl);
            Assert.AreEqual("iconUser", esbConnection.JmsUsername);
            Assert.AreEqual("jms!C0n", esbConnection.JmsPassword);
            Assert.AreEqual("jndiIconUser", esbConnection.JndiUsername);
            Assert.AreEqual("jndiIconUser8", esbConnection.JndiPassword);
            Assert.AreEqual("ItemQueueConnectionFactory", esbConnection.ConnectionFactoryName);
            Assert.AreEqual("esb", esbConnection.SslPassword);
            Assert.AreEqual("ClientAcknowledge", esbConnection.SessionMode);
            Assert.AreEqual("E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US", esbConnection.CertificateName);
            Assert.AreEqual("Root", esbConnection.CertificateStoreName);
            Assert.AreEqual("LocalMachine", esbConnection.CertificateStoreLocation);
            Assert.AreEqual("TST-ESB-EMS-1.wfm.pvt", esbConnection.TargetHostName);
            Assert.AreEqual("WFMESB.Technical.ItemMgmt.PointOfSaleResponse.Reply.Queue.V2", esbConnection.QueueName);
        }

        [TestMethod]
        public void BuildEsbConnectionModelFromSettingsDictionary_WhenAppSettingsForEsb_SetsServerUrlProperty()
        {
            // Arrange
            var appSettingsDictionary = serviceTestData.Services.AppSettings.FakeEsbSettings_TEST_Icon;
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            var esbConnection = externalConfigManager.BuildEsbConnectionModelFromSettingsDictionary(appSettingsDictionary, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.IsNotNull(esbConnection.ServerUrl);
        }

        [TestMethod]
        public void BuildEsbConnectionModelFromSettingsDictionary_WhenSettingsNull_DoesNotBuildEsbConnectionModelFromSettingsDictionary()
        {
            // Arrange
            Dictionary<string, string> appSettingsDictionary = null;
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            var esbConnection = externalConfigManager.BuildEsbConnectionModelFromSettingsDictionary(appSettingsDictionary, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.IsNull(esbConnection);
        }

        [TestMethod]
        public void BuildEsbConnectionModelFromSettingsDictionary_WhenSettingsNotForEsb_DoesNotBuildEsbConnectionModelFromSettingsDictionary()
        {
            // Arrange
            var appSettingsDictionary = serviceTestData.Services.AppSettings.FakeAppSettings_NoEsb_1;
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            var esbConnection = externalConfigManager.BuildEsbConnectionModelFromSettingsDictionary(appSettingsDictionary, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.IsNull(esbConnection);
        }

        [TestMethod]
        public void BuildEsbConnectionModelFromSettingsDictionary_WhenSettingsForEsb_AddsEsbConnection()
        {
            // Arrange
            var appSettingsDictionary = serviceTestData.Services.AppSettings.FakeEsbSettings_TEST_Mammoth;
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            var esbConnection = externalConfigManager.BuildEsbConnectionModelFromSettingsDictionary(appSettingsDictionary, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.IsNotNull(esbConnection);
        }

        [TestMethod]
        public void BuildEsbConnectionModelFromSettingsDictionary_WhenSettingsForEsbFromEsbSection_AddsEsbConnection1_WithExpectedProperties()
        {
            // Arrange
            var appSettingsDictionary = serviceTestData.Services.AppSettings.FakeEsbSettings_Perf_Mammoth_FromEsbSectionForEsb_Lowercase;
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            var esbConnection = externalConfigManager.BuildEsbConnectionModelFromSettingsDictionary(appSettingsDictionary, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.AreEqual("ESB", esbConnection.ConnectionName);
            Assert.AreEqual(EsbConnectionTypeEnum.Mammoth, esbConnection.ConnectionType);
            Assert.AreEqual("ssl://PERF-ESB-EMS-1.wfm.pvt:27293,ssl://PERF-ESB-EMS-2.wfm.pvt:27293", esbConnection.ServerUrl);
            Assert.AreEqual("mammothUser", esbConnection.JmsUsername);
            Assert.AreEqual("jmsM@mm#!", esbConnection.JmsPassword);
            Assert.AreEqual("jndiMammothUser", esbConnection.JndiUsername);
            Assert.AreEqual("jndiM@mm$$$", esbConnection.JndiPassword);
            Assert.AreEqual("ItemQueueConnectionFactory", esbConnection.ConnectionFactoryName);
            Assert.AreEqual("esb", esbConnection.SslPassword);
            Assert.AreEqual("ClientAcknowledge", esbConnection.SessionMode);
            Assert.AreEqual("E=uday.bhaskar@wholefoods.com, CN=uday.bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=Austin, S=TX, C=US", esbConnection.CertificateName);
            Assert.AreEqual("Root", esbConnection.CertificateStoreName);
            Assert.AreEqual("LocalMachine", esbConnection.CertificateStoreLocation);
            Assert.AreEqual("PERF-ESB-EMS-1.wfm.pvt", esbConnection.TargetHostName);
            Assert.AreEqual("WFMESB.Enterprise.Item.ItemGateway.Queue.V2", esbConnection.QueueName);
        }

        [TestMethod]
        public void BuildEsbConnectionModelFromSettingsDictionary_WhenSettingsForEsbFromEsbSection_AddsEsbConnection2_WithExpectedProperties()
        {
            // Arrange
            var appSettingsDictionary = serviceTestData.Services.AppSettings.FakeEsbSettings_Perf_Icon_FromEsbSectionForR10_Lowercase;
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            var esbConnection = externalConfigManager.BuildEsbConnectionModelFromSettingsDictionary(appSettingsDictionary, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.AreEqual("R10", esbConnection.ConnectionName);
            Assert.AreEqual(EsbConnectionTypeEnum.Icon, esbConnection.ConnectionType);
            Assert.AreEqual("ssl://PERF-ESB-EMS-1.wfm.pvt:27293,ssl://PERF-ESB-EMS-2.wfm.pvt:27293", esbConnection.ServerUrl);
            Assert.AreEqual("iconUser", esbConnection.JmsUsername);
            Assert.AreEqual("jms!C0n", esbConnection.JmsPassword);
            Assert.AreEqual("jndiIconUser", esbConnection.JndiUsername);
            Assert.AreEqual("jndiIconUser8", esbConnection.JndiPassword);
            Assert.AreEqual("ItemQueueConnectionFactory", esbConnection.ConnectionFactoryName);
            Assert.AreEqual("esb", esbConnection.SslPassword);
            Assert.AreEqual("ClientAcknowledge", esbConnection.SessionMode);
            Assert.AreEqual("E=uday.bhaskar@wholefoods.com, CN=uday.bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=Austin, S=TX, C=US", esbConnection.CertificateName);
            Assert.AreEqual("Root", esbConnection.CertificateStoreName);
            Assert.AreEqual("LocalMachine", esbConnection.CertificateStoreLocation);
            Assert.AreEqual("PERF-ESB-EMS-1.wfm.pvt", esbConnection.TargetHostName);
            Assert.AreEqual("WFMESB.Commerce.Retail.ItemMgmt.Item.Queue.V2", esbConnection.QueueName);
        }

        [TestMethod]
        public void BuildEsbConnectionModelFromSettingsDictionary_WhenExistingAppSettingsForEsb_AddsEsbConnection_WithExpectedProperties()
        {
            // Arrange
            var appSettingsDictionary = serviceTestData.Services.AppSettings.FakeEsbSettings_TEST_Icon;
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            var esbConnection = externalConfigManager
                .BuildEsbConnectionModelFromSettingsDictionary(appSettingsDictionary, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.AreEqual("ESB", esbConnection.ConnectionName);
            Assert.AreEqual(EsbConnectionTypeEnum.Icon, esbConnection.ConnectionType);
            Assert.AreEqual("ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293", esbConnection.ServerUrl);
            Assert.AreEqual("iconUser", esbConnection.JmsUsername);
            Assert.AreEqual("jms!C0n", esbConnection.JmsPassword);
            Assert.AreEqual("jndiIconUser", esbConnection.JndiUsername);
            Assert.AreEqual("jndiIconUser8", esbConnection.JndiPassword);
            Assert.AreEqual("ItemQueueConnectionFactory", esbConnection.ConnectionFactoryName);
            Assert.AreEqual("esb", esbConnection.SslPassword);
            Assert.AreEqual("ClientAcknowledge", esbConnection.SessionMode);
            Assert.AreEqual("E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US", esbConnection.CertificateName);
            Assert.AreEqual("Root", esbConnection.CertificateStoreName);
            Assert.AreEqual("LocalMachine", esbConnection.CertificateStoreLocation);
            Assert.AreEqual("TST-ESB-EMS-1.wfm.pvt", esbConnection.TargetHostName);
            Assert.AreEqual("WFMESB.Technical.ItemMgmt.PointOfSaleResponse.Reply.Queue.V2", esbConnection.QueueName);
        }

        [TestMethod]
        public void BuildEsbConnectionModelsFromCustomConfigDictionaries_WhenNoAppSettings_ReturnsEmptyList()
        {
            // Arrange
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            var esbConnections = externalConfigManager.BuildEsbConnectionModelsFromCustomConfigDictionaries(new List<Dictionary<string, string>>(), configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.IsNotNull(esbConnections);
            Assert.AreEqual(0, esbConnections.Count);
        }

        [TestMethod]
        public void DictionaryContainsEsbServerKey_WhenAppSettingsDictionaryEmpty_ReturnsFalse()
        {
            // Arrange
            var appSettingsDictionary = new Dictionary<string,string>();
            // Act
            var result = ExternalConfigXmlManager.DictionaryContainsEsbServerKey(appSettingsDictionary);
            // Asssert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DictionaryContainsEsbServerKey_WhenAppSettingsDictionaryLacksKey_ReturnsFalse()
        {
            // Arrange
            var nonEsbSettingsDictionary = serviceTestData.Services.AppSettings.FakeAppSettings_NoEsb_2;
            // Act
            var result = ExternalConfigXmlManager.DictionaryContainsEsbServerKey(nonEsbSettingsDictionary);
            // Asssert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DictionaryContainsEsbServerKey_WhenAppSettingsDictionaryHasKey_ReturnsTrue()
        {
            // Arrange
            var settingsDictionaryWithEsbServerKey = serviceTestData.Services.AppSettings.FakeAppSettings_UndisinguishedWithEsb;
            var serviceConfigManager = new ExternalConfigXmlManager();
            // Act
            var result = ExternalConfigXmlManager.DictionaryContainsEsbServerKey(settingsDictionaryWithEsbServerKey);
            // Asssert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void FilterAppSettingsForNonEsbSettings_WhenPassedNullSettings_ReturnsEmptyDictionary()
        {
            // Arrange
            Dictionary<string, string> appSettingsDictionary = null;
            // Act
            var filteredAppSettings = ExternalConfigXmlManager.FilterAppSettingsForNonEsbSettings(appSettingsDictionary);
            // Asssert
            Assert.IsNotNull(filteredAppSettings);
            Assert.AreEqual(0, filteredAppSettings.Count);
        }

        [TestMethod]
        public void FilterAppSettingsForNonEsbSettings_WhenPassedMixedSettings_ReturnsOnlyNonEsbSettings()
        {
            // Arrange
            var appSettingsDictionary = serviceTestData.Services.AppSettings.FakeAppSettings_UndisinguishedWithEsb;
            var expectedCount = 9;
            // Act
            var filteredAppSettings = ExternalConfigXmlManager.FilterAppSettingsForNonEsbSettings(appSettingsDictionary);
            // Asssert
            Assert.IsNotNull(filteredAppSettings);
            Assert.AreEqual(expectedCount, filteredAppSettings.Count);
        }

        [TestMethod]
        public void FilterAppSettingsForNonEsbSettings_WhenPassedOnlyEsbSettings_ReturnsEmptyDictionay()
        {
            // Arrange
            var appSettingsDictionary = serviceTestData.Services.AppSettings.FakeEsbSettings_TEST_Mammoth;
            var expectedCount = 0;
            // Act
            var filteredAppSettings = ExternalConfigXmlManager.FilterAppSettingsForNonEsbSettings(appSettingsDictionary);
            // Asssert
            Assert.IsNotNull(filteredAppSettings);
            Assert.AreEqual(expectedCount, filteredAppSettings.Count);
        }

        [TestMethod]
        public void FilterAppSettingsForNonEsbSettings_WhenPassedOnlyNonEsbSettings_ReturnsOnlyNonEsbSettings()
        {
            // Arrange
            var appSettingsDictionary = serviceTestData.Services.AppSettings.FakeAppSettings_NoEsb_1;
            var expectedCount = 19;
            // Act
            var filteredAppSettings = ExternalConfigXmlManager.FilterAppSettingsForNonEsbSettings(appSettingsDictionary);
            // Asssert
            Assert.IsNotNull(filteredAppSettings);
            Assert.AreEqual(expectedCount, filteredAppSettings.Count);
        }

        [TestMethod]
        public void FilterAppSettingsForOnlyEsbSettings_WhenPassedNullSettings_ReturnsEmptyDictionary()
        {
            // Arrange
            Dictionary<string, string> appSettingsDictionary = null;
            var expectedCount = 0;
            // Act
            var filteredAppSettings = ExternalConfigXmlManager.FilterAppSettingsForOnlyEsbSettings(appSettingsDictionary);
            // Asssert
            Assert.IsNotNull(filteredAppSettings);
            Assert.AreEqual(expectedCount, filteredAppSettings.Count);
        }

        [TestMethod]
        public void FilterAppSettingsForOnlyEsbSettings_WhenPassedMixedSettings_ReturnsOnlyEsbSettings()
        {
            // Arrange
            var appSettingsDictionary = serviceTestData.Services.AppSettings.FakeAppSettings_UndisinguishedWithEsb;
            var expectedCount = 14;
            // Act
            var filteredAppSettings = ExternalConfigXmlManager.FilterAppSettingsForOnlyEsbSettings(appSettingsDictionary);
            // Asssert
            Assert.IsNotNull(filteredAppSettings);
            Assert.AreEqual(expectedCount, filteredAppSettings.Count);
        }

        [TestMethod]
        public void FilterAppSettingsForOnlyEsbSettings_WhenPassedOnlyNonEsbSettings_ReturnsEmptyDictionay()
        {
            // Arrange
            var appSettingsDictionary = serviceTestData.Services.AppSettings.FakeAppSettings_NoEsb_1;
            var expectedCount = 0;
            // Act
            var filteredAppSettings = ExternalConfigXmlManager.FilterAppSettingsForOnlyEsbSettings(appSettingsDictionary);
            // Asssert
            Assert.IsNotNull(filteredAppSettings);
            Assert.AreEqual(expectedCount, filteredAppSettings.Count);
        }

        [TestMethod]
        public void FilterAppSettingsForOnlyEsbSettings_WhenPassedOnlyEsbSettings_ReturnsOnlyEsbSettings()
        {
            // Arrange
            var appSettingsDictionary = serviceTestData.Services.AppSettings.FakeEsbSettings_TEST_Mammoth;
            var expectedCount = 16;
            // Act
            var filteredAppSettings = ExternalConfigXmlManager.FilterAppSettingsForOnlyEsbSettings(appSettingsDictionary);
            // Asssert
            Assert.IsNotNull(filteredAppSettings);
            Assert.AreEqual(expectedCount, filteredAppSettings.Count);
        }

        [TestMethod]
        public void DictionaryContainsEsbServerKey_WhenPassedNullDictionary_ReturnsFalse()
        {
            // Arrange
            Dictionary<string, string> appSettingsDictionary = null;
            var expectedResult = false;
            // Act
            var actualResult = ExternalConfigXmlManager.DictionaryContainsEsbServerKey(appSettingsDictionary);
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void DictionaryContainsEsbServerKey_WhenDictionaryDoesNotContainServerKey_ReturnsFalse()
        {
            // Arrange
            var appSettingsDictionary = serviceTestData.Services.AppSettings.FakeAppSettings_NoEsb_1;
            var expectedResult = false;
            // Act
            var actualResult = ExternalConfigXmlManager.DictionaryContainsEsbServerKey(appSettingsDictionary);
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void DictionaryContainsEsbServerKey_WhenDictionaryContainsTitleCaseServerKey_ReturnsTrue()
        {
            // Arrange
            var appSettingsDictionary = serviceTestData.Services.AppSettings.FakeEsbSettings_TEST_Mammoth;
            var expectedResult = true;
            // Act
            var actualResult = ExternalConfigXmlManager.DictionaryContainsEsbServerKey(appSettingsDictionary);
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void DictionaryContainsEsbServerKey_WhenDictionaryContainsLowerCamelCaseServerKey_ReturnsTrue()
        {
            // Arrange
            var appSettingsDictionary = new Dictionary<string, string>
            {
                { "serverUrl", "ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293" },
            };
            var expectedResult = true;
            // Act
            var actualResult = ExternalConfigXmlManager.DictionaryContainsEsbServerKey(appSettingsDictionary);
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void DictionaryContainsEsbServerKey_WhenDictionaryContainsUpperCaseServerKey_ReturnsTrue()
        {
            // Arrange
            var appSettingsDictionary = new Dictionary<string, string>
            {
                { "SERVERURL", "ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293" },
            };
            var expectedResult = true;
            // Act
            var actualResult = ExternalConfigXmlManager.DictionaryContainsEsbServerKey(appSettingsDictionary);
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void DictionaryContainsEsbServerKey_WhenDictionaryContainsLowerCaseServerKey_ReturnsTrue()
        {
            // Arrange
            var appSettingsDictionary = new Dictionary<string, string>
            {
                { "serverurl", "ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293" },
            };
            var expectedResult = true;
            // Act
            var actualResult = ExternalConfigXmlManager.DictionaryContainsEsbServerKey(appSettingsDictionary);
            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void RewriteAppSettings_WhenInValidPath_DoesNothing()
        {
            // Arrange
            var configUpdater = new ExternalConfigXmlManager();
            var configFilePath = $"{testDataFolder}\\BAD.exe.config";
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
            // ... just checking that no exception was thrown - if bad path, just forget about it
        }

        [TestMethod]
        public void BuildEsbConnectionModelFromSettingsDictionary_WhenPassedEmptySettingsValues_SetsAppSettingsDictionaryValues()
        {
            // Arrange
            var appSettings = new Dictionary<string, string>();
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            var esbConnection = externalConfigManager.BuildEsbConnectionModelFromSettingsDictionary(appSettings, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.IsNull(esbConnection);
        }

        [TestMethod]
        public void BuildEsbConnectionModelFromSettingsDictionary_WhenPassedSettingsValues_NotIncludingEsb_SetsAppSettingsDictionaryValues()
        {
            // Arrange
            var appSettings = serviceTestData.Services.AppSettings.FakeEsbSettings_TEST_Mammoth;
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            var esbConnection = externalConfigManager.BuildEsbConnectionModelFromSettingsDictionary(appSettings, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.AreEqual("ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293", esbConnection.ServerUrl);
            Assert.AreEqual("mammothUser", esbConnection.JmsUsername);
            Assert.AreEqual("jmsM@mm#!", esbConnection.JmsPassword);
            Assert.AreEqual("jndiMammothUser", esbConnection.JndiUsername);
            Assert.AreEqual("jndiM@mm$$$", esbConnection.JndiPassword);
            Assert.AreEqual("ItemQueueConnectionFactory", esbConnection.ConnectionFactoryName);
            Assert.AreEqual("esb", esbConnection.SslPassword);
            Assert.AreEqual("AutoAcknowledge", esbConnection.SessionMode);
            Assert.AreEqual("E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US",
                esbConnection.CertificateName);
            Assert.AreEqual("Root", esbConnection.CertificateStoreName);
            Assert.AreEqual("LocalMachine", esbConnection.CertificateStoreLocation);
            Assert.AreEqual("TST-ESB-EMS-1.wfm.pvt", esbConnection.TargetHostName);
            Assert.AreEqual("WFMESB.Commerce.Retail.LocaleMgmt.Locale.Queue.V2", esbConnection.LocaleQueueName);
            Assert.AreEqual("WFMESB.Commerce.Retail.HierarchyMgmt.Hierarchy.Queue.V2", esbConnection.HierarchyQueueName);
            Assert.AreEqual("WFMESB.Commerce.Retail.ItemMgmt.Item.Queue.V2", esbConnection.ItemQueueName);
            Assert.AreEqual("WFMESB.Commerce.Retail.ItemMgmt.SelectionGroup.Queue.V2", esbConnection.ProductSelectionGroupQueueName);
        }

        [TestMethod]
        public void BuildEsbConnectionModelFromSettingsDictionary_WhenPassedSettingsValuesNotIncludingEsb_DoesNotSetEsbConnection()
        {
            // Arrange
            var appSettings = serviceTestData.Services.AppSettings.FakeAppSettings_NoEsb_2;
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            var esbConnection = externalConfigManager.BuildEsbConnectionModelFromSettingsDictionary(appSettings, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.IsNull(esbConnection);
        }

        [TestMethod]
        public void BuildEsbConnectionModelFromSettingsDictionary_WhenPassedSettingsValuesIncludingEsb_SetsEsbConnection()
        {
            // Arrange
            var appSettings = serviceTestData.Services.AppSettings.FakeAppSettings_UndisinguishedWithEsb;
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            var esbConnection = externalConfigManager.BuildEsbConnectionModelFromSettingsDictionary(appSettings, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.IsNotNull(esbConnection);
        }

        [TestMethod]
        public void BuildEsbConnectionModelFromSettingsDictionary_WhenPassedSettingsValuesIncludingEsb_SetsEsbConnection_WithExpectedValues()
        {
            // Arrange
            var appSettings = serviceTestData.Services.AppSettings.FakeAppSettings_UndisinguishedWithEsb;
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            var esbConnection = externalConfigManager.BuildEsbConnectionModelFromSettingsDictionary(appSettings, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.AreEqual("ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293",
                esbConnection.ServerUrl);
            Assert.AreEqual("iconUser", esbConnection.JmsUsername);
            Assert.AreEqual("jms!C0n", esbConnection.JmsPassword);
            Assert.AreEqual("jndiIconUser", esbConnection.JndiUsername);
            Assert.AreEqual("jndiIconUser8", esbConnection.JndiPassword);
            Assert.AreEqual("ItemQueueConnectionFactory",
                esbConnection.ConnectionFactoryName);
            Assert.AreEqual("esb", esbConnection.SslPassword);
            Assert.AreEqual("ClientAcknowledge", esbConnection.SessionMode);
            Assert.AreEqual("E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US",
                esbConnection.CertificateName);
            Assert.AreEqual("Root", esbConnection.CertificateStoreName);
            Assert.AreEqual("LocalMachine", esbConnection.CertificateStoreLocation);
            Assert.AreEqual("TST-ESB-EMS-1.wfm.pvt", esbConnection.TargetHostName);
            Assert.AreEqual("WFMESB.Technical.ItemMgmt.PointOfSaleResponse.Reply.Queue.V2",
                esbConnection.QueueName);
        }

        [TestMethod]
        public void BuildEsbConnectionModelsFromCustomConfigDictionaries_WhenPassedNull_DoesNotSetEsbConnection()
        {
            // Arrange
            List<Dictionary<string, string>> esbConfigData = null;
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            var esbConnections = externalConfigManager.BuildEsbConnectionModelsFromCustomConfigDictionaries(esbConfigData, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.IsNotNull(esbConnections);
            Assert.AreEqual(0, esbConnections.Count);
        }

        [TestMethod]
        public void BuildEsbConnectionModelsFromCustomConfigDictionaries_WhenPassedSettingsIrrelevantToEsb_DoesNotAddEsbConnection()
        {
            // Arrange
            var esbConfigData = new List<Dictionary<string, string>>()
            {
                serviceTestData.Services.AppSettings.FakeAppSettings_NoEsb_1
            };
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            var esbConnections = externalConfigManager.BuildEsbConnectionModelsFromCustomConfigDictionaries(esbConfigData, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.IsNotNull(esbConnections);
            Assert.AreEqual(0, esbConnections.Count);
        }

        [TestMethod]
        public void BuildEsbConnectionModelsFromCustomConfigDictionaries_WhenPassedSettingsForSingleConnection_AddsEsbConnection()
        {
            // Arrange
            var esbConfigData = new List<Dictionary<string, string>>()
            {
                serviceTestData.Services.AppSettings.FakeEsbSettings_TEST_Mammoth
            };
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            var esbConnections = externalConfigManager.BuildEsbConnectionModelsFromCustomConfigDictionaries(esbConfigData, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.IsNotNull(esbConnections);
            Assert.AreEqual(1, esbConnections.Count);
        }

        [TestMethod]
        public void BuildEsbConnectionModelsFromCustomConfigDictionaries_WhenPassedSettingsValues_IncludingEsb_SetsAppSettingsDictionaryValues_WithExpectedValues()
        {
            // Arrange
            var appSettings = new List<Dictionary<string, string>> {
                serviceTestData.Services.AppSettings.FakeAppSettings_UndisinguishedWithEsb
            };
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            var esbConnections = externalConfigManager.BuildEsbConnectionModelsFromCustomConfigDictionaries(appSettings, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.AreEqual("ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293", esbConnections[0].ServerUrl);
            Assert.AreEqual("iconUser", esbConnections[0].JmsUsername);
            Assert.AreEqual("jms!C0n", esbConnections[0].JmsPassword);
            Assert.AreEqual("jndiIconUser", esbConnections[0].JndiUsername);
            Assert.AreEqual("jndiIconUser8", esbConnections[0].JndiPassword);
            Assert.AreEqual("ItemQueueConnectionFactory", esbConnections[0].ConnectionFactoryName);
            Assert.AreEqual("esb", esbConnections[0].SslPassword);
            Assert.AreEqual("ClientAcknowledge", esbConnections[0].SessionMode);
            Assert.AreEqual("E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US", esbConnections[0].CertificateName);
            Assert.AreEqual("Root", esbConnections[0].CertificateStoreName);
            Assert.AreEqual("LocalMachine", esbConnections[0].CertificateStoreLocation);
            Assert.AreEqual("TST-ESB-EMS-1.wfm.pvt", esbConnections[0].TargetHostName);
            Assert.AreEqual("WFMESB.Technical.ItemMgmt.PointOfSaleResponse.Reply.Queue.V2", esbConnections[0].QueueName);
            Assert.AreEqual("30000", esbConnections[0].ReconnectDelay);
        }

        [TestMethod]
        public void BuildEsbConnectionModelsFromCustomConfigDictionaries_WhenPassedSettingsForSingleConnection_AddsEsbConnection_WithExpectedValues()
        {
            // Arrange
            var esbConfigData = new List<Dictionary<string, string>>()
            {
                serviceTestData.Services.AppSettings.FakeEsbSettings_TEST_Mammoth
            };
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            var esbConnections = externalConfigManager.BuildEsbConnectionModelsFromCustomConfigDictionaries(esbConfigData, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.AreEqual("ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293",
              esbConnections[0].ServerUrl);
            Assert.AreEqual("mammothUser", esbConnections[0].JmsUsername);
            Assert.AreEqual("jmsM@mm#!", esbConnections[0].JmsPassword);
            Assert.AreEqual("jndiMammothUser", esbConnections[0].JndiUsername);
            Assert.AreEqual("jndiM@mm$$$", esbConnections[0].JndiPassword);
            Assert.AreEqual("ItemQueueConnectionFactory",
                esbConnections[0].ConnectionFactoryName);
            Assert.AreEqual("esb", esbConnections[0].SslPassword);
            Assert.AreEqual("AutoAcknowledge", esbConnections[0].SessionMode);
            Assert.AreEqual("E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US",
                esbConnections[0].CertificateName);
            Assert.AreEqual("Root", esbConnections[0].CertificateStoreName);
            Assert.AreEqual("LocalMachine", esbConnections[0].CertificateStoreLocation);
            Assert.AreEqual("TST-ESB-EMS-1.wfm.pvt", esbConnections[0].TargetHostName);
            Assert.AreEqual("WFMESB.Commerce.Retail.LocaleMgmt.Locale.Queue.V2",
                esbConnections[0].LocaleQueueName);
            Assert.AreEqual("WFMESB.Commerce.Retail.HierarchyMgmt.Hierarchy.Queue.V2",
                esbConnections[0].HierarchyQueueName);
            Assert.AreEqual("WFMESB.Commerce.Retail.ItemMgmt.Item.Queue.V2",
                esbConnections[0].ItemQueueName);
            Assert.AreEqual("WFMESB.Commerce.Retail.ItemMgmt.SelectionGroup.Queue.V2",
                esbConnections[0].ProductSelectionGroupQueueName);
        }

        [TestMethod]
        public void BuildEsbConnectionModelsFromCustomConfigDictionaries_WhenPassedSettingsForMultipleConnections_AddsEsbConnections()
        {
            // Arrange
            var esbConfigData = new List<Dictionary<string, string>>()
            {
                serviceTestData.Services.AppSettings.FakeEsbSettings_TEST_Ewic_Listener,
                serviceTestData.Services.AppSettings.FakeEsbSettings_TEST_Ewic_Producer
            };
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            var esbConnections = externalConfigManager.BuildEsbConnectionModelsFromCustomConfigDictionaries(esbConfigData, configTestData.Models.EsbEnvironmentsList);
            // Assert
            Assert.IsNotNull(esbConnections);
            Assert.AreEqual(2, esbConnections.Count);
        }

        [TestMethod]
        public void BuildEsbConnectionModelsFromCustomConfigDictionaries_WhenPassedSettingsForMultipleConnections_AddsEsbConnections_WithExpectedValues()
        {
            // Arrange
            var esbConfigData = new List<Dictionary<string, string>>()
            {
                serviceTestData.Services.AppSettings.FakeEsbSettings_TEST_Ewic_Listener,
                serviceTestData.Services.AppSettings.FakeEsbSettings_TEST_Ewic_Producer
            };
            var externalConfigManager = new ExternalConfigXmlManager();
            // Act
            var esbConnections = externalConfigManager.BuildEsbConnectionModelsFromCustomConfigDictionaries(esbConfigData, configTestData.Models.EsbEnvironmentsList);
            // Assert
            var listenerConnection = esbConnections[0];
            Assert.AreEqual("listener", listenerConnection.ConnectionName);
            Assert.AreEqual("ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293",
                listenerConnection.ServerUrl);
            Assert.AreEqual("ewicIconUser", listenerConnection.JmsUsername);
            Assert.AreEqual("ewic\"#*8", listenerConnection.JmsPassword);
            Assert.AreEqual("jndiEwicIconUser", listenerConnection.JndiUsername);
            Assert.AreEqual("jndiEw!cUser", listenerConnection.JndiPassword);
            Assert.AreEqual("EwicQueueConnectionFactory", listenerConnection.ConnectionFactoryName);
            Assert.AreEqual("esb", listenerConnection.SslPassword);
            Assert.AreEqual("ClientAcknowledge", listenerConnection.SessionMode);
            Assert.AreEqual("E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US",
                listenerConnection.CertificateName);
            Assert.AreEqual("Root", listenerConnection.CertificateStoreName);
            Assert.AreEqual("LocalMachine", listenerConnection.CertificateStoreLocation);
            Assert.AreEqual("TST-ESB-EMS-1.wfm.pvt", listenerConnection.TargetHostName);
            Assert.AreEqual("WFMESB.Commerce.Retail.EWICMgmt.EWIC.Queue.V1",
                listenerConnection.QueueName);
            Assert.AreEqual("30000", listenerConnection.ReconnectDelay);

            var producerConnection = esbConnections[1];
            Assert.AreEqual("producer", producerConnection.ConnectionName);
            Assert.AreEqual("ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293",
                producerConnection.ServerUrl);
            Assert.AreEqual("ewicIconUser", producerConnection.JmsUsername);
            Assert.AreEqual("ewic\"#*8", producerConnection.JmsPassword);
            Assert.AreEqual("jndiEwicIconUser", producerConnection.JndiUsername);
            Assert.AreEqual("jndiEw!cUser", producerConnection.JndiPassword);
            Assert.AreEqual("EwicQueueConnectionFactory", producerConnection.ConnectionFactoryName);
            Assert.AreEqual("esb", producerConnection.SslPassword);
            Assert.AreEqual("ClientAcknowledge", producerConnection.SessionMode);
            Assert.AreEqual("E=uday.bhaskar@wholefoods.com, CN=Uday Bhaskar, OU=InfraESBAdmins@wholefoods.com, O=Whole Foods Market, L=\"Austin \", S=TX, C=US",
                producerConnection.CertificateName);
            Assert.AreEqual("Root", producerConnection.CertificateStoreName);
            Assert.AreEqual("LocalMachine", producerConnection.CertificateStoreLocation);
            Assert.AreEqual("TST-ESB-EMS-1.wfm.pvt", producerConnection.TargetHostName);
            Assert.AreEqual("WFMESB.Commerce.PointOfSaleMgmt.NonSequencedRequest.Queue.V2",
                producerConnection.QueueName);
            Assert.AreEqual("30000", producerConnection.ReconnectDelay);
        }

        [TestMethod]
        public void DetermineDatabaseEnvironment_WhenIconDev_ReturnDevEnvironment()
        {
            // Arrange
            var expectedEnvironment = EnvironmentEnum.Dev0;
            var configReader = new ExternalConfigXmlManager();
            // Act 
            var actualEnvironment = configReader.DetermineDatabaseEnvironment(
                DatabaseCategoryEnum.Icon, @"cewd1815\SqlSHARED2012D", "IconDev", configTestData.Models.EnvironmentsList);
            // Assert
            Assert.AreEqual(expectedEnvironment, actualEnvironment);
        }

        [TestMethod]
        public void DetermineDatabaseEnvironment_WhenIconDev_ReturnDevEnvironment_IgnoresCase()
        {
            // Arrange
            var expectedEnvironment = EnvironmentEnum.Dev0;
            var configReader = new ExternalConfigXmlManager();
            // Act 
            var actualEnvironment = configReader.DetermineDatabaseEnvironment(
                DatabaseCategoryEnum.Icon, @"cewd1815\sqlshared2012d", "icondev", configTestData.Models.EnvironmentsList);
            // Assert
            Assert.AreEqual(expectedEnvironment, actualEnvironment);
        }

        [TestMethod]
        public void DetermineDatabaseEnvironment_WhenIconPerf_ReturnPerfEnvironment()
        {
            // Arrange
            var expectedEnvironment = EnvironmentEnum.Perf;
            var configReader = new ExternalConfigXmlManager();
            // Act 
            var actualEnvironment = configReader.DetermineDatabaseEnvironment(
                DatabaseCategoryEnum.Icon, @"CEWD1815\SQLSHARED2012D", "iCONLoadTest", configTestData.Models.EnvironmentsList);
            // Assert
            Assert.AreEqual(expectedEnvironment, actualEnvironment);
        }

        [TestMethod]
        public void DetermineDatabaseEnvironment_WhenMammothDev_ReturnsDevEnvironment()
        {
            // Arrange
            var expectedEnvironment = EnvironmentEnum.Dev0;
            var configReader = new ExternalConfigXmlManager();
            // Act 
            var actualEnvironment = configReader.DetermineDatabaseEnvironment(
                DatabaseCategoryEnum.Mammoth, @"mammoth-db01-dev\mammoth", "Mammoth_Dev", configTestData.Models.EnvironmentsList);
            // Assert
            Assert.AreEqual(expectedEnvironment, actualEnvironment);
        }

        [TestMethod]
        public void DetermineDatabaseEnvironment_WhenMammothTest_ReturnsTestEnvironment()
        {
            // Arrange
            var expectedEnvironment = EnvironmentEnum.Tst0;
            var configReader = new ExternalConfigXmlManager();
            // Act 
            var actualEnvironment = configReader.DetermineDatabaseEnvironment(
                DatabaseCategoryEnum.Mammoth, @"MAMMOTH-DB01-DEV\MAMMOTH", "Mammoth", configTestData.Models.EnvironmentsList);
            // Assert
            Assert.AreEqual(expectedEnvironment, actualEnvironment);
        }

        [TestMethod]
        public void DetermineDatabaseEnvironment_WhenMammothTst1_ReturnsTst1Environment()
        {
            // Arrange
            var expectedEnvironment = EnvironmentEnum.Tst1;
            var configReader = new ExternalConfigXmlManager();
            // Act 
            var actualEnvironment = configReader.DetermineDatabaseEnvironment(
                DatabaseCategoryEnum.Mammoth, @"mammoth-db01-tst01", "Mammoth", configTestData.Models.EnvironmentsList);
            // Assert
            Assert.AreEqual(expectedEnvironment, actualEnvironment);
        }

        [TestMethod]
        public void DetermineDatabaseEnvironment_WhenIrmaDev_ReturnsDevEnvironment()
        {
            // Arrange
            var expectedEnvironment = EnvironmentEnum.Dev0;
            var configReader = new ExternalConfigXmlManager();
            // Act 
            var actualEnvironment = configReader.DetermineDatabaseEnvironment(
                DatabaseCategoryEnum.IRMA, @"idd-ma\mad", "ItemCatalog_Test", configTestData.Models.EnvironmentsList);
            // Assert
            Assert.AreEqual(expectedEnvironment, actualEnvironment);
        }

        [TestMethod]
        public void DetermineDatabaseEnvironment_WhenIrmaTest_ReturnsTestEnvironment()
        {
            // Arrange
            var expectedEnvironment = EnvironmentEnum.Tst0;
            var configReader = new ExternalConfigXmlManager();
            // Act 
            var actualEnvironment = configReader.DetermineDatabaseEnvironment(
                DatabaseCategoryEnum.IRMA, @"idt-pn\pnt", "ItemCatalog_Test", configTestData.Models.EnvironmentsList);
            // Assert
            Assert.AreEqual(expectedEnvironment, actualEnvironment);
        }

        [TestMethod]
        public void DetermineDatabaseEnvironment_WhenIrmaTst1_ReturnsTst1Environment()
        {
            // Arrange
            var expectedEnvironment = EnvironmentEnum.Tst1;
            var configReader = new ExternalConfigXmlManager();
            // Act 
            var actualEnvironment = configReader.DetermineDatabaseEnvironment(
                DatabaseCategoryEnum.IRMA, @"ma-db01-tst01", "ItemCatalog", configTestData.Models.EnvironmentsList);
            // Assert
            Assert.AreEqual(expectedEnvironment, actualEnvironment);
        }

        [TestMethod]
        public void DetermineDatabaseEnvironment_WhenIrmaQA_ReturnsQAEnvironment()
        {
            // Arrange
            var expectedEnvironment = EnvironmentEnum.QA;
            var configReader = new ExternalConfigXmlManager();
            // Act 
            var actualEnvironment = configReader.DetermineDatabaseEnvironment(
                DatabaseCategoryEnum.IRMA, @"idq-fl\flq", "ItemCatalog", configTestData.Models.EnvironmentsList);
            // Assert
            Assert.AreEqual(expectedEnvironment, actualEnvironment);
        }

        [TestMethod]
        public void DetermineDatabaseEnvironment_WhenVim_ReturnsUndefinedEnvironment()
        {
            // Arrange
            var expectedEnvironment = EnvironmentEnum.Undefined;
            var configReader = new ExternalConfigXmlManager();
            // Act 
            var actualEnvironment = configReader.DetermineDatabaseEnvironment(
                DatabaseCategoryEnum.Vim, @"((anyWeirdVimdb_string)(port5930))", "vim_vim_vim", configTestData.Models.EnvironmentsList);
            // Assert
            Assert.AreEqual(expectedEnvironment, actualEnvironment);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIconSqlWithIconLogging_ReadsDbConnection()
        {
            // Arrange
            var configPath = $"{testDataFolder}\\SampleDbConfig_IconSqlWithIconLogging.config";
            var configXmlDoc = XDocument.Load(configPath);
            var configReader = new ExternalConfigXmlManager();
            // Act
            var dbConfig = configReader.ReadDatabaseConfiguration(configXmlDoc, configTestData.Models.EnvironmentsList);
            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon));
            var iconConnection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon);
            Assert.IsNotNull(iconConnection);
            Assert.AreEqual("Icon", iconConnection.DatabaseName);
            Assert.AreEqual(@"cewd1815\SQLSHARED2012D", iconConnection.ServerName);
            Assert.AreEqual("Icon", iconConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Tst0, iconConnection.Environment);
            Assert.IsFalse(iconConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIconSqlWithIconLogging_ReadsLoggingConnection()
        {
            // Arrange
            var configPath = $"{testDataFolder}\\SampleDbConfig_IconSqlWithIconLogging.config";
            var configXmlDoc = XDocument.Load(configPath);
            var configReader = new ExternalConfigXmlManager(configPath);
            // Act
            var dbConfig = configReader.ReadDatabaseConfiguration(configXmlDoc, configTestData.Models.EnvironmentsList);
            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsNotNull(dbConfig.LoggingConnection);
            Assert.AreSame(dbConfig.LoggingConnection, dbConfig.Connections.Single(c => c.IsUsedForLogging));
            Assert.AreEqual("Icon", dbConfig.LoggingConnection.DatabaseName);
            Assert.AreEqual(@"cewd1815\SQLSHARED2012D", dbConfig.LoggingConnection.ServerName);
            Assert.AreEqual("dbLogVimLocaleController", dbConfig.LoggingConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Tst0, dbConfig.LoggingConnection.Environment);
            Assert.IsFalse(dbConfig.LoggingConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIconEfWithIconLogging_ReadsDbConnection()
        {
            // Arrange
            var configPath = $"{testDataFolder}\\SampleDbConfig_IconEfWithIconLogging.config";
            var configXmlDoc = XDocument.Load(configPath);
            var configReader = new ExternalConfigXmlManager(configPath);
            // Act
            var dbConfig = configReader.ReadDatabaseConfiguration(configXmlDoc, configTestData.Models.EnvironmentsList);
            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon));
            var iconConnection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon);
            Assert.IsNotNull(iconConnection);
            Assert.AreEqual("Icon", iconConnection.DatabaseName);
            Assert.AreEqual(@"cewd1815\SQLSHARED2012D", iconConnection.ServerName);
            Assert.AreEqual("IconContext", iconConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Tst0, iconConnection.Environment);
            Assert.IsTrue(iconConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIconEfWithIconLogging_ReadsLoggingConnection()
        {
            // Arrange
            var configPath = $"{testDataFolder}\\SampleDbConfig_IconEfWithIconLogging.config";
            var configXmlDoc = XDocument.Load(configPath);
            var configReader = new ExternalConfigXmlManager(configPath);
            // Act
            var dbConfig = configReader.ReadDatabaseConfiguration(configXmlDoc, configTestData.Models.EnvironmentsList);
            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsNotNull(dbConfig.LoggingConnection);
            Assert.AreSame(dbConfig.LoggingConnection, dbConfig.Connections.Single(c => c.IsUsedForLogging));
            Assert.AreEqual("Icon", dbConfig.LoggingConnection.DatabaseName);
            Assert.AreEqual(@"cewd1815\SQLSHARED2012D", dbConfig.LoggingConnection.ServerName);
            Assert.AreEqual("dbLogIconEsb", dbConfig.LoggingConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Tst0, dbConfig.LoggingConnection.Environment);
            Assert.IsFalse(dbConfig.LoggingConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIconSqlAndIconEfAndMammothSqlWithIconLogging_ReadsDbConnections()
        {
            // Arrange
            var configPath = $"{testDataFolder}\\SampleDbConfig_IconSqlAndIconEfAndMammothSqlWithIconLogging.config";
            var configXmlDoc = XDocument.Load(configPath);
            var configReader = new ExternalConfigXmlManager(configPath);
            // Act
            var dbConfig = configReader.ReadDatabaseConfiguration(configXmlDoc, configTestData.Models.EnvironmentsList);
            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon));

            var iconConnection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon);
            Assert.IsNotNull(iconConnection);
            Assert.AreEqual("Icon", iconConnection.DatabaseName);
            Assert.AreEqual(@"cewd1815\SQLSHARED2012D", iconConnection.ServerName);
            Assert.AreEqual("IconContext", iconConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Tst0, iconConnection.Environment);
            Assert.IsTrue(iconConnection.IsEntityFramework);

            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Mammoth));
            var mammothConnection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Mammoth);
            Assert.IsNotNull(mammothConnection);
            Assert.AreEqual("Mammoth", mammothConnection.DatabaseName);
            Assert.AreEqual(@"MAMMOTH-DB01-DEV\MAMMOTH", mammothConnection.ServerName);
            Assert.AreEqual("Mammoth", mammothConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Tst0, mammothConnection.Environment);
            Assert.IsFalse(mammothConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIconSqlAndIconEfAndMammothSqlWithIconLogging_ReadsLoggingConnections()
        {
            // Arrange
            var configPath = $"{testDataFolder}\\SampleDbConfig_IconSqlAndIconEfAndMammothSqlWithIconLogging.config";
            var configXmlDoc = XDocument.Load(configPath);
            var configReader = new ExternalConfigXmlManager(configPath);
            // Act
            var dbConfig = configReader.ReadDatabaseConfiguration(configXmlDoc, configTestData.Models.EnvironmentsList);
            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsNotNull(dbConfig.LoggingConnection);
            Assert.AreSame(dbConfig.LoggingConnection, dbConfig.Connections.Single(c => c.IsUsedForLogging));
            Assert.AreEqual("Icon", dbConfig.LoggingConnection.DatabaseName);
            Assert.AreEqual(@"cewd1815\SQLSHARED2012D", dbConfig.LoggingConnection.ServerName);
            Assert.AreEqual("dbLogIconEsb", dbConfig.LoggingConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Tst0, dbConfig.LoggingConnection.Environment);
            Assert.IsFalse(dbConfig.LoggingConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasMammothSqlWithMammothLogging_ReadsDbConnection()
        {
            // Arrange
            var configPath = $"{testDataFolder}\\SampleDbConfig_MammothSqlWithMammothLogging.config";
            var configXmlDoc = XDocument.Load(configPath);
            var configReader = new ExternalConfigXmlManager(configPath);
            // Act
            var dbConfig = configReader.ReadDatabaseConfiguration(configXmlDoc, configTestData.Models.EnvironmentsList);
            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Mammoth));
            var mammothConnection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Mammoth);
            Assert.IsNotNull(mammothConnection);
            Assert.AreEqual("Mammoth", mammothConnection.DatabaseName);
            Assert.AreEqual(@"MAMMOTH-DB01-DEV\MAMMOTH", mammothConnection.ServerName);
            Assert.AreEqual("MammothContext", mammothConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Tst0, mammothConnection.Environment);
            Assert.IsFalse(mammothConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasMammothSqlWithMammothLogging_ReadsLoggingConnection()
        {
            // Arrange
            var configPath = $"{testDataFolder}\\SampleDbConfig_MammothSqlWithMammothLogging.config";
            var configXmlDoc = XDocument.Load(configPath);
            var configReader = new ExternalConfigXmlManager(configPath);
            // Act
            var dbConfig = configReader.ReadDatabaseConfiguration(configXmlDoc, configTestData.Models.EnvironmentsList);
            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsNotNull(dbConfig.LoggingConnection);
            Assert.AreSame(dbConfig.LoggingConnection, dbConfig.Connections.Single(c => c.IsUsedForLogging));
            Assert.AreEqual("Mammoth", dbConfig.LoggingConnection.DatabaseName);
            Assert.AreEqual(@"MAMMOTH-DB01-DEV\MAMMOTH", dbConfig.LoggingConnection.ServerName);
            Assert.AreEqual("dbLogMammoth", dbConfig.LoggingConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Tst0, dbConfig.LoggingConnection.Environment);
            Assert.IsFalse(dbConfig.LoggingConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIrmaSqlWithMammothLogging_ReadsDbConnections()
        {
            // Arrange
            var configPath = $"{testDataFolder}\\SampleDbConfig_IrmaSqlWithMammothLogging.config";
            var configXmlDoc = XDocument.Load(configPath);
            var configReader = new ExternalConfigXmlManager(configPath);
            // Act
            var dbConfig = configReader.ReadDatabaseConfiguration(configXmlDoc, configTestData.Models.EnvironmentsList);
            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.IRMA));
            var irmaConnections = dbConfig.GetAllNonLoggingConnectionsOfCategory(DatabaseCategoryEnum.IRMA);
            Assert.AreEqual(12, irmaConnections.Count);

            var irmaFlConnection = dbConfig.Connections
                .Single(c => c.Category == DatabaseCategoryEnum.IRMA && c.ServerName.Contains("fl"));
            Assert.IsNotNull(irmaFlConnection);
            Assert.AreEqual("ItemCatalog_Test", irmaFlConnection.DatabaseName);
            Assert.AreEqual(@"idd-fl\fld", irmaFlConnection.ServerName);
            Assert.AreEqual("ItemCatalog_FL", irmaFlConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Dev0, irmaFlConnection.Environment);
            Assert.IsFalse(irmaFlConnection.IsEntityFramework);

            var irmaPnConnection = dbConfig.Connections
                .Single(c => c.Category == DatabaseCategoryEnum.IRMA && c.ServerName.Contains("pn"));
            Assert.IsNotNull(irmaPnConnection);
            Assert.AreEqual("ItemCatalog_Test", irmaPnConnection.DatabaseName);
            Assert.AreEqual(@"idt-pn\pnt", irmaPnConnection.ServerName);
            Assert.AreEqual("ItemCatalog_PN", irmaPnConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Tst0, irmaPnConnection.Environment);
            Assert.IsFalse(irmaPnConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIrmaSqlWithMammothLogging_ReadsLoggingConnection()
        {
            // Arrange
            var configPath = $"{testDataFolder}\\SampleDbConfig_IrmaSqlWithMammothLogging.config";
            var configXmlDoc = XDocument.Load(configPath);
            var configReader = new ExternalConfigXmlManager(configPath);
            // Act
            var dbConfig = configReader.ReadDatabaseConfiguration(configXmlDoc, configTestData.Models.EnvironmentsList);
            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsNotNull(dbConfig.LoggingConnection);
            Assert.AreSame(dbConfig.LoggingConnection, dbConfig.Connections.Single(c => c.IsUsedForLogging));
            Assert.AreEqual("Mammoth", dbConfig.LoggingConnection.DatabaseName);
            Assert.AreEqual(@"MAMMOTH-DB01-DEV\MAMMOTH", dbConfig.LoggingConnection.ServerName);
            Assert.AreEqual("dbLogMammoth", dbConfig.LoggingConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Tst0, dbConfig.LoggingConnection.Environment);
            Assert.IsFalse(dbConfig.LoggingConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIconEfAndIrmaSqlWithIconLogging_ReadsDbConnections()
        {
            // Arrange
            var configPath = $"{testDataFolder}\\SampleDbConfig_IconEfAndIrmaSqlWithIconLogging.config";
            var configXmlDoc = XDocument.Load(configPath);
            var configReader = new ExternalConfigXmlManager(configPath);
            // Act
            var dbConfig = configReader.ReadDatabaseConfiguration(configXmlDoc, configTestData.Models.EnvironmentsList);
            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon));
            var iconConnection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon);
            Assert.IsNotNull(iconConnection);
            Assert.AreEqual("Icon", iconConnection.DatabaseName);
            Assert.AreEqual(@"cewd1815\SQLSHARED2012D", iconConnection.ServerName);
            Assert.AreEqual("IconContext", iconConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Tst0, iconConnection.Environment);
            Assert.IsTrue(iconConnection.IsEntityFramework);

            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.IRMA));
            var irmaConnections = dbConfig.GetAllNonLoggingConnectionsOfCategory(DatabaseCategoryEnum.IRMA);
            Assert.AreEqual(12, irmaConnections.Count);

            var irmaFlConnection = dbConfig.Connections
                .Single(c => c.Category == DatabaseCategoryEnum.IRMA && c.ServerName.Contains("fl"));
            Assert.IsNotNull(irmaFlConnection);
            Assert.AreEqual("ItemCatalog_Test", irmaFlConnection.DatabaseName);
            Assert.AreEqual(@"idd-fl\fld", irmaFlConnection.ServerName);
            Assert.AreEqual("ItemCatalog_FL", irmaFlConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Dev0, irmaFlConnection.Environment);
            Assert.IsFalse(irmaFlConnection.IsEntityFramework);

            var irmaPnConnection = dbConfig.Connections
                .Single(c => c.Category == DatabaseCategoryEnum.IRMA && c.ServerName.Contains("pn"));
            Assert.IsNotNull(irmaPnConnection);
            Assert.AreEqual("ItemCatalog_Test", irmaPnConnection.DatabaseName);
            Assert.AreEqual(@"idt-pn\pnt", irmaPnConnection.ServerName);
            Assert.AreEqual("ItemCatalog_PN", irmaPnConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Tst0, irmaPnConnection.Environment);
            Assert.IsFalse(irmaPnConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIconEfAndIrmaSqlWithIconLogging_ReadsLoggingConnection()
        {
            // Arrange
            var configPath = $"{testDataFolder}\\SampleDbConfig_IconEfAndIrmaSqlWithIconLogging.config";
            var configXmlDoc = XDocument.Load(configPath);
            var configReader = new ExternalConfigXmlManager(configPath);
            // Act
            var dbConfig = configReader.ReadDatabaseConfiguration(configXmlDoc, configTestData.Models.EnvironmentsList);
            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsNotNull(dbConfig.LoggingConnection);
            Assert.AreSame(dbConfig.LoggingConnection, dbConfig.Connections.Single(c => c.IsUsedForLogging));
            Assert.AreEqual("Icon", dbConfig.LoggingConnection.DatabaseName);
            Assert.AreEqual("dbLogIconGlobalController", dbConfig.LoggingConnection.ConnectionStringName);
            Assert.AreEqual(@"cewd1815\SQLSHARED2012D", dbConfig.LoggingConnection.ServerName);
            Assert.AreEqual(EnvironmentEnum.Tst0, dbConfig.LoggingConnection.Environment);
            Assert.IsFalse(dbConfig.LoggingConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIconEfAndIrmaEfWithIconLogging_ReadsDbConnections()
        {
            // Arrange
            var configPath = $"{testDataFolder}\\SampleDbConfig_IconEfAndIrmaEfWithIconLogging.config";
            var configXmlDoc = XDocument.Load(configPath);
            var configReader = new ExternalConfigXmlManager(configPath);
            // Act
            var dbConfig = configReader.ReadDatabaseConfiguration(configXmlDoc, configTestData.Models.EnvironmentsList);
            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon));
            var iconConnection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon);
            Assert.IsNotNull(iconConnection);
            Assert.AreEqual("Icon", iconConnection.DatabaseName);
            Assert.AreEqual(@"cewd1815\SQLSHARED2012D", iconConnection.ServerName);
            Assert.AreEqual("IconContext", iconConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Tst0, iconConnection.Environment);
            Assert.IsTrue(iconConnection.IsEntityFramework);

            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.IRMA));
            var irmaConnections = dbConfig.GetAllNonLoggingConnectionsOfCategory(DatabaseCategoryEnum.IRMA);
            Assert.AreEqual(12, irmaConnections.Count);

            var irmaFlConnection = dbConfig.Connections
                .Single(c => c.Category == DatabaseCategoryEnum.IRMA && c.ServerName.Contains("fl"));
            Assert.IsNotNull(irmaFlConnection);
            Assert.AreEqual("ItemCatalog_Test", irmaFlConnection.DatabaseName);
            Assert.AreEqual(@"idd-fl\fld", irmaFlConnection.ServerName);
            Assert.AreEqual("ItemCatalog_FL", irmaFlConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Dev0, irmaFlConnection.Environment);
            Assert.IsTrue(irmaFlConnection.IsEntityFramework);

            var irmaPnConnection = dbConfig.Connections
                .Single(c => c.Category == DatabaseCategoryEnum.IRMA && c.ServerName.Contains("pn"));
            Assert.IsNotNull(irmaPnConnection);
            Assert.AreEqual("ItemCatalog_Test", irmaPnConnection.DatabaseName);
            Assert.AreEqual(@"idt-pn\pnt", irmaPnConnection.ServerName);
            Assert.AreEqual("ItemCatalog_PN", irmaPnConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Tst0, irmaPnConnection.Environment);
            Assert.IsTrue(irmaPnConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIconEfAndIrmaEfWithIconLogging_ReadsLoggingConnection()
        {
            // Arrange
            var configPath = $"{testDataFolder}\\SampleDbConfig_IconEfAndIrmaEfWithIconLogging.config";
            var configXmlDoc = XDocument.Load(configPath);
            var configReader = new ExternalConfigXmlManager(configPath);
            // Act
            var dbConfig = configReader.ReadDatabaseConfiguration(configXmlDoc, configTestData.Models.EnvironmentsList);
            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsNotNull(dbConfig.LoggingConnection);
            Assert.AreSame(dbConfig.LoggingConnection, dbConfig.Connections.Single(c => c.IsUsedForLogging));
            Assert.AreEqual("Icon", dbConfig.LoggingConnection.DatabaseName);
            Assert.AreEqual(@"cewd1815\SQLSHARED2012D", dbConfig.LoggingConnection.ServerName);
            Assert.AreEqual("dbLogIconRegionalController", dbConfig.LoggingConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Tst0, dbConfig.LoggingConnection.Environment);
            Assert.IsFalse(dbConfig.LoggingConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIconSqlAndIrmaSqlAndMammothSqlWithIconLogging_ReadsDbConnections()
        {
            // Arrange
            var configPath = $"{testDataFolder}\\SampleDbConfig_IconSqlAndIrmaSqlAndMammothSqlWithIconLogging.config";
            var configXmlDoc = XDocument.Load(configPath);
            var configReader = new ExternalConfigXmlManager(configPath);
            // Act
            var dbConfig = configReader.ReadDatabaseConfiguration(configXmlDoc, configTestData.Models.EnvironmentsList);
            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon));
            var iconConnection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon);
            Assert.IsNotNull(iconConnection);
            Assert.AreEqual("Icon", iconConnection.DatabaseName);
            Assert.AreEqual(@"CEWD1815\SQLSHARED2012D", iconConnection.ServerName);
            Assert.AreEqual("Icon", iconConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Tst0, iconConnection.Environment);
            Assert.IsFalse(iconConnection.IsEntityFramework);

            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Mammoth));
            var mammothConnection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Mammoth);
            Assert.IsNotNull(mammothConnection);
            Assert.AreEqual("Mammoth", mammothConnection.DatabaseName);
            Assert.AreEqual(@"MAMMOTH-DB01-DEV\MAMMOTH", mammothConnection.ServerName);
            Assert.AreEqual("Mammoth", mammothConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Tst0, mammothConnection.Environment);
            Assert.IsFalse(mammothConnection.IsEntityFramework);

            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.IRMA));
            var irmaConnections = dbConfig.GetAllNonLoggingConnectionsOfCategory(DatabaseCategoryEnum.IRMA);
            Assert.AreEqual(12, irmaConnections.Count);

            var irmaFlConnection = dbConfig.Connections
                .Single(c => c.Category == DatabaseCategoryEnum.IRMA && c.ServerName.Contains("fl"));
            Assert.IsNotNull(irmaFlConnection);
            Assert.AreEqual("ItemCatalog_Test", irmaFlConnection.DatabaseName);
            Assert.AreEqual(@"idd-fl\fld", irmaFlConnection.ServerName);
            Assert.AreEqual("ItemCatalog_FL", irmaFlConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Dev0, irmaFlConnection.Environment);
            Assert.IsFalse(irmaFlConnection.IsEntityFramework);

            var irmaPnConnection = dbConfig.Connections
                .Single(c => c.Category == DatabaseCategoryEnum.IRMA && c.ServerName.Contains("pn"));
            Assert.IsNotNull(irmaPnConnection);
            Assert.AreEqual("ItemCatalog_Test", irmaPnConnection.DatabaseName);
            Assert.AreEqual(@"idt-pn\pnt", irmaPnConnection.ServerName);
            Assert.AreEqual("ItemCatalog_PN", irmaPnConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Tst0, irmaPnConnection.Environment);
            Assert.IsFalse(irmaPnConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasIconSqlAndIrmaSqlAndMammothSqlWithIconLogging_ReadsLoggingConnection()
        {
            // Arrange
            var configPath = $"{testDataFolder}\\SampleDbConfig_IconSqlAndIrmaSqlAndMammothSqlWithIconLogging.config";
            var configXmlDoc = XDocument.Load(configPath);
            var configReader = new ExternalConfigXmlManager(configPath);
            // Act
            var dbConfig = configReader.ReadDatabaseConfiguration(configXmlDoc, configTestData.Models.EnvironmentsList);
            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsNotNull(dbConfig.LoggingConnection);
            Assert.AreSame(dbConfig.LoggingConnection, dbConfig.Connections.Single(c => c.IsUsedForLogging));
            Assert.AreEqual("Icon", dbConfig.LoggingConnection.DatabaseName);
            Assert.AreEqual(@"cewd1815\SQLSHARED2012D", dbConfig.LoggingConnection.ServerName);
            Assert.AreEqual("dbLogMonitor", dbConfig.LoggingConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Tst0, dbConfig.LoggingConnection.Environment);
            Assert.IsFalse(dbConfig.LoggingConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasVimSqlWithFileLogging_ReadsDbConnections()
        {
            // Arrange
            var configPath = $"{testDataFolder}\\SampleDbConfig_VimSqlWithFileLogging.config";
            var configXmlDoc = XDocument.Load(configPath);
            var configReader = new ExternalConfigXmlManager(configPath);
            // Act
            var dbConfig = configReader.ReadDatabaseConfiguration(configXmlDoc, configTestData.Models.EnvironmentsList);
            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Vim));
            var vimConnection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Vim);
            Assert.IsNotNull(vimConnection);
            Assert.AreEqual("", vimConnection.DatabaseName);
            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=vim_dt.wfm.pvt)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=vim_dt)))", vimConnection.ServerName);
            Assert.AreEqual("VIM", vimConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Undefined, vimConnection.Environment);
            Assert.IsFalse(vimConnection.IsEntityFramework);

            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Other));
            var promoConnection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Other);
            Assert.IsNotNull(promoConnection);
            Assert.AreEqual("wfmpromotions", promoConnection.DatabaseName);
            Assert.AreEqual(@"promodb-dev", promoConnection.ServerName);
            Assert.AreEqual("PROMO", promoConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Undefined, promoConnection.Environment);
            Assert.IsFalse(promoConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenAppHasVimSqlWithFileLogging_LoggingConnectionIsNull()
        {
            // Arrange
            var configPath = $"{testDataFolder}\\SampleDbConfig_VimSqlWithFileLogging.config";
            var configXmlDoc = XDocument.Load(configPath);
            var configReader = new ExternalConfigXmlManager(configPath);

            // Act
            var dbConfig = configReader.ReadDatabaseConfiguration(configXmlDoc, configTestData.Models.EnvironmentsList);

            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsNull(dbConfig.LoggingConnection);
            Assert.AreEqual("None", dbConfig.LoggingSummary);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenEncryptedConnection_ReadsDbConnection()
        {
            // Arrange
            var configPath = $"{testDataFolder}\\SampleDbConfig_EncryptedDatabaseConnection.config";
            var configXmlDoc = XDocument.Load(configPath);
            var configReader = new ExternalConfigXmlManager(configPath);

            // Act
            var dbConfig = configReader.ReadDatabaseConfiguration(configXmlDoc, configTestData.Models.EnvironmentsList);

            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Encrypted));
            var encryptedConnection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Encrypted);
            Assert.IsNotNull(encryptedConnection);
            Assert.AreEqual("{Encrypted}", encryptedConnection.DatabaseName);
            Assert.AreEqual("{Encrypted}", encryptedConnection.ServerName);
            Assert.AreEqual("{Encrypted}", encryptedConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Undefined, encryptedConnection.Environment);
            Assert.IsFalse(encryptedConnection.IsEntityFramework);
        }

        [TestMethod]
        public void ReadDatabaseConfiguration_WhenIconAndIrmaEfDev1_ReadsDbConnection()
        {
            // Arrange
            var configPath = $"{testDataFolder}\\SampleDbConfig_IconEfAndIrmaEfWithIconLoggingDev1.config";
            var configXmlDoc = XDocument.Load(configPath);
            var configReader = new ExternalConfigXmlManager(configPath);

            // Act
            var dbConfig = configReader.ReadDatabaseConfiguration(configXmlDoc, configTestData.Models.EnvironmentsList);

            // Assert
            Assert.IsNotNull(dbConfig);
            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon));
            var iconConnection = dbConfig.GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum.Icon);
            Assert.IsNotNull(iconConnection);
            Assert.AreEqual("Icon", iconConnection.DatabaseName);
            Assert.AreEqual(@"icon-db01-dev", iconConnection.ServerName);
            Assert.AreEqual("IconContext", iconConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Dev1, iconConnection.Environment);
            Assert.IsTrue(iconConnection.IsEntityFramework);

            Assert.IsTrue(dbConfig.HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum.IRMA));
            var irmaConnections = dbConfig.GetAllNonLoggingConnectionsOfCategory(DatabaseCategoryEnum.IRMA);
            Assert.AreEqual(5, irmaConnections.Count);

            var irmaFlConnection = dbConfig.Connections
                .Single(c => c.Category == DatabaseCategoryEnum.IRMA && c.ServerName.Contains("fl"));
            Assert.IsNotNull(irmaFlConnection);
            Assert.AreEqual("ItemCatalog", irmaFlConnection.DatabaseName);
            Assert.AreEqual(@"fl-db01-dev", irmaFlConnection.ServerName);
            Assert.AreEqual("ItemCatalog_FL", irmaFlConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Dev1, irmaFlConnection.Environment);
            Assert.IsTrue(irmaFlConnection.IsEntityFramework);

            var irmaPnConnection = dbConfig.Connections
                .Single(c => c.Category == DatabaseCategoryEnum.IRMA && c.ServerName.Contains("rm"));
            Assert.IsNotNull(irmaPnConnection);
            Assert.AreEqual("ItemCatalog", irmaPnConnection.DatabaseName);
            Assert.AreEqual(@"rm-db01-dev", irmaPnConnection.ServerName);
            Assert.AreEqual("ItemCatalog_RM", irmaPnConnection.ConnectionStringName);
            Assert.AreEqual(EnvironmentEnum.Dev1, irmaPnConnection.Environment);
            Assert.IsTrue(irmaPnConnection.IsEntityFramework);
        }
    }
}
