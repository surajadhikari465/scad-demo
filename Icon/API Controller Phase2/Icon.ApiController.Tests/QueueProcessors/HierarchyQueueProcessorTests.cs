using Icon.ApiController.Common;
using Icon.ApiController.Controller.Monitoring;
using Icon.ApiController.Controller.QueueProcessors;
using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.Controller.Serializers;
using Icon.ApiController.DataAccess.Commands;
using Icon.ApiController.DataAccess.Queries;
using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Esb.Producer;
using Icon.Framework;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;
using Icon.ActiveMQ.Producer;

namespace Icon.ApiController.Tests.QueueProcessors
{
    [TestClass]
    public class HierarchyQueueProcessorTests
    {
        private HierarchyQueueProcessor queueProcessor;

        private Mock<ILogger<HierarchyQueueProcessor>> mockLogger;
        private Mock<IQueueReader<MessageQueueHierarchy, Contracts.HierarchyType>> mockQueueReader;
        private Mock<ISerializer<Contracts.HierarchyType>> mockSerializer;
        private Mock<IQueryHandler<GetFinancialHierarchyClassesParameters, List<HierarchyClass>>> mockGetFinancialClassesQueryHandler;
        private Mock<ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>>> mockSaveXmlMessageCommandHandler;
        private Mock<ICommandHandler<AssociateMessageToQueueCommand<MessageQueueHierarchy, MessageHistory>>> mockAssociateMessageToQueueCommandHandler;
        private Mock<ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueueHierarchy>>> mockSetProcessedDateCommandHandler;
        private Mock<ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>> mockUpdateMessageHistoryCommandHandler;
        private Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueHierarchy>>> mockUpdateMessageQueueStatusCommandHandler;
        private Mock<ICommandHandler<UpdateSentToEsbHierarchyTraitCommand>> mockUpdatePublishedHierarchyTraitCommandHandler;
        private Mock<ICommandHandler<UpdateStagedProductStatusCommand>> mockUpdateStagedProductStatusCommandHandler;
        private Mock<ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueHierarchy>>> mockMarkQueuedEntriesAsInProcessCommandHandler;
        private Mock<IEsbProducer> mockProducer;
        private Mock<IActiveMQProducer> mockActiveMqProducer;
        private ApiControllerSettings settings;
        private Mock<IMessageProcessorMonitor> mockMonitor;
        private APIMessageProcessorLogEntry actualMonitorLogEntry;

        [TestInitialize]
        public void Initialize()
        {
            settings = new ApiControllerSettings();
            mockLogger = new Mock<ILogger<HierarchyQueueProcessor>>();
            mockQueueReader = new Mock<IQueueReader<MessageQueueHierarchy, Contracts.HierarchyType>>();
            mockSerializer = new Mock<ISerializer<Contracts.HierarchyType>>();
            mockGetFinancialClassesQueryHandler = new Mock<IQueryHandler<GetFinancialHierarchyClassesParameters, List<HierarchyClass>>>();
            mockSaveXmlMessageCommandHandler = new Mock<ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>>>();
            mockAssociateMessageToQueueCommandHandler = new Mock<ICommandHandler<AssociateMessageToQueueCommand<MessageQueueHierarchy, MessageHistory>>>();
            mockSetProcessedDateCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueueHierarchy>>>();
            mockUpdateMessageHistoryCommandHandler = new Mock<ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>>();
            mockUpdateMessageQueueStatusCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueHierarchy>>>();
            mockUpdatePublishedHierarchyTraitCommandHandler = new Mock<ICommandHandler<UpdateSentToEsbHierarchyTraitCommand>>();
            mockUpdateStagedProductStatusCommandHandler = new Mock<ICommandHandler<UpdateStagedProductStatusCommand>>();
            mockMarkQueuedEntriesAsInProcessCommandHandler = new Mock<ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueHierarchy>>>();
            mockProducer = new Mock<IEsbProducer>();
            mockActiveMqProducer = new Mock<IActiveMQProducer>();
            mockMonitor = new Mock<IMessageProcessorMonitor>();
            actualMonitorLogEntry = new APIMessageProcessorLogEntry();
            //set up the mock's RecordResults() method with a callback so that we can examine the data passed to it
            mockMonitor.Setup(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()))
                .Callback<APIMessageProcessorLogEntry>(methodData => actualMonitorLogEntry = methodData);

