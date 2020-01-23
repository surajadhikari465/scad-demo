using Dapper;
using Icon.Esb.Schemas.Wfm.Contracts;
using Mammoth.Common.DataAccess;
using Mammoth.PrimeAffinity.Library.MessageBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;

namespace Mammoth.PrimeAffinity.Library.Commands.Tests.Queries
{
    [TestClass]
    public class ArchivePrimeAffinityMessageCommandHandlerTests
    {
        private ArchivePrimeAffinityMessageCommandHandler commandHandler;
        private ArchivePrimeAffinityMessageCommand command;
        private SqlConnection sqlConnection;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            commandHandler = new ArchivePrimeAffinityMessageCommandHandler(sqlConnection);
            command = new ArchivePrimeAffinityMessageCommand();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void ArchivePrimeAffinityMessage_MessageExist_ShouldArchiveMessageAndPriceModels()
        {
            //Given
            command.Message = "<test>test</test>";
            command.MessageHeadersJson = "TestJson";
            command.MessageId = Guid.NewGuid().ToString();
            command.MessageStatusId = MessageStatusTypes.Ready;
            command.PrimeAffinityMessageModels = new List<PrimeAffinityMessageModel>
            {
                new PrimeAffinityMessageModel
                {
                    BusinessUnitID = 1234,
                    ErrorCode = null,
                    ErrorDetails = null,
                    ItemID = 12345,
                    ItemTypeCode = "RTL",
                    MessageAction = ActionEnum.AddOrUpdate,
                    Region = "FL",
                    ScanCode = "123456",
                    StoreName = "Test",
                    InternalPriceObject = new { Prop1 = "TestProp1", Prop2 = "TestProp2"}
                },
                new PrimeAffinityMessageModel
                {
                    BusinessUnitID = 12345,
                    ErrorCode = null,
                    ErrorDetails = null,
                    ItemID = 123456,
                    ItemTypeCode = "RTL",
                    MessageAction = ActionEnum.AddOrUpdate,
                    Region = "FL",
                    ScanCode = "123456",
                    StoreName = "Test",
                    InternalPriceObject = new { Prop1 = "TestProp1", Prop2 = "TestProp2"}
                }
            };

            //When
            commandHandler.Execute(command);

            //Then
            var archivedMessage = sqlConnection.QuerySingle(
                "SELECT * FROM esb.MessageArchive WHERE MessageID = @MessageId",
                command);

            Assert.AreEqual(command.Message, archivedMessage.MessageBody);
            Assert.AreEqual(command.MessageHeadersJson, archivedMessage.MessageHeadersJson);
            Assert.AreEqual(command.MessageId, archivedMessage.MessageID);
            Assert.AreEqual(command.MessageStatusId, archivedMessage.MessageStatusID);

            var archivedPrice1 = sqlConnection.QuerySingle(
                "SELECT * FROM esb.MessageArchiveDetailPrimePsg WHERE ItemID = @ItemID AND BusinessUnitID = @BusinessUnitID",
                command.PrimeAffinityMessageModels[0]);
            AssertPriceIsEqualToArchivedPrice(command.MessageId, command.PrimeAffinityMessageModels[0], archivedPrice1);

            var archivedPrice2 = sqlConnection.QuerySingle(
                "SELECT * FROM esb.MessageArchiveDetailPrimePsg WHERE ItemID = @ItemID AND BusinessUnitID = @BusinessUnitID",
                command.PrimeAffinityMessageModels[1]);
            AssertPriceIsEqualToArchivedPrice(command.MessageId, command.PrimeAffinityMessageModels[1], archivedPrice2);
        }

        private void AssertPriceIsEqualToArchivedPrice(string messasgeId, PrimeAffinityMessageModel priceModel, dynamic archivedPrice)
        {
            Assert.AreEqual(priceModel.MessageAction.ToString(), archivedPrice.MessageAction);
            Assert.AreEqual(priceModel.Region, archivedPrice.Region);
            Assert.AreEqual(priceModel.ItemID, archivedPrice.ItemID);
            Assert.AreEqual(priceModel.BusinessUnitID, archivedPrice.BusinessUnitID);
            Assert.AreEqual(messasgeId, archivedPrice.MessageID);
            Assert.AreEqual(JsonConvert.SerializeObject(priceModel), archivedPrice.JsonObject);
            Assert.AreEqual(priceModel.ErrorCode, archivedPrice.ErrorCode);
            Assert.AreEqual(priceModel.ErrorDetails, archivedPrice.ErrorDetails);
        }
    }
}
