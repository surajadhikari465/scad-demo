using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NutritionWebApi.Common;
using NutritionWebApi.Common.Models;
using NutritionWebApi.DataAccess.Commands;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace NutritionWebApi.Tests.Integration.Commands
{
	class MessageQueueInfo
	{
		public string Plu { get; set; }
		public int ServingUnits { get; set; }
		public string RecipeName { get; set; }
		public int? HshRating { get; set; }
		public int? VitaminA { get; set; }
	}
	
	[TestClass]
	public class UpdateNutritionItemCommandHandlerTests
	{
		private IDbConnectionProvider connectionProvider;
		private string connectionString;
		private AddOrUpdateNutritionItemCommandHandler commandHandler;
		NutritionItemModel nutritionItem;

		[TestInitialize]
		public void Initialize()
		{
			this.connectionProvider = new DbConnectionProvider();
			this.connectionString = ApiConfigSettings.Instance.ConnectionString;
			this.connectionProvider.Connection = new SqlConnection(this.connectionString);
			this.connectionProvider.Connection.Open();
			this.commandHandler = new AddOrUpdateNutritionItemCommandHandler(this.connectionProvider);

			string sql = "select top 1* from nutrition.ItemNutrition itn join dbo.scancode sc on itn.plu = sc.scancode";
			nutritionItem = this.connectionProvider.Connection.Query<NutritionItemModel>(sql, commandType: CommandType.Text).FirstOrDefault();
		}

		[TestCleanup]
		public void Cleanup()
		{
			if(this.connectionProvider.Connection != null)
			{
				this.connectionProvider.Connection.Dispose();
			}
		}

		[TestMethod]
		public void UpdateNutritionItemCommandHandler_SuccessfulExecution_NutritionItemIsUpdateed()
		{
			// Given.
			List<NutritionItemModel> itemModel = new List<NutritionItemModel>();
			nutritionItem.RecipeName = "Integration Test";
			itemModel.Add(nutritionItem);

			// When.
			string result = commandHandler.Execute(new AddOrUpdateNutritionItemCommand() { NutritionItems = itemModel });

			// Then.
			Assert.IsNotNull(result);
		}

		 private List<NutritionItemModel> BuildNutritionItemList(int count)
     {
         var itemList = new List<NutritionItemModel>();
         for (int i = 1; i <= count; i++)
         {
             var builder = new Common.Builders.NutritionItemModelBuilder().WithPlu((i).ToString())
                 .WithRecipeName("Integration Test Recipe " + (i).ToString())
							.WithServingUnits(1)
							.WithHshRating(3)
							.WithVitaminA(5);
             itemList.Add(builder);
         }
         return itemList;
     }

		[TestMethod]
		public void UpdateNutritionItemCommandHandler_SuccessfulExecution_ProductMessageIsGenerated()
		{
			// Given.
			var itemModel = new List<NutritionItemModel>();
			nutritionItem.RecipeName = "Integration test recipe";
			nutritionItem.ServingUnits = 1;
			nutritionItem.HshRating = 3;
			nutritionItem.VitaminA = 5;

			itemModel.Add(nutritionItem);

			// When
			var result = commandHandler.Execute(new AddOrUpdateNutritionItemCommand() { NutritionItems = itemModel });
			var messageID = this.connectionProvider.Connection.ExecuteScalar(sql: $"SELECT MessageQueueId FROM app.MessageQueueProduct WHERE ScanCode = '{nutritionItem.Plu}' AND MessageStatusId = 1;", commandType: CommandType.Text);
			var actualNutritionMessage = this.connectionProvider.Connection.Query<MessageQueueInfo>(sql: $"SELECT Plu, ServingUnits, RecipeName, HshRating, VitaminA FROM app.MessageQueueNutrition WHERE MessageQueueId = {messageID}" ).FirstOrDefault();

			// Then.
			Assert.IsNotNull(result);
			Assert.AreEqual(nutritionItem.Plu, actualNutritionMessage.Plu);
			Assert.AreEqual(nutritionItem.ServingUnits, actualNutritionMessage.ServingUnits);
			Assert.AreEqual(nutritionItem.RecipeName, actualNutritionMessage.RecipeName);
			Assert.AreEqual(nutritionItem.HshRating, actualNutritionMessage.HshRating);
			Assert.AreEqual(nutritionItem.VitaminA, actualNutritionMessage.VitaminA);
		}
	}
}