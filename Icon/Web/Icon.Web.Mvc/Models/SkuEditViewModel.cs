using Icon.Web.DataAccess.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Icon.Web.Mvc.Models
{
    public class SkuEditViewModel : SkuPricelineBaseModel
    {
        /// <summary>
        /// Gets or sets the Sku Id.
        /// </summary>
        [Display(Name = "SKU ID")]
        public int SkuId { get; set; }

        [Display(Name = "SKU Primary Scan Code")]
        public string PrimaryItemUpc { get; set; }

        /// <summary>
        /// Get or set the count of items.
        /// </summary>
        [Display(Name = "Item Count")]
        public int? CountOfItems { get; set; }

        /// <summary>
        /// Gets or sets the Sku Description.
        /// </summary>
        [Display(Name = "SKU Description")]
        public string SkuDescription { get; set; }

        public SkuEditViewModel()
        {
        }

        public SkuEditViewModel(SkuModel sku)
        {
            SkuId = sku.SkuId;
            SkuDescription = sku.SkuDescription;
            PrimaryItemUpc = sku.PrimaryItemUpc;
            LastModifiedBy = sku.LastModifiedBy;
            LastModifiedDate = sku.LastModifiedDate;
            CountOfItems = sku.CountOfItems;
            CreatedBy = sku.CreatedBy;
            CreatedDate = sku.CreatedDate;
        }
    }
}