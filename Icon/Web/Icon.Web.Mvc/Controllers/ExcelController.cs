using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Extensions;
using Icon.Web.Mvc.Excel.ModelMappers;
using Icon.Web.Mvc.Excel.Models;
using Icon.Web.Mvc.Excel.Services;
using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Utility;
using Infragistics.Documents.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;

namespace Icon.Web.Controllers
{
    public class ExcelController : Controller
    {
        private const int MaxNumberOfItemsToExportFromSearch = 10000;
        private IExcelExporterService exporterService;
        private IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> queryHandler;
        private IQueryHandler<GetCertificationAgenciesParameters, List<CertificationAgencyModel>> getCertificationAgenciesQuery;
        private IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>> bulkScanCodeSearchQuery;
        private IQueryHandler<GetItemsBySearchParameters, ItemsBySearchResultsModel> getItemsBySearchQueryHandler;
        private IQueryHandler<GetDefaultTaxClassMismatchesParameters, List<DefaultTaxClassMismatchModel>> getDefaultTaxClassMismatchesQuery;
        private IInfragisticsHelper infragisticsHelper;
        private IExcelService<ItemExcelModel> itemExcelService;
        private IExcelModelMapper<ItemSearchModel, ItemExcelModel> itemModelMapper;

        public ExcelController(IExcelExporterService exporterService,
            IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> queryHandler,
            IQueryHandler<GetCertificationAgenciesParameters, List<CertificationAgencyModel>> getCertificationAgenciesQuery,
            IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>> bulkScanCodeSearchQuery,
            IQueryHandler<GetItemsBySearchParameters, ItemsBySearchResultsModel> getItemsBySearchQueryHandler,
            IQueryHandler<GetDefaultTaxClassMismatchesParameters, List<DefaultTaxClassMismatchModel>> getDefaultTaxClassMismatchesQuery,
            IInfragisticsHelper infragisticsHelper, 
            IExcelService<ItemExcelModel> itemExcelService,
            IExcelModelMapper<ItemSearchModel, ItemExcelModel> itemModelMapper)
        {
            this.exporterService = exporterService;
            this.queryHandler = queryHandler;
            this.getCertificationAgenciesQuery = getCertificationAgenciesQuery;
            this.bulkScanCodeSearchQuery = bulkScanCodeSearchQuery;
            this.getItemsBySearchQueryHandler = getItemsBySearchQueryHandler;
            this.getDefaultTaxClassMismatchesQuery = getDefaultTaxClassMismatchesQuery;
            this.infragisticsHelper = infragisticsHelper;
            this.itemExcelService = itemExcelService;
            this.itemModelMapper = itemModelMapper;
        }

        [HttpPost]
        public void IrmaItemExport(List<IrmaItemViewModel> items)
        {
            foreach (IrmaItemViewModel item in items)
            {
                if (item.MerchandiseHierarchyClassId == null)
                {
                    item.MerchandiseHierarchyClassName = String.Empty;
                }
                else
                {
                    item.MerchandiseHierarchyClassName = queryHandler.Search(new GetHierarchyClassByIdParameters { HierarchyClassId = item.MerchandiseHierarchyClassId.Value }).ToFlattenedHierarchyClassString();
                }
            }

            var irmaItemExporter = exporterService.GetIrmaItemExporter();
            irmaItemExporter.ExportData = items;
            irmaItemExporter.Export();

            SendForDownload(irmaItemExporter.ExportModel.ExcelWorkbook, irmaItemExporter.ExportModel.ExcelWorkbook.CurrentFormat, "NewItems");
        }

        [HttpGet]
        public void BulkNewItemExport()
        {
            var exportData = new List<BulkImportNewItemModel>();

            if (Session["new_item_upload_errors"] == null)
            {
                throw new InvalidOperationException(Icon.Web.Mvc.Resources.Excel.ExcelExportInvalidSessionCache);
            }
            else
            {
                exportData = Session["new_item_upload_errors"] as List<BulkImportNewItemModel>;
            }

            var bulkItemExporter = exporterService.GetBulkNewItemExporter();
            bulkItemExporter.ExportData = exportData;
            bulkItemExporter.Export();

            SendForDownload(bulkItemExporter.ExportModel.ExcelWorkbook, bulkItemExporter.ExportModel.ExcelWorkbook.CurrentFormat, "BulkNewItemErrors");
        }

