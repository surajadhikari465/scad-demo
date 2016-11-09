using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NutritionWebApi.Common;
using NutritionWebApi.Common.Models;
using NutritionWebApi.DataAccess.Commands;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Icon.Framework;

namespace NutritionWebApi.Tests.Integration.Commands
{
    [TestClass]
    public class UpdateNutritionItemCommandHandlerTests
    {
        private IDbConnectionProvider connectionProvider;
        private string connectionString;
        private AddOrUpdateNutritionItemCommandHandler commandHandler;
        NutritionItemModel nutritionItem;
        private IconContext context;

        [TestInitialize]
        public void Initialize()
        {
            this.connectionProvider = new DbConnectionProvider();
            this.connectionString = ApiConfigSettings.Instance.ConnectionString;
            this.connectionProvider.Connection = new SqlConnection(this.connectionString);
            this.connectionProvider.Connection.Open();
            this.commandHandler = new AddOrUpdateNutritionItemCommandHandler(this.connectionProvider);
            context = new IconContext();

            string sql = "select * from nutrition.ItemNutrition itn join dbo.scancode sc on itn.plu = sc.scancode";

            nutritionItem = this.connectionProvider.Connection.Query<NutritionItemModel>(sql, commandType: CommandType.Text).AsList().FirstOrDefault();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (this.connectionProvider.Connection != null)
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


        [TestMethod]
        public void UpdateNutritionItemCommandHandler_SuccessfulExecution_ProductMessageIsGenerated()
        {
            // Given.
            List<NutritionItemModel> itemModel = new List<NutritionItemModel>();            
            nutritionItem.RecipeName = "Integration test recipe";
            nutritionItem.ServingUnits = 1;
            nutritionItem.HshRating = 3;
            nutritionItem.VitaminA = 5;

            itemModel.Add(nutritionItem);

            // When.
            string result = commandHandler.Execute(new AddOrUpdateNutritionItemCommand() { NutritionItems = itemModel });

            var actualMessage = context.MessageQueueProduct
                .Where(mq => mq.ScanCode == nutritionItem.Plu && mq.MessageStatusId == MessageStatusTypes.Ready)
                .OrderByDescending(mq => mq.MessageQueueId)
                .First();

            var actualNutritionMessage = context.MessageQueueNutrition
                .Where(mqn => mqn.MessageQueueProduct.MessageQueueId == actualMessage.MessageQueueId)
                .FirstOrDefault();

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
