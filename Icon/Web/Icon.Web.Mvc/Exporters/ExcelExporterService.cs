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
using Icon.Web.Mvc.Utility;

namespace Icon.Web.Mvc.Exporters
{
    public class ExcelExporterService : IExcelExporterService
    {
        private IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>> getHierarchyClassesQueryHandler;
        private IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>> getAttributesQueryHandler;
        private IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>> getBarcodeTypeQueryHandler;
        private IQueryHandler<GetContactsParameters, List<ContactModel>> getContactsQuery;
        private IQueryHandler<GetContactTypesParameters, List<ContactTypeModel>> getContactTypeQuery;
        private ExcelExportModel exportModel = new ExcelExportModel(WorkbookFormat.Excel2007);
        private SqlConnection connection;
        private IQueryHandler<EmptyQueryParameters<List<ItemColumnOrderModel>>, List<ItemColumnOrderModel>> getItemColumnOrderQueryHandler;

        public ExcelExporterService(IQueryHandler<EmptyQueryParameters<List<ItemColumnOrderModel>>, List<ItemColumnOrderModel>> getItemColumnOrderQueryHandler)
        {
            this.getItemColumnOrderQueryHandler = getItemColumnOrderQueryHandler;
        }

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

        public BrandTemplateExporter GetBrandTemplateExporter()
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
            getHierarchyClassesQueryHandler = new GetHierarchyClassesQueryHandler(connection);
            BrandTemplateExporter exporter = new BrandTemplateExporter(getHierarchyClassesQueryHandler);
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
           
            ItemNewTemplateExporter itemTemplateNewExporter = new ItemNewTemplateExporter(getHierarchyClassesQueryHandler, getAttributesQueryHandler, getBarcodeTypeQueryHandler, this.getItemColumnOrderQueryHandler);
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
        
        public ContactNewTemplateExporter GetContactBlankTemplateExporter()
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
            getHierarchyClassesQueryHandler = new GetHierarchyClassesQueryHandler(connection);
            getContactsQuery = new GetContactsQuery(connection);
            getContactTypeQuery = new GetContactTypesQuery(connection);
            ContactNewTemplateExporter contactTemplateNewExporter = new ContactNewTemplateExporter(getHierarchyClassesQueryHandler, getContactsQuery, getContactTypeQuery);
            contactTemplateNewExporter.ExportModel = exportModel;

            contactTemplateNewExporter.AddSpreadsheetColumns();

            return contactTemplateNewExporter;
        }
    }
}