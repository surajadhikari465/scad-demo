using System;
using Irma.Framework;

namespace Irma.Testing.Builders
{
    public class TestPriceBuilder
    {
        private int item_Key;
        private int store_No;
        private byte multiple;
        private decimal price1;
        private decimal mSRPPrice;
        private byte mSRPMultiple;
        private int pricingMethod_ID;
        private byte sale_Multiple;
        private decimal sale_Price;
        private System.Nullable<System.DateTime> sale_Start_Date;
        private System.Nullable<System.DateTime> sale_End_Date;
        private byte sale_Max_Quantity;
        private byte sale_Earned_Disc1;
        private byte sale_Earned_Disc2;
        private byte sale_Earned_Disc3;
        private bool restricted_Hours;
        private System.Nullable<System.DateTime> avgCostUpdated;
        private bool iBM_Discount;
        private System.Nullable<decimal> pOSPrice;
        private System.Nullable<decimal> pOSSale_Price;
        private System.Nullable<bool> notAuthorizedForSale;
        private System.Nullable<bool> compFlag;
        private System.Nullable<int> posTare;
        private System.Nullable<int> linkedItem;
        private System.Nullable<bool> grillPrint;
        private System.Nullable<int> ageCode;
        private System.Nullable<bool> visualVerify;
        private System.Nullable<bool> srCitizenDiscount;
        private System.Nullable<byte> priceChgTypeId;
        private System.Nullable<int> exceptionSubteam_No;
        private string pOSLinkCode;
        private System.Nullable<int> kitchenRoute_ID;
        private System.Nullable<byte> routing_Priority;
        private System.Nullable<bool> consolidate_Price_To_Prev_Item;
        private System.Nullable<bool> print_Condiment_On_Receipt;
        private System.Nullable<bool> age_Restrict;
        private System.Nullable<int> competitivePriceTypeID;
        private System.Nullable<byte> bandwidthPercentageHigh;
        private System.Nullable<byte> bandwidthPercentageLow;
        private System.Nullable<int> mixMatch;
        private bool discountable;
        private System.Nullable<int> lastScannedUserId_DTS;
        private System.Nullable<int> lastScannedUserId_NonDTS;
        private System.Nullable<System.DateTime> lastScannedDate_DTS;
        private System.Nullable<System.DateTime> lastScannedDate_NonDTS;
        private bool localItem;
        private System.Nullable<int> itemSurcharge;
        private System.Nullable<bool> electronicShelfTag;

        public TestPriceBuilder()
        {
            this.item_Key = 0;
            this.store_No = 0;
            this.multiple = 0;
            this.price1 = 0;
            this.mSRPPrice = 0;
            this.mSRPMultiple = 0;
            this.pricingMethod_ID = 0;
            this.sale_Multiple = 0;
            this.sale_Price = 0;
            this.sale_Start_Date = null;
            this.sale_End_Date = null;
            this.sale_Max_Quantity = 0;
            this.sale_Earned_Disc1 = 0;
            this.sale_Earned_Disc2 = 0;
            this.sale_Earned_Disc3 = 0;
            this.restricted_Hours = false;
            this.avgCostUpdated = null;
            this.iBM_Discount = false;
            this.pOSPrice = null;
            this.pOSSale_Price = null;
            this.notAuthorizedForSale = null;
            this.compFlag = null;
            this.posTare = null;
            this.linkedItem = null;
            this.grillPrint = null;
            this.ageCode = null;
            this.visualVerify = null;
            this.srCitizenDiscount = null;
            this.priceChgTypeId = null;
            this.exceptionSubteam_No = null;
            this.pOSLinkCode = null;
            this.kitchenRoute_ID = null;
            this.routing_Priority = null;
            this.consolidate_Price_To_Prev_Item = null;
            this.print_Condiment_On_Receipt = null;
            this.age_Restrict = null;
            this.competitivePriceTypeID = null;
            this.bandwidthPercentageHigh = null;
            this.bandwidthPercentageLow = null;
            this.mixMatch = null;
            this.discountable = false;
            this.lastScannedUserId_DTS = null;
            this.lastScannedUserId_NonDTS = null;
            this.lastScannedDate_DTS = null;
            this.lastScannedDate_NonDTS = null;
            this.localItem = false;
            this.itemSurcharge = null;
            this.electronicShelfTag = null;
        }

