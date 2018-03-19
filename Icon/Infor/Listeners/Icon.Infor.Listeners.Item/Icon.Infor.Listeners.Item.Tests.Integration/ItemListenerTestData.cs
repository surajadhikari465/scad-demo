using Icon.Esb.Subscriber;
using Icon.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;

namespace Icon.Infor.Listeners.Item.Tests.Integration
{
    public class ItemListenerTestData
    {
        private const string message_1_id = "ec339683-14e6-4142-8de9-7b5c00960d62";

        private const string item_A_ScanCode = "888888888";
        private const int item_A_ItemId = 999999999;

        private const int item_A_BrandClass_Id = 77253;
        private const string item_A_BrandClass_Name = @"MELISSA &amp; DOUG";
        private const int item_A_BrandClass_ParentId = 0;

        private const int item_A_TaxClass_Id = 0000000;
        private const string item_A_TaxClass_Name = "0000000 GENERAL MERCHANDISE";
        private const int item_A_TaxClass_ParentId = 0;

        private const int item_A_NatClass_Id = 4000505;
        //private const string item_A_NatClass_Name = "";
        private const int item_A_NatClass_ParentId = 4000499;

        private const int item_A_FinClass_Id = 6460;
        private const string item_A_FinClass_Name = "Lifestyle (6460)";
        private const int item_A_FinClass_ParentId = 0;

        private const int item_A_MerchClass_Id = 83633;
        //private const string item_A_MerchClass_Name = "";
        private const int item_A_MerchClass_ParentId = 83146;

        //private const string item_B_ScanCode = "77206234";
        //private const int item_B_ItemId = 170477;        

        public string Message_1_ID => message_1_id;
        public string Item_A_ScanCode => item_A_ScanCode;
        public int Item_A_ItemID => item_A_ItemId;
        //public string Item_B_ScanCode => item_B_ScanCode;
        //public int Item_B_ItemID => item_B_ItemId;

        public IEsbMessage GetMockEsbMessage(string xmlData, string iconMessageId = message_1_id)
        {
            Mock<IEsbMessage> mockMessage = new Mock<IEsbMessage>();
            mockMessage.Setup(m => m.MessageText).Returns(xmlData);
            mockMessage.Setup(m => m.GetProperty("IconMessageID")).Returns(iconMessageId);

            return mockMessage.Object;
        }
        public SampleAttributeData Attribs = new SampleAttributeData();

        public class SampleAttributeData
        {
            //  source: "" (empty) | target: SLAW, ESL | level: Runtime
            private bool sampleTrait_taxable = true;
            public bool Taxable => sampleTrait_taxable;

            //  source: if({LabelTypeDesc}="SMALL",1,0) | target: SLAW, ESL | level: Runtime
            private string sampleTrait_labelTypeID = "test LabelTypeID val";
            public string LabelTypeID => sampleTrait_labelTypeID;

            //  source: IFNULL(TPRPrice,RegularPrice,TprPrice) | target: SLAW, ESL, OnePlum | level: Runtime
            private decimal sampleTrait_effectivePrice = 8.88M;
            public decimal EffectivePrice => sampleTrait_effectivePrice;

            //  source: IFNULL(TPRPrice,RegularPriceMultiple,TprPriceMultiple) | target: SLAW, ESL, OnePlum | level: Runtime
            private int sampleTrait_effectivePriceMultiple = 1;
            public int EffectivePriceMultiple => sampleTrait_effectivePriceMultiple;

            //  source: IFNULL(TPRPrice,RegularPriceReason,TprPriceReason) | target: SLAW, ESL, OnePlum | level: Runtime
            private string sampleTrait_effectivePriceReason = "test Effective Price Reason val";
            public string EffectivePriceReason => sampleTrait_effectivePriceReason;

            //  source: IFNULL(TPRPrice,RegularPriceType,TprPriceType) | target: SLAW, ESL, OnePlum | level: Runtime
            private string sampleTrait_effectivePriceType = "test Effective Price Type val";
            public string EffectivePriceType => sampleTrait_effectivePriceType;

