namespace Icon.Dashboard.DataFileAccess.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using DataFileAccess.Models;
    using DataFileAccess.Services;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using FluentAssertions;

    [TestClass]
    public class DataServiceEsbEnvironmentTests
    {
        private string dataFilePath = "IconApplication.xml";
        protected const string appName = "Mammoth.Price.Controller";
        protected const string appServer = "vm-icon-testX";
        protected const string appConfigPath = @"\\vm-icon-testX\e$\Mammoth\Price Controller_NA\Mammoth.Price.Controller.exe.config";
        protected const string appDisplayName = "Mammoth Price Controller";

        #region helpers
        protected EsbEnvironmentDefinition GetValidObjectToTest()
        {
            var esbEnvironment = new EsbEnvironmentDefinition
            {
                Name = "NAMEYNAME",
                ServerUrl = "asldka2.saodif.com",
                TargetHostName = "OLEBESSIE",
                JmsUsername = "jmsUserAAA",
                JmsPassword = "$)($292e^&",
                JndiUsername = "jndiUser1@dsapofi.org",
                JndiPassword = "!09/2PO7oo",
            };
            return esbEnvironment;
        }

        protected void AssertObjectPropertiesMatch(Dictionary<string, string> expected, IEsbEnvironmentDefinition actual)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected[nameof(actual.Name)], actual.Name);
            Assert.AreEqual(expected[nameof(actual.ServerUrl)], actual.ServerUrl);
            Assert.AreEqual(expected[nameof(actual.TargetHostName)], actual.TargetHostName);
            Assert.AreEqual(expected[nameof(actual.JmsUsername)], actual.JmsUsername);
            Assert.AreEqual(expected[nameof(actual.JmsPassword)], actual.JmsPassword);
            Assert.AreEqual(expected[nameof(actual.JndiUsername)], actual.JndiUsername);
            Assert.AreEqual(expected[nameof(actual.JndiPassword)], actual.JndiPassword);
        }

        protected void AssertObjectPropertiesMatch(IEsbEnvironmentDefinition expected, IEsbEnvironmentDefinition actual)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.ServerUrl, actual.ServerUrl);
            Assert.AreEqual(expected.TargetHostName, actual.TargetHostName);
            Assert.AreEqual(expected.JmsUsername, actual.JmsUsername);
            Assert.AreEqual(expected.JmsPassword, actual.JmsPassword);
            Assert.AreEqual(expected.JndiUsername, actual.JndiUsername);
            Assert.AreEqual(expected.JndiPassword, actual.JndiPassword);
        }
        #endregion

        [TestCleanup]
        public void Cleanup()
        {
            List<string> environmentNamesToCheck = new List<string>() { "ADDED", "deleteMe", "Delete After Updating" };
            foreach (var name in environmentNamesToCheck)
            {
                var addedDuringTest = IconDashboardDataService.Instance.GetEsbEnvironment(dataFilePath, name);
                if (addedDuringTest != null)
                {
                    IconDashboardDataService.Instance.DeleteEsbEnvironment(addedDuringTest, this.dataFilePath);
                }
            }
        }

        [TestMethod]
        public void WhenDataServiceCreated_ThenInstance_ShouldHaveEsbEnvironmentFactories()
        {
            // Then
            Assert.IsTrue(IconDashboardDataService.Instance.EsbEnvironmentFactories.Any());
        }

        [TestMethod]
        public void WhenExistingEsbEnvironmentInDataFile_ThenGetEsbEnvironment_ShouldReturnEsbEnvironment()
        {
            // Given
            // assume we already have data in the data (config) file with data as follows
            var expectedValues = new Dictionary<string, string>()
            {
                { nameof(EsbEnvironmentDefinition.Name), "ENV_A" },
                { nameof(EsbEnvironmentDefinition.ServerUrl), "ssl://cerd1617.wfm.pvt:17293" },
                { nameof(EsbEnvironmentDefinition.TargetHostName), "cerd1617.wfm.pvt" },
                { nameof(EsbEnvironmentDefinition.JmsUsername), "jmsUser1" },
                { nameof(EsbEnvironmentDefinition.JmsPassword), "secret1234" },
                { nameof(EsbEnvironmentDefinition.JndiUsername), "jndiUserX" },
                { nameof(EsbEnvironmentDefinition.JndiPassword), "topSecret99" }
            };

            // When
            var actual = IconDashboardDataService.Instance.GetEsbEnvironment(dataFilePath, "ENV_A");

            // Then
            AssertObjectPropertiesMatch(expectedValues, actual);
        }

        [TestMethod]
        public void WhenExistingEsbEnvironmentInDataFile_ThenAddEsbEnvironment_ShouldReturnWithNewEsbEnvironment()
        {
            // Given
            var toAdd = new EsbEnvironmentDefinition
            {
                Name = "ADDED",
                ServerUrl = "myServer",
                TargetHostName = "myHost",
                JmsUsername = "user1",
                JmsPassword = "p@ssw0rd1",
                JndiUsername = "userA",
                JndiPassword = "p@ssw0rd2",
            };

            // When
            IconDashboardDataService.Instance.AddEsbEnvironment(toAdd, this.dataFilePath);

            // Then
            var retrieved = IconDashboardDataService.Instance.GetEsbEnvironment(this.dataFilePath, toAdd.Name);
            AssertObjectPropertiesMatch(toAdd, retrieved);
            //cleanup
            IconDashboardDataService.Instance.DeleteEsbEnvironment(retrieved, this.dataFilePath);
        }

        [TestMethod]
        public void WhenConfigHasEsbEnvironment_ThenDeleteEsbEnvironment_ShouldReturnWithoutEsbEnvironment()
        {
            // Given  
            const string expectedName = "deleteMe";
            var addToDelete = new EsbEnvironmentDefinition
            {
                Name = expectedName,
                ServerUrl = "esbServer",
                TargetHostName = "esbHost",
                JmsUsername = "xxx",
                JmsPassword = "123",
                JndiUsername = "yyy",
                JndiPassword = "@666",
            };
            IconDashboardDataService.Instance.AddEsbEnvironment(addToDelete, this.dataFilePath);

            var toDelete = new EsbEnvironmentDefinition
            {
                Name = expectedName,
                ServerUrl = "esbServer",
                TargetHostName = "esbHost",
                JmsUsername = "xxx",
                JmsPassword = "123",
                JndiUsername = "yyy",
                JndiPassword = "@666",
            };

            // When
            IconDashboardDataService.Instance.DeleteEsbEnvironment(toDelete, this.dataFilePath);

            // Then
            var envs = IconDashboardDataService.Instance.GetEsbEnvironments(this.dataFilePath)
                .Where(a => a.Name == expectedName);
            Assert.IsFalse(envs.Any());
        }

        [TestMethod]
        public void WhenConfigHasEsbEnvironment_ThenUpdateEsbEnvironment_ShouldReturnWithUpdates()
        {
            // Given  
            const string expectedName = "Delete After Updating";
            var addForUpdating = new EsbEnvironmentDefinition
            {
                Name = expectedName,
                ServerUrl = "my_server",
                TargetHostName = "my_host",
                JmsUsername = "aaaaaa",
                JmsPassword = "123",
                JndiUsername = "bbbbbb",
                JndiPassword = "@p@ssw0rd",
            };
            IconDashboardDataService.Instance.AddEsbEnvironment(addForUpdating, this.dataFilePath);

            var toUpdate = new EsbEnvironmentDefinition
            {
                Name = expectedName,
                ServerUrl = "a_Nother_server",
                TargetHostName = "bad_host",
                JmsUsername = "jmsguy",
                JmsPassword = "secret1234",
                JndiUsername = "jndiguy",
                JndiPassword = "soupOrSecret",
            };

            // When
            IconDashboardDataService.Instance.UpdateEsbEnvironment(toUpdate, this.dataFilePath);

            var updated = IconDashboardDataService.Instance.GetEsbEnvironment(this.dataFilePath, expectedName);

            // Then
            AssertObjectPropertiesMatch(toUpdate, updated);
            //cleanup
            IconDashboardDataService.Instance.DeleteEsbEnvironment(updated, this.dataFilePath);
        }

        [TestMethod]
        public void WhenUsingValidDataFile_GetEsbEnvironments_ShouldRetrieveExpectedEsbEnvironmentCount()
        {
            // Given 
            var dataFilePath = "IconApplication.xml";
            const int expectedCount = 3;
            // When
            var environments = IconDashboardDataService.Instance.GetEsbEnvironments(dataFilePath);
            // Then
            environments.Should().NotBeNull();
            environments.Count().Should().Be(expectedCount);
        }

        [TestMethod]
        public void WhenUsingValidDataFile_GetEsbEnvironments_ShouldRetrieveEnvironmentB_WithExpectedFields()
        {
            // Given 
            var dataFilePath = "IconApplication.xml";
            var expectedServerUrl = @"ssl://cerd1617.wfm.pvt:17293";
            var expectedNumberOfListenerThreads = 1;
            // When
            var ees = IconDashboardDataService.Instance.GetEsbEnvironments(dataFilePath);
            // Then
            var environmentB = ees.First(e => e.Name == "ENV_B");
            environmentB.Should().NotBeNull();
            environmentB.ServerUrl.Should().BeEquivalentTo(expectedServerUrl);
            environmentB.NumberOfListenerThreads.Should().Be(expectedNumberOfListenerThreads);
        }
        
    }
}
