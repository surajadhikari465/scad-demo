using Icon.Dashboard.Mvc.Models;
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
    public class RemoteServiceConfigDataModelUnitTests
    {
        private RemoteServiceTestData serviceTestData = new RemoteServiceTestData();

        [TestMethod]
        public void Constructor_InitializesAppSettingsDictionary()
        {
            // Arrange
            // Act
            var serviceConfigData = new RemoteServiceConfigDataModel();
            // Asssert
            Assert.IsNotNull(serviceConfigData.NonEsbAppSettings);
        }

        [TestMethod]
        public void Constructor_InitializesEsbConnectionsList()
        {
            // Arrange
            var serviceConfigData = new RemoteServiceConfigDataModel();
            // Act
            // Asssert
            Assert.IsNotNull(serviceConfigData.EsbConnections);
        }

        [TestMethod]
        public void NonEsbAppSettings_WhenAppSettingsDictionaryEmpty_ReturnsEmptyDictionar()
        {
            // Arrange
            var serviceConfigData = new RemoteServiceConfigDataModel();
            // Act
            // Asssert
            Assert.IsNotNull(serviceConfigData.NonEsbAppSettings);
            Assert.AreEqual(0, serviceConfigData.NonEsbAppSettings.Count);
        }

    }
}