            //  source: IFNULL(TPRPrice,RegularSellableUOM,TprSellableUOM) | target: SLAW, ESL, OnePlum | level: Runtime
            private string sampleTrait_effectiveSellingUnit = "test Effective Selling Unit val";
            public string EffectiveSellingUnit => sampleTrait_effectiveSellingUnit;

            //  source: Infor GPM | target: SLAW, ESL | level: Price
            private DateTime sampleTrait_newTagExpiration = new DateTime();
            public DateTime NewTagExpiration => sampleTrait_newTagExpiration;

            //  source: Infor GPM | target: SLAW, ESL, OnePlum | level: Price
            private string sampleTrait_regularSellableUOM = "test Reg Selling Unit val";
            public string RegularSellableUOM => sampleTrait_regularSellableUOM;

            //  source: Infor GPM | target: SLAW, ESL, OnePlum | level: Price
            private decimal sampleTrait_rewardPrice = 8.88M;
            public decimal RewardPrice => sampleTrait_rewardPrice;

            //  source: Infor GPM | target: SLAW, ESL, OnePlum | level: Price
            private DateTime sampleTrait_rewardPriceEndDate = new DateTime();
            public DateTime RewardPriceEndDate => sampleTrait_rewardPriceEndDate;

            //  source: Infor GPM | target: SLAW, ESL, OnePlum | level: Price
            private int sampleTrait_rewardPriceMultiple = 1;
            public int RewardPriceMultiple => sampleTrait_rewardPriceMultiple;

            //  source: Infor GPM | target: SLAW, ESL, OnePlum | level: Price
            private string sampleTrait_rewardPriceReason = "test Reward Price Reason val";
            public string RewardPriceReason => sampleTrait_rewardPriceReason;

            //  source: Infor GPM | target: SLAW, ESL, OnePlum | level: Price
            private DateTime sampleTrait_rewardPriceStartDate = new DateTime();
            public DateTime RewardPriceStartDate => sampleTrait_rewardPriceStartDate;

            //  source: Infor GPM | target: SLAW, ESL, OnePlum | level: Price
            private string sampleTrait_rewardPriceType = "test Reward Price Type val";
            public string RewardPriceType => sampleTrait_rewardPriceType;

            //  source: Infor GPM | target: SLAW, ESL, OnePlum | level: Price
            private string sampleTrait_rewardSellableUOM = "test Reward Selling Unit val";
            public string RewardSellableUOM => sampleTrait_rewardSellableUOM;

            //  source: Infor GPM | target: SLAW, ESL, OnePlum | level: Price
            private string sampleTrait_tprSellableUOM = "test TPR Selling Unit val";
            public string TprSellableUOM => sampleTrait_tprSellableUOM;

            //  source: Infor/Icon | target: SLAW, ESL, OnePlum | level: Item
            private string sampleTrait_allergens = "test Allergens val";
            public string Allergens => sampleTrait_allergens;

            //  source: Infor/Icon | target: SLAW, ESL, OnePlum | level: Item
            private string sampleTrait_animalWelfareRating = "test AnimalWelfareRating val";
            public string AnimalWelfareRating => sampleTrait_animalWelfareRating;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private string sampleTrait_brandName = "test Brand_Name val";
            public string BrandName => sampleTrait_brandName;

            //  source: Infor/Icon | target: SLAW, ESL, OnePlum | level: Store
            private int sampleTrait_businessUnitID = 1;
            public int BusinessUnitID => sampleTrait_businessUnitID;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private string sampleTrait_cheeseMilkType = "test CheeseMilkType val";
            public string CheeseMilkType => sampleTrait_cheeseMilkType;

            //  source: Infor/Icon | target: SLAW, ESL, OnePlum | level: Item
            private string sampleTrait_customerFriendlyDescription = "test Customer Friendly Description val";
            public string CustomerFriendlyDescription => sampleTrait_customerFriendlyDescription;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private string sampleTrait_ecoScaleRating = "test EcoScaleRating val";
            public string EcoScaleRating => sampleTrait_ecoScaleRating;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private string sampleTrait_fairTrade = "test Fair Trade val";
            public string FairTrade => sampleTrait_fairTrade;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private string sampleTrait_flexibleText = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure etc...";
            public string FlexibleText => sampleTrait_flexibleText;