        [HttpGet]
        public void NewItemTemplateExport()
        {
            var newItemTemplateExporter = exporterService.GetNewItemTemplateExporter();
            newItemTemplateExporter.Export();

            SendForDownload(newItemTemplateExporter.ExportModel.ExcelWorkbook, newItemTemplateExporter.ExportModel.ExcelWorkbook.CurrentFormat, "NewItem");
        }

        [HttpPost]
        public void HierarchyExport(string hierarchyName, List<HierarchyClassExportViewModel> hierarchyClasses)
        {
            var hierarchyClassExporter = exporterService.GetHierarchyClassExporter();
            hierarchyClassExporter.ExportData = hierarchyClasses;
            hierarchyClassExporter.Export();

            SendForDownload(hierarchyClassExporter.ExportModel.ExcelWorkbook, hierarchyClassExporter.ExportModel.ExcelWorkbook.CurrentFormat, hierarchyName + "Hierarchy");
        }

        /// <summary>
        /// Receives a Json object that can be deserialized into BrandExportViewModel.
        /// This is called from the brand-index.js code.
        /// Receiving a stringified JSON object and deserializing it on the server was much faster than
        /// letting the model binder work for a huge list.
        /// </summary>
        /// <param name="brands">stringified JSON object of BrandExportViewModel</param>
        [HttpPost]
        public void BrandExport(string brands)
        {
            List<BrandExportViewModel> deserializedBrands = JsonConvert.DeserializeObject<List<BrandExportViewModel>>(brands);

            var brandExporter = exporterService.GetBrandExporter();
            brandExporter.ExportData = deserializedBrands;
            brandExporter.Export();

            SendForDownload(brandExporter.ExportModel.ExcelWorkbook, brandExporter.ExportModel.ExcelWorkbook.CurrentFormat, "Brand");
        }

        [HttpGet]
        public void BulkBrandExport()
        {
            var exportData = new List<BulkImportBrandModel>();

            if (Session["grid_search_results"] == null)
            {
                throw new InvalidOperationException(Icon.Web.Mvc.Resources.Excel.ExcelExportInvalidSessionCache);
            }
            else
            {
                exportData = Session["grid_search_results"] as List<BulkImportBrandModel>;
            }

            var bulkBrandExporter = exporterService.GetBulkBrandExporter();
            bulkBrandExporter.ExportData = exportData;
            bulkBrandExporter.Export();

            SendForDownload(bulkBrandExporter.ExportModel.ExcelWorkbook, bulkBrandExporter.ExportModel.ExcelWorkbook.CurrentFormat, "BulkBrandErrors");
        }

        [HttpPost]
        public void ItemExport(List<string> items)
        {
            var parameters = new GetItemsByBulkScanCodeSearchParameters { ScanCodes = items };
            var searchResults = bulkScanCodeSearchQuery.Search(parameters);

            var exportResponse = itemExcelService.Export(new ExportRequest<ItemExcelModel>
            {
                Rows = itemModelMapper.Map(searchResults).ToList()
            });

            SendForDownload(exportResponse.ExcelWorkbook, WorkbookFormat.Excel2007, "Item");
        }

        [HttpGet]
        public void ItemSearchExport(ItemSearchViewModel viewModel)
        {
            var searchParameters = viewModel.GetSearchParameters(pageSize:MaxNumberOfItemsToExportFromSearch);
            
            var result = infragisticsHelper.ParseSortParameterFromQueryString(ControllerContext.HttpContext.Request.QueryString);
            if (result.SortParameterExists)
            {
                searchParameters.SortOrder = result.SortOrder;
                searchParameters.SortColumn = result.SortColumn;
            }
            var searchResults = getItemsBySearchQueryHandler.Search(searchParameters).Items;

            var exportResponse = itemExcelService.Export(new ExportRequest<ItemExcelModel>
            {
                Rows = itemModelMapper.Map(searchResults).ToList()
            });

            SendForDownload(exportResponse.ExcelWorkbook, WorkbookFormat.Excel2007, "Item");
        }

