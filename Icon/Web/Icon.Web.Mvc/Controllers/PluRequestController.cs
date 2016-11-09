using AutoMapper;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.Common.Validators;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Extensions;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Excel;
using Infragistics.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using System.Reflection;
using System.Web.Mvc;
using Icon.Common.DataAccess;
using Icon.Web.Mvc.Attributes;

namespace Icon.Web.Controllers
{
    public class PluRequestController : Controller
    {
        private ILogger logger;
        private IQueryHandler<GetPluRequestByIdParameters, PLURequest> getPluRequestByIdQueryHandler;
        private IManagerHandler<UpdatePluRequestManager> updatePluRequestManagerHandler;
        private IManagerHandler<AddPluRequestManager> addPluRequestManagerHandler;
        private IQueryHandler<GetPluRequestsParameters, List<PLURequest>> getPluRequestsBySearchQueryHandler;
        private IQueryHandler<GetPluRequestChangeHistoryByIdParameters, List<PLURequestChangeHistory>> getPluRequestChangeHistoryByIdQueryHandle;
        private IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQueryHandler;
        private IExcelExporterService excelExporterService;

        public PluRequestController(
            ILogger logger,
            IQueryHandler<GetPluRequestByIdParameters, PLURequest> getPluRequestByIdQueryHandler,
            IManagerHandler<UpdatePluRequestManager> updatePluRequestManagerHandler,
            IManagerHandler<AddPluRequestManager> addPluRequestManagerHandler,
            IQueryHandler<GetPluRequestsParameters, List<PLURequest>> getPluRequestsBySearchQueryHandler,
            IQueryHandler<GetPluRequestChangeHistoryByIdParameters, List<PLURequestChangeHistory>> getPluRequestChangeHistoryByIdQueryHandle,
            IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQueryHandler,
            IExcelExporterService excelExporterService)
        {
            this.logger = logger;
            this.getPluRequestByIdQueryHandler = getPluRequestByIdQueryHandler;
            this.updatePluRequestManagerHandler = updatePluRequestManagerHandler;
            this.addPluRequestManagerHandler = addPluRequestManagerHandler;
            this.getPluRequestsBySearchQueryHandler = getPluRequestsBySearchQueryHandler;
            this.getPluRequestChangeHistoryByIdQueryHandle = getPluRequestChangeHistoryByIdQueryHandle;
            this.getHierarchyLineageQueryHandler = getHierarchyLineageQueryHandler;
            this.excelExporterService = excelExporterService;
        }

        //
        // GET: /PluRequest/
        public ActionResult Index()
        {
            var viewModel = new PluRequestSearchViewModel
            {
                RetailUoms = GetRetailUomSelectList(String.Empty, includeInitialBlank: true)
            };

            ViewData["CreateItemMessage"] = TempData["CreateItemMessage"];

            return View(viewModel);
        }

        //
        // GET: /PluRequest/Search
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Search(PluRequestSearchViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                viewModel.RetailUoms = GetRetailUomSelectList(String.Empty, includeInitialBlank: true);
                return PartialView("_PluRequestOptionsPartial", viewModel);
            }

            // Execute the search.
            GetPluRequestsParameters searchParameters = new GetPluRequestsParameters
            {

                requestStatus = viewModel.Status.Single(s => s.Value == viewModel.SelectedStatusId.ToString()).Text.ToEnum<PluRequestStatus>()
            };

            List<PLURequest> pLURequestList = new List<PLURequest>();
            pLURequestList = getPluRequestsBySearchQueryHandler.Search(searchParameters);

            HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());
            viewModel.BrandHierarchyClasses = GetHierarchyLineage(hierarchyListModel.BrandHierarchyList);
            viewModel.MerchandiseHierarchyClasses = GetHierarchyLineage(hierarchyListModel.MerchandiseHierarchyList);
            viewModel.NationalHierarchyClasses = GetHierarchyLineage(hierarchyListModel.NationalHierarchyList);
            viewModel.FinanacialHierarchyClasses = GetHierarchyLineage(hierarchyListModel.FinancialHierarchyList);

            List<HierarchyClassViewModel> iconBrands = hierarchyListModel.BrandHierarchyList.Select(b => new HierarchyClassViewModel(b)).DistinctBy(hc => hc.HierarchyClassName).ToList();

            // To make it easier to work in the View, project the PluRequest objects to PluRequestViewModel objects.
            viewModel.PluRequests = pLURequestList.ToViewModels();

           viewModel.PluRequests =  pLURequestList.Select(item => new PluRequestViewModel(item)
            {

                IsNewBrand = !iconBrands.Exists(brand => item.brandName.Trim().ToLower() == brand.HierarchyClassName.Trim().ToLower())
            }).ToList();

