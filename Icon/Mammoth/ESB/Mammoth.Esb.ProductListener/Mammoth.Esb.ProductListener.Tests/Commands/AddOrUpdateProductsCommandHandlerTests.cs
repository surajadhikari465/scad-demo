using Dapper;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Esb.ProductListener.Commands;
using Mammoth.Esb.ProductListener.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Mammoth.Esb.ProductListener.Tests.Commands
{
    [TestClass]
    public class AddOrUpdateProductsCommandHandlerTests
    {
        private AddOrUpdateProductsCommandHandler commandHandler;
        private SqlDbProvider dbProvider;
        private int testClassHCID = 8000003;
        private int testSubBrickHCID = 9000004;
        private int testHierarchyNationalClassID;
        private int testHierarchyHierarchyMerchandiseID;

        [TestInitialize]
        public void Initialize()
        {
            dbProvider = new SqlDbProvider();
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            dbProvider.Connection.Open();
            dbProvider.Transaction = dbProvider.Connection.BeginTransaction();

            commandHandler = new AddOrUpdateProductsCommandHandler(dbProvider);

            testHierarchyNationalClassID = dbProvider.Connection.Query<int>(
                "insert into dbo.Hierarchy_NationalClass(FamilyHCID, CategoryHCID, SubcategoryHCID, ClassHCID)" +
                "   output inserted.HierarchyNationalClassID " +
                $"values(8000000, 8000001, 8000002, {testClassHCID})",
                transaction: dbProvider.Transaction)
                .First();
            testHierarchyHierarchyMerchandiseID = dbProvider.Connection.Query<int>(
                "insert into dbo.Hierarchy_Merchandise(SegmentHCID, FamilyHCID, ClassHCID, BrickHCID, SubBrickHCID)" +
                "   output inserted.HierarchyMerchandiseID " +
                $"values(9000000, 9000001, 9000002, 9000003, {testSubBrickHCID})",
                transaction: dbProvider.Transaction)
                .First();
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbProvider.Transaction.Rollback();
            dbProvider.Transaction.Dispose();
            dbProvider.Connection.Dispose();
        }

        [TestMethod]
        public void AddOrUpdateProducts_ProductExist_ShouldUpdateProduct()
        {
            //Given
            var itemId = 20000000;
            InsertItemIntoDatabase(itemId);
            var itemModel = CreateItemModel(itemId);

            //When
            commandHandler.Execute(
                new AddOrUpdateProductsCommand
                {
                    Items = new List<ItemModel>
                    {
                        itemModel
                    }
                });

            //Then
            var item = dbProvider.Connection.Query<dynamic>(
                "SELECT * FROM dbo.Items WHERE ItemID = @ItemId",
                new { ItemId = itemId },
                dbProvider.Transaction)
                .Single();
            Assert.AreEqual(itemModel.GlobalAttributes.ItemTypeID, item.ItemTypeID);
            Assert.AreEqual(testHierarchyHierarchyMerchandiseID, item.HierarchyMerchandiseID);
            Assert.AreEqual(testHierarchyNationalClassID, item.HierarchyNationalClassID);
            Assert.AreEqual(itemModel.GlobalAttributes.BrandHCID, item.BrandHCID);
            Assert.AreEqual(itemModel.GlobalAttributes.TaxClassHCID, item.TaxClassHCID);
            Assert.AreEqual(itemModel.GlobalAttributes.PSNumber, item.PSNumber);
            Assert.AreEqual(itemModel.GlobalAttributes.Desc_Product, item.Desc_Product);
            Assert.AreEqual(itemModel.GlobalAttributes.Desc_POS, item.Desc_POS);
            Assert.AreEqual(itemModel.GlobalAttributes.PackageUnit, item.PackageUnit);
            Assert.AreEqual(itemModel.GlobalAttributes.RetailSize, item.RetailSize);
            Assert.AreEqual(itemModel.GlobalAttributes.RetailUOM, item.RetailUOM);
            Assert.AreEqual(itemModel.GlobalAttributes.Desc_CustomerFriendly, item.Desc_CustomerFriendly);
            Assert.IsNotNull(item.AddedDate);

            var signAttributes = dbProvider.Connection.Query<dynamic>(
                "SELECT * FROM dbo.ItemAttributes_Sign WHERE ItemID = @ItemId",
                new { ItemId = itemId },
                dbProvider.Transaction)
                .Single();

            Assert.AreEqual(itemModel.SignAttributes.CheeseMilkType, signAttributes.CheeseMilkType);
            Assert.AreEqual(itemModel.SignAttributes.Agency_GlutenFree, signAttributes.Agency_GlutenFree);
            Assert.AreEqual(itemModel.SignAttributes.Agency_Kosher, signAttributes.Agency_Kosher);
            Assert.AreEqual(itemModel.SignAttributes.Agency_NonGMO, signAttributes.Agency_NonGMO);
            Assert.AreEqual(itemModel.SignAttributes.Agency_Organic, signAttributes.Agency_Organic);
            Assert.AreEqual(itemModel.SignAttributes.Agency_Vegan, signAttributes.Agency_Vegan);
            Assert.AreEqual(itemModel.SignAttributes.IsAirChilled, signAttributes.IsAirChilled);
            Assert.AreEqual(itemModel.SignAttributes.IsBiodynamic, signAttributes.IsBiodynamic);
            Assert.AreEqual(itemModel.SignAttributes.IsCheeseRaw, signAttributes.IsCheeseRaw);
            Assert.AreEqual(itemModel.SignAttributes.IsDryAged, signAttributes.IsDryAged);
            Assert.AreEqual(itemModel.SignAttributes.IsFreeRange, signAttributes.IsFreeRange);
            Assert.AreEqual(itemModel.SignAttributes.IsGrassFed, signAttributes.IsGrassFed);
            Assert.AreEqual(itemModel.SignAttributes.IsMadeInHouse, signAttributes.IsMadeInHouse);
            Assert.AreEqual(itemModel.SignAttributes.IsMsc, signAttributes.IsMsc);
            Assert.AreEqual(itemModel.SignAttributes.IsPastureRaised, signAttributes.IsPastureRaised);
            Assert.AreEqual(itemModel.SignAttributes.IsPremiumBodyCare, signAttributes.IsPremiumBodyCare);
            Assert.AreEqual(itemModel.SignAttributes.IsVegetarian, signAttributes.IsVegetarian);
            Assert.AreEqual(itemModel.SignAttributes.IsWholeTrade, signAttributes.IsWholeTrade);
            Assert.AreEqual(itemModel.SignAttributes.Rating_AnimalWelfare, signAttributes.Rating_AnimalWelfare);
            Assert.AreEqual(itemModel.SignAttributes.Rating_EcoScale, signAttributes.Rating_EcoScale);
            Assert.AreEqual(itemModel.SignAttributes.Rating_HealthyEating, signAttributes.Rating_HealthyEating);
            Assert.AreEqual(itemModel.SignAttributes.Seafood_FreshOrFrozen, signAttributes.Seafood_FreshOrFrozen);
            Assert.AreEqual(itemModel.SignAttributes.Seafood_CatchType, signAttributes.Seafood_CatchType);

            var nutritionAttributes = dbProvider.Connection.Query<dynamic>(
                "SELECT * FROM dbo.ItemAttributes_Nutrition WHERE ItemID = @ItemId",
                new { ItemId = itemId },
                dbProvider.Transaction)
                .Single();

            Assert.AreEqual(itemModel.NutritionAttributes.RecipeName, nutritionAttributes.RecipeName);
            Assert.AreEqual(itemModel.NutritionAttributes.Allergens, nutritionAttributes.Allergens);
            Assert.AreEqual(itemModel.NutritionAttributes.Ingredients, nutritionAttributes.Ingredients);
            Assert.AreEqual(itemModel.NutritionAttributes.ServingsPerPortion, (float)nutritionAttributes.ServingsPerPortion);
            Assert.AreEqual(itemModel.NutritionAttributes.ServingSizeDesc, nutritionAttributes.ServingSizeDesc);
            Assert.AreEqual(itemModel.NutritionAttributes.ServingPerContainer, nutritionAttributes.ServingPerContainer);
            Assert.AreEqual(itemModel.NutritionAttributes.HshRating, nutritionAttributes.HshRating);
            Assert.AreEqual(itemModel.NutritionAttributes.ServingUnits, nutritionAttributes.ServingUnits);
            Assert.AreEqual(itemModel.NutritionAttributes.SizeWeight, nutritionAttributes.SizeWeight);
            Assert.AreEqual(itemModel.NutritionAttributes.Calories, nutritionAttributes.Calories);
            Assert.AreEqual(itemModel.NutritionAttributes.CaloriesFat, nutritionAttributes.CaloriesFat);
            Assert.AreEqual(itemModel.NutritionAttributes.CaloriesSaturatedFat, nutritionAttributes.CaloriesSaturatedFat);
            Assert.AreEqual(itemModel.NutritionAttributes.TotalFatWeight, nutritionAttributes.TotalFatWeight);
            Assert.AreEqual(itemModel.NutritionAttributes.TotalFatPercentage, nutritionAttributes.TotalFatPercentage);
            Assert.AreEqual(itemModel.NutritionAttributes.SaturatedFatWeight, nutritionAttributes.SaturatedFatWeight);
            Assert.AreEqual(itemModel.NutritionAttributes.SaturatedFatPercent, nutritionAttributes.SaturatedFatPercent);
            Assert.AreEqual(itemModel.NutritionAttributes.PolyunsaturatedFat, nutritionAttributes.PolyunsaturatedFat);
            Assert.AreEqual(itemModel.NutritionAttributes.MonounsaturatedFat, nutritionAttributes.MonounsaturatedFat);
            Assert.AreEqual(itemModel.NutritionAttributes.CholesterolWeight, nutritionAttributes.CholesterolWeight);
            Assert.AreEqual(itemModel.NutritionAttributes.CholesterolPercent, nutritionAttributes.CholesterolPercent);
            Assert.AreEqual(itemModel.NutritionAttributes.SodiumWeight, nutritionAttributes.SodiumWeight);
            Assert.AreEqual(itemModel.NutritionAttributes.SodiumPercent, nutritionAttributes.SodiumPercent);
            Assert.AreEqual(itemModel.NutritionAttributes.PotassiumWeight, nutritionAttributes.PotassiumWeight);
            Assert.AreEqual(itemModel.NutritionAttributes.PotassiumPercent, nutritionAttributes.PotassiumPercent);
            Assert.AreEqual(itemModel.NutritionAttributes.TotalCarbohydrateWeight, nutritionAttributes.TotalCarbohydrateWeight);
            Assert.AreEqual(itemModel.NutritionAttributes.TotalCarbohydratePercent, nutritionAttributes.TotalCarbohydratePercent);
            Assert.AreEqual(itemModel.NutritionAttributes.DietaryFiberWeight, nutritionAttributes.DietaryFiberWeight);
            Assert.AreEqual(itemModel.NutritionAttributes.DietaryFiberPercent, nutritionAttributes.DietaryFiberPercent);
            Assert.AreEqual(itemModel.NutritionAttributes.SolubleFiber, nutritionAttributes.SolubleFiber);
            Assert.AreEqual(itemModel.NutritionAttributes.InsolubleFiber, nutritionAttributes.InsolubleFiber);
            Assert.AreEqual(itemModel.NutritionAttributes.Sugar, nutritionAttributes.Sugar);
            Assert.AreEqual(itemModel.NutritionAttributes.SugarAlcohol, nutritionAttributes.SugarAlcohol);
            Assert.AreEqual(itemModel.NutritionAttributes.OtherCarbohydrates, nutritionAttributes.OtherCarbohydrates);
            Assert.AreEqual(itemModel.NutritionAttributes.ProteinWeight, nutritionAttributes.ProteinWeight);
            Assert.AreEqual(itemModel.NutritionAttributes.ProteinPercent, nutritionAttributes.ProteinPercent);
            Assert.AreEqual(itemModel.NutritionAttributes.VitaminA, nutritionAttributes.VitaminA);
            Assert.AreEqual(itemModel.NutritionAttributes.Betacarotene, nutritionAttributes.Betacarotene);
            Assert.AreEqual(itemModel.NutritionAttributes.VitaminC, nutritionAttributes.VitaminC);
            Assert.AreEqual(itemModel.NutritionAttributes.Calcium, nutritionAttributes.Calcium);
            Assert.AreEqual(itemModel.NutritionAttributes.Iron, nutritionAttributes.Iron);
            Assert.AreEqual(itemModel.NutritionAttributes.VitaminD, nutritionAttributes.VitaminD);
            Assert.AreEqual(itemModel.NutritionAttributes.VitaminE, nutritionAttributes.VitaminE);
            Assert.AreEqual(itemModel.NutritionAttributes.Thiamin, nutritionAttributes.Thiamin);
            Assert.AreEqual(itemModel.NutritionAttributes.Riboflavin, nutritionAttributes.Riboflavin);
            Assert.AreEqual(itemModel.NutritionAttributes.Niacin, nutritionAttributes.Niacin);
            Assert.AreEqual(itemModel.NutritionAttributes.VitaminB6, nutritionAttributes.VitaminB6);
            Assert.AreEqual(itemModel.NutritionAttributes.Folate, nutritionAttributes.Folate);
            Assert.AreEqual(itemModel.NutritionAttributes.VitaminB12, nutritionAttributes.VitaminB12);
            Assert.AreEqual(itemModel.NutritionAttributes.Biotin, nutritionAttributes.Biotin);
            Assert.AreEqual(itemModel.NutritionAttributes.PantothenicAcid, nutritionAttributes.PantothenicAcid);
            Assert.AreEqual(itemModel.NutritionAttributes.Phosphorous, nutritionAttributes.Phosphorous);
            Assert.AreEqual(itemModel.NutritionAttributes.Iodine, nutritionAttributes.Iodine);
            Assert.AreEqual(itemModel.NutritionAttributes.Magnesium, nutritionAttributes.Magnesium);
            Assert.AreEqual(itemModel.NutritionAttributes.Zinc, nutritionAttributes.Zinc);
            Assert.AreEqual(itemModel.NutritionAttributes.Copper, nutritionAttributes.Copper);
            Assert.AreEqual(itemModel.NutritionAttributes.TransFat, nutritionAttributes.TransFat);
            Assert.AreEqual(itemModel.NutritionAttributes.TransFatWeight, nutritionAttributes.TransFatWeight);
            Assert.AreEqual(itemModel.NutritionAttributes.CaloriesFromTransFat, nutritionAttributes.CaloriesFromTransFat);
            Assert.AreEqual(itemModel.NutritionAttributes.Om6Fatty, nutritionAttributes.Om6Fatty);
            Assert.AreEqual(itemModel.NutritionAttributes.Om3Fatty, nutritionAttributes.Om3Fatty);
            Assert.AreEqual(itemModel.NutritionAttributes.Starch, nutritionAttributes.Starch);
            Assert.AreEqual(itemModel.NutritionAttributes.Chloride, nutritionAttributes.Chloride);
            Assert.AreEqual(itemModel.NutritionAttributes.Chromium, nutritionAttributes.Chromium);
            Assert.AreEqual(itemModel.NutritionAttributes.VitaminK, nutritionAttributes.VitaminK);
            Assert.AreEqual(itemModel.NutritionAttributes.Manganese, nutritionAttributes.Manganese);
            Assert.AreEqual(itemModel.NutritionAttributes.Molybdenum, nutritionAttributes.Molybdenum);
            Assert.AreEqual(itemModel.NutritionAttributes.Selenium, nutritionAttributes.Selenium);

            var extAttributes = dbProvider.Connection.Query<dynamic>(
                "SELECT * FROM dbo.ItemAttributes_Ext WHERE ItemID = @ItemId",
                new { ItemId = itemId },
                dbProvider.Transaction)
                .ToList();
            Assert.AreEqual(10, extAttributes.Count);
            Assert.AreEqual(itemModel.ExtendedAttributes.FairTrade, extAttributes.Single(ea => ea.AttributeID == Attributes.FairTrade).AttributeValue);
            Assert.AreEqual(itemModel.ExtendedAttributes.FlexibleText, extAttributes.Single(ea => ea.AttributeID == Attributes.FlexibleText).AttributeValue);
            Assert.AreEqual(itemModel.ExtendedAttributes.MadeWithBiodynamicGrapes, extAttributes.Single(ea => ea.AttributeID == Attributes.MadeWithBiodynamicGrapes).AttributeValue);
            Assert.AreEqual(itemModel.ExtendedAttributes.MadeWithOrganicGrapes, extAttributes.Single(ea => ea.AttributeID == Attributes.MadeWithOrganicGrapes).AttributeValue);
            Assert.AreEqual(itemModel.ExtendedAttributes.NutritionRequired, extAttributes.Single(ea => ea.AttributeID == Attributes.NutritionRequired).AttributeValue);
            Assert.AreEqual(itemModel.ExtendedAttributes.PrimeBeef, extAttributes.Single(ea => ea.AttributeID == Attributes.PrimeBeef).AttributeValue);
            Assert.AreEqual(itemModel.ExtendedAttributes.RainforestAlliance, extAttributes.Single(ea => ea.AttributeID == Attributes.RainforestAlliance).AttributeValue);
            Assert.AreEqual(itemModel.ExtendedAttributes.RefrigeratedOrShelfStable, extAttributes.Single(ea => ea.AttributeID == Attributes.RefrigeratedOrShelfStable).AttributeValue);
            Assert.AreEqual(itemModel.ExtendedAttributes.SmithsonianBirdFriendly, extAttributes.Single(ea => ea.AttributeID == Attributes.SmithsonianBirdFriendly).AttributeValue);
            Assert.AreEqual(itemModel.ExtendedAttributes.Wic, extAttributes.Single(ea => ea.AttributeID == Attributes.Wic).AttributeValue);
        }

        [TestMethod]
        public void AddOrUpdateProducts_ProductDoesntExist_ShouldAddProduct()
        {
            var itemId = 20000000;
            var itemModel = CreateItemModel(itemId);

            //When
            commandHandler.Execute(
                new AddOrUpdateProductsCommand
                {
                    Items = new List<ItemModel>
                    {
                        itemModel
                    }
                });

            //Then
            var item = dbProvider.Connection.Query<dynamic>(
                "SELECT * FROM dbo.Items WHERE ItemID = @ItemId",
                new { ItemId = itemId },
                dbProvider.Transaction)
                .Single();
            Assert.AreEqual(itemModel.GlobalAttributes.ItemTypeID, item.ItemTypeID);
            Assert.AreEqual(testHierarchyHierarchyMerchandiseID, item.HierarchyMerchandiseID);
            Assert.AreEqual(testHierarchyNationalClassID, item.HierarchyNationalClassID);
            Assert.AreEqual(itemModel.GlobalAttributes.BrandHCID, item.BrandHCID);
            Assert.AreEqual(itemModel.GlobalAttributes.TaxClassHCID, item.TaxClassHCID);
            Assert.AreEqual(itemModel.GlobalAttributes.PSNumber, item.PSNumber);
            Assert.AreEqual(itemModel.GlobalAttributes.Desc_Product, item.Desc_Product);
            Assert.AreEqual(itemModel.GlobalAttributes.Desc_POS, item.Desc_POS);
            Assert.AreEqual(itemModel.GlobalAttributes.PackageUnit, item.PackageUnit);
            Assert.AreEqual(itemModel.GlobalAttributes.RetailSize, item.RetailSize);
            Assert.AreEqual(itemModel.GlobalAttributes.RetailUOM, item.RetailUOM);
            Assert.AreEqual(itemModel.GlobalAttributes.Desc_CustomerFriendly, item.Desc_CustomerFriendly);
            Assert.IsNotNull(item.AddedDate);

            var signAttributes = dbProvider.Connection.Query<dynamic>(
                "SELECT * FROM dbo.ItemAttributes_Sign WHERE ItemID = @ItemId",
                new { ItemId = itemId },
                dbProvider.Transaction)
                .Single();

            Assert.AreEqual(itemModel.SignAttributes.CheeseMilkType, signAttributes.CheeseMilkType);
            Assert.AreEqual(itemModel.SignAttributes.Agency_GlutenFree, signAttributes.Agency_GlutenFree);
            Assert.AreEqual(itemModel.SignAttributes.Agency_Kosher, signAttributes.Agency_Kosher);
            Assert.AreEqual(itemModel.SignAttributes.Agency_NonGMO, signAttributes.Agency_NonGMO);
            Assert.AreEqual(itemModel.SignAttributes.Agency_Organic, signAttributes.Agency_Organic);
            Assert.AreEqual(itemModel.SignAttributes.Agency_Vegan, signAttributes.Agency_Vegan);
            Assert.AreEqual(itemModel.SignAttributes.IsAirChilled, signAttributes.IsAirChilled);
            Assert.AreEqual(itemModel.SignAttributes.IsBiodynamic, signAttributes.IsBiodynamic);
            Assert.AreEqual(itemModel.SignAttributes.IsCheeseRaw, signAttributes.IsCheeseRaw);
            Assert.AreEqual(itemModel.SignAttributes.IsDryAged, signAttributes.IsDryAged);
            Assert.AreEqual(itemModel.SignAttributes.IsFreeRange, signAttributes.IsFreeRange);
            Assert.AreEqual(itemModel.SignAttributes.IsGrassFed, signAttributes.IsGrassFed);
            Assert.AreEqual(itemModel.SignAttributes.IsMadeInHouse, signAttributes.IsMadeInHouse);
            Assert.AreEqual(itemModel.SignAttributes.IsMsc, signAttributes.IsMsc);
            Assert.AreEqual(itemModel.SignAttributes.IsPastureRaised, signAttributes.IsPastureRaised);
            Assert.AreEqual(itemModel.SignAttributes.IsPremiumBodyCare, signAttributes.IsPremiumBodyCare);
            Assert.AreEqual(itemModel.SignAttributes.IsVegetarian, signAttributes.IsVegetarian);
            Assert.AreEqual(itemModel.SignAttributes.IsWholeTrade, signAttributes.IsWholeTrade);
            Assert.AreEqual(itemModel.SignAttributes.Rating_AnimalWelfare, signAttributes.Rating_AnimalWelfare);
            Assert.AreEqual(itemModel.SignAttributes.Rating_EcoScale, signAttributes.Rating_EcoScale);
            Assert.AreEqual(itemModel.SignAttributes.Rating_HealthyEating, signAttributes.Rating_HealthyEating);
            Assert.AreEqual(itemModel.SignAttributes.Seafood_FreshOrFrozen, signAttributes.Seafood_FreshOrFrozen);
            Assert.AreEqual(itemModel.SignAttributes.Seafood_CatchType, signAttributes.Seafood_CatchType);

            var nutritionAttributes = dbProvider.Connection.Query<dynamic>(
                "SELECT * FROM dbo.ItemAttributes_Nutrition WHERE ItemID = @ItemId",
                new { ItemId = itemId },
                dbProvider.Transaction)
                .Single();

            Assert.AreEqual(itemModel.NutritionAttributes.RecipeName, nutritionAttributes.RecipeName);
            Assert.AreEqual(itemModel.NutritionAttributes.Allergens, nutritionAttributes.Allergens);
            Assert.AreEqual(itemModel.NutritionAttributes.Ingredients, nutritionAttributes.Ingredients);
            Assert.AreEqual(itemModel.NutritionAttributes.ServingsPerPortion, (float)nutritionAttributes.ServingsPerPortion);
            Assert.AreEqual(itemModel.NutritionAttributes.ServingSizeDesc, nutritionAttributes.ServingSizeDesc);
            Assert.AreEqual(itemModel.NutritionAttributes.ServingPerContainer, nutritionAttributes.ServingPerContainer);
            Assert.AreEqual(itemModel.NutritionAttributes.HshRating, nutritionAttributes.HshRating);
            Assert.AreEqual(itemModel.NutritionAttributes.ServingUnits, nutritionAttributes.ServingUnits);
            Assert.AreEqual(itemModel.NutritionAttributes.SizeWeight, nutritionAttributes.SizeWeight);
            Assert.AreEqual(itemModel.NutritionAttributes.Calories, nutritionAttributes.Calories);
            Assert.AreEqual(itemModel.NutritionAttributes.CaloriesFat, nutritionAttributes.CaloriesFat);
            Assert.AreEqual(itemModel.NutritionAttributes.CaloriesSaturatedFat, nutritionAttributes.CaloriesSaturatedFat);
            Assert.AreEqual(itemModel.NutritionAttributes.TotalFatWeight, nutritionAttributes.TotalFatWeight);
            Assert.AreEqual(itemModel.NutritionAttributes.TotalFatPercentage, nutritionAttributes.TotalFatPercentage);
            Assert.AreEqual(itemModel.NutritionAttributes.SaturatedFatWeight, nutritionAttributes.SaturatedFatWeight);
            Assert.AreEqual(itemModel.NutritionAttributes.SaturatedFatPercent, nutritionAttributes.SaturatedFatPercent);
            Assert.AreEqual(itemModel.NutritionAttributes.PolyunsaturatedFat, nutritionAttributes.PolyunsaturatedFat);
            Assert.AreEqual(itemModel.NutritionAttributes.MonounsaturatedFat, nutritionAttributes.MonounsaturatedFat);
            Assert.AreEqual(itemModel.NutritionAttributes.CholesterolWeight, nutritionAttributes.CholesterolWeight);
            Assert.AreEqual(itemModel.NutritionAttributes.CholesterolPercent, nutritionAttributes.CholesterolPercent);
            Assert.AreEqual(itemModel.NutritionAttributes.SodiumWeight, nutritionAttributes.SodiumWeight);
            Assert.AreEqual(itemModel.NutritionAttributes.SodiumPercent, nutritionAttributes.SodiumPercent);
            Assert.AreEqual(itemModel.NutritionAttributes.PotassiumWeight, nutritionAttributes.PotassiumWeight);
            Assert.AreEqual(itemModel.NutritionAttributes.PotassiumPercent, nutritionAttributes.PotassiumPercent);
            Assert.AreEqual(itemModel.NutritionAttributes.TotalCarbohydrateWeight, nutritionAttributes.TotalCarbohydrateWeight);
            Assert.AreEqual(itemModel.NutritionAttributes.TotalCarbohydratePercent, nutritionAttributes.TotalCarbohydratePercent);
            Assert.AreEqual(itemModel.NutritionAttributes.DietaryFiberWeight, nutritionAttributes.DietaryFiberWeight);
            Assert.AreEqual(itemModel.NutritionAttributes.DietaryFiberPercent, nutritionAttributes.DietaryFiberPercent);
            Assert.AreEqual(itemModel.NutritionAttributes.SolubleFiber, nutritionAttributes.SolubleFiber);
            Assert.AreEqual(itemModel.NutritionAttributes.InsolubleFiber, nutritionAttributes.InsolubleFiber);
            Assert.AreEqual(itemModel.NutritionAttributes.Sugar, nutritionAttributes.Sugar);
            Assert.AreEqual(itemModel.NutritionAttributes.SugarAlcohol, nutritionAttributes.SugarAlcohol);
            Assert.AreEqual(itemModel.NutritionAttributes.OtherCarbohydrates, nutritionAttributes.OtherCarbohydrates);
            Assert.AreEqual(itemModel.NutritionAttributes.ProteinWeight, nutritionAttributes.ProteinWeight);
            Assert.AreEqual(itemModel.NutritionAttributes.ProteinPercent, nutritionAttributes.ProteinPercent);
            Assert.AreEqual(itemModel.NutritionAttributes.VitaminA, nutritionAttributes.VitaminA);
            Assert.AreEqual(itemModel.NutritionAttributes.Betacarotene, nutritionAttributes.Betacarotene);
            Assert.AreEqual(itemModel.NutritionAttributes.VitaminC, nutritionAttributes.VitaminC);
            Assert.AreEqual(itemModel.NutritionAttributes.Calcium, nutritionAttributes.Calcium);
            Assert.AreEqual(itemModel.NutritionAttributes.Iron, nutritionAttributes.Iron);
            Assert.AreEqual(itemModel.NutritionAttributes.VitaminD, nutritionAttributes.VitaminD);
            Assert.AreEqual(itemModel.NutritionAttributes.VitaminE, nutritionAttributes.VitaminE);
            Assert.AreEqual(itemModel.NutritionAttributes.Thiamin, nutritionAttributes.Thiamin);
            Assert.AreEqual(itemModel.NutritionAttributes.Riboflavin, nutritionAttributes.Riboflavin);
            Assert.AreEqual(itemModel.NutritionAttributes.Niacin, nutritionAttributes.Niacin);
            Assert.AreEqual(itemModel.NutritionAttributes.VitaminB6, nutritionAttributes.VitaminB6);
            Assert.AreEqual(itemModel.NutritionAttributes.Folate, nutritionAttributes.Folate);
            Assert.AreEqual(itemModel.NutritionAttributes.VitaminB12, nutritionAttributes.VitaminB12);
            Assert.AreEqual(itemModel.NutritionAttributes.Biotin, nutritionAttributes.Biotin);
            Assert.AreEqual(itemModel.NutritionAttributes.PantothenicAcid, nutritionAttributes.PantothenicAcid);
            Assert.AreEqual(itemModel.NutritionAttributes.Phosphorous, nutritionAttributes.Phosphorous);
            Assert.AreEqual(itemModel.NutritionAttributes.Iodine, nutritionAttributes.Iodine);
            Assert.AreEqual(itemModel.NutritionAttributes.Magnesium, nutritionAttributes.Magnesium);
            Assert.AreEqual(itemModel.NutritionAttributes.Zinc, nutritionAttributes.Zinc);
            Assert.AreEqual(itemModel.NutritionAttributes.Copper, nutritionAttributes.Copper);
            Assert.AreEqual(itemModel.NutritionAttributes.TransFat, nutritionAttributes.TransFat);
            Assert.AreEqual(itemModel.NutritionAttributes.TransFatWeight, nutritionAttributes.TransFatWeight);
            Assert.AreEqual(itemModel.NutritionAttributes.CaloriesFromTransFat, nutritionAttributes.CaloriesFromTransFat);
            Assert.AreEqual(itemModel.NutritionAttributes.Om6Fatty, nutritionAttributes.Om6Fatty);
            Assert.AreEqual(itemModel.NutritionAttributes.Om3Fatty, nutritionAttributes.Om3Fatty);
            Assert.AreEqual(itemModel.NutritionAttributes.Starch, nutritionAttributes.Starch);
            Assert.AreEqual(itemModel.NutritionAttributes.Chloride, nutritionAttributes.Chloride);
            Assert.AreEqual(itemModel.NutritionAttributes.Chromium, nutritionAttributes.Chromium);
            Assert.AreEqual(itemModel.NutritionAttributes.VitaminK, nutritionAttributes.VitaminK);
            Assert.AreEqual(itemModel.NutritionAttributes.Manganese, nutritionAttributes.Manganese);
            Assert.AreEqual(itemModel.NutritionAttributes.Molybdenum, nutritionAttributes.Molybdenum);
            Assert.AreEqual(itemModel.NutritionAttributes.Selenium, nutritionAttributes.Selenium);

            var extAttributes = dbProvider.Connection.Query<dynamic>(
                "SELECT * FROM dbo.ItemAttributes_Ext WHERE ItemID = @ItemId",
                new { ItemId = itemId },
                dbProvider.Transaction)
                .ToList();
            Assert.AreEqual(10, extAttributes.Count);
            Assert.AreEqual(itemModel.ExtendedAttributes.FairTrade, extAttributes.Single(ea => ea.AttributeID == Attributes.FairTrade).AttributeValue);
            Assert.AreEqual(itemModel.ExtendedAttributes.FlexibleText, extAttributes.Single(ea => ea.AttributeID == Attributes.FlexibleText).AttributeValue);
            Assert.AreEqual(itemModel.ExtendedAttributes.MadeWithBiodynamicGrapes, extAttributes.Single(ea => ea.AttributeID == Attributes.MadeWithBiodynamicGrapes).AttributeValue);
            Assert.AreEqual(itemModel.ExtendedAttributes.MadeWithOrganicGrapes, extAttributes.Single(ea => ea.AttributeID == Attributes.MadeWithOrganicGrapes).AttributeValue);
            Assert.AreEqual(itemModel.ExtendedAttributes.NutritionRequired, extAttributes.Single(ea => ea.AttributeID == Attributes.NutritionRequired).AttributeValue);
            Assert.AreEqual(itemModel.ExtendedAttributes.PrimeBeef, extAttributes.Single(ea => ea.AttributeID == Attributes.PrimeBeef).AttributeValue);
            Assert.AreEqual(itemModel.ExtendedAttributes.RainforestAlliance, extAttributes.Single(ea => ea.AttributeID == Attributes.RainforestAlliance).AttributeValue);
            Assert.AreEqual(itemModel.ExtendedAttributes.RefrigeratedOrShelfStable, extAttributes.Single(ea => ea.AttributeID == Attributes.RefrigeratedOrShelfStable).AttributeValue);
            Assert.AreEqual(itemModel.ExtendedAttributes.SmithsonianBirdFriendly, extAttributes.Single(ea => ea.AttributeID == Attributes.SmithsonianBirdFriendly).AttributeValue);
            Assert.AreEqual(itemModel.ExtendedAttributes.Wic, extAttributes.Single(ea => ea.AttributeID == Attributes.Wic).AttributeValue);
        }

        [TestMethod]
        public void AddOrUpdateProducts_ExtendedAttributesAreNull_DeleteExistingExtendedAttributes()
        {
            //Given
            var itemId = 20000000;
            InsertItemIntoDatabase(itemId);
            var itemModel = CreateItemModel(itemId);
            itemModel.ExtendedAttributes = new ExtendedAttributesModel();

            //When
            commandHandler.Execute(
                new AddOrUpdateProductsCommand
                {
                    Items = new List<ItemModel>
                    {
                        itemModel
                    }
                });

            //Then
            var extAttributes = dbProvider.Connection.Query<dynamic>(
                "SELECT * FROM dbo.ItemAttributes_Ext WHERE ItemID = @ItemId",
                new { ItemId = itemId },
                dbProvider.Transaction)
                .ToList();
            Assert.AreEqual(0, extAttributes.Count);
        }

        private ItemModel CreateItemModel(int itemId)
        {
            return new ItemModel
            {
                GlobalAttributes = new GlobalAttributesModel
                {
                    ItemID = itemId,
                    ItemTypeID = 2,
                    ScanCode = "12345",
                    SubBrickID = testSubBrickHCID,
                    NationalClassID = testClassHCID,
                    BrandHCID = 3,
                    TaxClassHCID = 4,
                    PSNumber = 5,
                    Desc_Product = "Test Desc Updated",
                    Desc_POS = "Test POS Desc Updated",
                    PackageUnit = "2",
                    RetailSize = "2",
                    RetailUOM = "OZ",
                    FoodStampEligible = true,
                    Desc_CustomerFriendly = "Test CustomerFriendlyDescription Updated"
                },
                SignAttributes = new SignAttributesModel
                {
                    ItemID = itemId,
                    IsAirChilled = false,
                    Rating_AnimalWelfare = "Test AnimalWelfareRating",
                    IsBiodynamic = false,
                    CheeseMilkType = "Test CheeseMilkType",
                    IsCheeseRaw = false,
                    IsDryAged = false,
                    Rating_EcoScale = "Test EcoScaleRating",
                    IsFreeRange = false,
                    Agency_GlutenFree = "Test GlutenFreeAgency",
                    IsGrassFed = false,
                    Rating_HealthyEating = "Test HealthyEatingRating",
                    Agency_Kosher = "Test KosherAgency",
                    IsMadeInHouse = false,
                    IsMsc = false,
                    Agency_NonGMO = "Test NonGmoAgency",
                    Agency_Organic = "Test OrganicAgency",
                    IsPastureRaised = false,
                    IsPremiumBodyCare = false,
                    Seafood_CatchType = "Test SeafoodCatchType",
                    Seafood_FreshOrFrozen = "Test SeafoodFreshOrFrozen",
                    Agency_Vegan = "Test VeganAgency",
                    IsVegetarian = false,
                    IsWholeTrade = false
                },
                NutritionAttributes = new NutritionAttributesModel
                {
                    ItemID = itemId,
                    Allergens = "Test Allergens",
                    Betacarotene = 1,
                    Biotin = 2,
                    Calcium = 3,
                    Calories = 4,
                    CaloriesFat = 5,
                    CaloriesFromTransFat = 6,
                    CaloriesSaturatedFat = 7,
                    Chloride = 8,
                    CholesterolPercent = 9,
                    CholesterolWeight = 10,
                    Chromium = 11,
                    Copper = 12,
                    DietaryFiberPercent = 13,
                    DietaryFiberWeight = 14,
                    Folate = 15,
                    HshRating = 16,
                    Ingredients = "Test Ingredients",
                    InsolubleFiber = 17,
                    Iodine = 18,
                    Iron = 19,
                    Magnesium = 20,
                    Manganese = 21,
                    Molybdenum = 22,
                    MonounsaturatedFat = 23,
                    Niacin = 24,
                    Om3Fatty = 25,
                    Om6Fatty = 26,
                    OtherCarbohydrates = 27,
                    PantothenicAcid = 28,
                    Phosphorous = 29,
                    PolyunsaturatedFat = 30,
                    PotassiumPercent = 31,
                    PotassiumWeight = 32,
                    ProteinPercent = 33,
                    ProteinWeight = 34,
                    RecipeName = "Recipe Name",
                    Riboflavin = 35,
                    SaturatedFatPercent = 36,
                    SaturatedFatWeight = 37,
                    Selenium = 38,
                    ServingPerContainer = "Test ServingPerContainer",
                    ServingSizeDesc = "Test ServingSizeDesc",
                    ServingsPerPortion = 39,
                    ServingUnits = 40,
                    SizeWeight = 41,
                    SodiumPercent = 42,
                    SodiumWeight = 43,
                    SolubleFiber = 44,
                    Starch = 45,
                    Sugar = 46,
                    SugarAlcohol = 47,
                    Thiamin = 48,
                    TotalCarbohydratePercent = 49,
                    TotalCarbohydrateWeight = 50,
                    TotalFatPercentage = 51,
                    TotalFatWeight = 52,
                    TransFat = 53,
                    TransFatWeight = 54,
                    VitaminA = 55,
                    VitaminB12 = 56,
                    VitaminB6 = 57,
                    VitaminC = 58,
                    VitaminD = 59,
                    VitaminE = 60,
                    VitaminK = 61,
                    Zinc = 62
                },
                ExtendedAttributes = new ExtendedAttributesModel
                {
                    ItemId = itemId,
                    FairTrade = "Test FairTrade",
                    FlexibleText = "Test FlexibleText",
                    MadeWithBiodynamicGrapes = "Test MadeWithBiodynamicGrapes",
                    MadeWithOrganicGrapes = "Test MadeWithOrganicGrapes",
                    NutritionRequired = "Test NutritionRequired",
                    PrimeBeef = "Test PrimeBeef",
                    RainforestAlliance = "Test RainforestAlliance",
                    RefrigeratedOrShelfStable = "Test RefrigeratedOrShelfStable",
                    SmithsonianBirdFriendly = "Test SmithsonianBirdFriendly",
                    Wic = "Test Wic"
                }
            };
        }

        private void InsertItemIntoDatabase(int itemId)
        {
            dbProvider.Connection.Execute(
                            $@"
                            DECLARE @itemId INT = {itemId}
                            INSERT INTO dbo.Items(
                                ItemID, 
                                ItemTypeID, 
                                ScanCode, 
                                HierarchyMerchandiseID, 
                                HierarchyNationalClassID, 
                                BrandHCID, 
                                TaxClassHCID, 
                                PSNumber, 
                                Desc_Product, 
                                Desc_POS, 
                                PackageUnit, 
                                RetailSize, 
                                RetailUOM, 
                                FoodStampEligible,
                                Desc_CustomerFriendly)
                            VALUES (@itemId, 1, '1234', 1, 1, 1, 1, 1, 'Test Desc', 'Test POS Desc', '1', '1', 'EA', 0, 'CustomerFriendlyDescription')

                            INSERT INTO dbo.ItemAttributes_Sign(
                                ItemID,               
                                CheeseMilkType,       
                                Agency_GlutenFree,    
                                Agency_Kosher,        
                                Agency_NonGMO,        
                                Agency_Organic,       
                                Agency_Vegan,         
                                IsAirChilled,         
                                IsBiodynamic,         
                                IsCheeseRaw,          
                                IsDryAged,            
                                IsFreeRange,          
                                IsGrassFed,           
                                IsMadeInHouse,        
                                IsMsc,                
                                IsPastureRaised,      
                                IsPremiumBodyCare,    
                                IsVegetarian,         
                                IsWholeTrade,         
                                Rating_AnimalWelfare, 
                                Rating_EcoScale,      
                                Rating_HealthyEating, 
                                Seafood_FreshOrFrozen,
                                Seafood_CatchType,    
                                AddedDate,            
                                ModifiedDate)
                            values(
                                @itemID,
                                'Cheese',
                                'GlutenFree',
                                'Kosher',
                                'NonGMO',
                                'Organic',
                                'Vegan',
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                'AnimalWelfare',
                                'EcoScale',
                                'HealthyEating',
                                'FreshOrFrozen',
                                'CatchType',
                                GETDATE(),
                                null)

                            INSERT INTO dbo.ItemAttributes_Nutrition(
                                ItemID,
		                        RecipeName,
		                        Allergens,
		                        Ingredients,
		                        ServingsPerPortion,
		                        ServingSizeDesc,
		                        ServingPerContainer,
		                        HshRating,
		                        ServingUnits,
		                        SizeWeight,
		                        Calories,
		                        CaloriesFat,
		                        CaloriesSaturatedFat,
		                        TotalFatWeight,
		                        TotalFatPercentage,
		                        SaturatedFatWeight,
		                        SaturatedFatPercent,
		                        PolyunsaturatedFat,
		                        MonounsaturatedFat,
		                        CholesterolWeight,
		                        CholesterolPercent,
		                        SodiumWeight,
		                        SodiumPercent,
		                        PotassiumWeight,
		                        PotassiumPercent,
		                        TotalCarbohydrateWeight,
		                        TotalCarbohydratePercent,
		                        DietaryFiberWeight,
		                        DietaryFiberPercent,
		                        SolubleFiber,
		                        InsolubleFiber,
		                        Sugar,
		                        SugarAlcohol,
		                        OtherCarbohydrates,
		                        ProteinWeight,
		                        ProteinPercent,
		                        VitaminA,
		                        Betacarotene,
		                        VitaminC,
		                        Calcium,
		                        Iron,
		                        VitaminD,
		                        VitaminE,
		                        Thiamin,
		                        Riboflavin,
		                        Niacin,
		                        VitaminB6,
		                        Folate,
		                        VitaminB12,
		                        Biotin,
		                        PantothenicAcid,
		                        Phosphorous,
		                        Iodine,
		                        Magnesium,
		                        Zinc,
		                        Copper,
		                        TransFat,
		                        CaloriesFromTransFat,
		                        Om6Fatty,
		                        Om3Fatty,
		                        Starch,
		                        Chloride,
		                        Chromium,
		                        VitaminK,
		                        Manganese,
		                        Molybdenum,
		                        Selenium,
		                        TransFatWeight)
                            VALUES(
                                @itemID,
                                'RecipeName',
		                        'Allergens',
		                        'Ingredients',
		                        0,
		                        '',
		                        '',
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0,
		                        0)

                            INSERT INTO dbo.ItemAttributes_Ext(ItemID, AttributeID, AttributeValue, AddedDate)
                            VALUES  (@itemID, {Attributes.FairTrade}, 'Test', GETDATE()),
                                    (@itemID, {Attributes.FlexibleText}, 'Test', GETDATE()),
                                    (@itemID, {Attributes.MadeWithBiodynamicGrapes}, 'Test', GETDATE()),
                                    (@itemID, {Attributes.MadeWithOrganicGrapes}, 'Test', GETDATE()),
                                    (@itemID, {Attributes.NutritionRequired}, 'Test', GETDATE()),
                                    (@itemID, {Attributes.PrimeBeef}, 'Test', GETDATE()),
                                    (@itemID, {Attributes.RainforestAlliance}, 'Test', GETDATE()),
                                    (@itemID, {Attributes.RefrigeratedOrShelfStable}, 'Test', GETDATE()),
                                    (@itemID, {Attributes.SmithsonianBirdFriendly}, 'Test', GETDATE()),
                                    (@itemID, {Attributes.Wic}, 'Test', GETDATE())",
                            transaction: dbProvider.Transaction);
        }
    }
}
