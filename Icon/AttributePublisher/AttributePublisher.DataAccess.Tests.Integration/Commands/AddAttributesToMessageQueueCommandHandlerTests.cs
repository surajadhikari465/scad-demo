using AttributePublisher.DataAccess.Commands;
using AttributePublisher.DataAccess.Models;
using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;

namespace AttributePublisher.DataAccess.Tests.Integration.Commands
{
    [TestClass]
    public class AddAttributesToMessageQueueCommandHandlerTests
    {
        private AddAttributesToMessageQueueCommandHandler commandHandler;
        private SqlConnection sqlConnection;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
            commandHandler = new AddAttributesToMessageQueueCommandHandler(sqlConnection);
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
            sqlConnection.Dispose();
        }

        [TestMethod]
        public void AddAttributesToMessageQueue_Execute_AddsAttributesToMessageQueue()
        {
            //When
            commandHandler.Execute(new AddAttributesToMessageQueueCommand
            {
                Attributes = new List<AttributeModel>
                {
                    new AttributeModel { AttributeId = 100000 },
                    new AttributeModel { AttributeId = 100001 },
                    new AttributeModel { AttributeId = 100002 },
                    new AttributeModel { AttributeId = 100003 },
                }
            });

            //Then
            var queuedAttributesCount = sqlConnection.QueryFirst<int>("SELECT COUNT(*) FROM esb.MessageQueueAttribute WHERE AttributeId IN (100000, 100001, 100002, 100003)");
            Assert.AreEqual(4, queuedAttributesCount);
        }
    }
}
