using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NutritionWebApi.Common;
using NutritionWebApi.Common.Models;
using NutritionWebApi.DataAccess.Commands;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace NutritionWebApi.Tests.Integration.Commands
{
    [TestClass]
    public class DeleteNutritionCommandHandlerTests
    {
        private IDbConnectionProvider connectionProvider;
        private string connectionString;
        private DeleteNutritionCommandHandler commandHandler;
        private DeleteNutritionCommand command;
        private NutritionItemModel nutritionItem;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            this.connectionProvider = new DbConnectionProvider();
            this.connectionString = ApiConfigSettings.Instance.ConnectionString;
            this.transaction = new TransactionScope();
            this.connectionProvider.Connection = new SqlConnection(this.connectionString);
            this.connectionProvider.Connection.Open();
            this.commandHandler = new DeleteNutritionCommandHandler(this.connectionProvider);

            string sql = @"select top 1 * from nutrition.ItemNutrition itn join dbo.scancode sc on itn.plu = sc.scancode join app.IRMAItemSubscription s on sc.scanCode = s.identifier";
            nutritionItem = this.connectionProvider.Connection.Query<NutritionItemModel>(sql, commandType: CommandType.Text).FirstOrDefault();
            this.command = new DeleteNutritionCommand
            {
                Plus = new List<string> { nutritionItem.Plu }
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.transaction.Dispose();
            if (this.connectionProvider.Connection != null)
            {
                this.connectionProvider.Connection.Dispose();
            }
        }

        [TestMethod]
        public void DeleteNutrition_OnePluWithNutritionRecord_DeletesNutritionRecord()
        {
            // When
            this.commandHandler.Execute(this.command);

            // Then
            string plu = this.command.Plus.First();
            string sql = "SELECT Plu FROM nutrition.ItemNutrition WHERE Plu = @plu";
            var nutritionRecord = this.connectionProvider.Connection.Query<string>(sql, new { plu = plu });
            Assert.IsTrue(nutritionRecord.Count() == 0, $"The nutrition record for PLU {plu} was not deleted as expected.");
        }

        [TestMethod]
        public void DeleteNutrition_OnePluWithOneRegionSubscription_AddsDeleteNutritionEventQueueRecord()
        {
            // When
            this.commandHandler.Execute(this.command);

            // Then
            string plu = this.command.Plus.First();
            string eventQueueSql = "SELECT * FROM app.EventQueue WHERE EventMessage = @plu AND EventId = 14"; // EventId 14 = Delete Nutrition Event Type
            var events = this.connectionProvider.Connection.Query<dynamic>(eventQueueSql, new { plu = plu });

            string subscriptionSql = "SELECT RegionCode FROM app.IRMAItemSubscription WHERE identifier = @plu";
            var regionCodes = this.connectionProvider.Connection.Query<string>(subscriptionSql, new { plu = plu });

            Assert.IsTrue(events.Count() > 0, $"There were no delete nutrition events created in app.EventQueue for Plu {plu}.");
            foreach (var region in regionCodes)
            {
                Assert.IsTrue(events.Any(e => e.RegionCode == region), $"No delete nutrition event created in app.EventQueue for Plu {plu} for Region {region}.");
            }            
        }

        [TestMethod]
        public void DeleteNutrition_OnePluWithNutritionRecord_AddMessageQueueItemRecord()
        {
            // When
            this.commandHandler.Execute(this.command);

            // Then
            string plu = this.command.Plus.First();
            int itemId = this.connectionProvider.Connection.Query<int>("SELECT ItemId FROM ScanCode sc WHERE sc.scanCode = @plu", new { plu = plu }).First();
            string messageQueueSql = "SELECT * FROM esb.MessageQueueItem WHERE ItemId = @ItemId";
            var messageQueueItems = this.connectionProvider.Connection.Query<dynamic>(messageQueueSql, new { ItemId = itemId });

            Assert.IsTrue(messageQueueItems.Count() > 0, $"There were no esb.MessageQueueItem recordsd created in app.EventQueue for Plu {plu}, ItemId {itemId}.");
        }
    }
}
