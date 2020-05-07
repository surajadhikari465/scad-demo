using Icon.Common.DataAccess;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Extensions;
using Icon.Web.Mvc.Attributes;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Utility;
using Infragistics.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using Icon.Common;
using Icon.Common.Models;
using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Excel;
using System.Web;
using DevTrends.MvcDonutCaching;

namespace Icon.Web.Mvc.Controllers
{
    public class AttributeController : Controller
    {
        private ILogger logger;
        private IconWebAppSettings settings;
        private IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>> getAttributesQueryHandler;
        private IQueryHandler<GetDataTypeParameters, List<DataTypeModel>> getDataTypeQueryHandler;
        private IQueryHandler<GetCharacterSetParameters, List<CharacterSetModel>> getCharacterSetQueryHandler;
        private IManagerHandler<AddAttributeManager> addAttributeManagerHandler;
        private IManagerHandler<UpdateAttributeManager> updateAttributeManagerHandler;
        private IQueryHandler<GetAttributeByAttributeIdParameters, AttributeModel> getAttributeByAttributeIdQuery;
        private IQueryHandler<GetCharacterSetsByAttributeParameters, List<AttributeCharacterSetModel>> getCharacterSetsByAttributeParameters;
        private IQueryHandler<GetPickListByAttributeParameters, List<PickListModel>> getPickListByAttributeParameters;
        private IAttributesHelper attributesHelper;
        private IExcelExporterService exporterService;
        private IDonutCacheManager cacheManager;
        private IQueryHandler<EmptyAttributesParameters, IEnumerable<AttributeModel>> getItemCountOnAttributesQueryHandler;
        private IOrderFieldsHelper orderFieldsHelper;

        public AttributeController(
            ILogger logger,
            IconWebAppSettings settings,
            IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>> getAttributesQueryHandler,
            IQueryHandler<GetDataTypeParameters, List<DataTypeModel>> getDataTypeQueryHandler,
            IQueryHandler<GetCharacterSetParameters, List<CharacterSetModel>> getCharacterSetQueryHandler,
            IManagerHandler<AddAttributeManager> addAttributeManagerHandler,
            IQueryHandler<GetAttributeByAttributeIdParameters, AttributeModel> getAttributeByAttributeIdQuery,
            IManagerHandler<UpdateAttributeManager> updateAttributeManagerHandler,
            IQueryHandler<GetCharacterSetsByAttributeParameters, List<AttributeCharacterSetModel>> getCharacterSetsByAttributeParameters,
            IQueryHandler<GetPickListByAttributeParameters, List<PickListModel>> getPickListByAttributeParameters,
            IAttributesHelper attributesHelper,
            IExcelExporterService exporterService,
            IDonutCacheManager cacheManager,
            IQueryHandler<EmptyAttributesParameters, IEnumerable<AttributeModel>> getItemCountOnAttributesQueryHandler,
            IOrderFieldsHelper orderFieldsHelper)
        {
            this.logger = logger;
            this.settings = settings;
            this.getAttributesQueryHandler = getAttributesQueryHandler;
            this.getDataTypeQueryHandler = getDataTypeQueryHandler;
            this.getCharacterSetQueryHandler = getCharacterSetQueryHandler;
            this.addAttributeManagerHandler = addAttributeManagerHandler;
            this.getAttributeByAttributeIdQuery = getAttributeByAttributeIdQuery;
            this.updateAttributeManagerHandler = updateAttributeManagerHandler;
            this.getCharacterSetsByAttributeParameters = getCharacterSetsByAttributeParameters;
            this.getPickListByAttributeParameters = getPickListByAttributeParameters;
            this.attributesHelper = attributesHelper;
            this.exporterService = exporterService;
            this.cacheManager = cacheManager;
            this.getItemCountOnAttributesQueryHandler = getItemCountOnAttributesQueryHandler;
            this.orderFieldsHelper = orderFieldsHelper;
        }

        // GET: Attribute
        public ActionResult Index()
        {
            return View(new AttributeViewModel() { UserWriteAccess = GetWriteAccess() });
        }

        [GridDataSourceAction]
        public ActionResult GridDataSource()
        {
            var model = this.getItemCountOnAttributesQueryHandler.Search(new EmptyAttributesParameters());
            return View(model.ToViewModels().AsQueryable());
        }

