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
using WebSupport.ViewModels;
using Esb.Core.EsbServices;

namespace WebSupport.Controllers
{
    public class PricesAllController : Controller
    {
        private ILogger logger;
        private IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>> getStores;
        private IEsbService<PricesAllViewModel> esbMessageService;

        public PricesAllController(
            ILogger logger,
            IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>> getStores,
            IEsbService<PricesAllViewModel> esbMessageService)
        {
            this.logger = logger;
            this.getStores = getStores;
            this.esbMessageService = esbMessageService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return ResetViewModel(new PricesAllViewModel());
        }

        [HttpPost]
		public ActionResult Index(PricesAllViewModel viewModel)
        {
            try
            {
                if(!ModelState.IsValid
                    || viewModel.DownstreamSystems == null
                    || viewModel.SelectedStores == null
                    || viewModel.DownstreamSystems.Length == 0
                    || viewModel.SelectedStores.Length == 0)
                {
                    return ResetViewModel(viewModel);
                }

                var systems = StaticData.DownstreamSystems
                    .Where((x, i) => viewModel.DownstreamSystems.Contains(i))
                    .ToHashSet<string>(StringComparer.CurrentCultureIgnoreCase);

                if(systems.Contains("Pricer") && systems.Count > 1)
                {
                    TempData["Error"] = "Pricer cannot be selected with other downstream systems!";
                    return ResetViewModel(viewModel);
                }

                LogRequest("ActivePriceRequest Start", viewModel);
                var response = esbMessageService.Send(viewModel);
                LogRequest("ActivePriceRequest Result", viewModel, response);

                if (response.Status == EsbServiceResponseStatus.Failed)
				{
				    TempData["Error"] = response.ErrorDetails;
					return ResetViewModel(viewModel);
				}

                return null;
            }
            catch (Exception ex)
            {
                logger.Error(JsonConvert.SerializeObject(
					new
					{
						Message = "Unexpected error occurred.",
						Controller = nameof(PricesAllController),
						ViewModel = viewModel,
						Exception = ex
					}));

				TempData["Error"] = "Unexpected error occurred. Error: " + ex.ToString();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
		public JsonResult Stores(int regionIndex)
		{
            try
            {
                if(regionIndex <= 0)
                {
                    return Json(new List<StoreTransferObject>(), JsonRequestBehavior.AllowGet);
                }

                var allRegions = StaticData.WholeFoodsRegions;

                if(allRegions.Length > 0 && regionIndex < allRegions.Length)
                {
                    if (String.IsNullOrWhiteSpace(allRegions[regionIndex]))
				    {
					    throw new Exception($"Invalid region '{allRegions[regionIndex] ?? "null"}'");
				    }
				    else
				    {
					    var stores = getStores.Search(new GetStoresForRegionParameters { Region = allRegions[regionIndex] });
					    return Json(stores, JsonRequestBehavior.AllowGet);
				    }
                }
                else
                {
                    throw new Exception( $"Invalid region code {regionIndex}");
                }
            }
            catch(Exception)
            {
                throw;
            }
		}

        private void LogRequest(string action, PricesAllViewModel viewModel, EsbServiceResponse response = null)
		{
			if(response != null)
			{
		        logger.Info(JsonConvert.SerializeObject(
				    new
					{
					    Action = action,
						Stores = viewModel.Stores,
						Response = response
				    }));
			}
			else
			{
				logger.Info(JsonConvert.SerializeObject(
					new
					{
						Action = action,
						Stores = viewModel.Stores,
					}));
			}
		}

        private ActionResult ResetViewModel(PricesAllViewModel viewModel)
        {
            viewModel.SetRegionAndSystems(StaticData.WholeFoodsRegions.Where(x => String.Compare(x, "TS", StringComparison.InvariantCultureIgnoreCase) != 0), StaticData.DownstreamSystems);

            if(viewModel.RegionIndex > 0)
            {
                var regionParam = new GetStoresForRegionParameters { Region = viewModel.Regions[viewModel.RegionIndex].Text };
                var stores = getStores.Search(regionParam).Select(x => new StoreViewModel(x)).ToArray();
                viewModel.SetStoreOptions(stores);
            }

            return View(viewModel);   
        }
    }
}