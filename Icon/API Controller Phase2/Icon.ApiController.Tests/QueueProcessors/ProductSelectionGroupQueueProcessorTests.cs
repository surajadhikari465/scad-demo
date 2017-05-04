using Icon.ApiController.Controller.QueueProcessors;
using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.Controller.Serializers;
using Icon.ApiController.DataAccess.Commands;
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
using Icon.ApiController.Common;
using Icon.ApiController.Controller.Monitoring;

namespace Icon.ApiController.Tests.QueueProcessors
{
    [TestClass]
    public class ProductSelectionGroupQueueProcessorTests
    {
        private ProductSelectionGroupQueueProcessor queueProcessor;

        private Mock<ILogger<ProductSelectionGroupQueueProcessor>> mockLogger;
        private Mock<ISerializer<Contracts.SelectionGroupsType>> mockSerializer;
        private Mock<IQueueReader<MessageQueueProductSelectionGroup, Contracts.SelectionGroupsType>> mockQueueReader;
        private Mock<ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>>> mockSaveXmlMessageCommandHandler;
        private Mock<ICommandHandler<AssociateMessageToQueueCommand<MessageQueueProductSelectionGroup, MessageHistory>>> mockAssociateMessageToQueueCommandHandler;
        private Mock<ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueueProductSelectionGroup>>> mockSetProcessedDateCommandHandler;
        private Mock<ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>> mockUpdateMessageHistoryCommandHandler;
        private Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueProductSelectionGroup>>> mockUpdateMessageQueueStatusCommandHandler;
        private Mock<ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueProductSelectionGroup>>> mockMarkQueuedEntriesAsInProcessCommandHandler;
        private Mock<IEsbProducer> mockProducer;
        private ApiControllerSettings settings;
        private Mock<IMessageProcessorMonitor> mockMonitor;
        private APIMessageProcessorLogEntry actualMonitorLogEntry;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger<ProductSelectionGroupQueueProcessor>>();
            mockSerializer = new Mock<ISerializer<Contracts.SelectionGroupsType>>();
            mockQueueReader = new Mock<IQueueReader<MessageQueueProductSelectionGroup, Contracts.SelectionGroupsType>>();
            mockSaveXmlMessageCommandHandler = new Mock<ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>>>();
            mockAssociateMessageToQueueCommandHandler = new Mock<ICommandHandler<AssociateMessageToQueueCommand<MessageQueueProductSelectionGroup, MessageHistory>>>();
            mockSetProcessedDateCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueueProductSelectionGroup>>>();
            mockUpdateMessageHistoryCommandHandler = new Mock<ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>>();
            mockUpdateMessageQueueStatusCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueProductSelectionGroup>>>();
            mockMarkQueuedEntriesAsInProcessCommandHandler = new Mock<ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueProductSelectionGroup>>>();
            mockProducer = new Mock<IEsbProducer>();
            settings = new ApiControllerSettings();
            mockMonitor = new Mock<IMessageProcessorMonitor>();
            actualMonitorLogEntry = new APIMessageProcessorLogEntry();
            //set up the mock's RecordResults() method with a callback so that we can examine the data passed to it
            mockMonitor.Setup(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()))
                .Callback<APIMessageProcessorLogEntry>(methodData => actualMonitorLogEntry = methodData);