            //  source: Infor/Icon | target: SLAW, ESL, OnePlum | level: Item
            private string sampleTrait_freshOrFrozenSeafood = "test FreshOrFrozen val";
            public string FreshOrFrozenSeafood => sampleTrait_freshOrFrozenSeafood;

            //  source: Infor/Icon | target: SLAW, ESL, OnePlum | level: Store
            private string sampleTrait_geographicalState = "test Store State val";
            public string GeographicalState => sampleTrait_geographicalState;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private string sampleTrait_globalPricingProgram = "test Global Pricing Program val";
            public string GlobalPricingProgram => sampleTrait_globalPricingProgram;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private string sampleTrait_glutenFreeAgency = "test GlutenFree val";
            public string GlutenFreeAgency => sampleTrait_glutenFreeAgency;

            //  source: Infor/Icon | target: SLAW, ESL, OnePlum | level: Item
            private string sampleTrait_healthyEatingRating = "test HealthyEatingRating val";
            public string HealthyEatingRating => sampleTrait_healthyEatingRating;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private bool sampleTrait_isAirChilled = true;
            public bool IsAirChilled => sampleTrait_isAirChilled;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private bool sampleTrait_isBiodynamic = true;
            public bool IsBiodynamic => sampleTrait_isBiodynamic;

            //  source: Infor/Icon | target: SLAW, ESL, OnePlum | level: Item
            private bool sampleTrait_isCheeseRaw = true;
            public bool IsCheeseRaw => sampleTrait_isCheeseRaw;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private bool sampleTrait_isDryAged = true;
            public bool IsDryAged => sampleTrait_isDryAged;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private bool sampleTrait_isFreeRange = true;
            public bool IsFreeRange => sampleTrait_isFreeRange;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private bool sampleTrait_isGrassFed = true;
            public bool IsGrassFed => sampleTrait_isGrassFed;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private bool sampleTrait_isMadeInHouse = true;
            public bool IsMadeInHouse => sampleTrait_isMadeInHouse;

            //  source: Infor/Icon | target: SLAW, ESL, OnePlum | level: Item
            private bool sampleTrait_isMsc = true;
            public bool IsMsc => sampleTrait_isMsc;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private bool sampleTrait_isPastureRaised = true;
            public bool IsPastureRaised => sampleTrait_isPastureRaised;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private bool sampleTrait_isPremiumBodyCare = true;
            public bool IsPremiumBodyCare => sampleTrait_isPremiumBodyCare;

            //  source: Infor/Icon | target: SLAW, ESL, OnePlum | level: Item
            private bool sampleTrait_isVegetarian = true;
            public bool IsVegetarian => sampleTrait_isVegetarian;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private bool sampleTrait_isWholeTrade = true;
            public bool IsWholeTrade => sampleTrait_isWholeTrade;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private string sampleTrait_itemDescription = "test ItemDescription val";
            public string ItemDescription => sampleTrait_itemDescription;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private string sampleTrait_itemType = "test Item Type (was Retail Sale) val";
            public string ItemType => sampleTrait_itemType;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private string sampleTrait_kosherAgency = "";
            public string KosherAgency => sampleTrait_kosherAgency;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private string sampleTrait_madeWithOrganicGrapes = "Agricultural Services Certified Organic (ASCO)";
            public string MadeWithOrganicGrapes => sampleTrait_madeWithOrganicGrapes;

            //  source: Infor/Icon | target: SLAW, ESL, OnePlum | level: Item
            private string sampleTrait_merchandiseSubBrick = "test SubcategoryID val";
            public string MerchandiseSubBrick => sampleTrait_merchandiseSubBrick;

            //  source: Infor/Icon | target: SLAW, ESL, OnePlum | level: Item
            private string sampleTrait_nonGmoAgency = "test NonGmo val";
            public string NonGmoAgency => sampleTrait_nonGmoAgency;

