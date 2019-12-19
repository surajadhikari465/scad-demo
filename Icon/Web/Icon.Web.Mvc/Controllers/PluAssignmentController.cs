using DevTrends.MvcDonutCaching;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Attributes;
using Icon.Web.Mvc.Excel;
using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Extensions;
using Icon.Web.Mvc.Models;
using Infragistics.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Icon.Web.Controllers
{
    [RedirectFilterAttribute]
    public class PluAssignmentController : Controller
    {
        private ILogger logger;
        private IManagerHandler<AddItemManager> addItemManagerHandler;
        private IQueryHandler<GetAvailablePlusByCategoryParameters, List<IRMAItem>> getAvailablePlusByCategoryQueryHandler;
        private IQueryHandler<GetAvailableScanCodesByCategoryParameters, List<IRMAItem>> getAvailableScanCodesByCategoryQueryHandler;
        private IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQueryHandler;
        private IQueryHandler<GetPluCategoriesParameters, List<PLUCategory>> getPluCategoriesQuery;
        private IExcelExporterService excelExporterService;
        private IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> hieararchyQueryHandler;

        public PluAssignmentController(
            ILogger logger,
            IManagerHandler<AddItemManager> addItemManagerHandler,
            IQueryHandler<GetAvailablePlusByCategoryParameters, List<IRMAItem>> getAvailablePlusByCategoryQueryHandler,
            IQueryHandler<GetAvailableScanCodesByCategoryParameters, List<IRMAItem>> getAvailableScanCodesByCategoryQueryHandler,
            IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQueryHandler,
            IQueryHandler<GetPluCategoriesParameters, List<PLUCategory>> getPluCategoriesQuery,
            IExcelExporterService excelExporterService,
            IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> hieararchyQueryHandler)
        {
            this.logger = logger;
            this.addItemManagerHandler = addItemManagerHandler;
            this.getAvailablePlusByCategoryQueryHandler = getAvailablePlusByCategoryQueryHandler;
            this.getAvailableScanCodesByCategoryQueryHandler = getAvailableScanCodesByCategoryQueryHandler;
            this.getHierarchyLineageQueryHandler = getHierarchyLineageQueryHandler;
            this.getPluCategoriesQuery = getPluCategoriesQuery;
            this.excelExporterService = excelExporterService;
            this.hieararchyQueryHandler = hieararchyQueryHandler;
        }

        //
        // GET: /Item/
        public ActionResult Index()
        {
            var viewModel = new PluAssignmentSearchViewModel
            {
                PluCategories = GetPluCategorySelectList()
            };

            return View(viewModel);
        }

        //
        // GET: /Item/Search
        [DonutOutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Search(PluAssignmentSearchViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                viewModel.PluCategories = GetPluCategorySelectList();
                return PartialView("_PluSearchOptionsPartial", viewModel);
            }

            // Execute the search.
            List<IRMAItem> itemsList = null;
            bool isPluCategory = getPluCategoriesQuery.Search(new GetPluCategoriesParameters()).Single(c => c.PluCategoryID == viewModel.SelectedPluCategoryId).BeginRange.IsPosOrScalePlu();
            int maxPlus = string.IsNullOrEmpty(viewModel.MaxPlus) ? 0 : Int32.Parse(viewModel.MaxPlus);

            if(isPluCategory)
            {
                GetAvailablePlusByCategoryParameters searchParameters = new GetAvailablePlusByCategoryParameters
                {
                    PluCategoryId = viewModel.SelectedPluCategoryId,
                    MaxPlus = maxPlus                
                };
                itemsList = getAvailablePlusByCategoryQueryHandler.Search(searchParameters);
            }
            else
            {
                itemsList = getAvailableScanCodesByCategoryQueryHandler.Search(new GetAvailableScanCodesByCategoryParameters
                    {
                        CategoryId = viewModel.SelectedPluCategoryId,
                        MaxScanCodes = maxPlus
                    });
            }

            // To make it easier to work in the View, project the Item objects to ItemViewModel objects.
            viewModel.Items = itemsList.Select(item => new IrmaItemViewModel(item)).ToList();
            viewModel.PluCategories = GetPluCategorySelectList();
            viewModel.RetailUoms = GetRetailUomSelectList(String.Empty);

            HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());
            viewModel.BrandHierarchyClasses = GetHierarchyLineage(hierarchyListModel.BrandHierarchyList);
            viewModel.TaxHierarchyClasses = GetHierarchyLineage(hierarchyListModel.TaxHierarchyList);
            viewModel.MerchandiseHierarchyClasses = GetHierarchyLineage(hierarchyListModel.MerchandiseHierarchyList);
            viewModel.NationalHierarchyClasses = GetHierarchyLineage(hierarchyListModel.NationalHierarchyList);

            if (maxPlus > itemsList.Count)
            {
                ViewData["SearchWarning"] = "The number of PLUs specified isn't available.  Showing all available PLUs.";
            }

            return PartialView("_PluSearchResultsPartial", viewModel);
        }

        [WriteAccessAuthorize(IsJsonResult =true)]
        public ActionResult SaveChangesInGrid()
        {
            ViewData["GenerateCompactJSONResponse"] = false;

            List<Transaction<IrmaItemViewModel>> transactions = new GridModel()
                .LoadTransactions<IrmaItemViewModel>(HttpContext.Request.Form["ig_transactions"]);

            if (!transactions.Any())
            {
                return Json(new { Success = false, Error = "No new values were specified for the item." });
            }

            IrmaItemViewModel item = transactions.First().row;

            var addItemManager = new AddItemManager
            {
                //Item = item.ToBulkImportNewItemModel(true),
                //UserName = User.Identity.Name,
            };

            try
            {
                addItemManagerHandler.Execute(addItemManager);
            }
            catch (Exception exception)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(exception, this.GetType(), MethodBase.GetCurrentMethod());

                return Json(new
                    {
                        Success = false,
                        Error = String.Join(" ", exception.Message)
                    });
            }

            return Json(new
                {
                    Success = true,
                    Message = "Successfully validated all selected items."
                });
        }

        private IEnumerable<SelectListItem> GetPluCategorySelectList()
        {
            List<PLUCategory> pluCategoryList = getPluCategoriesQuery.Search(new GetPluCategoriesParameters());

            var dropDownList = pluCategoryList.Select(pc =>
                new DropDownViewModel
                {
                    Id = pc.PluCategoryID,
                    Name = pc.PluCategoryName
                });

            return dropDownList.ToSelectListItem();
        }

        private List<HierarchyClassViewModel> GetHierarchyLineage(List<HierarchyClassModel> hierarchyList)
        {
            List<HierarchyClassViewModel> hierarchyClasses = hierarchyList.HierarchyClassForCombo();
            return hierarchyClasses;
        }

        private SelectList GetRetailUomSelectList(string selectedUom)
        {
            var uoms = UomCodes.ByName.Values.ToList();
            return new SelectList(uoms, selectedUom);
        }
    }
}