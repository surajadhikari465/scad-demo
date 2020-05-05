using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Common.Models;
using Icon.Common.Validators.ItemAttributes;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.Common.BulkUpload;
using Icon.Web.Common.Utility;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Infrastructure.ItemSearch;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Attributes;
using Icon.Web.Mvc.Domain.BulkImport;
using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Extensions;
using Icon.Web.Mvc.InfragisticsHelpers;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Utility;
using Icon.Web.Mvc.Utility.ItemHistory;
using Infragistics.Documents.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Constants = Icon.Common.Constants;

namespace Icon.Web.Controllers
{
    public class ItemController : Controller
    {
        private ILogger logger;
        private IconWebAppSettings settings;
        private IQueryHandler<GetItemsParameters, GetItemsResult> getItemsQueryHandler;
        private IQueryHandler<GetItemParameters, ItemDbModel> getItemQueryHandler;
        private IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>> getAttributesQueryHandler;
        private IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeInforModel>>, IEnumerable<AttributeInforModel>> getInforAttributesQueryHandler;
        private IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>> getHierarchyClassesQueryHandler;
        private IQueryHandler<GetItemHistoryParameters, IEnumerable<ItemHistoryDbModel>> getItemHistoryQueryHandler;
        private IQueryHandler<GetItemHierarchyClassHistoryParameters, ItemHierarchyClassHistoryAllModel> getItemHierarchyHistoryQueryHandler;
        private IManagerHandler<UpdateItemManager> updateItemManagerHandler;
        private IInfragisticsHelper infragisticsHelper;
        private IQueryHandler<GetItemPropertiesFromMerchHierarchyParameters, MerchDependentItemPropertiesModel> getItemPropertiesFromMerchQueryHandler;
        private IManagerHandler<AddItemManager> addItemManagerHandler;
        private IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>> getBarcodeTypeQueryHandler;
        private IExcelExporterService exporterService;
        private IItemAttributesValidatorFactory itemAttributesValidatorFactory;
        private IItemHistoryBuilder itemHistoryBuilder;
        private IQueryHandler<GetItemInforHistoryParameters, IEnumerable<ItemInforHistoryDbModel>> getItemInforHistoryQueryHandler;
        private IOrderFieldsHelper orderFieldsHelper;
        private IHistoryModelTransformer historyModelTransformer;
        private readonly IQueryHandler<GetItemsByIdSearchParameters, GetItemsResult> getItemsByIdHandler;
        private IQueryHandler<GetScanCodesParameters, List<string>> getScanCodeQueryHandler;

        public ItemController(
            ILogger logger,
            IQueryHandler<GetItemsParameters, GetItemsResult> getItemsQueryHandler,
            IQueryHandler<GetItemHistoryParameters, IEnumerable<ItemHistoryDbModel>> getItemHistoryQueryHandler,
            IQueryHandler<GetItemHierarchyClassHistoryParameters, ItemHierarchyClassHistoryAllModel> getItemHierarchyHistoryQueryHandler,
            IQueryHandler<GetItemParameters, ItemDbModel> getItemQueryHandler,
            IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>> getAttributesQueryHandler,
            IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeInforModel>>, IEnumerable<AttributeInforModel>> getInforAttributesQueryHandler,
            IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>> getHierarchyClassesQueryHandler,
            IManagerHandler<UpdateItemManager> updateItemManagerHandler,
            IInfragisticsHelper infragisticsHelper,
            IManagerHandler<AddItemManager> addItemManagerHandler,
            IQueryHandler<GetItemPropertiesFromMerchHierarchyParameters, MerchDependentItemPropertiesModel> getItemPropertiesFromMerchQueryHandler,
            IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>> getBarcodeTypeQueryHandler,
            IExcelExporterService exporterService,
            IItemAttributesValidatorFactory itemAttributesValidatorFactory,
            IQueryHandler<GetItemInforHistoryParameters,
            IEnumerable<ItemInforHistoryDbModel>> getItemInforHistoryQueryHandler,
            IconWebAppSettings settings,
            IItemHistoryBuilder itemHistoryBuilder,
            IHistoryModelTransformer historyModelTransformer,
            IQueryHandler<GetItemsByIdSearchParameters, GetItemsResult> getItemsByIdHandler,
            IOrderFieldsHelper orderFieldsHelper,
            IQueryHandler<GetScanCodesParameters, List<string>> getScanCodeQueryHandler)
        {
            this.logger = logger;
            this.settings = settings;
            this.getItemsQueryHandler = getItemsQueryHandler;
            this.getItemQueryHandler = getItemQueryHandler;
            this.getAttributesQueryHandler = getAttributesQueryHandler;
            this.getInforAttributesQueryHandler = getInforAttributesQueryHandler;
            this.getHierarchyClassesQueryHandler = getHierarchyClassesQueryHandler;
            this.updateItemManagerHandler = updateItemManagerHandler;
            this.infragisticsHelper = infragisticsHelper;
            this.addItemManagerHandler = addItemManagerHandler;
            this.getItemHistoryQueryHandler = getItemHistoryQueryHandler;
            this.getItemPropertiesFromMerchQueryHandler = getItemPropertiesFromMerchQueryHandler;
            this.getBarcodeTypeQueryHandler = getBarcodeTypeQueryHandler;
            this.exporterService = exporterService;
            this.itemAttributesValidatorFactory = itemAttributesValidatorFactory;
            this.getItemHierarchyHistoryQueryHandler = getItemHierarchyHistoryQueryHandler;
            this.itemHistoryBuilder = itemHistoryBuilder;
            this.getItemInforHistoryQueryHandler = getItemInforHistoryQueryHandler;
            this.historyModelTransformer = historyModelTransformer;
            this.getItemsByIdHandler = getItemsByIdHandler;
            this.orderFieldsHelper = orderFieldsHelper;
            this.getScanCodeQueryHandler = getScanCodeQueryHandler;
        }