            //  source: Infor/Icon | target: SLAW, ESL, OnePlum | level: Item
            private string sampleTrait_nutritionRequired = "test NutritionRequired val";
            public string NutritionRequired => sampleTrait_nutritionRequired;

            //  source: Infor/Icon | target: SLAW, ESL, OnePlum | level: Item
            private string sampleTrait_organicAgency = "test Organic val";
            public string OrganicAgency => sampleTrait_organicAgency;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private string sampleTrait_packageUnit = "test PackageSize val";
            public string PackageUnit => sampleTrait_packageUnit;

            //  source: Infor/Icon | target: OnePlum | level: Item Locale
            private string sampleTrait_percentageTareWeight = "25";
            public string PercentageTareWeight => sampleTrait_percentageTareWeight;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private string sampleTrait_posDescription = "test POS_Desc val";
            public string PosDescription => sampleTrait_posDescription;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private string sampleTrait_primeBeef = "Yes";
            public string PrimeBeef => sampleTrait_primeBeef;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private string sampleTrait_rainforestAlliance = "Yes";
            public string RainforestAlliance => sampleTrait_rainforestAlliance;

            //  source: Infor/Icon | target: SLAW, ESL, OnePlum | level: Item
            private string sampleTrait_refrigerated = "Shelf Stable";
            public string Refrigerated => sampleTrait_refrigerated;

            //  source: Infor/Icon | target: SLAW, ESL, OnePlum | level: Store
            private string sampleTrait_regionAbbrev = "QQ";
            public string RegionAbbrev => sampleTrait_regionAbbrev;

            //  source: Infor/Icon | target: SLAW, ESL, OnePlum | level: Item
            private string sampleTrait_retailSize = "test RetailSize val";
            public string RetailSize => sampleTrait_retailSize;

            //  source: Infor/Icon | target: SLAW, ESL, OnePlum | level: Item
            private string sampleTrait_retailUOM = "test RetailUOM val";
            public string RetailUOM => sampleTrait_retailUOM;

            //  source: Infor/Icon | target: SLAW, ESL, OnePlum | level: Item
            private string sampleTrait_scanCode = "94011";
            public string ScanCode => sampleTrait_scanCode;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private string sampleTrait_seafoodCatchType = "test SeafoodCatchType val";
            public string SeafoodCatchType => sampleTrait_seafoodCatchType;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private string sampleTrait_smithsonianBirdFriendly = "No";
            public string SmithsonianBirdFriendly => sampleTrait_smithsonianBirdFriendly;

            //  source: Infor/Icon | target: SLAW, ESL, OnePlum | level: Item
            private int sampleTrait_subTeamNumber = 1;
            public int SubTeamNumber => sampleTrait_subTeamNumber;

            //  source: Infor/Icon | target: SLAW, ESL, OnePlum | level: Item
            private string sampleTrait_veganAgency = "1";
            public string VeganAgency => sampleTrait_veganAgency;

            //  source: Infor/Icon | target: SLAW, ESL | level: Item
            private string sampleTrait_wIC = "No";
            public string WIC => sampleTrait_wIC;

            //  source: IRMA/Mammoth | target: SLAW, ESL | level: Item Locale
            private string sampleTrait_altRetailSize = "test Alternate Size val";
            public string AltRetailSize => sampleTrait_altRetailSize;

            //  source: IRMA/Mammoth | target: SLAW, ESL | level: Item Locale
            private string sampleTrait_altRetailUOM = "test Alternate UOM val";
            public string AltRetailUOM => sampleTrait_altRetailUOM;

            //  source: IRMA/Mammoth | target: OnePlum | level: Item Locale
            private string sampleTrait_cfsSendToScale = "test SendtoCFS val";
            public string CfsSendToScale => sampleTrait_cfsSendToScale;

            //  source: IRMA/Mammoth | target: SLAW, ESL, OnePlum | level: Item Locale
            private string sampleTrait_colorAdded = "test ColorAdded val";
            public string ColorAdded => sampleTrait_colorAdded;

            //  source: IRMA/Mammoth | target: SLAW, ESL, OnePlum | level: Item Locale
            private string sampleTrait_countryOfProcessing = "test Country of Processing val";
            public string CountryOfProcessing => sampleTrait_countryOfProcessing;

