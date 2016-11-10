﻿
using System;
using System.CodeDom.Compiler;

namespace Icon.Framework
{
    /// <summary>
    /// Trait auto generated constants
    /// </summary>

    [GeneratedCode("TextTemplatingFileGenerator", "10")]
    public static class TraitPatterns
    {
        public const string ProductDescription = @"^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,60}$";
        public const string PosDescription = @"^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,25}$";
        public const string PackageUnit = @"^[1-9][0-9]{0,2}$";
        public const string FoodStampEligible = @"0|1";
        public const string PosScaleTare = @"0|1";
        public const string DepartmentSale = @"0|1";
        public const string GiftCard = @"0|1";
        public const string RetailSize = @"^[0-9]*\.?[0-9]+$";
        public const string RetailUom = @"^[a-zA-z ]+$";
        public const string TmDiscountEligible = @"0|1";
        public const string CaseDiscountEligible = @"0|1";
        public const string ProhibitDiscount = @"0|1";
        public const string AgeRestrict = @"^[0-9]*\.?[0-9]+$";
        public const string Recall = @"0|1";
        public const string RestrictedHours = @"^[0-9]*\.?[0-9]+$";
        public const string SoldByWeight = @"0|1";
        public const string ForceTare = @"0|1";
        public const string QuantityRequired = @"0|1";
        public const string PriceRequired = @"0|1";
        public const string QuantityProhibit = @"^[0-9]*\.?[0-9]+$";
        public const string VisualVerify = @"0|1";
        public const string LockedForSale = @"0|1";
        public const string AuthorizedForSale = @"0|1";
        public const string Delete = @"0|1";
        public const string Locale = @"^[0-9]*\.?[0-9]+$";
        public const string Price = @"^[0-9]*\.?[0-9]+$";
        public const string PriceMultiple = @"^[0-9]*\.?[0-9]+$";
        public const string PriceStartDate = @"^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$";
        public const string PriceEndDate = @"^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$";
        public const string ShortRomance = @"^[a-zA-Z0-9_]*$";
        public const string LongRomance = @"^[a-zA-Z0-9_]*$";
        public const string GlutenFree = @"0|1";
        public const string PremiumBodyCare = @"0|1";
        public const string Exclusive = @"0|1";
        public const string WholeTrade = @"0|1";
        public const string NonGmo = @"0|1";
        public const string Hsh = @"0|1";
        public const string E2 = @"0|1";
        public const string Vegan = @"0|1";
        public const string Vegetarian = @"0|1";
        public const string Kosher = @"0|1";
        public const string EcoScaleRating = @"Baseline|Premium|Ultra-Premium";
        public const string Organic = @"0|1";
        public const string RegionAbbreviation = @"^[a-zA-Z0-9_]*$";
        public const string PsBusinessUnitId = @"^[a-zA-Z0-9_]*$";
        public const string ScancodeVersion = @"^[0-9]*\.?[0-9]+$";
        public const string InsertDate = @"^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$";
        public const string ModifiedDate = @"^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$";
        public const string ValidationDate = @"^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$";
        public const string MerchFinMapping = @"^[a-zA-Z0-9_]*$";
        public const string GlAccount = @"^[a-zA-Z0-9_]*$";
        public const string TaxAbbreviation = @"^[\d]{7} [^<>]{1,42}$";
        public const string SubBrickCode = @"^[a-zA-Z0-9_]*$";
        public const string FinancialHierarchyCode = @"^[a-zA-Z0-9_]*$";
        public const string SentToEsb = @"^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$";
        public const string PhoneNumber = @"^[0-9]{10,11}$";
        public const string ContactPerson = @"^[a-zA-Z0-9_]*$";
        public const string StoreAbbreviation = @"^[a-zA-Z0-9_]*$";
        public const string NonMerchandise = @"Bottle Deposit|CRV|Coupon|Bottle Return|CRV Credit|Legacy POS Only|Blackhawk Fee|Non-Retail";
        public const string LinkedScanCode = @"^[0-9]*\.?[0-9]+$";
        public const string ModifiedUser = @"^[a-zA-Z0-9_]*$";
        public const string Affinity = @"1";
        public const string PosDepartmentNumber = @"^[0-9_]*$";
        public const string TeamNumber = @"^[0-9_]*$";
        public const string TeamName = @"^[a-zA-Z0-9_]*$";
        public const string NonAlignedSubteam = @"1";
        public const string BrandAbbreviation = @"^[a-zA-Z0-9 &]{1,10}$";
        public const string TaxRomance = @"^[\d]{7} [^<>]{1,142}$";
        public const string MerchDefaultTaxAssociatation = @"^[0-9]*$";
        public const string NationalClassCode = @"^[0-9]*$";
        public const string HiddenItem = @"0|1";
        public const string Notes = @"^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,255}$";
        public const string IrmaStoreId = @"^[a-zA-Z0-9_]*$";
        public const string StorePosType = @"^[a-zA-Z0-9_]*$";
        public const string Fax = @"^[a-zA-Z0-9_]*$";
        public const string DefaultCertificationAgency = @"1";
        public const string AnimalWelfareRating = @"^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$";
        public const string Biodynamic = @"^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$";
        public const string CheeseMilkType = @"^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$";
        public const string CheeseRaw = @"^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$";
        public const string HealthyEatingRating = @"^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$";
        public const string AirChilled = @"^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$";
        public const string DryAged = @"^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$";
        public const string FreeRange = @"^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$";
        public const string GrassFed = @"^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$";
        public const string MadeInHouse = @"^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$";
        public const string Msc = @"^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$";
        public const string PastureRaised = @"^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$";
        public const string SeafoodCatchType = @"^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$";
        public const string FreshOrFrozen = @"^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,50}$";
        public const string RecipeName = @"";
        public const string Allergens = @"";
        public const string Ingredients = @"";
        public const string Selenium = @"";
        public const string PolyunsaturatedFat = @"";
        public const string MonounsaturatedFat = @"";
        public const string PotassiumWeight = @"";
        public const string PotassiumPercent = @"";
        public const string DietaryFiberPercent = @"";
        public const string SolubleFiber = @"";
        public const string InsolubleFiber = @"";
        public const string SugarAlcohol = @"";
        public const string OtherCarbohydrates = @"";
        public const string ProteinPercent = @"";
        public const string Betacarotene = @"";
        public const string VitaminD = @"";
        public const string VitaminE = @"";
        public const string Thiamin = @"";
        public const string Riboflavin = @"";
        public const string Niacin = @"";
        public const string VitaminB6 = @"";
        public const string Folate = @"";
        public const string VitaminB12 = @"";
        public const string Biotin = @"";
        public const string PantothenicAcid = @"";
        public const string Phosphorous = @"";
        public const string Iodine = @"";
        public const string Magnesium = @"";
        public const string Zinc = @"";
        public const string Copper = @"";
        public const string Transfat = @"";
        public const string Om6Fatty = @"";
        public const string Om3Fatty = @"";
        public const string Starch = @"";
        public const string Chloride = @"";
        public const string Chromium = @"";
        public const string VitaminK = @"";
        public const string Manganese = @"";
        public const string Molybdenum = @"";
        public const string CaloriesFromTransFat = @"";
        public const string CaloriesSaturatedFat = @"";
        public const string ServingPerContainer = @"";
        public const string ServingSizeDesc = @"";
        public const string ServingsPerPortion = @"";
        public const string ServingUnits = @"";
        public const string SizeWeight = @"";
        public const string TransfatWeight = @"";
        public const string DeliverySystem = @"^[a-zA-z ]+$";
        public const string CaseinFree = @"0|1";
        public const string FairTradeCertified = @"Fair Trade USA|Fair Trade International|IMO USA|Rainforest Alliance|Whole Foods Market";
        public const string Hemp = @"0|1";
        public const string OrganicPersonalCare = @"0|1";
        public const string NutritionRequired = @"0|1";
        public const string DrainedWeight = @"^\d+(\.\d{1,4})?$";
        public const string DrainedWeightUom = @"OZ|ML";
        public const string AlcoholByVolume = @"^(?!0\d)\d{1,2}(\.\d{1,2})?$";
        public const string MainProductName = @"^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,255}$";
        public const string ProductFlavorType = @"^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,255}$";
        public const string Paleo = @"0|1";
        public const string LocalLoanProducer = @"0|1";
	}
}