        public ActionResult Index()
        {
            Session["GetItemsParametersViewModel"] = new GetItemsParametersViewModel();
            return View();
        }

        public ActionResult HasWriteAccess()
        {
            return this.Content(JsonConvert.SerializeObject(this.GetWriteAccess() == Enums.WriteAccess.Full ? true : false), "application/json");
        }

        /// <summary>
        /// This method is a POST method but should not have write security. The Item grid
        /// POSTs to this method while searching and a read only user needs to have access for 
        /// GridDataSource to function correctly. 
        /// </summary>
        /// <param name="getItemsParametersViewModel"></param>
        [HttpPost]
        public void SaveGetItemsParameters(GetItemsParametersViewModel getItemsParametersViewModel)
        {
            Session["GetItemsParametersViewModel"] = getItemsParametersViewModel;
            logger.Debug($"sessionID={this.Session.SessionID}, getItemsParametersViewModel={JsonConvert.SerializeObject(getItemsParametersViewModel)}");
        }

        public ActionResult GridDataSource()
        {
            var getItemsParametersViewModel = Session["GetItemsParametersViewModel"] as GetItemsParametersViewModel;

            if (getItemsParametersViewModel == null)
            {
                logger.Error($"sessionID={this.Session.SessionID}, getItemsParametersViewModel was null");
                throw new Exception("Search parameters were not set");
            }
            else
            {
                logger.Debug($"sessionID={this.Session.SessionID}, getItemsParametersViewModel={JsonConvert.SerializeObject(getItemsParametersViewModel)}");
            }

            int top = int.Parse(Request.QueryString["$top"]);
            int skip = int.Parse(Request.QueryString["$skip"]);
            string orderByValue = null;
            string orderByOrder = null;

            if (Request.QueryString.AllKeys.Any(k => k.Contains("$orderby")))
            {
                var splitOrderBy = Request.QueryString["$orderby"].Split(' ');
                orderByValue = splitOrderBy[0];
                orderByOrder = splitOrderBy[1];
            }
            ItemResultModel result = GetSearchResults(top, skip, orderByOrder, orderByValue);
            for (int i = 0; i < result.Records.Count; i++)
            {
                result.Records[i]["UserWriteAccess"] = this.GetWriteAccess();
            }
            return this.Content(JsonConvert.SerializeObject(result), "application/json");
        }
        public ActionResult GetMissingScanCodes()
        {
            List<string> missingScanCodes = new List<string>();
            if (Session["GetItemsParametersViewModel"] != null)
            {
                var getItemsParametersViewModel = Session["GetItemsParametersViewModel"] as GetItemsParametersViewModel;
                if (getItemsParametersViewModel.GetItemsAttributesParameters[0].AttributeName.Equals("ScanCode")
                    && getItemsParametersViewModel.GetItemsAttributesParameters[0].SearchOperator == AttributeSearchOperator.ExactlyMatchesAny)
                {
                    if (!string.IsNullOrWhiteSpace(getItemsParametersViewModel.GetItemsAttributesParameters[0].AttributeValue))
                    {
                        var searchParamScanCodes = getItemsParametersViewModel.GetItemsAttributesParameters[0].AttributeValue.Split(' ').ToList();
                        var missingScanCodeParam = new GetScanCodesParameters()
                        {
                            ListOfScanCodes = searchParamScanCodes
                        };
                        var existingScanCodes = getScanCodeQueryHandler.Search(missingScanCodeParam);
                        if (existingScanCodes != null && existingScanCodes.Any())
                        {
                            var notExistScanCodes = searchParamScanCodes.Distinct().Where(s => !existingScanCodes.Contains(s));
                            if (notExistScanCodes.Any())
                            {
                                missingScanCodes = notExistScanCodes.ToList();
                            }
                        }
                        else
                        {
                            missingScanCodes = searchParamScanCodes;
                        }
                    }
                }
            }
            return Json(new { MissingScanCodes = missingScanCodes }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [WriteAccessAuthorize]
        public ActionResult GridUpdate()
        {
            var transactions = infragisticsHelper.LoadTransactions<JObject>(HttpContext.Request.Form);
            var row = transactions.Single().row;

            var dictionary = row.ToObject<Dictionary<string, object>>()
                .ToDictionary(x => x.Key, x => (x.Value != null && x.Value is DateTime ? (x.Value as DateTime?).Value.ToUniversalTime().ToString() : x.Value?.ToString()));

            ItemEditViewModel model = new ItemEditViewModel()
            {
                ItemViewModel = new ItemViewModel()
                {
                    ItemId = Convert.ToInt32(dictionary["ItemId"]),
                    ScanCode = dictionary["ScanCode"],
                    MerchandiseHierarchyClassId = Convert.ToInt32(dictionary["MerchandiseHierarchyClassId"]),
                    BrandsHierarchyClassId = Convert.ToInt32(dictionary["BrandsHierarchyClassId"]),
                    TaxHierarchyClassId = Convert.ToInt32(dictionary["TaxHierarchyClassId"]),
                    FinancialHierarchyClassId = Convert.ToInt32(dictionary["FinancialHierarchyClassId"]),
                    NationalHierarchyClassId = Convert.ToInt32(dictionary["NationalHierarchyClassId"]),
                    ManufacturerHierarchyClassId = Convert.ToInt32(dictionary["ManufacturerHierarchyClassId"]),
                }
            };

            dictionary.Remove("ItemId");
            dictionary.Remove("ScanCode");
            dictionary.Remove("ScanCodeTypeId");
            dictionary.Remove("BarcodeType");
            dictionary.Remove("ScanCodeTypeDescription");
            dictionary.Remove("ItemType");
            dictionary.Remove("ItemTypeId");
            dictionary.Remove("ItemTypeDescription");
            dictionary.Remove("MerchandiseHierarchyClassId");
            dictionary.Remove("BrandsHierarchyClassId");
            dictionary.Remove("TaxHierarchyClassId");
            dictionary.Remove("FinancialHierarchyClassId");
            dictionary.Remove("NationalHierarchyClassId");
            dictionary.Remove("ManufacturerHierarchyClassId");

            List<string> errors = new List<string>();
            var attributeValidationErrors = dictionary
                .Select(kvp => itemAttributesValidatorFactory.CreateItemAttributesJsonValidator(kvp.Key).Validate(kvp.Value))
                .Where(r => !r.IsValid)
                .SelectMany(r => r.ErrorMessages)
                .ToList();
            if (attributeValidationErrors.Count > 0)
            {
                return Json(new { Success = false, Errors = attributeValidationErrors });
            }

            model.ItemViewModel.ItemAttributes = dictionary;
            this.UpdateItem(model);
            return Json(new { Success = true });
        }

        private void UpdateItem(ItemEditViewModel model)
        {
            var existingItem = this.getItemQueryHandler.Search(new GetItemParameters()
            {
                ScanCode = model.ItemViewModel.ScanCode
            }).ToViewModel();

            UpdateItemManager manager = new UpdateItemManager();
            manager.ItemId = model.ItemViewModel.ItemId;
            manager.ScanCode = model.ItemViewModel.ScanCode;
            manager.MerchandiseHierarchyClassId = model.ItemViewModel.MerchandiseHierarchyClassId;
            manager.BrandsHierarchyClassId = model.ItemViewModel.BrandsHierarchyClassId;
            manager.TaxHierarchyClassId = model.ItemViewModel.TaxHierarchyClassId;
            manager.NationalHierarchyClassId = model.ItemViewModel.NationalHierarchyClassId;
            manager.ManufacturerHierarchyClassId = model.ItemViewModel.ManufacturerHierarchyClassId.GetValueOrDefault();
            var merchDependentItemPropertiesModel = getItemPropertiesFromMerchQueryHandler.Search(new GetItemPropertiesFromMerchHierarchyParameters
            {
                MerchHierarchyClassId = manager.MerchandiseHierarchyClassId,
            });
            manager.FinancialHierarchyClassId = merchDependentItemPropertiesModel.FinancialHierarcyClassId;
            manager.ItemTypeId = ItemTypes.Ids[MerchToItemTypeCodeMapper.GetItemTypeCode(merchDependentItemPropertiesModel.NonMerchandiseTraitValue)];
            manager.ItemAttributes = model.ItemViewModel.ItemAttributes.Where(i => !string.IsNullOrWhiteSpace(i.Value)).ToDictionary(i => i.Key, i => i.Value);
            manager.ItemAttributes[Constants.Attributes.ProhibitDiscount] = merchDependentItemPropertiesModel.ProhibitDiscount.ToString().ToLower();
            manager.ItemAttributes[Constants.Attributes.ModifiedBy] = User.Identity.Name;
            manager.ItemAttributes[Constants.Attributes.ModifiedDateTimeUtc] = DateTime.UtcNow.ToFormattedDateTimeString();
            manager.ItemAttributes[Constants.Attributes.CreatedDateTimeUtc] = existingItem.ItemAttributes[Constants.Attributes.CreatedDateTimeUtc];
            manager.ItemAttributes[Constants.Attributes.CreatedBy] = existingItem.ItemAttributes[Constants.Attributes.CreatedBy];
            manager.ItemAttributes.Remove(Constants.Attributes.ItemTypeCode);

            List<AttributeModel> attributes = getAttributesQueryHandler.Search(new EmptyQueryParameters<IEnumerable<AttributeModel>>()).ToList();
            for (int i = 0; i < attributes.Count; i++)
            {
                if (attributes[i].DataTypeName == Constants.DataTypeNames.Boolean)
                {
                    if (manager.ItemAttributes.ContainsKey(attributes[i].AttributeName))
                    {
                        manager.ItemAttributes[attributes[i].AttributeName] = manager.ItemAttributes[attributes[i].AttributeName].ToLower();
                    }
                }

                if (attributes[i].DataTypeName == Constants.DataTypeNames.Date && !attributes[i].IsReadOnly)
                {
                    string attributeValue = string.Empty;
                    if (manager.ItemAttributes.TryGetValue(attributes[i].AttributeName, out attributeValue))
                    {
                        if (!string.IsNullOrWhiteSpace(attributeValue))
                        {
                            manager.ItemAttributes[attributes[i].AttributeName] =
                                Convert.ToDateTime(attributeValue).ToString("yyyy-MM-dd");
                        }
                    }
                }
            }

            this.updateItemManagerHandler.Execute(manager);
        }

        [HttpGet]
        public ActionResult Detail(string scanCode)
        {
            ItemDbModel item = getItemQueryHandler.Search(new GetItemParameters { ScanCode = scanCode });
            IEnumerable<AttributeModel> attributes = getAttributesQueryHandler.Search(new EmptyQueryParameters<IEnumerable<AttributeModel>>());

            ItemViewModel itemViewModel = item.ToViewModel();
            itemViewModel.MerchandiseHierarchyLineage = GetHierarchyLineage(Hierarchies.Merchandise, itemViewModel.MerchandiseHierarchyClassId);
            itemViewModel.BrandsHierarchyLineage = GetHierarchyLineage(Hierarchies.Brands, itemViewModel.BrandsHierarchyClassId);
            itemViewModel.TaxHierarchyLineage = GetHierarchyLineage(Hierarchies.Tax, itemViewModel.TaxHierarchyClassId);
            itemViewModel.FinancialHierarchyLineage = GetHierarchyLineage(Hierarchies.Financial, itemViewModel.FinancialHierarchyClassId);
            itemViewModel.NationalHierarchyLineage = GetHierarchyLineage(Hierarchies.National, itemViewModel.NationalHierarchyClassId);
            itemViewModel.ManufacturerHierarchyLineage = GetHierarchyLineage(Hierarchies.Manufacturer, itemViewModel.ManufacturerHierarchyClassId.GetValueOrDefault());
            var attributeViewModels = attributes.ToViewModels();

            var viewModel = new ItemDetailViewModel
            {
                ItemViewModel = itemViewModel,
                Attributes = attributeViewModels,
                ItemHistoryModel = this.GetItemHistoryModel(itemViewModel, false),
                OrderOfFields = orderFieldsHelper.OrderAllFields(attributeViewModels.Where(a =>a.IsActive).ToList())
            };
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult History(string scanCode)
        {
            var itemView = this.GetByScanCode(scanCode, true);
            return View(itemView.ItemHistoryModel);
        }

        [HttpGet]
        [WriteAccessAuthorize]
        public ActionResult Create()
        {
            ItemCreateViewModel itemCreateViewModel = new ItemCreateViewModel();
            BuildItemCreateViewModel(itemCreateViewModel, true);

            return View(itemCreateViewModel);
        }

        [HttpPost]
        [WriteAccessAuthorize]
        public ActionResult Create(ItemCreateViewModel itemCreateViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    List<string> allErrors = ModelState.Values.SelectMany(v => v.Errors).Select(s => s.ErrorMessage).ToList();
                    ViewData["ErrorMessages"] = allErrors;
                    BuildItemCreateViewModel(itemCreateViewModel);
                    return View(itemCreateViewModel);
                }

                AddItemManager manager = new AddItemManager();
                manager.MerchandiseHierarchyClassId = itemCreateViewModel.MerchandiseHierarchyClassId;
                manager.BrandsHierarchyClassId = itemCreateViewModel.BrandHierarchyClassId;
                manager.TaxHierarchyClassId = itemCreateViewModel.TaxHierarchyClassId;
                manager.NationalHierarchyClassId = itemCreateViewModel.NationalHierarchyClassId;
                manager.ManufacturerHierarchyClassId = itemCreateViewModel.ManufacturerHierarchyClassId.GetValueOrDefault();
                manager.BarCodeTypeId = itemCreateViewModel.BarcodeTypeId;
                manager.ScanCode = itemCreateViewModel.ScanCode;

                var merchDependentItemPropertiesModel = getItemPropertiesFromMerchQueryHandler.Search(new GetItemPropertiesFromMerchHierarchyParameters
                {
                    MerchHierarchyClassId = manager.MerchandiseHierarchyClassId,
                });

                manager.FinancialHierarchyClassId = merchDependentItemPropertiesModel.FinancialHierarcyClassId;
                manager.ItemTypeId = ItemTypes.Ids[MerchToItemTypeCodeMapper.GetItemTypeCode(merchDependentItemPropertiesModel.NonMerchandiseTraitValue)];
                manager.ItemAttributes = itemCreateViewModel.ItemAttributes.Where(i => !string.IsNullOrWhiteSpace(i.Value)).ToDictionary(i => i.Key, i => i.Value);
                manager.ItemAttributes.Add(Constants.Attributes.ProhibitDiscount, merchDependentItemPropertiesModel.ProhibitDiscount.ToString().ToLower());
                manager.ItemAttributes[Constants.Attributes.CreatedBy] = User.Identity.Name;
                manager.ItemAttributes[Constants.Attributes.CreatedDateTimeUtc] = DateTime.UtcNow.ToFormattedDateTimeString();
                manager.ItemAttributes[Constants.Attributes.ModifiedBy] = User.Identity.Name;
                manager.ItemAttributes[Constants.Attributes.ModifiedDateTimeUtc] = manager.ItemAttributes[Constants.Attributes.CreatedDateTimeUtc];
                List<AttributeModel> attributes = getAttributesQueryHandler.Search(new EmptyQueryParameters<IEnumerable<AttributeModel>>()).ToList();
                for (int i = 0; i < attributes.Count; i++)
                {
                    if (attributes[i].DataTypeName == Constants.DataTypeNames.Date && !attributes[i].IsReadOnly)
                    {
                        string attributeValue = string.Empty;
                        if (manager.ItemAttributes.TryGetValue(attributes[i].AttributeName, out attributeValue))
                        {
                            if (!string.IsNullOrWhiteSpace(attributeValue))
                            {
                                manager.ItemAttributes[attributes[i].AttributeName] =
                                    Convert.ToDateTime(attributeValue).ToString("yyyy-MM-dd");
                            }
                        }
                    }
                }
                addItemManagerHandler.Execute(manager);

                return RedirectToAction("Detail", new { scanCode = manager.ScanCode });
            }
            catch (Exception ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                List<string> allErrors = new List<string>() { ex.Message };
                ViewData["ErrorMessages"] = allErrors;
                BuildItemCreateViewModel(itemCreateViewModel);
                return View(itemCreateViewModel);
            }
        }

        [WriteAccessAuthorize]
        public ActionResult Edit(string scanCode)
        {
            return View(this.GetByScanCode(scanCode));
        }

        [HttpPost]
        [WriteAccessAuthorize]
        public ActionResult Edit(ItemEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                this.UpdateItem(viewModel);
                ItemEditViewModel updatedModel = this.GetByScanCode(viewModel.ItemViewModel.ScanCode);
                updatedModel.Success = true;
                ModelState.Clear();
                return View(updatedModel);
            }
            else
            {
                IEnumerable<AttributeModel> attributes = getAttributesQueryHandler.Search(new EmptyQueryParameters<IEnumerable<AttributeModel>>());
                viewModel.Attributes = attributes.ToViewModels().ToList();
                viewModel.ItemHistoryModel = GetItemHistoryModel(viewModel.ItemViewModel, false);
                viewModel.Success = false;
                viewModel.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(s => s.ErrorMessage).ToList();
                return View(viewModel);
            }
        }

