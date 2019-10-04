using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Dashboard.Mvc.Helpers;
using Icon.Dashboard.Mvc.UnitTests.TestData;

namespace Icon.Dashboard.Mvc.UnitTests.HelperTests
{
    /// <summary>
    /// Summary description for UtilsUnitTests
    /// </summary>
    [TestClass]
    public class UtilsUnitTests
    {
        private RemoteServiceTestData serviceTestData = new RemoteServiceTestData();

        [TestMethod]
        public void SplitHostsFromServerUrlSetting_WhenServerUrlIsNull_ReturnsNull()
        {
            // Arrange
            string serverValueFromAppSettings = null;
            // Act
            var actualHosts = Utils.SplitHostsFromServerUrlSetting(serverValueFromAppSettings);
            // Assert
            Assert.IsNull(actualHosts);
        }

        [TestMethod]
        public void SplitHostsFromServerUrlSetting_WhenServerUrlIsEmpty_ReturnsNull()
        {
            // Arrange
            string serverValueFromAppSettings = "";
            // Act
            var actualHosts = Utils.SplitHostsFromServerUrlSetting(serverValueFromAppSettings);
            // Assert
            Assert.IsNull(actualHosts);
        }

        [TestMethod]
        public void SplitHostsFromServerUrlSetting_WhenServerUrlIsWhitespace_ReturnsNull()
        {
            // Arrange
            string serverValueFromAppSettings = "  ";
            // Act
            var actualHosts = Utils.SplitHostsFromServerUrlSetting(serverValueFromAppSettings);
            // Assert
            Assert.IsNull(actualHosts);
        }

        [TestMethod]
        public void SplitHostsFromServerUrlSetting_WhenServerUrlHasOneValue_ReturnsExpectedList()
        {
            // Arrange
            var serverValueFromAppSettings = "ssl://DEV-ESB-EMS-1.wfm.pvt:7233";
            var expectedHosts = new List<string> { "DEV-ESB-EMS-1" };
            // Act
            var actualHosts = Utils.SplitHostsFromServerUrlSetting(serverValueFromAppSettings);
            // Assert
            CustomAsserts.ListsAreEqual(expectedHosts, actualHosts);
        }

        [TestMethod]
        public void SplitHostsFromServerUrlSetting_WhenServerUrlHasTwoalues_ReturnsExpectedList()
        {
            // Arrange
            var serverValueFromAppSettings = "ssl://TST-ESB-EMS-1.wfm.pvt:17293,ssl://TST-ESB-EMS-2.wfm.pvt:17293";
            var expectedHosts = new List<string> { "TST-ESB-EMS-1", "TST-ESB-EMS-2" };
            // Act
            var actualHosts = Utils.SplitHostsFromServerUrlSetting(serverValueFromAppSettings);
            // Assert
            CustomAsserts.ListsAreEqual(expectedHosts, actualHosts);
        }

        [TestMethod]
        public void CombineDictionariesIgnoreDuplicates_WhenParmaetersNull_ReturnsEmptyDictionary()
        {
            // Arrange
            // Act
            var combinedAppSettings = Utils
                .CombineDictionariesIgnoreDuplicates<string,string>(null);
            // Assert
            Assert.IsNotNull(combinedAppSettings);
            Assert.AreEqual(0, combinedAppSettings.Count);
        }

        [TestMethod]
        public void CombineDictionariesIgnoreDuplicates_WhenFirstDictionaryNull_ReturnsAllValuesFromSecond()
        {
            // Arrange
            Dictionary<string, string> nonEsbAppSettings = null;
            var esbAppSettings = serviceTestData.Services.AppSettings.FakeEsbSettings_TEST_Icon;
                // Act
            var combinedAppSettings = Utils
                .CombineDictionariesIgnoreDuplicates(nonEsbAppSettings, esbAppSettings);
            // Assert
            Assert.IsNotNull(combinedAppSettings);
            Assert.AreEqual(esbAppSettings.Count, combinedAppSettings.Count);
        }

