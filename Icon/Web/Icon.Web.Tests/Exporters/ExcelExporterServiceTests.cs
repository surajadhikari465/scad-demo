using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Icon.Web.Tests.Unit.Exporters
{
    [TestClass]
    public class ExcelExporterServiceTests
    {
        private IExcelExporterService exporterService;
        private Mock<IOrderFieldsHelper> mockOrderFieldsHelper;


        [TestInitialize]
        public void Initialize()
        {
            mockOrderFieldsHelper = new Mock<IOrderFieldsHelper>();
        }

        [TestMethod]
        public void GetItemTemplateNewExporter_NoError_ShouldReturnItemTemplateNewExporter()
        {
            // Given.
            exporterService = new ExcelExporterService(mockOrderFieldsHelper.Object);

            // When.
            var itemTemplateNewExporter = exporterService.GetItemTemplateNewExporter(null, true, true);

            // Then.
            Assert.IsInstanceOfType(itemTemplateNewExporter, typeof(ItemNewTemplateExporter));
        }

        [TestMethod]
        public void GetNationalClassExporter_NoError_ShouldReturnNationalClassExporter()
        {
            // Given.
            exporterService = new ExcelExporterService(mockOrderFieldsHelper.Object);

            // When.
            var nationalClassExporter = exporterService.GetNationalClassExporter();

            // Then.
            Assert.IsInstanceOfType(nationalClassExporter, typeof(NationalClassExporter));
        }

        [TestMethod]
        public void GetAttributeExporter_NoError_ShouldReturnAttributeExporter()
        {
            // Given.
            exporterService = new ExcelExporterService(mockOrderFieldsHelper.Object);

            // When.
            var attributeExporter = exporterService.GetAttributeExporter();

            // Then.
            Assert.IsInstanceOfType(attributeExporter, typeof(AttributeExporter));
        }

        [TestMethod]
        public void GetContactBlankTemplateExporter_NoError_ShouldReturnContactExporter()
        {
            // Given.
            exporterService = new ExcelExporterService(mockOrderFieldsHelper.Object);

            // When.
            var contactBlankExporter = exporterService.GetContactBlankTemplateExporter();

            // Then.
            Assert.IsInstanceOfType(contactBlankExporter, typeof(ContactNewTemplateExporter));
        }
    }
}