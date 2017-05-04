using Icon.ApiController.Common;
using Icon.ApiController.Controller.CollectionProcessors;
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

namespace Icon.ApiController.Tests.QueueProcessors
{
    [TestClass]
    public class ItemLocaleQueueProcessorTests
    {
        private ItemLocaleQueueProcessor queueProcessor;

        private Mock<ILogger<ItemLocaleQueueProcessor>> mockLogger;
        private Mock<ISerializer<Contracts.items>> mockSerializer;
        private Mock<IQueueReader<MessageQueueItemLocale, Contracts.items>> mockQueueReader;
        private Mock<ICollectionProcessor<List<int>>> mockProductMessageProcessor;
        private Mock<ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>>> mockSaveToMessageHistoryCommandHandler;
        private Mock<ICommandHandler<AssociateMessageToQueueCommand<MessageQueueItemLocale, MessageHistory>>> mockAssociateMessageToQueueCommandHandler;
        private Mock<ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueueItemLocale>>> mockUpdateProcessedDateCommandHandler;
        private Mock<ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>> mockUpdateMessageHistoryCommandHandler;
        private Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueItemLocale>>> mockUpdateMessageQueueStatusCommandHandler;
        private Mock<ICommandHandler<UpdateInProcessBusinessUnitCommand>> mockUpdateInProcessBusinessUnitCommandHandler;
        private Mock<ICommandHandler<ClearBusinessUnitInProcessCommand>> mockClearBusinessUnitInProcessCommandHandler;
        private Mock<IQueryHandler<GetNextAvailableBusinessUnitParameters, int?>> mockGetNextAvailableBusinessUnitQueryHandler;
        private Mock<ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueItemLocale>>> mockMarkQueuedEntriesAsInProcessCommandHandler;
        private Mock<IEsbProducer> mockProducer;
        private Mock<IMessageProcessorMonitor> mockMonitor;
        private APIMessageProcessorLogEntry actualMonitorLogEntry;
        private ApiControllerSettings settings;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger<ItemLocaleQueueProcessor>>();
            mockQueueReader = new Mock<IQueueReader<MessageQueueItemLocale, Contracts.items>>();
            mockSerializer = new Mock<ISerializer<Contracts.items>>();
            mockProductMessageProcessor = new Mock<ICollectionProcessor<List<int>>>();
            mockSaveToMessageHistoryCommandHandler = new Mock<ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>>>();
            mockAssociateMessageToQueueCommandHandler = new Mock<ICommandHandler<AssociateMessageToQueueCommand<MessageQueueItemLocale, MessageHistory>>>();
            mockUpdateProcessedDateCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueueItemLocale>>>();
            mockUpdateMessageHistoryCommandHandler = new Mock<ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>>();
            mockUpdateMessageQueueStatusCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueItemLocale>>>();
            mockUpdateInProcessBusinessUnitCommandHandler = new Mock<ICommandHandler<UpdateInProcessBusinessUnitCommand>>();
            mockClearBusinessUnitInProcessCommandHandler = new Mock<ICommandHandler<ClearBusinessUnitInProcessCommand>>();
            mockGetNextAvailableBusinessUnitQueryHandler = new Mock<IQueryHandler<GetNextAvailableBusinessUnitParameters, int?>>();
            mockMarkQueuedEntriesAsInProcessCommandHandler = new Mock<ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueItemLocale>>>();
            mockProducer = new Mock<IEsbProducer>();
            settings = new ApiControllerSettings() { ProcessLinkedItems = true };
            mockMonitor = new Mock<IMessageProcessorMonitor>();
            actualMonitorLogEntry = new APIMessageProcessorLogEntry();
            //set up the mock's RecordResults() method with a callback so that we can examine the data passed to it
            mockMonitor.Setup(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()))
                .Callback<APIMessageProcessorLogEntry>(methodData => actualMonitorLogEntry = methodData);

