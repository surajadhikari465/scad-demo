using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PushController.DataAccess.Commands;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PushController.Tests.DataAccess.Commands
{
    [TestClass]
    public class AddItemLocaleMessagesBulkCommandTests
    {
        private AddItemLocaleMessagesBulkCommandHandler addItemLocaleMessagesBulkCommandHandler;
        private GlobalIconContext context;
        private DbContextTransaction transaction;
        private Mock<ILogger<AddItemLocaleMessagesBulkCommandHandler>> mockLogger;
        private List<MessageQueueItemLocale> messagesToInsert;
        private IRMAPush irmaPushEntry;

        [TestInitialize]
        public void Initialize()
        {
            context = new GlobalIconContext(new IconContext());

            mockLogger = new Mock<ILogger<AddItemLocaleMessagesBulkCommandHandler>>();
            addItemLocaleMessagesBulkCommandHandler = new AddItemLocaleMessagesBulkCommandHandler(mockLogger.Object, context);

            transaction = context.Context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void SetupTestMessages()
        {
            messagesToInsert = new List<MessageQueueItemLocale>();
            irmaPushEntry = new TestIrmaPushBuilder();
            context.Context.IRMAPush.Add(irmaPushEntry);
            context.Context.SaveChanges();

            for (int i = 0; i < 100; i++)
            {
                var message = new TestItemLocaleMessageBuilder().WithIrmaPushId(irmaPushEntry.IRMAPushID);
                messagesToInsert.Add(message);
            }
        }

        [TestMethod]
        public void AddItemLocaleMessages_EmtpyList_WarningShouldBeLogged()
        {
            // Given.
            messagesToInsert = new List<MessageQueueItemLocale>();

            // When.
            var command = new AddItemLocaleMessagesBulkCommand
            {
                Messages = messagesToInsert
            };

            addItemLocaleMessagesBulkCommandHandler.Execute(command);

            // Then.
            mockLogger.Verify(l => l.Warn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void AddItemLocaleMessages_NullList_WarningShouldBeLogged()
        {
            // Given.
            messagesToInsert = null;

            // When.
            var command = new AddItemLocaleMessagesBulkCommand
            {
                Messages = messagesToInsert
            };

            addItemLocaleMessagesBulkCommandHandler.Execute(command);

            // Then.
            mockLogger.Verify(l => l.Warn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void AddItemLocaleMessages_ValidListOfMessages_MessagesShouldBeSavedToTheDatabase()
        {
            // Given.
            SetupTestMessages();

            // When.
            var command = new AddItemLocaleMessagesBulkCommand
            {
                Messages = messagesToInsert
            };

            addItemLocaleMessagesBulkCommandHandler.Execute(command);

            // Then.
            var insertedMessages = context.Context.MessageQueueItemLocale.Where(mq => mq.IRMAPushID == irmaPushEntry.IRMAPushID).ToList();
            Assert.AreEqual(messagesToInsert.Count, insertedMessages.Count);
        }
    }
}
