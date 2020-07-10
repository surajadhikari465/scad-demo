using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Controllers
{
    /// <summary>
    /// Sku Controller.
    /// </summary>
    public class SkuController : Controller
    {
        IQueryHandler<GetItemGroupParameters, IEnumerable<ItemGroupModel>> skuQuery;
        IQueryHandler<GetItemGroupItemCountParameters, IEnumerable<SkuItemCountModel>> skuItemCount;

        /// <summary>
        /// Initializes an instance of SkuController.
        /// </summary>
        /// <param name="skuQuery">Sku Query.</param>
        /// <param name="skuItemCount">Sku Item Count.</param>
        public SkuController(
            IQueryHandler<GetItemGroupParameters, IEnumerable<ItemGroupModel>> skuQuery,
            IQueryHandler<GetItemGroupItemCountParameters, IEnumerable<SkuItemCountModel>> skuItemCount)
        {
            if (skuQuery == null)
            {
                throw new ArgumentNullException(nameof(skuQuery));
            }
            if (skuItemCount == null)
            {
                throw new ArgumentNullException(nameof(skuItemCount));
            }

            this.skuQuery = skuQuery;
            this.skuItemCount = skuItemCount;
        }

        /// <summary>
        /// GET: /Sku
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET: /Sku/AllSku
        /// List of all Skus.
        /// </summary>
        /// <returns>Json with a list of Sku.</returns>
        public ActionResult AllSku()
        {
            var skus = skuQuery.Search(new GetItemGroupParameters { ItemGroupTypeId = ItemGroupTypeId.Sku });

            var skuViewModels = skus.Select((itemGroup) => {
                var itemGroupAttributes = JsonConvert.DeserializeObject<ItemGroupAttributes>(itemGroup.ItemGroupAttributesJson);

                return new SkuViewModel
                {
                    SkuId = itemGroup.ItemGroupId,
                    SkuDescription = itemGroupAttributes.SKUDescription,
                    PrimaryItemUpc = itemGroup.ScanCode,
                    CountOfItems = null
                };
            });

            return Json(new { data = skuViewModels.ToList() }, behavior: JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GET: /Sku/AllSkuCount
        /// List of Sku item count.
        /// </summary>
        /// <returns>Json with a list of Sku item count.</returns>
        public ActionResult AllSkuCount()
        {
            var skusCountList = skuItemCount.Search(new GetItemGroupItemCountParameters { ItemGroupTypeId = ItemGroupTypeId.Sku })
                .Select(sic => new SkuItemCountViewModel { SkuId = sic.ItemGroupId, CountOfItems = sic.CountOfItems} );
            return Json(skusCountList.ToList(), behavior: JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Class for ItemGroupAttributes deserialization.
        /// </summary>
        private class ItemGroupAttributes
        {
            /// <summary>
            /// Gets or sets the Sku Description.
            /// </summary>
            public string SKUDescription { get; set; }
        }
    }
}