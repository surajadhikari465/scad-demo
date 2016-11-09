using System;
using Irma.Framework;

namespace Irma.Testing.Builders
{
    public class TestItemOverrideBuilder
    {
        private int item_Key;
        private int storeJurisdictionID;
        private string item_Description;
        private string sign_Description;
        private decimal package_Desc1;
        private decimal package_Desc2;
        private int package_Unit_ID;
        private int retail_Unit_ID;
        private int vendor_Unit_ID;
        private int distribution_Unit_ID;
        private string pOS_Description;
        private bool food_Stamps;
        private bool price_Required;
        private bool quantity_Required;
        private System.Nullable<int> manufacturing_Unit_ID;
        private System.Nullable<bool> qtyProhibit;
        private System.Nullable<int> groupList;
        private System.Nullable<bool> case_Discount;
        private System.Nullable<bool> coupon_Multiplier;
        private System.Nullable<short> misc_Transaction_Sale;
        private System.Nullable<short> misc_Transaction_Refund;
        private System.Nullable<int> ice_Tare;
        private System.Nullable<int> brand_ID;
        private System.Nullable<int> origin_ID;
        private System.Nullable<int> countryProc_ID;
        private System.Nullable<bool> sustainabilityRankingRequired;
        private System.Nullable<int> sustainabilityRankingID;
        private System.Nullable<int> labelType_ID;
        private bool costedByWeight;
        private System.Nullable<decimal> average_Unit_Weight;
        private bool ingredient;
        private System.Nullable<bool> recall_Flag;
        private System.Nullable<bool> lockAuth;
        private bool not_Available;
        private string not_AvailableNote;
        private bool fSA_Eligible;
        private string product_Code;
        private System.Nullable<int> unit_Price_Category;
        private System.Nullable<int> lastModifiedUser_ID;

        public TestItemOverrideBuilder()
        {
            this.item_Key = 0;
            this.storeJurisdictionID = 0;
            this.item_Description = null;
            this.sign_Description = null;
            this.package_Desc1 = 0;
            this.package_Desc2 = 0;
            this.package_Unit_ID = 0;
            this.retail_Unit_ID = 0;
            this.vendor_Unit_ID = 0;
            this.distribution_Unit_ID = 0;
            this.pOS_Description = null;
            this.food_Stamps = false;
            this.price_Required = false;
            this.quantity_Required = false;
            this.manufacturing_Unit_ID = null;
            this.qtyProhibit = null;
            this.groupList = null;
            this.case_Discount = null;
            this.coupon_Multiplier = null;
            this.misc_Transaction_Sale = null;
            this.misc_Transaction_Refund = null;
            this.ice_Tare = null;
            this.brand_ID = null;
            this.origin_ID = null;
            this.countryProc_ID = null;
            this.sustainabilityRankingRequired = null;
            this.sustainabilityRankingID = null;
            this.labelType_ID = null;
            this.costedByWeight = false;
            this.average_Unit_Weight = null;
            this.ingredient = false;
            this.recall_Flag = null;
            this.lockAuth = null;
            this.not_Available = false;
            this.not_AvailableNote = null;
            this.fSA_Eligible = false;
            this.product_Code = null;
            this.unit_Price_Category = null;
            this.lastModifiedUser_ID = null;
        }

        public TestItemOverrideBuilder WithItem_Key(int item_Key)
        {
            this.item_Key = item_Key;
            return this;
        }

        public TestItemOverrideBuilder WithStoreJurisdictionID(int storeJurisdictionID)
        {
            this.storeJurisdictionID = storeJurisdictionID;
            return this;
        }

        public TestItemOverrideBuilder WithItem_Description(string item_Description)
        {
            this.item_Description = item_Description;
            return this;
        }

        public TestItemOverrideBuilder WithSign_Description(string sign_Description)
        {
            this.sign_Description = sign_Description;
            return this;
        }

        public TestItemOverrideBuilder WithPackage_Desc1(decimal package_Desc1)
        {
            this.package_Desc1 = package_Desc1;
            return this;
        }

        public TestItemOverrideBuilder WithPackage_Desc2(decimal package_Desc2)
        {
            this.package_Desc2 = package_Desc2;
            return this;
        }

        public TestItemOverrideBuilder WithPackage_Unit_ID(int package_Unit_ID)
        {
            this.package_Unit_ID = package_Unit_ID;
            return this;
        }

