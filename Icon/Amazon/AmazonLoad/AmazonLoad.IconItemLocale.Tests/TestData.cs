using AmazonLoad.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonLoad.IconItemLocale.Tests
{
    public class TestData
    {
        public ItemLocaleModelForWormhole ItemLocale_BU10042_UPC = new ItemLocaleModelForWormhole
        {
            RegionCode = "MA",
            BusinessUnit = 10042,
            InforItemId = 184758,
            ScanCode = "68993453888",
            LocaleName = "Arlington",
            LocaleTypeCode = "ST",
            LocaleTypeDesc = "Store",
            LocaleID = 617,
            ScanCodeTypeId = 1,
            ScanCodeTypeDesc = "UPC",
            ItemTypeCode = "RTL",
            ItemTypeDesc = "Retail Sale",
            Authorized = true,
            LockedForSale = false,
            Recall = true,
            Sold_By_Weight = false,
            Quantity_Required = false,
            Price_Required = true,
            QtyProhibit = false,
            VisualVerify = true,
            PosScaleTare = 1 * .01m,
            Case_Discount = false,
            TMDiscountEligible = true,
            AgeCode = 1,
            Restricted_Hours = true,
            LinkedItemId = null,
            LinkedItemScanCode = null,
            LinkedItemTypeCode = null,
        };

        public ItemLocaleModelForWormhole ItemLocale_BU10042_PosPlu = new ItemLocaleModelForWormhole
        {
            RegionCode = "MA",
            BusinessUnit = 10042,
            InforItemId = 18097,
            ScanCode = "81070",
            LocaleName = "Arlington",
            LocaleTypeCode = "ST",
            LocaleTypeDesc = "Store",
            LocaleID = 617,
            ScanCodeTypeId = 2,
            ScanCodeTypeDesc = "POS PLU",
            ItemTypeCode = "RTL",
            ItemTypeDesc = "Retail Sale",
            Authorized = true,
            LockedForSale = false,
            Recall = false,
            Sold_By_Weight = false,
            Quantity_Required = false,
            Price_Required = true,
            QtyProhibit = false,
            VisualVerify = false,
            PosScaleTare = 0m,
            Case_Discount = false,
            TMDiscountEligible = true,
            AgeCode = 2,
            Restricted_Hours = true,
            LinkedItemId = null,
            LinkedItemScanCode = null,
            LinkedItemTypeCode = null,
        };

        public ItemLocaleModelForWormhole ItemLocale_BU10042_ScalePlu = new ItemLocaleModelForWormhole
        {
            RegionCode = "MA",
            BusinessUnit = 10042,
            InforItemId = 2271068,
            ScanCode = "20655000000",
            LocaleName = "Arlington",
            LocaleTypeCode = "ST",
            LocaleTypeDesc = "Store",
            LocaleID = 617,
            ScanCodeTypeId = 3,
            ScanCodeTypeDesc = "SCALE PLU",
            ItemTypeCode = "RTL",
            ItemTypeDesc = "Retail Sale",
            Authorized = true,
            LockedForSale = false,
            Recall = false,
            Sold_By_Weight = true,
            Quantity_Required = false,
            Price_Required = false,
            QtyProhibit = true,
            VisualVerify = false,
            PosScaleTare = 0m,
            Case_Discount = true,
            TMDiscountEligible = true,
            AgeCode = 0,
            Restricted_Hours = false,
            LinkedItemId = null,
            LinkedItemScanCode = null,
            LinkedItemTypeCode = null,
        };

        public ItemLocaleModelForWormhole ItemLocale_BU10042_UPC_WithLinkedItem = new ItemLocaleModelForWormhole
        {
            RegionCode = "MA",
            BusinessUnit = 10042,
            InforItemId = 193595,
            ScanCode = "83715200163",
            LocaleName = "Arlington",
            LocaleTypeCode = "ST",
            LocaleTypeDesc = "Store",
            LocaleID = 617,
            ScanCodeTypeId = 1,
            ScanCodeTypeDesc = "UPC",
            ItemTypeCode = "RTL",
            ItemTypeDesc = "Retail Sale",
            Authorized = true,
            LockedForSale = false,
            Recall = false,
            Sold_By_Weight = false,
            Quantity_Required = false,
            Price_Required = false,
            QtyProhibit = false,
            VisualVerify = false,
            PosScaleTare = 0m,
            Case_Discount = true,
            TMDiscountEligible = true,
            AgeCode = null,
            Restricted_Hours = false,
            LinkedItemId = 10700,
            LinkedItemScanCode = "161",
            LinkedItemTypeCode = "DEP"
        };

        public ItemLocaleModelForWormhole ItemLocale_BU10048_UPC_NonAuthorized = new ItemLocaleModelForWormhole
        {
            RegionCode = "MA",
            BusinessUnit = 10048,
            InforItemId = 4034315,
            ScanCode = "7675012092",
            LocaleName = "Bethesda",
            LocaleTypeCode = "ST",
            LocaleTypeDesc = "Store",
            LocaleID = 591,
            ScanCodeTypeId = 1,
            ScanCodeTypeDesc = "UPC",
            ItemTypeCode = "RTL",
            ItemTypeDesc = "Retail Sale",
            Authorized = false,
            LockedForSale = false,
            Recall = false,
            Sold_By_Weight = false,
            Quantity_Required = false,
            Price_Required = true,
            QtyProhibit = false,
            VisualVerify = false,
            PosScaleTare = 0m,
            Case_Discount = false,
            TMDiscountEligible = true,
            AgeCode = 2,
            Restricted_Hours = true,
            LinkedItemId = null,
            LinkedItemScanCode = null,
            LinkedItemTypeCode = null,
        };

        public IconStoreModel GetTestStore(string region)
        {
            return new IconStoreModel
            {
                RegionCode = region,
                LocaleID = 617,
                LocaleName = "Arlington",
                BusinessUnit = 10042,
                LocaleTypeCode = "ST",
                LocaleTypeDesc = "Store"
            };
        }

        public List<ProductSelectionGroupModel> TestPsgs
        {
            get
            {
                return testPsgs;
            }
        }
        public List<ProductSelectionGroupModel> TestPsgsWithNulls
        {
            get
            {
                testPsgs.AddRange(testPsgsWithNulls);
                return testPsgs;
            }
        }

        private List<ProductSelectionGroupModel> testPsgs = new List<ProductSelectionGroupModel>
        {
                new ProductSelectionGroupModel
                {
                    ProductSelectionGroupName = "Prohibit_TM_Discount",
                    ProductSelectionGroupTypeId = 1,
                    TraitId = 9,
                    TraitValue = "0",
                    ProductSelectionGroupTypeName = "Consumable",
                },
                new ProductSelectionGroupModel
                {
                    ProductSelectionGroupName = "CaseDiscountEligible",
                    ProductSelectionGroupTypeId = 1,
                    TraitId = 10,
                    TraitValue = "1",
                    ProductSelectionGroupTypeName = "Consumable"
                },
                new ProductSelectionGroupModel
                {
                    ProductSelectionGroupName = "Prohibit_Case_Discount",
                    ProductSelectionGroupTypeId = 1,
                    TraitId = 10,
                    TraitValue = "0",
                    ProductSelectionGroupTypeName = "Consumable"
                },
                new ProductSelectionGroupModel
                {
                    ProductSelectionGroupName = "Prohibit_Discount_Items",
                    ProductSelectionGroupTypeId = 1,
                    TraitId = 11,
                    TraitValue = "1",
                    ProductSelectionGroupTypeName = "Consumable"
                },
                new ProductSelectionGroupModel
                {
                    ProductSelectionGroupName = "Restrict 18",
                    ProductSelectionGroupTypeId = 1,
                    TraitId = 12,
                    TraitValue = "1",
                    ProductSelectionGroupTypeName = "Consumable"
                },
                new ProductSelectionGroupModel
                {
                    ProductSelectionGroupName = "Restrict 21",
                    ProductSelectionGroupTypeId = 1,
                    TraitId = 12,
                    TraitValue = "2",
                    ProductSelectionGroupTypeName = "Consumable"
                },
                new ProductSelectionGroupModel
                {
                    ProductSelectionGroupName = "DateRestriction",
                    ProductSelectionGroupTypeId = 1,
                    TraitId = 14,
                    TraitValue = "1",
                    ProductSelectionGroupTypeName = "Consumable"
                },
        };
        private List<ProductSelectionGroupModel> testPsgsWithNulls = new List<ProductSelectionGroupModel>
        {
            new ProductSelectionGroupModel
            {
                ProductSelectionGroupName = "Specialty Container Group",
                ProductSelectionGroupTypeId = 2,
                TraitId = null,
                TraitValue = null,
                ProductSelectionGroupTypeName = "OnlineConsumable",
            },
            new ProductSelectionGroupModel
            {
                ProductSelectionGroupName = "Bag_Fee",
                ProductSelectionGroupTypeId = 1,
                TraitId = null,
                TraitValue = null,
                ProductSelectionGroupTypeName = "Consumable",
            },
        };
    }
}
