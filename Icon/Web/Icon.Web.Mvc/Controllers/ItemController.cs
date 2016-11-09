using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.Common.Utility;
using Icon.Web.Common.Validators;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Extensions;
using Icon.Web.Mvc.Attributes;
using Icon.Web.Mvc.Extensions;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Utility;
using Infragistics.Web.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Icon.Web.Controllers
{
    public class ItemController : Controller
    {
        private ILogger logger;
        private IManagerHandler<UpdateItemManager> updateItemManagerHandler;
        private IManagerHandler<ValidateItemManager> validateItemManagerHandler;
        private IManagerHandler<AddItemManager> addItemManagerHandler;
        private IObjectValidator<ItemViewModel> itemViewModelValidator;
        private IQueryHandler<GetItemsBySearchParameters, ItemsBySearchResultsModel> getItemsBySearchQueryHandler;
        private IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQueryHandler;
        private IQueryHandler<GetCertificationAgenciesParameters, List<CertificationAgencyModel>> getCertificationAgenciesQuery;
        private ICommandHandler<AddProductMessageCommand> addProductMessageCommandHandler;
        private IInfragisticsHelper infragisticsHelper;
        private IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>> getItemsByBulkScanCodeSearcParameters;

        public ItemController(
            ILogger logger,
            IManagerHandler<UpdateItemManager> updateItemManagerHandler,
            IManagerHandler<ValidateItemManager> validateItemManagerHandler,
            IManagerHandler<AddItemManager> addItemManagerHandler,
            IObjectValidator<ItemViewModel> itemViewModelValidator,
            IQueryHandler<GetItemsBySearchParameters, ItemsBySearchResultsModel> getItemsBySearchQueryHandler,
            IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQueryHandler,
            IQueryHandler<GetCertificationAgenciesParameters, List<CertificationAgencyModel>> getCertificationAgenciesQuery,
            ICommandHandler<AddProductMessageCommand> addProductMessageHandler,
            IInfragisticsHelper infragisticsHelper,
            IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>> getItemsByBulkScanCodeSearcParameters)
        {
            this.logger = logger;
            this.updateItemManagerHandler = updateItemManagerHandler;
            this.validateItemManagerHandler = validateItemManagerHandler;
            this.addItemManagerHandler = addItemManagerHandler;
            this.itemViewModelValidator = itemViewModelValidator;
            this.getItemsBySearchQueryHandler = getItemsBySearchQueryHandler;
            this.getHierarchyLineageQueryHandler = getHierarchyLineageQueryHandler;
            this.getCertificationAgenciesQuery = getCertificationAgenciesQuery;
            this.addProductMessageCommandHandler = addProductMessageHandler;
            this.infragisticsHelper = infragisticsHelper;
            this.getItemsByBulkScanCodeSearcParameters = getItemsByBulkScanCodeSearcParameters;
    }
        
        public ActionResult Index()
        {
            var viewModel = new ItemSearchViewModel
            {
                RetailUoms = GetRetailUomSelectList(string.Empty, includeInitialBlank: true),
                DeliverySystems = GetDeliverySystemSelectList(string.Empty, includeInitialBlank: true),
                DrainedWeightUomOptions = GetDrainedWeightUomSelectList(string.Empty, includeInitialBlank: true),
                FairTradeCertifiedOptions = GetFairTradeCertifiedSelectList(string.Empty, includeInitialBlank: true)
            }; 

            ViewData["CreateItemMessage"] = TempData["CreateItemMessage"];

            return View(viewModel);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Search(ItemSearchViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                viewModel.RetailUoms = GetRetailUomSelectList(string.Empty, includeInitialBlank: true);
                viewModel.DeliverySystems = GetDeliverySystemSelectList(string.Empty, includeInitialBlank: true);
                viewModel.DrainedWeightUomOptions = GetDrainedWeightUomSelectList(string.Empty, includeInitialBlank: true);
                viewModel.FairTradeCertifiedOptions = GetFairTradeCertifiedSelectList(string.Empty, includeInitialBlank: true);
                return PartialView("_ItemSearchOptionsPartial", viewModel);
            }

            viewModel.TotalRecordsCount = getItemsBySearchQueryHandler.Search(viewModel.GetSearchParameters(true)).ItemsCount;
            
            viewModel.ItemSearchResults.RetailUoms = GetRetailUomSelectList(string.Empty, includeInitialBlank: false);
            viewModel.ItemSearchResults.DeliverySystems = GetDeliverySystemSelectList(string.Empty, includeInitialBlank: false);
            viewModel.ItemSearchResults.AnimalWelfareRatings = AnimalWelfareRatings.AsDictionary.Select(kvp => new HierarchyClassViewModel { HierarchyClassId = kvp.Key, HierarchyClassLineage = kvp.Value }).ToList();
            viewModel.ItemSearchResults.MilkTypes = MilkTypes.AsDictionary.Select(kvp => new HierarchyClassViewModel { HierarchyClassId = kvp.Key, HierarchyClassLineage = kvp.Value }).ToList();
            viewModel.ItemSearchResults.EcoScaleRatings = EcoScaleRatings.AsDictionary.Select(kvp => new HierarchyClassViewModel { HierarchyClassId = kvp.Key, HierarchyClassLineage = kvp.Value }).ToList();
            viewModel.ItemSearchResults.SeafoodFreshOrFrozenTypes = SeafoodFreshOrFrozenTypes.AsDictionary.OrderBy(kvp => kvp.Value).Select(kvp => new HierarchyClassViewModel { HierarchyClassId = kvp.Key, HierarchyClassLineage = kvp.Value }).ToList();
            viewModel.ItemSearchResults.SeafoodCatchTypes = SeafoodCatchTypes.AsDictionary.Select(kvp => new HierarchyClassViewModel { HierarchyClassId = kvp.Key, HierarchyClassLineage = kvp.Value }).ToList();

            var certificationAgencies = getCertificationAgenciesQuery.Search(new GetCertificationAgenciesParameters());
            viewModel.ItemSearchResults.GlutenFreeAgencies = certificationAgencies.Where(ca => ca.GlutenFree == "1").ToList();
            viewModel.ItemSearchResults.KosherAgencies = certificationAgencies.Where(ca => ca.Kosher == "1").ToList();
            viewModel.ItemSearchResults.NonGmoAgencies = certificationAgencies.Where(ca => ca.NonGMO == "1").ToList();
            viewModel.ItemSearchResults.OrganicAgencies = certificationAgencies.Where(ca => ca.Organic == "1").ToList();
            viewModel.ItemSearchResults.VeganAgencies = certificationAgencies.Where(ca => ca.Vegan == "1").ToList();

            HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());
            viewModel.ItemSearchResults.BrandHierarchyClasses = GetHierarchyLineage(hierarchyListModel.BrandHierarchyList);
            viewModel.ItemSearchResults.TaxHierarchyClasses = GetHierarchyLineage(hierarchyListModel.TaxHierarchyList);
            viewModel.ItemSearchResults.MerchandiseHierarchyClasses = GetHierarchyLineage(hierarchyListModel.MerchandiseHierarchyList);
            viewModel.ItemSearchResults.NationalHierarchyClasses = GetHierarchyLineage(hierarchyListModel.NationalHierarchyList);

            viewModel.ItemSearchResults.NullableBooleanComboBoxValues = new NullableBooleanComboBoxValuesViewModel();
            viewModel.ItemSearchResults.BooleanComboBoxValues = new BooleanComboBoxValuesViewModel();

            ViewData["displayAddItemLink"] = true;
            ViewData["dataSourceUrl"] = BuildItemSearchDataSourceUrl(viewModel);
            ViewData["totalRecordsCount"] = viewModel.TotalRecordsCount;

            return PartialView("_ItemSearchResults", viewModel.ItemSearchResults);
        }

        private string BuildItemSearchDataSourceUrl(ItemSearchViewModel viewModel)
        {
            return Url.Action("SearchJson", "Item", viewModel.GetRouteValuesObject());
        }
        
        public ActionResult SearchJson(ItemSearchViewModel viewModel)
        {
            var searchParameters = viewModel.GetSearchParameters();

            var result = infragisticsHelper.ParseSortParameterFromQueryString(HttpContext.Request.QueryString);
            if (result.SortParameterExists)
            {
                searchParameters.SortOrder = result.SortOrder;
                searchParameters.SortColumn = result.SortColumn;
            }

            var itemsList = getItemsBySearchQueryHandler.Search(searchParameters)
                .Items
                .ToViewModels();

            if (result.SortParameterExists)
            {
                return Json(new
                {
                    Records = itemsList,
                    TotalRecordsCount = viewModel.TotalRecordsCount,
                    MetaData = new
                    {
                        GroupBy = result.SortColumn
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Records = itemsList, TotalRecordsCount = viewModel.TotalRecordsCount }, JsonRequestBehavior.AllowGet);
            }
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

        public ActionResult NationalAutocomplete(string term)
        {
            HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());
            List<HierarchyClassModel> nationalHierarchyClasses = hierarchyListModel.NationalHierarchyList;

            var taxMatches = nationalHierarchyClasses
                .Where(hc => hc.HierarchyClassName.ToLower().Contains(term.ToLower()) || hc.HierarchyLineage.Contains(term.ToLower()))
                .OrderBy(hc => hc.HierarchyClassName)
                .Select(hc => hc.HierarchyLineage).ToArray();

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
        
        public ActionResult Edit(string scanCode)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return RedirectToAction("Index");
            }

            var viewModel = BuildItemEditViewModel(scanCode);
            if (TempData["MessageSendSuccess"] != null)
            {
                ViewData["UpdateSuccess"] = TempData["MessageSendSuccess"];
            }
            if (TempData["MessageSendFailed"] != null)
            {
                ViewData["UpdateFailed"] = TempData["MessageSendFailed"];
            }
            return View(viewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [WriteAccessAuthorize]
        public ActionResult Edit(ItemEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(BuildItemEditViewModel(viewModel.ScanCode));
            }

            var brand = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters())
                .BrandHierarchyList
                .FirstOrDefault(hc => hc.HierarchyClassName == viewModel.BrandName);

            if(brand == null)
            {
                ViewData["UpdateFailed"] = string.Format("Brand {0} does not exist.", viewModel.BrandName);
                return View(BuildItemEditViewModel(viewModel.ScanCode));
            }
            else
            {
                var manager = new UpdateItemManager
                {
                    Item = viewModel.ToBulkImportModel(brand.HierarchyClassId),
                    UserName = User.Identity.Name                    
                };

                try
                {
                    updateItemManagerHandler.Execute(manager);
                    ViewData["UpdateSuccess"] = "Update successful.";
                }
                catch (Exception ex)
                {
                    string errorDetails = ex.Message;
                    ViewData["UpdateFailed"] = errorDetails;
                }

                // Rebuild lists and appropriate pieces of view.
                return View(BuildItemEditViewModel(viewModel.ScanCode));
            }
        }
        
        [HttpPost]
        public ActionResult SendProductMessage(int itemId, string scanCode)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return RedirectToAction("Index");
            }

            var parameters = new GetItemByIdParameters
            {
                ItemId = itemId
            };

            try
            {
                addProductMessageCommandHandler.Execute(new AddProductMessageCommand { ItemId = itemId });
                TempData["MessageSendSuccess"] = "Message generated successfully.";
            }
            catch (Exception e)
            {
                string errorDetails = "There was an error creating the message for the ESB.";
                logger.Error(errorDetails + e.InnerException);
                TempData["MessageSendFailed"] = errorDetails;
            }
            return RedirectToAction("Edit", new { scanCode = scanCode });
        }

        [WriteAccessAuthorize(IsJsonResult = true)]
        public ActionResult SaveChangesInGrid()
        {
            ViewData["GenerateCompactJSONResponse"] = false;

            List<Transaction<ItemViewModel>> transactions = new GridModel()
                .LoadTransactions<ItemViewModel>(HttpContext.Request.Form["ig_transactions"]);

            if (!transactions.Any())
            {
                return Json(new { Success = false, Error = "No new values were specified for the item." });
            }

            try
            {
                updateItemManagerHandler.Execute(new UpdateItemManager
                {
                    Item = transactions.First().row.ToBulkImportModel(),
                    UserName = User.Identity.Name,
                });
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

        /// <summary>
        /// Validates selected rows into Icon.
        /// This is only available when all fields are populated and it populates the validatedDate Trait in Icon
        /// </summary>
        /// <param name="selected">Selected igGrid (infragistics grid) rows</param>
        /// <returns>JsonResult: bool Success, string Message</returns>
        [HttpPost]
        [WriteAccessAuthorize(IsJsonResult = true)]
        public ActionResult ValidateSelected(List<ItemViewModel> selected)
        {
            if (selected == null || !selected.Any())
            {
                return Json(new
                {
                    Success = false,
                    Message = "No items were selected to validate."
                });
            }

            List<ItemViewModel> erroneousItems = new List<ItemViewModel>();
            string errorMessage = null;

            foreach (var row in selected)
            {
                ValidateItemManager validateItemManager = new ValidateItemManager
                {
                    ScanCode = row.ScanCode,
                    UserName = User.Identity.Name
                };

                try
                {
                    validateItemManagerHandler.Execute(validateItemManager);
                }
                catch (CommandException exception)
                {
                    var exceptionLogger = new ExceptionLogger(logger);
                    exceptionLogger.LogException(exception, this.GetType(), MethodBase.GetCurrentMethod());
                    erroneousItems.Add(row);
                    errorMessage = exception.Message;
                }
                catch (Exception ex)
                {
                    var exceptionLogger = new ExceptionLogger(logger);
                    exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                    erroneousItems.Add(row);
                    errorMessage = ex.Message;
                }
            }

            if (erroneousItems.Count == 0)
            {
                return Json(new
                {
                    Success = true,
                    Message = "Successfully validated all selected items."
                });
            }
            else
            {
                string listOfErroneousIdentifiers = erroneousItems
                    .Select(ei => ei.ScanCode)
                    .Aggregate((identifier1, identifier2) => identifier1 + ", " + identifier2)
                    .TrimEnd(',');

                return Json(new
                {
                    Success = false,
                    Message = errorMessage != null ? errorMessage : string.Format("Validation failed for the following identifiers: {0}", listOfErroneousIdentifiers)
                });
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            var viewModel = new ItemCreateViewModel();
            HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());
            viewModel.MerchandiseHierarchyClasses = GetHierarchyLineage(hierarchyListModel.MerchandiseHierarchyList);
            viewModel.TaxHierarchyClasses = GetHierarchyLineage(hierarchyListModel.TaxHierarchyList);
            viewModel.BrowsingHierarchyClasses = new SelectList(GetHierarchyLineage(hierarchyListModel.BrowsingHierarchyList), "HierarchyClassId", "HierarchyClassLineage");
            viewModel.NationalHierarchyClasses = GetHierarchyLineage(hierarchyListModel.NationalHierarchyList);

            var certificationAgencies = getCertificationAgenciesQuery.Search(new GetCertificationAgenciesParameters());
            viewModel.GlutenFreeAgencies = GetAgencySelectList(certificationAgencies.Where(ca => ca.GlutenFree == "1").ToList());
            viewModel.KosherAgencies = GetAgencySelectList(certificationAgencies.Where(ca => ca.Kosher == "1").ToList());
            viewModel.NonGmoAgencies = GetAgencySelectList(certificationAgencies.Where(ca => ca.NonGMO == "1").ToList());
            viewModel.OrganicAgencies = GetAgencySelectList(certificationAgencies.Where(ca => ca.Organic == "1").ToList());
            viewModel.VeganAgencies = GetAgencySelectList(certificationAgencies.Where(ca => ca.Vegan == "1").ToList());


            viewModel.RetailUoms = GetRetailUomSelectList(string.Empty, true);
            viewModel.DeliverySystems = GetDeliverySystemSelectList(string.Empty, true);

            return View(viewModel);
        }

        [HttpPost]
        [WriteAccessAuthorize]
        public ActionResult Create(ItemCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());
                viewModel.MerchandiseHierarchyClasses = GetHierarchyLineage(hierarchyListModel.MerchandiseHierarchyList);
                viewModel.TaxHierarchyClasses = GetHierarchyLineage(hierarchyListModel.TaxHierarchyList);
                viewModel.BrowsingHierarchyClasses = new SelectList(GetHierarchyLineage(hierarchyListModel.BrowsingHierarchyList), "HierarchyClassId", "HierarchyClassLineage");
                viewModel.NationalHierarchyClasses = GetHierarchyLineage(hierarchyListModel.NationalHierarchyList);
                viewModel.RetailUoms = GetRetailUomSelectList(string.Empty, true);
                viewModel.DeliverySystems = GetDeliverySystemSelectList(string.Empty, true);
                var certificationAgencies = getCertificationAgenciesQuery.Search(new GetCertificationAgenciesParameters());
                viewModel.GlutenFreeAgencies = GetAgencySelectList(certificationAgencies.Where(ca => ca.GlutenFree == "1").ToList());
                viewModel.KosherAgencies = GetAgencySelectList(certificationAgencies.Where(ca => ca.Kosher == "1").ToList());
                viewModel.NonGmoAgencies = GetAgencySelectList(certificationAgencies.Where(ca => ca.NonGMO == "1").ToList());
                viewModel.OrganicAgencies = GetAgencySelectList(certificationAgencies.Where(ca => ca.Organic == "1").ToList());
                viewModel.VeganAgencies = GetAgencySelectList(certificationAgencies.Where(ca => ca.Vegan == "1").ToList());

                return View(viewModel);
            }

            try
            {
                AddItemManager addItemManager = new AddItemManager
                {
                    Item = viewModel.ToBulkImportNewItemModel(),
                    UserName = User.Identity.Name
                };
                addItemManagerHandler.Execute(addItemManager);
            }
            catch (Exception ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());

                ViewData["ErrorMessage"] = ex.Message;

                HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());
                viewModel.MerchandiseHierarchyClasses = GetHierarchyLineage(hierarchyListModel.MerchandiseHierarchyList);
                viewModel.TaxHierarchyClasses = GetHierarchyLineage(hierarchyListModel.TaxHierarchyList);
                viewModel.BrowsingHierarchyClasses = new SelectList(GetHierarchyLineage(hierarchyListModel.BrowsingHierarchyList), "HierarchyClassId", "HierarchyClassLineage");
                viewModel.NationalHierarchyClasses = GetHierarchyLineage(hierarchyListModel.NationalHierarchyList);
                viewModel.RetailUoms = GetRetailUomSelectList(string.Empty, true);
                viewModel.DeliverySystems = GetDeliverySystemSelectList(string.Empty, true);
                var certificationAgencies = getCertificationAgenciesQuery.Search(new GetCertificationAgenciesParameters());
                viewModel.GlutenFreeAgencies = GetAgencySelectList(certificationAgencies.Where(ca => ca.GlutenFree == "1").ToList());
                viewModel.KosherAgencies = GetAgencySelectList(certificationAgencies.Where(ca => ca.Kosher == "1").ToList());
                viewModel.NonGmoAgencies = GetAgencySelectList(certificationAgencies.Where(ca => ca.NonGMO == "1").ToList());
                viewModel.OrganicAgencies = GetAgencySelectList(certificationAgencies.Where(ca => ca.Organic == "1").ToList());
                viewModel.VeganAgencies = GetAgencySelectList(certificationAgencies.Where(ca => ca.Vegan == "1").ToList());

                return View(viewModel);
            }

            TempData["CreateItemMessage"] = "Created item successfully.";

            return RedirectToAction("Create");
        }

        private ItemEditViewModel BuildItemEditViewModel(string scanCode)
        {
            // Build view from selected item.
            var item = getItemsByBulkScanCodeSearcParameters.Search(new GetItemsByBulkScanCodeSearchParameters { ScanCodes = new List<string> { scanCode } }).First();
            ItemEditViewModel viewModel = new ItemEditViewModel(item);

            // Populate selection lists in view.
            viewModel.RetailUoms = GetRetailUomSelectList(viewModel.RetailUom, includeInitialBlank: true);
            viewModel.DeliverySystems = GetDeliverySystemSelectList(viewModel.DeliverySystem, includeInitialBlank: true);
            HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());
            viewModel.MerchandiseHierarchyClasses = new SelectList(GetHierarchyLineage(hierarchyListModel.MerchandiseHierarchyList), "HierarchyClassId", "HierarchyClassLineage");
            viewModel.TaxHierarchyClasses = new SelectList(GetHierarchyLineage(hierarchyListModel.TaxHierarchyList), "HierarchyClassId", "HierarchyClassLineage");
            viewModel.BrowsingHierarchyClasses = new SelectList(GetHierarchyLineage(hierarchyListModel.BrowsingHierarchyList), "HierarchyClassId", "HierarchyClassLineage");
            viewModel.NationalHierarchyClasses = new SelectList(GetHierarchyLineage(hierarchyListModel.NationalHierarchyList), "HierarchyClassId", "HierarchyClassLineage");

            var certificationAgencies = getCertificationAgenciesQuery.Search(new GetCertificationAgenciesParameters());
            viewModel.GlutenFreeAgencies = new SelectList(certificationAgencies.Where(ca => ca.GlutenFree == "1"), "HierarchyClassId", "HierarchyClassName");
            viewModel.KosherAgencies = new SelectList(certificationAgencies.Where(ca => ca.Kosher == "1"), "HierarchyClassId", "HierarchyClassName");
            viewModel.NonGmoAgencies = new SelectList(certificationAgencies.Where(ca => ca.NonGMO == "1"), "HierarchyClassId", "HierarchyClassName");
            viewModel.OrganicAgencies = new SelectList(certificationAgencies.Where(ca => ca.Organic == "1"), "HierarchyClassId", "HierarchyClassName");
            viewModel.VeganAgencies = new SelectList(certificationAgencies.Where(ca => ca.Vegan == "1"), "HierarchyClassId", "HierarchyClassName");

            return viewModel;
        }

        private SelectList GetRetailUomSelectList(string selectedUom, bool includeInitialBlank)
        {
            var uoms = UomCodes.ByName.Values.ToList();

            if (includeInitialBlank)
            {
                // Insert empty value at the beginning of the list to allow for an un-selected state.
                uoms.Insert(0, string.Empty);
            }

            return new SelectList(uoms, selectedUom);
        }

        private SelectList GetDrainedWeightUomSelectList(string selectedValue, bool includeInitialBlank)
        {
            var values = DrainedWeightUoms.Values.ToList();

            if (includeInitialBlank)
            {
                // Insert empty value at the beginning of the list to allow for an un-selected state.
                values.Insert(0, string.Empty);
            }

            return new SelectList(values, selectedValue);
        }

        private SelectList GetFairTradeCertifiedSelectList(string selectedValue, bool includeInitialBlank)
        {
            var values = FairTradeCertifiedValues.Values.ToList();

            if (includeInitialBlank)
            {
                // Insert empty value at the beginning of the list to allow for an un-selected state.
                values.Insert(0, string.Empty);
            }

            return new SelectList(values, selectedValue);
        }


        private SelectList GetDeliverySystemSelectList(string selectedDeliverySystem, bool includeInitialBlank)
        {
            var deliverySystems = DeliverySystems.AsDictionary.Values.ToList();

            if (includeInitialBlank)
            {
                // Insert empty value at the beginning of the list to allow for an un-selected state.
                deliverySystems.Insert(0, string.Empty);
            }

            return new SelectList(deliverySystems, selectedDeliverySystem);
        }

        private List<HierarchyClassViewModel> GetHierarchyLineage(List<HierarchyClassModel> hierarchyList)
        {
            List<HierarchyClassViewModel> hierarchyClasses = hierarchyList.HierarchyClassForCombo();
            return hierarchyClasses;
        }

        private SelectList GetAgencySelectList(List<CertificationAgencyModel> certificateAgencyList)
        {
            return new SelectList(certificateAgencyList, "HierarchyClassId", "HierarchyClassName");
        }
        
        [ComboDataSourceAction]
        public ActionResult Combo()
        {
            var queryString = HttpContext.Request.QueryString;
            using (var context = new IconContext())
            {
                var brands = context.HierarchyClass.Where(hc => hc.hierarchyID == Hierarchies.Brands)
                    .OrderBy(hc => hc.hierarchyClassName)
                    .Select(hc => new { Id = hc.hierarchyClassID, Name = hc.hierarchyClassName })
                    .ToList();

                return View(brands.AsQueryable());
            }
        }
    }
}