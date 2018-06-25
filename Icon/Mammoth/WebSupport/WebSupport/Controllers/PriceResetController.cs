using Icon.Common.DataAccess;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebSupport.DataAccess.Queries;
using WebSupport.ViewModels;
using WebSupport.DataAccess.TransferObjects;
using WebSupport.Models;
using Esb.Core.EsbServices;
using Newtonsoft.Json;

namespace WebSupport.Controllers
{
    public class PriceResetController : Controller
    {
        private ILogger logger;
        private IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>> queryForStores;
        private IEsbService<PriceResetRequestViewModel> priceMessageService;

        public PriceResetController(
            ILogger logger,
            IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>> storesQuery,
            IEsbService<PriceResetRequestViewModel> service)
        {
            this.logger = logger;
            this.queryForStores = storesQuery;
            this.priceMessageService = service;
        }

        // GET: PriceReset
        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = new PriceResetRequestViewModel();
            viewModel.SetRegionAndSystemOptions(StaticData.WholeFoodsRegions, StaticData.DownstreamSystems);
            return View(viewModel);
        }

        [HttpPost]
        // POST: PriceReset
        public ActionResult Index(PriceResetRequestViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    LogPriceResetRequest("Log PriceResetRequest Start", viewModel);
                    var response = priceMessageService.Send(viewModel);
                    LogPriceResetRequest("Log PriceResetRequest Result", viewModel, response);
                    if (response.Status == EsbServiceResponseStatus.Failed)
                    {
                        TempData["Error"] = response.ErrorDetails;
                        return HandleErrorForPriceReset(viewModel);
                    }
                }
                else
                {
                    return HandleErrorForPriceReset(viewModel);
                }
            }
            catch (Exception ex)
            {
                logger.Error(JsonConvert.SerializeObject(
                    new
                    {
                        Message = "Unexpected error occurred",
                        Controller = nameof(PriceResetController),
                        ViewModel = viewModel,
                        Exception = ex
                    }));
                TempData["Error"] = "Unexpected error occurred. Error:" + ex.ToString();
            }

            return RedirectToAction("Index");
        }

        private ActionResult HandleErrorForPriceReset(PriceResetRequestViewModel viewModel)
        {
            //re-populate options for view model
            var existingRegionChoice = viewModel.RegionIndex;
            viewModel.SetRegionAndSystemOptions(StaticData.WholeFoodsRegions, StaticData.DownstreamSystems);
            if (viewModel.RegionIndex > 0)
            {
                var chosenRegion = StaticData.WholeFoodsRegions.ElementAt(viewModel.RegionIndex);
                var queryParam = new GetStoresForRegionParameters { Region = chosenRegion };
                //TODO re-pop already selected stores (if stores are valid but items/downstream not)
                var stores = queryForStores.Search(queryParam).Select(sto => new StoreViewModel(sto));
                viewModel.SetStoreOptions(stores);
            }
            return View(viewModel);
        }

        [HttpGet]
        public JsonResult Stores(int regionCode)
        {
            if (StaticData.WholeFoodsRegions.Length > 0 && regionCode < StaticData.WholeFoodsRegions.Length)
            {
                var chosenRegion = StaticData.WholeFoodsRegions.ElementAt(regionCode);
                if (String.IsNullOrWhiteSpace(chosenRegion))
                {
                    throw new ArgumentException($"invalid region '{chosenRegion??"null"}'", nameof(chosenRegion));
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

        private void LogPriceResetRequest(string action, PriceResetRequestViewModel viewModel, EsbServiceResponse response = null)
        {
            if(response != null)
            {
                if(response.Status == EsbServiceResponseStatus.Sent)
                {
                    logger.Info(JsonConvert.SerializeObject(
                        new
                        {
                            Action = action,
                            Stores = viewModel.Stores,
                            Items = viewModel.Items,
                            Response = response
                        }));
                }
                else
                {
                    logger.Error(JsonConvert.SerializeObject(
                        new
                        {
                            Action = action,
                            Stores = viewModel.Stores,
                            Items = viewModel.Items,
                            Response = response
                        }));
                }
            }
            else
            {
                logger.Info(JsonConvert.SerializeObject(
                    new
                    {
                        Action = action,
                        Stores = viewModel.Stores,
                        Items = viewModel.Items
                    }));
            }
        }
    }
}