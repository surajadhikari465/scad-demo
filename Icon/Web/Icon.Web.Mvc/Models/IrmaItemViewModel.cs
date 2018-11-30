using Icon.Framework;

namespace Icon.Web.Mvc.Models
{
    public class IrmaItemViewModel
    {
        public int IrmaItemId { get; set; }
        public string Region { get; set; }
        public string Identifier { get; set; }
        public bool DefaultIdentifier { get; set; }
        public string ItemDescription { get; set; }
        public string PosDescription { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int PackageUnit { get; set; }
        public decimal? RetailSize { get; set; }
        public string RetailUom { get; set; }
        public string DeliverySystem { get; set; }
        public bool FoodStamp { get; set; }
        public decimal PosScaleTare { get; set; }
        public string MerchandiseHierarchyClassName { get; set; }
        public int? MerchandiseHierarchyClassId { get; set; }
        public string TaxHierarchyClassName { get; set; }
        public int? TaxHierarchyClassId { get; set; }
        public bool IsNewBrand { get; set; }
        public bool HasInvalidData { get; set; }
        public bool IsUnsupportedUom { get; set; }
        public string IrmaSubTeamName { get; set; }
        public string NationalHierarchyClassName { get; set; }
        public int? NationalHierarchyClassId { get; set; }
        public string AnimalWelfareRating { get; set; }

        public bool? Biodynamic { get; set; }

        public string CheeseMilkType { get; set; }

        public bool? CheeseRaw { get; set; }

        public string EcoScaleRating { get; set; }

        public string GlutenFreeAgency { get; set; }

        public string KosherAgency { get; set; }

        public bool? Msc { get; set; }

        public string NonGmoAgency { get; set; }

        public string OrganicAgency { get; set; }

        public bool? PremiumBodyCare { get; set; }

        public string SeafoodFreshOrFrozen { get; set; }

        public string SeafoodCatchType { get; set; }

        public string VeganAgency { get; set; }

        public bool? Vegetarian { get; set; }

        public bool? WholeTrade { get; set; }

        public bool? GrassFed { get; set; }
        public bool? PastureRaised { get; set; }
        public bool? FreeRange { get; set; }
        public bool? DryAged { get; set; }
        public bool? AirChilled { get; set; }
        public bool? MadeInHouse { get; set; }
        public decimal? AlcoholByVolume { get; set; }

        public IrmaItemViewModel() {}

        public IrmaItemViewModel(IRMAItem irmaItem)
        {
            this.IrmaItemId = irmaItem.irmaItemID;
            this.BrandName = irmaItem.brandName;
            this.Region = irmaItem.regioncode;
            this.Identifier = irmaItem.identifier;
            this.DefaultIdentifier = irmaItem.defaultIdentifier;
            this.ItemDescription = irmaItem.itemDescription;
            this.PosDescription = irmaItem.posDescription;
            this.PackageUnit = irmaItem.packageUnit;
            this.RetailUom = irmaItem.retailUom;
            this.DeliverySystem = irmaItem.DeliverySystem;
            this.RetailSize = irmaItem.retailSize;
            this.FoodStamp = irmaItem.foodStamp;
            this.PosScaleTare = irmaItem.posScaleTare;
            this.MerchandiseHierarchyClassId = irmaItem.merchandiseClassID;            
            this.TaxHierarchyClassId = irmaItem.taxClassID;
            this.IrmaSubTeamName = irmaItem.irmaSubTeamName;
            this.NationalHierarchyClassId = irmaItem.nationalClassID;
            AnimalWelfareRating = irmaItem.AnimalWelfareRating;
            Biodynamic = irmaItem.Biodynamic;
            CheeseMilkType = irmaItem.MilkType;
            CheeseRaw = irmaItem.CheeseRaw;
            EcoScaleRating = irmaItem.EcoScaleRating;
            Msc = irmaItem.Msc;
            PremiumBodyCare = irmaItem.PremiumBodyCare;
            SeafoodFreshOrFrozen = irmaItem.FreshOrFrozen;
            SeafoodCatchType = irmaItem.SeafoodCatchType;
            Vegetarian = irmaItem.Vegetarian;
            WholeTrade = irmaItem.WholeTrade;
            GrassFed = irmaItem.GrassFed;
            PastureRaised = irmaItem.PastureRaised;
            FreeRange = irmaItem.FreeRange;
            DryAged = irmaItem.DryAged;
            AirChilled = irmaItem.AirChilled;
            MadeInHouse = irmaItem.MadeInHouse;
            AlcoholByVolume = irmaItem.AlcoholByVolume;
        }
    }
}