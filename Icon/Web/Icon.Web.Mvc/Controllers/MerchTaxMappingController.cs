using Icon.Common.DataAccess;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Extensions;
using Icon.Web.Mvc.Attributes;
using Icon.Web.Mvc.Importers;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Controllers
{
    public class MerchTaxMappingController : Controller
    {
        private ILogger logger;
        private IQueryHandler<GetMerchTaxMappingsParameters, List<MerchTaxMappingModel>> getMerchTaxMappingQuery;
        private IManagerHandler<AddMerchTaxAssociationManager> addMerchTaxAssociationManagerHandler;
        private IManagerHandler<UpdateMerchTaxAssociationManager> updateMerchTaxAssociationManagerHandler;
        private IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQueryHandler;
        private ICommandHandler<DeleteMerchTaxMappingByIdCommand> deleteMerchTaxMappingByIdHandler;
        private ISpreadsheetImporter<BulkImportItemModel> itemSpreadsheetImporter;
        private ICommandHandler<BulkImportCommand<BulkImportItemModel>> bulkImportCommand;

        public MerchTaxMappingController(
            ILogger logger,
            IQueryHandler<GetMerchTaxMappingsParameters, List<MerchTaxMappingModel>> getMerchTaxMappingQuery,
            IManagerHandler<AddMerchTaxAssociationManager> addMerchTaxAssociationManagerHandler,
            IManagerHandler<UpdateMerchTaxAssociationManager> updateMerchTaxAssociationManagerHandler,
            IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQueryHandler,
            ICommandHandler<DeleteMerchTaxMappingByIdCommand> deleteMerchTaxMappingByIdHandler,
            ISpreadsheetImporter<BulkImportItemModel> itemSpreadsheetImporter,
            ICommandHandler<BulkImportCommand<BulkImportItemModel>> bulkImportCommand)
        {
            this.logger = logger;
            this.getMerchTaxMappingQuery = getMerchTaxMappingQuery;
            this.addMerchTaxAssociationManagerHandler = addMerchTaxAssociationManagerHandler;
            this.updateMerchTaxAssociationManagerHandler = updateMerchTaxAssociationManagerHandler;
            this.getHierarchyLineageQueryHandler = getHierarchyLineageQueryHandler;
            this.deleteMerchTaxMappingByIdHandler = deleteMerchTaxMappingByIdHandler;
            this.itemSpreadsheetImporter = itemSpreadsheetImporter;
            this.bulkImportCommand = bulkImportCommand;
        }

        // GET: MerchTaxMapping
        public ActionResult Index()
        {
            List<MerchTaxMappingModel> merchTaxMappingModelList = getMerchTaxMappingQuery.Search(new GetMerchTaxMappingsParameters());
            MerchTaxMappingIndexViewModel viewModel = BuildViewModel(merchTaxMappingModelList);

            return View(viewModel);
        }

        private MerchTaxMappingIndexViewModel BuildViewModel(List<MerchTaxMappingModel> merchTaxMappingModelList)
        {
            var viewModel = new MerchTaxMappingIndexViewModel { GridViewModel = new MerchTaxMappingGridViewModel() };

            viewModel.GridViewModel.MerchTaxMappingList = merchTaxMappingModelList
                .Select(pc => new MerchTaxMappingViewModel
                {
                    MerchandiseHierarchyClassId = pc.MerchandiseHierarchyClassId,
                    MerchandiseHierarchyClassName = pc.MerchandiseHierarchyClassName,
                    MerchandiseHierarchyClassLineage = pc.MerchandiseHierarchyClassLineage,
                    TaxHierarchyClassId = pc.TaxHierarchyClassId,
                    TaxHierarchyClassName = pc.TaxHierarchyClassName,
                    TaxHierarchyClassLineage = pc.TaxHierarchyClassLineage
                }).ToList();
            return viewModel;
        }

        // GET: MerchTaxMapping/Create
        public ActionResult Create()
        {
            var viewModel = new MerchTaxMappingCreateModel();

            HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());

            viewModel.MerchandiseHierarchyClasses = new SelectList(GetHierarchyLineage(hierarchyListModel.MerchandiseHierarchyList), "HierarchyClassId", "HierarchyClassLineage");
            viewModel.TaxHierarchyClasses = new SelectList(GetHierarchyLineage(hierarchyListModel.TaxHierarchyList), "HierarchyClassId", "HierarchyClassLineage");

            return View(viewModel);
        }

        // POST: MerchTaxMapping/Create
        [HttpPost]
        [WriteAccessAuthorize]
        public ActionResult Create(MerchTaxMappingCreateModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());
                viewModel.MerchandiseHierarchyClasses = new SelectList(GetHierarchyLineage(hierarchyListModel.MerchandiseHierarchyList), "HierarchyClassId", "HierarchyClassLineage");
                viewModel.TaxHierarchyClasses = new SelectList(GetHierarchyLineage(hierarchyListModel.TaxHierarchyList), "HierarchyClassId", "HierarchyClassLineage");

                return View(viewModel);
            }

            var addMerchTaxMappingManager = new AddMerchTaxAssociationManager
            {
                MerchandiseHierarchyClassId = viewModel.MerchandiseHierarchyClassId.HasValue ? viewModel.MerchandiseHierarchyClassId.Value : 0,
                TaxHierarchyClassId = viewModel.TaxHierarchyClassId
            };

            try
            {
                addMerchTaxAssociationManagerHandler.Execute(addMerchTaxMappingManager);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());

                ViewData["ErrorMessage"] = ex.Message;

                HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());

                viewModel.MerchandiseHierarchyClasses = new SelectList(GetHierarchyLineage(hierarchyListModel.MerchandiseHierarchyList), "HierarchyClassId", "HierarchyClassLineage");
                viewModel.TaxHierarchyClasses = new SelectList(GetHierarchyLineage(hierarchyListModel.TaxHierarchyList), "HierarchyClassId", "HierarchyClassLineage");

                return View(viewModel);
            }
        }

        // GET: MerchTaxMapping/Edit/
        public ActionResult Edit(int MerchandiseHierarchyClassId)
        {
            var viewModel = BuildMerchTaxMappingViewModel(MerchandiseHierarchyClassId);

            return View(viewModel);
        }

        // POST: MerchTaxMapping/Edit/
        [HttpPost]
        [WriteAccessAuthorize]
        public ActionResult Edit(MerchTaxMappingEditModel viewModel)
        {
            HierarchyClassListModel hierarchyListModel;

            if (!ModelState.IsValid)
            {
                hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());
                viewModel.TaxHierarchyClasses = new SelectList(GetHierarchyLineage(hierarchyListModel.TaxHierarchyList), "HierarchyClassId", "HierarchyClassLineage");

                return View(viewModel);
            }

            var updateMerchTaxMappingManager = new UpdateMerchTaxAssociationManager
            {
                MerchandiseHierarchyClassId = viewModel.MerchandiseHierarchyClassId.HasValue ? viewModel.MerchandiseHierarchyClassId.Value : 0,
                TaxHierarchyClassId = viewModel.TaxHierarchyClassId
            };

            try
            {
                updateMerchTaxAssociationManagerHandler.Execute(updateMerchTaxMappingManager);
                ViewData["SuccessMessage"] = "Merchandise/Tax mapping update was successful.";
            }
            catch (Exception ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = ex.Message;
            }

            hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());
            viewModel.TaxHierarchyClasses = new SelectList(GetHierarchyLineage(hierarchyListModel.TaxHierarchyList), "HierarchyClassId", "HierarchyClassLineage");

            return View(viewModel);
        }

        // GET: MerchTaxMapping/Delete/
        public ActionResult Delete(int MerchandiseHierarchyClassId)
        {
            List<MerchTaxMappingModel> merchTaxMappingModelList = getMerchTaxMappingQuery.Search(new GetMerchTaxMappingsParameters() { MerchandiseHierarchyClassId = MerchandiseHierarchyClassId });

            if (merchTaxMappingModelList != null)
            {
                var MerchTaxMappingViewModel = merchTaxMappingModelList
                    .Select(mt => new MerchTaxMappingViewModel
                    {
                        MerchandiseHierarchyClassId = mt.MerchandiseHierarchyClassId,
                        MerchandiseHierarchyClassName = mt.MerchandiseHierarchyClassName,
                        TaxHierarchyClassId = mt.TaxHierarchyClassId,
                        TaxHierarchyClassName = mt.TaxHierarchyClassName,
                        MerchandiseHierarchyClassLineage = mt.MerchandiseHierarchyClassLineage,
                        TaxHierarchyClassLineage = mt.TaxHierarchyClassLineage
                    }).Single();

                return View(MerchTaxMappingViewModel);
            }
            else
            {
                return View(new MerchTaxMappingViewModel());
            }
        }

        [HttpPost]
        [WriteAccessAuthorize]
        public ActionResult Delete(MerchTaxMappingViewModel viewModel)
        {
            try
            {
                deleteMerchTaxMappingByIdHandler.Execute(new DeleteMerchTaxMappingByIdCommand() { MerchHierarchyClassID = viewModel.MerchandiseHierarchyClassId.Value });
            }

            catch (CommandException exception)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(exception, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = "An error occurred while trying to delete this mapping.";
                return View(viewModel);
            }

            return RedirectToAction("Index");
        }

        // POST: /MerchTaxMapping/Import
        [HttpPost]
        [ValidateAntiForgeryToken]
        [WriteAccessAuthorize]
        public ActionResult Import([Bind(Include = "ExcelAttachment")] MerchTaxMappingIndexViewModel viewModel)
        {
            List<MerchTaxMappingModel> merchTaxMappingModelList = getMerchTaxMappingQuery.Search(new GetMerchTaxMappingsParameters());

            if (!ModelState.IsValid)
            {
                return View("Index", BuildViewModel(merchTaxMappingModelList));
            }

            itemSpreadsheetImporter.Workbook = Workbook.Load(viewModel.ExcelAttachment.InputStream);

            if (!itemSpreadsheetImporter.IsValidSpreadsheetType())
            {
                viewModel.ValidSpreadsheetType = false;
                ViewData["ErrorMessage"] = "Invalid spreadsheet type detected.  Please check column headers.";
                return View("Index", BuildViewModel(merchTaxMappingModelList));
            }

            itemSpreadsheetImporter.ConvertSpreadsheetToModel();
            itemSpreadsheetImporter.ValidateSpreadsheetData();

            viewModel.ErrorItems = itemSpreadsheetImporter.ErrorRows;
            viewModel.ValidItemsCount = itemSpreadsheetImporter.ValidRows.Count;

            if (viewModel.ValidItemsCount > 0)
            {
                BulkImportCommand<BulkImportItemModel> data = new BulkImportCommand<BulkImportItemModel> { BulkImportData = itemSpreadsheetImporter.ValidRows, UserName = User.Identity.Name };

                try
                {
                    bulkImportCommand.Execute(data);
                    ViewData["SuccessMessage"] = "Items imported successfully.";
                }
                catch (CommandException ex)
                {
                    var exceptionLogger = new ExceptionLogger(logger);
                    exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                    ViewData["ErrorMessage"] = String.Format("An error occurred: {0}", ex.Message);
                }
            }

            if (viewModel.ErrorItems.Count > 0)
            {
                var errors = viewModel.ErrorItems.Select(e => new DefaultTaxClassMismatchExportModel
                {
                    ScanCode = e.ScanCode,
                    Brand = e.brandNameDisplayOnly,
                    ProductDescription = e.productionDescriptionDisplayOnly,
                    MerchandiseLineage = e.merchandiseClassDisplayOnly,
                    DefaultTaxClass = e.DefaultTaxClass,
                    TaxClassOverride = e.TaxLineage,
                    Error = e.Error
                }).ToList();

                Session["tax_corrections_upload_errors"] = errors;

                return View("ImportErrors", errors);
            }

            return View("Index", BuildViewModel(merchTaxMappingModelList));
        }

        private MerchTaxMappingEditModel BuildMerchTaxMappingViewModel(int hierarchyClassId)
        {
            List<MerchTaxMappingModel> merchTaxMappingModelList = getMerchTaxMappingQuery.Search(new GetMerchTaxMappingsParameters() { MerchandiseHierarchyClassId = hierarchyClassId });

            var MerchTaxMappingViewModel = merchTaxMappingModelList
                .Select(mt => new MerchTaxMappingEditModel
                {
                    MerchandiseHierarchyClassId = mt.MerchandiseHierarchyClassId,
                    MerchandiseHierarchyClassName = mt.MerchandiseHierarchyClassName,
                    MerchandiseHierarchyClassLineage = mt.MerchandiseHierarchyClassLineage,
                    TaxHierarchyClassId = mt.TaxHierarchyClassId,
                    TaxHierarchyClassName = mt.TaxHierarchyClassName,
                    TaxHierarchyClassLineage = mt.TaxHierarchyClassLineage
                }).Single();

            HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());
            MerchTaxMappingViewModel.TaxHierarchyClasses = new SelectList(GetHierarchyLineage(hierarchyListModel.TaxHierarchyList), "HierarchyClassId", "HierarchyClassLineage");

            return MerchTaxMappingViewModel;
        }

        private List<HierarchyClassViewModel> GetHierarchyLineage(List<HierarchyClassModel> hierarchyList)
        {
            List<HierarchyClassViewModel> hierarchyClasses = hierarchyList.HierarchyClassForCombo();
            return hierarchyClasses;
        }
    }
}
