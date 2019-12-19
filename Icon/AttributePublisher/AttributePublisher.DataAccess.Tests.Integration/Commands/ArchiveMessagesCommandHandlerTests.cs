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
    public class ArchiveMessagesCommandHandlerTests
    {
        private ArchiveMessagesCommandHandler commandHandler;
        private ArchiveMessagesCommand command;
        private SqlConnection sqlConnection;
        private TransactionScope transaction;
        private int attributeId;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
            commandHandler = new ArchiveMessagesCommandHandler(sqlConnection);
            command = new ArchiveMessagesCommand();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
            sqlConnection.Dispose();
        }

        [TestMethod]
        public void ArchiveMessages_1Message_ArchivesMessage()
        {
            //Given
            string messageId = "TestMessageId";
            command.Messages = new List<MessageArchiveModel>
            {
                new MessageArchiveModel
                {
                    AttributeModels = new List<MessageArchiveAttributeModel>
                    {
                        new MessageArchiveAttributeModel
                        {
                            AttributeId = 1,
                            MessageId = messageId,
                            MessageQueueAttributeJson = "Test"
                        }
                    },
                    Message = "<test>test</test>",
                    MessageHeaders = "Test Headers",
                    MessageId = messageId,
                    MessageTypeId = 14
                }
            };

            //When
            commandHandler.Execute(command);

            //Then
            var messageArchive = sqlConnection.QuerySingle<MessageArchiveModel>("SELECT * FROM esb.MessageArchive WHERE MessageId = @MessageId", new { MessageId = messageId });

            Assert.AreEqual(command.Messages[0].Message, messageArchive.Message);
            Assert.AreEqual(command.Messages[0].MessageHeaders, messageArchive.MessageHeaders);
            Assert.AreEqual(command.Messages[0].MessageId, messageArchive.MessageId);
            Assert.AreEqual(command.Messages[0].MessageTypeId, messageArchive.MessageTypeId);

            var messageModel = sqlConnection.QuerySingle<MessageArchiveAttributeModel>("SELECT * FROM esb.MessageQueueAttributeArchive WHERE MessageId = @MessageId", new { MessageId = messageId });

            Assert.AreEqual(command.Messages[0].AttributeModels[0].AttributeId, messageModel.AttributeId);
            Assert.AreEqual(command.Messages[0].AttributeModels[0].MessageId, messageModel.MessageId);
            Assert.AreEqual(command.Messages[0].AttributeModels[0].MessageQueueAttributeJson, messageModel.MessageQueueAttributeJson);
        }
    }
}
