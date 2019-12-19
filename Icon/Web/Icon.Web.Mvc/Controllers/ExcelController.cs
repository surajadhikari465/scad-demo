using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Extensions;
using Icon.Web.Mvc.Excel.ModelMappers;
using Icon.Web.Mvc.Excel.Models;
using Icon.Web.Mvc.Excel.Services;
using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
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
        private IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>> bulkScanCodeSearchQuery;
        private IQueryHandler<GetItemsBySearchParameters, ItemsBySearchResultsModel> getItemsBySearchQueryHandler;
        private IQueryHandler<GetDefaultTaxClassMismatchesParameters, List<DefaultTaxClassMismatchModel>> getDefaultTaxClassMismatchesQuery;
        private IExcelService<ItemExcelModel> itemExcelService;
        private IExcelModelMapper<ItemSearchModel, ItemExcelModel> itemModelMapper;

        public ExcelController(IExcelExporterService exporterService,
            IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> queryHandler,        
            IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>> bulkScanCodeSearchQuery,
            IQueryHandler<GetItemsBySearchParameters, ItemsBySearchResultsModel> getItemsBySearchQueryHandler,
            IQueryHandler<GetDefaultTaxClassMismatchesParameters, List<DefaultTaxClassMismatchModel>> getDefaultTaxClassMismatchesQuery,
            IExcelService<ItemExcelModel> itemExcelService,
            IExcelModelMapper<ItemSearchModel, ItemExcelModel> itemModelMapper)
        {
            this.exporterService = exporterService;
            this.queryHandler = queryHandler;
            this.bulkScanCodeSearchQuery = bulkScanCodeSearchQuery;
            this.getItemsBySearchQueryHandler = getItemsBySearchQueryHandler;
            this.getDefaultTaxClassMismatchesQuery = getDefaultTaxClassMismatchesQuery;
            this.itemExcelService = itemExcelService;
            this.itemModelMapper = itemModelMapper;
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
            
            //var result = infragisticsHelper.ParseSortParameterFromQueryString(ControllerContext.HttpContext.Request.QueryString);
            //if (result.SortParameterExists)
            //{
            //    searchParameters.SortOrder = result.SortOrder;
            //    searchParameters.SortColumn = result.SortColumn;
            //}
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
        public void BrandTemplateExport()
        {
            var brandTemplateExporter = exporterService.GetBrandTemplateExporter();
            brandTemplateExporter.Export();

            SendForDownload(brandTemplateExporter.ExportModel.ExcelWorkbook, brandTemplateExporter.ExportModel.ExcelWorkbook.CurrentFormat, "Brand");
        }

        [HttpPost]
        public void BarcodeTypeExport(List<BarcodeTypeViewModel> barcodeTypeList)
        {
            var barcodeTypeExporter = exporterService.GetBarcodeTypeExporter();
            barcodeTypeExporter.ExportData = barcodeTypeList;
            barcodeTypeExporter.Export();

            SendForDownload(barcodeTypeExporter.ExportModel.ExcelWorkbook, barcodeTypeExporter.ExportModel.ExcelWorkbook.CurrentFormat, "BarcodeType");
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