        [WriteAccessAuthorize]
        public ActionResult Edit(int attributeId)
        {
            AttributeViewModel viewModel = BuildEditViewModel(attributeId);

            TempData["viewModel"] = viewModel;

            return View(viewModel);
        }

        [HttpPost]
        [WriteAccessAuthorize]
        public ActionResult Edit(AttributeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = BuildEditViewModel(viewModel.AttributeId);
                InvalidateViewModel(viewModel);
                return View(viewModel);
            }

            if (viewModel.IsSpecialCharactersSelected && viewModel.SpecialCharacterSetSelected != Constants.SpecialCharactersAll)
            {
                ValidateSpecialCharacters(viewModel);
                var attribute = this.getAttributeByAttributeIdQuery.Search(new GetAttributeByAttributeIdParameters() { AttributeId = viewModel.AttributeId });

                var oldAllowed = new HashSet<char>(attribute.SpecialCharactersAllowed.Distinct().OrderBy(c => c).ToArray());
                var newAllowed = new HashSet<char>(viewModel.SpecialCharactersAllowed.Distinct().OrderBy(c => c).ToArray());

                //Check the intersection between previous and new character sets
                if (oldAllowed.Any(x => !newAllowed.Contains(x)))
                {
                    ViewData["ErrorMessages"] = new List<string>() { "Cannot remove preexisting special characters" };
                    PopulateAttributeViewModelOnError(viewModel);
                    TempData["viewModel"] = viewModel;
                    return View(viewModel);
                }
            }

            ValidatePickList(viewModel);
            ValidateDefaultValue(viewModel);
            UpdateAttributeManager manager = new UpdateAttributeManager
            {
                Attribute = new AttributeModel
                {
                    AttributeId = viewModel.AttributeId,
                    DisplayName = viewModel.DisplayName,
                    Description = viewModel.Description,
                    TraitCode = viewModel.TraitCode,
                    DataTypeId = viewModel.DataTypeId,
                    IsRequired = viewModel.IsRequired,
                    MaxLengthAllowed = viewModel.MaxLengthAllowed,
                    IsPickList = viewModel.IsPickList,
                    MinimumNumber = viewModel.MinimumNumber,
                    MaximumNumber = viewModel.MaximumNumber,
                    NumberOfDecimals = viewModel.NumberOfDecimals,
                    DefaultValue = viewModel.DefaultValue,
                    IsActive = viewModel.IsActive,
                    SpecialCharactersAllowed = viewModel.IsSpecialCharactersSelected ? (viewModel.SpecialCharacterSetSelected == Constants.SpecialCharactersAll) ? Constants.SpecialCharactersAll : viewModel.SpecialCharactersAllowed : null,
                    CharacterSetRegexPattern = attributesHelper.CreateCharacterSetRegexPattern(
                        viewModel.DataTypeId,
                        viewModel.AvailableCharacterSets,
                        viewModel.IsSpecialCharactersSelected ? (viewModel.SpecialCharacterSetSelected == Constants.SpecialCharactersAll) ? Constants.SpecialCharactersAll : viewModel.SpecialCharactersAllowed : null)
                },
                CharacterSetModelList = (viewModel.DataTypeId == (int)DataType.Text) ? viewModel.AvailableCharacterSets.Where(p => p.IsSelected == true).ToList() : null,
                PickListModel = viewModel.PickListData
            };

            try
            {
                updateAttributeManagerHandler.Execute(manager);
                this.cacheManager.ClearCacheForController("Attribute");
            }
            catch (Exception ex)
            {
                viewModel = BuildEditViewModel(viewModel.AttributeId);
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                InvalidateViewModel(viewModel);
                return View(viewModel);
            }

            viewModel = BuildEditViewModel(viewModel.AttributeId);

            ViewData["SuccessMessage"] = $"Updated attribute: {viewModel.DisplayName} successfully.";
            TempData["viewModel"] = viewModel;

            return View(viewModel);
        }

