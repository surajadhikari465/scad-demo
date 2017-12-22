using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSupport.Controllers
{
    public class PriceRefreshController : Controller
    {
        private IRefreshPriceService refreshPriceService;

        public PriceRefreshController(IRefreshPriceService refreshPriceService)
        {
            this.refreshPriceService = refreshPriceService;
        }

        // GET: PriceRefresh
        public ActionResult Index()
        {
            PriceRefreshViewModel viewModel = new PriceRefreshViewModel();
            if(TempData["Errors"] != null)
            {
                viewModel.Errors = TempData["Errors"] as List<string>;
            }
            if(TempData["Success"] != null)
            {
                viewModel.Success = TempData["Success"] as bool?;
            }
            return View();
        }

        public ActionResult RefreshPrices(RefreshPricesRequest request)
        {
            List<SystemViewModel> systems = request.Systems;
            List<StoreViewModel> stores = request.Stores;
            List<ScanCodeViewModel> scanCodes = request.ScanCodes;

            var response = refreshPriceService.RefreshPrices(
                systems.Select(s => s.Name).ToList(),
                stores.Select(s => s.BusinessUnit).ToList(),
                scanCodes.Select(s => s.ScanCode).ToList());

            if(response.Errors != null && response.Errors.Count > 0)
            {
                TempData["Errors"] = response.Errors;
            }
            else
            {
                TempData["Success"] = true;
            }
            return RedirectToAction("Index");
        }

        public class RefreshPricesRequest
        {
            public List<SystemViewModel> Systems { get; set; }
            public List<ScanCodeViewModel> ScanCodes { get; set; }
            public List<StoreViewModel> Stores { get; set; }
        }

        public class SystemViewModel
        {
            public string Name { get; set; }
        }

        public class ScanCodeViewModel
        {
            public string ScanCode { get; set; }
        }

        public class StoreViewModel
        {
            public string Name { get; set; }
            public string BusinessUnit { get; set; }
        }

        public class PriceRefreshViewModel
        {
            public PriceRefreshViewModel()
            {
                Errors = new List<string>();
            }

            public List<string> Errors { get; set; }
            public bool? Success { get; set; }
        }
    }

    public interface IRefreshPriceService
    {
        RefreshPriceResponse RefreshPrices(List<string> systems, List<string> stores, List<string> scanCodes);
    }

    public class RefreshPriceResponse
    {
        public List<string> Errors { get; set; }
    }
}