            //  source: IRMA/Mammoth | target: OnePlum | level: Item Locale
            private bool sampleTrait_defaultScanCode = true;
            public bool DefaultScanCode => sampleTrait_defaultScanCode;

            //  source: IRMA/Mammoth | target: SLAW, ESL | level: Item Locale
            private string sampleTrait_discontinued = "test Discontinued val";
            public string Discontinued => sampleTrait_discontinued;

            //  source: IRMA/Mammoth | target: SLAW, ESL | level: Item Locale
            private string sampleTrait_exclusive = "test Exclusive val";
            public string Exclusive => sampleTrait_exclusive;

            //  source: IRMA/Mammoth | target: SLAW, ESL, OnePlum | level: Item Locale
            private string sampleTrait_extraText = "test Scale/Non-Scale ExtraText val";
            public string ExtraText => sampleTrait_extraText;

            //  source: IRMA/Mammoth | target: OnePlum | level: Item Locale
            private string sampleTrait_forceTare = "1";
            public string ForceTare => sampleTrait_forceTare;

            //  source: IRMA/Mammoth | target: SLAW, ESL | level: Item Locale
            private string sampleTrait_labelTypeDesc = "test LabelTypeDesc val";
            public string LabelTypeDesc => sampleTrait_labelTypeDesc;

            //  source: IRMA/Mammoth | target: SLAW, ESL, OnePlum | level: Item Locale
            private string sampleTrait_linkedScanCodeBrand = "test LinkCodeBrandName val";
            public string LinkedScanCodeBrand => sampleTrait_linkedScanCodeBrand;

            //  source: IRMA/Mammoth | target: SLAW, ESL, OnePlum | level: Item Locale
            private string sampleTrait_linkedScanCodePrice = "test LinkCodePrice val";
            public string LinkedScanCodePrice => sampleTrait_linkedScanCodePrice;

            //  source: IRMA/Mammoth | target: SLAW, ESL | level: Item Locale
            private string sampleTrait_localItem = "test Local Item val";
            public string LocalItem => sampleTrait_localItem;

            //  source: IRMA/Mammoth | target: SLAW, ESL | level: Item Locale
            private string sampleTrait_locality = "test Locality val";
            public string Locality => sampleTrait_locality;

            //  source: IRMA/Mammoth | target: SLAW, ESL | level: Item Locale
            private string sampleTrait_orderedByInfor = "test Ordered by Predictix val";
            public string OrderedByInfor => sampleTrait_orderedByInfor;

            //  source: IRMA/Mammoth | target: SLAW, ESL, OnePlum | level: Item Locale
            private string sampleTrait_origin = "test Origin val";
            public string Origin => sampleTrait_origin;

            //  source: IRMA/Mammoth | target: SLAW, ESL | level: Item Locale
            private string sampleTrait_productCode = "test Product Code val";
            public string ProductCode => sampleTrait_productCode;

            //  source: IRMA/Mammoth | target: SLAW, ESL, OnePlum | level: Item Locale
            private string sampleTrait_retailUnit = "test RetailUnit val";
            public string RetailUnit => sampleTrait_retailUnit;

            //  source: IRMA/Mammoth | target: SLAW, ESL, OnePlum | level: Item Locale
            private bool sampleTrait_scaleItem = true;
            public bool ScaleItem => sampleTrait_scaleItem;

            //  source: IRMA/Mammoth | target: SLAW, ESL, OnePlum | level: Item Locale
            private string sampleTrait_scalePLUDigits = "test ScalePLUDigits val";
            public string ScalePLUDigits => sampleTrait_scalePLUDigits;

            //  source: IRMA/Mammoth | target: OnePlum | level: Item Locale
            private string sampleTrait_shelfLife = "88";
            public string ShelfLife => sampleTrait_shelfLife;

            //  source: IRMA/Mammoth | target: SLAW, ESL | level: Item Locale
            private string sampleTrait_signDescription = "test Sign Description val";
            public string SignDescription => sampleTrait_signDescription;

