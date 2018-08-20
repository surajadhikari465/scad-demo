using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MammothWebApi.DataAccess.Queries;
using Mammoth.Common.DataAccess.DbProviders;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using System.Linq;
using MammothWebApi.DataAccess.Models;

namespace MammothWebApi.Tests.DataAccess.Queries
{
    [TestClass]
    public class GetItemNutritionByItemIdQueryHandlerTests
    {
        private GetItemNutritionAttributesByItemIdQueryHandler queryHandler;
        private GetItemNutritionAttributesByItemIdQuery query;
        private SqlDbProvider db;
        private int id;

        [TestInitialize]
        public void Initialize()
        {
            db = new SqlDbProvider();
            db.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();


            id = 1882959;
            queryHandler = new GetItemNutritionAttributesByItemIdQueryHandler(db);
            query = new GetItemNutritionAttributesByItemIdQuery() {ItemIds = new List<int>() {id}};
        }
        [TestMethod]
        public void GetAllBusinessUnits_LocalesExist_ShouldReturnAllBusinessUnits()
        {
            //Given
            var expectedResults = db.Connection.Query<ItemNutritionAttributes>(
                @"SELECT 
		ian.ItemID, 
		ian.Calories, 
		ian.ServingUnits, 
		ian.ServingsPerPortion, 
		ian.ServingSizeDesc, 
		ian.ServingPerContainer
	FROM dbo.ItemAttributes_Nutrition ian
    where ian.ItemId = @Id",
                new {Id=id},
                db.Transaction).ToList();

            //When
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
