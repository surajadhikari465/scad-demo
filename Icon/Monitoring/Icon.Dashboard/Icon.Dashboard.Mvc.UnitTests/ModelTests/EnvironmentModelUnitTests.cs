using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.Models.CustomConfigElements;
using Icon.Dashboard.Mvc.UnitTests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.Mvc.UnitTests.ModelTests
{
    [TestClass]
    public class EnvironmentModelUnitTests
    {
        protected ConfigTestData testData = new ConfigTestData();

        [TestMethod]
        public void EnvironmentModel_DefaultConstructor_InitializesCollections()
        {
            // Arrange
            // Act
            var environmentModel = new EnvironmentModel();
            // Assert
            Assert.IsNotNull(environmentModel.AppServers);
        }

        [TestMethod]
        public void EnvironmentModel_EnvironmentElementConstructor_SetsExpectedProperties()
        {
            // Arrange
            var testElement = testData.Elements.Tst0;
            // Act
            var environmentModel = new EnvironmentModel(testElement);
            // Assert
            Assert.AreEqual("Tst0", environmentModel.Name);
            Assert.AreEqual(EnvironmentEnum.Tst0, environmentModel.EnvironmentEnum);
            Assert.AreEqual(true, environmentModel.IsEnabled);
            Assert.AreEqual("http://irmatestapp1/IconDashboard", environmentModel.DashboardUrl);
            Assert.AreEqual("irmatestapp1", environmentModel.WebServer);
            CustomAsserts.ListsAreEqual(
                new List<string> { "vm-icon-test1", "vm-icon-test2" },
                environmentModel.AppServers);
            Assert.AreEqual(@"http://icon-test/", environmentModel.IconWebUrl);
            Assert.AreEqual(@"http://irmatestapp1/MammothWebSupport", environmentModel.MammothWebSupportUrl);
            CustomAsserts.ListsAreEqual(
                new List<string> { @"https://cerd1669:18090/", @"https://cerd1670:18090/" },
                environmentModel.TibcoAdminUrls);
            Assert.AreEqual(@"CEWD1815\SQLSHARED2012D", environmentModel.IconDatabaseServer);
            Assert.AreEqual("iCON", environmentModel.IconDatabaseName);
            Assert.AreEqual(@"MAMMOTH-DB01-DEV\MAMMOTH", environmentModel.MammothDatabaseServer);
            Assert.AreEqual("Mammoth", environmentModel.MammothDatabaseName);
            CustomAsserts.ListsAreEqual(
                new List<string> { @"idt-nc\nct",@"idt-ne\net",@"idt-pn\pnt",@"idt-sp\spt",@"idt-sw\swt",@"idt-uk\ukt" }
                , environmentModel.IrmaDatabaseServers);
            Assert.AreEqual("ItemCatalog_Test", environmentModel.IrmaDatabaseName);
        }

    }
}