        private AttributeViewModel BuildEditViewModel(int attributeId)
        {
            var attribute = this.getAttributeByAttributeIdQuery.Search(new GetAttributeByAttributeIdParameters() { AttributeId = attributeId });
            var viewModel = new AttributeViewModel
            {
                AttributeId = attribute.AttributeId,
                AttributeName = attribute.AttributeName,
                DisplayName = attribute.DisplayName,
                Description = attribute.Description,
                IsRequired = attribute.IsRequired,
                SpecialCharactersAllowed = attribute.SpecialCharactersAllowed,
                TraitCode = attribute.TraitCode,
                DataTypeId = attribute.DataTypeId.Value,
                MaxLengthAllowed = attribute.MaxLengthAllowed,
                MaximumNumber = attribute.MaximumNumber,
                MinimumNumber = attribute.MinimumNumber,
                NumberOfDecimals = attribute.NumberOfDecimals,
                IsPickList = attribute.IsPickList,
                DefaultValue = attribute.DefaultValue,
                IsActive = attribute.IsActive,
                UserWriteAccess = GetWriteAccess(),
                Action = ActionEnum.Update,
            };

            if (attribute.SpecialCharactersAllowed != null)
            {
                viewModel.IsSpecialCharactersSelected = true;
                if (attribute.SpecialCharactersAllowed == Constants.SpecialCharactersAll)
                {
                    viewModel.SpecialCharacterSetSelected = Constants.SpecialCharactersAll;
                    viewModel.SpecialCharactersAllowed = string.Empty;
                }
                else
                {
                    viewModel.SpecialCharacterSetSelected = "Specific";
                }
            }
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>();

            if (viewModel.DataTypeId == (int)DataType.Boolean)
            {
                viewModel.AvailableDefaultValuesForBoolean = GetAvailableDefaultValuesForBoolean();
            }

            PopulateListsForAttributeModel(viewModel);

            return viewModel;
        }

        [WriteAccessAuthorize]
        public ActionResult Create()
        {
            AttributeViewModel attributeViewModel = BuildAttributeModel(string.Empty, true);
            attributeViewModel.Action = ActionEnum.Add;

            return View(attributeViewModel);
        }

        [HttpPost]
        [WriteAccessAuthorize]
        public ActionResult Create(AttributeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                PopulateAttributeViewModelOnError(viewModel);
                InvalidateViewModel(viewModel);
                return View(viewModel);
            }

            ValidatePickList(viewModel);
            ValidateDefaultValue(viewModel);
            ValidateSpecialCharacters(viewModel);
            AddAttributeManager manager = new AddAttributeManager
            {
                Attribute = new AttributeModel
                {
                    AttributeId = viewModel.AttributeId,
                    DisplayName = viewModel.DisplayName,
                    AttributeName = GetAttributeNameFromDisplayName(viewModel.DisplayName),
                    IsRequired = viewModel.IsRequired,
                    Description = viewModel.Description,
                    TraitCode = viewModel.TraitCode,
                    DataTypeId = viewModel.DataTypeId,
                    MaxLengthAllowed = viewModel.MaxLengthAllowed,
                    IsPickList = viewModel.IsPickList,
                    MinimumNumber = viewModel.MinimumNumber,
                    MaximumNumber = viewModel.MaximumNumber,
                    NumberOfDecimals = viewModel.NumberOfDecimals,
                    DefaultValue = viewModel.DefaultValue,
                    SpecialCharactersAllowed = viewModel.IsSpecialCharactersSelected ? (viewModel.SpecialCharacterSetSelected == Constants.SpecialCharactersAll) ? Constants.SpecialCharactersAll : viewModel.SpecialCharactersAllowed : null,
                    CharacterSetRegexPattern = attributesHelper.CreateCharacterSetRegexPattern(
                        viewModel.DataTypeId,
                        viewModel.AvailableCharacterSets,
                        viewModel.IsSpecialCharactersSelected ? (viewModel.SpecialCharacterSetSelected == Constants.SpecialCharactersAll) ? Constants.SpecialCharactersAll : viewModel.SpecialCharactersAllowed : null)
                },
                CharacterSetModelList = (viewModel.DataTypeId == (int)DataType.Text) ? viewModel.AvailableCharacterSets.Where(p => p.IsSelected == true).ToList() : null,
                PickListModel = viewModel.IsPickList ? (viewModel.PickListData == null) ? null : viewModel.PickListData.Where(p => p.PickListValue != null).ToList() : null
            };

            try
            {
                addAttributeManagerHandler.Execute(manager);
                this.cacheManager.ClearCacheForController("Attribute");
            }
            catch (Exception ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                List<string> allErrors = new List<string>() { ex.Message };
                ViewData["ErrorMessages"] = allErrors;
                PopulateAttributeViewModelOnError(viewModel);
                TempData["viewModel"] = viewModel;
                return View(viewModel);
            }

