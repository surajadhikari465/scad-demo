using Icon.Web.Mvc.Exporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Icon.Web.Tests.Unit.Exporters
{
    [TestClass]
    public class ExcelExporterServiceTests
    {
        private IExcelExporterService exporterService;

        [TestMethod]
        public void GetItemExporter_NoError_ShouldReturnItemExporter()
        {
            // Given.
            exporterService = new ExcelExporterService();

            // When.
            var itemExporter = exporterService.GetItemExporter();

            // Then.
            Assert.IsInstanceOfType(itemExporter, typeof(ItemExporter));
        }

        [TestMethod]
        public void GetBulkItemExporter_NoError_ShouldReturnBulkItemExporter()
        {
            // Given.
            exporterService = new ExcelExporterService();

            // When.
            var bulkItemExporter = exporterService.GetBulkItemExporter();

            // Then.
            Assert.IsInstanceOfType(bulkItemExporter, typeof(BulkItemExporter));
        }

        [TestMethod]
        public void GetNewItemTemplateExporter_NoError_ShouldReturnNewItemTemplateExporter()
        {
            // Given.
            exporterService = new ExcelExporterService();

            // When.
            var newItemTemplateExporter = exporterService.GetNewItemTemplateExporter();

            // Then.
            Assert.IsInstanceOfType(newItemTemplateExporter, typeof(NewItemTemplateExporter));
        }

        [TestMethod]
        public void GetBulkNewItemExporter_NoError_ShouldReturnNewBulkItemTemplateExporter()
        {
            // Given.
            exporterService = new ExcelExporterService();

            // When.
            var bulkNewItemExporter = exporterService.GetBulkNewItemExporter();

            // Then.
            Assert.IsInstanceOfType(bulkNewItemExporter, typeof(BulkNewItemExporter));
        }

        [TestMethod]
        public void GetIrmaItemExporter_NoError_ShouldReturnIrmaItemExporter()
        {
            // Given.
            exporterService = new ExcelExporterService();

            // When.
            var irmaItemExporter = exporterService.GetIrmaItemExporter();

            // Then.
            Assert.IsInstanceOfType(irmaItemExporter, typeof(IrmaItemExporter));
        }

        [TestMethod]
        public void GetPluExporter_NoError_ShouldReturnPluExporter()
        {
            // Given.
            exporterService = new ExcelExporterService();

            // When.
            var pluExporter = exporterService.GetPluExporter();

            // Then.
            Assert.IsInstanceOfType(pluExporter, typeof(PluExporter));
        }

        [TestMethod]
        public void GetBulkPluExporter_NoError_ShouldReturnBulkPluExporter()
        {
            // Given.
            exporterService = new ExcelExporterService();

            // When.
            var bulkPluExporter = exporterService.GetBulkPluExporter();

            // Then.
            Assert.IsInstanceOfType(bulkPluExporter, typeof(BulkPluExporter));
        }

        [TestMethod]
        public void GetDefaultTaxMismatchExporter_NoError_ShouldReturnTaxMismatchExporter()
        {
            // Given.
            exporterService = new ExcelExporterService();

            // When.
            var taxMismatchExporter = exporterService.GetDefaultTaxMismatchExporter();

            // Then.
            Assert.IsInstanceOfType(taxMismatchExporter, typeof(DefaultTaxMismatchesExporter));
        }

        [TestMethod]
        public void GetCertificationAgencyExporter_NoError_ShouldReturnCertificationAgencyExporter()
        {
            // Given.
            exporterService = new ExcelExporterService();

            // When.
            var certificationAgencyExporter = exporterService.GetCertificationAgencyExporter();

            // Then.
            Assert.IsInstanceOfType(certificationAgencyExporter, typeof(CertificationAgencyExporter));
        }
    }
}
