using System;
using System.ComponentModel.DataAnnotations;

namespace MammothWebApi.Models
{
    public class PriceRequestModel
    {
        [Required]
        [Range(minimum: 1, maximum: int.MaxValue)]
        public int BusinessUnitId { get; set; }

        [Required]
        [MaxLength(13)]
        public string ScanCode { get; set; }

        [Required]
        [Range(typeof(DateTime), "9/26/2017", "1/18/2038")]
        public DateTime EffectiveDate { get; set; }
    }
}