        public TestPriceBuilder WithItem_Key(int item_Key)
        {
            this.item_Key = item_Key;
            return this;
        }

        public TestPriceBuilder WithStore_No(int store_No)
        {
            this.store_No = store_No;
            return this;
        }

        public TestPriceBuilder WithMultiple(byte multiple)
        {
            this.multiple = multiple;
            return this;
        }

        public TestPriceBuilder WithPrice1(decimal price1)
        {
            this.price1 = price1;
            return this;
        }

        public TestPriceBuilder WithMSRPPrice(decimal mSRPPrice)
        {
            this.mSRPPrice = mSRPPrice;
            return this;
        }

        public TestPriceBuilder WithMSRPMultiple(byte mSRPMultiple)
        {
            this.mSRPMultiple = mSRPMultiple;
            return this;
        }

        public TestPriceBuilder WithPricingMethod_ID(int pricingMethod_ID)
        {
            this.pricingMethod_ID = pricingMethod_ID;
            return this;
        }

        public TestPriceBuilder WithSale_Multiple(byte sale_Multiple)
        {
            this.sale_Multiple = sale_Multiple;
            return this;
        }

        public TestPriceBuilder WithSale_Price(decimal sale_Price)
        {
            this.sale_Price = sale_Price;
            return this;
        }

        public TestPriceBuilder WithSale_Start_Date(System.Nullable<System.DateTime> sale_Start_Date)
        {
            this.sale_Start_Date = sale_Start_Date;
            return this;
        }

        public TestPriceBuilder WithSale_End_Date(System.Nullable<System.DateTime> sale_End_Date)
        {
            this.sale_End_Date = sale_End_Date;
            return this;
        }

        public TestPriceBuilder WithSale_Max_Quantity(byte sale_Max_Quantity)
        {
            this.sale_Max_Quantity = sale_Max_Quantity;
            return this;
        }

        public TestPriceBuilder WithSale_Earned_Disc1(byte sale_Earned_Disc1)
        {
            this.sale_Earned_Disc1 = sale_Earned_Disc1;
            return this;
        }

        public TestPriceBuilder WithSale_Earned_Disc2(byte sale_Earned_Disc2)
        {
            this.sale_Earned_Disc2 = sale_Earned_Disc2;
            return this;
        }

        public TestPriceBuilder WithSale_Earned_Disc3(byte sale_Earned_Disc3)
        {
            this.sale_Earned_Disc3 = sale_Earned_Disc3;
            return this;
        }

        public TestPriceBuilder WithRestricted_Hours(bool restricted_Hours)
        {
            this.restricted_Hours = restricted_Hours;
            return this;
        }

        public TestPriceBuilder WithAvgCostUpdated(System.Nullable<System.DateTime> avgCostUpdated)
        {
            this.avgCostUpdated = avgCostUpdated;
            return this;
        }

        public TestPriceBuilder WithIBM_Discount(bool iBM_Discount)
        {
            this.iBM_Discount = iBM_Discount;
            return this;
        }

        public TestPriceBuilder WithPOSPrice(System.Nullable<decimal> pOSPrice)
        {
            this.pOSPrice = pOSPrice;
            return this;
        }

        public TestPriceBuilder WithPOSSale_Price(System.Nullable<decimal> pOSSale_Price)
        {
            this.pOSSale_Price = pOSSale_Price;
            return this;
        }

        public TestPriceBuilder WithNotAuthorizedForSale(System.Nullable<bool> notAuthorizedForSale)
        {
            this.notAuthorizedForSale = notAuthorizedForSale;
            return this;
        }

        public TestPriceBuilder WithCompFlag(System.Nullable<bool> compFlag)
        {
            this.compFlag = compFlag;
            return this;
        }

