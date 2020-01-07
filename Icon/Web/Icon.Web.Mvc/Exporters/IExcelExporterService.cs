using System.Collections.Generic;
using Icon.Web.Mvc.Models;

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
        ContactExporter GetContactExporter();
    }
}