            queueProcessor = new ItemLocaleQueueProcessor(
                settings,
                mockLogger.Object,
                mockQueueReader.Object,
                mockSerializer.Object,
                mockProductMessageProcessor.Object,
                mockSaveToMessageHistoryCommandHandler.Object,
                mockAssociateMessageToQueueCommandHandler.Object,
                mockUpdateProcessedDateCommandHandler.Object,
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
        public void ProcessQueuedItemLocaleEvents_NoQueuedMessages_GroupMessagesShouldNotBeCalled()
        {
            // Given.
            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(new List<MessageQueueItemLocale>());

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockQueueReader.Verify(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueItemLocale>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedItemLocaleEvents_GetGroupedMessagesReturnsEmpty_BuildMiniBulkShouldNotBeCalled()
        {
            // Given.
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale> { TestHelpers.GetFakeMessageQueueItemLocale(1, 1, ItemTypeCodes.RetailSale) };
            var fakeMessageQueueItemLocalesEmpty = new List<MessageQueueItemLocale>();

            var queuedMessages = new Queue<List<MessageQueueItemLocale>>();
            queuedMessages.Enqueue(fakeMessageQueueItemLocales);
            queuedMessages.Enqueue(fakeMessageQueueItemLocalesEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueItemLocale>>())).Returns(new List<MessageQueueItemLocale>());

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockQueueReader.Verify(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueItemLocale>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedItemLocaleEvents_MiniBulkReturnsEmpty_SerializeShouldNotBeCalled()
        {
            // Given.
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale> { TestHelpers.GetFakeMessageQueueItemLocale(1, 1, ItemTypeCodes.RetailSale) };
            var fakeMessageQueueItemLocalesEmpty = new List<MessageQueueItemLocale>();

            var queuedMessages = new Queue<List<MessageQueueItemLocale>>();
            queuedMessages.Enqueue(fakeMessageQueueItemLocales);
            queuedMessages.Enqueue(fakeMessageQueueItemLocalesEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueItemLocale>>())).Returns(fakeMessageQueueItemLocales);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueItemLocale>>())).Returns(new Contracts.items { item = new Contracts.ItemType[0] });

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockSerializer.Verify(s => s.Serialize(It.IsAny<Contracts.items>(), It.IsAny<TextWriter>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedItemLocaleEvents_MiniBulkContainsNoLinkedItems_GenerateMessagesShouldNotBeCalled()
        {
            // Given.
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale> { TestHelpers.GetFakeMessageQueueItemLocale(1, 1, ItemTypeCodes.RetailSale) };
            var fakeMessageQueueItemLocalesEmpty = new List<MessageQueueItemLocale>();

            var queuedMessages = new Queue<List<MessageQueueItemLocale>>();
            queuedMessages.Enqueue(fakeMessageQueueItemLocales);
            queuedMessages.Enqueue(fakeMessageQueueItemLocalesEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueItemLocale>>())).Returns(fakeMessageQueueItemLocales);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueItemLocale>>())).Returns(TestHelpers.GetFakeItemLocaleMiniBulk(false, ItemTypeCodes.RetailSale));

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockProductMessageProcessor.Verify(mg => mg.GenerateMessages(It.IsAny<List<int>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedItemLocaleEvents_MiniBulkContainsLinkedItems_GenerateMessagesShouldBeCalledOnce()
        {
            // Given.
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale> { TestHelpers.GetFakeMessageQueueItemLocale(1, 1, ItemTypeCodes.RetailSale) };
            var fakeMessageQueueItemLocalesEmpty = new List<MessageQueueItemLocale>();

            var queuedMessages = new Queue<List<MessageQueueItemLocale>>();
            queuedMessages.Enqueue(fakeMessageQueueItemLocales);
            queuedMessages.Enqueue(fakeMessageQueueItemLocalesEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueItemLocale>>())).Returns(fakeMessageQueueItemLocales);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueItemLocale>>())).Returns(TestHelpers.GetFakeItemLocaleMiniBulk(true, ItemTypeCodes.RetailSale));

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockProductMessageProcessor.Verify(mg => mg.GenerateMessages(It.IsAny<List<int>>()), Times.Once);
        }

        [TestMethod]
        public void ProcessQueuedItemLocaleEvents_MiniBulkContainsLinkedItemsButLinkedItemProcessingIsDisabled_GenerateMessagesShouldBeCalledOnce()
        {
            // Given.
            settings.ProcessLinkedItems = false;

            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale> { TestHelpers.GetFakeMessageQueueItemLocale(1, 1, ItemTypeCodes.RetailSale) };
            var fakeMessageQueueItemLocalesEmpty = new List<MessageQueueItemLocale>();

            var queuedMessages = new Queue<List<MessageQueueItemLocale>>();
            queuedMessages.Enqueue(fakeMessageQueueItemLocales);
            queuedMessages.Enqueue(fakeMessageQueueItemLocalesEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueItemLocale>>())).Returns(fakeMessageQueueItemLocales);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueItemLocale>>())).Returns(TestHelpers.GetFakeItemLocaleMiniBulk(true, ItemTypeCodes.RetailSale));

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockProductMessageProcessor.Verify(mg => mg.GenerateMessages(It.IsAny<List<int>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedItemLocaleEvents_MiniBulkContainsNoBottleReturns_GenerateMessagesShouldNotBeCalled()
        {
            // Given.
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale> { TestHelpers.GetFakeMessageQueueItemLocale(1, 1, ItemTypeCodes.RetailSale) };
            var fakeMessageQueueItemLocalesEmpty = new List<MessageQueueItemLocale>();

            var queuedMessages = new Queue<List<MessageQueueItemLocale>>();
            queuedMessages.Enqueue(fakeMessageQueueItemLocales);
            queuedMessages.Enqueue(fakeMessageQueueItemLocalesEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueItemLocale>>())).Returns(fakeMessageQueueItemLocales);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueItemLocale>>())).Returns(TestHelpers.GetFakeItemLocaleMiniBulk(false, ItemTypeCodes.RetailSale));

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockProductMessageProcessor.Verify(mg => mg.GenerateMessages(It.IsAny<List<int>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedItemLocaleEvents_MiniBulkContainsBottleReturns_GenerateMessagesShouldBeCalledOnce()
        {
            // Given.
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale> { TestHelpers.GetFakeMessageQueueItemLocale(1, 1, ItemTypeCodes.Return) };
            var fakeMessageQueueItemLocalesEmpty = new List<MessageQueueItemLocale>();

            var queuedMessages = new Queue<List<MessageQueueItemLocale>>();
            queuedMessages.Enqueue(fakeMessageQueueItemLocales);
            queuedMessages.Enqueue(fakeMessageQueueItemLocalesEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueItemLocale>>())).Returns(fakeMessageQueueItemLocales);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueItemLocale>>())).Returns(TestHelpers.GetFakeItemLocaleMiniBulk(true, ItemTypeCodes.Return));
            
            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockProductMessageProcessor.Verify(mg => mg.GenerateMessages(It.IsAny<List<int>>()), Times.Once);
        }

        [TestMethod]
        public void ProcessQueuedItemLocaleEvents_SerializationError_UpdateMessageQueueStatusCommandShouldBeCalled()
        {
            // Given.
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale> { TestHelpers.GetFakeMessageQueueItemLocale(1, 1, ItemTypeCodes.RetailSale) };
            var fakeMessageQueueItemLocalesEmpty = new List<MessageQueueItemLocale>();

            var queuedMessages = new Queue<List<MessageQueueItemLocale>>();
            queuedMessages.Enqueue(fakeMessageQueueItemLocales);
            queuedMessages.Enqueue(fakeMessageQueueItemLocalesEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueItemLocale>>())).Returns(fakeMessageQueueItemLocales);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueItemLocale>>())).Returns(TestHelpers.GetFakeItemLocaleMiniBulk(false, ItemTypeCodes.RetailSale));
            mockSerializer.Setup(s => s.Serialize(It.IsAny<Contracts.items>(), It.IsAny<TextWriter>())).Returns(string.Empty);

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockUpdateMessageQueueStatusCommandHandler.Verify(c => c.Execute(It.IsAny<UpdateMessageQueueStatusCommand<MessageQueueItemLocale>>()), Times.Once);
            mockSaveToMessageHistoryCommandHandler.Verify(c => c.Execute(It.IsAny<SaveToMessageHistoryCommand<MessageHistory>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedItemLocaleEvents_NoErrors_UpdateMessageHistoryCommandShouldBeCalled()
        {
            // Given.
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale> { TestHelpers.GetFakeMessageQueueItemLocale(1, 1, ItemTypeCodes.RetailSale) };
            var fakeMessageQueueItemLocalesEmpty = new List<MessageQueueItemLocale>();

            var queuedMessages = new Queue<List<MessageQueueItemLocale>>();
            queuedMessages.Enqueue(fakeMessageQueueItemLocales);
            queuedMessages.Enqueue(fakeMessageQueueItemLocalesEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueItemLocale>>())).Returns(fakeMessageQueueItemLocales);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueItemLocale>>())).Returns(TestHelpers.GetFakeItemLocaleMiniBulk(false, ItemTypeCodes.RetailSale));
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
            mockLogger.Verify(m => m.Info(It.Is<string>(s => s == "Ending the ItemLocale queue processor.  No further queued messages were found in Ready status.")), Times.Once);
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
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale> { TestHelpers.GetFakeMessageQueueItemLocale(1, 1, ItemTypeCodes.RetailSale) };

            mockQueueReader.SetupSequence(qr => qr.GetQueuedMessages())
                .Returns(fakeMessageQueueItemLocales)
                .Returns(new List<MessageQueueItemLocale>())
                .Returns(fakeMessageQueueItemLocales)
                .Returns(new List<MessageQueueItemLocale>())
                .Returns(fakeMessageQueueItemLocales)
                .Returns(new List<MessageQueueItemLocale>());
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueItemLocale>>())).Returns(fakeMessageQueueItemLocales);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueItemLocale>>())).Returns(TestHelpers.GetFakeItemLocaleMiniBulk(false, ItemTypeCodes.RetailSale));
            mockSerializer.Setup(s => s.Serialize(It.IsAny<Contracts.items>(), It.IsAny<TextWriter>())).Returns("Test");
            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));

            //When
            queueProcessor.ProcessMessageQueue();

            //Then
            mockUpdateInProcessBusinessUnitCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateInProcessBusinessUnitCommand>()), Times.Exactly(3));
            mockProducer.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Exactly(3));
            mockLogger.Verify(m => m.Info(It.Is<string>(s => s == "Ending the ItemLocale queue processor.  No further queued messages were found in Ready status.")), Times.Once);
            mockClearBusinessUnitInProcessCommandHandler.Verify(m => m.Execute(It.IsAny<ClearBusinessUnitInProcessCommand>()), Times.Exactly(3));
        }

        [TestMethod]
        public void ProcessQueuedItemLocaleEvents_WhenNothingQueued_ShouldNotCallJobMonitor()
        {
            // Given.
            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(new List<MessageQueueItemLocale>());

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Never);
        }

        private void SetupMockMessageQueueSequence(List<MessageQueueItemLocale> messageQueue, int sequenceIndexToFailSerialization)
        {
            SetupMockMessageQueueSequence(messageQueue, new List<int> { sequenceIndexToFailSerialization });
        }

        private void SetupMockMessageQueueSequence(List<MessageQueueItemLocale> messageQueue, List<int> sequenceIndicesToFailSerialization = null)
        {
            if (sequenceIndicesToFailSerialization == null) sequenceIndicesToFailSerialization = new List<int>();
            mockQueueReader.SetupSequence(qr => qr.GetQueuedMessages())
                .Returns(messageQueue.Count > 0 ? new List<MessageQueueItemLocale>() { messageQueue[0] } : new List<MessageQueueItemLocale>())
                .Returns(messageQueue.Count > 1 ? new List<MessageQueueItemLocale>() { messageQueue[1] } : new List<MessageQueueItemLocale>())
                .Returns(messageQueue.Count > 2 ? new List<MessageQueueItemLocale>() { messageQueue[2] } : new List<MessageQueueItemLocale>())
                .Returns(messageQueue.Count > 3 ? new List<MessageQueueItemLocale>() { messageQueue[3] } : new List<MessageQueueItemLocale>())
                .Returns(messageQueue.Count > 4 ? new List<MessageQueueItemLocale>() { messageQueue[4] } : new List<MessageQueueItemLocale>());
            mockQueueReader.SetupSequence(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueItemLocale>>()))
                .Returns(messageQueue.Count > 0 ? new List<MessageQueueItemLocale>() { messageQueue[0] } : new List<MessageQueueItemLocale>())
                .Returns(messageQueue.Count > 1 ? new List<MessageQueueItemLocale>() { messageQueue[1] } : new List<MessageQueueItemLocale>())
                .Returns(messageQueue.Count > 2 ? new List<MessageQueueItemLocale>() { messageQueue[2] } : new List<MessageQueueItemLocale>())
                .Returns(messageQueue.Count > 3 ? new List<MessageQueueItemLocale>() { messageQueue[3] } : new List<MessageQueueItemLocale>())
                .Returns(messageQueue.Count > 4 ? new List<MessageQueueItemLocale>() { messageQueue[4] } : new List<MessageQueueItemLocale>());
            mockQueueReader.SetupSequence(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueItemLocale>>()))
                .Returns(messageQueue.Count > 0 ? TestHelpers.GetFakeItemLocaleMiniBulk(false, ItemTypeCodes.RetailSale, messageQueue[0].ItemId) : null)
                .Returns(messageQueue.Count > 1 ? TestHelpers.GetFakeItemLocaleMiniBulk(false, ItemTypeCodes.RetailSale, messageQueue[1].ItemId) : null)
                .Returns(messageQueue.Count > 2 ? TestHelpers.GetFakeItemLocaleMiniBulk(false, ItemTypeCodes.RetailSale, messageQueue[2].ItemId) : null)
                .Returns(messageQueue.Count > 3 ? TestHelpers.GetFakeItemLocaleMiniBulk(false, ItemTypeCodes.RetailSale, messageQueue[3].ItemId) : null)
                .Returns(messageQueue.Count > 4 ? TestHelpers.GetFakeItemLocaleMiniBulk(false, ItemTypeCodes.RetailSale, messageQueue[4].ItemId) : null);
            mockSerializer.SetupSequence(s => s.Serialize(It.IsAny<Contracts.items>(), It.IsAny<TextWriter>()))
                .Returns(messageQueue.Count > 0 && !sequenceIndicesToFailSerialization.Contains(0) ? "Test1" : null)
                .Returns(messageQueue.Count > 1 && !sequenceIndicesToFailSerialization.Contains(1) ? "Test2" : null)
                .Returns(messageQueue.Count > 2 && !sequenceIndicesToFailSerialization.Contains(2) ? "Test3" : null)
                .Returns(messageQueue.Count > 3 && !sequenceIndicesToFailSerialization.Contains(3) ? "Test4" : null)
                .Returns(messageQueue.Count > 4 && !sequenceIndicesToFailSerialization.Contains(4) ? "Test5" : null);
            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
        }

        [TestMethod]
        public void ProcessQueuedItemLocaleEvents_WhenSuccessful_ShouldCallJobMonitor_WithExpectedValues()
        {
            // Given.
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale> {
                TestHelpers.GetFakeMessageQueueItemLocale(1, 1, ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueItemLocale(2, 2, ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueItemLocale(3, 3, ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueItemLocale(4, 4, ItemTypeCodes.RetailSale)
            };
            SetupMockMessageQueueSequence(fakeMessageQueueItemLocales);
            int expectedLogCountSuccessful = fakeMessageQueueItemLocales.Count;
            int expectedLogCountFailed = 0;            

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Once);
            Assert.AreEqual(MessageTypes.ItemLocale, actualMonitorLogEntry.MessageTypeID);
            Assert.AreEqual(expectedLogCountSuccessful, actualMonitorLogEntry.CountProcessedMessages.GetValueOrDefault(0));
            Assert.AreEqual(expectedLogCountFailed, actualMonitorLogEntry.CountFailedMessages.GetValueOrDefault(0));
            Assert.IsNotNull(actualMonitorLogEntry.StartTime);
            Assert.IsNotNull(actualMonitorLogEntry.EndTime);
            Assert.IsTrue(actualMonitorLogEntry.EndTime >= actualMonitorLogEntry.StartTime);
        }

        [TestMethod]
        public void ProcessQueuedItemLocaleEvents_WhenSerializationErrorForOne_ShouldCallJobMonitor_WithExpectedValues()
        {
            // Given.
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale> {
                TestHelpers.GetFakeMessageQueueItemLocale(1, 1, ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueItemLocale(2, 2, ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueItemLocale(3, 3, ItemTypeCodes.RetailSale)
            };
            SetupMockMessageQueueSequence(fakeMessageQueueItemLocales, 1);
            int expectedLogCountSuccessful = 2;
            int expectedLogCountFailed = 1;

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Once);
            Assert.AreEqual(MessageTypes.ItemLocale, actualMonitorLogEntry.MessageTypeID);
            Assert.AreEqual(expectedLogCountSuccessful, actualMonitorLogEntry.CountProcessedMessages.GetValueOrDefault(0));
            Assert.AreEqual(expectedLogCountFailed, actualMonitorLogEntry.CountFailedMessages.GetValueOrDefault(0));
            Assert.IsNotNull(actualMonitorLogEntry.StartTime);
            Assert.IsNotNull(actualMonitorLogEntry.EndTime);
            Assert.IsTrue(actualMonitorLogEntry.EndTime >= actualMonitorLogEntry.StartTime);
        }

        [TestMethod]
        public void ProcessQueuedItemLocaleEvents_WhenSerializationErrorForAll_ShouldCallJobMonitor_WithExpectedValues()
        {
            // Given.
            var fakeMessageQueueItemLocales = new List<MessageQueueItemLocale> {
                TestHelpers.GetFakeMessageQueueItemLocale(1, 1, ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueItemLocale(2, 2, ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueItemLocale(3, 3, ItemTypeCodes.RetailSale)
            };
            SetupMockMessageQueueSequence(fakeMessageQueueItemLocales, new List<int>() { 0, 1, 2 });
            int expectedLogCountSuccessful = 0;
            int expectedLogCountFailed = 3;

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Once);
            Assert.AreEqual(MessageTypes.ItemLocale, actualMonitorLogEntry.MessageTypeID);
            Assert.AreEqual(expectedLogCountSuccessful, actualMonitorLogEntry.CountProcessedMessages.GetValueOrDefault(0));
            Assert.AreEqual(expectedLogCountFailed, actualMonitorLogEntry.CountFailedMessages.GetValueOrDefault(0));
            Assert.IsNotNull(actualMonitorLogEntry.StartTime);
            Assert.IsNotNull(actualMonitorLogEntry.EndTime);
            Assert.IsTrue(actualMonitorLogEntry.EndTime >= actualMonitorLogEntry.StartTime);
        }
    }
}
