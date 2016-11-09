namespace Icon.Dashboard.DataFileAccess.Tests.Models
{
    using DataFileAccess.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using System.Linq;
    using System.Xml.Linq;

    [TestClass]
    public class IconApplicationIdentifierTests
    {

        [TestMethod]
        public void WhenConstructingIconApplicationIdentifier_WithParameters_ShouldSetProperties()
        {
            // Given
            const string appName = "icon.App.Name.1";
            const string server = "vvv-xxxx-11";

            // When
            var appIdentifier = new IconApplicationIdentifier(appName, server);

            // Then
            Assert.AreEqual(appName, appIdentifier.Name);
            Assert.AreEqual(server, appIdentifier.Server);
        }
    }
}