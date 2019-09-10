﻿using System;

namespace AmazonLoad.IconItem
{
    public class ItemModel
    {
        public DateTime InsertDate { get; set; }
        public int ItemId { get; set; }
        public int LocaleId { get; set; }
        public string ItemTypeCode { get; set; }
        public string ItemTypeDesc { get; set; }
        public int ScanCodeId { get; set; }
        public string ScanCode { get; set; }
        public int ScanCodeTypeId { get; set; }
        public string ScanCodeTypeDesc { get; set; }
        public string ProductDescription { get; set; }
        public string PosDescription { get; set; }
        public string PackageUnit { get; set; }
        public string RetailSize { get; set; }
        public string RetailUom { get; set; }
        public string FoodStampEligible { get; set; }
        public bool ProhibitDiscount { get; set; }
        public string DepartmentSale { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int BrandLevel { get; set; }
        public int? BrandParentId { get; set; }
        public int? BrowsingClassId { get; set; }
        public string BrowsingClassName { get; set; }
        public int? BrowsingLevel { get; set; }
        public int? BrowsingParentId { get; set; }
        public int MerchandiseClassId { get; set; }
        public string MerchandiseClassName { get; set; }
        public int MerchandiseLevel { get; set; }
        public int? MerchandiseParentId { get; set; }
        public int TaxClassId { get; set; }
        public string TaxClassName { get; set; }
        public int TaxLevel { get; set; }
        public int? TaxParentId { get; set; }
        public string FinancialClassId { get; set; }
        public string FinancialClassName { get; set; }
        public int FinancialLevel { get; set; }
        public int? FinancialParentId { get; set; }
        public string AnimalWelfareRating { get; set; }
        public bool? Biodynamic { get; set; }
        public string CheeseMilkType { get; set; }
        public bool? CheeseRaw { get; set; }
        public string EcoScaleRating { get; set; }
        public string GlutenFreeAgency { get; set; }
        public string HealthyEatingRating { get; set; }
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
        public string CustomerFriendlyDescription { get; set; }
        public string NutritionRequired { get; set; }
        public string GlobalPricingProgram { get; set; }
        public string SelfCheckoutItemTareGroup { get; set; }
        public string FlexibleText { get; set; }
        public int? ShelfLife { get; set; }
        public string FairTradeCertified { get; set; }
        public string MadeWithOrganicGrapes { get; set; }
        public bool? PrimeBeef { get; set; }
        public bool? RainforestAlliance { get; set; }
        public string Refrigerated { get; set; }
        public bool? SmithsonianBirdFriendly { get; set; }
        public bool? WicEligible { get; set; }
        public int NationalClassId { get; set; }
        public string NationalClassName { get; set; }
        public int NationalLevel { get; set; }
        public int? NationalParentId { get; set; }
        public bool? Hidden { get; set; }
        public string Plu { get; set; }
        public string RecipeName { get; set; }
        public string Allergens { get; set; }
        public string Ingredients { get; set; }
        public double? ServingsPerPortion { get; set; }
        public string ServingSizeDesc { get; set; }
        public string ServingPerContainer { get; set; }
        public int? HshRating { get; set; }
        public byte? ServingUnits { get; set; }
        public int? SizeWeight { get; set; }
        public int? Calories { get; set; }
        public int? CaloriesFat { get; set; }
        public int? CaloriesSaturatedFat { get; set; }
        public decimal? TotalFatWeight { get; set; }
        public short? TotalFatPercentage { get; set; }
        public decimal? SaturatedFatWeight { get; set; }
        public short? SaturatedFatPercent { get; set; }
        public decimal? PolyunsaturatedFat { get; set; }
        public decimal? MonounsaturatedFat { get; set; }
        public decimal? CholesterolWeight { get; set; }
        public short? CholesterolPercent { get; set; }
        public decimal? SodiumWeight { get; set; }
        public short? SodiumPercent { get; set; }
        public decimal? PotassiumWeight { get; set; }
        public short? PotassiumPercent { get; set; }
        public decimal? TotalCarbohydrateWeight { get; set; }
        public short? TotalCarbohydratePercent { get; set; }
        public decimal? DietaryFiberWeight { get; set; }
        public short? DietaryFiberPercent { get; set; }
        public decimal? SolubleFiber { get; set; }
        public decimal? InsolubleFiber { get; set; }
        public decimal? Sugar { get; set; }
        public decimal? SugarAlcohol { get; set; }
        public decimal? OtherCarbohydrates { get; set; }
        public decimal? ProteinWeight { get; set; }
        public short? ProteinPercent { get; set; }
        public short? VitaminA { get; set; }
        public short? Betacarotene { get; set; }
        public short? VitaminC { get; set; }
        public short? Calcium { get; set; }
        public short? Iron { get; set; }
        public short? VitaminD { get; set; }
        public short? VitaminE { get; set; }
        public short? Thiamin { get; set; }
        public short? Riboflavin { get; set; }
        public short? Niacin { get; set; }
        public short? VitaminB6 { get; set; }
        public short? Folate { get; set; }
        public short? VitaminB12 { get; set; }
        public short? Biotin { get; set; }
        public short? PantothenicAcid { get; set; }
        public short? Phosphorous { get; set; }
        public short? Iodine { get; set; }
        public short? Magnesium { get; set; }
        public short? Zinc { get; set; }
        public short? Copper { get; set; }
        public decimal? Transfat { get; set; }
        public int? CaloriesFromTransfat { get; set; }
        public decimal? Om6Fatty { get; set; }
        public decimal? Om3Fatty { get; set; }
        public decimal? Starch { get; set; }
        public short? Chloride { get; set; }
        public short? Chromium { get; set; }
        public short? VitaminK { get; set; }
        public short? Manganese { get; set; }
        public short? Molybdenum { get; set; }
        public short? Selenium { get; set; }
        public decimal? TransfatWeight { get; set; }
        public int? HazardousMaterialFlag { get; set; }
        public string HazardousMaterialTypeCode { get; set; }
        public string Line { get; set; }
        public string SKU { get; set; }
        public string PriceLine { get; set; }
        public string VariantSize { get; set; }
        public bool EStoreNutritionRequired { get; set; }
        public bool? PrimeNowEligible { get; set; }
        public bool EstoreEligible { get; set; }
        public bool? TSFEligible { get; set; }
        public bool? WFMEligilble { get; set; }
        public bool? Other3PEligible { get; set; }
        public bool? HospitalityItem { get; set; }

        public bool? KitchenItem { get; set; }

        public string KitchenDescription { get; set; }

        public string ImageURL { get; set; }

        public string DataSource { get; set; }

        public string NonGMOTransparency { get; set; }

        public Decimal? ItemDepth { get; set; }

        public Decimal? ItemHeight { get; set; }

        public Decimal? ItemWidth { get; set; }

        public Decimal? Cube { get; set; }

        public Decimal? ItemWeight { get; set; }

        public Decimal? TrayDepth { get; set; }

        public Decimal? TrayHeight { get; set; }

        public Decimal? TrayWidth { get; set; }

        public string Labeling { get; set; }

        public string CountryOfOrigin { get; set; }

        public string PackageGroup { get; set; }

        public string PackageGroupType { get; set; }

        public string PrivateLabel { get; set; }

        public string Appellation { get; set; }

        public bool? FairTradeClaim { get; set; }

        public bool? GlutenFreeClaim { get; set; }

        public bool? NonGMOClaim { get; set; }

        public bool? OrganicClaim { get; set; }

        public string Varietal { get; set; }

        public string BeerStyle { get; set; }

        public string LineExtension { get; set; }

        public bool? LocalLoanProducer { get; set; }

        public bool? OrganicPersonalCare { get; set; }

        public bool? Paleo { get; set; }

        public string ProductFlavorOrType { get; set; }

        public bool? CaseinFree { get; set; }

        public string DeliverySystem { get; set; }

        public bool? Hemp { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public Decimal? AddedSugarsWeight { get; set; }

        public short? AddedSugarsPercent { get; set; }

        public Decimal? CalciumWeight { get; set; }

        public Decimal? IronWeight { get; set; }

        public Decimal? VitaminDWeight { get; set; }
    }
}