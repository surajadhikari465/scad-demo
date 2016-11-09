using Icon.ApiController.Controller.QueueProcessors;
using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.Controller.Serializers;
using Icon.ApiController.DataAccess.Commands;
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
using Icon.ApiController.Common;
using Icon.ApiController.DataAccess.Queries;
using Icon.ApiController.Controller.Monitoring;
using System.Linq;

namespace Icon.ApiController.Tests.QueueProcessors
{
    [TestClass]
    public class PriceQueueProcessorTests
    {
        private PriceQueueProcessor queueProcessor;
        private Mock<ILogger<PriceQueueProcessor>> mockLogger;
        private Mock<IRenewableContext<IconContext>> mockContext;
        private Mock<ISerializer<Contracts.items>> mockSerializer;
        private Mock<IQueueReader<MessageQueuePrice, Contracts.items>> mockQueueReader;
        private Mock<ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>>> mockSaveToMessageHistoryCommandHandler;
        private Mock<ICommandHandler<AssociateMessageToQueueCommand<MessageQueuePrice, MessageHistory>>> mockAssociateMessageToQueueCommandHandler;
        private Mock<ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueuePrice>>> mockSetProcessedDateCommandHandler;
        private Mock<ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>> mockUpdateMessageHistoryCommandHandler;
        private Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueuePrice>>> mockUpdateMessageQueueStatusCommandHandler;
        private Mock<ICommandHandler<UpdateInProcessBusinessUnitCommand>> mockUpdateInProcessBusinessUnitCommandHandler;
        private Mock<ICommandHandler<ClearBusinessUnitInProcessCommand>> mockClearBusinessUnitInProcessCommandHandler;
        private Mock<IQueryHandler<GetNextAvailableBusinessUnitParameters, int?>> mockGetNextAvailableBusinessUnitQueryHandler;
        private Mock<ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueuePrice>>> mockMarkQueuedEntriesAsInProcessCommandHandler;
        private Mock<IEsbProducer> mockProducer;
        private ApiControllerSettings settings;
        private Mock<IMessageProcessorMonitor> mockMonitor;
        private APIMessageProcessorLogEntry actualMonitorLogEntry;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger<PriceQueueProcessor>>();
            mockContext = new Mock<IRenewableContext<IconContext>>();
            mockSerializer = new Mock<ISerializer<Contracts.items>>();
            mockQueueReader = new Mock<IQueueReader<MessageQueuePrice, Contracts.items>>();
            mockSaveToMessageHistoryCommandHandler = new Mock<ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>>>();
            mockAssociateMessageToQueueCommandHandler = new Mock<ICommandHandler<AssociateMessageToQueueCommand<MessageQueuePrice, MessageHistory>>>();
            mockSetProcessedDateCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueuePrice>>>();
            mockUpdateMessageHistoryCommandHandler = new Mock<ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>>();
            mockUpdateMessageQueueStatusCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueuePrice>>>();
            mockProducer = new Mock<IEsbProducer>();
            settings = new ApiControllerSettings();
            mockUpdateInProcessBusinessUnitCommandHandler = new Mock<ICommandHandler<UpdateInProcessBusinessUnitCommand>>();
            mockClearBusinessUnitInProcessCommandHandler = new Mock<ICommandHandler<ClearBusinessUnitInProcessCommand>>();
            mockGetNextAvailableBusinessUnitQueryHandler = new Mock<IQueryHandler<GetNextAvailableBusinessUnitParameters, int?>>();
            mockMarkQueuedEntriesAsInProcessCommandHandler = new Mock<ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueuePrice>>>();
            mockMonitor = new Mock<IMessageProcessorMonitor>();
            actualMonitorLogEntry = new APIMessageProcessorLogEntry();
            //set up the mock's RecordResults() method with a callback so that we can examine the data passed to it
            mockMonitor.Setup(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()))
                .Callback<APIMessageProcessorLogEntry>(methodData => actualMonitorLogEntry = methodData);

