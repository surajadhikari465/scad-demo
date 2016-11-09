using Infragistics.Documents.Excel;
using MessageGenerationWeb.Excel;
using MessageGenerationWeb.Services;
using MessageGenerationWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MessageGenerationWeb.Controllers
{
    public class ItemPriceController : Controller
    {
        private ItemPriceDeleteSpreadsheetManager spreadsheetManager;
        private ItemPriceService itemPriceService;

        public ItemPriceController()
        {
            spreadsheetManager = new ItemPriceDeleteSpreadsheetManager();
            itemPriceService = new ItemPriceService();
        }

        // GET: ItemPrice
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ImportDelete()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportDelete(SpreadsheetViewModel spreadsheet)
        {
            spreadsheetManager.Workbook = Workbook.Load(spreadsheet.ExcelAttachment.InputStream);
            var models = spreadsheetManager.ConvertToModel();

            itemPriceService.DeleteItemPrices(models);

            ViewBag.Message = "Delete Successful";
            return View();
        }
    }
}