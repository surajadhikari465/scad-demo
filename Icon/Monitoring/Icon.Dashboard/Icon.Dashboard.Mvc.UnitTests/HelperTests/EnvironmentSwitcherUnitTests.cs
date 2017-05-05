using FluentAssertions;
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
        public void EnvironmentSwitcher_WithDefinedAppSettings_ShouldReturnsExpectedCount()
        {
            // Arrange;
            var envSwitcher = new EnvironmentSwitcher();
            const int expectedServerCount = 4;
            // Act
            var servers = envSwitcher.GetServersForEnvironments();
            // Assert
            servers.Should().NotBeNull();
            servers.Count.Should().Be(expectedServerCount);
        }

        [TestMethod]
        public void EnvironmentSwitcher_ForDevEnvironment_ShouldReturnsExpectedServerData()
        {
            // Arrange;
            var envSwitcher = new EnvironmentSwitcher();
            var env = "DEV";
            var expectedServerName = $"irma{env}app1";
            // Act
            var servers = envSwitcher.GetServersForEnvironments();
            // Assert
            var serverForEnv = servers.First(s => 
                String.Compare(s.Key, env, StringComparison.CurrentCultureIgnoreCase) == 0);
            serverForEnv.Should().NotBeNull();
            serverForEnv.Value.ToLower().ShouldBeEquivalentTo(expectedServerName.ToLower());
        }

        [TestMethod]
        public void EnvironmentSwitcher_ForTestEnvironment_ShouldReturnsExpectedServerData()
        {
            // Arrange;
            var envSwitcher = new EnvironmentSwitcher();
            var env = "TEST";
            var expectedServerName = $"irma{env}app1";
            // Act
            var servers = envSwitcher.GetServersForEnvironments();
            // Assert
            var serverForEnv = servers.First(s =>
                String.Compare(s.Key, env, StringComparison.CurrentCultureIgnoreCase) == 0);
            serverForEnv.Should().NotBeNull();
            serverForEnv.Value.ToLower().ShouldBeEquivalentTo(expectedServerName.ToLower());
        }

        [TestMethod]
        public void EnvironmentSwitcher_ForQAEnvironment_ShouldReturnsExpectedServerData()
        {
            // Arrange;
            var envSwitcher = new EnvironmentSwitcher();
            var env = "QA";
            var expectedServerName = $"irma{env}app1";
            // Act
            var servers = envSwitcher.GetServersForEnvironments();
            // Assert
            var serverForEnv = servers.First(s =>
                String.Compare(s.Key, env, StringComparison.CurrentCultureIgnoreCase) == 0);
            serverForEnv.Should().NotBeNull();
            serverForEnv.Value.ToLower().ShouldBeEquivalentTo(expectedServerName.ToLower());
        }

        [TestMethod]
        public void EnvironmentSwitcher_ForPRODEnvironment_ShouldReturnsExpectedServerData()
        {
            // Arrange;
            var envSwitcher = new EnvironmentSwitcher();
            var env = "PROD";
            var expectedServerName = $"{env}appserver";
            // Act
            var servers = envSwitcher.GetServersForEnvironments();
            // Assert
            var serverForEnv = servers.First(s =>
                String.Compare(s.Key, env, StringComparison.CurrentCultureIgnoreCase) == 0);
            serverForEnv.Should().NotBeNull();
            serverForEnv.Value.ToLower().ShouldBeEquivalentTo(expectedServerName.ToLower());
        }
    }
}