        [HttpGet]
        public void BulkItemExport()
        {
            if (Session["consolidated_item_upload_errors"] == null)
            {
                throw new InvalidOperationException(Icon.Web.Mvc.Resources.Excel.ExcelExportInvalidSessionCache);
            }
            else
            {
                var exportData = Session["consolidated_item_upload_errors"] as List<ItemExcelModel>;

                var exportResponse = itemExcelService.Export(new ExportRequest<ItemExcelModel>
                {
                    Rows = exportData
                });

                SendForDownload(exportResponse.ExcelWorkbook, WorkbookFormat.Excel2007, "BulkItemErrors");
            }
        }

        [HttpGet]
        public void ItemTemplateExport()
        {
            var exportResponse = itemExcelService.Export(new ExportRequest<ItemExcelModel>
            {
                CreateTemplate = true
            });

            SendForDownload(exportResponse.ExcelWorkbook, WorkbookFormat.Excel2007, "Item");
        }

        [HttpGet]
        public void PluExport()
        {
            var exportData = new List<PluMappingViewModel>();

            if (Session["grid_search_results"] == null)
            {
                throw new InvalidOperationException(Icon.Web.Mvc.Resources.Excel.ExcelExportInvalidSessionCache);
            }
            else
            {
                exportData = Session["grid_search_results"] as List<PluMappingViewModel>;
            }

            var pluExporter = exporterService.GetPluExporter();
            pluExporter.ExportData = exportData;
            pluExporter.Export();

            SendForDownload(pluExporter.ExportModel.ExcelWorkbook, pluExporter.ExportModel.ExcelWorkbook.CurrentFormat, "PLUMapping");
        }

        [HttpGet]
        public void BulkPluExport()
        {
            var exportData = new List<BulkImportPluModel>();

            if (Session["grid_search_results"] == null)
            {
                throw new InvalidOperationException(Icon.Web.Mvc.Resources.Excel.ExcelExportInvalidSessionCache);
            }
            else
            {
                exportData = Session["grid_search_results"] as List<BulkImportPluModel>;
            }

            var bulkPluExporter = exporterService.GetBulkPluExporter();
            bulkPluExporter.ExportData = exportData;
            bulkPluExporter.Export();

            SendForDownload(bulkPluExporter.ExportModel.ExcelWorkbook, bulkPluExporter.ExportModel.ExcelWorkbook.CurrentFormat, "BulkPLUMappingErrors");
        }

        [HttpGet]
        public void BulkCertificationAgencyExport()
        {
            var exportData = new List<BulkImportCertificationAgencyModel>();

            if (Session["certification_agency_import_errors"] == null)
            {
                throw new InvalidOperationException(Icon.Web.Mvc.Resources.Excel.ExcelExportInvalidSessionCache);
            }
            else
            {
                exportData = Session["certification_agency_import_errors"] as List<BulkImportCertificationAgencyModel>;
            }

            var certificationExporter = exporterService.GetCertificationAgencyExporter();
            certificationExporter.ExportData = exportData;
            certificationExporter.Export();

            SendForDownload(certificationExporter.ExportModel.ExcelWorkbook, certificationExporter.ExportModel.ExcelWorkbook.CurrentFormat, "BulkCertificationAgencyErrors");
        }

        [HttpGet]
        public void CertificationAgencyTemplateExport()
        {
            var exporter = exporterService.GetCertificationAgencyExporter();
            exporter.Export();

            SendForDownload(exporter.ExportModel.ExcelWorkbook, exporter.ExportModel.ExcelWorkbook.CurrentFormat, "CertificationAgencies");
        }

        [HttpGet]
        public void CertificationAgencyExport()
        {
            var exportData = getCertificationAgenciesQuery.Search(new GetCertificationAgenciesParameters())
                .Select(ca => new BulkImportCertificationAgencyModel
                {
                    CertificationAgencyNameAndId = ca.HierarchyClassName + "|" + ca.HierarchyClassId,
                    GlutenFree = ca.GlutenFree,
                    Kosher = ca.Kosher,
                    NonGmo = ca.NonGMO,
                    Organic = ca.Organic,
                    DefaultOrganic = ca.DefaultOrganic,
                    Vegan = ca.Vegan
                })
                .ToList();

            var exporter = exporterService.GetCertificationAgencyExporter();
            exporter.ExportData = exportData;
            exporter.Export();

            SendForDownload(exporter.ExportModel.ExcelWorkbook, exporter.ExportModel.ExcelWorkbook.CurrentFormat, "CertificationAgencies");
        }

