
namespace Icon.Esb.EwicAplListener.Common.Models
{
    public class EwicItemModel
    {
        public string AgencyId { get; set; }
        public string ScanCode { get; set; }
        public string ItemDescription { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal? PackageSize { get; set; }
        public decimal? BenefitQuantity { get; set; }
        public string BenefitUnitDescription { get; set; }
        public decimal? ItemPrice { get; set; }
        public string PriceType { get; set; }
    }
}
