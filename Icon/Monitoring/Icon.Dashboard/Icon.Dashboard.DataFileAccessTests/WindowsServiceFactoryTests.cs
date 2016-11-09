namespace Icon.Dashboard.DataFileAccess.Tests
{
    using DataFileAccess.Constants;
    using DataFileAccess.Models;
    using DataFileAccess.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;
    using System.Xml.Linq;

    [TestClass]
    public class WindowsServiceFactoryTests
    {
        private XDocument config;
        private WindowsServiceFactory testFactory;

        [TestInitialize]
        public void Initialize()
        {
            this.config = XDocument.Load("IconApplication.xml");
            this.testFactory = new WindowsServiceFactory();
        }

        [TestMethod]
        public void WhenValidConfig_ThenGetApplication_ShouldReturnValidApplication()
        {
            // Given
            var firstApplicationElement = this.config.Root.Element(ApplicationSchema.Applications).Elements().First();

            // When
            var app = this.testFactory.GetApplication(firstApplicationElement);

            // Then
            Assert.AreEqual("Mammoth.Price.Controller", app.Name);
            Assert.AreEqual("\\\\vm-icon-test1\\e$\\Mammoth\\Price Controller\\Mammoth.Price.Controller.exe.config", app.ConfigFilePath);
            Assert.AreEqual("Mammoth Price Controller", app.DisplayName);
            Assert.AreEqual(EnvironmentEnum.Test, app.Environment);
            Assert.AreEqual("vm-icon-test1", app.Server);
            Assert.AreEqual(ApplicationTypeEnum.WindowsService, app.TypeOfApplication);
            Assert.AreEqual(DataFlowSystemEnum.Icon, app.DataFlowFrom);
            Assert.AreEqual(DataFlowSystemEnum.None, app.DataFlowTo);
        }
    }
}