            //  source: IRMA/Mammoth | target: SLAW, ESL | level: Item Locale
            private string sampleTrait_signRomanceTextLong = "test SignRomanceTextLong val";
            public string SignRomanceTextLong => sampleTrait_signRomanceTextLong;

            //  source: IRMA/Mammoth | target: SLAW, ESL | level: Item Locale
            private string sampleTrait_signRomanceTextShort = "test SignRomanceTextShort val";
            public string SignRomanceTextShort => sampleTrait_signRomanceTextShort;

            //  source: IRMA/Mammoth | target: OnePlum | level: Item Locale
            private string sampleTrait_unwrappedTareWeight = "test Unwrapped Tare Weight val";
            public string UnwrappedTareWeight => sampleTrait_unwrappedTareWeight;

            //  source: IRMA/Mammoth | target: SLAW, ESL | level: Item Locale
            private string sampleTrait_uomRegulationChicagoBaby = "test UomRegulationChicagoBaby val";
            public string UomRegulationChicagoBaby => sampleTrait_uomRegulationChicagoBaby;

            //  source: IRMA/Mammoth | target: SLAW, ESL | level: Item Locale
            private string sampleTrait_uomRegulationTagUom = "test UomRegulationTagUom val";
            public string UomRegulationTagUom => sampleTrait_uomRegulationTagUom;

            //  source: IRMA/Mammoth | target: OnePlum | level: Item Locale
            private string sampleTrait_useBy = "test Use By val";
            public string UseBy => sampleTrait_useBy;

            //  source: IRMA/Mammoth | target: SLAW, ESL | level: Item Locale
            private decimal sampleTrait_vendorCaseSize = 8.88M;
            public decimal VendorCaseSize => sampleTrait_vendorCaseSize;

            //  source: IRMA/Mammoth | target: SLAW, ESL | level: Item Locale
            private string sampleTrait_vendorItemID = "test Vendor_Item_ID val";
            public string VendorItemID => sampleTrait_vendorItemID;

            //  source: IRMA/Mammoth | target: SLAW, ESL | level: Item Locale
            private string sampleTrait_vendorKey = "test Vendor_Key val";
            public string VendorKey => sampleTrait_vendorKey;

            //  source: IRMA/Mammoth | target: SLAW, ESL | level: Item Locale
            private string sampleTrait_vendorName = "test Vendor Name val";
            public string VendorName => sampleTrait_vendorName;

            //  source: IRMA/Mammoth | target: OnePlum | level: Item Locale
            private string sampleTrait_wrappedTareWeight = "test Wrapped Tare Weight val";
            public string WrappedTareWeight => sampleTrait_wrappedTareWeight;

            //  source: IRMA/Mammoth or Infor GPM | target: SLAW, ESL | level: Price
            private string sampleTrait_priceReason = "test Price Reason val";
            public string PriceReason => sampleTrait_priceReason;

            //  source: IRMA/Mammoth or Infor GPM | target: SLAW, ESL, OnePlum | level: Price
            private string sampleTrait_priceType = "test Price Type val";
            public string PriceType => sampleTrait_priceType;

            //  source: IRMA/Mammoth or Infor GPM | target: SLAW, ESL, OnePlum | level: Price
            private decimal sampleTrait_regularPrice = 8.88M;
            public decimal RegularPrice => sampleTrait_regularPrice;

            //  source: IRMA/Mammoth or Infor GPM | target: SLAW, ESL, OnePlum | level: Price
            private int sampleTrait_regularPriceMultiple = 1;
            public int RegularPriceMultiple => sampleTrait_regularPriceMultiple;

            //  source: IRMA/Mammoth or Infor GPM | target: SLAW, ESL, OnePlum | level: Price
            private string sampleTrait_regularPriceReason = "test Reg Price Reason val";
            public string RegularPriceReason => sampleTrait_regularPriceReason;

            //  source: IRMA/Mammoth or Infor GPM | target: SLAW, ESL, OnePlum | level: Price
            private string sampleTrait_regularPriceType = "test Reg Price Type val";
            public string RegularPriceType => sampleTrait_regularPriceType;