        public TestItemOverrideBuilder WithRetail_Unit_ID(int retail_Unit_ID)
        {
            this.retail_Unit_ID = retail_Unit_ID;
            return this;
        }

        public TestItemOverrideBuilder WithVendor_Unit_ID(int vendor_Unit_ID)
        {
            this.vendor_Unit_ID = vendor_Unit_ID;
            return this;
        }

        public TestItemOverrideBuilder WithDistribution_Unit_ID(int distribution_Unit_ID)
        {
            this.distribution_Unit_ID = distribution_Unit_ID;
            return this;
        }

        public TestItemOverrideBuilder WithPOS_Description(string pOS_Description)
        {
            this.pOS_Description = pOS_Description;
            return this;
        }

        public TestItemOverrideBuilder WithFood_Stamps(bool food_Stamps)
        {
            this.food_Stamps = food_Stamps;
            return this;
        }

        public TestItemOverrideBuilder WithPrice_Required(bool price_Required)
        {
            this.price_Required = price_Required;
            return this;
        }

        public TestItemOverrideBuilder WithQuantity_Required(bool quantity_Required)
        {
            this.quantity_Required = quantity_Required;
            return this;
        }

        public TestItemOverrideBuilder WithManufacturing_Unit_ID(System.Nullable<int> manufacturing_Unit_ID)
        {
            this.manufacturing_Unit_ID = manufacturing_Unit_ID;
            return this;
        }

        public TestItemOverrideBuilder WithQtyProhibit(System.Nullable<bool> qtyProhibit)
        {
            this.qtyProhibit = qtyProhibit;
            return this;
        }

        public TestItemOverrideBuilder WithGroupList(System.Nullable<int> groupList)
        {
            this.groupList = groupList;
            return this;
        }

        public TestItemOverrideBuilder WithCase_Discount(System.Nullable<bool> case_Discount)
        {
            this.case_Discount = case_Discount;
            return this;
        }

        public TestItemOverrideBuilder WithCoupon_Multiplier(System.Nullable<bool> coupon_Multiplier)
        {
            this.coupon_Multiplier = coupon_Multiplier;
            return this;
        }

        public TestItemOverrideBuilder WithMisc_Transaction_Sale(System.Nullable<short> misc_Transaction_Sale)
        {
            this.misc_Transaction_Sale = misc_Transaction_Sale;
            return this;
        }

        public TestItemOverrideBuilder WithMisc_Transaction_Refund(System.Nullable<short> misc_Transaction_Refund)
        {
            this.misc_Transaction_Refund = misc_Transaction_Refund;
            return this;
        }

        public TestItemOverrideBuilder WithIce_Tare(System.Nullable<int> ice_Tare)
        {
            this.ice_Tare = ice_Tare;
            return this;
        }

        public TestItemOverrideBuilder WithBrand_ID(System.Nullable<int> brand_ID)
        {
            this.brand_ID = brand_ID;
            return this;
        }

        public TestItemOverrideBuilder WithOrigin_ID(System.Nullable<int> origin_ID)
        {
            this.origin_ID = origin_ID;
            return this;
        }

        public TestItemOverrideBuilder WithCountryProc_ID(System.Nullable<int> countryProc_ID)
        {
            this.countryProc_ID = countryProc_ID;
            return this;
        }

        public TestItemOverrideBuilder WithSustainabilityRankingRequired(System.Nullable<bool> sustainabilityRankingRequired)
        {
            this.sustainabilityRankingRequired = sustainabilityRankingRequired;
            return this;
        }

        public TestItemOverrideBuilder WithSustainabilityRankingID(System.Nullable<int> sustainabilityRankingID)
        {
            this.sustainabilityRankingID = sustainabilityRankingID;
            return this;
        }

        public TestItemOverrideBuilder WithLabelType_ID(System.Nullable<int> labelType_ID)
        {
            this.labelType_ID = labelType_ID;
            return this;
        }

        public TestItemOverrideBuilder WithCostedByWeight(bool costedByWeight)
        {
            this.costedByWeight = costedByWeight;
            return this;
        }

        public TestItemOverrideBuilder WithAverage_Unit_Weight(System.Nullable<decimal> average_Unit_Weight)
        {
            this.average_Unit_Weight = average_Unit_Weight;
            return this;
        }

