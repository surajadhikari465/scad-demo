using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Icon.Web.Mvc.Models
{
    /// <summary>
    /// Sku View Model.
    /// </summary>
    public class SkuViewModel
    {
        /// <summary>
        /// Gets or sets the Sku Id.
        /// </summary>
        [ReadOnly(true)]
        [Display(Name = "Sku Id")]
        public int SkuId { get; set; }

        /// <summary>
        /// Gets or sets the Sku Description.
        /// </summary>
        [Required]
        [Display(Name = "Description")]
        [MaxLength(255, ErrorMessage = "Name must be 255 characters or less")]
        public string SkuDescription { get; set; }

        /// <summary>
        /// Gets or sets the Primary Item Upc.
        /// </summary>
        [Required]
        [Display(Name = "PrimaryItemUpc")]
        [MaxLength(13, ErrorMessage = "Primary Item Upc 13 characters or less")]
        public string PrimaryItemUpc { get; set; }

        /// <summary>
        /// Get or set the count of items.
        /// </summary>
        [ReadOnly(true)]
        [Display(Name = "Count Of Items")]
        public int? CountOfItems { get; set; }
    }
}