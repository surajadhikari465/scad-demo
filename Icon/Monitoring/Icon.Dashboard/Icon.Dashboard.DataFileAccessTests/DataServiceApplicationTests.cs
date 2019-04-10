namespace Icon.Dashboard.DataFileAccess.Tests
{
    using DataFileAccess.Constants;
    using DataFileAccess.Models;
    using DataFileAccess.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;

    [TestClass]
    public class DataServiceApplicationTests
    {
        private string configPath = "IconApplication.xml";
        private string xsdPath = @"../../../Icon.Dashboard.DataFileAccess/Schema/Applications.xsd";

        [TestMethod]
        public void WhenDataServiceCreated_ThenInstance_ShouldHaveFactories()
        {
            // Then
            Assert.IsTrue(IconDashboardDataService.Instance.ApplicationFactories.Any());
        }

        [TestMethod]
        public void WhenConfigHasWindowsService_ThenGetApplications_ShouldReturnWindowService()
        {
            // When
            var applications = IconDashboardDataService.Instance.GetApplications(configPath);
            var app = IconDashboardDataService.Instance.GetApplication(configPath, "Mammoth.Price.Controller", "vm-icon-test1");

            // Then
            Assert.AreEqual("Mammoth.Price.Controller", app.Name);
            Assert.AreEqual("\\\\vm-icon-test1\\e$\\Mammoth\\Price Controller\\Mammoth.Price.Controller.exe.config", app.ConfigFilePath);
            Assert.AreEqual("Mammoth Price Controller", app.DisplayName);
            Assert.AreEqual("vm-icon-test1", app.Server);
            //Assert.AreEqual(ApplicationTypeEnum.WindowsService, app.TypeOfApplication);
        }

        [TestMethod]
        public void WhenNewValidWindowsService_ThenAddApplication_ShouldReturnWithNewApplication()
        {
            // Given
            var winService = new IconService
            {
                Name = "Mammoth.ItemLocale.Controller",
                ConfigFilePath = "\\\\vm-icon-test1\\e$\\Mammoth\\Item Locale Controller\\Mammoth.ItemLocale.Controller.exe.config",
                DisplayName = "Mammoth ItemLocale Controller",
                Server = "vm-icon-test1",
            };

            winService.FindAndCreateInstance();

            // When
            IconDashboardDataService.Instance.AddApplication(winService, this.configPath);
            var app = IconDashboardDataService.Instance.GetApplication(configPath, winService.Name, winService.Server);

            // Then
            Assert.AreEqual("Mammoth.ItemLocale.Controller", app.Name);
            Assert.AreEqual("\\\\vm-icon-test1\\e$\\Mammoth\\Item Locale Controller\\Mammoth.ItemLocale.Controller.exe.config", app.ConfigFilePath);
            Assert.AreEqual("Mammoth ItemLocale Controller", app.DisplayName);
            Assert.AreEqual("vm-icon-test1", app.Server);
            //Assert.AreEqual(ApplicationTypeEnum.WindowsService, app.TypeOfApplication);
        }

        [TestMethod]
        public void WhenConfigHasWindowsService_ThenDeleteApplication_ShouldReturnWithoutApplication()
        {
            // Given
            var winService = new IconService
            {
                Name = "Mammoth.Delete.Controller",
                ConfigFilePath = "\\\\vm-icon-test1\\e$\\Mammoth\\Item Locale Controller\\Mammoth.ItemLocale.Controller.exe.config",
                DisplayName = "Mammoth ItemLocale Controller",
                Server = "vm-icon-test1",
            };

            IconDashboardDataService.Instance.AddApplication(winService, this.configPath);

            // When
            IconDashboardDataService.Instance.DeleteApplication(winService, this.configPath);
            var apps = IconDashboardDataService.Instance.GetApplications(this.configPath)
                .Where(a => a.Name == "Mammoth.Delete.Controller" && a.Server == "vm-icon-test1");

            // Then
            Assert.IsFalse(apps.Any());
        }

        [TestMethod]
        public void WhenConfigHasWindowsService_ThenUpdateApplication_ShouldReturnWithUpdates()
        {
            // Given
            var winService = new IconService
            {
                Name = "Mammoth.Update.Controller",
                ConfigFilePath = "\\\\vm-icon-test1\\e$\\Mammoth\\Item Locale Controller\\Mammoth.ItemLocale.Controller.exe.config",
                DisplayName = "Mammoth Update Controller",
                Server = "vm-icon-test1",
            };

            IconDashboardDataService.Instance.AddApplication(winService, this.configPath);

            var updateService = new IconService
            {
                Name = "Mammoth.Update.Controller",
                ConfigFilePath = "update.config",
                DisplayName = "update name",
                Server = "vm-icon-test1",
            };

            // When
            IconDashboardDataService.Instance.UpdateApplication(updateService, this.configPath);
            var app = IconDashboardDataService.Instance.GetApplications(this.configPath)
                .First(a => a.Name == "Mammoth.Update.Controller" && a.Server == "vm-icon-test1");

            // Then
            Assert.AreEqual("Mammoth.Update.Controller", app.Name);
            Assert.AreEqual("vm-icon-test1", app.Server);
            Assert.AreEqual("update.config", app.ConfigFilePath);
            Assert.AreEqual("update name", app.DisplayName);

            //Assert.AreEqual(ApplicationTypeEnum.WindowsService, app.TypeOfApplication);
        }        

        [TestMethod]
        [ExpectedException(typeof(XmlSchemaException))]
        public void WhenXmlDataFileHasInvalidSchema_ThenVerifyDataFileSchema_ShouldThrowExeception()
        {
            //Given
            //invalid xml data file
            var invalidDataFile = "InvalidDataFile.xml";
            //When
            IconDashboardDataService.Instance.VerifyDataFileSchema(invalidDataFile, xsdPath);
            //Then
            //expected exception attribute should have encountered the expected exception
        }

        [TestMethod]
        public void WhenXmlDataFileHasValidSchema_ThenVerifyDataFileSchema_ShouldDoNothing()
        {
            //Given
            //existing valid xml data file
            var validDataFile = this.configPath;
            //When
            IconDashboardDataService.Instance.VerifyDataFileSchema(validDataFile, xsdPath);
            //Then
            //if no exception occurs, we are good
        }
    }
}
