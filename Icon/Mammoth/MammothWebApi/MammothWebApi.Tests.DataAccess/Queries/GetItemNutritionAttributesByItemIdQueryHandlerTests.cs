using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MammothWebApi.DataAccess.Queries;
using Mammoth.Common.DataAccess.DbProviders;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using System.Linq;
using System.Transactions;
using MammothWebApi.DataAccess.Models;

namespace MammothWebApi.Tests.DataAccess.Queries
{
    [TestClass]
    public class GetItemNutritionByItemIdQueryHandlerTests
    {
        private GetItemNutritionAttributesByItemIdQueryHandler queryHandler;
        private GetItemNutritionAttributesByItemIdQuery query;
        private SqlDbProvider db;
        private TransactionScope transaction;
        private int id;
        private int testValidPSNumber = 8888;
        private List<int> testItemIds;


        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            db = new SqlDbProvider();
            db.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();
            testItemIds = new List<int> {99999990, 99999991};
            queryHandler = new GetItemNutritionAttributesByItemIdQueryHandler(db);
            
            InsertTestData();

        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        private void InsertTestData()
        {
            foreach (var itemId in testItemIds)
            {
                InsertTestItem(itemId, testValidPSNumber);
            }

            InsertTestNutritionAttributes();
        }

        private void InsertTestNutritionAttributes()
        {

            //99999990
            db.Connection.Execute(@"Insert Into dbo.ItemAttributes_Nutrition  ([ItemID],[RecipeName],[Allergens],[Ingredients],[ServingsPerPortion],[ServingSizeDesc],[ServingPerContainer],[HshRating],[ServingUnits],[SizeWeight],[Calories],[CaloriesFat],[CaloriesSaturatedFat],[TotalFatWeight],[TotalFatPercentage],[SaturatedFatWeight],[SaturatedFatPercent],[PolyunsaturatedFat],[MonounsaturatedFat],[CholesterolWeight],[CholesterolPercent],[SodiumWeight],[SodiumPercent],[PotassiumWeight],[PotassiumPercent],[TotalCarbohydrateWeight],[TotalCarbohydratePercent],[DietaryFiberWeight],[DietaryFiberPercent],[SolubleFiber],[InsolubleFiber],[Sugar],[SugarAlcohol],[OtherCarbohydrates],[ProteinWeight],[ProteinPercent],[VitaminA],[Betacarotene],[VitaminC],[Calcium],[Iron],[VitaminD],[VitaminE],[Thiamin],[Riboflavin],[Niacin],[VitaminB6],[Folate],[VitaminB12],[Biotin],[PantothenicAcid],[Phosphorous],[Iodine],[Magnesium],[Zinc],[Copper],[Transfat],[CaloriesFromTransFat],[Om6Fatty],[Om3Fatty],[Starch],[Chloride],[Chromium],[VitaminK],[Manganese],[Molybdenum],[Selenium],[TransFatWeight],[AddedDate],[ModifiedDate])
                                    Values (N'99999990',N'Bar, Peanut Butter By LB Chris MA',N'CONTAINS:MILK,EGGS,WHEAT,PEANUTS,SOY.',N'Ingredients: Natural Peanut Butter (dry roasted peanuts), Confectionary Sugar (sugar, cornstarch), Butter, Granulated Sugar, Peanuts, Enriched Wheat Flour (wheat flour, niacin, reduced iron, thiamine mononitrate, riboflavin, enzyme, folic acid), Milk Chocolate (sugar, cocoa butter, whole milk powder, unsweetened chocolate, soy lecithin, natural vanilla extract), Semi Sweet Chocolate (sugar, unsweetened chocolate, cocoa butter, soy lecithin, natural vanilla extract), Heavy Cream, Canola Oil, Eggs, Cocoa Powder, Corn Syrup, Invert Sugar, Black Cocoa.',4,N'1 ea',N'varied',0,1,1,580,360,130,40.0,61,14.0,70,0.0,0.0,50.0,17,90.0,4,0.0,0,54.0,18,4.0,16,0.0,0.0,40.0,0.0,0.0,12.0,24,20,0,0,4,15,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0.0,0,0.0,0.0,0.0,0,0,0,0,0,0,0.0,'2018-06-04 13:59:41.550',NULL)");

            //99999991
            db.Connection.Execute(@"Insert Into  dbo.ItemAttributes_Nutrition ([ItemID],[RecipeName],[Allergens],[Ingredients],[ServingsPerPortion],[ServingSizeDesc],[ServingPerContainer],[HshRating],[ServingUnits],[SizeWeight],[Calories],[CaloriesFat],[CaloriesSaturatedFat],[TotalFatWeight],[TotalFatPercentage],[SaturatedFatWeight],[SaturatedFatPercent],[PolyunsaturatedFat],[MonounsaturatedFat],[CholesterolWeight],[CholesterolPercent],[SodiumWeight],[SodiumPercent],[PotassiumWeight],[PotassiumPercent],[TotalCarbohydrateWeight],[TotalCarbohydratePercent],[DietaryFiberWeight],[DietaryFiberPercent],[SolubleFiber],[InsolubleFiber],[Sugar],[SugarAlcohol],[OtherCarbohydrates],[ProteinWeight],[ProteinPercent],[VitaminA],[Betacarotene],[VitaminC],[Calcium],[Iron],[VitaminD],[VitaminE],[Thiamin],[Riboflavin],[Niacin],[VitaminB6],[Folate],[VitaminB12],[Biotin],[PantothenicAcid],[Phosphorous],[Iodine],[Magnesium],[Zinc],[Copper],[Transfat],[CaloriesFromTransFat],[Om6Fatty],[Om3Fatty],[Starch],[Chloride],[Chromium],[VitaminK],[Manganese],[Molybdenum],[Selenium],[TransFatWeight],[AddedDate],[ModifiedDate])
                                    Values (N'99999991',N'Turnover, Cherry 4PK Orange Bakery CE',N'CONTAINS:WHEAT',N'Ingredients: Dough (flour [enriched wheat flour (wheat flour,  niacin, reduced iron, thiamin mononitrate, riboflavin, folic acid), malted barley flour, ascorbic acid as a dough conditioner], water, shortening [palm oil, beta carotene (color)], vital wheat gluten, salt), Cherry Filling (cherries, sugar*, water, corn syrup*, modified corn starch*, contains 2% or less of the following: gellan gum, citric acid, salt, natural flavor*), Topping (crystal sugar*). *Non-GMO',4,N'1 ea',N'4',0,1,1,280,130,60,14.0,22,7.0,35,0.0,0.0,0.0,0,270.0,11,0.0,0,35.0,12,1.0,4,0.0,0.0,8.0,0.0,0.0,4.0,8,6,0,0,0,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0.0,0,0.0,0.0,0.0,0,0,0,0,0,0,0.0,'2018-06-04 13:59:41.550',NULL)");
        }

        private void InsertTestItem(int itemId, int pSNumber)
        {
            db.Connection.Execute(
                $@" INSERT INTO dbo.Items(
                            ItemID, 
                            ScanCode, 
                            PSNumber, 
                            ItemTypeID)
                        SELECT @ItemId, 
                            @ScanCode, 
                            @PSNumber, 
                            ItemTypeID 
                            FROM dbo.ItemTypes 
                        WHERE itemTypeCode = @ItemTypeCode",
                new { ItemId = itemId, ScanCode = "sc" + itemId, PSNumber = pSNumber, ItemTypeCode = "TST" });
        }

        [TestMethod]
        public void GetItemNutritionAttributes_ReturnSpecificQuery()
        {


            //Given
            var testItem1 = 99999990;
            
            var expectedResults = db.Connection.Query<ItemNutritionAttributes>(
                @"SELECT 
		ian.ItemID, 
		ian.Calories, 
		ian.ServingUnits, 
		ian.ServingsPerPortion, 
		ian.ServingSizeDesc, 
		ian.ServingPerContainer
	FROM dbo.ItemAttributes_Nutrition ian
    where ian.ItemId = @id", new  {id=testItem1}).ToList();

            //When
            query = new GetItemNutritionAttributesByItemIdQuery() { ItemIds = new List<int>() { testItem1 } };
            var actualResults = queryHandler.Search(query);

            //Then
            Assert.IsTrue(expectedResults.First().Calories == actualResults.First().Calories);
            Assert.IsTrue(expectedResults.First().ItemId == actualResults.First().ItemId);
            Assert.IsTrue(expectedResults.First().ServingsPerPortion == actualResults.First().ServingsPerPortion);
            Assert.IsTrue(expectedResults.First().ServingPerContainer == actualResults.First().ServingPerContainer);
            Assert.IsTrue(expectedResults.First().ServingSizeDesc == actualResults.First().ServingSizeDesc);

        }
    }
}
