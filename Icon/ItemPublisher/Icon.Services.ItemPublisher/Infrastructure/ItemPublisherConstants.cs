using System.Collections.Generic;

namespace Icon.Services.ItemPublisher.Infrastructure
{
    public static class ItemPublisherConstants
    {
        public static class Attributes
        {
            public const string DepartmentSale = "DepartmentSale";
            public const string HospitalityItem = "HospitalityItem";
            public const string KitchenItem = "KitchenItem";
            public const string Url1 = "URL1";
            public const string Kosher = "Kosher";
            public const string KitchenDescription = "KitchenDescription";
            public const string FoodStampEligible = "FoodStampEligible";
            public const string ProhibitDiscount = "ProhibitDiscount";
            public const string Vegetarian = "Vegetarian";

            public static readonly List<string> SpecialAttributesWithAgencyNames = new List<string> { Attributes.Vegetarian };
        }

        public static class DataTypes
        {
            public const string Boolean = "Boolean";
            public const string Text = "Text";
        }

        public static class ProductSelectionGroups
        {
            public const string FoodStamp = "Food_Stamp";
            public const string ProhibitDiscountItems = "Prohibit_Discount_Items";
        }

        public static class TraitCodes
        {
            public const string UomTraitCode = "RUM";
        }

        public const string RetailSaleTypeCodeDescription = "Retail Sale";
        public const string RetailSaleTypeCode = "RTL";
        public const string NonRetailSaleTypeCode = "NRT";
    }
}