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
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;

namespace Icon.Web.Controllers
{
    public class BulkPluMappingController : Controller
    {
        private ILogger logger;
        private IPluSpreadsheetImporter importer;
        private ICommandHandler<BulkImportCommand<BulkImportPluModel>> command;

        public BulkPluMappingController(ILogger logger, IPluSpreadsheetImporter importer, ICommandHandler<BulkImportCommand<BulkImportPluModel>> command)
        {
            this.logger = logger;
            this.importer = importer;
            this.command = command;
        }

        //
        // GET: /BulkPluMapping/
        public ActionResult Index()
        {
            return View(new BulkPluMappingViewModel());
        }

        // POST: /BulkPluMapping/Import
        [WriteAccessAuthorize(IsJsonResult = false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Import([Bind(Include = "ExcelAttachment")] BulkPluMappingViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", viewModel);
            }

            importer.Workbook = Workbook.Load(viewModel.ExcelAttachment.InputStream);

            if (!importer.ValidSpreadsheetType())
            {
                viewModel.ValidSpreadsheetType = false;
                return View("Index", viewModel);
            };

            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            viewModel.ErrorItems = importer.ErrorRows;
            viewModel.RemapRows = importer.Remappings;
            viewModel.ValidItemsCount = importer.ValidRows.Count;

            if (viewModel.ValidItemsCount > 0)
            {
                BulkImportCommand<BulkImportPluModel> data = new BulkImportCommand<BulkImportPluModel> { BulkImportData = importer.ValidRows, UserName = User.Identity.Name };
                
                try
                {
                    command.Execute(data);
                }
                catch (CommandException ex)
                {
                    var exceptionLogger = new ExceptionLogger(logger);
                    exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                    throw;
                }
            }

            Session["grid_search_results"] = viewModel.ErrorItems;

            return View(viewModel);
        }

        //
        // POST: /BulkPluMapping/Remapping
        [HttpPost]
        public ActionResult Remapping(List<BulkImportPluRemapModel> remapRows)
        {
            importer.Remappings = remapRows;

            importer.ConvertRemappingsToModel();

            if (importer.ValidRows.Count > 0)
            {
                foreach (var row in importer.ValidRows)
                {
                    List<BulkImportPluModel> pluImportData = new List<BulkImportPluModel> { row };
                    BulkImportCommand<BulkImportPluModel> data = new BulkImportCommand<BulkImportPluModel> { BulkImportData = pluImportData, UserName = User.Identity.Name };

                    try
                    {
                        command.Execute(data);
                    }
                    catch (CommandException ex)
                    {
                        var exceptionLogger = new ExceptionLogger(logger);
                        exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                        throw;
                    }
                }
            }
            
            return Json("Mappings have been overwritten successfully.");
        }
    }
}