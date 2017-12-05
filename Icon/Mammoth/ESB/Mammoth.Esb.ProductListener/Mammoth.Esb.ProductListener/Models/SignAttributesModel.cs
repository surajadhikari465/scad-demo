namespace Mammoth.Esb.ProductListener.Models
{
    public class SignAttributesModel
    {
        public int ItemID { get; set; }
        public string CheeseMilkType { get; set; }
        public string Agency_GlutenFree { get; set; }
        public string Agency_Kosher { get; set; }
        public string Agency_NonGMO { get; set; }
        public string Agency_Organic { get; set; }
        public string Agency_Vegan { get; set; }
        public bool? IsAirChilled { get; set; }
        public bool? IsBiodynamic { get; set; }
        public bool? IsCheeseRaw { get; set; }
        public bool? IsDryAged { get; set; }
        public bool? IsFreeRange { get; set; }
        public bool? IsGrassFed { get; set; }
        public bool? IsMadeInHouse { get; set; }
        public bool? IsMsc { get; set; }
        public bool? IsPastureRaised { get; set; }
        public bool? IsPremiumBodyCare { get; set; }
        public bool? IsVegetarian { get; set; }
        public bool? IsWholeTrade { get; set; }
        public string Rating_AnimalWelfare { get; set; }
        public string Rating_EcoScale { get; set; }
        public string Rating_HealthyEating { get; set; }
        public string Seafood_FreshOrFrozen { get; set; }
        public string Seafood_CatchType { get; set; }
    }
}