        [HttpGet]
        public void BrandTemplateExport()
        {
            var brandTemplateExporter = exporterService.GetBrandTemplateExporter();
            brandTemplateExporter.Export();

            SendForDownload(brandTemplateExporter.ExportModel.ExcelWorkbook, brandTemplateExporter.ExportModel.ExcelWorkbook.CurrentFormat, "Brand");
        }

        [HttpPost]
        public void PluCategoryExport(List<PluCategoryViewModel> plucategoryList)
        {
            var pluCategoryExporter = exporterService.GetPluCategoryExporter();
            pluCategoryExporter.ExportData = plucategoryList;
            pluCategoryExporter.Export();

            SendForDownload(pluCategoryExporter.ExportModel.ExcelWorkbook, pluCategoryExporter.ExportModel.ExcelWorkbook.CurrentFormat, "PLUCategory");
        }

        [HttpGet]
        public void DefaultTaxMismatchExport()
        {
            var queryResults = getDefaultTaxClassMismatchesQuery.Search(new GetDefaultTaxClassMismatchesParameters());

            var exportData = queryResults.Select(r => new DefaultTaxClassMismatchExportModel
            {
                ScanCode = r.ScanCode,
                Brand = r.Brand,
                ProductDescription = r.ProductDescription,
                MerchandiseLineage = r.MerchandiseLineage,
                DefaultTaxClass = r.DefaultTaxClass,
                TaxClassOverride = r.TaxClassOverride,
                Error = String.Empty
            }).ToList();

            var exporter = exporterService.GetDefaultTaxMismatchExporter();
            exporter.ExportData = exportData;
            exporter.Export();

            SendForDownload(exporter.ExportModel.ExcelWorkbook, exporter.ExportModel.ExcelWorkbook.CurrentFormat, "DefaultTaxMismatches");
        }

        [HttpPost]
        public void DefaultTaxMismatchExport(List<DefaultTaxClassMismatchExportModel> viewModel)
        {
            if (Session["tax_corrections_upload_errors"] != null)
            {
                var cachedErrors = Session["tax_corrections_upload_errors"] as List<DefaultTaxClassMismatchExportModel>;

                var exportModels = new List<DefaultTaxClassMismatchExportModel>();
                DefaultTaxClassMismatchExportModel exportModel;
                string modelError;

                foreach (var error in cachedErrors)
                {
                    modelError = cachedErrors.Single(e => e.ScanCode == error.ScanCode).Error;

                    exportModel = new DefaultTaxClassMismatchExportModel
                    {
                        ScanCode = error.ScanCode,
                        Brand = error.Brand,
                        ProductDescription = error.ProductDescription,
                        MerchandiseLineage = error.MerchandiseLineage,
                        DefaultTaxClass = error.DefaultTaxClass,
                        TaxClassOverride = error.TaxClassOverride,
                        Error = modelError
                    };

                    exportModels.Add(exportModel);
                }

                var exporter = exporterService.GetDefaultTaxMismatchExporter();
                exporter.ExportData = exportModels;
                exporter.Export();

                SendForDownload(exporter.ExportModel.ExcelWorkbook, exporter.ExportModel.ExcelWorkbook.CurrentFormat, "DefaultTaxMismatches");
            }
            else
            {
                throw new InvalidOperationException(Icon.Web.Mvc.Resources.Excel.ExcelExportInvalidSessionCache);
            }
        }

        [SecuritySafeCritical]
        private void SendForDownload(Workbook document, WorkbookFormat excelFormat, string source)
        {
            string documentFileNameRoot = String.Format("IconExport_{0}_{1}.xlsx", source, DateTime.Now.ToString("yyyyMMddHHmmss"));

            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=" + documentFileNameRoot);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.SetCookie(new HttpCookie("fileDownload", "true") { Path = "/" });
            document.SetCurrentFormat(excelFormat);
            document.Save(Response.OutputStream);
            Response.End();
        }

        private string ConvertDateFormat(string stringDateValue)
        {
            if (String.IsNullOrEmpty(stringDateValue))
            {
                return stringDateValue;
            }

            DateTime dateValue = DateTime.Parse(stringDateValue);
            if (dateValue != null)
            {
                return dateValue.ToString("yyyy-MM-dd");
            }

            return String.Empty;
        }
    }
}
