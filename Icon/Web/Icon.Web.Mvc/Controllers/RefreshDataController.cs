using Icon.Common.DataAccess;
using Icon.Logging;
using Icon.Web.DataAccess.Commands;
using Icon.Web.Extensions;
using Icon.Web.Mvc.Models.RefreshData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Controllers
{
    public class RefreshDataController : Controller
    {
        private ICommandHandler<RefreshItemsCommand> addProductMessagesCommandHandler;
        private ICommandHandler<RefreshLocalesCommand> addLocaleMessagesCommandHandler;
        private ICommandHandler<RefreshBrandsCommand> addBrandMessagesCommandHandler;
        private ICommandHandler<RefreshMerchandiseHierarchyClassesCommand> addMerchandiseHierarchyClassMessagesCommandHandler;
        private ILogger logger;

        public RefreshDataController(ICommandHandler<RefreshItemsCommand> addProductMessagesCommandHandler,
            ICommandHandler<RefreshLocalesCommand> addLocaleMessagesCommandHandler,
            ICommandHandler<RefreshBrandsCommand> addBrandMessagesCommandHandler,
            ICommandHandler<RefreshMerchandiseHierarchyClassesCommand> addMerchandiseHierarchyClassMessagesCommandHandler,
            ILogger logger)
        {
            this.addProductMessagesCommandHandler = addProductMessagesCommandHandler;
            this.addLocaleMessagesCommandHandler = addLocaleMessagesCommandHandler;
            this.addBrandMessagesCommandHandler = addBrandMessagesCommandHandler;
            this.addMerchandiseHierarchyClassMessagesCommandHandler = addMerchandiseHierarchyClassMessagesCommandHandler;
            this.logger = logger;
        }

        // GET: RefreshData
        public ActionResult Items()
        {
            return View(new RefreshDataViewModel());
        }

        [HttpPost]
        public ActionResult Items(RefreshDataViewModel viewModel)
        {
            try
            {
                if (viewModel == null || String.IsNullOrWhiteSpace(viewModel.Identifiers))
                {
                    return View(viewModel);
                }

                string[] parsedScanCodes = viewModel.Identifiers.ParseByLine();
                string[] tooLongScanCodes = parsedScanCodes.Where(sc => sc.Length > 13).ToArray();
                string[] validScanCodes = parsedScanCodes.Except(tooLongScanCodes).ToArray();

                string[] uniqueScanCodes = validScanCodes.Distinct().ToArray();
                string[] uniqueScanCodesWithNoWhitespaceLines = uniqueScanCodes.Where(sc => !String.IsNullOrWhiteSpace(sc)).ToArray();
                List<string> scanCodesToRefresh = uniqueScanCodesWithNoWhitespaceLines.Take(3000).ToList();

                addProductMessagesCommandHandler.Execute(new RefreshItemsCommand { ScanCodes = scanCodesToRefresh });
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                ViewBag.Message = "Unexpected exception occurred: " + ex.Message;
                return View(viewModel);
            }
            ViewBag.Message = "Refreshed items successfully.";
            return View(new RefreshDataViewModel());
        }

        public ActionResult Locales()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Locales(RefreshDataViewModel viewModel)
        {
            try
            {
                if (viewModel == null || string.IsNullOrWhiteSpace(viewModel.Identifiers))
                {
                    return View(viewModel);
                }

                List<int> localeIdsToRefresh = viewModel.Identifiers.ParseByLine()
                    .Take(3000)
                    .Select(id => int.Parse(id))
                    .ToList();

                addLocaleMessagesCommandHandler.Execute(new RefreshLocalesCommand { LocaleIds = localeIdsToRefresh });
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                ViewBag.Message = "Unexpected exception occurred: " + ex.Message;
                return View(viewModel);
            }
            ViewBag.Message = "Refreshed Locales successfully.";
            return View(new RefreshDataViewModel());
        }

        public ActionResult Brands()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Brands(RefreshDataViewModel viewModel)
        {
            try
            {
                if (viewModel == null || string.IsNullOrWhiteSpace(viewModel.Identifiers))
                {
                    return View(viewModel);
                }

                List<int> brandIds = viewModel.Identifiers.ParseByLine()
                    .Take(3000)
                    .Select(id => int.Parse(id))
                    .ToList();

                addBrandMessagesCommandHandler.Execute(new RefreshBrandsCommand { Brands = brandIds });
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                ViewBag.Message = "Unexpected exception occurred: " + ex.Message;
                return View(viewModel);
            }
            ViewBag.Message = "Refreshed Brands successfully.";
            return View(new RefreshDataViewModel());
        }

        public ActionResult Merchandise()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Merchandise(RefreshDataViewModel viewModel)
        {
            try
            {
                if (viewModel == null || String.IsNullOrWhiteSpace(viewModel.Identifiers))
                {
                    return View(viewModel);
                }

                List<int> merchandiseHierarchyClassIds = viewModel.Identifiers.ParseByLine()
                    .Take(3000)
                    .Select(id => int.Parse(id))
                    .ToList();

                addMerchandiseHierarchyClassMessagesCommandHandler.Execute(new RefreshMerchandiseHierarchyClassesCommand
                {
                    MerchandiseHierarchyClassIds = merchandiseHierarchyClassIds
                });
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                ViewBag.Message = "Unexpected exception occurred: " + ex.Message;
                return View(viewModel);
            }
            ViewBag.Message = "Refreshed Merchandise Hierarchy Classes successfully.";
            return View(new RefreshDataViewModel());
        }
    }
}