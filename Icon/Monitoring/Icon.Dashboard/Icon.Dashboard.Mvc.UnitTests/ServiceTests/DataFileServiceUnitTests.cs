using Icon.Dashboard.Mvc.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.Mvc.UnitTests.ServiceTests
{
    [TestClass]
    public class DataFileServiceUnitTests
    {
        [TestMethod]
        public void DataFileServiceWrapper_GetHostsFromServerUrl_BlankUrl_ReturnsNull()
        {
            // Arrange
            string serverUrlValue = "";
            // Act
            var hosts = DataFileServiceWrapper.GetHostsFromServerUrl(serverUrlValue);
            // Assert
            Assert.IsNull(hosts);
        }

        [TestMethod]
        public void DataFileServiceWrapper_GetHostsFromServerUrl_SingleTypicalEsbUrl_ReturnsHost()
        {
            // Arrange
            string serverUrlValue = "ssl://cerd1640.wfm.pvt:27293";
            string expectedHostValue = "cerd1640";
            // Act
            var hosts = DataFileServiceWrapper.GetHostsFromServerUrl(serverUrlValue);
            // Assert
            Assert.AreEqual(1, hosts.Count());
            Assert.AreEqual(expectedHostValue, hosts.ElementAt(0));
        }

        [TestMethod]
        public void DataFileServiceWrapper_GetHostsFromServerUrl_DoubleTypicalEsbValue_ReturnsExpectedHosts()
        {
            // Arrange
            string serverUrlValue = "ssl://cerd1639.wfm.pvt:27293,ssl://cerd1640.wfm.pvt:27293";
            string expectedHostValue0 = "cerd1639";
            string expectedHostValue1 = "cerd1640";
            // Act
            var hosts = DataFileServiceWrapper.GetHostsFromServerUrl(serverUrlValue);
            // Assert
            Assert.AreEqual(2, hosts.Count());
            Assert.AreEqual(expectedHostValue0, hosts.ElementAt(0));
            Assert.AreEqual(expectedHostValue1, hosts.ElementAt(1));
        }

        [TestMethod]
        public void DataFileServiceWrapper_GetHostsFromServerUrl_NormalWebUrl_ReturnsHost()
        {
            // Arrange
            string serverUrlValue = "https://www.wholefoods.com:8080";
            string expectedHostValue = "www.wholefoods.com";
            // Act
            var hosts = DataFileServiceWrapper.GetHostsFromServerUrl(serverUrlValue);
            // Assert
            Assert.AreEqual(1, hosts.Count());
            Assert.AreEqual(expectedHostValue, hosts.ElementAt(0));
        }
    }
}