        public TestPriceBuilder WithPosTare(System.Nullable<int> posTare)
        {
            this.posTare = posTare;
            return this;
        }

        public TestPriceBuilder WithLinkedItem(System.Nullable<int> linkedItem)
        {
            this.linkedItem = linkedItem;
            return this;
        }

        public TestPriceBuilder WithGrillPrint(System.Nullable<bool> grillPrint)
        {
            this.grillPrint = grillPrint;
            return this;
        }

        public TestPriceBuilder WithAgeCode(System.Nullable<int> ageCode)
        {
            this.ageCode = ageCode;
            return this;
        }

        public TestPriceBuilder WithVisualVerify(System.Nullable<bool> visualVerify)
        {
            this.visualVerify = visualVerify;
            return this;
        }

        public TestPriceBuilder WithSrCitizenDiscount(System.Nullable<bool> srCitizenDiscount)
        {
            this.srCitizenDiscount = srCitizenDiscount;
            return this;
        }

        public TestPriceBuilder WithPriceChgTypeId(System.Nullable<byte> priceChgTypeId)
        {
            this.priceChgTypeId = priceChgTypeId;
            return this;
        }

        public TestPriceBuilder WithExceptionSubteam_No(System.Nullable<int> exceptionSubteam_No)
        {
            this.exceptionSubteam_No = exceptionSubteam_No;
            return this;
        }

        public TestPriceBuilder WithPOSLinkCode(string pOSLinkCode)
        {
            this.pOSLinkCode = pOSLinkCode;
            return this;
        }

        public TestPriceBuilder WithKitchenRoute_ID(System.Nullable<int> kitchenRoute_ID)
        {
            this.kitchenRoute_ID = kitchenRoute_ID;
            return this;
        }

        public TestPriceBuilder WithRouting_Priority(System.Nullable<byte> routing_Priority)
        {
            this.routing_Priority = routing_Priority;
            return this;
        }

        public TestPriceBuilder WithConsolidate_Price_To_Prev_Item(System.Nullable<bool> consolidate_Price_To_Prev_Item)
        {
            this.consolidate_Price_To_Prev_Item = consolidate_Price_To_Prev_Item;
            return this;
        }

        public TestPriceBuilder WithPrint_Condiment_On_Receipt(System.Nullable<bool> print_Condiment_On_Receipt)
        {
            this.print_Condiment_On_Receipt = print_Condiment_On_Receipt;
            return this;
        }

        public TestPriceBuilder WithAge_Restrict(System.Nullable<bool> age_Restrict)
        {
            this.age_Restrict = age_Restrict;
            return this;
        }

        public TestPriceBuilder WithCompetitivePriceTypeID(System.Nullable<int> competitivePriceTypeID)
        {
            this.competitivePriceTypeID = competitivePriceTypeID;
            return this;
        }

        public TestPriceBuilder WithBandwidthPercentageHigh(System.Nullable<byte> bandwidthPercentageHigh)
        {
            this.bandwidthPercentageHigh = bandwidthPercentageHigh;
            return this;
        }

        public TestPriceBuilder WithBandwidthPercentageLow(System.Nullable<byte> bandwidthPercentageLow)
        {
            this.bandwidthPercentageLow = bandwidthPercentageLow;
            return this;
        }

        public TestPriceBuilder WithMixMatch(System.Nullable<int> mixMatch)
        {
            this.mixMatch = mixMatch;
            return this;
        }

        public TestPriceBuilder WithDiscountable(bool discountable)
        {
            this.discountable = discountable;
            return this;
        }

        public TestPriceBuilder WithLastScannedUserId_DTS(System.Nullable<int> lastScannedUserId_DTS)
        {
            this.lastScannedUserId_DTS = lastScannedUserId_DTS;
            return this;
        }

        public TestPriceBuilder WithLastScannedUserId_NonDTS(System.Nullable<int> lastScannedUserId_NonDTS)
        {
            this.lastScannedUserId_NonDTS = lastScannedUserId_NonDTS;
            return this;
        }

