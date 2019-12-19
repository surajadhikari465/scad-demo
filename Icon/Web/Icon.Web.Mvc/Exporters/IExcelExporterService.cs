using System.Collections.Generic;

namespace Icon.Web.Mvc.Exporters
{
    public interface IExcelExporterService
    {
        BarcodeTypeExporter GetBarcodeTypeExporter();
        HierarchyClassExporter GetHierarchyClassExporter();
        BrandExporter GetBrandExporter();
        ManufacturerExporter GetManufacturerExporter();
        BulkBrandExporter GetBulkBrandExporter();
        BrandTemplateExporter GetBrandTemplateExporter();
        ItemNewTemplateExporter GetItemTemplateNewExporter(List<string> selectedColumnNames = null, bool exportAllAttributes = false, bool exportNewItemTemplate = false);
        NationalClassExporter GetNationalClassExporter();
        AttributeExporter GetAttributeExporter();
    }
}