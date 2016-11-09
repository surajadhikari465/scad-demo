using IconRegional.Core.Models;
using IconRegional.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IconRegional.Web.Controllers
{
    public class ItemController : Controller
    {
        private static List<ItemModel> items;
        static ItemController()
        {
            items = new List<ItemModel>
            {
                new ItemModel { ScanCode = "3331", ProductDescription = "Item 1" },
                new ItemModel { ScanCode = "3332", ProductDescription = "Item 2" },
                new ItemModel { ScanCode = "3333", ProductDescription = "Item 3" },
            };
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(ItemSearchViewModel viewModel)
        {
            var parsedScanCodes = viewModel.ScanCodes
                .Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(sc => sc.Trim())
                .ToList();
            var totalNumberOfItems = items.Count(i => parsedScanCodes.Contains(i.ScanCode));
            var itemsFromSearch = items
                .Where(i => parsedScanCodes.Contains(i.ScanCode))
                .Skip(viewModel.Skip)
                .Take(viewModel.Take)
                .ToList();

            return PartialView(
                "_ItemSearchResults", 
                new ItemSearchResultsViewModel
                {
                    Items = itemsFromSearch,
                    TotalNumberOfItems = totalNumberOfItems
                });
        }

        public ActionResult Export(ItemSearchViewModel viewModel)
        {
            return null;
        }
    }
}