            ViewData["SuccessMessage"] = $"Created attribute: {viewModel.DisplayName} successfully.";
            ModelState.Clear();

            return View(BuildAttributeModel());
        }

        private string GetAttributeNameFromDisplayName(string displayName)
        {
            return Regex.Replace(displayName, @"[^0-9a-zA-Z]+", "");
        }

        private void PopulateAttributeViewModelOnError(AttributeViewModel viewModel)
        {
            viewModel.AvailableDataTypes = GetAvailableDataTypes();

            if (viewModel.SpecialCharactersAllowed == null)
            {
                viewModel.SpecialCharactersAllowed = string.Empty;
            }

        }

        private AttributeViewModel BuildAttributeModel(string specialCharactersAllowed = null, bool shouldAllCharacterSetsBeSelected = false, int? dataTypeId = null)
        {
            AttributeViewModel attributeViewModel = new AttributeViewModel(string.Empty);
            PopulateListsForAttributeModel(attributeViewModel, shouldAllCharacterSetsBeSelected);

            return attributeViewModel;
        }

        private void PopulateListsForAttributeModel(AttributeViewModel attributeViewModel, bool shouldAllCharacterSetsBeSelected = false)
        {
            List<CharacterSetModel> characterSetModels = getCharacterSetQueryHandler.Search(new GetCharacterSetParameters());

            if (shouldAllCharacterSetsBeSelected)
            {
                attributeViewModel.AvailableCharacterSets = characterSetModels.Select(c => { c.IsSelected = true; return c; }).ToList();
            }
            else
            {
                attributeViewModel.AvailableCharacterSets = characterSetModels;
            }

            attributeViewModel.AvailableDataTypes = GetAvailableDataTypes();

            if (attributeViewModel.SpecialCharactersAllowed == null)
            {
                attributeViewModel.SpecialCharactersAllowed = string.Empty;
            }
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

        public ActionResult GetDataTypeView(int? dataTypeId, int? attributeId)
        {
            AttributeViewModel attributeViewModel;

            if (!TempData.ContainsKey("viewModel"))
            {
                attributeViewModel = BuildAttributeModel(string.Empty, true, dataTypeId);
                attributeViewModel.IsSpecialCharactersSelected = true;
                attributeViewModel.SpecialCharacterSetSelected = Constants.SpecialCharactersAll;
            }
            else
            {
                attributeViewModel = (AttributeViewModel)TempData["viewModel"];
            }

            if (!TempData.ContainsKey("IsError"))
            {
                if (attributeId.HasValue && attributeId.Value > 0)
                {
                    var characterSets = this.getCharacterSetsByAttributeParameters.Search(new GetCharacterSetsByAttributeParameters()
                    {
                        AttributeId = attributeId.Value
                    });
                    var pickList = this.getPickListByAttributeParameters.Search(new GetPickListByAttributeParameters()
                    {
                        AttributeId = attributeId.Value
                    });

                    if (characterSets.Any())
                    {
                        int characterSetCount = 0;
                        foreach (AttributeCharacterSetModel characterset in characterSets)
                        {
                            characterSetCount = 0;
                            foreach (var charModel in attributeViewModel.AvailableCharacterSets)
                            {
                                if (charModel.CharacterSetId.Equals(characterset.CharacterSetModel.CharacterSetId))
                                {
                                    attributeViewModel.AvailableCharacterSets[characterSetCount].IsSelected = true;
                                    break;
                                }

                                characterSetCount++;
                            }
                        }
                    }

                    if (pickList.Any())
                    {
                        attributeViewModel.PickListData = pickList;
                        if (attributeViewModel.IsPickList)
                        {
                            attributeViewModel.IsPickList = true;
                        }
                    }
                }
            }

            switch (dataTypeId)
            {
                case (int)DataType.Text:
                    return PartialView("~/Views/Attribute/Shared/_DataTypeText.cshtml", attributeViewModel);
                case (int)DataType.Number:
                    return PartialView("~/Views/Attribute/Shared/_DataTypeNumber.cshtml", attributeViewModel);

                default:
                    return Content("");
            }
        }

        [DonutOutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Server, VaryByParam = "")]
        public ActionResult All()
        {
            var attributes = getAttributesQueryHandler
                .Search(new EmptyQueryParameters<IEnumerable<AttributeModel>>())
                .Select(a => new AttributeViewModel(a)).Where(a => a.IsActive)
                .ToList();

            Dictionary<string, string> orderedFields = orderFieldsHelper.OrderAllFields(attributes);

            return Json(new { Attributes = attributes, DefaultFields = orderedFields }, JsonRequestBehavior.AllowGet);
        }

        List<SelectListItem> GetAvailableDataTypes()
        {
            return getDataTypeQueryHandler.Search(new GetDataTypeParameters())
                .Select(e => new SelectListItem { Value = e.DataTypeId.ToString(), Text = e.DataType })
                .ToList();
        }

        List<SelectListItem> GetAvailableDefaultValuesForBoolean()
        {
            return new List<SelectListItem>()
            {   new SelectListItem{Text ="", Value = null},
                new SelectListItem{Text ="True", Value = "true"},
                new SelectListItem{Text ="False", Value = "false"}
            };
        }

        public void Export()
        {
            List<AttributeViewModel> attributes = getItemCountOnAttributesQueryHandler.Search(new EmptyAttributesParameters())
                .Select(a => new AttributeViewModel(a))
                .ToList();

            if (attributes.Any())
            {
                List<CharacterSetModel> characterSetModels =
                    getCharacterSetQueryHandler.Search(new GetCharacterSetParameters());
                for (int attributeModelCount = 0; attributeModelCount < attributes.Count; attributeModelCount++)
                {
                    var characterSets = getCharacterSetsByAttributeParameters.Search(new GetCharacterSetsByAttributeParameters()
                    {
                        AttributeId = attributes[attributeModelCount].AttributeId
                    });
                    if (characterSets.Any())
                    {
                        foreach (AttributeCharacterSetModel characterset in characterSets)
                        {
                            attributes[attributeModelCount].AvailableCharacterSets.Add(characterset.CharacterSetModel);
                        }
                    }
                }
            }

            var attributeExporter = exporterService.GetAttributeExporter();
            attributeExporter.ExportData = attributes.ToList();
            attributeExporter.Export();

            ExcelHelper.SendForDownload(Response, attributeExporter.ExportModel.ExcelWorkbook, attributeExporter.ExportModel.ExcelWorkbook.CurrentFormat, "Attributes");
        }

        void InvalidateViewModel(AttributeViewModel viewModel)
        {
            if (viewModel == null) return;

            TempData["viewModel"] = viewModel;
            ViewData["ErrorMessages"] = ModelState.Values.SelectMany(v => v.Errors).Select(s => s.ErrorMessage).Distinct().ToList();
            ModelState.Clear();
        }

        void ValidatePickList(AttributeViewModel viewModel)
        {
            if (viewModel != null && viewModel.PickListData != null)
            {
                foreach (var item in viewModel.PickListData)
                {
                    item.PickListValue = String.IsNullOrWhiteSpace(item.PickListValue)
                                         ? null
                                         : item.PickListValue.Trim();
                }

                viewModel.PickListData = viewModel.IsPickList
                    ? viewModel.PickListData?.Where(x => !x.IsPickListSelectedForDelete)
                      .Where(x => !String.IsNullOrEmpty(x.PickListValue))
                      .GroupBy(x => x.PickListValue, StringComparer.InvariantCultureIgnoreCase)
                      .Select(x => x.First())
                      .ToList()
                    : null;

                viewModel.IsPickList = (viewModel.PickListData != null && viewModel.PickListData.Count > 0);
            }
        }

        void ValidateDefaultValue(AttributeViewModel viewModel)
        {
            if (viewModel != null)
            {
                if (String.IsNullOrWhiteSpace(viewModel.DefaultValue))
                {
                    viewModel.DefaultValue = null;
                }
                else
                {
                    viewModel.DefaultValue = viewModel.DefaultValue.Trim();
                }
            }
        }

        void ValidateSpecialCharacters(AttributeViewModel viewModel)
        {
            if (viewModel != null && viewModel.IsSpecialCharactersSelected && viewModel.SpecialCharacterSetSelected != Constants.SpecialCharactersAll)
            {
                viewModel.SpecialCharactersAllowed = new string(viewModel.SpecialCharactersAllowed.Replace(" ", String.Empty).Distinct().OrderBy(x => x).ToArray());
            }
        }       
    }
}