        private string GetHierarchyLineage(int hierarchyId, int hierarchyClassId)
        {
            return getHierarchyClassesQueryHandler
                  .Search(new GetHierarchyClassesParameters
                  {
                      HierarchyId = hierarchyId,
                      HierarchyClassId = hierarchyClassId
                  })
                  .FirstOrDefault()?
                  .HierarchyLineage;
        }

        private void BuildItemCreateViewModel(ItemCreateViewModel itemCreateViewModel, bool shouldUpdateToPlu = false)
        {
            var attributeModels = getAttributesQueryHandler.Search(new EmptyQueryParameters<IEnumerable<AttributeModel>>());
            var filteredAttributeModels = attributeModels.Where(a => a.AttributeName != Constants.Attributes.ProhibitDiscount && a.IsActive);

            itemCreateViewModel.Attributes = filteredAttributeModels.ToViewModels();
            itemCreateViewModel.OrderOfFields = orderFieldsHelper.OrderAllFields(itemCreateViewModel.Attributes.ToList());
            var barcodeTypes = getBarcodeTypeQueryHandler.Search(new GetBarcodeTypeParameters());
            itemCreateViewModel.BarcodeTypes = barcodeTypes;

            if (shouldUpdateToPlu)
            {
                itemCreateViewModel.ScanCodeType = Constants.Plu;

            }
        }

