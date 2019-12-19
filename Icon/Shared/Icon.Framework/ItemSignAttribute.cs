//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Icon.Framework
{
    using System;
    using System.Collections.Generic;
    
    public partial class ItemSignAttribute
    {
        public int ItemSignAttributeID { get; set; }
        public int ItemID { get; set; }
        public Nullable<int> AnimalWelfareRatingId { get; set; }
        public bool Biodynamic { get; set; }
        public Nullable<int> CheeseMilkTypeId { get; set; }
        public bool CheeseRaw { get; set; }
        public Nullable<int> EcoScaleRatingId { get; set; }
        public string GlutenFreeAgencyName { get; set; }
        public Nullable<int> HealthyEatingRatingId { get; set; }
        public string KosherAgencyName { get; set; }
        public bool Msc { get; set; }
        public string NonGmoAgencyName { get; set; }
        public string OrganicAgencyName { get; set; }
        public bool PremiumBodyCare { get; set; }
        public Nullable<int> SeafoodFreshOrFrozenId { get; set; }
        public Nullable<int> SeafoodCatchTypeId { get; set; }
        public string VeganAgencyName { get; set; }
        public bool Vegetarian { get; set; }
        public bool WholeTrade { get; set; }
        public bool GrassFed { get; set; }
        public bool PastureRaised { get; set; }
        public bool FreeRange { get; set; }
        public bool DryAged { get; set; }
        public bool AirChilled { get; set; }
        public bool MadeInHouse { get; set; }
        public string CustomerFriendlyDescription { get; set; }
        public string AnimalWelfareRating { get; set; }
        public string MilkType { get; set; }
        public string DeliverySystems { get; set; }
        public string DrainedWeightUom { get; set; }
        public string EcoScaleRating { get; set; }
        public string FreshOrFrozen { get; set; }
        public string SeafoodCatchType { get; set; }
    
        public virtual Item Item { get; set; }
    }
}
