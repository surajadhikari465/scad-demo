using System.ComponentModel.DataAnnotations;
using WFM.OutOfStock.API.Validation;

namespace WFM.OutOfStock.API.Domain.Request
{
    public sealed class UploadItemsRequest
    {
        [Required, RegionCode]
        public string RegionCode { get; set; }

        [Required]
        public string StoreName { get; set; }

        [Required, ListItems]
        public string[] Items { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string UserEmail { get; set; }
    }
}