        private ItemEditViewModel GetByScanCode(string scanCode, bool isInforHistory = false)
        {
            ItemDbModel item = this.getItemQueryHandler.Search(new GetItemParameters()
            {
                ScanCode = scanCode
            });

            IEnumerable<AttributeModel> attributes = getAttributesQueryHandler.Search(new EmptyQueryParameters<IEnumerable<AttributeModel>>());
            ItemViewModel itemViewModel = item.ToViewModel();
            itemViewModel.MerchandiseHierarchyLineage = GetHierarchyLineage(Hierarchies.Merchandise, itemViewModel.MerchandiseHierarchyClassId);
            itemViewModel.BrandsHierarchyLineage = GetHierarchyLineage(Hierarchies.Brands, itemViewModel.BrandsHierarchyClassId);
            itemViewModel.TaxHierarchyLineage = GetHierarchyLineage(Hierarchies.Tax, itemViewModel.TaxHierarchyClassId);
            itemViewModel.FinancialHierarchyLineage = GetHierarchyLineage(Hierarchies.Financial, itemViewModel.FinancialHierarchyClassId);
            itemViewModel.NationalHierarchyLineage = GetHierarchyLineage(Hierarchies.National, itemViewModel.NationalHierarchyClassId);
            itemViewModel.ManufacturerHierarchyLineage = GetHierarchyLineage(Hierarchies.Manufacturer, itemViewModel.ManufacturerHierarchyClassId.GetValueOrDefault());
            var attributeViewModel = attributes.Where(a =>a.IsActive).ToViewModels().ToList();

            return new ItemEditViewModel
            {
                ItemViewModel = itemViewModel,
                Attributes = attributeViewModel,
                ItemHistoryModel = GetItemHistoryModel(itemViewModel, isInforHistory),
                OrderOfFields = orderFieldsHelper.OrderAllFields(attributeViewModel)
            };
        }