            queueProcessor = new ProductSelectionGroupQueueProcessor(
                settings,
                mockLogger.Object,
                mockQueueReader.Object,
                mockSerializer.Object,
                mockSaveXmlMessageCommandHandler.Object,
                mockAssociateMessageToQueueCommandHandler.Object,
                mockSetProcessedDateCommandHandler.Object,
                mockUpdateMessageHistoryCommandHandler.Object,
                mockUpdateMessageQueueStatusCommandHandler.Object,
                mockMarkQueuedEntriesAsInProcessCommandHandler.Object,
                mockProducer.Object,
                mockMonitor.Object);
        }

        [TestMethod]
        public void ProcessQueuedProductEvents_NoQueuedMessages_GroupMessagesShouldNotBeCalled()
        {
            // Given.
            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(new List<MessageQueueProductSelectionGroup>());

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockQueueReader.Verify(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueProductSelectionGroup>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedProductEvents_GetGroupedMessagesReturnsEmpty_BuildMiniBulkShouldNotBeCalled()
        {
            // Given.
            var fakeMessageQueueProductSelectionGroups = new List<MessageQueueProductSelectionGroup> { TestHelpers.GetFakeMessageQueueProductSelectionGroup() };
            var fakeMessageQueueProductSelectionGroupsEmpty = new List<MessageQueueProductSelectionGroup>();

            var queuedMessages = new Queue<List<MessageQueueProductSelectionGroup>>();
            queuedMessages.Enqueue(fakeMessageQueueProductSelectionGroups);
            queuedMessages.Enqueue(fakeMessageQueueProductSelectionGroupsEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueProductSelectionGroup>>())).Returns(new List<MessageQueueProductSelectionGroup>());

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockQueueReader.Verify(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueProductSelectionGroup>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedProductEvents_MiniBulkReturnsEmpty_SerializeShouldNotBeCalled()
        {
            // Given.
            var fakeMessageQueueProductSelectionGroups = new List<MessageQueueProductSelectionGroup> { TestHelpers.GetFakeMessageQueueProductSelectionGroup() };
            var fakeMessageQueueProductSelectionGroupsEmpty = new List<MessageQueueProductSelectionGroup>();

            var queuedMessages = new Queue<List<MessageQueueProductSelectionGroup>>();
            queuedMessages.Enqueue(fakeMessageQueueProductSelectionGroups);
            queuedMessages.Enqueue(fakeMessageQueueProductSelectionGroupsEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueProductSelectionGroup>>())).Returns(fakeMessageQueueProductSelectionGroups);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueProductSelectionGroup>>())).Returns(new Contracts.SelectionGroupsType { group = new Contracts.GroupTypeType[0] });

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockSerializer.Verify(s => s.Serialize(It.IsAny<Contracts.SelectionGroupsType>(), It.IsAny<TextWriter>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedProductEvents_SerializationError_UpdateMessageQueueStatusCommandShouldBeCalled()
        {
            // Given.
            var fakeMessageQueueProductSelectionGroups = new List<MessageQueueProductSelectionGroup> { TestHelpers.GetFakeMessageQueueProductSelectionGroup() };
            var fakeMessageQueueProductSelectionGroupsEmpty = new List<MessageQueueProductSelectionGroup>();

            var queuedMessages = new Queue<List<MessageQueueProductSelectionGroup>>();
            queuedMessages.Enqueue(fakeMessageQueueProductSelectionGroups);
            queuedMessages.Enqueue(fakeMessageQueueProductSelectionGroupsEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueProductSelectionGroup>>())).Returns(fakeMessageQueueProductSelectionGroups);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueProductSelectionGroup>>())).Returns(new Contracts.SelectionGroupsType { group = new Contracts.GroupTypeType[] { new Contracts.GroupTypeType { id = "1" } } });
            mockSerializer.Setup(s => s.Serialize(It.IsAny<Contracts.SelectionGroupsType>(), It.IsAny<TextWriter>())).Returns(string.Empty);

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockUpdateMessageQueueStatusCommandHandler.Verify(c => c.Execute(It.IsAny<UpdateMessageQueueStatusCommand<MessageQueueProductSelectionGroup>>()), Times.Once);
            mockSaveXmlMessageCommandHandler.Verify(c => c.Execute(It.IsAny<SaveToMessageHistoryCommand<MessageHistory>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedProductEvents_NoErrors_UpdateMessageHistoryCommandShouldBeCalled()
        {
            // Given.
            var fakeMessageQueueProductSelectionGroups = new List<MessageQueueProductSelectionGroup> { TestHelpers.GetFakeMessageQueueProductSelectionGroup() };
            var fakeMessageQueueProductSelectionGroupsEmpty = new List<MessageQueueProductSelectionGroup>();

            var queuedMessages = new Queue<List<MessageQueueProductSelectionGroup>>();
            queuedMessages.Enqueue(fakeMessageQueueProductSelectionGroups);
            queuedMessages.Enqueue(fakeMessageQueueProductSelectionGroupsEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueProductSelectionGroup>>())).Returns(fakeMessageQueueProductSelectionGroups);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueProductSelectionGroup>>())).Returns(new Contracts.SelectionGroupsType { group = new Contracts.GroupTypeType[] { new Contracts.GroupTypeType { id = "1" } } });
            mockSerializer.Setup(s => s.Serialize(It.IsAny<Contracts.SelectionGroupsType>(), It.IsAny<TextWriter>())).Returns("Test");
            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockUpdateMessageHistoryCommandHandler.Verify(c => c.Execute(It.IsAny<UpdateMessageHistoryStatusCommand<MessageHistory>>()), Times.Once);
        }

        [TestMethod]
        public void ProcessQueuedProductSelectionGroupEvents_WhenNothingQueued_ShouldNotCallJobMonitor()
        {
            // Given.
            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(new List<MessageQueueProductSelectionGroup>());

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Never);
        }

        private void SetupMockMessageQueueSequence(List<MessageQueueProductSelectionGroup> messageQueue, int sequenceIndexToFailSerialization)
        {
            SetupMockMessageQueueSequence(messageQueue, new List<int> { sequenceIndexToFailSerialization });
        }

        private void SetupMockMessageQueueSequence(List<MessageQueueProductSelectionGroup> messageQueue, List<int> sequenceIndicesToFailSerialization = null)
        {
            if (sequenceIndicesToFailSerialization == null) sequenceIndicesToFailSerialization = new List<int>();
            mockQueueReader.SetupSequence(qr => qr.GetQueuedMessages())
                .Returns(messageQueue.Count > 0 ? new List<MessageQueueProductSelectionGroup>() { messageQueue[0] } : new List<MessageQueueProductSelectionGroup>())
                .Returns(messageQueue.Count > 1 ? new List<MessageQueueProductSelectionGroup>() { messageQueue[1] } : new List<MessageQueueProductSelectionGroup>())
                .Returns(messageQueue.Count > 2 ? new List<MessageQueueProductSelectionGroup>() { messageQueue[2] } : new List<MessageQueueProductSelectionGroup>())
                .Returns(messageQueue.Count > 3 ? new List<MessageQueueProductSelectionGroup>() { messageQueue[3] } : new List<MessageQueueProductSelectionGroup>())
                .Returns(messageQueue.Count > 4 ? new List<MessageQueueProductSelectionGroup>() { messageQueue[4] } : new List<MessageQueueProductSelectionGroup>());
            mockQueueReader.SetupSequence(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueProductSelectionGroup>>()))
                .Returns(messageQueue.Count > 0 ? new List<MessageQueueProductSelectionGroup>() { messageQueue[0] } : new List<MessageQueueProductSelectionGroup>())
                .Returns(messageQueue.Count > 1 ? new List<MessageQueueProductSelectionGroup>() { messageQueue[1] } : new List<MessageQueueProductSelectionGroup>())
                .Returns(messageQueue.Count > 2 ? new List<MessageQueueProductSelectionGroup>() { messageQueue[2] } : new List<MessageQueueProductSelectionGroup>())
                .Returns(messageQueue.Count > 3 ? new List<MessageQueueProductSelectionGroup>() { messageQueue[3] } : new List<MessageQueueProductSelectionGroup>())
                .Returns(messageQueue.Count > 4 ? new List<MessageQueueProductSelectionGroup>() { messageQueue[4] } : new List<MessageQueueProductSelectionGroup>());
            mockQueueReader.SetupSequence(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueProductSelectionGroup>>()))
                .Returns(messageQueue.Count > 0 ? new Contracts.SelectionGroupsType { group = new Contracts.GroupTypeType[] { new Contracts.GroupTypeType { id = messageQueue[0].ProductSelectionGroupId.ToString() } } } : null)
                .Returns(messageQueue.Count > 1 ? new Contracts.SelectionGroupsType { group = new Contracts.GroupTypeType[] { new Contracts.GroupTypeType { id = messageQueue[1].ProductSelectionGroupId.ToString() } } } : null)
                .Returns(messageQueue.Count > 2 ? new Contracts.SelectionGroupsType { group = new Contracts.GroupTypeType[] { new Contracts.GroupTypeType { id = messageQueue[2].ProductSelectionGroupId.ToString() } } } : null)
                .Returns(messageQueue.Count > 3 ? new Contracts.SelectionGroupsType { group = new Contracts.GroupTypeType[] { new Contracts.GroupTypeType { id = messageQueue[3].ProductSelectionGroupId.ToString() } } } : null)
                .Returns(messageQueue.Count > 4 ? new Contracts.SelectionGroupsType { group = new Contracts.GroupTypeType[] { new Contracts.GroupTypeType { id = messageQueue[4].ProductSelectionGroupId.ToString() } } } : null);
            mockSerializer.SetupSequence(s => s.Serialize(It.IsAny<Contracts.SelectionGroupsType>(), It.IsAny<TextWriter>()))
                .Returns(messageQueue.Count > 0 && !sequenceIndicesToFailSerialization.Contains(0) ? "Test1" : null)
                .Returns(messageQueue.Count > 1 && !sequenceIndicesToFailSerialization.Contains(1) ? "Test2" : null)
                .Returns(messageQueue.Count > 2 && !sequenceIndicesToFailSerialization.Contains(2) ? "Test3" : null)
                .Returns(messageQueue.Count > 3 && !sequenceIndicesToFailSerialization.Contains(3) ? "Test4" : null)
                .Returns(messageQueue.Count > 4 && !sequenceIndicesToFailSerialization.Contains(4) ? "Test5" : null);
            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
        }

        [TestMethod]
        public void ProcessQueuedProductSelectionGroupEvents_WhenSuccessful_ShouldCallJobMonitor_WithExpectedValues()
        {
            // Given.
            var fakeMessageQueueProductSelectionGroups = new List<MessageQueueProductSelectionGroup> {
                TestHelpers.GetFakeMessageQueueProductSelectionGroup(1),
                TestHelpers.GetFakeMessageQueueProductSelectionGroup(2),
                TestHelpers.GetFakeMessageQueueProductSelectionGroup(3),
                TestHelpers.GetFakeMessageQueueProductSelectionGroup(4)
            };
            SetupMockMessageQueueSequence(fakeMessageQueueProductSelectionGroups);
            int expectedLogCountSuccessful = fakeMessageQueueProductSelectionGroups.Count;
            int expectedLogCountFailed = 0;

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Once);
            Assert.AreEqual(MessageTypes.ProductSelectionGroup, actualMonitorLogEntry.MessageTypeID);
            Assert.AreEqual(expectedLogCountSuccessful, actualMonitorLogEntry.CountProcessedMessages.GetValueOrDefault(0));
            Assert.AreEqual(expectedLogCountFailed, actualMonitorLogEntry.CountFailedMessages.GetValueOrDefault(0));
            Assert.IsNotNull(actualMonitorLogEntry.StartTime);
            Assert.IsNotNull(actualMonitorLogEntry.EndTime);
            Assert.IsTrue(actualMonitorLogEntry.EndTime >= actualMonitorLogEntry.StartTime);
        }

        [TestMethod]
        public void ProcessQueuedProductSelectionGroupEvents_WhenSerializationErrorForOne_ShouldCallJobMonitor_WithExpectedValues()
        {
            // Given.
            var fakeMessageQueueProductSelectionGroups = new List<MessageQueueProductSelectionGroup> {
                TestHelpers.GetFakeMessageQueueProductSelectionGroup(1),
                TestHelpers.GetFakeMessageQueueProductSelectionGroup(2),
                TestHelpers.GetFakeMessageQueueProductSelectionGroup(3)
            };
            SetupMockMessageQueueSequence(fakeMessageQueueProductSelectionGroups, 1);
            int expectedLogCountSuccessful = 2;
            int expectedLogCountFailed = 1;

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Once);
            Assert.AreEqual(MessageTypes.ProductSelectionGroup, actualMonitorLogEntry.MessageTypeID);
            Assert.AreEqual(expectedLogCountSuccessful, actualMonitorLogEntry.CountProcessedMessages.GetValueOrDefault(0));
            Assert.AreEqual(expectedLogCountFailed, actualMonitorLogEntry.CountFailedMessages.GetValueOrDefault(0));
            Assert.IsNotNull(actualMonitorLogEntry.StartTime);
            Assert.IsNotNull(actualMonitorLogEntry.EndTime);
            Assert.IsTrue(actualMonitorLogEntry.EndTime >= actualMonitorLogEntry.StartTime);
        }

        [TestMethod]
        public void ProcessQueuedProductSelectionGroupEvents_WhenSerializationErrorForAll_ShouldCallJobMonitor_WithExpectedValues()
        {
            // Given.
            var fakeMessageQueueProductSelectionGroups = new List<MessageQueueProductSelectionGroup> {
                TestHelpers.GetFakeMessageQueueProductSelectionGroup(1),
                TestHelpers.GetFakeMessageQueueProductSelectionGroup(2),
                TestHelpers.GetFakeMessageQueueProductSelectionGroup(3)
            };
            SetupMockMessageQueueSequence(fakeMessageQueueProductSelectionGroups, new List<int>() { 0, 1, 2 });
            int expectedLogCountSuccessful = 0;
            int expectedLogCountFailed = 3;

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Once);
            Assert.AreEqual(MessageTypes.ProductSelectionGroup, actualMonitorLogEntry.MessageTypeID);
            Assert.AreEqual(expectedLogCountSuccessful, actualMonitorLogEntry.CountProcessedMessages.GetValueOrDefault(0));
            Assert.AreEqual(expectedLogCountFailed, actualMonitorLogEntry.CountFailedMessages.GetValueOrDefault(0));
            Assert.IsNotNull(actualMonitorLogEntry.StartTime);
            Assert.IsNotNull(actualMonitorLogEntry.EndTime);
            Assert.IsTrue(actualMonitorLogEntry.EndTime >= actualMonitorLogEntry.StartTime);
        }
    }
}
