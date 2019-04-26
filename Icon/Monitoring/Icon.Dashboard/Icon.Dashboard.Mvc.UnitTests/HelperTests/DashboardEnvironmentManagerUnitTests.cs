using FluentAssertions;
using Icon.Dashboard.Mvc.Helpers;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.Services;
using Icon.Dashboard.Mvc.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.Mvc.UnitTests.HelperTests
{
    [TestClass]
    public class DashboardEnvironmentManagerUnitTests
    {
        [TestMethod]
        public void EnvironmentManager_GetWebServersDictionary_WithDefinedAppSettings_ShouldReturnsExpectedCount()
        {
            // Arrange
            var environmentManager = new DashboardEnvironmentManager();
            const int expectedServerCount = 4;
            // Act
            var servers = environmentManager.GetWebServersDictionary();
            // Assert
            servers.Should().NotBeNull();
            servers.Count.Should().Be(expectedServerCount);
        }

        [TestMethod]
        public void EnvironmentManager_GetWebServerForEnvironment_DEV_ShouldReturnsExpectedServerData()
        {
            // Arrange
            var environmentManager = new DashboardEnvironmentManager();
            var env = EnvironmentEnum.Dev;
            var expectedServerName = $"irma{env}app1";
            // Act
            var serverForEnv = environmentManager.GetWebServerKvpForEnvironment(env);
            // Assert
            serverForEnv.Value.ToLower().Should().BeEquivalentTo(expectedServerName.ToLower());
        }

        [TestMethod]
        public void EnvironmentManager_GetWebServerKvpForEnvironment_TST_ShouldReturnsExpectedServerData()
        {
            // Arrange
            var environmentManager = new DashboardEnvironmentManager();
            var env = EnvironmentEnum.Test;
            var expectedServerName = $"irma{env}app1";
            // Act
            var serverForEnv = environmentManager.GetWebServerKvpForEnvironment(env);
            // Assert
            serverForEnv.Value.ToLower().Should().BeEquivalentTo(expectedServerName.ToLower());
        }

        [TestMethod]
        public void EnvironmentManager_GetWebServerKvpForEnvironment_QA_ShouldReturnsExpectedServerData()
        {
            // Arrange
            var environmentManager = new DashboardEnvironmentManager();
            var env = EnvironmentEnum.QA;
            var expectedServerName = $"irma{env}app1";
            // Act
            var serverForEnv = environmentManager.GetWebServerKvpForEnvironment(env);
            // Assert
            serverForEnv.Value.ToLower().Should().BeEquivalentTo(expectedServerName.ToLower());
        }

        [TestMethod]
        public void EnvironmentManager_GetWebServerKvpForEnvironment_Perf_ShouldReturnsExpectedServerData()
        {
            // Arrange
            var environmentManager = new DashboardEnvironmentManager();
            var env = EnvironmentEnum.Perf;
            // Act
            var serverForEnv = environmentManager.GetWebServerKvpForEnvironment(env);
            // Assert
            Assert.IsNull(serverForEnv.Value);
        }

        [TestMethod]
        public void EnvironmentManager_GetWebServerKvpForEnvironment_PRD_ShouldReturnsExpectedServerData()
        {
            // Arrange
            var environmentManager = new DashboardEnvironmentManager();
            var env = EnvironmentEnum.Prd;
            var expectedServerName = $"irma{env}app1";
            // Act
            var serverForEnv = environmentManager.GetWebServerKvpForEnvironment(env);
            // Assert
            serverForEnv.Value.ToLower().Should().BeEquivalentTo(expectedServerName.ToLower());
        }

        [TestMethod]
        public void EnvironmentManager_GetSupportServerLinks_Dev_ShouldReturnsExpectedUrls()
        {
            // Arrange
            var environment = EnvironmentEnum.Dev;
            var environmentManager = new DashboardEnvironmentManager();
            var expectedMwsServerNameDev = $"Mammoth Web Support Dev";
            var expectedIconWebServerNameDev = $"Icon Web Dev";
            var expectedTibcoServerNameDev = $"TIBCO Admin Dev";
            var expectedMwsServerDev = $"http://irmadevapp1/MammothWebSupport";
            var expectedIconWebServerDev = $"http://icon-dev/";
            var expectedTibcoServerDev = $"https://cerd1668:8090/";
            // Act
            var serverUrls = environmentManager.GetSupportServerLinks(environment);
            // Assert
            serverUrls.Should().NotBeNull();
            Assert.IsTrue(serverUrls.Any(s=>s.Item1.Equals(expectedMwsServerNameDev)));
            Assert.AreEqual(expectedMwsServerDev, serverUrls.FirstOrDefault(s=>s.Item1.Equals(expectedMwsServerNameDev)).Item2);
            Assert.IsTrue(serverUrls.Any(s=>s.Item1.Equals(expectedIconWebServerNameDev)));
            Assert.AreEqual(expectedIconWebServerDev, serverUrls.FirstOrDefault(s=>s.Item1.Equals(expectedIconWebServerNameDev)).Item2);
            Assert.IsTrue(serverUrls.Any(s=>s.Item1.Equals(expectedTibcoServerNameDev)));
            Assert.AreEqual(expectedTibcoServerDev, serverUrls.FirstOrDefault(s=>s.Item1.Equals(expectedTibcoServerNameDev)).Item2);
        }

        [TestMethod]
        public void EnvironmentManager_GetSupportServerLinks_Test_ShouldReturnsExpectedUrls()
        {
            // Arrange
            var environment = EnvironmentEnum.Test;
            var environmentManager = new DashboardEnvironmentManager();
            var expectedMwsServerNameTest = $"Mammoth Web Support Test";
            var expectedIconWebServerNameTest = $"Icon Web Test";
            var expectedTibcoServerNameTest1 = $"TIBCO Admin Test 1";
            var expectedTibcoServerNameTest2 = $"TIBCO Admin Test 2";
            var expectedMwsServerTest = $"http://irmatestapp1/MammothWebSupport";
            var expectedIconWebServerTest = $"http://icon-test/";
            var expectedTibcoServerTest1 = $"https://cerd1669:18090/";
            var expectedTibcoServerTest2 = $"https://cerd1670:18090/";
            // Act
            var serverUrls = environmentManager.GetSupportServerLinks(environment);
            // Assert
            serverUrls.Should().NotBeNull();
            Assert.IsTrue(serverUrls.Any(s=>s.Item1.Equals(expectedMwsServerNameTest)));
            Assert.AreEqual(expectedMwsServerTest, serverUrls.FirstOrDefault(s=>s.Item1.Equals(expectedMwsServerNameTest)).Item2);
            Assert.IsTrue(serverUrls.Any(s=>s.Item1.Equals(expectedIconWebServerNameTest)));
            Assert.AreEqual(expectedIconWebServerTest, serverUrls.FirstOrDefault(s=>s.Item1.Equals(expectedIconWebServerNameTest)).Item2);
            Assert.IsTrue(serverUrls.Any(s=>s.Item1.Equals(expectedTibcoServerNameTest1)));
            Assert.AreEqual(expectedTibcoServerTest1, serverUrls.FirstOrDefault(s=>s.Item1.Equals(expectedTibcoServerNameTest1)).Item2);
            Assert.IsTrue(serverUrls.Any(s => s.Item1.Equals(expectedTibcoServerNameTest2)));
            Assert.AreEqual(expectedTibcoServerTest2, serverUrls.FirstOrDefault(s => s.Item1.Equals(expectedTibcoServerNameTest2)).Item2);
        }

        [TestMethod]
        public void EnvironmentManager_GetSupportServerLinks_Perf_ShouldReturnsExpectedUrls()
        {
            // Arrange
            var environment = EnvironmentEnum.Perf;
            var environmentManager = new DashboardEnvironmentManager();
            var expectedMwsServerNamePerf = $"Mammoth Web Support Perf";
            var expectedIconWebServerNamePerf = $"Icon Web Perf";
            var expectedTibcoServerNamePerf1 = $"TIBCO Admin Perf 1";
            var expectedTibcoServerNamePerf2 = $"TIBCO Admin Perf 2";
            var expectedMwsServerPerf = $"http://irmaqaapp1/MammothWebSupportPerf";
            var expectedIconWebServerPerf = $"http://icon-perf/";
            var expectedTibcoServerPerf1 = $"https://cerd1666:28090/";
            var expectedTibcoServerPerf2 = $"https://cerd1667:28090/";
            string actual = string.Empty;
            // Act
            var serverUrls = environmentManager.GetSupportServerLinks(environment);
            // Assert
            serverUrls.Should().NotBeNull();
            Assert.IsTrue(serverUrls.Any(s=>s.Item1.Equals(expectedMwsServerNamePerf)));
            Assert.AreEqual(expectedMwsServerPerf, serverUrls.FirstOrDefault(s=>s.Item1.Equals(expectedMwsServerNamePerf)).Item2);
            Assert.IsTrue(serverUrls.Any(s=>s.Item1.Equals(expectedIconWebServerNamePerf)));
            Assert.AreEqual(expectedIconWebServerPerf, serverUrls.FirstOrDefault(s=>s.Item1.Equals(expectedIconWebServerNamePerf)).Item2);
            Assert.IsTrue(serverUrls.Any(s=>s.Item1.Equals(expectedTibcoServerNamePerf1)));
            Assert.AreEqual(expectedTibcoServerPerf1, serverUrls.FirstOrDefault(s=>s.Item1.Equals(expectedTibcoServerNamePerf1)).Item2);
            Assert.IsTrue(serverUrls.Any(s => s.Item1.Equals(expectedTibcoServerNamePerf2)));
            Assert.AreEqual(expectedTibcoServerPerf2, serverUrls.FirstOrDefault(s => s.Item1.Equals(expectedTibcoServerNamePerf2)).Item2);
        }


        [TestMethod]
        public void EnvironmentManager_GetSupportServerLinks_QA_ShouldReturnsExpectedUrls()
        {
            // Arrange
            var environment = EnvironmentEnum.QA;
            var environmentManager = new DashboardEnvironmentManager();
            var expectedMwsServerNameQa = $"Mammoth Web Support QA";
            var expectedIconWebServerNameQa = $"Icon Web QA";
            var expectedTibcoServerNameQa1 = $"TIBCO Admin QA 1";
            var expectedTibcoServerNameQa2 = $"TIBCO Admin QA 2";
            var expectedMwsServerQa = $"http://irmaqaapp1/MammothWebSupport";
            var expectedIconWebServerQa = $"http://icon-qa/";
            var expectedTibcoServerQa1 = $"https://cerd1673:28090/";
            var expectedTibcoServerQa2 = $"https://cerd1674:28090/";
            string actual = string.Empty;
            // Act
            var serverUrls = environmentManager.GetSupportServerLinks(environment);
            // Assert
            serverUrls.Should().NotBeNull();
            Assert.IsTrue(serverUrls.Any(s=>s.Item1.Equals(expectedMwsServerNameQa)));
            Assert.AreEqual(expectedMwsServerQa, serverUrls.FirstOrDefault(s=>s.Item1.Equals(expectedMwsServerNameQa)).Item2);
            Assert.IsTrue(serverUrls.Any(s=>s.Item1.Equals(expectedIconWebServerNameQa)));
            Assert.AreEqual(expectedIconWebServerQa, serverUrls.FirstOrDefault(s=>s.Item1.Equals(expectedIconWebServerNameQa)).Item2);
            Assert.IsTrue(serverUrls.Any(s=>s.Item1.Equals(expectedTibcoServerNameQa1)));
            Assert.AreEqual(expectedTibcoServerQa1, serverUrls.FirstOrDefault(s=>s.Item1.Equals(expectedTibcoServerNameQa1)).Item2);
            Assert.IsTrue(serverUrls.Any(s => s.Item1.Equals(expectedTibcoServerNameQa2)));
            Assert.AreEqual(expectedTibcoServerQa2, serverUrls.FirstOrDefault(s => s.Item1.Equals(expectedTibcoServerNameQa2)).Item2);
        }

        [TestMethod]
        public void EnvironmentManager_GetDefaultAppServersForEnvironment_DEV_ShouldReturnsExpectedUrls()
        {
            // Arrange
            var environmentManager = new DashboardEnvironmentManager();
            var env = EnvironmentEnum.Dev;
            List<string> expectedServerNames = new List<string>
            {
                "vm-icon-dev1"
            };

            // Act
            var actualSevers = environmentManager.GetDefaultAppServersForEnvironment(env);

            // Assert
            foreach ( var expectedServer in expectedServerNames)
            {
                Assert.IsTrue(actualSevers.Contains(expectedServer), $"Expectedj to find '{expectedServer}'");
            }
        }

        [TestMethod]
        public void EnvironmentManager_GetDefaultAppServersForEnvironment_TST_ShouldReturnsExpectedUrls()
        {
            // Arrange
            var environmentManager = new DashboardEnvironmentManager();
            var env = EnvironmentEnum.Test;
            List<string> expectedServerNames = new List<string>
            {
                "vm-icon-test1",
                "vm-icon-test2"
            };

            // Act
            var actualSevers = environmentManager.GetDefaultAppServersForEnvironment(env);

            // Assert
            foreach (var expectedServer in expectedServerNames)
            {
                Assert.IsTrue(actualSevers.Contains(expectedServer), $"Expected to find '{expectedServer}'");
            }
        }

        [TestMethod]
        public void EnvironmentManager_GetDefaultAppServersForEnvironment_QA_ShouldReturnsExpectedUrls()
        {
            // Arrange
            var environmentManager = new DashboardEnvironmentManager();
            var env = EnvironmentEnum.QA;
            List<string> expectedServerNames = new List<string>
            {
                "vm-icon-qa1",
                "vm-icon-qa2",
                "vm-icon-qa3",
                "vm-icon-qa4",
                "mammoth-app01-qa",
            };

            // Act
            var actualSevers = environmentManager.GetDefaultAppServersForEnvironment(env);

            // Assert
            foreach (var expectedServer in expectedServerNames)
            {
                Assert.IsTrue(actualSevers.Contains(expectedServer), $"Expected to find '{expectedServer}'");
            }
        }

        [TestMethod]
        public void EnvironmentManager_GetDefaultAppServersForEnvironment_PERF_ShouldReturnsExpectedUrls()
        {
            // Arrange
            var environmentManager = new DashboardEnvironmentManager();
            var env = EnvironmentEnum.Perf;
            List<string> expectedServerNames = new List<string>
            {
                "vm-icon-test2",
                "mammoth-app01-qa",
            };

            // Act
            var actualSevers = environmentManager.GetDefaultAppServersForEnvironment(env);

            // Assert
            foreach (var expectedServer in expectedServerNames)
            {
                Assert.IsTrue(actualSevers.Contains(expectedServer));
            }
        }

        [TestMethod]
        public void EnvironmentManager_GetDefaultAppServersForEnvironment_PRD_ShouldReturnsExpectedUrls()
        {
            // Arrange
            var environmentManager = new DashboardEnvironmentManager();
            var env = EnvironmentEnum.Prd;
            List<string> expectedServerNames = new List<string>
            {
                "vm-icon-prd1",
                "vm-icon-prd2",
                "vm-icon-prd3",
                "vm-icon-prd4",
                "vm-icon-prd5",
                "vm-icon-prd6",
                "mammoth-app01-prd",
                "mammoth-app02-prd",
            };

            // Act
            var actualSevers = environmentManager.GetDefaultAppServersForEnvironment(env);

            // Assert
            foreach (var expectedServer in expectedServerNames)
            {
                Assert.IsTrue(actualSevers.Contains(expectedServer), $"Expected to find '{expectedServer}'");
            }
        }

        [TestMethod]
        public void EnvironmentManager_GetDefaultEnvironmentNameFromWebhost_WhenLocalhost_ShouldReturnDev()
        {
            // Arrange
            var environmentManager = new DashboardEnvironmentManager();
            var webhost = "localhost";
            var expectedEnvironmentName = "Dev";

            // Act
            var actualEnvironmentName = environmentManager.GetDefaultEnvironmentNameFromWebhost(webhost);

            // Assert
            Assert.AreEqual(expectedEnvironmentName, actualEnvironmentName);
        }

        [TestMethod]
        public void EnvironmentManager_GetDefaultEnvironmentNameFromWebhostWhenIrmadevapp1_ShouldReturnDev()
        {
            // Arrange
            var environmentManager = new DashboardEnvironmentManager();
            var webhost = "irmadevapp1";
            var expectedEnvironmentName = "Dev";

            // Act
            var actualEnvironmentName = environmentManager.GetDefaultEnvironmentNameFromWebhost(webhost);

            // Assert
            Assert.AreEqual(expectedEnvironmentName, actualEnvironmentName);
        }


        [TestMethod]
        public void EnvironmentManager_GetDefaultEnvironmentNameFromWebhost_WhenIrmatestapp1_ShouldReturnTest()
        {
            // Arrange
            var environmentManager = new DashboardEnvironmentManager();
            var webhost = "irmatestapp1";
            var expectedEnvironmentName = "Test";

            // Act
            var actualEnvironmentName = environmentManager.GetDefaultEnvironmentNameFromWebhost(webhost);

            // Assert
            Assert.AreEqual(expectedEnvironmentName, actualEnvironmentName);
        }

        [TestMethod]
        public void EnvironmentManager_GetDefaultEnvironmentNameFromWebhost_WhenIrmaqaapp1_ShouldReturnQa()
        {
            // Arrange
            var environmentManager = new DashboardEnvironmentManager();
            var webhost = "irmaqaapp1";
            var expectedEnvironmentName = "QA";

            // Act
            var actualEnvironmentName = environmentManager.GetDefaultEnvironmentNameFromWebhost(webhost);

            // Assert
            Assert.AreEqual(expectedEnvironmentName, actualEnvironmentName);
        }

        [TestMethod]
        public void EnvironmentManager_GetDefaultEnvironmentNameFromWebhost_WhenIrmaprdapp1_ShouldReturnPrd()
        {
            // Arrange
            var environmentManager = new DashboardEnvironmentManager();
            var webhost = "irmaprdapp1";
            var expectedEnvironmentName = "Prd";

            // Act
            var actualEnvironmentName = environmentManager.GetDefaultEnvironmentNameFromWebhost(webhost);

            // Assert
            Assert.AreEqual(expectedEnvironmentName, actualEnvironmentName);
        }

        private void AssertEqualDashboardEnvironmentViewModels(DashboardEnvironmentViewModel expected, DashboardEnvironmentViewModel actual)
        {
            Assert.AreEqual(expected.id, actual.id);
            Assert.AreEqual(expected.Name, actual.Name);
            if (expected.AppServers == null)
            {
                Assert.IsNull(actual.AppServers);
            }
            else
            {
                Assert.IsNotNull(actual.AppServers);
                Assert.AreEqual(expected.AppServers.Count, actual.AppServers.Count);
                foreach( var expectedAppServer in expected.AppServers)
                {
                    var actualAppServer = actual.AppServers.FirstOrDefault(server => server.ServerName.Equals(expectedAppServer.ServerName, StringComparison.InvariantCultureIgnoreCase));
                    Assert.IsNotNull(actualAppServer);
                    Assert.AreEqual(expectedAppServer.id, actualAppServer.id);
                    Assert.AreEqual(expectedAppServer.parentId, actualAppServer.parentId);
                    Assert.AreEqual(expectedAppServer.ServerName, actualAppServer.ServerName);
                }
            }
        }
    }
}
