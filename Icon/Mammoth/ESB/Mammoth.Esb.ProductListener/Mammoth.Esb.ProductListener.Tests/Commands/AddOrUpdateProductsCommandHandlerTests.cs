using Dapper;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Esb.ProductListener.Commands;
using Mammoth.Esb.ProductListener.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            InsertNationalAndMerchandiseHierarchiesForTest();
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
            InsertSimulatedPreExistingItemIntoDatabase(itemId);
            var itemModel = CreateItemModelForAttributeTest(itemId);
            var commandData = new AddOrUpdateProductsCommand
            {
                Items = new List<ItemModel>
                    {
                        itemModel
                    }
            };

            //When
            commandHandler.Execute(commandData);

            //Then
            var globalAttributes = ReadItemAttributesDynamic(itemId);
            AssertGlobalAttributesAsExpected(itemModel.GlobalAttributes, globalAttributes);

            var signAttributes = ReadSignAttributesDynamic(itemId);
            AssertSignAttributesAsExpected(itemModel.SignAttributes, signAttributes);

            var nutritionAttributes = ReadNutritionAttributesDynamic(itemId);
            AssertNutritionAttributesAsExpected(itemModel.NutritionAttributes, nutritionAttributes);

            var extAttributes = ReadExtendedAttributesDynamic(itemId);
            AssertExtendedAttributesAsExpected(itemModel.ExtendedAttributes, extAttributes);
        }
        
        [TestMethod]
        public void AddOrUpdateProducts_ProductDoesntExist_ShouldAddProduct()
        {
            //Given
            var itemId = 20000000;
            var itemModel = CreateItemModelForAttributeTest(itemId);
            var commandData = new AddOrUpdateProductsCommand
            {
                Items = new List<ItemModel>
                    {
                        itemModel
                    }
            };

            //When
            commandHandler.Execute(commandData);

            //Then
            var globalAttributes = ReadItemAttributesDynamic(itemId);
            AssertGlobalAttributesAsExpected(itemModel.GlobalAttributes, globalAttributes);

            var signAttributes = ReadSignAttributesDynamic(itemId);
            AssertSignAttributesAsExpected(itemModel.SignAttributes, signAttributes);

            var nutritionAttributes = ReadNutritionAttributesDynamic(itemId);
            AssertNutritionAttributesAsExpected(itemModel.NutritionAttributes, nutritionAttributes);

            var extAttributes = ReadExtendedAttributesDynamic(itemId);
            AssertExtendedAttributesAsExpected(itemModel.ExtendedAttributes, extAttributes);
        }

        [TestMethod]
        public void AddOrUpdateProducts_ExtendedAttributesAreNull_DeleteExistingExtendedAttributes()
        {
            //Given
            var itemId = 20000000;
            InsertSimulatedPreExistingItemIntoDatabase(itemId);
            var itemModel = CreateItemModelForAttributeTest(itemId);
            itemModel.ExtendedAttributes = new ExtendedAttributesModel { ItemId = 20000000 };
            var commandData = new AddOrUpdateProductsCommand
            {
                Items = new List<ItemModel>
                    {
                        itemModel
                    }
            };

            //When
            commandHandler.Execute(commandData);

            //Then
            var extAttributes = ReadExtendedAttributesDynamic(itemId);
            Assert.AreEqual(0, extAttributes.Count);
        }

        [TestMethod]
        public void AddOrUpdateProducts_ProductHasNullAttributes_ShouldSetNullForAttributes()
        {
            //Given
            var itemId = 20000000;
            var itemModel = CreateItemModelForAttributeTest(itemId);
            itemModel.SignAttributes = new SignAttributesModel { ItemID = itemId };
            itemModel.NutritionAttributes = new NutritionAttributesModel { ItemID = itemId };
            itemModel.ExtendedAttributes = new ExtendedAttributesModel { ItemId = itemId };

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

            Assert.IsNull(signAttributes.CheeseMilkType);
            Assert.IsNull(signAttributes.Agency_GlutenFree);
            Assert.IsNull(signAttributes.Agency_Kosher);
            Assert.IsNull(signAttributes.Agency_NonGMO);
            Assert.IsNull(signAttributes.Agency_Organic);
            Assert.IsNull(signAttributes.Agency_Vegan);
            Assert.IsNull(signAttributes.IsAirChilled);
            Assert.IsNull(signAttributes.IsBiodynamic);
            Assert.IsNull(signAttributes.IsCheeseRaw);
            Assert.IsNull(signAttributes.IsDryAged);
            Assert.IsNull(signAttributes.IsFreeRange);
            Assert.IsNull(signAttributes.IsGrassFed);
            Assert.IsNull(signAttributes.IsMadeInHouse);
            Assert.IsNull(signAttributes.IsMsc);
            Assert.IsNull(signAttributes.IsPastureRaised);
            Assert.IsNull(signAttributes.IsPremiumBodyCare);
            Assert.IsNull(signAttributes.IsVegetarian);
            Assert.IsNull(signAttributes.IsWholeTrade);
            Assert.IsNull(signAttributes.Rating_AnimalWelfare);
            Assert.IsNull(signAttributes.Rating_EcoScale);
            Assert.IsNull(signAttributes.Rating_HealthyEating);
            Assert.IsNull(signAttributes.Seafood_FreshOrFrozen);
            Assert.IsNull(signAttributes.Seafood_CatchType);

            var nutritionAttributes = dbProvider.Connection.Query<dynamic>(
                "SELECT * FROM dbo.ItemAttributes_Nutrition WHERE ItemID = @ItemId",
                new { ItemId = itemId },
                dbProvider.Transaction)
                .Single();

            Assert.IsNull(nutritionAttributes.RecipeName);
            Assert.IsNull(nutritionAttributes.Allergens);
            Assert.IsNull(nutritionAttributes.Ingredients);
            Assert.IsNull(nutritionAttributes.ServingsPerPortion);
            Assert.IsNull(nutritionAttributes.ServingSizeDesc);
            Assert.IsNull(nutritionAttributes.ServingPerContainer);
            Assert.IsNull(nutritionAttributes.HshRating);
            Assert.IsNull(nutritionAttributes.ServingUnits);
            Assert.IsNull(nutritionAttributes.SizeWeight);
            Assert.IsNull(nutritionAttributes.Calories);
            Assert.IsNull(nutritionAttributes.CaloriesFat);
            Assert.IsNull(nutritionAttributes.CaloriesSaturatedFat);
            Assert.IsNull(nutritionAttributes.TotalFatWeight);
            Assert.IsNull(nutritionAttributes.TotalFatPercentage);
            Assert.IsNull(nutritionAttributes.SaturatedFatWeight);
            Assert.IsNull(nutritionAttributes.SaturatedFatPercent);
            Assert.IsNull(nutritionAttributes.PolyunsaturatedFat);
            Assert.IsNull(nutritionAttributes.MonounsaturatedFat);
            Assert.IsNull(nutritionAttributes.CholesterolWeight);
            Assert.IsNull(nutritionAttributes.CholesterolPercent);
            Assert.IsNull(nutritionAttributes.SodiumWeight);
            Assert.IsNull(nutritionAttributes.SodiumPercent);
            Assert.IsNull(nutritionAttributes.PotassiumWeight);
            Assert.IsNull(nutritionAttributes.PotassiumPercent);
            Assert.IsNull(nutritionAttributes.TotalCarbohydrateWeight);
            Assert.IsNull(nutritionAttributes.TotalCarbohydratePercent);
            Assert.IsNull(nutritionAttributes.DietaryFiberWeight);
            Assert.IsNull(nutritionAttributes.DietaryFiberPercent);
            Assert.IsNull(nutritionAttributes.SolubleFiber);
            Assert.IsNull(nutritionAttributes.InsolubleFiber);
            Assert.IsNull(nutritionAttributes.Sugar);
            Assert.IsNull(nutritionAttributes.SugarAlcohol);
            Assert.IsNull(nutritionAttributes.OtherCarbohydrates);
            Assert.IsNull(nutritionAttributes.ProteinWeight);
            Assert.IsNull(nutritionAttributes.ProteinPercent);
            Assert.IsNull(nutritionAttributes.VitaminA);
            Assert.IsNull(nutritionAttributes.Betacarotene);
            Assert.IsNull(nutritionAttributes.VitaminC);
            Assert.IsNull(nutritionAttributes.Calcium);
            Assert.IsNull(nutritionAttributes.Iron);
            Assert.IsNull(nutritionAttributes.VitaminD);
            Assert.IsNull(nutritionAttributes.VitaminE);
            Assert.IsNull(nutritionAttributes.Thiamin);
            Assert.IsNull(nutritionAttributes.Riboflavin);
            Assert.IsNull(nutritionAttributes.Niacin);
            Assert.IsNull(nutritionAttributes.VitaminB6);
            Assert.IsNull(nutritionAttributes.Folate);
            Assert.IsNull(nutritionAttributes.VitaminB12);
            Assert.IsNull(nutritionAttributes.Biotin);
            Assert.IsNull(nutritionAttributes.PantothenicAcid);
            Assert.IsNull(nutritionAttributes.Phosphorous);
            Assert.IsNull(nutritionAttributes.Iodine);
            Assert.IsNull(nutritionAttributes.Magnesium);
            Assert.IsNull(nutritionAttributes.Zinc);
            Assert.IsNull(nutritionAttributes.Copper);
            Assert.IsNull(nutritionAttributes.TransFat);
            Assert.IsNull(nutritionAttributes.TransFatWeight);
            Assert.IsNull(nutritionAttributes.CaloriesFromTransFat);
            Assert.IsNull(nutritionAttributes.Om6Fatty);
            Assert.IsNull(nutritionAttributes.Om3Fatty);
            Assert.IsNull(nutritionAttributes.Starch);
            Assert.IsNull(nutritionAttributes.Chloride);
            Assert.IsNull(nutritionAttributes.Chromium);
            Assert.IsNull(nutritionAttributes.VitaminK);
            Assert.IsNull(nutritionAttributes.Manganese);
            Assert.IsNull(nutritionAttributes.Molybdenum);
            Assert.IsNull(nutritionAttributes.Selenium);

            var extAttributes = dbProvider.Connection.Query<dynamic>(
                "SELECT * FROM dbo.ItemAttributes_Ext WHERE ItemID = @ItemId",
                new { ItemId = itemId },
                dbProvider.Transaction)
                .ToList();
            Assert.AreEqual(0, extAttributes.Count);
        }

        [TestMethod]
        public void AddOrUpdateProducts_2ProductsAndOneHasNullNutritionAttributes_ShouldNotThrowAnError()
        {
            //Given
            var itemModel1 = CreateItemModelForAttributeTest(999999999);
            var itemModel2 = CreateItemModelForAttributeTest(999999998);
            itemModel2.NutritionAttributes = null;

            var commandData = new AddOrUpdateProductsCommand
            {
                Items = new List<ItemModel>
                    {
                        itemModel1,
                        itemModel2
                    }
            };

            //When
            commandHandler.Execute(commandData);

            //Then
            //No Error means the test passes
        }

        private ItemModel CreateItemModelForAttributeTest(int itemId)
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
                    FairTradeCertified = "Test FairTradeCertified",
                    FlexibleText = "Test FlexibleText",
                    GlobalPricingProgram = "GlobalPricingProgram",
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

        private void InsertNationalAndMerchandiseHierarchiesForTest()
        {
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
        
        private void InsertSimulatedPreExistingItemIntoDatabase(int itemId)
        {
            #region global attributes
            const int itemTypeID = 1;
            const string scanCode = "1234";
            const int hierarchyMerchandiseID = 1;
            const int hierarchyNationalClassID = 1;
            const int brandHCID = 1;
            const int taxClassHCID = 1;
            const int pSNumber = 1;
            const string desc_Product= "Test Desc";
            const string desc_POS= "Test POS Desc";
            const string packageUnit ="1";
            const string retailSize = "1";
            const string retailUOM = "EA";
            const int foodStampEligible = 0;
            const string desc_CustomerFriendly = "CustomerFriendlyDescription";
            #endregion

            #region sign attributes
            const string cheeseMilkType = "Cheese";
            const string agency_GlutenFree = "GlutenFree";
            const string agency_Kosher = "Kosher";
            const string agency_NonGMO = "NonGMO";
            const string agency_Organic = "Organic";
            const string agency_Vegan = "Vegan";
            const int isAirChilled = 0;
            const int isBiodynamic = 0;
            const int isCheeseRaw = 0;
            const int isDryAged = 0;
            const int isFreeRange = 0;
            const int isGrassFed = 0;
            const int isMadeInHouse = 0;
            const int isMsc = 0;
            const int isPastureRaised = 0;
            const int isPremiumBodyCare = 0;
            const int isVegetarian = 0;
            const int isWholeTrade = 0;
            const string rating_AnimalWelfare = "AnimalWelfare";
            const string rating_EcoScale = "EcoScale";
            const string rating_HealthyEating = "HealthyEating";
            const string seafood_FreshOrFrozen = "FreshOrFrozen";
            const string seafood_CatchType = "CatchType";
            #endregion

            #region nutrition attributes
            const string recipeName = "RecipeName";
            const string allergens = "Allergens";
            const string ingredients = "Ingredients";
            const int servingsPerPortion = 0;
            const string servingSizeDesc = "";
            const string servingPerContainer = "";
            const int hshRating = 0;
            const int servingUnits = 0;
            const int sizeWeight = 0;
            const int calories = 0;
            const int caloriesFat = 0;
            const int caloriesSaturatedFat = 0;
            const int totalFatWeight = 0;
            const int totalFatPercentage = 0;
            const int saturatedFatWeight = 0;
            const int saturatedFatPercent = 0;
            const int polyunsaturatedFat = 0;
            const int monounsaturatedFat = 0;
            const int cholesterolWeight = 0;
            const int cholesterolPercent = 0;
            const int sodiumWeight = 0;
            const int sodiumPercent = 0;
            const int potassiumWeight = 0;
            const int potassiumPercent = 0;
            const int totalCarbohydrateWeight = 0;
            const int totalCarbohydratePercent = 0;
            const int dietaryFiberWeight = 0;
            const int dietaryFiberPercent = 0;
            const int solubleFiber = 0;
            const int insolubleFiber = 0;
            const int sugar = 0;
            const int sugarAlcohol = 0;
            const int otherCarbohydrates = 0;
            const int proteinWeight = 0;
            const int proteinPercent = 0;
            const int vitaminA = 0;
            const int betacarotene = 0;
            const int vitaminC = 0;
            const int calcium = 0;
            const int iron = 0;
            const int vitaminD = 0;
            const int vitaminE = 0;
            const int thiamin = 0;
            const int riboflavin = 0;
            const int niacin = 0;
            const int vitaminB6 = 0;
            const int folate = 0;
            const int vitaminB12 = 0;
            const int biotin = 0;
            const int pantothenicAcid = 0;
            const int phosphorous = 0;
            const int iodine = 0;
            const int magnesium = 0;
            const int zinc = 0;
            const int copper = 0;
            const int transFat = 0;
            const int caloriesFromTransFat = 0;
            const int om6Fatty = 0;
            const int om3Fatty = 0;
            const int starch = 0;
            const int chloride = 0;
            const int chromium = 0;
            const int vitaminK = 0;
            const int manganese = 0;
            const int molybdenum = 0;
            const int selenium = 0;
            const int transFatWeight = 0;
            #endregion

            #region extended attributes
            const string fairTradeCertified = "Test";
            const string flexibleText = "Test";
            const string madeWithBiodynamicGrapes = "Test";
            const string madeWithOrganicGrapes = "Test";
            const string nutritionRequired = "Test";
            const string primeBeef = "Test";
            const string rainforestAlliance = "Test";
            const string refrigeratedOrShelfStable = "Test";
            const string smithsonianBirdFriendly = "Test";
            const string wic = "Test";
            #endregion

            var sqlToRun = $@"
                DECLARE @itemId INT = {itemId};
                INSERT INTO dbo.Items(
                    ItemID, ItemTypeID, ScanCode, HierarchyMerchandiseID, HierarchyNationalClassID, 
                    BrandHCID, TaxClassHCID, PSNumber, Desc_Product,  Desc_POS, 
                    PackageUnit, RetailSize, RetailUOM,  FoodStampEligible, Desc_CustomerFriendly)
                VALUES (
                    @itemId, {itemTypeID}, '{scanCode}', {hierarchyMerchandiseID}, {hierarchyNationalClassID},
                    {brandHCID}, {taxClassHCID}, {pSNumber}, '{desc_Product}', '{desc_POS}',
                    '{packageUnit}', '{retailSize}', '{retailUOM}', {foodStampEligible}, '{desc_CustomerFriendly}'
                )

                INSERT INTO dbo.ItemAttributes_Sign(
                    ItemID,
                    CheeseMilkType, Agency_GlutenFree, Agency_Kosher, Agency_NonGMO, Agency_Organic, Agency_Vegan,
                    IsAirChilled, IsBiodynamic, IsCheeseRaw, IsDryAged, IsFreeRange, IsGrassFed,
                    IsMadeInHouse, IsMsc, IsPastureRaised, IsPremiumBodyCare, IsVegetarian, IsWholeTrade,
                    Rating_AnimalWelfare, Rating_EcoScale, Rating_HealthyEating, Seafood_FreshOrFrozen, Seafood_CatchType,
                    AddedDate, ModifiedDate)
                values(
                    @itemID,
                    '{cheeseMilkType}', '{agency_GlutenFree}', '{agency_Kosher}', '{agency_NonGMO}', '{agency_Organic}', '{agency_Vegan}',
                    {isAirChilled}, {isBiodynamic}, {isCheeseRaw}, {isDryAged}, {isFreeRange}, {isGrassFed},
                    {isMadeInHouse}, {isMsc}, {isPastureRaised}, {isPremiumBodyCare}, {isVegetarian}, {isWholeTrade},
                    '{rating_AnimalWelfare}', '{rating_EcoScale}', '{rating_HealthyEating}', '{seafood_FreshOrFrozen}', '{seafood_CatchType}',
                    GETDATE(), null)

                INSERT INTO dbo.ItemAttributes_Nutrition(
                    ItemID,
		            RecipeName, Allergens, Ingredients, ServingsPerPortion, ServingSizeDesc, ServingPerContainer,
		            HshRating, ServingUnits, SizeWeight, Calories, CaloriesFat, CaloriesSaturatedFat,
		            TotalFatWeight, TotalFatPercentage, SaturatedFatWeight, SaturatedFatPercent, PolyunsaturatedFat, MonounsaturatedFat,
		            CholesterolWeight, CholesterolPercent, SodiumWeight, SodiumPercent, PotassiumWeight, PotassiumPercent,
		            TotalCarbohydrateWeight, TotalCarbohydratePercent, DietaryFiberWeight, DietaryFiberPercent, SolubleFiber, InsolubleFiber,
		            Sugar, SugarAlcohol, OtherCarbohydrates, ProteinWeight, ProteinPercent, VitaminA,
		            Betacarotene, VitaminC, Calcium, Iron, VitaminD, VitaminE,
		            Thiamin, Riboflavin, Niacin, VitaminB6, Folate, VitaminB12,
		            Biotin, PantothenicAcid, Phosphorous, Iodine, Magnesium, Zinc,
		            Copper, TransFat, CaloriesFromTransFat, Om6Fatty, Om3Fatty, Starch,
		            Chloride, Chromium, VitaminK, Manganese, Molybdenum, Selenium,
		            TransFatWeight)
                VALUES(
                    @itemID,
                    '{recipeName}', '{allergens}', '{ingredients}', {servingsPerPortion}, '{servingSizeDesc}', '{servingPerContainer}',
                    {hshRating}, {servingUnits}, {sizeWeight}, {calories}, {caloriesFat}, {caloriesSaturatedFat},
		            {totalFatWeight}, {totalFatPercentage}, {saturatedFatWeight}, {saturatedFatPercent}, {polyunsaturatedFat}, {monounsaturatedFat},
		            {cholesterolWeight}, {cholesterolPercent}, {sodiumWeight}, {sodiumPercent}, {potassiumWeight}, {potassiumPercent},
		            {totalCarbohydrateWeight}, {totalCarbohydratePercent}, {dietaryFiberWeight}, {dietaryFiberPercent}, {solubleFiber}, {insolubleFiber},
		            {sugar}, {sugarAlcohol}, {otherCarbohydrates}, {proteinWeight}, {proteinPercent}, {vitaminA},
		            {betacarotene}, {vitaminC}, {calcium}, {iron}, {vitaminD}, {vitaminE},
		            {thiamin}, {riboflavin}, {niacin}, {vitaminB6}, {folate}, {vitaminB12},
		            {biotin}, {pantothenicAcid}, {phosphorous}, {iodine}, {magnesium}, {zinc},
		            {copper}, {transFat}, {caloriesFromTransFat}, {om6Fatty}, {om3Fatty}, {starch},
		            {chloride}, {chromium}, {vitaminK}, {manganese}, {molybdenum}, {selenium},
		            {transFatWeight})

                INSERT INTO dbo.ItemAttributes_Ext(ItemID, AttributeID, AttributeValue, AddedDate)
                VALUES  (@itemID, {Attributes.FairTradeCertified}, '{fairTradeCertified}', GETDATE()),
                        (@itemID, {Attributes.FlexibleText}, '{flexibleText}', GETDATE()),
                        (@itemID, {Attributes.MadeWithBiodynamicGrapes}, '{madeWithBiodynamicGrapes}', GETDATE()),
                        (@itemID, {Attributes.MadeWithOrganicGrapes}, '{madeWithOrganicGrapes}', GETDATE()),
                        (@itemID, {Attributes.NutritionRequired}, '{nutritionRequired}', GETDATE()),
                        (@itemID, {Attributes.PrimeBeef}, '{primeBeef}', GETDATE()),
                        (@itemID, {Attributes.RainforestAlliance}, '{rainforestAlliance}', GETDATE()),
                        (@itemID, {Attributes.RefrigeratedOrShelfStable}, '{refrigeratedOrShelfStable}', GETDATE()),
                        (@itemID, {Attributes.SmithsonianBirdFriendly}, '{smithsonianBirdFriendly}', GETDATE()),
                        (@itemID, {Attributes.Wic}, '{wic}', GETDATE())";

            dbProvider.Connection.Execute(sql: sqlToRun,
                transaction: dbProvider.Transaction);
        }

        private dynamic ReadItemAttributesDynamic(int itemId)
        {
            var items = dbProvider.Connection.Query<dynamic>(
                 "SELECT * FROM dbo.Items WHERE ItemID = @ItemId",
                 new { ItemId = itemId },
                 dbProvider.Transaction);
            return items?.Single();
        }

        private dynamic ReadSignAttributesDynamic(int itemId)
        {
            var signAttributes = dbProvider.Connection.Query<dynamic>(
                  "SELECT * FROM dbo.ItemAttributes_Sign WHERE ItemID = @ItemId",
                  new { ItemId = itemId },
                  dbProvider.Transaction);
            return signAttributes.Single();
        }

        private dynamic ReadNutritionAttributesDynamic(int itemId)
        {
            var nutritionAttributes = dbProvider.Connection.Query<dynamic>(
                "SELECT * FROM dbo.ItemAttributes_Nutrition WHERE ItemID = @ItemId",
                new { ItemId = itemId },
                dbProvider.Transaction);
            return nutritionAttributes?.Single();
        }

        private IList<dynamic> ReadExtendedAttributesDynamic(int itemId)
        {
            return dbProvider.Connection.Query<dynamic>(
                "SELECT * FROM dbo.ItemAttributes_Ext WHERE ItemID = @ItemId",
                new { ItemId = itemId },
                dbProvider.Transaction)
                .ToList();
        }

        private void AssertGlobalAttributesAsExpected(
            GlobalAttributesModel expectedGlobalAttributes,
            dynamic actualItemsRow)
        {
            Assert.AreEqual(expectedGlobalAttributes.ItemTypeID, actualItemsRow.ItemTypeID);
            Assert.AreEqual(this.testHierarchyNationalClassID, actualItemsRow.HierarchyNationalClassID);
            Assert.AreEqual(this.testHierarchyHierarchyMerchandiseID, actualItemsRow.HierarchyMerchandiseID);
            Assert.AreEqual(expectedGlobalAttributes.BrandHCID, actualItemsRow.BrandHCID);
            Assert.AreEqual(expectedGlobalAttributes.TaxClassHCID, actualItemsRow.TaxClassHCID);
            Assert.AreEqual(expectedGlobalAttributes.PSNumber, actualItemsRow.PSNumber);
            Assert.AreEqual(expectedGlobalAttributes.Desc_Product, actualItemsRow.Desc_Product);
            Assert.AreEqual(expectedGlobalAttributes.Desc_POS, actualItemsRow.Desc_POS);
            Assert.AreEqual(expectedGlobalAttributes.PackageUnit, actualItemsRow.PackageUnit);
            Assert.AreEqual(expectedGlobalAttributes.RetailSize, actualItemsRow.RetailSize);
            Assert.AreEqual(expectedGlobalAttributes.RetailUOM, actualItemsRow.RetailUOM);
            Assert.AreEqual(expectedGlobalAttributes.Desc_CustomerFriendly, actualItemsRow.Desc_CustomerFriendly);
            Assert.IsNotNull(actualItemsRow.AddedDate);
        }

        private void AssertNutritionAttributesAsExpected(
            NutritionAttributesModel expectedNutritionAttributes,
            dynamic actualItemAttributes_NutritionRow)
        {
            Assert.AreEqual(expectedNutritionAttributes.RecipeName, actualItemAttributes_NutritionRow.RecipeName);
            Assert.AreEqual(expectedNutritionAttributes.Allergens, actualItemAttributes_NutritionRow.Allergens);
            Assert.AreEqual(expectedNutritionAttributes.Ingredients, actualItemAttributes_NutritionRow.Ingredients);
            Assert.AreEqual(expectedNutritionAttributes.ServingsPerPortion, (float)actualItemAttributes_NutritionRow.ServingsPerPortion);
            Assert.AreEqual(expectedNutritionAttributes.ServingSizeDesc, actualItemAttributes_NutritionRow.ServingSizeDesc);
            Assert.AreEqual(expectedNutritionAttributes.ServingPerContainer, actualItemAttributes_NutritionRow.ServingPerContainer);
            Assert.AreEqual(expectedNutritionAttributes.HshRating, actualItemAttributes_NutritionRow.HshRating);
            Assert.AreEqual(expectedNutritionAttributes.ServingUnits, actualItemAttributes_NutritionRow.ServingUnits);
            Assert.AreEqual(expectedNutritionAttributes.SizeWeight, actualItemAttributes_NutritionRow.SizeWeight);
            Assert.AreEqual(expectedNutritionAttributes.Calories, actualItemAttributes_NutritionRow.Calories);
            Assert.AreEqual(expectedNutritionAttributes.CaloriesFat, actualItemAttributes_NutritionRow.CaloriesFat);
            Assert.AreEqual(expectedNutritionAttributes.CaloriesSaturatedFat, actualItemAttributes_NutritionRow.CaloriesSaturatedFat);
            Assert.AreEqual(expectedNutritionAttributes.TotalFatWeight, actualItemAttributes_NutritionRow.TotalFatWeight);
            Assert.AreEqual(expectedNutritionAttributes.TotalFatPercentage, actualItemAttributes_NutritionRow.TotalFatPercentage);
            Assert.AreEqual(expectedNutritionAttributes.SaturatedFatWeight, actualItemAttributes_NutritionRow.SaturatedFatWeight);
            Assert.AreEqual(expectedNutritionAttributes.SaturatedFatPercent, actualItemAttributes_NutritionRow.SaturatedFatPercent);
            Assert.AreEqual(expectedNutritionAttributes.PolyunsaturatedFat, actualItemAttributes_NutritionRow.PolyunsaturatedFat);
            Assert.AreEqual(expectedNutritionAttributes.MonounsaturatedFat, actualItemAttributes_NutritionRow.MonounsaturatedFat);
            Assert.AreEqual(expectedNutritionAttributes.CholesterolWeight, actualItemAttributes_NutritionRow.CholesterolWeight);
            Assert.AreEqual(expectedNutritionAttributes.CholesterolPercent, actualItemAttributes_NutritionRow.CholesterolPercent);
            Assert.AreEqual(expectedNutritionAttributes.SodiumWeight, actualItemAttributes_NutritionRow.SodiumWeight);
            Assert.AreEqual(expectedNutritionAttributes.SodiumPercent, actualItemAttributes_NutritionRow.SodiumPercent);
            Assert.AreEqual(expectedNutritionAttributes.PotassiumWeight, actualItemAttributes_NutritionRow.PotassiumWeight);
            Assert.AreEqual(expectedNutritionAttributes.PotassiumPercent, actualItemAttributes_NutritionRow.PotassiumPercent);
            Assert.AreEqual(expectedNutritionAttributes.TotalCarbohydrateWeight, actualItemAttributes_NutritionRow.TotalCarbohydrateWeight);
            Assert.AreEqual(expectedNutritionAttributes.TotalCarbohydratePercent, actualItemAttributes_NutritionRow.TotalCarbohydratePercent);
            Assert.AreEqual(expectedNutritionAttributes.DietaryFiberWeight, actualItemAttributes_NutritionRow.DietaryFiberWeight);
            Assert.AreEqual(expectedNutritionAttributes.DietaryFiberPercent, actualItemAttributes_NutritionRow.DietaryFiberPercent);
            Assert.AreEqual(expectedNutritionAttributes.SolubleFiber, actualItemAttributes_NutritionRow.SolubleFiber);
            Assert.AreEqual(expectedNutritionAttributes.InsolubleFiber, actualItemAttributes_NutritionRow.InsolubleFiber);
            Assert.AreEqual(expectedNutritionAttributes.Sugar, actualItemAttributes_NutritionRow.Sugar);
            Assert.AreEqual(expectedNutritionAttributes.SugarAlcohol, actualItemAttributes_NutritionRow.SugarAlcohol);
            Assert.AreEqual(expectedNutritionAttributes.OtherCarbohydrates, actualItemAttributes_NutritionRow.OtherCarbohydrates);
            Assert.AreEqual(expectedNutritionAttributes.ProteinWeight, actualItemAttributes_NutritionRow.ProteinWeight);
            Assert.AreEqual(expectedNutritionAttributes.ProteinPercent, actualItemAttributes_NutritionRow.ProteinPercent);
            Assert.AreEqual(expectedNutritionAttributes.VitaminA, actualItemAttributes_NutritionRow.VitaminA);
            Assert.AreEqual(expectedNutritionAttributes.Betacarotene, actualItemAttributes_NutritionRow.Betacarotene);
            Assert.AreEqual(expectedNutritionAttributes.VitaminC, actualItemAttributes_NutritionRow.VitaminC);
            Assert.AreEqual(expectedNutritionAttributes.Calcium, actualItemAttributes_NutritionRow.Calcium);
            Assert.AreEqual(expectedNutritionAttributes.Iron, actualItemAttributes_NutritionRow.Iron);
            Assert.AreEqual(expectedNutritionAttributes.VitaminD, actualItemAttributes_NutritionRow.VitaminD);
            Assert.AreEqual(expectedNutritionAttributes.VitaminE, actualItemAttributes_NutritionRow.VitaminE);
            Assert.AreEqual(expectedNutritionAttributes.Thiamin, actualItemAttributes_NutritionRow.Thiamin);
            Assert.AreEqual(expectedNutritionAttributes.Riboflavin, actualItemAttributes_NutritionRow.Riboflavin);
            Assert.AreEqual(expectedNutritionAttributes.Niacin, actualItemAttributes_NutritionRow.Niacin);
            Assert.AreEqual(expectedNutritionAttributes.VitaminB6, actualItemAttributes_NutritionRow.VitaminB6);
            Assert.AreEqual(expectedNutritionAttributes.Folate, actualItemAttributes_NutritionRow.Folate);
            Assert.AreEqual(expectedNutritionAttributes.VitaminB12, actualItemAttributes_NutritionRow.VitaminB12);
            Assert.AreEqual(expectedNutritionAttributes.Biotin, actualItemAttributes_NutritionRow.Biotin);
            Assert.AreEqual(expectedNutritionAttributes.PantothenicAcid, actualItemAttributes_NutritionRow.PantothenicAcid);
            Assert.AreEqual(expectedNutritionAttributes.Phosphorous, actualItemAttributes_NutritionRow.Phosphorous);
            Assert.AreEqual(expectedNutritionAttributes.Iodine, actualItemAttributes_NutritionRow.Iodine);
            Assert.AreEqual(expectedNutritionAttributes.Magnesium, actualItemAttributes_NutritionRow.Magnesium);
            Assert.AreEqual(expectedNutritionAttributes.Zinc, actualItemAttributes_NutritionRow.Zinc);
            Assert.AreEqual(expectedNutritionAttributes.Copper, actualItemAttributes_NutritionRow.Copper);
            Assert.AreEqual(expectedNutritionAttributes.TransFat, actualItemAttributes_NutritionRow.Transfat);
            Assert.AreEqual(expectedNutritionAttributes.TransFatWeight, actualItemAttributes_NutritionRow.TransFatWeight);
            Assert.AreEqual(expectedNutritionAttributes.CaloriesFromTransFat, actualItemAttributes_NutritionRow.CaloriesFromTransFat);
            Assert.AreEqual(expectedNutritionAttributes.Om6Fatty, actualItemAttributes_NutritionRow.Om6Fatty);
            Assert.AreEqual(expectedNutritionAttributes.Om3Fatty, actualItemAttributes_NutritionRow.Om3Fatty);
            Assert.AreEqual(expectedNutritionAttributes.Starch, actualItemAttributes_NutritionRow.Starch);
            Assert.AreEqual(expectedNutritionAttributes.Chloride, actualItemAttributes_NutritionRow.Chloride);
            Assert.AreEqual(expectedNutritionAttributes.Chromium, actualItemAttributes_NutritionRow.Chromium);
            Assert.AreEqual(expectedNutritionAttributes.VitaminK, actualItemAttributes_NutritionRow.VitaminK);
            Assert.AreEqual(expectedNutritionAttributes.Manganese, actualItemAttributes_NutritionRow.Manganese);
            Assert.AreEqual(expectedNutritionAttributes.Molybdenum, actualItemAttributes_NutritionRow.Molybdenum);
            Assert.AreEqual(expectedNutritionAttributes.Selenium, actualItemAttributes_NutritionRow.Selenium);
        }

        private void AssertSignAttributesAsExpected(SignAttributesModel expectedSignAttributes,
            dynamic actualItemAttributes_SignRow)
        {
            Assert.AreEqual(expectedSignAttributes.CheeseMilkType, actualItemAttributes_SignRow.CheeseMilkType);
            Assert.AreEqual(expectedSignAttributes.Agency_GlutenFree, actualItemAttributes_SignRow.Agency_GlutenFree);
            Assert.AreEqual(expectedSignAttributes.Agency_Kosher, actualItemAttributes_SignRow.Agency_Kosher);
            Assert.AreEqual(expectedSignAttributes.Agency_NonGMO, actualItemAttributes_SignRow.Agency_NonGMO);
            Assert.AreEqual(expectedSignAttributes.Agency_Organic, actualItemAttributes_SignRow.Agency_Organic);
            Assert.AreEqual(expectedSignAttributes.Agency_Vegan, actualItemAttributes_SignRow.Agency_Vegan);
            Assert.AreEqual(expectedSignAttributes.IsAirChilled, actualItemAttributes_SignRow.IsAirChilled);
            Assert.AreEqual(expectedSignAttributes.IsBiodynamic, actualItemAttributes_SignRow.IsBiodynamic);
            Assert.AreEqual(expectedSignAttributes.IsCheeseRaw, actualItemAttributes_SignRow.IsCheeseRaw);
            Assert.AreEqual(expectedSignAttributes.IsDryAged, actualItemAttributes_SignRow.IsDryAged);
            Assert.AreEqual(expectedSignAttributes.IsFreeRange, actualItemAttributes_SignRow.IsFreeRange);
            Assert.AreEqual(expectedSignAttributes.IsGrassFed, actualItemAttributes_SignRow.IsGrassFed);
            Assert.AreEqual(expectedSignAttributes.IsMadeInHouse, actualItemAttributes_SignRow.IsMadeInHouse);
            Assert.AreEqual(expectedSignAttributes.IsMsc, actualItemAttributes_SignRow.IsMsc);
            Assert.AreEqual(expectedSignAttributes.IsPastureRaised, actualItemAttributes_SignRow.IsPastureRaised);
            Assert.AreEqual(expectedSignAttributes.IsPremiumBodyCare, actualItemAttributes_SignRow.IsPremiumBodyCare);
            Assert.AreEqual(expectedSignAttributes.IsVegetarian, actualItemAttributes_SignRow.IsVegetarian);
            Assert.AreEqual(expectedSignAttributes.IsWholeTrade, actualItemAttributes_SignRow.IsWholeTrade);
            Assert.AreEqual(expectedSignAttributes.Rating_AnimalWelfare, actualItemAttributes_SignRow.Rating_AnimalWelfare);
            Assert.AreEqual(expectedSignAttributes.Rating_EcoScale, actualItemAttributes_SignRow.Rating_EcoScale);
            Assert.AreEqual(expectedSignAttributes.Rating_HealthyEating, actualItemAttributes_SignRow.Rating_HealthyEating);
            Assert.AreEqual(expectedSignAttributes.Seafood_FreshOrFrozen, actualItemAttributes_SignRow.Seafood_FreshOrFrozen);
            Assert.AreEqual(expectedSignAttributes.Seafood_CatchType, actualItemAttributes_SignRow.Seafood_CatchType);

        }

        private void AssertExtendedAttributesAsExpected(
            ExtendedAttributesModel expectedExtendedAttributes,
            IEnumerable<dynamic> actualItemAttributes_ExtRows)
        {
            Assert.AreEqual(
                expectedExtendedAttributes.FairTradeCertified,
                actualItemAttributes_ExtRows.Single(ea => ea.AttributeID == Attributes.FairTradeCertified).AttributeValue);
            Assert.AreEqual(
                expectedExtendedAttributes.FlexibleText,
                actualItemAttributes_ExtRows.Single(ea => ea.AttributeID == Attributes.FlexibleText).AttributeValue);
            Assert.AreEqual(
                expectedExtendedAttributes.GlobalPricingProgram,
                actualItemAttributes_ExtRows.Single(ea => ea.AttributeID == Attributes.GlobalPricingProgram).AttributeValue);
            Assert.AreEqual(
                expectedExtendedAttributes.MadeWithBiodynamicGrapes,
                actualItemAttributes_ExtRows.Single(ea => ea.AttributeID == Attributes.MadeWithBiodynamicGrapes).AttributeValue);
            Assert.AreEqual(
                expectedExtendedAttributes.MadeWithOrganicGrapes,
                actualItemAttributes_ExtRows.Single(ea => ea.AttributeID == Attributes.MadeWithOrganicGrapes).AttributeValue);
            Assert.AreEqual(
                expectedExtendedAttributes.NutritionRequired,
                actualItemAttributes_ExtRows.Single(ea => ea.AttributeID == Attributes.NutritionRequired).AttributeValue);
            Assert.AreEqual(
                expectedExtendedAttributes.PrimeBeef,
                actualItemAttributes_ExtRows.Single(ea => ea.AttributeID == Attributes.PrimeBeef).AttributeValue);
            Assert.AreEqual(
                expectedExtendedAttributes.RainforestAlliance,
                actualItemAttributes_ExtRows.Single(ea => ea.AttributeID == Attributes.RainforestAlliance).AttributeValue);
            Assert.AreEqual(
                expectedExtendedAttributes.RefrigeratedOrShelfStable,
                actualItemAttributes_ExtRows.Single(ea => ea.AttributeID == Attributes.RefrigeratedOrShelfStable).AttributeValue);
            Assert.AreEqual(
                expectedExtendedAttributes.SmithsonianBirdFriendly,
                actualItemAttributes_ExtRows.Single(ea => ea.AttributeID == Attributes.SmithsonianBirdFriendly).AttributeValue);
            Assert.AreEqual(
                expectedExtendedAttributes.Wic,
                actualItemAttributes_ExtRows.Single(ea => ea.AttributeID == Attributes.Wic).AttributeValue);
        }
    }
}
