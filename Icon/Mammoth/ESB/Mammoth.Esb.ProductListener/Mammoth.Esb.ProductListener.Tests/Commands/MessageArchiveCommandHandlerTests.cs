using Dapper;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Esb.ProductListener.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Mammoth.Esb.ProductListener.Tests.Commands
{
    [TestClass]
    public class MessageArchiveCommandHandlerTests
    {
        private MessageArchiveCommandHandler commandHandler;        
        private SqlDbProvider dbProvider;        

        [TestInitialize]
        public void Initialize()
        {
            dbProvider = new SqlDbProvider();
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            dbProvider.Connection.Open();
            dbProvider.Transaction = dbProvider.Connection.BeginTransaction();

            commandHandler = new MessageArchiveCommandHandler(dbProvider);            
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbProvider.Transaction.Rollback();
            dbProvider.Transaction.Dispose();
            dbProvider.Connection.Dispose();
        }

        [TestMethod]
        public void MessageArchive_WithFailedstatus_WithGlobalItem()
        {
            var messageId = "123";           
            var headerJson = new Dictionary<string, string>
            {
               { "IconMessageID", messageId},
               { "Source", "Icon" },
               { "TransactionType", "Global Item" },
               { "nonReceivingSysName", "" }
            };
            
            var messageArchiveCommand = new MessageArchiveCommand
            {                
                MessageId = messageId,
                MessageStatusId = 3,
                MessageTypeId = 8,
                InsertDateUtc = DateTime.UtcNow,
                MessageBody = System.IO.File.ReadAllText("TestMessages/MessageArchiveTestData.xml"),
                MessageHeadersJson = JsonConvert.SerializeObject(headerJson)
        };
            commandHandler.Execute(messageArchiveCommand);
            var message = dbProvider.Connection.Query<dynamic>(
               "SELECT * FROM esb.MessageArchive where MessageId  = @messageId",
               new {MessageId = messageId},
              transaction: dbProvider.Transaction).Single();

            Assert.AreEqual(messageArchiveCommand.MessageId, message.MessageID);
            Assert.AreEqual(messageArchiveCommand.MessageHeadersJson, message.MessageHeadersJson);            
        }
    }
}