        public TestItemOverrideBuilder WithIngredient(bool ingredient)
        {
            this.ingredient = ingredient;
            return this;
        }

        public TestItemOverrideBuilder WithRecall_Flag(System.Nullable<bool> recall_Flag)
        {
            this.recall_Flag = recall_Flag;
            return this;
        }

        public TestItemOverrideBuilder WithLockAuth(System.Nullable<bool> lockAuth)
        {
            this.lockAuth = lockAuth;
            return this;
        }

        public TestItemOverrideBuilder WithNot_Available(bool not_Available)
        {
            this.not_Available = not_Available;
            return this;
        }

        public TestItemOverrideBuilder WithNot_AvailableNote(string not_AvailableNote)
        {
            this.not_AvailableNote = not_AvailableNote;
            return this;
        }

        public TestItemOverrideBuilder WithFSA_Eligible(bool fSA_Eligible)
        {
            this.fSA_Eligible = fSA_Eligible;
            return this;
        }

        public TestItemOverrideBuilder WithProduct_Code(string product_Code)
        {
            this.product_Code = product_Code;
            return this;
        }

        public TestItemOverrideBuilder WithUnit_Price_Category(System.Nullable<int> unit_Price_Category)
        {
            this.unit_Price_Category = unit_Price_Category;
            return this;
        }

        public TestItemOverrideBuilder WithLastModifiedUser_ID(System.Nullable<int> lastModifiedUser_ID)
        {
            this.lastModifiedUser_ID = lastModifiedUser_ID;
            return this;
        }

        public ItemOverride Build()
        {
            ItemOverride itemOverride = new ItemOverride();

            itemOverride.Item_Key = this.item_Key;
            itemOverride.StoreJurisdictionID = this.storeJurisdictionID;
            itemOverride.Item_Description = this.item_Description;
            itemOverride.Sign_Description = this.sign_Description;
            itemOverride.Package_Desc1 = this.package_Desc1;
            itemOverride.Package_Desc2 = this.package_Desc2;
            itemOverride.Package_Unit_ID = this.package_Unit_ID;
            itemOverride.Retail_Unit_ID = this.retail_Unit_ID;
            itemOverride.Vendor_Unit_ID = this.vendor_Unit_ID;
            itemOverride.Distribution_Unit_ID = this.distribution_Unit_ID;
            itemOverride.POS_Description = this.pOS_Description;
            itemOverride.Food_Stamps = this.food_Stamps;
            itemOverride.Price_Required = this.price_Required;
            itemOverride.Quantity_Required = this.quantity_Required;
            itemOverride.Manufacturing_Unit_ID = this.manufacturing_Unit_ID;
            itemOverride.QtyProhibit = this.qtyProhibit;
            itemOverride.GroupList = this.groupList;
            itemOverride.Case_Discount = this.case_Discount;
            itemOverride.Coupon_Multiplier = this.coupon_Multiplier;
            itemOverride.Misc_Transaction_Sale = this.misc_Transaction_Sale;
            itemOverride.Misc_Transaction_Refund = this.misc_Transaction_Refund;
            itemOverride.Ice_Tare = this.ice_Tare;
            itemOverride.Brand_ID = this.brand_ID;
            itemOverride.Origin_ID = this.origin_ID;
            itemOverride.CountryProc_ID = this.countryProc_ID;
            itemOverride.SustainabilityRankingRequired = this.sustainabilityRankingRequired;
            itemOverride.SustainabilityRankingID = this.sustainabilityRankingID;
            itemOverride.LabelType_ID = this.labelType_ID;
            itemOverride.CostedByWeight = this.costedByWeight;
            itemOverride.Average_Unit_Weight = this.average_Unit_Weight;
            itemOverride.Ingredient = this.ingredient;
            itemOverride.Recall_Flag = this.recall_Flag;
            itemOverride.LockAuth = this.lockAuth;
            itemOverride.Not_Available = this.not_Available;
            itemOverride.Not_AvailableNote = this.not_AvailableNote;
            itemOverride.FSA_Eligible = this.fSA_Eligible;
            itemOverride.Product_Code = this.product_Code;
            itemOverride.Unit_Price_Category = this.unit_Price_Category;
            itemOverride.LastModifiedUser_ID = this.lastModifiedUser_ID;

            return itemOverride;
        }

        public static implicit operator ItemOverride(TestItemOverrideBuilder builder)
        {
            return builder.Build();
        }
    }
}