            queueProcessor = new PriceQueueProcessor(
                settings,
                mockLogger.Object,
                mockContext.Object,
                mockQueueReader.Object,
                mockSerializer.Object,
                mockSaveToMessageHistoryCommandHandler.Object,
                mockAssociateMessageToQueueCommandHandler.Object,
                mockSetProcessedDateCommandHandler.Object,
                mockUpdateMessageHistoryCommandHandler.Object,
                mockUpdateMessageQueueStatusCommandHandler.Object,
                mockUpdateInProcessBusinessUnitCommandHandler.Object,
                mockClearBusinessUnitInProcessCommandHandler.Object,
                mockGetNextAvailableBusinessUnitQueryHandler.Object,
                mockMarkQueuedEntriesAsInProcessCommandHandler.Object,
                mockProducer.Object,
                mockMonitor.Object);

            mockGetNextAvailableBusinessUnitQueryHandler.SetupSequence(m => m.Search(It.IsAny<GetNextAvailableBusinessUnitParameters>()))
                .Returns(12345)
                .Returns(null);
        }

        [TestMethod]
        public void ProcessQueuedPriceEvents_NoQueuedMessages_GroupMessagesShouldNotBeCalled()
        {
            // Given.
            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(new List<MessageQueuePrice>());

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockQueueReader.Verify(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueuePrice>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedPriceEvents_GetGroupedMessagesReturnsEmpty_BuildMiniBulkShouldNotBeCalled()
        {
            // Given.
            var fakeMessageQueuePrices = new List<MessageQueuePrice> { TestHelpers.GetFakeMessageQueuePrice(1, 1, 1m, 1m, 1m) };
            var fakeMessageQueuePricesEmpty = new List<MessageQueuePrice>();

            var queuedMessages = new Queue<List<MessageQueuePrice>>();
            queuedMessages.Enqueue(fakeMessageQueuePrices);
            queuedMessages.Enqueue(fakeMessageQueuePricesEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueuePrice>>())).Returns(new List<MessageQueuePrice>());

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockQueueReader.Verify(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueuePrice>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedPriceEvents_MiniBulkReturnsEmpty_SerializeShouldNotBeCalled()
        {
            // Given.
            var fakeMessageQueuePrices = new List<MessageQueuePrice> { TestHelpers.GetFakeMessageQueuePrice(1, 1, 1m, 1m, 1m) };
            var fakeMessageQueuePricesEmpty = new List<MessageQueuePrice>();

            var queuedMessages = new Queue<List<MessageQueuePrice>>();
            queuedMessages.Enqueue(fakeMessageQueuePrices);
            queuedMessages.Enqueue(fakeMessageQueuePricesEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueuePrice>>())).Returns(fakeMessageQueuePrices);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueuePrice>>())).Returns(new Contracts.items { item = new Contracts.ItemType[0] });

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockSerializer.Verify(s => s.Serialize(It.IsAny<Contracts.items>(), It.IsAny<TextWriter>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedPriceEvents_SerializationError_UpdateMessageQueueStatusCommandShouldBeCalled()
        {
            // Given.
            var fakeMessageQueuePrices = new List<MessageQueuePrice> { TestHelpers.GetFakeMessageQueuePrice(1, 1, 1m, 1m, 1m) };
            var fakeMessageQueuePricesEmpty = new List<MessageQueuePrice>();

            var queuedMessages = new Queue<List<MessageQueuePrice>>();
            queuedMessages.Enqueue(fakeMessageQueuePrices);
            queuedMessages.Enqueue(fakeMessageQueuePricesEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueuePrice>>())).Returns(fakeMessageQueuePrices);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueuePrice>>())).Returns(new Contracts.items { item = new Contracts.ItemType[] { new Contracts.ItemType { id = 1 } } });
            mockSerializer.Setup(s => s.Serialize(It.IsAny<Contracts.items>(), It.IsAny<TextWriter>())).Returns(String.Empty);

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockUpdateMessageQueueStatusCommandHandler.Verify(c => c.Execute(It.IsAny<UpdateMessageQueueStatusCommand<MessageQueuePrice>>()), Times.Once);
            mockSaveToMessageHistoryCommandHandler.Verify(c => c.Execute(It.IsAny<SaveToMessageHistoryCommand<MessageHistory>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedPriceEvents_NoErrors_UpdateMessageHistoryCommandShouldBeCalled()
        {
            // Given.
            var fakeMessageQueuePrices = new List<MessageQueuePrice> { TestHelpers.GetFakeMessageQueuePrice(1, 1, 1m, 1m, 1m) };
            var fakeMessageQueuePricesEmpty = new List<MessageQueuePrice>();

            var queuedMessages = new Queue<List<MessageQueuePrice>>();
            queuedMessages.Enqueue(fakeMessageQueuePrices);
            queuedMessages.Enqueue(fakeMessageQueuePricesEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueuePrice>>())).Returns(fakeMessageQueuePrices);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueuePrice>>())).Returns(new Contracts.items { item = new Contracts.ItemType[] { new Contracts.ItemType { id = 1 } } });
            mockSerializer.Setup(s => s.Serialize(It.IsAny<Contracts.items>(), It.IsAny<TextWriter>())).Returns("Test");
            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockUpdateMessageHistoryCommandHandler.Verify(c => c.Execute(It.IsAny<UpdateMessageHistoryStatusCommand<MessageHistory>>()), Times.Once);
        }

        [TestMethod]
        public void ProcessMessageQueue_BusinessUnitIsNull_ShouldNotTryToProcessMesssages()
        {
            //Given
            mockGetNextAvailableBusinessUnitQueryHandler.Setup(m => m.Search(It.IsAny<GetNextAvailableBusinessUnitParameters>()))
                .Returns((int?)null);

            //When
            queueProcessor.ProcessMessageQueue();

            //Then
            mockUpdateInProcessBusinessUnitCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateInProcessBusinessUnitCommand>()), Times.Never);
            mockProducer.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
            mockLogger.Verify(m => m.Info(It.Is<string>(s => s == "Ending the Price queue processor.  No further queued messages were found in Ready status.")), Times.Once);
        }

        [TestMethod]
        public void ProcessMessageQueue_BusinessUnitIsNullAfterMultipleRuns_ShouldStopProcessingMessagesWhenBusinessUnitIsNull()
        {
            //Given
            mockGetNextAvailableBusinessUnitQueryHandler.SetupSequence(m => m.Search(It.IsAny<GetNextAvailableBusinessUnitParameters>()))
                .Returns(12345)
                .Returns(12345)
                .Returns(12346)
                .Returns((int?)null);
            List<MessageQueuePrice> fakeMessageQueuePrices = new List<MessageQueuePrice> { TestHelpers.GetFakeMessageQueuePrice(1, 1, 1m, 1m, 1m) };

            mockQueueReader.SetupSequence(qr => qr.GetQueuedMessages())
                .Returns(fakeMessageQueuePrices)
                .Returns(new List<MessageQueuePrice>())
                .Returns(fakeMessageQueuePrices)
                .Returns(new List<MessageQueuePrice>())
                .Returns(fakeMessageQueuePrices)
                .Returns(new List<MessageQueuePrice>());
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueuePrice>>())).Returns(fakeMessageQueuePrices);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueuePrice>>())).Returns(new Contracts.items { item = new Contracts.ItemType[] { new Contracts.ItemType { id = 1 } } });
            mockSerializer.Setup(s => s.Serialize(It.IsAny<Contracts.items>(), It.IsAny<TextWriter>())).Returns("Test");
            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));

            //When
            queueProcessor.ProcessMessageQueue();

            //Then
            mockUpdateInProcessBusinessUnitCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateInProcessBusinessUnitCommand>()), Times.Exactly(3));
            mockProducer.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Exactly(3));
            mockLogger.Verify(m => m.Info(It.Is<string>(s => s == "Ending the Price queue processor.  No further queued messages were found in Ready status.")), Times.Once);
            mockClearBusinessUnitInProcessCommandHandler.Verify(m => m.Execute(It.IsAny<ClearBusinessUnitInProcessCommand>()), Times.Exactly(3));
        }

        [TestMethod]
        public void ProcessQueuedPriceEvents_WhenNothingQueued_ShouldNotCallJobMonitor()
        {
            // Given.
            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(new List<MessageQueuePrice>());

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Never);
        }

        private void SetupMockMessageQueueSequence(List<MessageQueuePrice> messageQueue, int sequenceIndexToFailSerialization)
        {
            SetupMockMessageQueueSequence(messageQueue, new List<int> { sequenceIndexToFailSerialization });
        }

        private void SetupMockMessageQueueSequence(List<MessageQueuePrice> messageQueue, List<int> sequenceIndicesToFailSerialization = null)
        {
            if (sequenceIndicesToFailSerialization == null) sequenceIndicesToFailSerialization = new List<int>();
            mockQueueReader.SetupSequence(qr => qr.GetQueuedMessages())
                .Returns(messageQueue.Count > 0 ? new List<MessageQueuePrice>() { messageQueue[0] } : new List<MessageQueuePrice>())
                .Returns(messageQueue.Count > 1 ? new List<MessageQueuePrice>() { messageQueue[1] } : new List<MessageQueuePrice>())
                .Returns(messageQueue.Count > 2 ? new List<MessageQueuePrice>() { messageQueue[2] } : new List<MessageQueuePrice>())
                .Returns(messageQueue.Count > 3 ? new List<MessageQueuePrice>() { messageQueue[3] } : new List<MessageQueuePrice>())
                .Returns(messageQueue.Count > 4 ? new List<MessageQueuePrice>() { messageQueue[4] } : new List<MessageQueuePrice>());
            mockQueueReader.SetupSequence(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueuePrice>>()))
                .Returns(messageQueue.Count > 0 ? new List<MessageQueuePrice>() { messageQueue[0] } : new List<MessageQueuePrice>())
                .Returns(messageQueue.Count > 1 ? new List<MessageQueuePrice>() { messageQueue[1] } : new List<MessageQueuePrice>())
                .Returns(messageQueue.Count > 2 ? new List<MessageQueuePrice>() { messageQueue[2] } : new List<MessageQueuePrice>())
                .Returns(messageQueue.Count > 3 ? new List<MessageQueuePrice>() { messageQueue[3] } : new List<MessageQueuePrice>())
                .Returns(messageQueue.Count > 4 ? new List<MessageQueuePrice>() { messageQueue[4] } : new List<MessageQueuePrice>());
            mockQueueReader.SetupSequence(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueuePrice>>()))
                .Returns(messageQueue.Count > 0 ? new Contracts.items { item = new Contracts.ItemType[] { new Contracts.ItemType { id = messageQueue[0].ItemId } } } : null)
                .Returns(messageQueue.Count > 1 ? new Contracts.items { item = new Contracts.ItemType[] { new Contracts.ItemType { id = messageQueue[1].ItemId } } } : null)
                .Returns(messageQueue.Count > 2 ? new Contracts.items { item = new Contracts.ItemType[] { new Contracts.ItemType { id = messageQueue[2].ItemId } } } : null)
                .Returns(messageQueue.Count > 3 ? new Contracts.items { item = new Contracts.ItemType[] { new Contracts.ItemType { id = messageQueue[3].ItemId } } } : null)
                .Returns(messageQueue.Count > 4 ? new Contracts.items { item = new Contracts.ItemType[] { new Contracts.ItemType { id = messageQueue[4].ItemId } } } : null);
            mockSerializer.SetupSequence(s => s.Serialize(It.IsAny<Contracts.items>(), It.IsAny<TextWriter>()))
                .Returns(messageQueue.Count > 0 && !sequenceIndicesToFailSerialization.Contains(0) ? "Test1" : null)
                .Returns(messageQueue.Count > 1 && !sequenceIndicesToFailSerialization.Contains(1) ? "Test2" : null)
                .Returns(messageQueue.Count > 2 && !sequenceIndicesToFailSerialization.Contains(2) ? "Test3" : null)
                .Returns(messageQueue.Count > 3 && !sequenceIndicesToFailSerialization.Contains(3) ? "Test4" : null)
                .Returns(messageQueue.Count > 4 && !sequenceIndicesToFailSerialization.Contains(4) ? "Test5" : null);
            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
        }

        [TestMethod]
        public void ProcessQueuedPriceEvents_WhenSuccessful_ShouldCallJobMonitor()
        {
            // Given.
            var fakeMessageQueuePrices = new List<MessageQueuePrice> { TestHelpers.GetFakeMessageQueuePrice(1, 1, 1m, 1m, 1m) };
            SetupMockMessageQueueSequence(fakeMessageQueuePrices);

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Once);
        }

        [TestMethod]
        public void ProcessQueuedPriceEvents_WhenSuccessful_ShouldCallJobMonitor_WithExpectedValues()
        {
            // Given.
            var fakeMessageQueuePrices = new List<MessageQueuePrice> {
                TestHelpers.GetFakeMessageQueuePrice(1, 1, 1m, 1m, 1m),
                TestHelpers.GetFakeMessageQueuePrice(2, 2, 2m, 2m, 2m),
                TestHelpers.GetFakeMessageQueuePrice(3, 3, 3m, 3m, 3m),
                TestHelpers.GetFakeMessageQueuePrice(4, 4, 4m, 4m, 4m)
            };
            SetupMockMessageQueueSequence(fakeMessageQueuePrices);
            int expectedLogCountSuccessful = fakeMessageQueuePrices.Count;
            int expectedLogCountFailed = 0;

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Once);
            Assert.AreEqual(MessageTypes.Price, actualMonitorLogEntry.MessageTypeID);
            Assert.AreEqual(expectedLogCountSuccessful, actualMonitorLogEntry.CountProcessedMessages.GetValueOrDefault(0));
            Assert.AreEqual(expectedLogCountFailed, actualMonitorLogEntry.CountFailedMessages.GetValueOrDefault(0));
            Assert.IsNotNull(actualMonitorLogEntry.StartTime);
            Assert.IsNotNull(actualMonitorLogEntry.EndTime);
            Assert.IsTrue(actualMonitorLogEntry.EndTime >= actualMonitorLogEntry.StartTime);
        }
        
        [TestMethod]
        public void ProcessQueuedPriceEvents_WhenSerializationErrorForOne__ShouldCallJobMonitor_WithExpectedValues()
        {
            // Given.
            var fakeMessageQueuePrices = new List<MessageQueuePrice> {
                TestHelpers.GetFakeMessageQueuePrice(1, 1, 1m, 1m, 1m),
                TestHelpers.GetFakeMessageQueuePrice(2, 2, 2m, 2m, 2m),
                TestHelpers.GetFakeMessageQueuePrice(3, 3, 3m, 3m, 3m)
            };
            SetupMockMessageQueueSequence(fakeMessageQueuePrices, 1);
            int expectedLogCountSuccessful = 2;
            int expectedLogCountFailed = 1;

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Once);
            Assert.AreEqual(MessageTypes.Price, actualMonitorLogEntry.MessageTypeID);
            Assert.AreEqual(expectedLogCountSuccessful, actualMonitorLogEntry.CountProcessedMessages.GetValueOrDefault(0));
            Assert.AreEqual(expectedLogCountFailed, actualMonitorLogEntry.CountFailedMessages.GetValueOrDefault(0));
            Assert.IsNotNull(actualMonitorLogEntry.StartTime);
            Assert.IsNotNull(actualMonitorLogEntry.EndTime);
            Assert.IsTrue(actualMonitorLogEntry.EndTime >= actualMonitorLogEntry.StartTime);
        }

        [TestMethod]
        public void ProcessQueuedPriceEvents_WhenSerializationErrorForAll__ShouldCallJobMonitor_WithExpectedValues()
        {
            // Given.
            var fakeMessageQueuePrices = new List<MessageQueuePrice> {
                TestHelpers.GetFakeMessageQueuePrice(1, 1, 1m, 1m, 1m),
                TestHelpers.GetFakeMessageQueuePrice(2, 2, 2m, 2m, 2m),
                TestHelpers.GetFakeMessageQueuePrice(3, 3, 3m, 3m, 3m)
            };
            SetupMockMessageQueueSequence(fakeMessageQueuePrices, new List<int>() { 0, 1, 2 });
            int expectedLogCountSuccessful = 0;
            int expectedLogCountFailed = 3;

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Once);
            Assert.AreEqual(MessageTypes.Price, actualMonitorLogEntry.MessageTypeID);
            Assert.AreEqual(expectedLogCountSuccessful, actualMonitorLogEntry.CountProcessedMessages.GetValueOrDefault(0));
            Assert.AreEqual(expectedLogCountFailed, actualMonitorLogEntry.CountFailedMessages.GetValueOrDefault(0));
            Assert.IsNotNull(actualMonitorLogEntry.StartTime);
            Assert.IsNotNull(actualMonitorLogEntry.EndTime);
            Assert.IsTrue(actualMonitorLogEntry.EndTime >= actualMonitorLogEntry.StartTime);
        }
    }
}
