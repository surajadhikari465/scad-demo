using Icon.Common.DataAccess;
using Icon.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebSupport.DataAccess.Queries;
using WebSupport.DataAccess.TransferObjects;
using WebSupport.Models;
using WebSupport.Services;
using WebSupport.ViewModels;

namespace WebSupport.Controllers
{
    public class PriceRefreshController : Controller
    {
        private ILogger logger;
        private IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>> queryForStores;
        private IRefreshPriceService refreshPriceService;

        public PriceRefreshController(
            ILogger logger,
            IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>> queryForStores,
            IRefreshPriceService refreshPriceService)
        {
            this.logger = logger;
            this.queryForStores = queryForStores;
            this.refreshPriceService = refreshPriceService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            PriceRefreshViewModel viewModel = new PriceRefreshViewModel();
            viewModel.SetRegionAndSystemOptions(StaticData.WholeFoodsRegions, StaticData.JustInTimeDownstreamSystems);
            if(TempData["Errors"] != null)
            {
                viewModel.Errors = TempData["Errors"] as List<string>;
            }
            if(TempData["Success"] != null)
            {
                viewModel.Success = TempData["Success"] as bool?;
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(PriceRefreshViewModel viewModel)
        {
            try
            {
                string region = StaticData.WholeFoodsRegions.ElementAt(viewModel.RegionIndex);
                List<string> systems = StaticData
                    .JustInTimeDownstreamSystems
                    .Where((s, i) => viewModel.DownstreamSystems.Contains(i))
                    .ToList();
                List<string> stores = viewModel.Stores.ToList();
                List<string> scanCodes = viewModel
                    .Items
                    .Split()
                    .Where(s => !String.IsNullOrWhiteSpace(s))
                    .ToList();

                var response = refreshPriceService.RefreshPrices(
                    region,
                    systems,
                    stores,
                    scanCodes);

                if (response.Errors != null && response.Errors.Count > 0)
                {
                    TempData["Errors"] = response.Errors;
                }
                else
                {
                    TempData["Success"] = true;
                }

                logger.Info(JsonConvert.SerializeObject(new
                {
                    PriceRefreshResponse = response
                }));
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