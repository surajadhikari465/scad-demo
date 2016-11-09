using Icon.ApiController.DataAccess.Commands;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data.Entity;
using System.Linq;

namespace Icon.ApiController.Tests.Commands
{
    [TestClass]
    public class SaveToMessageHistoryCommandHandlerTests
    {
        private SaveToMessageHistoryCommandHandler saveToMessageHistoryCommandHandler;
        private Mock<ILogger<SaveToMessageHistoryCommandHandler>> mockLogger;
        private IconContext context;
        private GlobalIconContext globalContext;
        private DbContextTransaction transaction;
        
        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            globalContext = new GlobalIconContext(context);

            mockLogger = new Mock<ILogger<SaveToMessageHistoryCommandHandler>>();
            saveToMessageHistoryCommandHandler = new SaveToMessageHistoryCommandHandler(mockLogger.Object, globalContext);

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void SaveToMessageHistory_NewMessageToSave_MessageShouldBeSaved()
        {
            // Given.
            var messageToSave = new MessageHistory
            {
                MessageStatusId = MessageStatusTypes.Ready,
                MessageTypeId = MessageTypes.Product,
                Message = "Test",
                InsertDate = DateTime.Now
            };

            var command = new SaveToMessageHistoryCommand<MessageHistory>
            {
                Message = messageToSave
            };

            // When.
            saveToMessageHistoryCommandHandler.Execute(command);

            // Then.
            var savedMessage = context.MessageHistory.Single(mh => mh.MessageHistoryId == messageToSave.MessageHistoryId);

            Assert.AreEqual(MessageStatusTypes.Ready, savedMessage.MessageStatusId);
            Assert.AreEqual(MessageTypes.Product, savedMessage.MessageTypeId);
            Assert.AreEqual("Test", savedMessage.Message);
            Assert.AreEqual(DateTime.Now.Date, savedMessage.InsertDate.Date);
        }
    }
}
