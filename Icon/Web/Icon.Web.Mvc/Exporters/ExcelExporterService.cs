using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using System.Collections.Generic;
using System;

namespace Icon.Web.Mvc.Exporters
{
    public class ExcelExporterService : IExcelExporterService
    {
        private IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQueryHandler;
        private IQueryHandler<GetMerchTaxMappingsParameters, List<MerchTaxMappingModel>> getMerchTaxMappingQueryHandler;
        private IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>> getCertificationAgenciesQueryHandler;
        private IQueryHandler<GetBrandsParameters, List<BrandModel>> getBrandsQuery;
        private ExcelExportModel exportModel = new ExcelExportModel(WorkbookFormat.Excel2007);

        public ItemExporter GetItemExporter()
        {
            getHierarchyLineageQueryHandler = new GetHierarchyLineageQuery(new IconContext());
            getMerchTaxMappingQueryHandler = new GetMerchTaxMappingsQuery(new IconContext());
            getCertificationAgenciesQueryHandler = new GetCertificationAgenciesByTraitQuery(new IconContext());

            ItemExporter itemExporter = new ItemExporter(getHierarchyLineageQueryHandler, getMerchTaxMappingQueryHandler, getCertificationAgenciesQueryHandler);
            itemExporter.ExportModel = exportModel;

            return itemExporter;
        }

        public BulkItemExporter GetBulkItemExporter()
        {
            getHierarchyLineageQueryHandler = new GetHierarchyLineageQuery(new IconContext());
            getMerchTaxMappingQueryHandler = new GetMerchTaxMappingsQuery(new IconContext());
            getCertificationAgenciesQueryHandler = new GetCertificationAgenciesByTraitQuery(new IconContext());

            BulkItemExporter bulkItemExporter = new BulkItemExporter(getHierarchyLineageQueryHandler, getMerchTaxMappingQueryHandler, getCertificationAgenciesQueryHandler);
            bulkItemExporter.ExportModel = exportModel;

            return bulkItemExporter;
        }

        public BulkNewItemExporter GetBulkNewItemExporter()
        {
            getHierarchyLineageQueryHandler = new GetHierarchyLineageQuery(new IconContext());
            getMerchTaxMappingQueryHandler = new GetMerchTaxMappingsQuery(new IconContext());
            getCertificationAgenciesQueryHandler = new GetCertificationAgenciesByTraitQuery(new IconContext());
            getBrandsQuery = new GetBrandsQuery(new IconContext());


            BulkNewItemExporter bulkItemExporter = new BulkNewItemExporter(getHierarchyLineageQueryHandler, getMerchTaxMappingQueryHandler, getCertificationAgenciesQueryHandler, getBrandsQuery);
            bulkItemExporter.ExportModel = exportModel;

            return bulkItemExporter;
        }

        public IrmaItemExporter GetIrmaItemExporter()
        {
            getHierarchyLineageQueryHandler = new GetHierarchyLineageQuery(new IconContext());
            getMerchTaxMappingQueryHandler = new GetMerchTaxMappingsQuery(new IconContext());
            getCertificationAgenciesQueryHandler = new GetCertificationAgenciesByTraitQuery(new IconContext());
            getBrandsQuery = new GetBrandsQuery(new IconContext());

            IrmaItemExporter irmaItemExporter = new IrmaItemExporter(getHierarchyLineageQueryHandler, getMerchTaxMappingQueryHandler, getCertificationAgenciesQueryHandler, getBrandsQuery);
            irmaItemExporter.ExportModel = exportModel;

            return irmaItemExporter;
        }

        public PluExporter GetPluExporter()
        {
            PluExporter pluExporter = new PluExporter();
            pluExporter.ExportModel = exportModel;

            return pluExporter;
        }

        public BulkPluExporter GetBulkPluExporter()
        {
            BulkPluExporter bulkPluExporter = new BulkPluExporter();
            bulkPluExporter.ExportModel = exportModel;

            return bulkPluExporter;
        }

        public NewItemTemplateExporter GetNewItemTemplateExporter()
        {
            getHierarchyLineageQueryHandler = new GetHierarchyLineageQuery(new IconContext());
            getMerchTaxMappingQueryHandler = new GetMerchTaxMappingsQuery(new IconContext());
            getCertificationAgenciesQueryHandler = new GetCertificationAgenciesByTraitQuery(new IconContext());
            getBrandsQuery = new GetBrandsQuery(new IconContext());

            NewItemTemplateExporter newItemTemplateExporter = new NewItemTemplateExporter(getHierarchyLineageQueryHandler, getMerchTaxMappingQueryHandler, getCertificationAgenciesQueryHandler, getBrandsQuery);
            newItemTemplateExporter.ExportModel = exportModel;

            return newItemTemplateExporter;
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

        public PluCategoryExporter GetPluCategoryExporter()
        {
            PluCategoryExporter pluCategoryExporter = new PluCategoryExporter();
            pluCategoryExporter.ExportModel = exportModel;
            return pluCategoryExporter;
        }

        public ItemTemplateExporter GetItemTemplateExporter()
        {
            getHierarchyLineageQueryHandler = new GetHierarchyLineageQuery(new IconContext());
            getMerchTaxMappingQueryHandler = new GetMerchTaxMappingsQuery(new IconContext());
            getCertificationAgenciesQueryHandler = new GetCertificationAgenciesByTraitQuery(new IconContext());

            ItemTemplateExporter itemTemplateExporter = new ItemTemplateExporter(getHierarchyLineageQueryHandler, getMerchTaxMappingQueryHandler, getCertificationAgenciesQueryHandler);
            itemTemplateExporter.ExportModel = exportModel;

            return itemTemplateExporter;
        }

        public PluRequestExporter GetPluRequestExporter()
        {
            getHierarchyLineageQueryHandler = new GetHierarchyLineageQuery(new IconContext());

            PluRequestExporter pluRequestExporter = new PluRequestExporter(getHierarchyLineageQueryHandler);
            pluRequestExporter.ExportModel = exportModel;

            return pluRequestExporter;
        }


        public CertificationAgencyExporter GetCertificationAgencyExporter()
        {
            return new CertificationAgencyExporter
            {
                ExportModel = exportModel
            };
        }

        public DefaultTaxMismatchesExporter GetDefaultTaxMismatchExporter()
        {
            getHierarchyLineageQueryHandler = new GetHierarchyLineageQuery(new IconContext());

            var exporter = new DefaultTaxMismatchesExporter(getHierarchyLineageQueryHandler);
            exporter.ExportModel = exportModel;

            return exporter;
        }
    }
}