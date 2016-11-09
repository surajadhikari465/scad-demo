using Icon.ApiController.Common;
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
    public class MarkUnsentMessagesAsInProcessCommandHandlerTests
    {
        private MarkUnsentMessagesAsInProcessCommandHandler markUnsentMessagesCommandHandler;
        private IconContext context;
        private GlobalIconContext globalContext;
        private DbContextTransaction transaction;
        private Mock<ILogger<MarkUnsentMessagesAsInProcessCommandHandler>> mockLogger;
        private List<MessageHistory> unsentMessages;
        private int miniBulkLimitMessageHistory;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            globalContext = new GlobalIconContext(context);

            mockLogger = new Mock<ILogger<MarkUnsentMessagesAsInProcessCommandHandler>>();
            markUnsentMessagesCommandHandler = new MarkUnsentMessagesAsInProcessCommandHandler(mockLogger.Object, globalContext);

            ControllerType.Instance = 99;
            miniBulkLimitMessageHistory = 100;

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void StageUnsentMessages()
        {
            context.MessageHistory.AddRange(unsentMessages);
            context.SaveChanges();
        }

        [TestMethod]
        public void MarkUnsentMessagesAsInProcess_MiniBulkLimitIsLessThanOne_ErrorShouldBeLogged()
        {
            // Given.
            var command = new MarkUnsentMessagesAsInProcessCommand
            {
                MiniBulkLimitMessageHistory = 0,
                MessageTypeId = MessageTypes.Product,
                Instance = ControllerType.Instance
            };

            // When.
            markUnsentMessagesCommandHandler.Execute(command);

            // Then.
            mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void MarkUnsentMessagesAsInProcess_OneMessageReady_OneMessageShouldBeMarkedAsInProcess()
        {
            // Given.
            unsentMessages = new List<MessageHistory>
            {
                new TestMessageHistoryBuilder()
            };

            StageUnsentMessages();

            var command = new MarkUnsentMessagesAsInProcessCommand
            {
                MiniBulkLimitMessageHistory = miniBulkLimitMessageHistory,
                MessageTypeId = MessageTypes.Product,
                Instance = ControllerType.Instance
            };

            // When.
            markUnsentMessagesCommandHandler.Execute(command);

            // Then.
            int unsentMessageId = unsentMessages[0].MessageHistoryId;
            var markedMessage = context.MessageHistory.Single(mh => mh.MessageHistoryId == unsentMessageId);

            // Have to reload the entity since the update was done via stored procedure.
            context.Entry(markedMessage).Reload();

            Assert.AreEqual(ControllerType.Instance, markedMessage.InProcessBy);
        }

        [TestMethod]
        public void MarkUnsentMessagesAsInProcess_OneMessageReadyWithOtherMessagesNotReady_OneMessageShouldBeMarkedAsInProcess()
        {
            // Given.
            unsentMessages = new List<MessageHistory>
            {
                new TestMessageHistoryBuilder().WithMessageStatusId(MessageStatusTypes.Ready),
                new TestMessageHistoryBuilder().WithMessageStatusId(MessageStatusTypes.Sent),
                new TestMessageHistoryBuilder().WithMessageStatusId(MessageStatusTypes.Failed),
                new TestMessageHistoryBuilder().WithMessageStatusId(MessageStatusTypes.Consumed),
            };

            StageUnsentMessages();

            var command = new MarkUnsentMessagesAsInProcessCommand
            {
                MiniBulkLimitMessageHistory = miniBulkLimitMessageHistory,
                MessageTypeId = MessageTypes.Product,
                Instance = ControllerType.Instance
            };

            // When.
            markUnsentMessagesCommandHandler.Execute(command);

            // Then.
            var unsentMessagesById = unsentMessages.Select(m => m.MessageHistoryId).ToList();
            var markedMessages = context.MessageHistory.Where(mh => unsentMessagesById.Contains(mh.MessageHistoryId)).ToList();

            // Have to reload the entities since the update was done via stored procedure.
            foreach (var entry in markedMessages)
            {
                context.Entry(entry).Reload();
            }

            Assert.AreEqual(ControllerType.Instance, markedMessages[0].InProcessBy);
            Assert.IsNull(markedMessages[1].InProcessBy);
            Assert.IsNull(markedMessages[2].InProcessBy);
            Assert.IsNull(markedMessages[3].InProcessBy);
        }

        [TestMethod]
        public void MarkUnsentMessagesAsInProcess_ThreeMessagesReadyAndThreeMessagesNotReady_ThreeMessagesShouldBeMarkedAsInProcess()
        {
            // Given.
            unsentMessages = new List<MessageHistory>
            {
                new TestMessageHistoryBuilder().WithMessageStatusId(MessageStatusTypes.Ready),
                new TestMessageHistoryBuilder().WithMessageStatusId(MessageStatusTypes.Ready),                
                new TestMessageHistoryBuilder().WithMessageStatusId(MessageStatusTypes.Ready),
                new TestMessageHistoryBuilder().WithMessageStatusId(MessageStatusTypes.Sent),
                new TestMessageHistoryBuilder().WithMessageStatusId(MessageStatusTypes.Failed),
                new TestMessageHistoryBuilder().WithMessageStatusId(MessageStatusTypes.Consumed)
            };

            StageUnsentMessages();

            var command = new MarkUnsentMessagesAsInProcessCommand
            {
                MiniBulkLimitMessageHistory = miniBulkLimitMessageHistory,
                MessageTypeId = MessageTypes.Product,
                Instance = ControllerType.Instance
            };

            // When.
            markUnsentMessagesCommandHandler.Execute(command);

            // Then.
            var unsentMessagesById = unsentMessages.Select(m => m.MessageHistoryId).ToList();
            var markedMessages = context.MessageHistory.Where(mh => unsentMessagesById.Contains(mh.MessageHistoryId)).ToList();

            // Have to reload the entities since the update was done via stored procedure.
            foreach (var entry in markedMessages)
            {
                context.Entry(entry).Reload();
            }

            Assert.AreEqual(ControllerType.Instance, markedMessages[0].InProcessBy);
            Assert.AreEqual(ControllerType.Instance, markedMessages[1].InProcessBy);
            Assert.AreEqual(ControllerType.Instance, markedMessages[2].InProcessBy);
            Assert.IsNull(markedMessages[3].InProcessBy);
            Assert.IsNull(markedMessages[4].InProcessBy);
            Assert.IsNull(markedMessages[5].InProcessBy);
        }

        [TestMethod]
        public void MarkUnsentMessagesAsInProcess_ThreeMessagesReadyAndThreeMessagesAlreadyInProcess_ThreeMessagesShouldBeMarkedAsInProcess()
        {
            // Given.
            unsentMessages = new List<MessageHistory>
            {
                new TestMessageHistoryBuilder().WithMessageStatusId(MessageStatusTypes.Ready).WithInProcessBy(null),
                new TestMessageHistoryBuilder().WithMessageStatusId(MessageStatusTypes.Ready).WithInProcessBy(null),                
                new TestMessageHistoryBuilder().WithMessageStatusId(MessageStatusTypes.Ready).WithInProcessBy(null),
                new TestMessageHistoryBuilder().WithMessageStatusId(MessageStatusTypes.Sent).WithInProcessBy(1),
                new TestMessageHistoryBuilder().WithMessageStatusId(MessageStatusTypes.Failed).WithInProcessBy(1),
                new TestMessageHistoryBuilder().WithMessageStatusId(MessageStatusTypes.Consumed).WithInProcessBy(1)
            };

            StageUnsentMessages();

            var command = new MarkUnsentMessagesAsInProcessCommand
            {
                MiniBulkLimitMessageHistory = miniBulkLimitMessageHistory,
                MessageTypeId = MessageTypes.Product,
                Instance = ControllerType.Instance
            };

            // When.
            markUnsentMessagesCommandHandler.Execute(command);

            // Then.
            var unsentMessagesById = unsentMessages.Select(m => m.MessageHistoryId).ToList();
            var markedMessages = context.MessageHistory.Where(mh => unsentMessagesById.Contains(mh.MessageHistoryId)).ToList();

            // Have to reload the entities since the update was done via stored procedure.
            foreach (var entry in markedMessages)
            {
                context.Entry(entry).Reload();
            }

            Assert.AreEqual(ControllerType.Instance, markedMessages[0].InProcessBy);
            Assert.AreEqual(ControllerType.Instance, markedMessages[1].InProcessBy);
            Assert.AreEqual(ControllerType.Instance, markedMessages[2].InProcessBy);
            Assert.AreEqual(1, markedMessages[3].InProcessBy);
            Assert.AreEqual(1, markedMessages[4].InProcessBy);
            Assert.AreEqual(1, markedMessages[5].InProcessBy);
        }

        [TestMethod]
        public void MarkUnsentMessagesAsInProcess_ThreeMessagesReadyAndThreeMessagesOfAnotherType_ThreeMessagesShouldBeMarkedAsInProcess()
        {
            // Given.
            unsentMessages = new List<MessageHistory>
            {
                new TestMessageHistoryBuilder().WithMessageStatusId(MessageStatusTypes.Ready).WithInProcessBy(null),
                new TestMessageHistoryBuilder().WithMessageStatusId(MessageStatusTypes.Ready).WithInProcessBy(null),                
                new TestMessageHistoryBuilder().WithMessageStatusId(MessageStatusTypes.Ready).WithInProcessBy(null),
                new TestMessageHistoryBuilder().WithMessageStatusId(MessageStatusTypes.Ready).WithInProcessBy(null).WithMessageTypeId(MessageTypes.ItemLocale),
                new TestMessageHistoryBuilder().WithMessageStatusId(MessageStatusTypes.Ready).WithInProcessBy(null).WithMessageTypeId(MessageTypes.ItemLocale),
                new TestMessageHistoryBuilder().WithMessageStatusId(MessageStatusTypes.Ready).WithInProcessBy(null).WithMessageTypeId(MessageTypes.ItemLocale)
            };

            StageUnsentMessages();

            var command = new MarkUnsentMessagesAsInProcessCommand
            {
                MiniBulkLimitMessageHistory = miniBulkLimitMessageHistory,
                MessageTypeId = MessageTypes.Product,
                Instance = ControllerType.Instance
            };

            // When.
            markUnsentMessagesCommandHandler.Execute(command);

            // Then.
            var unsentMessagesById = unsentMessages.Select(m => m.MessageHistoryId).ToList();
            var markedMessages = context.MessageHistory.Where(mh => unsentMessagesById.Contains(mh.MessageHistoryId)).ToList();

            // Have to reload the entities since the update was done via stored procedure.
            foreach (var entry in markedMessages)
            {
                context.Entry(entry).Reload();
            }

            Assert.AreEqual(ControllerType.Instance, markedMessages[0].InProcessBy);
            Assert.AreEqual(ControllerType.Instance, markedMessages[1].InProcessBy);
            Assert.AreEqual(ControllerType.Instance, markedMessages[2].InProcessBy);
            Assert.IsNull(markedMessages[3].InProcessBy);
            Assert.IsNull(markedMessages[4].InProcessBy);
            Assert.IsNull(markedMessages[5].InProcessBy);
        }

        [TestMethod]
        public void MarkUnsentMessagesAsInProcess_ThreeMessagesAlreadyInProcess_NoAdditionalMessagesShouldBeMarked()
        {
            // Given.
            unsentMessages = new List<MessageHistory>
            {
                new TestMessageHistoryBuilder().WithMessageStatusId(MessageStatusTypes.Ready).WithInProcessBy(ControllerType.Instance),
                new TestMessageHistoryBuilder().WithMessageStatusId(MessageStatusTypes.Ready).WithInProcessBy(ControllerType.Instance),                
                new TestMessageHistoryBuilder().WithMessageStatusId(MessageStatusTypes.Ready).WithInProcessBy(ControllerType.Instance)
            };

            StageUnsentMessages();

            var command = new MarkUnsentMessagesAsInProcessCommand
            {
                MiniBulkLimitMessageHistory = miniBulkLimitMessageHistory,
                MessageTypeId = MessageTypes.Product,
                Instance = ControllerType.Instance
            };

            // When.
            markUnsentMessagesCommandHandler.Execute(command);

            // Then.
            var markedMessages = context.MessageHistory.Where(mh => mh.InProcessBy == ControllerType.Instance).ToList();

            Assert.AreEqual(unsentMessages.Count, markedMessages.Count);
        }
    }
}