        private ItemHistoryViewModel GetItemHistoryModel(ItemViewModel viewModel, bool isInforHistory)
        {
            IEnumerable<ItemHistoryDbModel> history = this.getItemHistoryQueryHandler.Search(new GetItemHistoryParameters()
            {
                ItemId = viewModel.ItemId
            });

            ItemHierarchyClassHistoryAllModel itemHierarchyHistory = this.getItemHierarchyHistoryQueryHandler.Search(new GetItemHierarchyClassHistoryParameters()
            {
                ItemId = viewModel.ItemId
            });

            IEnumerable<ItemInforHistoryDbModel> inforHistory = this.getItemInforHistoryQueryHandler.Search(new GetItemInforHistoryParameters()
            {
                ItemId = viewModel.ItemId
            });

            if (isInforHistory)
            {
                IEnumerable<AttributeInforModel> attributes = getInforAttributesQueryHandler.Search(new EmptyQueryParameters<IEnumerable<AttributeInforModel>>());

                List<AttributeDisplayModel> displayAttributes = (from a in attributes
                                                                 select new AttributeDisplayModel
                                                                 {
                                                                     AttributeName = a.AttributeName,
                                                                     DisplayName = a.DisplayName
                                                                 }).ToList();

                return itemHistoryBuilder.BuildItemHistory(this.historyModelTransformer.TransformInforHistory(inforHistory, displayAttributes), new ItemHierarchyClassHistoryAllModel(), displayAttributes, viewModel);
            }
            else
            {
                List<AttributeModel> attributes = getAttributesQueryHandler.Search(new EmptyQueryParameters<IEnumerable<AttributeModel>>()).ToList();

                List<AttributeDisplayModel> displayAttributes = (from a in attributes
                                                                 select new AttributeDisplayModel
                                                                 {
                                                                     AttributeName = a.AttributeName,
                                                                     DisplayName = a.DisplayName
                                                                 }).ToList();

                return itemHistoryBuilder.BuildItemHistory(this.historyModelTransformer.ToViewModels(history), itemHierarchyHistory, displayAttributes, viewModel);
            }
        }