            //  source: IRMA/Mammoth or Infor GPM | target: SLAW, ESL, OnePlum | level: Price
            private DateTime sampleTrait_regularStartDate = new DateTime();
            public DateTime RegularStartDate => sampleTrait_regularStartDate;

            //  source: IRMA/Mammoth or Infor GPM | target: SLAW, ESL, OnePlum | level: Price
            private DateTime sampleTrait_tprEndDate = new DateTime();
            public DateTime TprEndDate => sampleTrait_tprEndDate;

            //  source: IRMA/Mammoth or Infor GPM | target: SLAW, ESL, OnePlum | level: Price
            private int sampleTrait_tprMultiple = 1;
            public int TprMultiple => sampleTrait_tprMultiple;

            //  source: IRMA/Mammoth or Infor GPM | target: SLAW, ESL, OnePlum | level: Price
            private decimal sampleTrait_tprPrice = 8.88M;
            public decimal TprPrice => sampleTrait_tprPrice;

            //  source: IRMA/Mammoth or Infor GPM | target: SLAW, ESL, OnePlum | level: Price
            private string sampleTrait_tprPriceReason = "test TPR Price Reason val";
            public string TprPriceReason => sampleTrait_tprPriceReason;

            //  source: IRMA/Mammoth or Infor GPM | target: SLAW, ESL, OnePlum | level: Price
            private string sampleTrait_tprPriceType = "test TPR Price Type val";
            public string TprPriceType => sampleTrait_tprPriceType;

            //  source: IRMA/Mammoth or Infor GPM | target: SLAW, ESL, OnePlum | level: Price
            private DateTime sampleTrait_tprStartDate = new DateTime();
            public DateTime TprStartDate => sampleTrait_tprStartDate;

            //  source: Nutrichef/Icon | target: SLAW, ESL, OnePlum | level: Nutrichef/Icon
            //private object sampleTrait_all Nutrition Fields… = "";
            //public object All Nutrition Fields… => sampleTrait_all Nutrition Fields…;

            //  source: Nutrichef/Icon | target: SLAW, ESL, OnePlum | level: Nutrition
            private int sampleTrait_calories = 1;
            public int Calories => sampleTrait_calories;

            //  source: Nutrichef/Icon | target: SLAW, ESL, OnePlum | level: Nutrition
            private int sampleTrait_ingredients = 1;
            public int Ingredients => sampleTrait_ingredients;

            //  source: Nutrichef/Icon | target: SLAW, ESL, OnePlum | level: Nutrition
            private string sampleTrait_servingSizeDesc = "test ServingSizeDesc val";
            public string ServingSizeDesc => sampleTrait_servingSizeDesc;

            //  source: Nutrichef/Icon | target: SLAW, ESL, OnePlum | level: Nutrition
            private double sampleTrait_servingsPerPortion = 3.33D;
            public double ServingsPerPortion => sampleTrait_servingsPerPortion;

            //  source: Nutrichef/Icon | target: SLAW, ESL, OnePlum | level: Nutrition
            private decimal sampleTrait_sodiumWeight = 8.88M;
            public decimal SodiumWeight => sampleTrait_sodiumWeight;

            //  source: Nutrichef/Icon | target: SLAW, ESL, OnePlum | level: Nutrition
            private decimal sampleTrait_sugar = 8.88M;
            public decimal Sugar => sampleTrait_sugar;

            //  source: Nutrichef/Icon | target: SLAW, ESL, OnePlum | level: Nutrichef/Icon
            private decimal sampleTrait_totalFatWeight = 8.88M;
            public decimal TotalFatWeight => sampleTrait_totalFatWeight;

            //  source: today as US short date (MM/dd/yyyy) | target: ESL, OnePlum | level: Runtime
            private DateTime sampleTrait_effectiveDate = new DateTime();
            public DateTime EffectiveDate => sampleTrait_effectiveDate;

            //  source:  | target:  | level: 
            private string sampleTrait_selfCheckoutItemTareGroup = "Prep Foods Container Group";
            public string SelfCheckoutItemTareGroup => sampleTrait_selfCheckoutItemTareGroup;
        }
    }
}
