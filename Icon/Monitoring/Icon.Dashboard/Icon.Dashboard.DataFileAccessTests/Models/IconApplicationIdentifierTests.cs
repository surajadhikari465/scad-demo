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
            const string configPath = @"\\vvv-xxxx-11\e$\Stuff\My Thingy_NA\Argle.Bargle.Controller.exe.config";
            const string displayName = "Apppy App";

            // When
            var app = new IconService(appName, server, configPath, displayName);

            // Then
            Assert.AreEqual(appName, app.Name);
            Assert.AreEqual(server, app.Server);
            Assert.AreEqual(configPath, app.ConfigFilePath);
            Assert.AreEqual(displayName, app.DisplayName);
        }
    }
}