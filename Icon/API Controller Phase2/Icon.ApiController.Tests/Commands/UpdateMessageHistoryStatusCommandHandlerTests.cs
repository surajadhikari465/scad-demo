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
    public class UpdateMessageHistoryStatusCommandHandlerTests
    {
        private UpdateMessageHistoryStatusCommandHandler updateMessageHistoryStatusCommandHandler;
        private Mock<ILogger<UpdateMessageHistoryStatusCommandHandler>> mockLogger;
        private IconContext context;
        private GlobalIconContext globalContext;
        private DbContextTransaction transaction;
        
        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            globalContext = new GlobalIconContext(context);

            mockLogger = new Mock<ILogger<UpdateMessageHistoryStatusCommandHandler>>();
            updateMessageHistoryStatusCommandHandler = new UpdateMessageHistoryStatusCommandHandler(mockLogger.Object, globalContext);

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void UpdateMessageHistoryStatus_StatusChangeFromReadyToSent_MessageStatusShouldBeUpdated()
        {
            // Given.
            var messageToUpdate = new MessageHistory
            {
                MessageStatusId = MessageStatusTypes.Ready,
                MessageTypeId = MessageTypes.Product,
                Message = "Test",
                InsertDate = DateTime.Now
            };

            context.MessageHistory.Add(messageToUpdate);
            context.SaveChanges();

            var command = new UpdateMessageHistoryStatusCommand<MessageHistory>
            {
                Message = messageToUpdate,
                MessageStatusId = MessageStatusTypes.Sent
            };

            // When.
            updateMessageHistoryStatusCommandHandler.Execute(command);

            // Then.
            var updatedMessage = context.MessageHistory.Single(mh => mh.MessageHistoryId == messageToUpdate.MessageHistoryId);

            Assert.AreEqual(MessageStatusTypes.Sent, updatedMessage.MessageStatusId);
            Assert.IsNull(updatedMessage.InProcessBy);
            Assert.AreEqual(DateTime.Now.Date, updatedMessage.ProcessedDate.Value.Date);
        }
    }
}
