using Icon.Web.Mvc.Exporters;

namespace Icon.Web.Mvc.Exporters
{
    public interface IExcelExporterService
    {
        ItemExporter GetItemExporter();
        BulkItemExporter GetBulkItemExporter();
        BulkNewItemExporter GetBulkNewItemExporter();
        IrmaItemExporter GetIrmaItemExporter();
        PluExporter GetPluExporter();
        BulkPluExporter GetBulkPluExporter();
        NewItemTemplateExporter GetNewItemTemplateExporter();
        HierarchyClassExporter GetHierarchyClassExporter();
        BrandExporter GetBrandExporter();
        BulkBrandExporter GetBulkBrandExporter();
        BrandTemplateExporter GetBrandTemplateExporter();
        PluCategoryExporter GetPluCategoryExporter();
        ItemTemplateExporter GetItemTemplateExporter();
        PluRequestExporter GetPluRequestExporter();
        CertificationAgencyExporter GetCertificationAgencyExporter();
        DefaultTaxMismatchesExporter GetDefaultTaxMismatchExporter();
    }
}