            mockGetFinancialClassesQueryHandler.Setup(q => q.Search(It.IsAny<GetFinancialHierarchyClassesParameters>())).Returns(new List<HierarchyClass>());

            queueProcessor = new HierarchyQueueProcessor(
                settings,
                mockLogger.Object,
                mockQueueReader.Object,
                mockSerializer.Object,
                mockGetFinancialClassesQueryHandler.Object,
                mockSaveXmlMessageCommandHandler.Object,
                mockAssociateMessageToQueueCommandHandler.Object,
                mockSetProcessedDateCommandHandler.Object,
                mockUpdateMessageHistoryCommandHandler.Object,
                mockUpdateMessageQueueStatusCommandHandler.Object,
                mockUpdateStagedProductStatusCommandHandler.Object,
                mockUpdatePublishedHierarchyTraitCommandHandler.Object,
                mockMarkQueuedEntriesAsInProcessCommandHandler.Object,
                mockProducer.Object,
                mockMonitor.Object,
                mockActiveMqProducer.Object);
        }

        [TestMethod]
        public void ProcessQueuedHierarchyEvents_BeforeMarkingMessagesAsInProcess_FinancialHierarchyCacheShouldBePopulated()
        {
            // Given.
            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(new List<MessageQueueHierarchy>());

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockGetFinancialClassesQueryHandler.Verify(q => q.Search(It.IsAny<GetFinancialHierarchyClassesParameters>()), Times.Once);
        }

