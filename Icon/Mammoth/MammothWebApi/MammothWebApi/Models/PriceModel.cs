using System;
using System.ComponentModel.DataAnnotations;

namespace MammothWebApi.Models
{
    public class PriceModel
    {
        [Required]
        [MaxLength(2)]
        public string Region { get; set; }
        [Required]
        [MaxLength(13)]
        public string ScanCode { get; set; }
        [Required]
        public int BusinessUnitId { get; set; }
        [Required]
        public int Multiple { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        [MaxLength(3)]
        public string PriceType { get; set; }
        [Required]
        [MaxLength(3)]
        public string PriceUom { get; set; }
        [Required]
        public string CurrencyCode { get; set; }
    }
}