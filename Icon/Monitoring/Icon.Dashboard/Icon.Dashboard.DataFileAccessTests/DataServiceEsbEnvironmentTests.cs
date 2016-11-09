namespace Icon.Dashboard.DataFileAccess.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using DataFileAccess.Models;
    using DataFileAccess.Services;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    [TestClass]
    public class DataServiceEsbEnvironmentTests
    {
        private string configPath = "IconApplication.xml";

        #region helpers
        protected EsbEnvironment GetValidObjectToTest()
        {
            var esbEnvironment = new EsbEnvironment
            {
                Name = "NAMEYNAME",
                ServerUrl = "asldka2.saodif.com",
                TargetHostName = "OLEBESSIE",
                JmsUsername = "jmsUserAAA",
                JmsPassword = "$)($292e^&",
                JndiUsername = "jndiUser1@dsapofi.org",
                JndiPassword = "!09/2PO7oo",
                Applications = new List<IconApplicationIdentifier>()
            };
            return esbEnvironment;
        }

        protected void AssertObjectPropertiesMatch(Dictionary<string, string> expected, IEsbEnvironment actual)
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

        protected void AssertObjectPropertiesMatch(IEsbEnvironment expected, IEsbEnvironment actual)
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

        protected void AssertEsbEnvironmentApplicationsProperty(IEsbEnvironment esbEnvironment, int? expectedNumberOfAplications)
        {
            if (expectedNumberOfAplications.HasValue)
            {
                Assert.IsNotNull(esbEnvironment.Applications);
                Assert.AreEqual(expectedNumberOfAplications.Value, esbEnvironment.Applications.Count());
            }
            else
            {
                Assert.IsNull(esbEnvironment.Applications);
            }
        }
        #endregion

        [TestCleanup]
        public void Cleanup()
        {
            List<string> environmentNamesToCheck = new List<string>() { "ADDED", "deleteMe", "Delete After Updating" };
            foreach (var name in environmentNamesToCheck)
            {
                var addedDuringTest = IconDashboardDataService.Instance.GetEsbEnvironment(configPath, name);
                if (addedDuringTest != null)
                {
                    IconDashboardDataService.Instance.DeleteEsbEnvironment(addedDuringTest, this.configPath);
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
                { nameof(EsbEnvironment.Name), "ENV_A" },
                { nameof(EsbEnvironment.ServerUrl), "ssl://cerd1617.wfm.pvt:17293" },
                { nameof(EsbEnvironment.TargetHostName), "cerd1617.wfm.pvt" },
                { nameof(EsbEnvironment.JmsUsername), "jmsUser1" },
                { nameof(EsbEnvironment.JmsPassword), "secret1234" },
                { nameof(EsbEnvironment.JndiUsername), "jndiUserX" },
                { nameof(EsbEnvironment.JndiPassword), "topSecret99" }
            };
            int expectedApplicationCount = 2;

            // When
            var actual = IconDashboardDataService.Instance.GetEsbEnvironment(configPath, "ENV_A");

            // Then
            AssertObjectPropertiesMatch(expectedValues, actual);
            AssertEsbEnvironmentApplicationsProperty(actual, expectedApplicationCount);
        }

        [TestMethod]
        public void WhenExistingEsbEnvironmentInDataFile_ThenAddEsbEnvironment_ShouldReturnWithNewEsbEnvironment()
        {
            // Given
            var toAdd = new EsbEnvironment
            {
                Name = "ADDED",
                ServerUrl = "myServer",
                TargetHostName = "myHost",
                JmsUsername = "user1",
                JmsPassword = "p@ssw0rd1",
                JndiUsername = "userA",
                JndiPassword = "p@ssw0rd2",
                Applications = new List<IconApplicationIdentifier>()
                {
                    new IconApplicationIdentifier("Mammoth.Price.Controller", "vm-icon-test1")
                }
            };

            // When
            IconDashboardDataService.Instance.AddEsbEnvironment(toAdd, this.configPath);

            // Then
            var retrieved = IconDashboardDataService.Instance.GetEsbEnvironment(this.configPath, toAdd.Name);
            AssertObjectPropertiesMatch(toAdd, retrieved);
            AssertEsbEnvironmentApplicationsProperty(retrieved, 1);
            //cleanup
            IconDashboardDataService.Instance.DeleteEsbEnvironment(retrieved, this.configPath);
        }

        [TestMethod]
        public void WhenConfigHasEsbEnvironment_ThenDeleteEsbEnvironment_ShouldReturnWithoutEsbEnvironment()
        {
            // Given  
            const string expectedName = "deleteMe";
            var addToDelete = new EsbEnvironment
            {
                Name = expectedName,
                ServerUrl = "esbServer",
                TargetHostName = "esbHost",
                JmsUsername = "xxx",
                JmsPassword = "123",
                JndiUsername = "yyy",
                JndiPassword = "@666",
                Applications = new List<IconApplicationIdentifier>()
                {
                    new IconApplicationIdentifier("Mammoth.Price.Controller", "vm-icon-test1")
                }
            };
            IconDashboardDataService.Instance.AddEsbEnvironment(addToDelete, this.configPath);

            var toDelete = new EsbEnvironment
            {
                Name = expectedName,
                ServerUrl = "esbServer",
                TargetHostName = "esbHost",
                JmsUsername = "xxx",
                JmsPassword = "123",
                JndiUsername = "yyy",
                JndiPassword = "@666",
                Applications = new List<IconApplicationIdentifier>()
                {
                    new IconApplicationIdentifier("Mammoth.Price.Controller", "vm-icon-test1")
                }
            };

            // When
            IconDashboardDataService.Instance.DeleteEsbEnvironment(toDelete, this.configPath);

            // Then
            var envs = IconDashboardDataService.Instance.GetEsbEnvironments(this.configPath)
                .Where(a => a.Name == expectedName);
            Assert.IsFalse(envs.Any());
        }

        [TestMethod]
        public void WhenConfigHasEsbEnvironment_ThenUpdateEsbEnvironment_ShouldReturnWithUpdates()
        {
            // Given  
            const string expectedName = "Delete After Updating";
            var addForUpdating = new EsbEnvironment
            {
                Name = expectedName,
                ServerUrl = "my_server",
                TargetHostName = "my_host",
                JmsUsername = "aaaaaa",
                JmsPassword = "123",
                JndiUsername = "bbbbbb",
                JndiPassword = "@p@ssw0rd",
                Applications = new List<IconApplicationIdentifier>()
                {
                    new IconApplicationIdentifier("Mammoth.Price.Controller", "vm-icon-test1")
                }
            };
            IconDashboardDataService.Instance.AddEsbEnvironment(addForUpdating, this.configPath);

            var toUpdate = new EsbEnvironment
            {
                Name = expectedName,
                ServerUrl = "a_Nother_server",
                TargetHostName = "bad_host",
                JmsUsername = "jmsguy",
                JmsPassword = "secret1234",
                JndiUsername = "jndiguy",
                JndiPassword = "soupOrSecret",
                Applications = new List<IconApplicationIdentifier>()
                {
                    new IconApplicationIdentifier("Mammoth.Price.Controller", "vm-icon-test1")
                }
            };

            // When
            IconDashboardDataService.Instance.UpdateEsbEnvironment(toUpdate, this.configPath);

            var updated = IconDashboardDataService.Instance.GetEsbEnvironment(this.configPath, expectedName);

            // Then
            AssertObjectPropertiesMatch(toUpdate, updated);
            AssertEsbEnvironmentApplicationsProperty(updated, 1);
            //cleanup
            IconDashboardDataService.Instance.DeleteEsbEnvironment(updated, this.configPath);
        }

        [TestMethod]
        public void WhenUsingTestDataFile_GetCurrentEsbEnvironment_GetsExpectedEnvironment()
        {
            // Given 
            // ...existing canned data file with an esb environment defined & application app.config(s) to match
            // When
            var currentEnvironment = IconDashboardDataService.Instance.GetCurrentEsbEnvironment(this.configPath);
            // Then
            Assert.IsNotNull(currentEnvironment);
            Assert.AreEqual("ENV_A", currentEnvironment.Name);
        }

        [TestMethod]
        public void WhenUsingDataFileWithNoEsbEnvironmentsDefined_GetCurrentEsbEnvironment_ReturnsNull()
        {
            // Given 
            // data file with no entries for anything esb-environment related
            var oldDataFile = "OldStyleDataFile.xml";
            // When
            var currentEnvironment = IconDashboardDataService.Instance.GetCurrentEsbEnvironment(oldDataFile);
            // Then
            Assert.IsNull(currentEnvironment);
        }
    }
}
