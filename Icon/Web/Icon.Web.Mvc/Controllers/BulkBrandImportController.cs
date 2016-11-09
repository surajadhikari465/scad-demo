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

namespace Icon.Web.Mvc.Controllers
{
    public class BulkBrandImportController : Controller
    {
        private ILogger logger;
        private ISpreadsheetImporter<BulkImportBrandModel> importer;
        private ICommandHandler<BulkImportCommand<BulkImportBrandModel>> bulkImportCommand;

        public BulkBrandImportController(
            ILogger logger,
            ISpreadsheetImporter<BulkImportBrandModel> importer,
            ICommandHandler<BulkImportCommand<BulkImportBrandModel>> bulkImportCommand)
        {
            this.logger = logger;
            this.importer = importer;
            this.bulkImportCommand = bulkImportCommand;
        }

        public ActionResult Index()
        {
            return View(new BulkImportViewModel<BulkImportBrandModel>());
        }

        [WriteAccessAuthorize(IsJsonResult = false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Import([Bind(Include = "ExcelAttachment")] BulkImportViewModel<BulkImportBrandModel> viewModel)
        {
            if(!ModelState.IsValid)
            {
                return View("Index", viewModel);
            }

            importer.Workbook = Workbook.Load(viewModel.ExcelAttachment.InputStream);

            if(!importer.IsValidSpreadsheetType())
            {
                viewModel.ValidSpreadsheetType = false;
                return View("Index", viewModel);
            }

            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            viewModel.ErrorItems = importer.ErrorRows;
            viewModel.ValidItemsCount = importer.ValidRows.Count;

            if(viewModel.ValidItemsCount > 0)
            {
                BulkImportCommand<BulkImportBrandModel> data = new BulkImportCommand<BulkImportBrandModel>
                {
                    BulkImportData = importer.ValidRows,
                    UserName = User.Identity.Name
                };

                try
                {
                    bulkImportCommand.Execute(data);
                }
                catch(CommandException ex)
                {
                    var exceptionLogger = new ExceptionLogger(logger);
                    exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                    throw;
                }
            }

            Session["grid_search_results"] = viewModel.ErrorItems;

            return View(viewModel);
        }
    }
}