        public TestPriceBuilder WithLastScannedDate_DTS(System.Nullable<System.DateTime> lastScannedDate_DTS)
        {
            this.lastScannedDate_DTS = lastScannedDate_DTS;
            return this;
        }

        public TestPriceBuilder WithLastScannedDate_NonDTS(System.Nullable<System.DateTime> lastScannedDate_NonDTS)
        {
            this.lastScannedDate_NonDTS = lastScannedDate_NonDTS;
            return this;
        }

        public TestPriceBuilder WithLocalItem(bool localItem)
        {
            this.localItem = localItem;
            return this;
        }

        public TestPriceBuilder WithItemSurcharge(System.Nullable<int> itemSurcharge)
        {
            this.itemSurcharge = itemSurcharge;
            return this;
        }

        public TestPriceBuilder WithElectronicShelfTag(System.Nullable<bool> electronicShelfTag)
        {
            this.electronicShelfTag = electronicShelfTag;
            return this;
        }

        public Price Build()
        {
            Price price = new Price();

            price.Item_Key = this.item_Key;
            price.Store_No = this.store_No;
            price.Multiple = this.multiple;
            price.Price1 = this.price1;
            price.MSRPPrice = this.mSRPPrice;
            price.MSRPMultiple = this.mSRPMultiple;
            price.PricingMethod_ID = this.pricingMethod_ID;
            price.Sale_Multiple = this.sale_Multiple;
            price.Sale_Price = this.sale_Price;
            price.Sale_Start_Date = this.sale_Start_Date;
            price.Sale_End_Date = this.sale_End_Date;
            price.Sale_Max_Quantity = this.sale_Max_Quantity;
            price.Sale_Earned_Disc1 = this.sale_Earned_Disc1;
            price.Sale_Earned_Disc2 = this.sale_Earned_Disc2;
            price.Sale_Earned_Disc3 = this.sale_Earned_Disc3;
            price.Restricted_Hours = this.restricted_Hours;
            price.AvgCostUpdated = this.avgCostUpdated;
            price.IBM_Discount = this.iBM_Discount;
            price.POSPrice = this.pOSPrice;
            price.POSSale_Price = this.pOSSale_Price;
            price.NotAuthorizedForSale = this.notAuthorizedForSale;
            price.CompFlag = this.compFlag;
            price.PosTare = this.posTare;
            price.LinkedItem = this.linkedItem;
            price.GrillPrint = this.grillPrint;
            price.AgeCode = this.ageCode;
            price.VisualVerify = this.visualVerify;
            price.SrCitizenDiscount = this.srCitizenDiscount;
            price.PriceChgTypeId = this.priceChgTypeId;
            price.ExceptionSubteam_No = this.exceptionSubteam_No;
            price.POSLinkCode = this.pOSLinkCode;
            price.KitchenRoute_ID = this.kitchenRoute_ID;
            price.Routing_Priority = this.routing_Priority;
            price.Consolidate_Price_To_Prev_Item = this.consolidate_Price_To_Prev_Item;
            price.Print_Condiment_On_Receipt = this.print_Condiment_On_Receipt;
            price.Age_Restrict = this.age_Restrict;
            price.CompetitivePriceTypeID = this.competitivePriceTypeID;
            price.BandwidthPercentageHigh = this.bandwidthPercentageHigh;
            price.BandwidthPercentageLow = this.bandwidthPercentageLow;
            price.MixMatch = this.mixMatch;
            price.Discountable = this.discountable;
            price.LastScannedUserId_DTS = this.lastScannedUserId_DTS;
            price.LastScannedUserId_NonDTS = this.lastScannedUserId_NonDTS;
            price.LastScannedDate_DTS = this.lastScannedDate_DTS;
            price.LastScannedDate_NonDTS = this.lastScannedDate_NonDTS;
            price.LocalItem = this.localItem;
            price.ItemSurcharge = this.itemSurcharge;
            price.ElectronicShelfTag = this.electronicShelfTag;

            return price;
        }

        public static implicit operator Price(TestPriceBuilder builder)
        {
            return builder.Build();
        }
    }
}