            viewModel.RetailUoms = GetRetailUomSelectList(String.Empty, includeInitialBlank: true);

           
            return PartialView("_PluRequestSearchResultsPartial", viewModel);
        }

        public ActionResult BrandAutocomplete(string term)
        {
            HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());
            List<HierarchyClassModel> brandHierarchyClasses = hierarchyListModel.BrandHierarchyList;

            var brandMatches = brandHierarchyClasses
                .Where(hc => hc.HierarchyClassName.ToLower().Contains(term.ToLower()))
                .OrderBy(hc => hc.HierarchyClassName)
                .Select(hc => hc.HierarchyClassName)
                .ToArray();

            return Json(brandMatches, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MerchandiseAutocomplete(string term)
        {
            HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());
            List<HierarchyClassModel> merchHierarchyClasses = hierarchyListModel.MerchandiseHierarchyList;

            var merchandiseMatches = merchHierarchyClasses
                .Where(hc => hc.HierarchyClassName.ToLower().Contains(term.ToLower()))
                .OrderBy(hc => hc.HierarchyClassName)
                .Select(hc => hc.HierarchyClassName).ToArray();

            return Json(merchandiseMatches, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MerchandiseWithSubTeamAutocomplete(string term)
        {
            HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());
            List<HierarchyClassModel> merchHierarchyClasses = hierarchyListModel.MerchandiseHierarchyList;

            var merchandiseMatches = merchHierarchyClasses
                .Where(hc => hc.HierarchyClassName.ToLower().Contains(term.ToLower()))
                .OrderBy(hc => hc.HierarchyClassName)
                .Select(hc => new { label = hc.HierarchyLineage.Split('|').LastOrDefault(), valueID = hc.HierarchyClassId }).ToArray();

            return Json(merchandiseMatches, JsonRequestBehavior.AllowGet);
        }


        public ActionResult TaxAutocomplete(string term)
        {
            HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());
            List<HierarchyClassModel> taxHierarchyClasses = hierarchyListModel.TaxHierarchyList;

            var taxMatches = taxHierarchyClasses
                .Where(hc => hc.HierarchyLineage.ToLower().Contains(term.ToLower()))
                .OrderBy(hc => hc.HierarchyLineage)
                .Select(hc => hc.HierarchyLineage).ToArray();

            return Json(taxMatches, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SubTeamAutocomplete(string term)
        {
          HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());
            List<HierarchyClassModel> financialHierarchyClasses = hierarchyListModel.FinancialHierarchyList;

            var subTeamMatches = financialHierarchyClasses
                .Where(hc => hc.HierarchyLineage.ToLower().Contains(term.ToLower()))
                .OrderBy(hc => hc.HierarchyLineage)
                .Select(hc => new { label = hc.HierarchyLineage, valueID = hc.HierarchyClassId }).ToArray();

            return Json(subTeamMatches, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NationalAutocomplete(string term)
        {
            HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());
            List<HierarchyClassModel> nationalHierarchyClasses = hierarchyListModel.NationalHierarchyList;

            var taxMatches = nationalHierarchyClasses
                .Where(hc => hc.HierarchyClassName.ToLower().Contains(term.ToLower()))
                .OrderBy(hc => hc.HierarchyClassName)
                .Select(hc => hc.HierarchyClassName).ToArray();

            return Json(taxMatches, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NationalAutocompleteWithClassCode(string term)
        {
            HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());
            List<HierarchyClassModel> nationalHierarchyClasses = hierarchyListModel.NationalHierarchyList;

            var taxMatches = nationalHierarchyClasses
                .Where(hc => hc.HierarchyClassName.ToLower().Contains(term.ToLower()))
                .OrderBy(hc => hc.HierarchyClassName)
                .Select(hc => new { label = hc.HierarchyLineage, valueID = hc.HierarchyClassId }).ToArray();

            return Json(taxMatches, JsonRequestBehavior.AllowGet);
        }

        // GET: /PluRequest/Edit
        public ActionResult Edit(int id)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return RedirectToAction("Index");
            }

            var viewModel = BuildPluRequestEditViewModel(id);
            
            if (TempData["MessageSendFailed"] != null)
            {
                ViewData["UpdateFailed"] = TempData["MessageSendFailed"];
            }
            return View(viewModel);
        }

        // POST: /PluRequest/Edit
        [WriteAccessAuthorize(IsJsonResult = true)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PluRequestEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(BuildPluRequestEditViewModel(viewModel.PluRequestId));
            }

            var manager = new UpdatePluRequestManager
            {
                PluRequestId = viewModel.PluRequestId,
                NationalPlu = viewModel.NationalPLU,
                BrandName = viewModel.BrandName,
                ProductDescription = viewModel.ProductDescription,
                PosDescription = viewModel.PosDescription,
                PackageUnit = int.Parse(viewModel.PackageUnit),
                RetailSize = decimal.Parse(viewModel.RetailSize),
                RetailUom = viewModel.RetailUom,
                FinancialHierarchyClassId = viewModel.FinancialHierarchyClassId.HasValue ? viewModel.FinancialHierarchyClassId.Value : -1,
                UserName = User.Identity.Name,
                NationalHierarchyClassId = viewModel.NationalHierarchyClassId.HasValue ? viewModel.NationalHierarchyClassId.Value : -1,
                MerchandiseHierarchyClassId = viewModel.MerchandiseHierarchyClassId,
                RequestStatus = viewModel.RequestStatus,
                Notes = viewModel.Note
            };

            try
            {
                updatePluRequestManagerHandler.Execute(manager);
                ViewData["UpdateSuccess"] = "Update successful.";
            }
            catch (Exception ex)
            {
                string errorDetails = ex.Message;
                ViewData["UpdateFailed"] = errorDetails;
            }

            // Rebuild lists and appropriate pieces of view.
            return View(BuildPluRequestEditViewModel(viewModel.PluRequestId));
        }

        [WriteAccessAuthorize(IsJsonResult = true)]
        public ActionResult SaveChangesInGrid()
        {
            ViewData["GenerateCompactJSONResponse"] = false;

            List<Transaction<PluRequestViewModel>> transactions = new GridModel()
                .LoadTransactions<PluRequestViewModel>(HttpContext.Request.Form["ig_transactions"]);

            if (!transactions.Any())
            {
                return Json(new { Success = false, Error = "No new values were specified for the request." });
            }

            PluRequestViewModel viewModel= transactions.First().row;

            var manager = new UpdatePluRequestManager
            {
                PluRequestId = viewModel.PluRequestID,
                NationalPlu = viewModel.NationalPLU,
                BrandName = viewModel.BrandName,
                ProductDescription = viewModel.ItemDescription,
                PosDescription = viewModel.PosDescription,
                PackageUnit = viewModel.PackageUnit,
                RetailSize = viewModel.RetailSize,
                RetailUom = viewModel.RetailUom,
                MerchandiseHierarchyClassId = viewModel.MerchandiseClassID,
                UserName = User.Identity.Name,
                NationalHierarchyClassId = viewModel.NationalClassID,
                RequestStatus = viewModel.RequestStatus,
                scanCodeTypeID = viewModel.ScanCodeTypeID,
                FinancialHierarchyClassId = viewModel.FinancialClassID
               
            };


            try
            {
                updatePluRequestManagerHandler.Execute(manager);
            }
            catch (CommandException exception)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(exception, this.GetType(), MethodBase.GetCurrentMethod());

                return Json(new { Success = false, Error = exception.Message });
            }
            catch (ArgumentException exception)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(exception, this.GetType(), MethodBase.GetCurrentMethod());

                return Json(new { Success = false, Error = exception.Message });
            }

            return Json(new { Success = true });
        }
       
        [HttpGet]
        public ActionResult Create()
        {
            var viewModel = new PluRequestCreateViewModel();
            HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());
            viewModel.NationalHierarchyClasses = new SelectList(GetHierarchyLineage(hierarchyListModel.NationalHierarchyList), "HierarchyClassId", "HierarchyClassLineage");
            viewModel.FinanacialHierarchyClasses = new SelectList(GetHierarchyLineage(hierarchyListModel.FinancialHierarchyList), "HierarchyClassId", "HierarchyClassLineage");

            viewModel.RetailUoms = GetRetailUomSelectList(String.Empty, true);

            return View(viewModel);
        }

        [WriteAccessAuthorize(IsJsonResult = false)]
        [HttpPost]
        public ActionResult Create(PluRequestCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());
                viewModel.NationalHierarchyClasses = new SelectList(GetHierarchyLineage(hierarchyListModel.NationalHierarchyList), "HierarchyClassId", "HierarchyClassLineage");
                viewModel.FinanacialHierarchyClasses = new SelectList(GetHierarchyLineage(hierarchyListModel.FinancialHierarchyList), "HierarchyClassId", "HierarchyClassLineage");
                viewModel.RetailUoms = GetRetailUomSelectList(String.Empty, true);

                return View(viewModel);
            }

            try
            {
               var manager = new AddPluRequestManager
                {                    
                    NationalPlu = viewModel.NationalPLU,
                    BrandName = viewModel.BrandName,
                    ProductDescription = viewModel.ProductDescription,
                    PosDescription = viewModel.PosDescription,
                    PackageUnit = int.Parse(viewModel.PackageUnit),
                    RetailSize = decimal.Parse(viewModel.RetailSize),
                    RetailUom = viewModel.RetailUom,
                    FinancialHierarchyClassId = viewModel.FinancialHierarchyClassId,
                    Notes = viewModel.Notes,
                    UserName = User.Identity.Name,
                    NationalHierarchyClassId = viewModel.NationalHierarchyClassId.HasValue ? viewModel.NationalHierarchyClassId.Value : -1,
                    PluType = viewModel.PluType
                };
               
                addPluRequestManagerHandler.Execute(manager);
            }
            catch (Exception ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());

                ViewData["ErrorMessage"] = ex.Message;

                HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());
                viewModel.FinanacialHierarchyClasses = new SelectList(GetHierarchyLineage(hierarchyListModel.FinancialHierarchyList), "HierarchyClassId", "HierarchyClassLineage");
                viewModel.NationalHierarchyClasses = new SelectList(GetHierarchyLineage(hierarchyListModel.NationalHierarchyList), "HierarchyClassId", "HierarchyClassLineage");
                viewModel.RetailUoms = GetRetailUomSelectList(String.Empty, true);

                return View(viewModel);
            }

            TempData["CreateItemMessage"] = "Created Request successfully.";

            return RedirectToAction("Create");
        }

        private PluRequestEditViewModel BuildPluRequestEditViewModel(int PluRequestId)
        {
            var parameters = new GetPluRequestByIdParameters
            {
                PluRequestId = PluRequestId
            };

            PLURequest pLURequest = getPluRequestByIdQueryHandler.Search(parameters);

            // Build view from selected item.
            PluRequestEditViewModel viewModel = new PluRequestEditViewModel(pLURequest);

            var changes = getPluRequestChangeHistoryByIdQueryHandle.Search(new GetPluRequestChangeHistoryByIdParameters() { PluRequestId = PluRequestId });
            viewModel.PluRequestChangeHistory = changes.ToViewModels();
            // Populate selection lists in view.
            viewModel.RetailUoms = GetRetailUomSelectList(viewModel.RetailUom, includeInitialBlank: true);
            HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());
            viewModel.FinanacialHierarchyClasses = new SelectList(GetHierarchyLineage(hierarchyListModel.FinancialHierarchyList), "HierarchyClassId", "HierarchyClassLineage");
            viewModel.NationalHierarchyClasses = new SelectList(GetHierarchyLineage(hierarchyListModel.NationalHierarchyList), "HierarchyClassId", "HierarchyClassLineage");

            return viewModel;
        }

        private SelectList GetRetailUomSelectList(string selectedUom, bool includeInitialBlank)
        {
            var uoms = UomCodes.ByName.Values.ToList();

            if (includeInitialBlank)
            {
                // Insert empty value at the beginning of the list to allow for an un-selected state.
                uoms.Insert(0, String.Empty);
            }

            return new SelectList(uoms, selectedUom);
        }

        private List<HierarchyClassViewModel> GetHierarchyLineage(List<HierarchyClassModel> hierarchyList)
        {
            List<HierarchyClassViewModel> hierarchyClasses = hierarchyList.HierarchyClassForCombo();
            return hierarchyClasses;
        }

        public void Export(PluRequestSearchViewModel viewModel)
        {
            
             // Execute the search.
            GetPluRequestsParameters searchParameters = new GetPluRequestsParameters
            {

                requestStatus = viewModel.Status.Single(s => s.Value == viewModel.SelectedStatusId.ToString()).Text.ToEnum<PluRequestStatus>()
            };

            List<PLURequest> itemsList = getPluRequestsBySearchQueryHandler.Search(searchParameters);

            // To make it easier to work in the View, project the Item objects to ItemViewModel objects.
            List<PluRequestViewModel> plueRequests = itemsList.Select(p => new PluRequestViewModel(p)).ToList();


            var pluRequestExporter = excelExporterService.GetPluRequestExporter();
            pluRequestExporter.ExportData = plueRequests;
            pluRequestExporter.Export();

            ExcelHelper.SendForDownload(Response, pluRequestExporter.ExportModel.ExcelWorkbook, pluRequestExporter.ExportModel.ExcelWorkbook.CurrentFormat, "PLURequest");
        }
        
    }
}