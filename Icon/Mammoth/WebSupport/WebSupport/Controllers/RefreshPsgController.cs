using Icon.Common.DataAccess;
using Icon.Logging;
using Mammoth.PrimeAffinity.Library.MessageBuilders;
using Mammoth.PrimeAffinity.Library.Processors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using WebSupport.DataAccess.Models;
using WebSupport.DataAccess.Queries;
using WebSupport.DataAccess.TransferObjects;
using WebSupport.Models;
using WebSupport.PrimeAffinity;
using WebSupport.ViewModels;

namespace WebSupport.Controllers
{
    public class RefreshPsgController : Controller
    {
        private ILogger logger;
        private IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>> queryForStores;
        private IQueryHandler<GetItemPsgDataParameters, IEnumerable<ItemPsgModel>> getItemPsgDataQueryHandler;
        private IQueryHandler<GetScanCodeExistsParameters, List<ScanCodeExistsModel>> getScanCodeExistsQueryHandler;
        private IPrimeAffinityPsgProcessor<PrimeAffinityPsgProcessorParameters> primeAffinityPsgProcessor;
        private PrimeAffinitySettings settings;
        private IQueryHandler<IsRegionOnGpmParameters, bool> isRegionOnGpmQuery;
        private IQueryHandler<GetItemPsgDataForGpmRegionParameters, IEnumerable<ItemPsgModel>> getItemPsgDataForRegionOnGpmQueryHandler;

        public RefreshPsgController(
            ILogger logger,
            IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>> queryForStores,
            IQueryHandler<GetItemPsgDataParameters, IEnumerable<ItemPsgModel>> getItemPsgDataQueryHandler,
            IQueryHandler<GetScanCodeExistsParameters, List<ScanCodeExistsModel>> getScanCodeExistsQueryHandler,
            IPrimeAffinityPsgProcessor<PrimeAffinityPsgProcessorParameters> primeAffinityPsgProcessor,
            PrimeAffinitySettings settings,
            IQueryHandler<IsRegionOnGpmParameters, bool> isRegionOnGpmQuery,
            IQueryHandler<GetItemPsgDataForGpmRegionParameters, IEnumerable<ItemPsgModel>> getItemPsgDataForRegionOnGpmQueryHandler)
        {
            this.logger = logger;
            this.queryForStores = queryForStores;
            this.getItemPsgDataQueryHandler = getItemPsgDataQueryHandler;
            this.getScanCodeExistsQueryHandler = getScanCodeExistsQueryHandler;
            this.primeAffinityPsgProcessor = primeAffinityPsgProcessor;
            this.settings = settings;
            this.isRegionOnGpmQuery = isRegionOnGpmQuery;
            this.getItemPsgDataForRegionOnGpmQueryHandler = getItemPsgDataForRegionOnGpmQueryHandler;
        }

