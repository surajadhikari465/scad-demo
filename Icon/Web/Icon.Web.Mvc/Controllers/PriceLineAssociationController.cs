using Icon.Common.DataAccess;
using Icon.FeatureFlags;
using Icon.Logging;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Controllers
{
    /// <summary>
    /// Price Line Association Controller.
    /// </summary>
    public class PriceLineAssociationController : Controller
    {
        private IQueryHandler<GetItemGroupByIdParameters, ItemGroupModel> getItemGroupByIdQuery;
        private IQueryHandler<GetItemGroupMembersParameters, IEnumerable<ItemGroupMember>> getItemGroupMembersQuery;
        private IQueryHandler<GetItemGroupAssociationSearchItemPartialParameters, IEnumerable<ItemGroupAssociationItemModel>> getItemGroupAssociationSearchItemPartialQuery;
        private ICommandHandler<SetPrimaryItemGroupItemCommand> setPrimaryItemGroupItemCommand;
        private ICommandHandler<AddItemToItemGroupCommand> addItemToItemGroupCommand;
        private IFeatureFlagService featureFlagService;
        private ILogger logger;

        /// <summary>
        /// Initializes an instance of PriceLineAssociationController.
        /// </summary>
        /// <param name="getItemGroupByIdQuery"></param>
        /// <param name="getItemGroupMembersQuery"></param>
        /// <param name="getItemGroupAssociationSearchItemPartialQuery">GetItemGroupAssociation Search Item Partial Query</param>
        /// <param name="setPrimaryItemGroupItemCommand"></param>
        public PriceLineAssociationController(
                IQueryHandler<GetItemGroupByIdParameters, ItemGroupModel> getItemGroupByIdQuery,
                IQueryHandler<GetItemGroupMembersParameters, IEnumerable<ItemGroupMember>> getItemGroupMembersQuery,
                IQueryHandler<GetItemGroupAssociationSearchItemPartialParameters, IEnumerable<ItemGroupAssociationItemModel>> getItemGroupAssociationSearchItemPartialQuery,
                ICommandHandler<SetPrimaryItemGroupItemCommand> setPrimaryItemGroupItemCommand,
                ICommandHandler<AddItemToItemGroupCommand> addItemToItemGroupCommand,
                IFeatureFlagService featureFlagService,
                ILogger logger)
        {
            if (getItemGroupByIdQuery == null)
            {
                throw new ArgumentNullException(nameof(getItemGroupByIdQuery));
            }
            if (getItemGroupMembersQuery == null)
            {
                throw new ArgumentNullException(nameof(getItemGroupMembersQuery));
            }
            if(setPrimaryItemGroupItemCommand == null)
            {
                throw new ArgumentNullException(nameof(setPrimaryItemGroupItemCommand));
            }
            if (getItemGroupAssociationSearchItemPartialQuery == null)
            {
                throw new ArgumentNullException(nameof(GetItemGroupAssociationSearchItemPartialQuery));
            }
            if (addItemToItemGroupCommand == null)
            {
                throw new ArgumentNullException(nameof(addItemToItemGroupCommand));
            }
            if (featureFlagService == null)
            {
                throw new ArgumentNullException(nameof(featureFlagService));
            }
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this.getItemGroupByIdQuery = getItemGroupByIdQuery;
            this.getItemGroupMembersQuery = getItemGroupMembersQuery;
            this.getItemGroupAssociationSearchItemPartialQuery = getItemGroupAssociationSearchItemPartialQuery;
            this.setPrimaryItemGroupItemCommand = setPrimaryItemGroupItemCommand;
            this.addItemToItemGroupCommand = addItemToItemGroupCommand;
            this.featureFlagService = featureFlagService;
            this.logger = logger;
        }

        /// <summary>
        /// // GET: PriceLineAssociation.
        /// </summary>
        /// <param name="priceLineId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(int priceLineId)
        {
            // Get the feature flag.
            bool priceLineManagementFeatureFlag;
            try
            {
                priceLineManagementFeatureFlag = this.featureFlagService.IsEnabled("PriceLineManagement");
            }
            catch
            {
                priceLineManagementFeatureFlag = false;
            }

            // If the feature is not enable, redirect.
            if (priceLineManagementFeatureFlag == false)
            {
                return Redirect("/");
            }
  
            var itemGroup = getItemGroupByIdQuery.Search(new GetItemGroupByIdParameters { ItemGroupId = priceLineId });
            if (itemGroup == null)
            {
                string msg = $"PriceLine id:" + priceLineId + " not found.";
                logger.Error(msg);
                ViewData["ErrorMessage"] = msg;
                return View("Error");
            }

            return View(itemGroup);
        }

        /// <summary>
        /// /PriceLineAssociation/GetAllRelatedItems.
        /// Used by the datatable to retrieve all item members of a priceline.
        /// </summary>
        /// <param name="priceLineId">Price Line Id.</param>
        /// <returns>Datatable rows.</returns>
        public JsonResult GetAllRelatedItems(int priceLineId)
        {
            var result = getItemGroupMembersQuery.Search(new GetItemGroupMembersParameters { ItemGroupId = priceLineId });
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Search Item By Prefix
        /// </summary>
        /// <param name="priceLineId">Price Line Id to exclude from the search</param>
        /// <param name="term">Item scan code prefix</param>
        /// <returns>Json witht 500 entries</returns>
        [HttpGet]
        public ActionResult SearchItemByPrefix(int priceLineId, string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                throw new ArgumentNullException(nameof(term));
            }

            var isNumberRegEx = new Regex("^[0-9]+$");
            if (isNumberRegEx.IsMatch(term) == false)
            {
                return new HttpStatusCodeResult(500, "Term should only be numbers");
            }

            var items = this.getItemGroupAssociationSearchItemPartialQuery.Search(
                new GetItemGroupAssociationSearchItemPartialParameters
                {
                    ItemGroupTypeId = ItemGroupTypeId.Priceline,
                    MaxResultSize = 500,
                    ScanCodePrefix = $"{term}%",
                    ExcludeItemGroupId = priceLineId
                });

            var autocompleteData = items.Select(i => new
            {
                label = $"{i.ScanCode} - {i.CustomerFriendlyDescription}",
                value = new ItemGroupAssociationAutoCompleteData
                {
                    itemId = i.ItemId,
                    scanCode = i.ScanCode,
                    description = i.CustomerFriendlyDescription,
                    previousPriceLineId = i.ItemGroupId,
                    previousPriceLine = i.PriceLineDescription,
                    isPrimary = i.IsPrimary,
                    isLastInpreviousPriceLine = ((i?.ItemGroupItemCount ?? 0) == 1),
                }
            });
            return Json(autocompleteData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Used by the datatable to set a new primaryId
        /// </summary>
        /// <param name="priceLineId"></param>
        /// <param name="itemId"></param>
        /// <returns>Json with the status.</returns>
        [HttpPost]
        [WriteAccessAuthorize]
        public ActionResult ChangePrimaryItem(int priceLineId, int itemId)
        {
            try
            {
                this.setPrimaryItemGroupItemCommand.Execute(
                    new SetPrimaryItemGroupItemCommand
                    {
                        ItemGroupId = priceLineId,
                        PrimaryItemId = itemId,
                    });

                return Json(new { success = true });
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(500, "Error changing primary item.");
            }
        }

        /// <summary>
        /// Used by the datatable to set a new primaryId
        /// </summary>
        /// <param name="priceLineId"></param>
        /// <param name="itemId"></param>
        /// <returns>Json with the status.</returns>
        [HttpPost]
        [WriteAccessAuthorize]
        public ActionResult AddItemToPriceLine(int priceLineId, int itemId)
        {
            System.Threading.Thread.Sleep(10000);
            try
            {
                this.addItemToItemGroupCommand.Execute(
                    new AddItemToItemGroupCommand
                    {
                        ItemGroupId = priceLineId,
                        ItemId = itemId,
                        ItemGroupTypeId = ItemGroupTypeId.Priceline
                    });

                return Json(new { success = true });
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(500, "Error adding item.");
            }
        }
    }
}