        [HttpGet]
        public void ItemTemplateNewExporter()
        {
            List<String> selectedColumnNames = null;

            if (Session["SelectedColumnNames"] != null)
            {
                selectedColumnNames = ((string[])Session["SelectedColumnNames"]).ToList();
                logger.Debug($"sessionID={this.Session.SessionID}, SelectedColumnNames={JsonConvert.SerializeObject(selectedColumnNames)}");
            }
            else
            {
                logger.Debug($"sessionID={this.Session.SessionID}, SelectedColumnNames was null");
            }

            var newItemTemplateExporter = exporterService.GetItemTemplateNewExporter(selectedColumnNames, true, true);
            newItemTemplateExporter.Export();

            SendForDownload(newItemTemplateExporter.ExportModel.ExcelWorkbook, newItemTemplateExporter.ExportModel.ExcelWorkbook.CurrentFormat, "NewItem", "IconImportTemplate");
        }

        [HttpPost]
        public void ItemSearchExport(string[] selectedColumnNames)
        {
            Session["SelectedColumnNames"] = selectedColumnNames;
            logger.Debug($"sessionID={this.Session.SessionID}, SelectedColumnNames={JsonConvert.SerializeObject(selectedColumnNames)}");

        }

        [HttpPost]
        public void ExportSelectedItems(List<int> selectedIds, string[] selectedColumnNames)
        {
            var result = GetMultipleSearchResults(selectedIds);

            Session["SelectedExportList"] = result;
            Session["SelectedColumnNames"] = selectedColumnNames;
        }

