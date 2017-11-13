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

namespace WebSupport.Controllers
{
    public class CheckPointRequestToGPMController : Controller
    {
        private ILogger logger;
       private IEsbService<CheckPointRequestViewModel> checkPointRequestMessageService;
        private IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>> queryForStores;
        public CheckPointRequestToGPMController(
            ILogger logger,
            IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>> storesQuery,
            IEsbService<CheckPointRequestViewModel> service)
        {
            this.logger = logger;
            this.queryForStores = storesQuery;
            this.checkPointRequestMessageService = service;
        }

        // GET: CheckPointRequestFromGPM
        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = new CheckPointRequestViewModel();
            viewModel.SetRegions(StaticData.WholeFoodsRegions);
            return View(viewModel);
        }

        [HttpPost]
        // POST: CheckPointRequestFromGPM
        public ActionResult Index(CheckPointRequestViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var response = checkPointRequestMessageService.Send(viewModel);
                if (response.Status == EsbServiceResponseStatus.Failed)
                {
                    ViewBag.Error = response.ErrorDetails;
                    return HandleErrorForPriceReset(viewModel);
                }
            }
            else
            {
                return HandleErrorForPriceReset(viewModel);
            }

            return RedirectToAction("Index");
        }

        private ActionResult HandleErrorForPriceReset(CheckPointRequestViewModel viewModel)
        {
            //re-populate options for view model
            var existingRegionChoice = viewModel.RegionIndex;
            viewModel.SetRegions(StaticData.WholeFoodsRegions);
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