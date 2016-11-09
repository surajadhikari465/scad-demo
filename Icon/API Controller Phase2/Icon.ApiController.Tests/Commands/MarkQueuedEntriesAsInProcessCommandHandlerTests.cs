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
    public class MarkQueuedEntriesAsInProcessCommandHandlerTests
    {
        private MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueItemLocale> markQueuedEntriesCommandHandler;
        private IconContext context;
        private GlobalIconContext globalContext;
        private DbContextTransaction transaction;
        private Mock<ILogger<MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueItemLocale>>> mockLogger;
        private IRMAPush testIrmaPush;
        private List<MessageQueueItemLocale> testQueuedMessages;
        private int lookAhead;
        private int businessUnitId;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            globalContext = new GlobalIconContext(context);

            mockLogger = new Mock<ILogger<MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueItemLocale>>>();
            markQueuedEntriesCommandHandler = new MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueItemLocale>(mockLogger.Object, globalContext);

            ControllerType.Instance = 99;
            lookAhead = 1000;
            businessUnitId = 10101;

            transaction = context.Database.BeginTransaction();

            // Because of the order in which the records are marked as InProcessBy, this test class may not execute successfully unless the 
            // MessageQueueItemLocale table is empty.

            // Rolling back the transaction in the Cleanup method will restore any truncated records after the test finishes.
            context.Database.ExecuteSqlCommand("truncate table app.MessageQueueItemLocale");

            testIrmaPush = new TestIrmaPushBuilder();
            context.IRMAPush.Add(testIrmaPush);
            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void StageTestQueuedMessages()
        {
            context.MessageQueueItemLocale.AddRange(testQueuedMessages);
            context.SaveChanges();
        }

        [TestMethod]
        public void MarkQueuedEntriesAsInProcess_LookAheadValueIsZero_ErrorShouldBeLogged()
        {
            // Given.
            var command = new MarkQueuedEntriesAsInProcessCommand<MessageQueueItemLocale>
            {
                Instance = ControllerType.Instance,
                LookAhead = default(int)
            };

            // When.
            markQueuedEntriesCommandHandler.Execute(command);

            // Then.
            mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void MarkQueuedEntriesAsInProcess_OneMessageReady_OneMessageShouldBeMarkedAsInProcess()
        {
            // Given.
            testQueuedMessages = new List<MessageQueueItemLocale>
            {
                new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPush.IRMAPushID).WithBusinessUnitId(businessUnitId)
            };

            StageTestQueuedMessages();

            var command = new MarkQueuedEntriesAsInProcessCommand<MessageQueueItemLocale>
            {
                Instance = ControllerType.Instance,
                LookAhead = lookAhead,
                BusinessUnit = businessUnitId
            };

            // When.
            markQueuedEntriesCommandHandler.Execute(command);

            // Then.
            var markedQueueEntry = context.MessageQueueItemLocale.Single(mq => mq.IRMAPushID == testIrmaPush.IRMAPushID);

            // Have to reload the entity since the update was done via stored procedure.
            context.Entry(markedQueueEntry).Reload();

            Assert.AreEqual(ControllerType.Instance, markedQueueEntry.InProcessBy);
        }

        [TestMethod]
        public void MarkQueuedEntriesAsInProcess_OneMessageReadyAndOneMessageNotReady_OneMessageShouldBeMarkedAsInProcess()
        {
            // Given.
            testQueuedMessages = new List<MessageQueueItemLocale>
            {
                new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPush.IRMAPushID).WithBusinessUnitId(businessUnitId),
                new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPush.IRMAPushID).WithBusinessUnitId(businessUnitId).WithMessageStatusId(MessageStatusTypes.Associated)
            };

            StageTestQueuedMessages();

            var command = new MarkQueuedEntriesAsInProcessCommand<MessageQueueItemLocale>
            {
                Instance = ControllerType.Instance,
                LookAhead = lookAhead,
                BusinessUnit = businessUnitId
            };

            // When.
            markQueuedEntriesCommandHandler.Execute(command);

            // Then.
            var markedQueueEntries = context.MessageQueueItemLocale.Where(mq => mq.IRMAPushID == testIrmaPush.IRMAPushID).ToList();

            // Have to reload the entities since the update was done via stored procedure.
            foreach (var entry in markedQueueEntries)
            {
                context.Entry(entry).Reload();
            }

            Assert.AreEqual(ControllerType.Instance, markedQueueEntries[0].InProcessBy);
            Assert.IsNull(markedQueueEntries[1].InProcessBy);
        }

        [TestMethod]
        public void MarkQueuedEntriesAsInProcess_OneMessagePreviouslyMarked_OneMessageShouldBeMarkedAsInProcess()
        {
            // Given.
            var markQueuedEntriesCommandHandler = new MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueItemLocale>(
                mockLogger.Object,
                globalContext);

            testQueuedMessages = new List<MessageQueueItemLocale>
            {
                new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPush.IRMAPushID).WithInProcessBy(ControllerType.Instance)
            };

            StageTestQueuedMessages();

            var command = new MarkQueuedEntriesAsInProcessCommand<MessageQueueItemLocale>
            {
                Instance = ControllerType.Instance,
                LookAhead = lookAhead
            };

            // When.
            markQueuedEntriesCommandHandler.Execute(command);

            // Then.
            var markedQueueEntry = context.MessageQueueItemLocale.Single(mq => mq.IRMAPushID == testIrmaPush.IRMAPushID);

            // Have to reload the entity since the update was done via stored procedure.
            context.Entry(markedQueueEntry).Reload();

            Assert.AreEqual(ControllerType.Instance, markedQueueEntry.InProcessBy);
        }

        [TestMethod]
        public void MarkQueuedEntriesAsInProcess_ThreeMessagesReadyAndThreeMessagesNotReady_ThreeMessagesShouldBeMarkedAsInProcess()
        {
            // Given.
            var markQueuedEntriesCommandHandler = new MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueItemLocale>(
                mockLogger.Object,
                globalContext);

            testQueuedMessages = new List<MessageQueueItemLocale>
            {
                new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPush.IRMAPushID).WithBusinessUnitId(businessUnitId),
                new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPush.IRMAPushID).WithBusinessUnitId(businessUnitId),
                new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPush.IRMAPushID).WithBusinessUnitId(businessUnitId),
                new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPush.IRMAPushID).WithBusinessUnitId(businessUnitId).WithMessageStatusId(MessageStatusTypes.Associated),
                new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPush.IRMAPushID).WithBusinessUnitId(businessUnitId).WithMessageStatusId(MessageStatusTypes.Failed),
                new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPush.IRMAPushID).WithBusinessUnitId(businessUnitId).WithMessageStatusId(MessageStatusTypes.Staged)
            };

            StageTestQueuedMessages();

            var command = new MarkQueuedEntriesAsInProcessCommand<MessageQueueItemLocale>
            {
                Instance = ControllerType.Instance,
                LookAhead = lookAhead,
                BusinessUnit = businessUnitId
            };

            // When.
            markQueuedEntriesCommandHandler.Execute(command);

            // Then.
            var markedQueueEntries = context.MessageQueueItemLocale.Where(mq => mq.IRMAPushID == testIrmaPush.IRMAPushID).ToList();

            // Have to reload the entities since the update was done via stored procedure.
            foreach (var entry in markedQueueEntries)
            {
                context.Entry(entry).Reload();
            }

            Assert.AreEqual(ControllerType.Instance, markedQueueEntries[0].InProcessBy);
            Assert.AreEqual(ControllerType.Instance, markedQueueEntries[1].InProcessBy);
            Assert.AreEqual(ControllerType.Instance, markedQueueEntries[2].InProcessBy);
            Assert.IsNull(markedQueueEntries[3].InProcessBy);
            Assert.IsNull(markedQueueEntries[4].InProcessBy);
            Assert.IsNull(markedQueueEntries[5].InProcessBy);
        }

        [TestMethod]
        public void MarkQueuedEntriesAsInProcess_OneMessageIsReadyAndBusinessUnitEqualsBusinessUnitId_OneMessageShouldBeMarkedAsInProcess()
        {
            // Given.
            var markQueuedEntriesCommandHandler = new MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueItemLocale>(
                mockLogger.Object,
                globalContext);

            testQueuedMessages = new List<MessageQueueItemLocale>
            {
                new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPush.IRMAPushID).WithBusinessUnitId(businessUnitId)
            };

            StageTestQueuedMessages();

            var command = new MarkQueuedEntriesAsInProcessCommand<MessageQueueItemLocale>
            {
                Instance = ControllerType.Instance,
                LookAhead = lookAhead,
                BusinessUnit = businessUnitId
            };

            // When.
            markQueuedEntriesCommandHandler.Execute(command);

            // Then.
            var markedQueueEntry = context.MessageQueueItemLocale.Single(mq => mq.IRMAPushID == testIrmaPush.IRMAPushID);

            // Have to reload the entity since the update was done via stored procedure.
            context.Entry(markedQueueEntry).Reload();

            Assert.AreEqual(ControllerType.Instance, markedQueueEntry.InProcessBy);
        }

        [TestMethod]
        public void MarkQueuedEntriesAsInProcess_OneMessageIsReadyAndBusinessUnitIsNot_MessageShouldNotBeMarkedAsInProcess()
        {
            // Given.
            var markQueuedEntriesCommandHandler = new MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueItemLocale>(
                mockLogger.Object,
                globalContext);

            testQueuedMessages = new List<MessageQueueItemLocale>
            {
                new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPush.IRMAPushID).WithBusinessUnitId(businessUnitId + 1)
            };

            StageTestQueuedMessages();

            var command = new MarkQueuedEntriesAsInProcessCommand<MessageQueueItemLocale>
            {
                Instance = ControllerType.Instance,
                LookAhead = lookAhead,
                BusinessUnit = businessUnitId
            };

            // When.
            markQueuedEntriesCommandHandler.Execute(command);

            // Then.
            var markedQueueEntry = context.MessageQueueItemLocale.Single(mq => mq.IRMAPushID == testIrmaPush.IRMAPushID);

            // Have to reload the entity since the update was done via stored procedure.
            context.Entry(markedQueueEntry).Reload();

            Assert.IsNull(markedQueueEntry.InProcessBy);
        }

        [TestMethod]
        public void MarkQueuedEntriesAsInProcess_CurrentInProcessRecordsAreLessThanLookAheadValue_AdditionalMessagesShouldBeMarkedToMatchLookAhead()
        {
            // Given.
            var markQueuedEntriesCommandHandler = new MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueItemLocale>(
                mockLogger.Object,
                globalContext);

            testQueuedMessages = new List<MessageQueueItemLocale>
            {
                new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPush.IRMAPushID).WithInProcessBy(ControllerType.Instance).WithBusinessUnitId(businessUnitId),
                new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPush.IRMAPushID).WithInProcessBy(ControllerType.Instance).WithBusinessUnitId(businessUnitId),
                new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPush.IRMAPushID).WithInProcessBy(ControllerType.Instance).WithBusinessUnitId(businessUnitId),
                new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPush.IRMAPushID).WithInProcessBy(null).WithBusinessUnitId(businessUnitId),
                new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPush.IRMAPushID).WithInProcessBy(null).WithBusinessUnitId(businessUnitId),
                new TestItemLocaleMessageBuilder().WithIrmaPushId(testIrmaPush.IRMAPushID).WithInProcessBy(null).WithBusinessUnitId(businessUnitId)
            };

            StageTestQueuedMessages();

            var command = new MarkQueuedEntriesAsInProcessCommand<MessageQueueItemLocale>
            {
                Instance = ControllerType.Instance,
                LookAhead = lookAhead,
                BusinessUnit = businessUnitId
            };

            // When.
            markQueuedEntriesCommandHandler.Execute(command);

            // Then.
            var markedQueueEntries = context.MessageQueueItemLocale.Where(mq => mq.InProcessBy == ControllerType.Instance).ToList();

            Assert.AreEqual(testQueuedMessages.Count, markedQueueEntries.Count);
        }
    }
}
