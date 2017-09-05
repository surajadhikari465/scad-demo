using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSupport.DataAccess.Commands;
using Mammoth.Framework;
using System.Transactions;
using Newtonsoft.Json;

namespace WebSupport.DataAccess.Test.Commands
{
    [TestClass]
    public class SaveSentMessageCommandHandlerTests
    {
        private SaveSentMessageCommandHandler commandHandler;
        private SaveSentMessageCommand command;
        private MammothContext context;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            context = new MammothContext();

            commandHandler = new SaveSentMessageCommandHandler(context);
            command = new SaveSentMessageCommand();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void SaveSentMessage_MessageExists_SaveMessageToTheDatabase()
        {
            //Given
            command.Message = "<test>test</test>";
            command.MessageId = Guid.NewGuid();
            command.MessageProperties = new Dictionary<string, string>
            {
                { "PriceResetMessage", true.ToString() },
                { "Test Key", "Test Value" }
            };

            //When
            commandHandler.Execute(command);

            //Then
            var priceResetMessageHistory = context.PriceResetMessageHistories.First(prmh => prmh.MessageId == command.MessageId);
            Assert.AreEqual(command.Message, priceResetMessageHistory.Message);
            Assert.AreEqual(command.MessageId, priceResetMessageHistory.MessageId);
            Assert.AreEqual(JsonConvert.SerializeObject(command.MessageProperties), priceResetMessageHistory.MessageProperties);
        }
    }
}
