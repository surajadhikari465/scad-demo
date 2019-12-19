using Icon.Common.DataAccess;
using Icon.Common.Models;
using Icon.Logging;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Attributes;
using Icon.Web.Mvc.Extensions;
using Icon.Web.Mvc.Models.RefreshData;
using Icon.Web.Mvc.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Controllers
{
    [AdminAccessAuthorizeAttribute]
    public class RefreshDataController : Controller
    {
        private ICommandHandler<PublishItemUpdatesCommand> publishItemUpdatesCommandHandler;
        private ICommandHandler<RefreshLocalesCommand> addLocaleMessagesCommandHandler;
        private ICommandHandler<RefreshHierarchiesCommand> addHierarchiesCommandHandler;
        private ICommandHandler<RefreshAttributesCommand> publishAttributeQueueCommandHandler;
        private IQueryHandler<GetAttributeByAttributeIdParameters, AttributeModel> getAttributeByAttributeIdQuery;
        private ILogger logger;
        private IconWebAppSettings settings;
        public RefreshDataController(ICommandHandler<PublishItemUpdatesCommand> publishItemUpdatesCommandHandler,
            ICommandHandler<RefreshLocalesCommand> addLocaleMessagesCommandHandler,
            ICommandHandler<RefreshHierarchiesCommand> addHierarchiesCommandHandler,
            ICommandHandler<RefreshAttributesCommand> publishAttributeQueueCommandHandler,
            IQueryHandler<GetAttributeByAttributeIdParameters, AttributeModel> getAttributeByAttributeIdQuery,
            ILogger logger,
            IconWebAppSettings settings)
        {
            this.publishItemUpdatesCommandHandler = publishItemUpdatesCommandHandler;
            this.addLocaleMessagesCommandHandler = addLocaleMessagesCommandHandler;
            this.addHierarchiesCommandHandler = addHierarchiesCommandHandler;
            this.publishAttributeQueueCommandHandler = publishAttributeQueueCommandHandler;
            this.getAttributeByAttributeIdQuery = getAttributeByAttributeIdQuery;
            this.logger = logger;
            this.settings = settings;

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

                publishItemUpdatesCommandHandler.Execute(new PublishItemUpdatesCommand { ScanCodes = scanCodesToRefresh });
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

        public ActionResult Attributes()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Attributes(RefreshAttributesViewModel viewModel)
        {
            try
            {
                if (viewModel == null || String.IsNullOrWhiteSpace(viewModel.AttributeIds))
                {
                    return View(viewModel);
                }

                string[] parsedIds = viewModel.AttributeIds.ParseByLine();

                string[] uniqueIds = parsedIds.Distinct().ToArray();
                string[] uniqueIdsWithNoWhitespaceLines = uniqueIds.Where(sc => !String.IsNullOrWhiteSpace(sc)).ToArray();
                List<string> attributeIdsToRefresh = uniqueIdsWithNoWhitespaceLines.Take(3000).ToList();

                List<string> badIds = new List<string>();
                foreach (var id in attributeIdsToRefresh)
                {
                    int idInt;
                    if(!int.TryParse(id, out idInt))
                    {
                        badIds.Add(id);
                    }
                    else
                    {
                        var attribute = this.getAttributeByAttributeIdQuery.Search(new GetAttributeByAttributeIdParameters() { AttributeId = idInt });
                        if (attribute == null)
                        {
                            badIds.Add(id);
                        }
                    }
                }

                if(badIds.Count > 0)
                {
                    string concat = string.Join(", ", badIds.ToArray());
                    ViewBag.Message = $"The following IDs are invalid: {concat}";
                    return View(viewModel);
                }

                publishAttributeQueueCommandHandler.Execute(new RefreshAttributesCommand { AttributeIds = attributeIdsToRefresh });
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                ViewBag.Message = "Unexpected exception occurred: " + ex.Message;
                return View(viewModel);
            }
            ViewBag.Message = "Refreshed attributes successfully.";
            return View(new RefreshAttributesViewModel());
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

        public ActionResult Hierarchy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Hierarchy(RefreshDataViewModel viewModel)
        {
            try
            {
                if (viewModel == null || string.IsNullOrWhiteSpace(viewModel.Identifiers))
                {
                    return View(viewModel);
                }

                List<int> hierarchyClassIds = viewModel.Identifiers.ParseByLine()
                    .Take(3000)
                    .Select(id => int.Parse(id))
                    .ToList();

                addHierarchiesCommandHandler.Execute(new RefreshHierarchiesCommand
                {
                    HierarchyClassIds = hierarchyClassIds,
                    IsManufacturerHierarchyMessage =settings.IsManufacturerHierarchyMessage
                });
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                ViewBag.Message = "Unexpected exception occurred: " + ex.Message;
                return View(viewModel);
            }

            ViewBag.Message = "Refreshed Hierarchy successfully.";
            return View(new RefreshDataViewModel());
        }
    }
}