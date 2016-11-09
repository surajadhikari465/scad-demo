using Icon.ApiController.DataAccess.Commands;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.ApiController.Tests.Commands
{
    [TestClass]
    public class AssociateMessageToQueueBulkCommandHandlerTests
    {
        private IconContext context;
        private DbContextTransaction transaction;
        private AssociateMessageToQueueCommandHandler<MessageQueueItemLocale> associateMessageToQueueCommandHandler;
        private Mock<ILogger<AssociateMessageToQueueCommandHandler<MessageQueueItemLocale>>> mockLogger;
        private int testMessageHistoryId;
        private int testIrmaPushId;
        private List<MessageQueueItemLocale> testQueuedMessages;
        private List<int> testMessageQueueId;
        private MessageHistory messageHistory;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            mockLogger = new Mock<ILogger<AssociateMessageToQueueCommandHandler<MessageQueueItemLocale>>>();

            associateMessageToQueueCommandHandler = new AssociateMessageToQueueCommandHandler<MessageQueueItemLocale>(
                mockLogger.Object,
                new GlobalIconContext(context));

            transaction = context.Database.BeginTransaction();

            // Insert a MessageHistory and IRMAPush first because of FK constraints.
            messageHistory = new MessageHistory
            {
                MessageTypeId = MessageTypes.ItemLocale,
                MessageStatusId = MessageStatusTypes.Ready,
                Message = String.Empty,
                InProcessBy = null,
                ProcessedDate = null
            };

            context.MessageHistory.Add(messageHistory);
            context.SaveChanges();

            testMessageHistoryId = messageHistory.MessageHistoryId;

            IRMAPush irmaPush = new TestIrmaPushBuilder();
            context.IRMAPush.Add(irmaPush);
            context.SaveChanges();

            testIrmaPushId = irmaPush.IRMAPushID;

            // Now create a list of queued messages.
            testQueuedMessages = new List<MessageQueueItemLocale>
            {
                new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPushId),
                new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPushId)
            };

            context.MessageQueueItemLocale.AddRange(testQueuedMessages);
            context.SaveChanges();

            testMessageQueueId = new List<int>
            {
                testQueuedMessages[0].MessageQueueId,
                testQueuedMessages[1].MessageQueueId
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void AssociateMessageToQueueExecute_NullList_WarningShouldBeLogged()
        {
            // Given.
            var command = new AssociateMessageToQueueCommand<MessageQueueItemLocale, MessageHistory>
            {
                MessageHistory = messageHistory,
                QueuedMessages = null
            };

            // When.
            associateMessageToQueueCommandHandler.Execute(command);

            // Then.
            mockLogger.Verify(l => l.Warn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void AssociateMessageToQueueExecute_EmptyList_WarningShouldBeLogged()
        {
            // Given.
            var command = new AssociateMessageToQueueCommand<MessageQueueItemLocale, MessageHistory>
            {
                MessageHistory = messageHistory,
                QueuedMessages = new List<MessageQueueItemLocale>()
            };

            // When.
            associateMessageToQueueCommandHandler.Execute(command);

            // Then.
            mockLogger.Verify(l => l.Warn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void AssociateMessageToQueueExecute_MessageIsAssociatedToQueue_QueueEntriesShouldBeAssociatedToMessage()
        {
            // Given.
            var command = new AssociateMessageToQueueCommand<MessageQueueItemLocale, MessageHistory>
            {
                MessageHistory = messageHistory,
                QueuedMessages = testQueuedMessages
            };

            // When.
            associateMessageToQueueCommandHandler.Execute(command);

            // Then.
            var queuedMessages = context.MessageQueueItemLocale.Where(mq => mq.IRMAPushID == testIrmaPushId).ToList();

            // Have to reload the entity since the update was done via stored procedure.
            foreach (var updatedQueuedMessage in queuedMessages)
            {
                context.Entry(updatedQueuedMessage).Reload();
            }
            
            bool hasAssociationToMessage = queuedMessages.TrueForAll(m => m.MessageHistoryId == testMessageHistoryId);

            Assert.IsTrue(hasAssociationToMessage);
        }

        [TestMethod]
        public void AssociateMessageToQueueExecute_MessageIsAssociatedToQueue_QueueEntriesShouldHaveAssociatedStatus()
        {
            // Given.
            var command = new AssociateMessageToQueueCommand<MessageQueueItemLocale, MessageHistory>
            {
                MessageHistory = messageHistory,
                QueuedMessages = testQueuedMessages
            };

            // When.
            associateMessageToQueueCommandHandler.Execute(command);

            // Then.
            var queuedMessages = context.MessageQueueItemLocale.Where(mq => mq.IRMAPushID == testIrmaPushId).ToList();

            // Have to reload the entity since the update was done via stored procedure.
            foreach (var updatedQueuedMessage in queuedMessages)
            {
                context.Entry(updatedQueuedMessage).Reload();
            }

            bool hasAssociatedStatus = queuedMessages.TrueForAll(m => m.MessageStatusId == MessageStatusTypes.Associated);

            Assert.IsTrue(hasAssociatedStatus);
        }
    }
}
