using Icon.ApiController.DataAccess.Commands;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.ApiController.Tests.Commands
{
    [TestClass]
    public class UpdateMessageQueueStatusCommandHandlerTests
    {
        private UpdateMessageQueueStatusCommandHandler<MessageQueueItemLocale> updateMessageQueueStatusCommandHandler;
        private Mock<ILogger<UpdateMessageQueueStatusCommandHandler<MessageQueueItemLocale>>> mockLogger;
        private IconContext context;
        private GlobalIconContext globalContext;
        private DbContextTransaction transaction;
        private IRMAPush testIrmaPush;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            globalContext = new GlobalIconContext(context);

            mockLogger = new Mock<ILogger<UpdateMessageQueueStatusCommandHandler<MessageQueueItemLocale>>>();
            updateMessageQueueStatusCommandHandler = new UpdateMessageQueueStatusCommandHandler<MessageQueueItemLocale>(mockLogger.Object, globalContext);

            transaction = context.Database.BeginTransaction();

            testIrmaPush = new TestIrmaPushBuilder();
            context.IRMAPush.Add(testIrmaPush);
            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void UpdateMessageQueueStatus_NullList_WarningShouldBeLogged()
        {
            // Given.
            var command = new UpdateMessageQueueStatusCommand<MessageQueueItemLocale>
            {
                QueuedMessages = null,
                MessageStatusId = MessageStatusTypes.Associated
            };

            // When.
            updateMessageQueueStatusCommandHandler.Execute(command);

            // Then.
            mockLogger.Verify(l => l.Warn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void UpdateMessageQueueStatus_EmptyList_WarningShouldBeLogged()
        {
            // Given.
            var command = new UpdateMessageQueueStatusCommand<MessageQueueItemLocale>
            {
                QueuedMessages = new List<MessageQueueItemLocale>(),
                MessageStatusId = MessageStatusTypes.Associated
            };

            // When.
            updateMessageQueueStatusCommandHandler.Execute(command);

            // Then.
            mockLogger.Verify(l => l.Warn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void UpdateMessageQueueStatus_StatusChangeFromReadyToAssociated_QueueStatusShouldBeUpdated()
        {
            // Given.
            MessageQueueItemLocale queueToUpdate = new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPush.IRMAPushID);

            context.MessageQueueItemLocale.Add(queueToUpdate);
            context.SaveChanges();

            var command = new UpdateMessageQueueStatusCommand<MessageQueueItemLocale>
            {
                QueuedMessages = new List<MessageQueueItemLocale> { queueToUpdate },
                MessageStatusId = MessageStatusTypes.Associated
            };

            // When.
            updateMessageQueueStatusCommandHandler.Execute(command);

            // Then.
            var updatedQueue = context.MessageQueueItemLocale.Single(mq => mq.IRMAPushID == testIrmaPush.IRMAPushID);

            Assert.AreEqual(MessageStatusTypes.Associated, updatedQueue.MessageStatusId);
        }

        [TestMethod]
        public void UpdateMessageQueueStatus_StatusChangeFromReadyToAssociated_InProcessByShouldNotBeChanged()
        {
            // Given.
            int instance = 99;

            MessageQueueItemLocale queueToUpdate = new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPush.IRMAPushID).WithInProcessBy(instance);

            context.MessageQueueItemLocale.Add(queueToUpdate);
            context.SaveChanges();

            var command = new UpdateMessageQueueStatusCommand<MessageQueueItemLocale>
            {
                QueuedMessages = new List<MessageQueueItemLocale> { queueToUpdate },
                MessageStatusId = MessageStatusTypes.Associated
            };

            // When.
            updateMessageQueueStatusCommandHandler.Execute(command);

            // Then.
            var updatedQueue = context.MessageQueueItemLocale.Single(mq => mq.IRMAPushID == testIrmaPush.IRMAPushID);

            Assert.AreEqual(instance, updatedQueue.InProcessBy);
        }

        [TestMethod]
        public void UpdateMessageQueueStatus_StatusChangeFromReadyToFailed_InProcessByShouldBeNull()
        {
            // Given.
            MessageQueueItemLocale queueToUpdate = new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPush.IRMAPushID).WithInProcessBy(99);

            context.MessageQueueItemLocale.Add(queueToUpdate);
            context.SaveChanges();

            var command = new UpdateMessageQueueStatusCommand<MessageQueueItemLocale>
            {
                QueuedMessages = new List<MessageQueueItemLocale> { queueToUpdate },
                MessageStatusId = MessageStatusTypes.Associated,
                ResetInProcessBy = true
            };

            // When.
            updateMessageQueueStatusCommandHandler.Execute(command);

            // Then.
            var updatedQueue = context.MessageQueueItemLocale.Single(mq => mq.IRMAPushID == testIrmaPush.IRMAPushID);

            Assert.IsNull(updatedQueue.InProcessBy);
        }
    }
}
