using FluentAssertions;
using Icon.Dashboard.DataFileAccess.Models;
using Icon.Dashboard.Mvc.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.Mvc.UnitTests.HelperTests
{
    [TestClass]
    public class EnvironmentSwitcherUnitTests
    {
        [TestMethod]
        public void EnvironmentSwitcher_GetWebServersForEnvironments_WithDefinedAppSettings_ShouldReturnsExpectedCount()
        {
            // Arrange;
            var envSwitcher = new EnvironmentSwitcher();
            const int expectedServerCount = 3;
            // Act
            var servers = envSwitcher.GetWebServersForEnvironments();
            // Assert
            servers.Should().NotBeNull();
            servers.Count.Should().Be(expectedServerCount);
        }

        [TestMethod]
        public void EnvironmentSwitcher_GetWebServerForEnvironment_DEV_ShouldReturnsExpectedServerData()
        {
            // Arrange;
            var envSwitcher = new EnvironmentSwitcher();
            var env = EnvironmentEnum.Dev;
            var expectedServerName = $"irma{env}app1";
            // Act
            var serverForEnv = envSwitcher.GetWebServerForEnvironment(env);
            // Assert
            serverForEnv.Should().NotBeNull();
            serverForEnv.Value.ToLower().Should().BeEquivalentTo(expectedServerName.ToLower());
        }

        [TestMethod]
        public void EnvironmentSwitcher_GetWebServerForEnvironment_TST_ShouldReturnsExpectedServerData()
        {
            // Arrange;
            var envSwitcher = new EnvironmentSwitcher();
            var env = EnvironmentEnum.Test;
            var expectedServerName = $"irma{env}app1";
            // Act
            var serverForEnv = envSwitcher.GetWebServerForEnvironment(env);
            // Assert
            serverForEnv.Should().NotBeNull();
            serverForEnv.Value.ToLower().Should().BeEquivalentTo(expectedServerName.ToLower());
        }

        [TestMethod]
        public void EnvironmentSwitcher_GetWebServerForEnvironment_QA_ShouldReturnsExpectedServerData()
        {
            // Arrange;
            var envSwitcher = new EnvironmentSwitcher();
            var env = EnvironmentEnum.QA;
            var expectedServerName = $"irma{env}app1";
            // Act
            var serverForEnv = envSwitcher.GetWebServerForEnvironment(env);
            // Assert
            serverForEnv.Should().NotBeNull();
            serverForEnv.Value.ToLower().Should().BeEquivalentTo(expectedServerName.ToLower());
        }

        //[TestMethod]
        //public void EnvironmentSwitcher_GetWebServerForEnvironment_Perf_ShouldReturnsExpectedServerData()
        //{
        //    // Arrange;
        //    var envSwitcher = new EnvironmentSwitcher();
        //    var env = EnvironmentEnum.Perf;
        //    var expectedServerName = $"irmatestapp1";
        //    // Act
        //    var serverForEnv = envSwitcher.GetWebServerForEnvironment(env);
        //    // Assert
        //    serverForEnv.Should().NotBeNull();
        //    serverForEnv.Value.ToLower().Should().BeEquivalentTo(expectedServerName.ToLower());
        //}

        //[TestMethod]
        //public void EnvironmentSwitcher_GetWebServerForEnvironment_PRD_ShouldReturnsExpectedServerData()
        //{
        //    // Arrange;
        //    var envSwitcher = new EnvironmentSwitcher();
        //    var env = EnvironmentEnum.Prd;
        //    var expectedServerName = $"irma{env}app1";
        //    // Act
        //    var serverForEnv = envSwitcher.GetWebServerForEnvironment(env);
        //    // Assert
        //    serverForEnv.Should().NotBeNull();
        //    serverForEnv.Value.ToLower().Should().BeEquivalentTo(expectedServerName.ToLower());
        //}

        [TestMethod]
        public void EnvironmentSwitcher_GetMammothWebSupportServerForEnvironment_ShouldReturnsExpectedUrl()
        {
            // Arrange;
            var envSwitcher = new EnvironmentSwitcher();
            var env = EnvironmentEnum.Test.ToString();
            var expectedServerName = $"http://irmatestapp1/MammothWebSupport";
            // Act
            var serverUrl = envSwitcher.GetMammothWebSupportServerForEnivronment(env);
            // Assert
            serverUrl.Should().NotBeNull();
            //appServerKvp.Key.Should().BeEquivalentTo(env.ToString());
            Assert.AreEqual(expectedServerName, serverUrl, true);
        }

        [TestMethod]
        public void EnvironmentSwitcher_GetTibcoServerForEnvironment_ShouldReturnsExpectedUrl()
        {
            // Arrange;
            var envSwitcher = new EnvironmentSwitcher();
            var env = EnvironmentEnum.Test.ToString();
            var expectedServerName = @"https://cerd1669:1809";
            // Act
            var serverUrl = envSwitcher.GetTibcoAdminServerForEnivronment(env);
            // Assert
            serverUrl.Should().NotBeNull();
            //appServerKvp.Key.Should().BeEquivalentTo(env.ToString());
            Assert.AreEqual(expectedServerName, serverUrl, true);
        }

        [TestMethod]
        public void EnvironmentSwitcher_GetIconWebServerForEnvironment_ShouldReturnsExpectedUrl()
        {
            // Arrange;
            var envSwitcher = new EnvironmentSwitcher();
            var env = EnvironmentEnum.Test.ToString();
            var expectedServerName = $"http://icon-{env.ToString()}/";
            // Act
            var serverUrl = envSwitcher.GetIconWebServerForEnvironment(env);
            // Assert
            serverUrl.Should().NotBeNull();
            //appServerKvp.Key.Should().BeEquivalentTo(env.ToString());
            Assert.AreEqual(expectedServerName, serverUrl, true); ;
        }

        [TestMethod]
        public void EnvironmentSwitcher_GetDefaultIconServersForEnvironment_DEV_ShouldReturnsExpectedUrls()
        {
            // Arrange;
            var envSwitcher = new EnvironmentSwitcher();
            var env = EnvironmentEnum.Dev;
            List<string> expectedServerNames = new List<string>
            {
                "vm-icon-dev1"
            };

            // Act
            var actualSevers = envSwitcher.GetDefaultIconServersForEnvironment(env);

            // Assert
            foreach ( var expectedServer in expectedServerNames)
            {
                Assert.IsTrue(actualSevers.Contains(expectedServer));
            }
        }

        [TestMethod]
        public void EnvironmentSwitcher_GetDefaultIconServersForEnvironment_TST_ShouldReturnsExpectedUrls()
        {
            // Arrange;
            var envSwitcher = new EnvironmentSwitcher();
            var env = EnvironmentEnum.Test;
            List<string> expectedServerNames = new List<string>
            {
                "vm-icon-test1",
                "vm-icon-test2"
            };

            // Act
            var actualSevers = envSwitcher.GetDefaultIconServersForEnvironment(env);

            // Assert
            foreach (var expectedServer in expectedServerNames)
            {
                Assert.IsTrue(actualSevers.Contains(expectedServer));
            }
        }

        [TestMethod]
        public void EnvironmentSwitcher_GetDefaultIconServersForEnvironment_QA_ShouldReturnsExpectedUrls()
        {
            // Arrange;
            var envSwitcher = new EnvironmentSwitcher();
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
            var actualSevers = envSwitcher.GetDefaultIconServersForEnvironment(env);

            // Assert
            foreach (var expectedServer in expectedServerNames)
            {
                Assert.IsTrue(actualSevers.Contains(expectedServer));
            }
        }

        //[TestMethod]
        //public void EnvironmentSwitcher_GetDefaultIconServersForEnvironment_PERF_ShouldReturnsExpectedUrls()
        //{
        //    // Arrange;
        //    var envSwitcher = new EnvironmentSwitcher();
        //    var env = EnvironmentEnum.Perf;
        //    List<string> expectedServerNames = new List<string>
        //    {
        //        "vm-icon-test2",
        //        "mammoth-app01-qa",
        //    };

        //    // Act
        //    var actualSevers = envSwitcher.GetDefaultIconServersForEnvironment(env);

        //    // Assert
        //    foreach (var expectedServer in expectedServerNames)
        //    {
        //        Assert.IsTrue(actualSevers.Contains(expectedServer));
        //    }
        //}

        //[TestMethod]
        //public void EnvironmentSwitcher_GetDefaultIconServersForEnvironment_PRD_ShouldReturnsExpectedUrls()
        //{
        //    // Arrange;
        //    var envSwitcher = new EnvironmentSwitcher();
        //    var env = EnvironmentEnum.Prd;
        //    List<string> expectedServerNames = new List<string>
        //    {
        //        "vm-icon-prd1",
        //        "vm-icon-prd2",
        //        "vm-icon-prd3",
        //        "vm-icon-prd4",
        //        "vm-icon-prd5",
        //        "vm-icon-prd6",
        //        "mammoth-app01-prd",
        //        "mammoth-app02-prd",
        //    };

        //    // Act
        //    var actualSevers = envSwitcher.GetDefaultIconServersForEnvironment(env);

        //    // Assert
        //    foreach (var expectedServer in expectedServerNames)
        //    {
        //        Assert.IsTrue(actualSevers.Contains(expectedServer));
        //    }
        //}
    }
}
