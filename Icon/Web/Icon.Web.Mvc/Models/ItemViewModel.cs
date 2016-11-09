using Icon.Framework;
using Icon.Web.Common.Utility;
using Icon.Web.DataAccess.Models;
using System;

namespace Icon.Web.Mvc.Models
{
    public class ItemViewModel
    {
        public int ItemId { get; set; }
        public string ScanCode { get; set; }
        public string BrandName { get; set; }
        public int BrandHierarchyClassId { get; set; }
        public string BrandLineage { get; set; }
        public string ProductDescription { get; set; }
        public string PosDescription { get; set; }
        public string PackageUnit { get; set; }
        public bool FoodStampEligible { get; set; }
        public string PosScaleTare { get; set; }
        public string RetailSize { get; set; }
        public string RetailUom { get; set; }
        public string DeliverySystem { get; set; }
        public string MerchandiseHierarchyName { get; set; }
        public int? MerchandiseHierarchyClassId { get; set; }
        public string MerchandiseLineage { get; set; }
        public string NationalHierarchyName { get; set; }
        public int? NationalHierarchyClassId { get; set; }
        public string NationalLineage { get; set; }
        public string TaxHierarchyName { get; set; }
        public int? TaxHierarchyClassId { get; set; }
        public string TaxLineage { get; set; }
        public string BrowsingHierarchyName { get; set; }
        public int? BrowsingHierarchyClassId { get; set; }
        public string BrowsingLineage { get; set; }
        public bool IsValidated { get; set; }
        public bool DepartmentSale { get; set; }
        public bool HiddenItem { get; set; }
        public string Notes { get; set; }
        public int? AnimalWelfareRatingId { get; set; }
        public bool? Biodynamic { get; set; }
        public int? CheeseMilkTypeId { get; set; }
        public bool? CheeseRaw { get; set; }
        public int? EcoScaleRatingId { get; set; }
        public int? GlutenFreeAgencyId { get; set; }
        public int? KosherAgencyId { get; set; }
        public bool? Msc { get; set; }
        public int? NonGmoAgencyId { get; set; }
        public int? OrganicAgencyId { get; set; }
        public bool? PremiumBodyCare { get; set; }
        public int? SeafoodFreshOrFrozenId { get; set; }
        public int? SeafoodCatchTypeId { get; set; }
        public int? VeganAgencyId { get; set; }
        public bool? Vegetarian { get; set; }
        public bool? WholeTrade { get; set; }
        public bool? GrassFed { get; set; }
        public bool? PastureRaised { get; set; }
        public bool? FreeRange { get; set; }
        public bool? DryAged { get; set; }
        public bool? AirChilled { get; set; }
        public bool? MadeInHouse { get; set; }
        public String CreatedDate { get; set; }
        public String LastModifiedDate { get; set; }
        public string LastModifiedUser { get; set; }
        public decimal? AlcoholByVolume { get; set; }
        public bool? CaseinFree { get; set; }
        public decimal? DrainedWeight { get; set; }
        public string DrainedWeightUom { get; set; }
        public string FairTradeCertified { get; set; }
        public bool? Hemp { get; set; }
        public bool? LocalLoanProducer { get; set; }
        public string MainProductName { get; set; }
        public bool? NutritionRequired { get; set; }
        public bool? OrganicPersonalCare { get; set; }
        public bool? Paleo { get; set; }
        public string ProductFlavorType { get; set; }

        public ItemViewModel() { }