        [HttpGet]
        public void ExportSelectedItems(bool exportAllAttributes)
        {

            if (Session["SelectedExportList"] == null)
            {
                logger.Debug($"sessionID={this.Session.SessionID}, SelectedExportList was null");
                return;
            }
            else
            {
                logger.Debug($"sessionID={this.Session.SessionID}, SelectedExportList={JsonConvert.SerializeObject(Session["SelectedExportList"])}");
            }

            List<string> selectedColumnNames = null;
            if (Session["SelectedColumnNames"] != null)
            {
                selectedColumnNames = ((string[])Session["SelectedColumnNames"]).ToList();
                logger.Debug($"sessionID={this.Session.SessionID}, SelectedColumnNames={JsonConvert.SerializeObject(selectedColumnNames)}");
            }
            else
            {
                logger.Debug($"sessionID={this.Session.SessionID}, SelectedColumnNames was null");
            }

            ItemResultModel results = Session["SelectedExportList"] as ItemResultModel;

            var newItemTemplateExporter = exporterService.GetItemTemplateNewExporter(selectedColumnNames, exportAllAttributes);
            newItemTemplateExporter.Export(results.Records.ToList());

            SendForDownload(newItemTemplateExporter.ExportModel.ExcelWorkbook, newItemTemplateExporter.ExportModel.ExcelWorkbook.CurrentFormat, "Item", "IconExport");

            Session["SelectedExportList"] = null;
        }

        [HttpGet]
        public void ItemSearchExport(bool exportAllAttributes)
        {
            List<String> selectedColumnNames = null;

            if (Session["SelectedColumnNames"] != null)
            {
                selectedColumnNames = ((string[])Session["SelectedColumnNames"]).ToList();
                logger.Debug($"sessionID={this.Session.SessionID}, SelectedColumnNames={JsonConvert.SerializeObject(selectedColumnNames)}");
            }
            else
            {
                logger.Debug($"sessionID={this.Session.SessionID}, SelectedColumnNames was null");
            }

            int maxNumberofItemsRecordsToBeExported = AppSettingsAccessor.GetIntSetting("maxNumberOfItemsRecordToBeExported", 10000);
            ItemResultModel result = GetSearchResults(maxNumberofItemsRecordsToBeExported, 0, null, null);

            var newItemTemplateExporter = exporterService.GetItemTemplateNewExporter(selectedColumnNames, exportAllAttributes);
            newItemTemplateExporter.Export(result.Records.ToList());
            SendForDownload(newItemTemplateExporter.ExportModel.ExcelWorkbook, newItemTemplateExporter.ExportModel.ExcelWorkbook.CurrentFormat, "Item", "IconExport");
        }

        private void SendForDownload(Workbook document, WorkbookFormat excelFormat, string source, string filePrefix)
        {
            string documentFileNameRoot = $"{filePrefix}_{source}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";

            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=" + documentFileNameRoot);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.SetCookie(new HttpCookie("fileDownload", "true") { Path = "/" });
            document.SetCurrentFormat(excelFormat);
            document.Save(Response.OutputStream);
            Response.End();
        }

        private ItemResultModel GetMultipleSearchResults(List<int> itemIds)
        {
            var dbResponse = getItemsByIdHandler.Search(new GetItemsByIdSearchParameters { ItemIds = itemIds });

            var dynamicObjects = dbResponse.Items
                .Select(i =>
                {
                    Dictionary<string, object> itemAttributes = JsonConvert.DeserializeObject<Dictionary<string, object>>(i.ItemAttributesJson);
                    itemAttributes["ItemId"] = i.ItemId;
                    itemAttributes["ItemTypeDescription"] = i.ItemTypeDescription;
                    itemAttributes["ScanCode"] = i.ScanCode;
                    itemAttributes["BarcodeType"] = i.BarcodeType;
                    itemAttributes["MerchandiseHierarchyClassId"] = i.MerchandiseHierarchyClassId;
                    itemAttributes["BrandsHierarchyClassId"] = i.BrandsHierarchyClassId;
                    itemAttributes["TaxHierarchyClassId"] = i.TaxHierarchyClassId;
                    itemAttributes["FinancialHierarchyClassId"] = i.FinancialHierarchyClassId;
                    itemAttributes["NationalHierarchyClassId"] = i.NationalHierarchyClassId;
                    itemAttributes["ManufacturerHierarchyClassId"] = i.ManufacturerHierarchyClassId;
                    itemAttributes["Brands"] = i.Brands;
                    itemAttributes["National"] = i.National;
                    itemAttributes["Merchandise"] = i.Merchandise;
                    itemAttributes["Tax"] = i.Tax;
                    itemAttributes["Financial"] = i.Financial;
                    itemAttributes["Manufacturer"] = i.Manufacturer;
                    return itemAttributes;
                });

            var result = new ItemResultModel
            {
                Records = dynamicObjects.ToList(),
                TotalRecordsCount = dbResponse.TotalRecordsCount
            };

            return result;
        }