        [TestMethod]
        public void CombineDictionariesIgnoreDuplicates_WhenSecondDictionaryNull_ReturnsAllValueFromFirst()
        {
            // Arrange
            var nonEsbAppSettings = serviceTestData.Services.AppSettings.FakeAppSettings_NoEsb_1;
            Dictionary<string, string> esbAppSettings = null;
            // Act
            var combinedAppSettings = Utils
                .CombineDictionariesIgnoreDuplicates(nonEsbAppSettings, esbAppSettings);
            // Assert
            Assert.IsNotNull(combinedAppSettings);
            Assert.AreEqual(nonEsbAppSettings.Count, combinedAppSettings.Count);
        }

        [TestMethod]
        public void CombineDictionariesIgnoreDuplicates_WhenFirstDictionaryEmpty_ReturnsAllValuesFromSecond()
        {
            // Arrange
            var nonEsbAppSettings = new Dictionary<string, string>();
            var esbAppSettings = serviceTestData.Services.AppSettings.FakeEsbSettings_TEST_Icon;
            // Act
            var combinedAppSettings = Utils
                .CombineDictionariesIgnoreDuplicates(nonEsbAppSettings, esbAppSettings);
            // Assert
            Assert.IsNotNull(combinedAppSettings);
            Assert.AreEqual(esbAppSettings.Count, combinedAppSettings.Count);
        }

        [TestMethod]
        public void CombineDictionariesIgnoreDuplicates_WhenSecondDictionaryEmpty_ReturnsAllValuesFromFirst()
        {
            // Arrange
            var nonEsbAppSettings = serviceTestData.Services.AppSettings.FakeAppSettings_NoEsb_1;
            var esbAppSettings = new Dictionary<string, string>();
            // Act
            var combinedAppSettings = Utils
                .CombineDictionariesIgnoreDuplicates(nonEsbAppSettings, esbAppSettings);
            // Assert
            Assert.IsNotNull(combinedAppSettings);
            Assert.AreEqual(nonEsbAppSettings.Count, combinedAppSettings.Count);
        }

        [TestMethod]
        public void CombineDictionariesIgnoreDuplicates_WhenNoDuplicates_ReturnsAllCombinedValues()
        {
            // Arrange
            var nonEsbAppSettings = serviceTestData.Services.AppSettings.FakeAppSettings_NoEsb_1;
            var esbAppSettings = serviceTestData.Services.AppSettings.FakeEsbSettings_TEST_Icon;
            var expectedCount = nonEsbAppSettings.Count + esbAppSettings.Count;
            // Act
            var combinedAppSettings = Utils
                .CombineDictionariesIgnoreDuplicates(nonEsbAppSettings, esbAppSettings);
            // Assert
            Assert.AreEqual(expectedCount, combinedAppSettings.Count);
        }

        [TestMethod]
        public void CombineDictionariesIgnoreDuplicates_WhenDuplicates_ReturnsOnlyFirstValues()
        {
            // Arrange
            var keyForDupe = "key1";
            var nonEsbAppSettings = new Dictionary<string, string>
            {
                {keyForDupe, "value 1"},
            };
            var esbAppSettings = new Dictionary<string, string>
            {
                {keyForDupe, "value 2"},
            };
            // Act
            var combinedAppSettings = Utils
                .CombineDictionariesIgnoreDuplicates(nonEsbAppSettings, esbAppSettings);
            // Assert
            Assert.AreEqual("value 1", combinedAppSettings[keyForDupe]);
        }

        [TestMethod]
        public void CombineDictionariesIgnoreDuplicates_WhenDuplicates_ReturnsAllNonDuplicateValues()
        {
            // Arrange
            var nonEsbAppSettings = new Dictionary<string, string>
            {
                {"key2", "value x" },
                {"key1", "value 1"},
                {"key3", "value xxx" },
            };
            var esbAppSettings = new Dictionary<string, string>
            {
                {"key4", "value yyy" },
                {"key1", "value 2"},
                {"key5", "value y" },
            };
            // Act
            var combinedAppSettings = Utils
                .CombineDictionariesIgnoreDuplicates(nonEsbAppSettings, esbAppSettings);
            // Assert
            Assert.AreEqual(5, combinedAppSettings.Count);
            Assert.AreEqual("value x", combinedAppSettings["key2"]);
            Assert.AreEqual("value xxx", combinedAppSettings["key3"]);
            Assert.AreEqual("value yyy", combinedAppSettings["key4"]);
            Assert.AreEqual("value y", combinedAppSettings["key5"]);
        }
    }
}