        public ItemViewModel(ItemSearchModel item)
        {
            ItemId = item.ItemId;
            ScanCode = item.ScanCode;

            BrandName = item.BrandName;
            BrandHierarchyClassId = item.BrandHierarchyClassId == null ? -1 : item.BrandHierarchyClassId.Value;

            ProductDescription = item.ProductDescription;
            PosDescription = item.PosDescription;
            PackageUnit = item.PackageUnit;
            FoodStampEligible = item.GetFoodStampEligible();
            PosScaleTare = item.PosScaleTare;
            RetailSize = item.RetailSize;
            RetailUom = item.RetailUom;
            DeliverySystem = item.DeliverySystem;
            Notes = item.Notes;

            MerchandiseHierarchyName = item.MerchandiseHierarchyName;
            MerchandiseHierarchyClassId = item.MerchandiseHierarchyClassId;

            TaxHierarchyName = item.TaxHierarchyName;
            TaxHierarchyClassId = item.TaxHierarchyClassId;

            BrowsingHierarchyName = item.BrowsingHierarchyName;
            BrowsingHierarchyClassId = item.BrowsingHierarchyClassId;
            DepartmentSale = item.GetDepartmentSale();
            HiddenItem = item.GetHiddenItemStatus();

            IsValidated = item.GetValidationStatus();

            NationalHierarchyName = item.NationalHierarchyName;
            NationalHierarchyClassId = item.NationalHierarchyClassId;

            AnimalWelfareRatingId = item.AnimalWelfareRatingId;
            Biodynamic = item.Biodynamic;
            CheeseMilkTypeId = item.CheeseMilkTypeId;
            CheeseRaw = item.CheeseRaw;
            EcoScaleRatingId = item.EcoScaleRatingId;
            GlutenFreeAgencyId = item.GlutenFreeAgencyId;
            KosherAgencyId = item.KosherAgencyId;
            Msc = item.Msc;
            NonGmoAgencyId = item.NonGmoAgencyId;
            OrganicAgencyId = item.OrganicAgencyId;
            PremiumBodyCare = item.PremiumBodyCare;
            SeafoodFreshOrFrozenId = item.SeafoodFreshOrFrozenId;
            SeafoodCatchTypeId = item.SeafoodCatchTypeId;
            VeganAgencyId = item.VeganAgencyId;
            Vegetarian = item.Vegetarian;
            WholeTrade = item.WholeTrade;
            GrassFed = item.GrassFed;
            PastureRaised = item.PastureRaised;
            FreeRange = item.FreeRange;
            DryAged = item.DryAged;
            AirChilled = item.AirChilled;
            MadeInHouse = item.MadeInHouse;
            DrainedWeightUom = item.DrainedWeightUom;
            AlcoholByVolume = ConversionUtility.ToNullableDecimal(item.AlcoholByVolume);
            CaseinFree = ConversionUtility.ToNullableBool(item.CaseinFree);
            DrainedWeight = ConversionUtility.ToNullableDecimal(item.DrainedWeight);
            FairTradeCertified = item.FairTradeCertified;
            Hemp = ConversionUtility.ToNullableBool(item.Hemp);
            LocalLoanProducer = ConversionUtility.ToNullableBool(item.LocalLoanProducer);
            NutritionRequired = ConversionUtility.ToNullableBool(item.NutritionRequired);
            OrganicPersonalCare = ConversionUtility.ToNullableBool(item.OrganicPersonalCare);
            MainProductName = item.MainProductName;
            Paleo = ConversionUtility.ToNullableBool(item.Paleo);
            ProductFlavorType = item.ProductFlavorType;

            if (item.CreatedDate == null)
            {
                CreatedDate = String.Empty;
            }
            else
            {
                DateTime tempDateTime;
                if (DateTime.TryParse(item.CreatedDate, out tempDateTime))
                {
                    CreatedDate = tempDateTime.ToString("MM/dd/yy HH:mm");
                }
            }
            if (item.LastModifiedDate == null)
            {
                LastModifiedDate = String.Empty;
            }
            else
            {
                DateTime tempDateTime;
                if (DateTime.TryParse(item.LastModifiedDate, out tempDateTime))
                {
                    LastModifiedDate = tempDateTime.ToString("MM/dd/yy HH:mm");
                }
            }
            LastModifiedUser = item.LastModifiedUser;
        }
    }
}