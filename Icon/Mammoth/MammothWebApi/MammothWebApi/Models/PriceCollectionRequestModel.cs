using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MammothWebApi.Models
{
    public class PriceCollectionRequestModel
    {
        [Required]
        public IEnumerable<StoreItem> StoreItems { get; set; }
        public bool IncludeFuturePrices { get; set; }
    }
}