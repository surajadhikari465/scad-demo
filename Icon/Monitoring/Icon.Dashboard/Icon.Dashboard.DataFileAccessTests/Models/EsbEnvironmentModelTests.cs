namespace Icon.Dashboard.DataFileAccess.Tests.Models
{
    using DataFileAccess.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    [TestClass]
    public class EsbEnvironmentModelTests
    {
        [TestMethod]
        public void WhenValidEsbEnvironmentModel_ThenCallAddApplication_ShouReturnApplicationIdentifier()
        {
            // Given
            const string name = "icon.App.Name.1";
            const string server = "vvv-xxxx-11";
            var environment = new EsbEnvironment();

            // When
            var appIdentifier = environment.AddApplication(name, server);

            // Then
            Assert.IsNotNull(appIdentifier);
            Assert.AreEqual(name, appIdentifier.Name);
            Assert.AreEqual(server, appIdentifier.Server);
        }

        [TestMethod]
        public void WhenNoApplicationsInEsbEnvironmentModel_ThenCallAddApplication_ShouldInitAndAddToApplications()
        {
            // Given
            const string name = "icon.App.Name.1";
            const string server = "vvv-xxxx-11";
            var environment = new EsbEnvironment();

            // When
            var appIdentifier = environment.AddApplication(name, server);

            // Then
            Assert.IsNotNull(environment.Applications);
            Assert.IsNotNull(environment.Applications[0]);
            Assert.AreEqual(name, environment.Applications[0].Name);
            Assert.AreEqual(server, environment.Applications[0].Server);
        }

        [TestMethod]
        public void WhenExisitngApplicationInEsbEnvironmentModel_ThenCallAddApplication_ShouldAddToApplications()
        {
            // Given
            const string name1 = "icon.App.Name.1";
            const string server1 = "vvv-xxxx-11";
            const string name2 = "icon.App.Name.2";
            const string server2 = "vvv-yyyy-22";
            var environment = new EsbEnvironment()
            {
                Applications = new List<IconApplicationIdentifier>()
                {
                    new IconApplicationIdentifier(name1, server1)
                }
            };

            // When
            var appIdentifier = environment.AddApplication(name2, server2);

            // Then
            Assert.IsNotNull(environment.Applications);
            Assert.AreEqual(2, environment.Applications.Count);
            Assert.IsNotNull(environment.Applications[0]);
            Assert.AreEqual(name1, environment.Applications[0].Name);
            Assert.AreEqual(server1, environment.Applications[0].Server);
            Assert.IsNotNull(environment.Applications[1]);
            Assert.AreEqual(name2, environment.Applications[1].Name);
            Assert.AreEqual(server2, environment.Applications[1].Server);
        }
    }
}