        [TestMethod]
        public void ProcessQueuedHierarchyEvents_NoQueuedMessages_GroupMessagesShouldNotBeCalled()
        {
            // Given.
            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(new List<MessageQueueHierarchy>());

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockQueueReader.Verify(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueHierarchy>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedHierarchyEvents_GetGroupedMessagesReturnsEmpty_BuildMiniBulkShouldNotBeCalled()
        {
            // Given.
            var fakeMessageQueueHierarchies = new List<MessageQueueHierarchy> { TestHelpers.GetFakeMessageQueueHierarchy(1, "Test", true) };
            var fakeMessageQueueHierarchiesEmpty = new List<MessageQueueHierarchy>();

            var queuedMessages = new Queue<List<MessageQueueHierarchy>>();
            queuedMessages.Enqueue(fakeMessageQueueHierarchies);
            queuedMessages.Enqueue(fakeMessageQueueHierarchiesEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueHierarchy>>())).Returns(new List<MessageQueueHierarchy>());

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockQueueReader.Verify(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueHierarchy>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedHierarchyEvents_MiniBulkReturnsEmpty_SerializeShouldNotBeCalled()
        {
            // Given.
            var fakeMessageQueueHierarchies = new List<MessageQueueHierarchy> { TestHelpers.GetFakeMessageQueueHierarchy(1, "Test", true) };
            var fakeMessageQueueHierarchiesEmpty = new List<MessageQueueHierarchy>();

            var queuedMessages = new Queue<List<MessageQueueHierarchy>>();
            queuedMessages.Enqueue(fakeMessageQueueHierarchies);
            queuedMessages.Enqueue(fakeMessageQueueHierarchiesEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueHierarchy>>())).Returns(fakeMessageQueueHierarchies);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueHierarchy>>())).Returns(new Contracts.HierarchyType { @class = new Contracts.HierarchyClassType[0] });

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockSerializer.Verify(s => s.Serialize(It.IsAny<Contracts.HierarchyType>(), It.IsAny<TextWriter>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedHierarchyEvents_SerializationError_UpdateMessageQueueStatusCommandShouldBeCalled()
        {
            // Given.
            var fakeMessageQueueHierarchies = new List<MessageQueueHierarchy> { TestHelpers.GetFakeMessageQueueHierarchy(1, "Test", true) };
            var fakeMessageQueueHierarchiesEmpty = new List<MessageQueueHierarchy>();

            var queuedMessages = new Queue<List<MessageQueueHierarchy>>();
            queuedMessages.Enqueue(fakeMessageQueueHierarchies);
            queuedMessages.Enqueue(fakeMessageQueueHierarchiesEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueHierarchy>>())).Returns(fakeMessageQueueHierarchies);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueHierarchy>>())).Returns(new Contracts.HierarchyType { @class = new Contracts.HierarchyClassType[] { new Contracts.HierarchyClassType { id = "1" } } });
            mockSerializer.Setup(s => s.Serialize(It.IsAny<Contracts.HierarchyType>(), It.IsAny<TextWriter>())).Returns(string.Empty);

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockUpdateMessageQueueStatusCommandHandler.Verify(c => c.Execute(It.IsAny<UpdateMessageQueueStatusCommand<MessageQueueHierarchy>>()), Times.Once);
            mockSaveXmlMessageCommandHandler.Verify(c => c.Execute(It.IsAny<SaveToMessageHistoryCommand<MessageHistory>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedHierarchyEvents_NoErrors_UpdateMessageHistoryCommandShouldBeCalled()
        {
            // Given.
            var fakeMessageQueueHierarchies = new List<MessageQueueHierarchy> { TestHelpers.GetFakeMessageQueueHierarchy(1, "Test", true) };
            var fakeMessageQueueHierarchiesEmpty = new List<MessageQueueHierarchy>();

            var queuedMessages = new Queue<List<MessageQueueHierarchy>>();
            queuedMessages.Enqueue(fakeMessageQueueHierarchies);
            queuedMessages.Enqueue(fakeMessageQueueHierarchiesEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueHierarchy>>())).Returns(fakeMessageQueueHierarchies);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueHierarchy>>())).Returns(new Contracts.HierarchyType { @class = new Contracts.HierarchyClassType[] { new Contracts.HierarchyClassType { id = "1" } } });
            mockSerializer.Setup(s => s.Serialize(It.IsAny<Contracts.HierarchyType>(), It.IsAny<TextWriter>())).Returns("Test");
            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
            mockActiveMqProducer.Setup(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockUpdateMessageHistoryCommandHandler.Verify(c => c.Execute(It.IsAny<UpdateMessageHistoryStatusCommand<MessageHistory>>()), Times.Once);
        }

        [TestMethod]
        public void ProcessQueuedHierarchyEvents_TaxMiniBulk_UpdateSentToEsbCommandShouldNotBeCalled()
        {
            // Given.
            var fakeMessageQueueHierarchies = new List<MessageQueueHierarchy> { TestHelpers.GetFakeMessageQueueHierarchy(Hierarchies.Tax, "Test", true) };
            var fakeMessageQueueHierarchiesEmpty = new List<MessageQueueHierarchy>();

            var queuedMessages = new Queue<List<MessageQueueHierarchy>>();
            queuedMessages.Enqueue(fakeMessageQueueHierarchies);
            queuedMessages.Enqueue(fakeMessageQueueHierarchiesEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueHierarchy>>())).Returns(fakeMessageQueueHierarchies);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueHierarchy>>())).Returns(new Contracts.HierarchyType { id = Hierarchies.Tax, name = HierarchyNames.Tax, @class = new Contracts.HierarchyClassType[] { new Contracts.HierarchyClassType { id = Hierarchies.Tax.ToString(), name = HierarchyNames.Tax } } });
            mockSerializer.Setup(s => s.Serialize(It.IsAny<Contracts.HierarchyType>(), It.IsAny<TextWriter>())).Returns("Test");
            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
            mockActiveMqProducer.Setup(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockUpdatePublishedHierarchyTraitCommandHandler.Verify(c => c.Execute(It.IsAny<UpdateSentToEsbHierarchyTraitCommand>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedHierarchyEvents_WhenNothingQueued_ShouldNotCallJobMonitor()
        {
            // Given.
            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(new List<MessageQueueHierarchy>());

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedHierarchyEvents_WhenSuccessful_ShouldCallJobMonitor()
        {
            // Given.
            var fakeMessageQueueHierarchys = new List<MessageQueueHierarchy> { TestHelpers.GetFakeMessageQueueHierarchy(1, "Test1", true) };
            var fakeMessageQueueHierarchysEmpty = new List<MessageQueueHierarchy>();

            var queuedMessages = new Queue<List<MessageQueueHierarchy>>();
            queuedMessages.Enqueue(fakeMessageQueueHierarchys);
            queuedMessages.Enqueue(fakeMessageQueueHierarchysEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueHierarchy>>())).Returns(fakeMessageQueueHierarchys);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueHierarchy>>()))
                .Returns(new Contracts.HierarchyType { @class = new Contracts.HierarchyClassType[] { new Contracts.HierarchyClassType { id = "1" } } });
            mockSerializer.Setup(s => s.Serialize(It.IsAny<Contracts.HierarchyType>(), It.IsAny<TextWriter>())).Returns("Test");
            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
            mockActiveMqProducer.Setup(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Once);
        }

        private void SetupMockMessageQueueSequence(List<MessageQueueHierarchy> messageQueue, int sequenceIndexToFailSerialization)
        {
            SetupMockMessageQueueSequence(messageQueue, new List<int> { sequenceIndexToFailSerialization });
        }

        private void SetupMockMessageQueueSequence(List<MessageQueueHierarchy> messageQueue, List<int> sequenceIndicesToFailSerialization = null)
        {
            if (sequenceIndicesToFailSerialization == null) sequenceIndicesToFailSerialization = new List<int>();
            mockQueueReader.SetupSequence(qr => qr.GetQueuedMessages())
                .Returns(messageQueue.Count > 0 ? new List<MessageQueueHierarchy>() { messageQueue[0] } : new List<MessageQueueHierarchy>())
                .Returns(messageQueue.Count > 1 ? new List<MessageQueueHierarchy>() { messageQueue[1] } : new List<MessageQueueHierarchy>())
                .Returns(messageQueue.Count > 2 ? new List<MessageQueueHierarchy>() { messageQueue[2] } : new List<MessageQueueHierarchy>())
                .Returns(messageQueue.Count > 3 ? new List<MessageQueueHierarchy>() { messageQueue[3] } : new List<MessageQueueHierarchy>())
                .Returns(messageQueue.Count > 4 ? new List<MessageQueueHierarchy>() { messageQueue[4] } : new List<MessageQueueHierarchy>());
            mockQueueReader.SetupSequence(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueHierarchy>>()))
                .Returns(messageQueue.Count > 0 ? new List<MessageQueueHierarchy>() { messageQueue[0] } : new List<MessageQueueHierarchy>())
                .Returns(messageQueue.Count > 1 ? new List<MessageQueueHierarchy>() { messageQueue[1] } : new List<MessageQueueHierarchy>())
                .Returns(messageQueue.Count > 2 ? new List<MessageQueueHierarchy>() { messageQueue[2] } : new List<MessageQueueHierarchy>())
                .Returns(messageQueue.Count > 3 ? new List<MessageQueueHierarchy>() { messageQueue[3] } : new List<MessageQueueHierarchy>())
                .Returns(messageQueue.Count > 4 ? new List<MessageQueueHierarchy>() { messageQueue[4] } : new List<MessageQueueHierarchy>());
            mockQueueReader.SetupSequence(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueHierarchy>>()))
                .Returns(messageQueue.Count > 0 ? new Contracts.HierarchyType { @class = new Contracts.HierarchyClassType[] { new Contracts.HierarchyClassType { id = messageQueue[0].HierarchyClassId } } } : null)
                .Returns(messageQueue.Count > 1 ? new Contracts.HierarchyType { @class = new Contracts.HierarchyClassType[] { new Contracts.HierarchyClassType { id = messageQueue[1].HierarchyClassId } } } : null)
                .Returns(messageQueue.Count > 2 ? new Contracts.HierarchyType { @class = new Contracts.HierarchyClassType[] { new Contracts.HierarchyClassType { id = messageQueue[2].HierarchyClassId } } } : null)
                .Returns(messageQueue.Count > 3 ? new Contracts.HierarchyType { @class = new Contracts.HierarchyClassType[] { new Contracts.HierarchyClassType { id = messageQueue[3].HierarchyClassId } } } : null)
                .Returns(messageQueue.Count > 4 ? new Contracts.HierarchyType { @class = new Contracts.HierarchyClassType[] { new Contracts.HierarchyClassType { id = messageQueue[4].HierarchyClassId } } } : null);
            mockSerializer.SetupSequence(s => s.Serialize(It.IsAny<Contracts.HierarchyType>(), It.IsAny<TextWriter>()))
                .Returns(messageQueue.Count > 0 && !sequenceIndicesToFailSerialization.Contains(0) ? "Test1" : null)
                .Returns(messageQueue.Count > 1 && !sequenceIndicesToFailSerialization.Contains(1) ? "Test2" : null)
                .Returns(messageQueue.Count > 2 && !sequenceIndicesToFailSerialization.Contains(2) ? "Test3" : null)
                .Returns(messageQueue.Count > 3 && !sequenceIndicesToFailSerialization.Contains(3) ? "Test4" : null)
                .Returns(messageQueue.Count > 4 && !sequenceIndicesToFailSerialization.Contains(4) ? "Test5" : null);
            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
            mockActiveMqProducer.Setup(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
        }

        [TestMethod]
        public void ProcessQueuedHierarchyEvents_WhenSuccessful_ShouldCallJobMonitor_WithExpectedValues()
        {
            // Given.
            var fakeMessageQueueHierarchys = new List<MessageQueueHierarchy> {
                TestHelpers.GetFakeMessageQueueHierarchy(1, "Test1", true, "1"),
                TestHelpers.GetFakeMessageQueueHierarchy(2, "Test2", true, "2"),
                TestHelpers.GetFakeMessageQueueHierarchy(3, "Test3", true, "3"),
                TestHelpers.GetFakeMessageQueueHierarchy(4, "Test4", true, "4")
            };
            SetupMockMessageQueueSequence(fakeMessageQueueHierarchys);
            int expectedLogCountSuccessful = fakeMessageQueueHierarchys.Count;
            int expectedLogCountFailed = 0;
            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Once);
            Assert.AreEqual(MessageTypes.Hierarchy, actualMonitorLogEntry.MessageTypeID);
            Assert.AreEqual(expectedLogCountSuccessful, actualMonitorLogEntry.CountProcessedMessages.GetValueOrDefault(0));
            Assert.AreEqual(expectedLogCountFailed, actualMonitorLogEntry.CountFailedMessages.GetValueOrDefault(0));
            Assert.IsNotNull(actualMonitorLogEntry.StartTime);
            Assert.IsNotNull(actualMonitorLogEntry.EndTime);
            Assert.IsTrue(actualMonitorLogEntry.EndTime >= actualMonitorLogEntry.StartTime);
        }

        [TestMethod]
        public void ProcessQueuedHierarchyEvents_WhenSerializationErrorForOne_ShouldCallJobMonitor_WithExpectedValues()
        {
            // Given.
            var fakeMessageQueueHierarchys = new List<MessageQueueHierarchy> {
                TestHelpers.GetFakeMessageQueueHierarchy(1, "Test1", true),
                TestHelpers.GetFakeMessageQueueHierarchy(2, "Test2", true),
                TestHelpers.GetFakeMessageQueueHierarchy(3, "Test3", true)
            };
            SetupMockMessageQueueSequence(fakeMessageQueueHierarchys, 1);
            int expectedLogCountSuccessful = 2;
            int expectedLogCountFailed = 1;

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Once);
            Assert.AreEqual(MessageTypes.Hierarchy, actualMonitorLogEntry.MessageTypeID);
            Assert.AreEqual(expectedLogCountSuccessful, actualMonitorLogEntry.CountProcessedMessages.GetValueOrDefault(0));
            Assert.AreEqual(expectedLogCountFailed, actualMonitorLogEntry.CountFailedMessages.GetValueOrDefault(0));
            Assert.IsNotNull(actualMonitorLogEntry.StartTime);
            Assert.IsNotNull(actualMonitorLogEntry.EndTime);
            Assert.IsTrue(actualMonitorLogEntry.EndTime >= actualMonitorLogEntry.StartTime);
        }

        [TestMethod]
        public void ProcessQueuedHierarchyEvents_WhenSerializationErrorForAll_ShouldCallJobMonitor_WithExpectedValues()
        {
            // Given.
            var fakeMessageQueueHierarchys = new List<MessageQueueHierarchy> {
                TestHelpers.GetFakeMessageQueueHierarchy(1, "Test1", true),
                TestHelpers.GetFakeMessageQueueHierarchy(2, "Test2", true),
                TestHelpers.GetFakeMessageQueueHierarchy(3, "Test3", true)
            };
            SetupMockMessageQueueSequence(fakeMessageQueueHierarchys, new List<int>() { 0, 1, 2 });
            int expectedLogCountSuccessful = 0;
            int expectedLogCountFailed = 3;

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Once);
            Assert.AreEqual(MessageTypes.Hierarchy, actualMonitorLogEntry.MessageTypeID);
            Assert.AreEqual(expectedLogCountSuccessful, actualMonitorLogEntry.CountProcessedMessages.GetValueOrDefault(0));
            Assert.AreEqual(expectedLogCountFailed, actualMonitorLogEntry.CountFailedMessages.GetValueOrDefault(0));
            Assert.IsNotNull(actualMonitorLogEntry.StartTime);
            Assert.IsNotNull(actualMonitorLogEntry.EndTime);
            Assert.IsTrue(actualMonitorLogEntry.EndTime >= actualMonitorLogEntry.StartTime);
        }

        [TestMethod]
        public void ProcessQueuedHierarchyEvents_WhenSendingEsbFailActiveMqSucceeds_MessageStatusShouldBeSentToActiveMq()
        {
            // Given.
            var fakeMessageQueueHierarchys = new List<MessageQueueHierarchy> { TestHelpers.GetFakeMessageQueueHierarchy(1, "Test1", true) };
            var fakeMessageQueueHierarchysEmpty = new List<MessageQueueHierarchy>();

            var queuedMessages = new Queue<List<MessageQueueHierarchy>>();
            queuedMessages.Enqueue(fakeMessageQueueHierarchys);
            queuedMessages.Enqueue(fakeMessageQueueHierarchysEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueHierarchy>>())).Returns(fakeMessageQueueHierarchys);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueHierarchy>>()))
                .Returns(new Contracts.HierarchyType { @class = new Contracts.HierarchyClassType[] { new Contracts.HierarchyClassType { id = "1" } } });
            mockSerializer.Setup(s => s.Serialize(It.IsAny<Contracts.HierarchyType>(), It.IsAny<TextWriter>())).Returns("Test");
            mockMonitor.Setup(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()));

            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Throws(new Exception());

            mockUpdateMessageHistoryCommandHandler.Setup(
                u => u.Execute(It.IsAny<UpdateMessageHistoryStatusCommand<MessageHistory>>())
            ).Callback<UpdateMessageHistoryStatusCommand<MessageHistory>>(
                (UpdateMessageHistoryStatusCommand<MessageHistory> cmd) =>
                {
                    // Checks if message status is SentToActiveMq
                    Assert.AreEqual(cmd.MessageStatusId, MessageStatusTypes.SentToActiveMq);
                }
            );

            // When.
            queueProcessor.ProcessMessageQueue();
        }

        [TestMethod]
        public void ProcessQueuedHierarchyEvents_WhenSendingActiveMqFailEsbSucceeds_MessageStatusShouldBeSentToEsb()
        {
            // Given.
            var fakeMessageQueueHierarchys = new List<MessageQueueHierarchy> { TestHelpers.GetFakeMessageQueueHierarchy(1, "Test1", true) };
            var fakeMessageQueueHierarchysEmpty = new List<MessageQueueHierarchy>();

            var queuedMessages = new Queue<List<MessageQueueHierarchy>>();
            queuedMessages.Enqueue(fakeMessageQueueHierarchys);
            queuedMessages.Enqueue(fakeMessageQueueHierarchysEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueHierarchy>>())).Returns(fakeMessageQueueHierarchys);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueHierarchy>>()))
                .Returns(new Contracts.HierarchyType { @class = new Contracts.HierarchyClassType[] { new Contracts.HierarchyClassType { id = "1" } } });
            mockSerializer.Setup(s => s.Serialize(It.IsAny<Contracts.HierarchyType>(), It.IsAny<TextWriter>())).Returns("Test");
            mockMonitor.Setup(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()));

            mockActiveMqProducer.Setup(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Throws(new Exception());

            mockUpdateMessageHistoryCommandHandler.Setup(
                u => u.Execute(It.IsAny<UpdateMessageHistoryStatusCommand<MessageHistory>>())
            ).Callback<UpdateMessageHistoryStatusCommand<MessageHistory>>(
                (UpdateMessageHistoryStatusCommand<MessageHistory> cmd) =>
                {
                    // Checks if message status is SentToActiveMq
                    Assert.AreEqual(cmd.MessageStatusId, MessageStatusTypes.SentToEsb);
                }
            );

            // When.
            queueProcessor.ProcessMessageQueue();
        }

        [TestMethod]
        public void ProcessQueuedHierarchyEvents_WhenSendingEsbAndActiveMqFail_MessageHistoryShouldNotBeUpdated()
        {
            // Given.
            var fakeMessageQueueHierarchys = new List<MessageQueueHierarchy> { TestHelpers.GetFakeMessageQueueHierarchy(1, "Test1", true) };
            var fakeMessageQueueHierarchysEmpty = new List<MessageQueueHierarchy>();

            var queuedMessages = new Queue<List<MessageQueueHierarchy>>();
            queuedMessages.Enqueue(fakeMessageQueueHierarchys);
            queuedMessages.Enqueue(fakeMessageQueueHierarchysEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueHierarchy>>())).Returns(fakeMessageQueueHierarchys);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueHierarchy>>()))
                .Returns(new Contracts.HierarchyType { @class = new Contracts.HierarchyClassType[] { new Contracts.HierarchyClassType { id = "1" } } });
            mockSerializer.Setup(s => s.Serialize(It.IsAny<Contracts.HierarchyType>(), It.IsAny<TextWriter>())).Returns("Test");
            mockMonitor.Setup(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()));

            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Throws(new Exception());
            mockActiveMqProducer.Setup(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Throws(new Exception());

            mockUpdateMessageHistoryCommandHandler.Setup(u => u.Execute(It.IsAny<UpdateMessageHistoryStatusCommand<MessageHistory>>()));

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockUpdateMessageHistoryCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateMessageHistoryStatusCommand<MessageHistory>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedHierarchyEvents_WhenSendingEsbAndActiveMqSucceeds_MessageStatusShouldBeSent()
        {
            // Given.
            var fakeMessageQueueHierarchys = new List<MessageQueueHierarchy> { TestHelpers.GetFakeMessageQueueHierarchy(1, "Test1", true) };
            var fakeMessageQueueHierarchysEmpty = new List<MessageQueueHierarchy>();

            var queuedMessages = new Queue<List<MessageQueueHierarchy>>();
            queuedMessages.Enqueue(fakeMessageQueueHierarchys);
            queuedMessages.Enqueue(fakeMessageQueueHierarchysEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueHierarchy>>())).Returns(fakeMessageQueueHierarchys);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueHierarchy>>()))
                .Returns(new Contracts.HierarchyType { @class = new Contracts.HierarchyClassType[] { new Contracts.HierarchyClassType { id = "1" } } });
            mockSerializer.Setup(s => s.Serialize(It.IsAny<Contracts.HierarchyType>(), It.IsAny<TextWriter>())).Returns("Test");
            mockMonitor.Setup(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()));

            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
            mockActiveMqProducer.Setup(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));

            mockUpdateMessageHistoryCommandHandler.Setup(
                u => u.Execute(It.IsAny<UpdateMessageHistoryStatusCommand<MessageHistory>>())
            ).Callback<UpdateMessageHistoryStatusCommand<MessageHistory>>(
                (UpdateMessageHistoryStatusCommand<MessageHistory> cmd) =>
                {
                    // Checks if message status is SentToActiveMq
                    Assert.AreEqual(cmd.MessageStatusId, MessageStatusTypes.Sent);
                }
            );

            // When.
            queueProcessor.ProcessMessageQueue();
        }
    }
}
