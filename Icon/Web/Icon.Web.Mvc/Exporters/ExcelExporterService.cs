using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using Icon.Common;
using Icon.Common.Models;
using Icon.Web.Mvc.Excel;

namespace Icon.Web.Mvc.Exporters
{
    public class ExcelExporterService : IExcelExporterService
    {
        private IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>> getHierarchyClassesQueryHandler;
        private IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>> getAttributesQueryHandler;
        private IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>> getBarcodeTypeQueryHandler;
        private ExcelExportModel exportModel = new ExcelExportModel(WorkbookFormat.Excel2007);
        private SqlConnection connection;

        public HierarchyClassExporter GetHierarchyClassExporter()
        {
            HierarchyClassExporter exporter = new HierarchyClassExporter();
            exporter.ExportModel = exportModel;

            return exporter;
        }

        public BrandExporter GetBrandExporter()
        {
            BrandExporter exporter = new BrandExporter();
            exporter.ExportModel = exportModel;

            return exporter;
        }

        public ManufacturerExporter GetManufacturerExporter()
        {
            ManufacturerExporter exporter = new ManufacturerExporter
            {
                ExportModel = exportModel
            };

            return exporter;
        }

        public BulkBrandExporter GetBulkBrandExporter()
        {
            BulkBrandExporter exporter = new BulkBrandExporter();
            exporter.ExportModel = exportModel;

            return exporter;
        }

        public BrandTemplateExporter GetBrandTemplateExporter()
        {
            BrandTemplateExporter exporter = new BrandTemplateExporter();
            exporter.ExportModel = exportModel;
            return exporter;
        }

        public BarcodeTypeExporter GetBarcodeTypeExporter()
        {
            BarcodeTypeExporter barcodeTypeExporter = new BarcodeTypeExporter();
            barcodeTypeExporter.ExportModel = exportModel;
            return barcodeTypeExporter;
        }

        public ItemNewTemplateExporter GetItemTemplateNewExporter(List<string> selectedColumnNames = null, bool exportAllAttributes = false, bool exportNewItemTemplate = false)
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
            getHierarchyClassesQueryHandler = new GetHierarchyClassesQueryHandler(connection);
            getAttributesQueryHandler = new GetAttributesQueryHandler(connection);
            getBarcodeTypeQueryHandler = new GetBarcodeTypesQuery(connection);
            ItemNewTemplateExporter itemTemplateNewExporter = new ItemNewTemplateExporter(getHierarchyClassesQueryHandler, getAttributesQueryHandler, getBarcodeTypeQueryHandler);
            itemTemplateNewExporter.ExportModel = exportModel;
            itemTemplateNewExporter.ExportAllAttributes = exportAllAttributes;
            itemTemplateNewExporter.SelectedColumnNames = selectedColumnNames;
            itemTemplateNewExporter.ExportNewItemTemplate = exportNewItemTemplate;
            itemTemplateNewExporter.ListHiddenColumnNames = new List<string>()
            {
                NewItemExcelHelper.NewExcelExportColumnNames.ItemId, NewItemExcelHelper.NewExcelExportColumnNames.ItemType, NewItemExcelHelper.NewExcelExportColumnNames.Financial, Constants.Attributes.ProhibitDiscount,
                Constants.Attributes.CreatedBy, Constants.Attributes.CreatedDateTimeUtc, Constants.Attributes.ModifiedBy, Constants.Attributes.ModifiedDateTimeUtc
            };
            itemTemplateNewExporter.AddSpreadsheetColumns();

            return itemTemplateNewExporter;
        }

        public NationalClassExporter GetNationalClassExporter()
        {
            NationalClassExporter exporter = new NationalClassExporter();
            exporter.ExportModel = exportModel;

            return exporter;
        }

        public AttributeExporter GetAttributeExporter()
        {
            AttributeExporter exporter = new AttributeExporter();
            exporter.ExportModel = exportModel;

            return exporter;
        }

        public ContactExporter GetContactExporter()
        {
            ContactExporter exporter = new ContactExporter();
            exporter.ExportModel = exportModel;

            return exporter;
        }

    }
}