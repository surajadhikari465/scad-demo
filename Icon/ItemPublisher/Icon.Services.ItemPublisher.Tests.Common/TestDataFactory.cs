using Icon.Services.ItemPublisher.Infrastructure;
using Icon.Services.ItemPublisher.Infrastructure.Models;
using Icon.Services.ItemPublisher.Infrastructure.Models.Builders;
using Icon.Services.ItemPublisher.Infrastructure.Models.Mappers;
using Icon.Services.ItemPublisher.Repositories.Entities;
using System;
using System.Collections.Generic;

namespace Icon.Services.Newitem.Test.Common
{
    public class TestDataFactory
    {
        public Item Item { get; set; } = new Item()
        {
            ItemId = 999999999,
            ItemTypeId = 1,
            ItemTypeDescription = ItemPublisherConstants.RetailSaleTypeCodeDescription,
            ItemTypeCode = ItemPublisherConstants.RetailSaleTypeCode,
            ScanCode = "1568",
            ScanCodeId = 95,
            ScanCodeTypeDesc = "POS PLU",
            ScanCodeTypeId = 2,
            ItemAttributesJson = @"{
                'ItemID':'493',
                'ItemType':'RTL',
                'EStoreEligible':'No',
                'EStoreNutritionRequired':'No',
                'IsHospitalityItem':'1',
                'IsHospitalityItemSpecified':'1',
                'IsKitchenItem':'1',
                'IsKitchenItemSpecified':'1',
                'FoodStampEligible':1,
                'ProhibitDiscount':'1',
                'IsActive':1
            }",
            SysStartTimeUtc = DateTime.Parse("2019-05-13 21:27:55.7347574"),
            SysEndTimeUtc = DateTime.Parse("9999-12-31 23:59:59.9999999"),
        };

        public Dictionary<string, string> Traits = new Dictionary<string, string>();

        public List<Hierarchy> Hierarchy { get; set; } = new List<Hierarchy>()
            {
                new Hierarchy()
                {
                    HierarchyClassId = 999999999,
                    HierarchyId = 3,
                    HierarchyName = "Tax",
                    HierarchyClassName = "Prepared food",
                    HierarchyLevel = 1,
                    ItemId = 493,
                    HierarchyClassParentId = null,
                    HierarchyClassParentName = null
                },
                new Hierarchy()
                {
                    HierarchyClassId = 999999998,
                    HierarchyId = 5,
                    HierarchyName = "Financial",
                    HierarchyClassName = "Money stuff",
                    HierarchyLevel = 1,
                    ItemId = 493,
                    HierarchyClassParentId = null,
                    HierarchyClassParentName = null
                },
                new Hierarchy()
                {
                    HierarchyClassId = 999999997,
                    HierarchyId = 1,
                    HierarchyName = "Merchandise",
                    HierarchyClassName = "Merch stuff",
                    HierarchyLevel = 1,
                    ItemId = 493,
                    HierarchyClassParentId = null,
                    HierarchyClassParentName = null
                }
            };

        public Nutrition Nutrition { get; set; } = new Nutrition()
        {
            RecipeId = 999999999,
            Plu = "1568",
            RecipeName = "Coffee Bar Chai Latte 8oz CE",
            Allergens = "CONTAINS:MILK.",
            Ingredients = "Ingredients: Chai Tea Milk.",
            ServingsPerPortion = 1,
            ServingSizeDesc = "1 ea",
            ServingPerContainer = "1",
            HshRating = 0,
            ServingUnits = 1,
            SizeWeight = 1,
            Calories = 80,
            CaloriesFat = 20,
            CaloriesSaturatedFat = 5,
            TotalFatWeight = 2.0m,
            TotalFatPercentage = 3,
            SaturatedFatWeight = 6.0m,
            SaturatedFatPercent = 10,
            PolyunsaturatedFat = 7.0m,
            MonounsaturatedFat = 5.0m,
            CholesterolWeight = 6.0m,
            CholesterolPercent = 23,
            SodiumWeight = 130.0m,
            SodiumPercent = 5,
            PotassiumWeight = 117.0m,
            PotassiumPercent = 34,
            TotalCarbohydrateWeight = 16.0m,
            TotalCarbohydratePercent = 5,
            DietaryFiberWeight = 2.0m,
            DietaryFiberPercent = 8,
            SolubleFiber = 6.0m,
            InsolubleFiber = 4.0m,
            Sugar = 14.0m,
            SugarAlcohol = 3.0m,
            OtherCarbohydrates = 9.0m,
            ProteinWeight = 1.0m,
            ProteinPercent = 2,
            VitaminA = 6,
            Betacarotene = 4,
            VitaminC = 3,
            Calcium = 10,
            Iron = 5,
            VitaminD = 2,
            VitaminE = 6,
            Thiamin = 5,
            Riboflavin = 8,
            Niacin = 3,
            VitaminB6 = 2,
            Folate = 3,
            VitaminB12 = 9,
            Biotin = 5,
            PantothenicAcid = 4,
            Phosphorous = 6,
            Iodine = 5,
            Magnesium = 4,
            Zinc = 9,
            Copper = 1,
            Transfat = 2.0m,
            CaloriesFromTransfat = 78,
            Om6Fatty = 3.0m,
            Om3Fatty = 7.0m,
            Starch = 3.0m,
            Chloride = 7,
            Chromium = 8,
            VitaminK = 9,
            Manganese = 4,
            Molybdenum = 3,
            Selenium = 2,
            TransfatWeight = 1.0m,
            InsertDate = DateTime.Parse("2016-11-02 08:50:07.5220153"),
            ModifiedDate = DateTime.Parse("2016-12-01 10:48:02.9383149"),
            AddedSugarsPercent = 7,
            AddedSugarsWeight = 8,
            CalciumWeight = 9,
            IronWeight = 10,
            VitaminDWeight = 11
        };

        public MessageQueueItemModel MessageQueueItemModel
        {
            get
            {
                MessageQueueItemModelBuilder messageQueueItemModelBuilder = new MessageQueueItemModelBuilder(new ItemMapper());
                return messageQueueItemModelBuilder.Build(this.Item, this.Hierarchy, this.Nutrition);
            }
        }
    }
}