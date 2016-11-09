using Icon.Common.DataAccess;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.Mvc.Attributes;
using Icon.Web.Mvc.Importers;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using System.Reflection;
using System.Web.Mvc;

namespace Icon.Web.Controllers
{
    public class BulkItemAddController : Controller
    {
        private ILogger logger;
        private ISpreadsheetImporter<BulkImportNewItemModel> newItemSpreadsheetImporter;
        private ICommandHandler<BulkImportCommand<BulkImportNewItemModel>> bulkImportCommand;

        public BulkItemAddController(
            ILogger logger,
            ISpreadsheetImporter<BulkImportNewItemModel> newItemSpreadsheetImporter, 
            ICommandHandler<BulkImportCommand<BulkImportNewItemModel>> bulkImportCommand)
        {
            this.logger = logger;
            this.newItemSpreadsheetImporter = newItemSpreadsheetImporter;
            this.bulkImportCommand = bulkImportCommand;
        }

        // GET: /BulkItemImport/Index
        public ActionResult Index()
        {
            return View(new BulkImportViewModel<BulkImportNewItemModel>());
        }

        // POST: /BulkItemImport/Import
        [WriteAccessAuthorize(IsJsonResult = false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Import([Bind(Include = "ExcelAttachment")] BulkImportViewModel<BulkImportNewItemModel> viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", viewModel);
            }

            newItemSpreadsheetImporter.Workbook = Workbook.Load(viewModel.ExcelAttachment.InputStream);

            if (!newItemSpreadsheetImporter.IsValidSpreadsheetType())
            {
                viewModel.ValidSpreadsheetType = false;
                return View("Index", viewModel);
            }

            newItemSpreadsheetImporter.ConvertSpreadsheetToModel();
            newItemSpreadsheetImporter.ValidateSpreadsheetData();

            viewModel.ErrorItems = newItemSpreadsheetImporter.ErrorRows;
            viewModel.ValidItemsCount = newItemSpreadsheetImporter.ValidRows.Count;

            if (viewModel.ValidItemsCount > 0)
            {
                BulkImportCommand<BulkImportNewItemModel> data = new BulkImportCommand<BulkImportNewItemModel> { BulkImportData = newItemSpreadsheetImporter.ValidRows, UserName = User.Identity.Name, UpdateAllItemFields=false };
                
                try
                {
                    bulkImportCommand.Execute(data);
                }
                catch (CommandException ex)
                {
                    var exceptionLogger = new ExceptionLogger(logger);
                    exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                    throw;
                }
            }

            Session["new_item_upload_errors"] = viewModel.ErrorItems;

            return View(viewModel);
        }
    }
}
