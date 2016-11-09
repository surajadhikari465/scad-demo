using Icon.Common.DataAccess;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Models;
using Icon.Web.Mvc.Attributes;
using Icon.Web.Mvc.Importers;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Controllers
{
    public class BulkCertificationAgencyImportController : Controller
    {
        private ILogger logger;
        private ISpreadsheetImporter<BulkImportCertificationAgencyModel> importer;
        private ICommandHandler<BulkImportCommand<BulkImportCertificationAgencyModel>> bulkImportCommand;

        public BulkCertificationAgencyImportController(ILogger logger,
            ISpreadsheetImporter<BulkImportCertificationAgencyModel> importer,
            ICommandHandler<BulkImportCommand<BulkImportCertificationAgencyModel>> bulkImportCommand)
        {
            this.logger = logger;
            this.importer = importer;
            this.bulkImportCommand = bulkImportCommand;
        }

        public ActionResult Index()
        {
            return View(new BulkImportViewModel<BulkImportCertificationAgencyModel>());
        }

        [WriteAccessAuthorize(IsJsonResult = false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Import([Bind(Include = "ExcelAttachment")] BulkImportViewModel<BulkImportCertificationAgencyModel> viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", viewModel);
            }

            importer.Workbook = Workbook.Load(viewModel.ExcelAttachment.InputStream);

            if (!importer.IsValidSpreadsheetType())
            {
                viewModel.ValidSpreadsheetType = false;
                return View("Index", viewModel);
            }

            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            viewModel.ErrorItems = importer.ErrorRows;
            viewModel.ValidItemsCount = importer.ValidRows.Count;

            if (viewModel.ValidItemsCount > 0)
            {
                BulkImportCommand<BulkImportCertificationAgencyModel> data = new BulkImportCommand<BulkImportCertificationAgencyModel>
                {
                    BulkImportData = importer.ValidRows,
                    UserName = User.Identity.Name
                };

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

            Session["certification_agency_import_errors"] = viewModel.ErrorItems;

            return View(viewModel);
        }
    }
}