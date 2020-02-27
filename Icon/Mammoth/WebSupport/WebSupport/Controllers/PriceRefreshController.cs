using Icon.Common.DataAccess;
using Icon.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private IQueryHandler<GetMammothItemIdsToScanCodesParameters, List<string>> searchScanCodes;

        public PriceRefreshController(
            ILogger logger,
            IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>> queryForStores,
            IRefreshPriceService refreshPriceService,
            IQueryHandler<GetMammothItemIdsToScanCodesParameters, List<string>> searchScanCodes)
        {
            this.logger = logger;
            this.queryForStores = queryForStores;
            this.refreshPriceService = refreshPriceService;
            this.searchScanCodes = searchScanCodes;
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
                var code = HttpStatusCode.Created;
                var message = String.Empty;
                string region = StaticData.WholeFoodsRegions.ElementAt(viewModel.RegionIndex);
                List<string> systems = StaticData
                    .JustInTimeDownstreamSystems
                    .Where((s, i) => viewModel.DownstreamSystems.Contains(i))
                    .ToList();
                List<string> stores = viewModel.Stores.ToList();

                var scanCodes = !viewModel.IsItemId 
                    ? viewModel.Codes
                    : searchScanCodes.Search(new GetMammothItemIdsToScanCodesParameters { ItemIds = viewModel.Codes });

                var response = refreshPriceService.RefreshPrices(
                    region,
                    systems,
                    stores,
                    scanCodes);

                if (response.Errors != null && response.Errors.Count > 0)
                {
                    TempData["Errors"] = response.Errors;
                    code = HttpStatusCode.ExpectationFailed;
                    message = String.Join(" ", response.Errors);
                }
                else
                {
                    TempData["Success"] = true;
                    code = HttpStatusCode.OK;
                    var invalidScanCodeCount = (refreshPriceService as RefreshPriceService).InvalidScanCodeCount;
                    message = String.Format("Messages has been successfully sent. {0}", invalidScanCodeCount > 0 ? $"Invalid Scan Code count: {invalidScanCodeCount}" : String.Empty);
                }

                logger.Info(JsonConvert.SerializeObject(new
                {
                    PriceRefreshResponse = response
                }));

                return RequestInfo(code, message);
            }
            catch (Exception ex)
            {
                TempData["Errors"] = $"Unexpected error occurred, check logs for more details. Error:{ex.Message}";
                logger.Error(JsonConvert.SerializeObject(new { Error = ex }));
                return RequestInfo(HttpStatusCode.BadRequest, ex.Message);
            }
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

        private ActionResult RequestInfo(HttpStatusCode statusCode, string errMessage)
        {
            //To prevent IIS from hijacking custom response or add the line below to web config file in <system.webServer> section
            //<httpErrors errorMode="DetailedLocalOnly" existingResponse="PassThrough"/>
            Response.TrySkipIisCustomErrors = true; 

            Response.StatusCode = (int)statusCode;
            Response.StatusDescription = errMessage.Replace(System.Environment.NewLine, " ").Trim();
            return Json(Response.StatusDescription);
        }
    }
}