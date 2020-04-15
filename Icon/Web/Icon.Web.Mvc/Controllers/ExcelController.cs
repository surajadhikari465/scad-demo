using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public ExcelController(IExcelExporterService exporterService,
            IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> queryHandler,        
            IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>> bulkScanCodeSearchQuery,
            IQueryHandler<GetItemsBySearchParameters, ItemsBySearchResultsModel> getItemsBySearchQueryHandler,
            IQueryHandler<GetDefaultTaxClassMismatchesParameters, List<DefaultTaxClassMismatchModel>> getDefaultTaxClassMismatchesQuery)
        {
            this.exporterService = exporterService;
            this.queryHandler = queryHandler;
            this.bulkScanCodeSearchQuery = bulkScanCodeSearchQuery;
            this.getItemsBySearchQueryHandler = getItemsBySearchQueryHandler;
            this.getDefaultTaxClassMismatchesQuery = getDefaultTaxClassMismatchesQuery;
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
