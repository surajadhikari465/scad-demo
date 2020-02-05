using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.Common.Utility;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Extensions;
using Icon.Web.Mvc.InfragisticsHelpers;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Utility;
using Infragistics.Documents.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Icon.Common.Models;
using Icon.Common.Validators.ItemAttributes;
using Constants = Icon.Common.Constants;
using Icon.Web.Mvc.Attributes;
using Icon.Web.Mvc.Utility.ItemHistory;
using Icon.Web.DataAccess.Infrastructure.ItemSearch;

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
        private IQueryHandler<GetBulkItemUploadErrorsPrameters, List<BulkUploadErrorModel>> getBulkUploadErrorsQueryHandler;
        private IManagerHandler<UpdateItemManager> updateItemManagerHandler;
        private IInfragisticsHelper infragisticsHelper;
        private IQueryHandler<GetItemPropertiesFromMerchHierarchyParameters, MerchDependentItemPropertiesModel> getItemPropertiesFromMerchQueryHandler;
        private IManagerHandler<AddItemManager> addItemManagerHandler;
        private IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>> getBarcodeTypeQueryHandler;
        private IExcelExporterService exporterService;
        private IManagerHandler<BulkItemUploadManager> bulkItemUploadManagerHandler;
        private IItemAttributesValidatorFactory itemAttributesValidatorFactory;
        private IItemHistoryBuilder itemHistoryBuilder;
        private IQueryHandler<GetBulkItemUploadStatusParameters, List<BulkItemUploadStatusModel>> getBulkUploadStatusQueryHandler;
        private IQueryHandler<GetItemInforHistoryParameters, IEnumerable<ItemInforHistoryDbModel>> getItemInforHistoryQueryHandler;
        private IQueryHandler<GetBulkItemUploadByIdParameters, BulkItemUploadStatusModel> getBulkUploadByIdQueryHandler;

        private IHistoryModelTransformer historyModelTransformer;
        private readonly IQueryHandler<GetItemsByIdSearchParameters, GetItemsResult> getItemsByIdHandler;

        public ItemController(
            ILogger logger,
            IQueryHandler<GetItemsParameters, GetItemsResult> getItemsQueryHandler,
            IQueryHandler<GetItemHistoryParameters, IEnumerable<ItemHistoryDbModel>> getItemHistoryQueryHandler,
            IQueryHandler<GetItemHierarchyClassHistoryParameters, ItemHierarchyClassHistoryAllModel> getItemHierarchyHistoryQueryHandler,
            IQueryHandler<GetItemParameters, ItemDbModel> getItemQueryHandler,
            IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>> getAttributesQueryHandler,
            IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeInforModel>>, IEnumerable<AttributeInforModel>> getInforAttributesQueryHandler,
            IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>> getHierarchyClassesQueryHandler,
            IQueryHandler<GetBulkItemUploadByIdParameters, BulkItemUploadStatusModel> getBulkUploadByIdQueryHandler,
            IManagerHandler<UpdateItemManager> updateItemManagerHandler,
            IInfragisticsHelper infragisticsHelper,
            IManagerHandler<AddItemManager> addItemManagerHandler,
            IQueryHandler<GetItemPropertiesFromMerchHierarchyParameters, MerchDependentItemPropertiesModel> getItemPropertiesFromMerchQueryHandler,
            IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>> getBarcodeTypeQueryHandler,
            IQueryHandler<GetBulkItemUploadStatusParameters, List<BulkItemUploadStatusModel>> getBulkUploadStatusQueryHandler,
            IQueryHandler<GetBulkItemUploadErrorsPrameters, List<BulkUploadErrorModel>> getBulkUploadErrorsQueryHandler,
            IExcelExporterService exporterService,
            IItemAttributesValidatorFactory itemAttributesValidatorFactory,
            IManagerHandler<BulkItemUploadManager> bulkItemUploadManagerHandler,
            IQueryHandler<GetItemInforHistoryParameters,
            IEnumerable<ItemInforHistoryDbModel>> getItemInforHistoryQueryHandler,
            IconWebAppSettings settings,
            IItemHistoryBuilder itemHistoryBuilder,
            IHistoryModelTransformer historyModelTransformer,
            IQueryHandler<GetItemsByIdSearchParameters, GetItemsResult> getItemsByIdHandler)
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
            this.bulkItemUploadManagerHandler = bulkItemUploadManagerHandler;
            this.getItemHierarchyHistoryQueryHandler = getItemHierarchyHistoryQueryHandler;
            this.itemHistoryBuilder = itemHistoryBuilder;
            this.getBulkUploadStatusQueryHandler = getBulkUploadStatusQueryHandler;
            this.getItemInforHistoryQueryHandler = getItemInforHistoryQueryHandler;
            this.historyModelTransformer = historyModelTransformer;
            this.getItemsByIdHandler = getItemsByIdHandler;
            this.getBulkUploadErrorsQueryHandler = getBulkUploadErrorsQueryHandler;
            this.getBulkUploadByIdQueryHandler = getBulkUploadByIdQueryHandler;
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

            if(getItemsParametersViewModel == null)
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

            var viewModel = new ItemDetailViewModel
            {
                ItemViewModel = itemViewModel,
                Attributes = attributes.ToViewModels(),
                ItemHistoryModel = this.GetItemHistoryModel(itemViewModel, false)
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
            var filteredAttributeModels = attributeModels.Where(a => a.AttributeName != Constants.Attributes.ProhibitDiscount);

            itemCreateViewModel.Attributes = filteredAttributeModels.ToViewModels();
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

            return new ItemEditViewModel
            {
                ItemViewModel = itemViewModel,
                Attributes = attributes.ToViewModels().ToList(),
                ItemHistoryModel = GetItemHistoryModel(itemViewModel, isInforHistory)
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
            var newItemTemplateExporter = exporterService.GetItemTemplateNewExporter(null, true, true);
            newItemTemplateExporter.Export();

            SendForDownload(newItemTemplateExporter.ExportModel.ExcelWorkbook, newItemTemplateExporter.ExportModel.ExcelWorkbook.CurrentFormat, "NewItem", "IconImportTemplate_");
        }

        [HttpPost]
        public void ItemSearchExport(string[] selectedColumnNames)
        {
            Session["SelectedColumnNames"] = selectedColumnNames;
            logger.Debug($"sessionID={this.Session.SessionID}, SelectedColumnNames={JsonConvert.SerializeObject(selectedColumnNames)}");

        }

        [HttpPost]
        public void ExportSelectedItems(List<int> selectedIds)
        {
            var result = GetMultipleSearchResults(selectedIds);

            Session["SelectedExportList"] = result;
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


        [HttpGet]
        public ActionResult BulkUploadStatus(int rowCount)
        {
            var data = this.getBulkUploadStatusQueryHandler.Search(new GetBulkItemUploadStatusParameters() { RowCount = rowCount });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [WriteAccessAuthorize]
        public ActionResult BulkUpload()
        {
            BulkUploadViewModel bulkUploadViewModel = new BulkUploadViewModel();
			ViewData["BulkUploadType"] = "Item";
			return View(bulkUploadViewModel);
        }

        [HttpGet]
        public ActionResult BulkUploadErrors(int Id)
        {
            var model = getBulkUploadByIdQueryHandler.Search(new GetBulkItemUploadByIdParameters {BulkItemUploadId = Id});

            return View(model);
        }

        [HttpGet]
        public ActionResult GetBulkUploadErrors(int Id)
        {
            var parameters = new GetBulkItemUploadErrorsPrameters() {BulkItemUploadId = Id};
            var data = this.getBulkUploadErrorsQueryHandler.Search(parameters);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [WriteAccessAuthorize]
        public ActionResult UploadFiles()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                var uploadedFileName = string.Empty;
                var uploadedFileType = Request.Form["fileType"];
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        var uploadedFile = files[i];

                        if (uploadedFile == null)
                        {
                            var result = new BulkUploadResultModel { Result = "Error", Message = "No file selected" };
                            return Json(result);
                        }

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = uploadedFile.FileName.Split(new char[] { '\\' });
                            uploadedFileName = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            uploadedFileName = uploadedFile.FileName;
                        }

                        var binaryReader = new BinaryReader(uploadedFile.InputStream);
                        var uploadedData = binaryReader.ReadBytes(uploadedFile.ContentLength);

                        try
                        {

                            var manager = new BulkItemUploadManager
                            {
                                BulkItemUploadModel = new BulkItemUploadModel
                                {
                                    FileName = uploadedFileName,
                                    FileContent = uploadedData,
                                    FileModeType = uploadedFileType == "UpdateExisting" ? 1 : 0,
                                    UploadedBy = User.Identity.Name
                                }
                            };
                            bulkItemUploadManagerHandler.Execute(manager);
                            var successMessage = $"File name: {uploadedFileName} uploaded successfully.";
                            var result = new BulkUploadResultModel { Result = "Success", Message = successMessage };
                            return Json(result);
                        }
                        catch (Exception ex)
                        {
                            var result = new BulkUploadResultModel { Result = "Error", Message = $"Error occurred. Error details: {ex.Message}" };
                            return Json(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    var result = new BulkUploadResultModel { Result = "Error", Message = $"Error occurred. Error details: {ex.Message}" };
                    return Json(result);
                }
            }
            else
            {
                var result = new BulkUploadResultModel { Result = "Error", Message = "No files selected" };

                return Json(result);
            }

            return null;
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

    }
}