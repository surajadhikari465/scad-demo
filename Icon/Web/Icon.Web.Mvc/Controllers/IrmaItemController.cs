using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.Common.Validators;
using Icon.Web.DataAccess.Commands;
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
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Icon.Web.Controllers
{
    public class IrmaItemController : Controller
    {
        private ILogger logger;
        private IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyClassesQuery;
        private ICommandHandler<UpdateIrmaItemCommand> updateIrmaItemHandler;
        private IManagerHandler<AddItemManager> addItemHandler;
        private IQueryHandler<GetIrmaItemsParameters, List<IRMAItem>> getIrmaItemsQuery;
        private ICommandHandler<DeleteIrmaItemCommand> deleteIrmaItemHandler;
        private IExcelExporterService excelExporterService;

        public IrmaItemController(
            ILogger logger,
            IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyClassesQuery,
            ICommandHandler<UpdateIrmaItemCommand> updateIrmaItemHandler,
            IManagerHandler<AddItemManager> addItemDeleteIrmaItemHandler,
            IQueryHandler<GetIrmaItemsParameters, List<IRMAItem>> getIrmaItemsQuery,          
            ICommandHandler<DeleteIrmaItemCommand> deleteIrmaItemHandler,
            IExcelExporterService excelExporterService)
        {
            this.logger = logger;
            this.getHierarchyClassesQuery = getHierarchyClassesQuery;
            this.updateIrmaItemHandler = updateIrmaItemHandler;
            this.addItemHandler = addItemDeleteIrmaItemHandler;
            this.getIrmaItemsQuery = getIrmaItemsQuery;
            this.deleteIrmaItemHandler = deleteIrmaItemHandler;
            this.excelExporterService = excelExporterService;
        }

        //
        // GET: /IrmaItem/
        public ActionResult Index()
        {
            return View(new IrmaItemSearchViewModel());
        }

        //
        // GET: /IrmaItem/Search/
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult Search(IrmaItemSearchViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return PartialView("_IrmaItemSearchOptionsPartial", viewModel);
            }

            GetIrmaItemsParameters parameters = new GetIrmaItemsParameters
            {
                Identifier = viewModel.Identifier,
                ItemDescription = viewModel.ItemDescription,
                Brand = viewModel.BrandName,
                RegionCode = viewModel.RegionCode,
                PartialScanCode = viewModel.PartialScanCode,
                TaxRomanceName = viewModel.TaxHierarchyName
            };

            List<IRMAItem> searchResults = getIrmaItemsQuery.Search(parameters);
            List<string> uoms = UomCodes.ByName.Values.ToList();
            List<string> deliverySystems = DeliverySystems.AsDictionary.Values.ToList();
            HierarchyClassListModel allHierarchies = getHierarchyClassesQuery.Search(new GetHierarchyLineageParameters());

            List<HierarchyClassViewModel> iconBrands = allHierarchies.BrandHierarchyList.Select(b => new HierarchyClassViewModel(b)).DistinctBy(hc => hc.HierarchyClassName).ToList();
            List<HierarchyClassViewModel> irmaBrands = searchResults.DistinctBy(fi => fi.brandName)
                .Select(fi => new HierarchyClassViewModel { HierarchyClassName = fi.brandName.Trim() })
                .ExceptBy(iconBrands, ib => ib.HierarchyClassName.Trim().ToLower())
                .Select(hc => new HierarchyClassViewModel { HierarchyClassName = hc.HierarchyClassName.Trim() })
                .ToList();

            viewModel.Uoms = uoms;
            viewModel.DeliverySystems = deliverySystems;
            viewModel.AllBrands = iconBrands.Union(irmaBrands).OrderBy(b => b.HierarchyClassName).ToList();
            viewModel.TaxHierarchyClasses = allHierarchies.TaxHierarchyList.Select(t => new HierarchyClassViewModel(t))
                .DistinctBy(t => t.HierarchyClassId).OrderBy(t => t.HierarchyClassLineage).ToList();
            viewModel.MerchandiseHierarchyClasses = allHierarchies.MerchandiseHierarchyList.Select(m => new HierarchyClassViewModel(m))
                .DistinctBy(m => m.HierarchyClassId).OrderBy(t => t.HierarchyClassLineage).ToList();
            viewModel.NationalHierarchyClasses = allHierarchies.NationalHierarchyList.Select(m => new HierarchyClassViewModel(m))
                .DistinctBy(m => m.HierarchyClassId).OrderBy(t => t.HierarchyClassLineage).ToList();
            viewModel.Items = searchResults.Select(item => new IrmaItemViewModel(item)
            {
                HasInvalidData = ItemContainsInValidData(item),
                IsNewBrand = irmaBrands.Any(brand => item.brandName.Trim().ToLower() == brand.HierarchyClassName.Trim().ToLower()),
                RetailUom = item.retailUom,
                IsUnsupportedUom = !uoms.Any(uom => (item.retailUom?? string.Empty).Trim().ToLower() == uom.Trim().ToLower()),
                MerchandiseHierarchyClassName = viewModel.MerchandiseHierarchyClasses.SingleOrDefault(merch => merch.HierarchyClassId == (item.merchandiseClassID ?? 0)) == null ? String.Empty :
                    viewModel.MerchandiseHierarchyClasses.SingleOrDefault(merch => merch.HierarchyClassId == (item.merchandiseClassID ?? 0)).HierarchyClassLineage,
                TaxHierarchyClassName = viewModel.TaxHierarchyClasses.SingleOrDefault(tax => tax.HierarchyClassId == (item.taxClassID ?? 0)) == null ? String.Empty :
                    viewModel.TaxHierarchyClasses.SingleOrDefault(tax => tax.HierarchyClassId == (item.taxClassID ?? 0)).HierarchyClassLineage,
                IrmaSubTeamName = item.irmaSubTeamName,
            }).ToList();
            viewModel.AnimalWelfareRatings = AnimalWelfareRatings.AsDictionary.Select(kvp => new HierarchyClassViewModel { HierarchyClassId = kvp.Key, HierarchyClassLineage = kvp.Value }).ToList();
            viewModel.MilkTypes = MilkTypes.AsDictionary.Select(kvp => new HierarchyClassViewModel { HierarchyClassId = kvp.Key, HierarchyClassLineage = kvp.Value }).ToList();
            viewModel.EcoScaleRatings = EcoScaleRatings.AsDictionary.Select(kvp => new HierarchyClassViewModel { HierarchyClassId = kvp.Key, HierarchyClassLineage = kvp.Value }).ToList();
            viewModel.SeafoodFreshOrFrozenTypes = SeafoodFreshOrFrozenTypes.AsDictionary.OrderBy(kvp => kvp.Value).Select(kvp => new HierarchyClassViewModel { HierarchyClassId = kvp.Key, HierarchyClassLineage = kvp.Value }).ToList();
            viewModel.SeafoodCatchTypes = SeafoodCatchTypes.AsDictionary.Select(kvp => new HierarchyClassViewModel { HierarchyClassId = kvp.Key, HierarchyClassLineage = kvp.Value }).ToList();

            //var certificationAgencies = getCertificationAgenciesQuery.Search(new GetCertificationAgenciesParameters());
            //viewModel.GlutenFreeAgencies = certificationAgencies.Where(ca => ca.GlutenFree == "1").ToList();
            //viewModel.KosherAgencies = certificationAgencies.Where(ca => ca.Kosher == "1").ToList();
            //viewModel.NonGmoAgencies = certificationAgencies.Where(ca => ca.NonGMO == "1").ToList();
            //viewModel.OrganicAgencies = certificationAgencies.Where(ca => ca.Organic == "1").ToList();
            //viewModel.VeganAgencies = certificationAgencies.Where(ca => ca.Vegan == "1").ToList();
            viewModel.NullableBooleanComboBoxValues = new NullableBooleanComboBoxValuesViewModel();

           
            return PartialView("_IrmaItemSearchResultsPartial", viewModel);
        }

        /// <summary>
        /// Loads selected rows into Icon, but does not officially 'Validate' the rows.
        /// This provides the option to load items into Icon DB without all the canonical fields
        /// </summary>
        /// <param name="selected">Selected igGrid (infragistics grid) rows</param>
        /// <returns>JsonResult: bool Success, string Message</returns>
        [HttpPost]
        [WriteAccessAuthorize(IsJsonResult = true)]
        public ActionResult LoadSelected(List<IrmaItemViewModel> selected)
        {
            if (selected == null || !selected.Any())
            {
                return Json(new
                {
                    Success = false,
                    Message = "No items were selected to load into Icon."
                });
            }

            return LoadOrValidateItems(selected, isValidated: false);
        }

        /// <summary>
        /// Validates selected rows into Icon.
        /// This is only available when all fields are populated and it populates the validatedDate Trait in Icon
        /// </summary>
        /// <param name="selected">Selected igGrid (infragistics grid) rows</param>
        /// <returns>JsonResult: bool Success, string Message</returns>
        [HttpPost]
        [WriteAccessAuthorize]
        public ActionResult ValidateSelected(List<IrmaItemViewModel> selected)
        {
            if (selected == null || selected.Count == 0)
            {
                return Json(new
                    {
                        Success = false,
                        Message = "No items were selected to validate."
                    });
            }

            return LoadOrValidateItems(selected, isValidated: true);
        }

        /// <summary>
        /// Adds list of items from new item grid into Icon.  It will validate or load the items depending on the IsValidated parameter.
        /// Any rows that were not successfully added, will be added to the erroneousItems list.
        /// The resulting Json message will list out the erroneous rows if there are any.
        /// </summary>
        /// <param name="selected">List of IrmaItemViewModel</param>
        /// <param name="erroneousItems">List of IrmaItemViewModel</param>
        /// <param name="isValidated">true = validate item, false = load item</param>
        /// <returns>JsonResult { Success, Message }</returns>
        private JsonResult LoadOrValidateItems(List<IrmaItemViewModel> selected, bool isValidated)
        {
            List<string> errorMessages = new List<string>();
            foreach (var row in selected)
            {
                AddItemManager addItem = new AddItemManager
                {
                    Item = row.ToBulkImportNewItemModel(isValidated),
                    UserName = User.Identity.Name
                };

                try
                {
                    addItemHandler.Execute(addItem);
                }
                catch (CommandException exception)
                {
                    var exceptionLogger = new ExceptionLogger(logger);
                    exceptionLogger.LogException(exception, this.GetType(), MethodBase.GetCurrentMethod());
                    errorMessages.Add(exception.Message);
                }
                catch (Exception exception)
                {
                    var exceptionLogger = new ExceptionLogger(logger);
                    exceptionLogger.LogException(exception, this.GetType(), MethodBase.GetCurrentMethod());
                    errorMessages.Add(exception.Message);
                }
            }

            if (errorMessages.Count == 0)
            {
                return Json(new
                {
                    Success = true,
                    Message = isValidated ? "Successfully validated all selected items." : "Successfully loaded all selected items."
                });
            }
            else
            {
                return Json(new
                {
                    Success = false,
                    Message = String.Join(" ", errorMessages.Select(m => m))
                });
            }
        }

        // Processes the Update for each row in the Infragistics igGrid
        [WriteAccessAuthorize(IsJsonResult = true)]
        public ActionResult SaveChangesInGrid()
        {
            ViewData["GenerateCompactJSONResponse"] = false;

            GridModel gridModel = new GridModel();
            List<Transaction<IrmaItemViewModel>> transactions = gridModel.LoadTransactions<IrmaItemViewModel>(HttpContext.Request.Form["ig_transactions"]);
            
            foreach (Transaction<IrmaItemViewModel> item in transactions)
            {
                UpdateIrmaItemCommand update = new UpdateIrmaItemCommand();
                update.IrmaItemId = item.row.IrmaItemId;
                update.BrandName = item.row.BrandName;
                update.ItemDescription = item.row.ItemDescription;
                update.PosDescription = item.row.PosDescription;
                update.PackageUnit = item.row.PackageUnit;
                update.RetailSize = item.row.RetailSize;
                update.RetailUom = item.row.RetailUom;
                update.DeliverySystem = item.row.DeliverySystem;
                update.FoodStampEligible = item.row.FoodStamp;
                update.PosScaleTare = item.row.PosScaleTare;
                update.TaxHierarchyClassId = item.row.TaxHierarchyClassId;
                update.MerchandiseHierarchyClassId = item.row.MerchandiseHierarchyClassId;
                update.NationalHierarchyClassId = item.row.NationalHierarchyClassId;
                update.AnimalWelfareRatingId = item.row.AnimalWelfareRatingId;
                update.Biodynamic = item.row.Biodynamic;
                update.CheeseMilkTypeId = item.row.CheeseMilkTypeId;
                update.CheeseRaw = item.row.CheeseRaw;
                update.EcoScaleRatingId = item.row.EcoScaleRatingId;
                update.Msc = item.row.Msc;
                update.PremiumBodyCare = item.row.PremiumBodyCare;
                update.SeafoodFreshOrFrozenId = item.row.SeafoodFreshOrFrozenId;
                update.SeafoodCatchTypeId = item.row.SeafoodCatchTypeId;
                update.Vegetarian = item.row.Vegetarian;
                update.WholeTrade = item.row.WholeTrade;
                update.GrassFed = item.row.GrassFed;
                update.PastureRaised = item.row.PastureRaised;
                update.FreeRange = item.row.FreeRange;
                update.DryAged = item.row.DryAged;
                update.AirChilled = item.row.AirChilled;
                update.MadeInHouse = item.row.MadeInHouse;
                update.AlcoholByVolume = item.row.AlcoholByVolume;

                try
                {
                    updateIrmaItemHandler.Execute(update);                    
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

        /// <summary>
        /// Deletes selected rows from Icon new items.
        /// </summary>
        /// <param name="selected">Selected igGrid (infragistics grid) rows</param>
        /// <returns>JsonResult: bool Success, string Message</returns>
        [HttpPost]
        [WriteAccessAuthorize(IsJsonResult = true)]
        public ActionResult DeleteSelected(List<IrmaItemViewModel> selected)
        {
            if (selected == null || selected.Count == 0)
            {
                return Json(new
                {
                    Success = false,
                    Message = "No items were selected to delete."
                });
            }

            return DeleteItems(selected);
        }
             
        private JsonResult DeleteItems(List<IrmaItemViewModel> selected)
        {
            List<string> errorMessages = new List<string>();

            foreach (var row in selected)
            {
                DeleteIrmaItemCommand deleteItem = new DeleteIrmaItemCommand();
                deleteItem.IrmaItemId = row.IrmaItemId;   

                try
                {                   
                    deleteIrmaItemHandler.Execute(deleteItem);
                }
                catch (CommandException exception)
                {
                    var exceptionLogger = new ExceptionLogger(logger);
                    exceptionLogger.LogException(exception, this.GetType(), MethodBase.GetCurrentMethod());
                    errorMessages.Add(exception.Message);
                }
            }

            if (errorMessages.Count == 0)
            {
                return Json(new
                {
                    Success = true,
                    Message = "Successfully deleted all selected items."
                });
            }
            else
            {
                return Json(new
                {
                    Success = false,
                    Message = String.Join(" ", errorMessages.Select(m => m))
                });
            }
        }

        public ActionResult BrandAutocomplete(string term)
        {
            var brandMatches = getIrmaItemsQuery.Search(new GetIrmaItemsParameters())
                .Where(item => item.brandName.ToLower().Contains(term.ToLower()))
                .OrderBy(item => item.brandName)
                .Select(item => item.brandName)
                .Distinct()
                .ToArray();

            return Json(brandMatches, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TaxAutocomplete(string term)
        {
            HierarchyClassListModel allHierarchies = getHierarchyClassesQuery.Search(new GetHierarchyLineageParameters());

            List<HierarchyClassModel> taxHierarchyClasses = GetTaxHiearchyForDropDown(allHierarchies.TaxHierarchyList);

            var taxMatches = taxHierarchyClasses
                .Where(hc => hc.HierarchyLineage.ToLower().Contains(term.ToLower()))
                .OrderBy(hc => hc.HierarchyLineage)
                .Select(hc => hc.HierarchyLineage).ToArray();

            return Json(taxMatches, JsonRequestBehavior.AllowGet);
        }

        public void ExportAll()
        {
            List<IRMAItem> newIrmaItems = getIrmaItemsQuery.Search(new GetIrmaItemsParameters());
            List<IrmaItemViewModel> irmaItems = newIrmaItems
                .Select(b => new IrmaItemViewModel
                {
                    IrmaItemId = b.irmaItemID,
                    BrandName = b.brandName,
                    Region = b.regioncode,
                    Identifier = b.identifier,
                    DefaultIdentifier = b.defaultIdentifier,
                    ItemDescription = b.itemDescription,
                    PosDescription = b.posDescription,
                    PackageUnit = b.packageUnit,
                    RetailUom = b.retailUom,
                    RetailSize = b.retailSize,
                    DeliverySystem = b.DeliverySystem,
                    FoodStamp = b.foodStamp,
                    PosScaleTare = b.posScaleTare,
                    IrmaSubTeamName = b.irmaSubTeamName,
                    MerchandiseHierarchyClassId = b.merchandiseClassID,
                    TaxHierarchyClassId = b.taxClassID,
                    AnimalWelfareRatingId = b.AnimalWelfareRatingId,
                    Biodynamic = b.Biodynamic,
                    CheeseMilkTypeId = b.CheeseMilkTypeId,
                    CheeseRaw = b.CheeseRaw,
                    EcoScaleRatingId = b.EcoScaleRatingId,
                    Msc = b.Msc,
                    PremiumBodyCare = b.PremiumBodyCare,
                    SeafoodFreshOrFrozenId = b.SeafoodFreshOrFrozenId,
                    SeafoodCatchTypeId = b.SeafoodCatchTypeId,
                    Vegetarian = b.Vegetarian,
                    WholeTrade = b.WholeTrade,
                    GrassFed = b.GrassFed,
                    PastureRaised = b.PastureRaised,
                    FreeRange = b.FreeRange,
                    DryAged = b.DryAged,
                    AirChilled = b.AirChilled,
                    MadeInHouse = b.MadeInHouse,
                    NationalHierarchyClassId = b.nationalClassID,
                    AlcoholByVolume = b.AlcoholByVolume
                })
                .ToList();

            var IrmaItemExporter = excelExporterService.GetIrmaItemExporter();
            IrmaItemExporter.ExportData = irmaItems;
            IrmaItemExporter.Export();

            ExcelHelper.SendForDownload(Response, IrmaItemExporter.ExportModel.ExcelWorkbook, IrmaItemExporter.ExportModel.ExcelWorkbook.CurrentFormat, "IrmaItem");
        }

        private bool ItemContainsInValidData(IRMAItem irmaItem)
        {
            return (
                    !IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.ProductDescription).IsValid(irmaItem.itemDescription ?? string.Empty, null)
                    || !IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.PosDescription).IsValid(irmaItem.posDescription ?? string.Empty, null)
                    || !IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.BrandName).IsValid(irmaItem.brandName ?? string.Empty, null)
                    );
        }

        private List<HierarchyClassModel> GetTaxHiearchyForDropDown(List<HierarchyClassModel> TaxHierarchyList)
        {
            TaxHierarchyList.Add(new HierarchyClassModel()
                {
                    HierarchyClassId = 0,
                    HierarchyClassName = Constants.UndefinedTaxName,
                    HierarchyLevel = HierarchyLevels.Tax,
                    HierarchyLineage = Constants.UndefinedTaxName,
                    HierarchyParentClassId = null,
                    HierarchyId = 0
                });
            return TaxHierarchyList;            
        }
    }
}
