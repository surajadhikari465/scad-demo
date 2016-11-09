using System;
using System.ComponentModel.DataAnnotations;

namespace MammothWebApi.Models
{
    public class ItemModel
    {
        public int ItemId { get; set; }
        public int ItemTypeId { get; set; }
        [Required]
        public string ScanCode { get; set; }
        public string ProductDescription { get; set; }
        public string PosDescription { get; set; }
        public string PackageUnit { get; set; }
        public string RetailSize { get; set; }
        public string RetailUom { get; set; }
        public string FoodStamp { get; set; }
        public int? SubBrickId { get; set; }
        public int BrandId { get; set; }
        public int? TaxClassId { get; set; }
        public int? SubTeamId { get; set; }
        public int? NationalClassId { get; set; }
    }
}
