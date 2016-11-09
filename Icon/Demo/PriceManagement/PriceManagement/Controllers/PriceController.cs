using PriceManagement.DataAccess;
using PriceManagement.Extensions;
using PriceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PriceManagement.Controllers
{
    public class PriceController : Controller
    {
        IRepository repository;
        
        public PriceController()
        {
            this.repository = new Repository();
        }
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string scanCodes)
        {
            var scanCodeList = scanCodes.Split('\n').Select(sc => sc.Trim());

            var items = repository.Items.Where(i => scanCodeList.Contains(i.ScanCode));

            return View(items.ToViewModels());
        }

        public ActionResult GetPricesForRegion(string regionCode, int itemId)
        {
            var locales = repository.Locales.Where(l => l.RegionCode == regionCode);

        }

        [HttpPost]
        public ActionResult Save(List<PriceModel> prices)
        {
            repository.AddRange(prices);

            return Json(new { Success = true });
        }
    }
}