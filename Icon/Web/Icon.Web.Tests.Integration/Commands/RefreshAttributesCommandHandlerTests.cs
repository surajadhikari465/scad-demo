using System.Collections.Generic;
using System.Linq;
using Dapper;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Models;
using Icon.Web.Tests.Integration.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class RefreshAttributesCommandHandlerTests
    {
        private IDbProvider db;
        private RefreshAttributesCommandHandler commandHandler;
        private RefreshAttributesCommand command;

        [TestInitialize]
        public void Initialize()
        {
            db = new SqlDbProvider();
            db.Connection = SqlConnectionBuilder.CreateIconConnection();
            db.Connection.Open();

            commandHandler = new RefreshAttributesCommandHandler(db);
        }

        [TestMethod]
        public void RefreshAttributes_CreatesSingleDatabaseRecordInMessageQueueAttributeTable()
        {
            //Given
            string attributeId = "1";
            command = new RefreshAttributesCommand
            {
                AttributeIds = new List<string>(new string[] { attributeId })
            };

            string sql = @"
                SELECT 
                    [MessageQueueAttributeId]
                    ,[AttributeId]
                    ,[InsertDateUtc]
                FROM esb.MessageQueueAttribute
                WHERE AttributeId = @attributeId";

            var queued = db.Connection.Query<MessageQueueAttributeModel>(sql,
                new
                {
                    attributeId
                });
            int preCount = queued.Count();

            //When
            commandHandler.Execute(command);

            //Then
            queued = db.Connection.Query<MessageQueueAttributeModel>(sql,
                new
                {
                    attributeId
                });

            Assert.AreEqual(preCount + 1, queued.Count(), $"Expected one new entry in database, found {queued.Count() - preCount}");
        }

        [TestMethod]
        public void RefreshAttributes_CreatesMultipleDatabaseRecordInMessageQueueAttributeTable()
        {
            //Given
            List<string> attributeIds = new List<string>(new string[] { "1", "2", "3" });
            command = new RefreshAttributesCommand
            {
                AttributeIds = attributeIds
            };

            string joined = string.Join(", ", attributeIds.ToArray());
            string sql = $@"
                SELECT 
                    [MessageQueueAttributeId]
                    ,[AttributeId]
                    ,[InsertDateUtc]
                FROM esb.MessageQueueAttribute
                WHERE AttributeId in ({joined})";
            var queued = db.Connection.Query<MessageQueueAttributeModel>(sql);
            int preCount = queued.Count();

            //When
            commandHandler.Execute(command);

            //Then
            queued = db.Connection.Query<MessageQueueAttributeModel>(sql);

            Assert.AreEqual(preCount + attributeIds.Count, queued.Count(), $"Expected {attributeIds.Count} additional entries in database, found {queued.Count() - preCount}");
        }
    }
}
