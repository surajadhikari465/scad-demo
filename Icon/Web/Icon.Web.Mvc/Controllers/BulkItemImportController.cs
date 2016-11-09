using Icon.Common.DataAccess;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Models;
using Icon.Web.Mvc.Attributes;
using Icon.Web.Mvc.Excel.Models;
using Icon.Web.Mvc.Excel.Services;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Icon.Web.Controllers
{
    public class BulkItemImportController : Controller
    {
        private ILogger logger;
        private ICommandHandler<BulkImportCommand<BulkImportItemModel>> bulkImportCommand;
        private IExcelService<ItemExcelModel> excelService;

        public BulkItemImportController(
            ILogger logger,
            IExcelService<ItemExcelModel> excelService,
            ICommandHandler<BulkImportCommand<BulkImportItemModel>> bulkImportCommand)
        {
            this.logger = logger;
            this.excelService = excelService;
            this.bulkImportCommand = bulkImportCommand;
        }

        // GET: /BulkItemImport/Index
        public ActionResult Index()
        {
            // TODO : Fix to ItemExcelModel
            return View(new BulkImportViewModel<ItemExcelModel>()); 
        }

        // POST: /BulkItemImport/Import
        [WriteAccessAuthorize(IsJsonResult = false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Import([Bind(Include = "ExcelAttachment")] BulkImportViewModel<ItemExcelModel> viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", viewModel);
            }

            try
            {
                var importedWorkbook = Workbook.Load(viewModel.ExcelAttachment.InputStream);
                var importResponse = this.excelService.Import(importedWorkbook);

                if(!string.IsNullOrWhiteSpace(importResponse.ErrorMessage))
                {
                    viewModel.ValidSpreadsheetType = false;
                    return View("Index", viewModel);
                }
                else if (importResponse.Items.Any())
                {
                    var data = new BulkImportCommand<BulkImportItemModel>
                    {
                        BulkImportData = importResponse.Items.Select(i => i.ConvertToDataModel()).ToList(),
                        UserName = User.Identity.Name,
                        UpdateAllItemFields = false
                    };

                    bulkImportCommand.Execute(data);
                }

                viewModel.ValidItemsCount = importResponse.Items.Count();
                var errorItems = importResponse.ErrorItems;

                viewModel.ErrorItems = errorItems;
                Session["consolidated_item_upload_errors"] = errorItems;

                return View(viewModel);
            }
            catch (Exception ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                throw;
            }
        }
    }
}