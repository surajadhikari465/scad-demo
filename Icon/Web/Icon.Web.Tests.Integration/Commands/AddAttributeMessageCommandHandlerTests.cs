using Dapper;
using Icon.Common.Models;
using Icon.Web.DataAccess.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class AddAttributeMessageCommandHandlerTests
    {
        private AddAttributeMessageCommandHandler commandHandler;
        private AddAttributeMessageCommand command;
        private SqlConnection connection;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
            commandHandler = new AddAttributeMessageCommandHandler(connection);
            command = new AddAttributeMessageCommand();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
            connection.Dispose();
        }

        [TestMethod]
        public void AddAttributeMessage_Execute_AddsMessageToMessageQueue()
        {
            //When
            commandHandler.Execute(new AddAttributeMessageCommand { AttributeModel = new AttributeModel { AttributeId = 1000 } });

            //Then
            var attributeMessage = connection.QueryFirst("SELECT * FROM esb.MessageQueueAttribute WHERE AttributeId = 1000");
            Assert.IsNotNull(attributeMessage);
        }
    }
}
