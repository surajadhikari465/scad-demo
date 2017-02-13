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
using System.Transactions;

namespace Icon.ApiController.Tests.Commands
{
    [TestClass]
    public class UpdateMessageQueueProcessedDateCommandHandlerTests
    {
        private IconContext context;
        private TransactionScope transaction;
        private UpdateMessageQueueProcessedDateCommandHandler<MessageQueueItemLocale> updateProcessedDateCommandHandler;
        private Mock<ILogger<UpdateMessageQueueProcessedDateCommandHandler<MessageQueueItemLocale>>> mockLogger;
        private IRMAPush testIrmaPush;
        private List<MessageQueueItemLocale> testQueuedMessages;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            context = new IconContext();

            mockLogger = new Mock<ILogger<UpdateMessageQueueProcessedDateCommandHandler<MessageQueueItemLocale>>>();

            updateProcessedDateCommandHandler = new UpdateMessageQueueProcessedDateCommandHandler<MessageQueueItemLocale>(
                mockLogger.Object,
                new IconDbContextFactory());

            testIrmaPush = new TestIrmaPushBuilder();
            context.IRMAPush.Add(testIrmaPush);
            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        private void StageTestQueuedMessages()
        {
            context.MessageQueueItemLocale.AddRange(testQueuedMessages);
            context.SaveChanges();
        }

        [TestMethod]
        public void UpdateProcessedDate_NullList_WarningShouldBeLogged()
        {
            // Given.
            testQueuedMessages = null;

            var command = new UpdateMessageQueueProcessedDateCommand<MessageQueueItemLocale>
            {
                ProcessedDate = DateTime.Now,
                MessagesToUpdate = testQueuedMessages
            };

            // When.
            updateProcessedDateCommandHandler.Execute(command);

            // Then.
            mockLogger.Verify(l => l.Warn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void UpdateProcessedDate_EmptyList_WarningShouldBeLogged()
        {
            // Given.
            testQueuedMessages = new List<MessageQueueItemLocale>();

            var command = new UpdateMessageQueueProcessedDateCommand<MessageQueueItemLocale>
            {
                ProcessedDate = DateTime.Now,
                MessagesToUpdate = testQueuedMessages
            };

            // When.
            updateProcessedDateCommandHandler.Execute(command);

            // Then.
            mockLogger.Verify(l => l.Warn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void UpdateProcessedDate_OneMessageToUpdate_ProcessedDateShouldBeUpdatedForThatMessage()
        {
            // Given.
            testQueuedMessages = new List<MessageQueueItemLocale>
            {
                new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPush.IRMAPushID)
            };

            StageTestQueuedMessages();

            var command = new UpdateMessageQueueProcessedDateCommand<MessageQueueItemLocale>
            {
                ProcessedDate = DateTime.Now,
                MessagesToUpdate = testQueuedMessages
            };

            // When.
            updateProcessedDateCommandHandler.Execute(command);

            // Then.
            var updatedQueuedMessage = context.MessageQueueItemLocale.Single(mq => mq.IRMAPushID == testIrmaPush.IRMAPushID);

            // Have to reload the entity since the update was done via stored procedure.
            context.Entry(updatedQueuedMessage).Reload();

            Assert.AreEqual(DateTime.Now.Date, updatedQueuedMessage.ProcessedDate.Value.Date);
        }

        [TestMethod]
        public void UpdateProcessedDate_ThreeMessagesToUpdate_ProcessedDateShouldBeUpdatedForThreeMessages()
        {
            // Given.
            testQueuedMessages = new List<MessageQueueItemLocale>
            {
                new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPush.IRMAPushID).WithInProcessBy(1),
                new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPush.IRMAPushID).WithInProcessBy(1),
                new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPush.IRMAPushID).WithInProcessBy(1)
            };

            StageTestQueuedMessages();

            var command = new UpdateMessageQueueProcessedDateCommand<MessageQueueItemLocale>
            {
                ProcessedDate = DateTime.Now,
                MessagesToUpdate = testQueuedMessages
            };

            // When.
            updateProcessedDateCommandHandler.Execute(command);

            // Then.
            var updatedQueuedMessages = context.MessageQueueItemLocale.Where(mq => mq.IRMAPushID == testIrmaPush.IRMAPushID).ToList();

            // Have to reload the entity since the update was done via stored procedure.
            foreach (var message in updatedQueuedMessages)
            {
                context.Entry(message).Reload();
            }

            bool allQueuedMessagesWereUpdated = updatedQueuedMessages.TrueForAll(m => 
                m.ProcessedDate.Value.Date == DateTime.Now.Date && m.InProcessBy == null);

            Assert.IsTrue(allQueuedMessagesWereUpdated);
        }
    }
}
