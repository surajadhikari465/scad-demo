﻿using DevTrends.MvcDonutCaching;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Attributes;
using Icon.Web.Mvc.Models;
using Infragistics.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Icon.Web.Controllers
{
    public class PluCategoryController : Controller
    {
        private ILogger logger;
        private IQueryHandler<GetPluCategoriesParameters, List<PLUCategory>> getPluCategoriesQuery;
        private IManagerHandler<AddPluCategoryManager> addPluCategoryManager;
        private IManagerHandler<UpdatePluCategoryManager> updatePluCategoryManager;
        private IQueryHandler<GetPluCategoryByIdParameters, PLUCategory> getPluCageroyByIdQuery;
        private ICommandHandler<DeletePluCategoryByIdCommand> deletePluCategoryByIdCommand;
        private readonly IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>> getBarcodeTypesQueryHandler;

        public PluCategoryController(
            ILogger logger,
            IQueryHandler<GetPluCategoriesParameters, List<PLUCategory>> getPluCategoriesQuery,
            IManagerHandler<AddPluCategoryManager> addPluCategoryManager,
            IManagerHandler<UpdatePluCategoryManager> updatePluCategoryManager,
            IQueryHandler<GetPluCategoryByIdParameters, PLUCategory> getPluCageroyByIdQuery,
            ICommandHandler<DeletePluCategoryByIdCommand> deletePluCategoryByIdCommand,
            IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>> getBarcodeTypesQueryHandler)
        {
            this.logger = logger;
            this.getPluCategoriesQuery = getPluCategoriesQuery;
            this.addPluCategoryManager = addPluCategoryManager;
            this.updatePluCategoryManager = updatePluCategoryManager;
            this.getPluCageroyByIdQuery = getPluCageroyByIdQuery;
            this.deletePluCategoryByIdCommand = deletePluCategoryByIdCommand;
            this.getBarcodeTypesQueryHandler = getBarcodeTypesQueryHandler;
        }

        //
        // GET: /PluCategory/
        [DonutOutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index()
        {
            // Get PLU categoty range data from Database
            var barcodeTypesList = getBarcodeTypesQueryHandler.Search(new GetBarcodeTypeParameters { });

            // Populate ViewModel
            BarcodeTypeGridViewModel viewModel = new BarcodeTypeGridViewModel();

            viewModel.BarcodeTypes = barcodeTypesList
                .Select(bt => new BarcodeTypeViewModel
                    {
                        BarcodeTypeId = bt.BarcodeTypeId,
                        BarcodeType = bt.BarcodeType,
                        BeginRange = bt.BeginRange?.ToString(),
                        EndRange = bt.EndRange?.ToString(),
                        ScalePlu = bt.ScalePlu
                    });

            return View(viewModel);
        }

        //
        // GET: /PluCategory/Create/
        [WriteAccessAuthorize]
        [RedirectFilterAttribute]
        public ActionResult Create()
        {
            return View(new BarcodeTypeViewModel());
        }

        //
        // POST: /PluCategory/Create
        [WriteAccessAuthorize]
        [HttpPost]
        [RedirectFilterAttribute]
        public ActionResult Create(BarcodeTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                // Add new PLU category
                AddPluCategoryManager manager = new AddPluCategoryManager
                {
                    PluCategoryName = viewModel.BarcodeType,
                    BeginRange = viewModel.BeginRange,
                    EndRange = viewModel.EndRange
                };

                addPluCategoryManager.Execute(manager);
                string successMessage = String.Format("Successfully added category {0}.", viewModel.BarcodeType);

                ViewData["SuccessMessage"] = successMessage;
                ModelState.Clear();
                return View();
            }
            catch (CommandException exception)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(exception, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = "Error: " + exception.Message;
                return View(viewModel);
            }
        }

        //
        // GET: /PluCategory/Delete/
        [WriteAccessAuthorize]
        [RedirectFilterAttribute]
        public ActionResult Delete(int PluCategoryId)
        {
            PLUCategory plucategory = getPluCageroyByIdQuery.Search(new GetPluCategoryByIdParameters() { PluCategoryID = PluCategoryId });
            if (plucategory != null)
            {
                return View(new BarcodeTypeViewModel
                        {
                            BarcodeTypeId = plucategory.PluCategoryID,
                            BarcodeType = plucategory.PluCategoryName,
                            BeginRange = plucategory.BeginRange.ToString(),
                            EndRange = plucategory.EndRange.ToString()
                        });
            }
            else
            {
                return View(new BarcodeTypeViewModel());
            }
        }

        //
        // POST: /PluCategory/Create
        [WriteAccessAuthorize]
        [HttpPost]
        [RedirectFilterAttribute]
        public ActionResult Delete(PluCategoryViewModel viewModel)
        {
            try
            {
                deletePluCategoryByIdCommand.Execute(new DeletePluCategoryByIdCommand() { PluCategoryID = viewModel.PluCategoryId });
            }

            catch (CommandException exception)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(exception, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = "Error: There was a problem with applying this delete on the database.";
                return View(viewModel);
            }
            
            return RedirectToAction("Index");
        }

        // Processes the Update and Add for each row in the Infragistics igGrid
        [WriteAccessAuthorize(IsJsonResult = true)]
        [RedirectFilterAttribute]
        public ActionResult SaveChangesInGrid()
        {
            ViewData["GenerateCompactJSONResponse"] = false;
            GridModel m = new GridModel();
            List<Transaction<BarcodeTypeViewModel>> transactions = m.LoadTransactions<BarcodeTypeViewModel>(HttpContext.Request.Form["ig_transactions"]);

            if (!transactions.Any())
            {
                return Json(new { Success = false, Error = "No new values were specified for the PLU category." });
            }

            foreach (Transaction<BarcodeTypeViewModel> item in transactions)
            {
                try
                {
                    ValidateRange(item.row.BeginRange, item.row.EndRange);

                    UpdatePluCategoryManager command = new UpdatePluCategoryManager
                    {
                        PluCategoryId = item.row.BarcodeTypeId,
                        PluCategoryName = item.row.BarcodeType,
                        BeginRange = item.row.BeginRange,
                        EndRange = item.row.EndRange
                    };

                    updatePluCategoryManager.Execute(command);
                }
                catch (CommandException exception)
                {
                    var exceptionLogger = new ExceptionLogger(logger);
                    exceptionLogger.LogException(exception, this.GetType(), MethodBase.GetCurrentMethod());
                    return Json(new { Success = false, Error = exception.Message });
                }
            }

            return Json(new { Success = true });
        }

        private void ValidateRange(string beginRange, string endRange)
        {
            long startRangeLong = 0;
            long endRangeLong = 0;

            try
            {
                startRangeLong = Int64.Parse(beginRange);
                endRangeLong = Int64.Parse(endRange);
            }
            catch (Exception)
            {
                throw new CommandException("Category Range exception: Please enter a numeric value.");
            }
            if (startRangeLong >= endRangeLong)
            {
                throw new CommandException("Category Range exception: PLU Category start value must be less than end value.");
            }
            else
            {
                if (beginRange.Length > 6)
                {
                    startRangeLong = Int64.Parse(beginRange.Substring(0, 6));
                }
                if(endRange.Length > 6)
                {
                   endRangeLong = Int64.Parse(endRange.Substring(0,6));
                }
                if ((endRangeLong - startRangeLong) > 40000)
                {
                    throw new CommandException("Category Range exception: PLU Category start and end range difference should be no more than 40,000.");
                }
            }
        }
    }
}
