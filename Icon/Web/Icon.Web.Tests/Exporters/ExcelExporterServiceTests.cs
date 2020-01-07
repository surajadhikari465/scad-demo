using Icon.Web.Mvc.Exporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Icon.Web.Tests.Unit.Exporters
{
    [TestClass] [Ignore]
    public class ExcelExporterServiceTests
    {
        private IExcelExporterService exporterService;

        [TestMethod]
        public void GetItemTemplateNewExporter_NoError_ShouldReturnItemTemplateNewExporter()
        {
            // Given.
            exporterService = new ExcelExporterService();

            // When.
            var itemTemplateNewExporter = exporterService.GetItemTemplateNewExporter(null, true, true);

            // Then.
            Assert.IsInstanceOfType(itemTemplateNewExporter, typeof(ItemNewTemplateExporter));
        }

        [TestMethod]
        public void GetNationalClassExporter_NoError_ShouldReturnNationalClassExporter()
        {
            // Given.
            exporterService = new ExcelExporterService();

            // When.
            var nationalClassExporter = exporterService.GetNationalClassExporter();

            // Then.
            Assert.IsInstanceOfType(nationalClassExporter, typeof(NationalClassExporter));
        }

        [TestMethod]
        public void GetAttributeExporter_NoError_ShouldReturnAttributeExporter()
        {
            // Given.
            exporterService = new ExcelExporterService();

            // When.
            var attributeExporter = exporterService.GetAttributeExporter();

            // Then.
            Assert.IsInstanceOfType(attributeExporter, typeof(AttributeExporter));
        }

        [TestMethod]
        public void GetContactExporter_NoError_ShouldReturnContactExporter()
        {
            // Given.
            exporterService = new ExcelExporterService();

            // When.
            var contactExporter = exporterService.GetContactExporter();

            // Then.
            Assert.IsInstanceOfType(contactExporter, typeof(ContactExporter));
        }
    }
}