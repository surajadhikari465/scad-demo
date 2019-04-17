using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Item.Models
{
    public class ItemModel
    {
        public string ErrorCode { get; set; }
        public string ErrorDetails { get; set; }
        public int ItemId { get; set; }
        public string ItemTypeCode { get; set; }
        public string ScanCode { get; set; }
        public string ScanCodeType { get; set; }
        public string MerchandiseHierarchyClassId { get; set; }
        public string BrandsHierarchyClassId { get; set; }
        public string TaxHierarchyClassId { get; set; }
        public string FinancialHierarchyClassId { get; set; }
        public string NationalHierarchyClassId { get; set; }
        public string ProductDescription { get; set; }
        public string PosDescription { get; set; }
        public string FoodStampEligible { get; set; }
        public string PosScaleTare { get; set; }
        public string ProhibitDiscount { get; set; }
        public string PackageUnit { get; set; }
        public string RetailSize { get; set; }
        public string RetailUom { get; set; }
        public string AnimalWelfareRating { get; set; }
        public string Biodynamic { get; set; }
        public string MilkType { get; set; }
        public string CheeseRaw { get; set; }
        public string EcoScaleRating { get; set; }
        public string GlutenFree { get; set; }
        public string Kosher { get; set; }
        public string Msc { get; set; }
        public string NonGmo { get; set; }
        public string Organic { get; set; }
        public string PremiumBodyCare { get; set; }
        public string FreshOrFrozen { get; set; }
        public string SeafoodCatchType { get; set; }
        public string Vegan { get; set; }
        public string Vegetarian { get; set; }
        public string WholeTrade { get; set; }
        public string GrassFed { get; set; }
        public string PastureRaised { get; set; }
        public string FreeRange { get; set; }
        public string DryAged { get; set; }
        public string AirChilled { get; set; }
        public string MadeInHouse { get; set; }
        public string AlcoholByVolume { get; set; }
        public string CaseinFree { get; set; }
        public string DrainedWeight { get; set; }
        public string DrainedWeightUom { get; set; }
        public string FairTradeCertified { get; set; }
        public string Hemp { get; set; }
        public string LocalLoanProducer { get; set; }
        public string MainProductName { get; set; }
        public string NutritionRequired { get; set; }
        public string OrganicPersonalCare { get; set; }
        public string Paleo { get; set; }
        public string ProductFlavorType { get; set; }
        public string Notes { get; set; }
        public string HiddenItem { get; set; }
        public string DeliverySystem { get; set; }
        public string InsertDate { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public Guid InforMessageId { get; set; }
        public bool ContainesDuplicateMerchandiseClass { get; set; }
        public bool ContainesDuplicateNationalClass { get; set; }
        public DateTime MessageParseTime { get; set; }
        public decimal? SequenceId { get; set; }
        public string CustomerFriendlyDescription { get; set; }
        public string GlobalPricingProgram { get; set; }
        public string FlexibleText { get; set; }
        public string MadeWithOrganicGrapes { get; set; }
        public string PrimeBeef { get; set; }
        public string RainforestAlliance { get; set; }
        public string Refrigerated { get; set; }
        public string SmithsonianBirdFriendly { get; set; }
        public string WicEligible { get; set; }
        public string ShelfLife { get; set; }
        public string SelfCheckoutItemTareGroup { get; set; }
        public string KitchenDescription { get; set; }
        public string ImageUrl { get; set; }
        public bool? KitchenItem { get; set; }
        public bool? HospitalityItem { get; set; }
        public string Line { get; set; }
        public string SKU { get; set; }
        public string PriceLine { get; set; }
        public string VariantSize { get; set; }
        public string EStoreNutritionRequired { get; set; }
        public string PrimeNowEligible { get; set; }
        public string EstoreEligible { get; set; }
        public string TSFEligible { get; set; }
        public string WFMEligilble { get; set; }
        public string Other3PEligible { get; set; }
        public string DataSource { get; set; }
        public string NonGMOTransparency { get; set; }
        public string ItemDepth { get; set; }
        public string ItemHeight { get; set; }
        public string ItemWidth { get; set; }
        public string Cube { get; set; }
        public string ItemWeight { get; set; }
        public string TrayDepth { get; set; }
        public string TrayHeight { get; set; }
        public string TrayWidth { get; set; }
        public string Labeling { get; set; }
        public string CountryofOrigin { get; set; }
        public string PackageGroup { get; set; }
        public string PackageGroupType { get; set; }
        public string PrivateLabel { get; set; }
        public string Appellation { get; set; }
        public string FairTradeClaim { get; set; }
        public string GlutenFreeClaim { get; set; }
        public string NonGMOClaim { get; set; }
        public string OrganicClaim { get; set; }
        public string Varietal { get; set; }
        public string BeerStyle { get; set; }
        public string LineExtension { get; set; }
    }
}