using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.Models
{
    /// <summary>
    /// Price Line View Model.
    /// </summary>
    public class PriceLineViewModel
    {
        /// <summary>
        /// Gets or sets the Price Line Id.
        /// </summary>
        [ReadOnly(true)]
        [Display(Name = "Price Line Id")]
        public int PriceLineId { get; set; }

        /// <summary>
        /// Gets or sets the Price Line Description.
        /// </summary>
        [Required]
        [Display(Name = "Price Line Description")]
        [MaxLength(255, ErrorMessage = "Description must be 255 characters or less")]
        public string PriceLineDescription { get; set; }

        /// <summary>
        /// Gets or sets the Price Line Size.
        /// </summary>
        [Display(Name = "Price Line Size")]
        public string PriceLineSize { get; set; }

        /// <summary>
        /// Gets or sets the Price Line UOM.
        /// </summary>
        [Display(Name = "Price Line UOM")]
        [MaxLength(25, ErrorMessage = "UOM must be 25 characters or less")]
        public string PriceLineUOM { get; set; }

        /// <summary>
        /// Gets or sets the Primary Item Upc.
        /// </summary>
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