        private ItemResultModel GetSearchResults(int top, int skip, string orderByOrder, string orderByValue, string itemId = "")
        {
            var getItemsParametersViewModel = Session["GetItemsParametersViewModel"] as GetItemsParametersViewModel;

            var getItemsParameters = new GetItemsParameters
            {
                Skip = skip,
                OrderByOrder = orderByOrder ?? "ASC",
                OrderByValue = orderByValue ?? "ItemId",
                Top = top,
                ItemAttributeJsonParameters = !String.IsNullOrWhiteSpace(itemId) ? new List<ItemSearchCriteria>()
                {
                    new ItemSearchCriteria("ItemId", AttributeSearchOperator.ExactlyMatchesAll, itemId)
                } : new List<ItemSearchCriteria>() { }
            };

            getItemsParametersViewModel.GetItemsAttributesParameters.ForEach(x =>
            {
                if (!string.IsNullOrWhiteSpace(x.AttributeValue) || (x.SearchOperator == AttributeSearchOperator.HasAttribute || x.SearchOperator == AttributeSearchOperator.DoesNotHaveAttribute))
                {
                    var criteria = new ItemSearchCriteria(x.AttributeName,
                        x.SearchOperator,
                        x.AttributeValue);

                    if (criteria.Values.Count > 0 || (x.SearchOperator == AttributeSearchOperator.HasAttribute || x.SearchOperator == AttributeSearchOperator.DoesNotHaveAttribute))
                    {
                        getItemsParameters.ItemAttributeJsonParameters.Add(criteria);
                    }
                }
            });

            var queryResult = getItemsQueryHandler.Search(getItemsParameters);

            var dynamicObjects = queryResult.Items
                .Select(i =>
                {
                    Dictionary<string, object> itemAttributes = JsonConvert.DeserializeObject<Dictionary<string, object>>(i.ItemAttributesJson);
                    itemAttributes["ItemId"] = i.ItemId;
                    itemAttributes["ItemTypeDescription"] = i.ItemTypeDescription;
                    itemAttributes["ScanCode"] = i.ScanCode;
                    itemAttributes["BarcodeType"] = i.BarcodeType;
                    itemAttributes["MerchandiseHierarchyClassId"] = i.MerchandiseHierarchyClassId;
                    itemAttributes["BrandsHierarchyClassId"] = i.BrandsHierarchyClassId;
                    itemAttributes["TaxHierarchyClassId"] = i.TaxHierarchyClassId;
                    itemAttributes["FinancialHierarchyClassId"] = i.FinancialHierarchyClassId;
                    itemAttributes["NationalHierarchyClassId"] = i.NationalHierarchyClassId;
                    itemAttributes["ManufacturerHierarchyClassId"] = i.ManufacturerHierarchyClassId;
                    itemAttributes["Brands"] = i.Brands;
                    itemAttributes["National"] = i.National;
                    itemAttributes["Merchandise"] = i.Merchandise;
                    itemAttributes["Tax"] = i.Tax;
                    itemAttributes["Financial"] = i.Financial;
                    itemAttributes["Manufacturer"] = i.Manufacturer;
                    return itemAttributes;
                });

            var result = new ItemResultModel
            {
                Records = dynamicObjects.ToList(),
                TotalRecordsCount = queryResult.TotalRecordsCount,
                Query = queryResult.Query
            };

            return result;

        }

        private Enums.WriteAccess GetWriteAccess()
        {
            bool hasWriteAccess = this.settings.WriteAccessGroups.Split(',').Any(x => this.HttpContext.User.IsInRole(x.Trim()));
            var userAccess = Enums.WriteAccess.None;

            if (hasWriteAccess)
            {
                userAccess = Enums.WriteAccess.Full;
            }

            return userAccess;
        }

        private ActionResult RequestInfo(string errMessage, HttpStatusCode statusCode)
        {
            //To prevent IIS from hijacking custom response or add the line below to web config file in <system.webServer> section
            //<httpErrors errorMode="DetailedLocalOnly" existingResponse="PassThrough"/>
            Response.TrySkipIisCustomErrors = true;

            Response.StatusCode = (int)statusCode;
            Response.StatusDescription = errMessage;
            return Json(errMessage);
        }
    }
}