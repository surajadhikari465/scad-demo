using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitBuilderWebApi.DatabaseModels
{
    public partial class Items
    {
        public Items()
        {
            Kit = new HashSet<Kit>();
            LinkGroupItem = new HashSet<LinkGroupItem>();
        }

        public int ItemId { get; set; }
        [Required(ErrorMessage = "ScanCode is required.")]
        public string ScanCode { get; set; }
        [StringLength(255, ErrorMessage = "Product Desc can have maximum length of 255.")]
        public string ProductDesc { get; set; }
        [StringLength(255, ErrorMessage = "Customer Friendly Description can have maximum length of 255.")]
        public string CustomerFriendlyDesc { get; set; }
        [StringLength(255, ErrorMessage = "Kitchen Description can have maximum length of 255.")]
        public string KitchenDesc { get; set; }
        [StringLength(255, ErrorMessage = "Brand Name can have maximum length of 255.")]
        public string BrandName { get; set; }
        public string LargeImageUrl { get; set; }
        public string SmallImageUrl { get; set; }
        public DateTime InsertDate { get; set; }

        public ICollection<Kit> Kit { get; set; }
        public ICollection<LinkGroupItem> LinkGroupItem { get; set; }
    }
}