        [HttpGet]
        public ActionResult Index()
        {
            RefreshPsgViewModel viewModel = new RefreshPsgViewModel();
            viewModel.SetRegion(StaticData.WholeFoodsRegions);
            if (TempData["Errors"] != null)
            {
                viewModel.Errors = TempData["Errors"] as List<string>;
            }
            if (TempData["Success"] != null)
            {
                viewModel.Success = TempData["Success"] as bool?;
            }
            if (TempData["NonExistingScanCodesMessage"] != null)
            {
                viewModel.NonExistingScanCodesMessage = TempData["NonExistingScanCodesMessage"] as string;
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(RefreshPsgViewModel viewModel)
        {
            try
            {
                var region = StaticData.WholeFoodsRegions.ElementAt(viewModel.RegionIndex);
                var businessUnitIds = viewModel.Stores.Select(s => int.Parse(s)).ToList();
                var scanCodes = viewModel
                    .Items
                    .Split()
                    .Where(s => !String.IsNullOrWhiteSpace(s))
                    .ToList();
                var scanCodeExistsModels = getScanCodeExistsQueryHandler.Search(new GetScanCodeExistsParameters { ScanCodes = scanCodes });
                var existingScanCodes = scanCodeExistsModels
                    .Where(sc => sc.Exists)
                    .Select(sc => sc.ScanCode)
                    .ToList();

                bool isRegionOnRpm = isRegionOnGpmQuery.Search(new IsRegionOnGpmParameters { Region = region });

                if (existingScanCodes.Any())
                {
                    IEnumerable<ItemPsgModel> itemPsgModels;

                    if (!isRegionOnRpm)
                    {
                        itemPsgModels = getItemPsgDataQueryHandler.Search(new GetItemPsgDataParameters
                        {
                            Region = region,
                            ScanCodes = existingScanCodes,
                            BusinessUnitIds = businessUnitIds,
                            ExcludedPsNumbers = settings.ExcludedPSNumbers,
                            PriceTypes = settings.PriceTypes
                        });
                    }
                    else
                    {
                        itemPsgModels = getItemPsgDataForRegionOnGpmQueryHandler.Search(new GetItemPsgDataForGpmRegionParameters
                        {
                            Region = region,
                            ScanCodes = existingScanCodes,
                            BusinessUnitIds = businessUnitIds,
                            ExcludedPsNumbers = settings.ExcludedPSNumbers
                        });

                    }

                    var primeAffinityModels = ConvertToPsgModels(itemPsgModels);

                    foreach (var primeAffinityModelGroup in primeAffinityModels.GroupBy(m => m.MessageAction))
                    {
                        primeAffinityPsgProcessor.SendPsgs(new PrimeAffinityPsgProcessorParameters
                        {
                            Region = region,
                            MessageAction = primeAffinityModelGroup.Key,
                            PrimeAffinityMessageModels = primeAffinityModelGroup
                        });
                    }

                    TempData["Success"] = true;
                }

                var nonExistingScanCodes = scanCodeExistsModels
                    .Where(sc => !sc.Exists)
                    .ToList();
                if (nonExistingScanCodes.Any())
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (var nonExistingScanCode in nonExistingScanCodes)
                    {
                        stringBuilder.Append($"{nonExistingScanCode.ScanCode} does not exist so no messages were sent for it.");
                    }
                    TempData["NonExistingScanCodesMessage"] = stringBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                TempData["Errors"] = $"Unexpected error occurred, check logs for more details. Error:{ex.Message}";
                logger.Error(JsonConvert.SerializeObject(new
                {
                    Error = ex
                }));
            }
            return RedirectToAction("Index");
        }

        private IEnumerable<PrimeAffinityMessageModel> ConvertToPsgModels(IEnumerable<ItemPsgModel> psgData)
        {
            return psgData.Select(p => new PrimeAffinityMessageModel
            {
                BusinessUnitID = p.BusinessUnitId,
                InternalPriceObject = new { Source = "Mammoth Web Support", SourceData = p.SourceData },
                ItemID = p.ItemId,
                ItemTypeCode = p.ItemTypeCode,
                MessageAction = (Icon.Esb.Schemas.Wfm.Contracts.ActionEnum)Enum.Parse(typeof(Icon.Esb.Schemas.Wfm.Contracts.ActionEnum), p.MessageAction),
                Region = p.Region,
                ScanCode = p.ScanCode,
                StoreName = p.StoreName,
            });
        }

        [HttpGet]
        public JsonResult Stores(int regionCode)
        {
            if (StaticData.WholeFoodsRegions.Length > 0 && regionCode < StaticData.WholeFoodsRegions.Length)
            {
                var chosenRegion = StaticData.WholeFoodsRegions.ElementAt(regionCode);
                if (String.IsNullOrWhiteSpace(chosenRegion))
                {
                    throw new ArgumentException($"invalid region '{chosenRegion ?? "null"}'", nameof(chosenRegion));
                }
                else
                {
                    var stores = queryForStores.Search(new GetStoresForRegionParameters { Region = chosenRegion });
                    return Json(stores, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(regionCode), $"invalid region code {regionCode}");
            }
        }
    }
}