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
using Icon.ApiController.Controller.Monitoring;

namespace Icon.ApiController.Tests.QueueProcessors
{
    [TestClass]
    public class ProductQueueProcessorTests
    {
        private ProductQueueProcessor queueProcessor;

        private Mock<ILogger<ProductQueueProcessor>> mockLogger;
        private Mock<IRenewableContext<IconContext>> mockContext;
        private Mock<ISerializer<Contracts.items>> mockSerializer;
        private Mock<IQueueReader<MessageQueueProduct, Contracts.items>> mockQueueReader;
        private Mock<ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>>> mockSaveXmlMessageCommandHandler;
        private Mock<ICommandHandler<AssociateMessageToQueueCommand<MessageQueueProduct, MessageHistory>>> mockAssociateMessageToQueueCommandHandler;
        private Mock<ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueueProduct>>> mockSetProcessedDateCommandHandler;
        private Mock<ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>> mockUpdateMessageHistoryCommandHandler;
        private Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueProduct>>> mockUpdateMessageQueueStatusCommandHandler;
        private Mock<ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueProduct>>> mockMarkQueuedEntriesAsInProcessCommandHandler;
        private Mock<IEsbProducer> mockProducer;
        private ApiControllerSettings settings;
        private Mock<IMessageProcessorMonitor> mockMonitor;
        private APIMessageProcessorLogEntry actualMonitorLogEntry;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger<ProductQueueProcessor>>();
            mockContext = new Mock<IRenewableContext<IconContext>>();
            mockSerializer = new Mock<ISerializer<Contracts.items>>();
            mockQueueReader = new Mock<IQueueReader<MessageQueueProduct, Contracts.items>>();
            mockSaveXmlMessageCommandHandler = new Mock<ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>>>();
            mockAssociateMessageToQueueCommandHandler = new Mock<ICommandHandler<AssociateMessageToQueueCommand<MessageQueueProduct, MessageHistory>>>();
            mockSetProcessedDateCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueueProduct>>>();
            mockUpdateMessageHistoryCommandHandler = new Mock<ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>>();
            mockUpdateMessageQueueStatusCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueProduct>>>();
            mockMarkQueuedEntriesAsInProcessCommandHandler = new Mock<ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueProduct>>>();
            mockProducer = new Mock<IEsbProducer>();
            settings = new ApiControllerSettings();
            mockMonitor = new Mock<IMessageProcessorMonitor>();
            actualMonitorLogEntry = new APIMessageProcessorLogEntry();
            //set up the mock's RecordResults() method with a callback so that we can examine the data passed to it
            mockMonitor.Setup(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()))
                .Callback<APIMessageProcessorLogEntry>(methodData => actualMonitorLogEntry = methodData);

            queueProcessor = new ProductQueueProcessor(
                settings,
                mockLogger.Object,
                mockContext.Object,
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
            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(new List<MessageQueueProduct>());

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockQueueReader.Verify(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueProduct>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedProductEvents_GetGroupedMessagesReturnsEmpty_BuildMiniBulkShouldNotBeCalled()
        {
            // Given.
            var fakeMessageQueueProducts = new List<MessageQueueProduct> { TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.RetailSale) };
            var fakeMessageQueueProductsEmpty = new List<MessageQueueProduct>();

            var queuedMessages = new Queue<List<MessageQueueProduct>>();
            queuedMessages.Enqueue(fakeMessageQueueProducts);
            queuedMessages.Enqueue(fakeMessageQueueProductsEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueProduct>>())).Returns(new List<MessageQueueProduct>());

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockQueueReader.Verify(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueProduct>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedProductEvents_MiniBulkReturnsEmpty_SerializeShouldNotBeCalled()
        {
            // Given.
            var fakeMessageQueueProducts = new List<MessageQueueProduct> { TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.RetailSale) };
            var fakeMessageQueueProductsEmpty = new List<MessageQueueProduct>();

            var queuedMessages = new Queue<List<MessageQueueProduct>>();
            queuedMessages.Enqueue(fakeMessageQueueProducts);
            queuedMessages.Enqueue(fakeMessageQueueProductsEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueProduct>>())).Returns(fakeMessageQueueProducts);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueProduct>>())).Returns(new Contracts.items { item = new Contracts.ItemType[0] });

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockSerializer.Verify(s => s.Serialize(It.IsAny<Contracts.items>(), It.IsAny<TextWriter>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedProductEvents_SerializationError_UpdateMessageQueueStatusCommandShouldBeCalled()
        {
            // Given.
            var fakeMessageQueueProducts = new List<MessageQueueProduct> { TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.RetailSale) };
            var fakeMessageQueueProductsEmpty = new List<MessageQueueProduct>();

            var queuedMessages = new Queue<List<MessageQueueProduct>>();
            queuedMessages.Enqueue(fakeMessageQueueProducts);
            queuedMessages.Enqueue(fakeMessageQueueProductsEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueProduct>>())).Returns(fakeMessageQueueProducts);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueProduct>>())).Returns(new Contracts.items { item = new Contracts.ItemType[] { new Contracts.ItemType { id = 1 } } });
            mockSerializer.Setup(s => s.Serialize(It.IsAny<Contracts.items>(), It.IsAny<TextWriter>())).Returns(String.Empty);

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockUpdateMessageQueueStatusCommandHandler.Verify(c => c.Execute(It.IsAny<UpdateMessageQueueStatusCommand<MessageQueueProduct>>()), Times.Once);
            mockSaveXmlMessageCommandHandler.Verify(c => c.Execute(It.IsAny<SaveToMessageHistoryCommand<MessageHistory>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedProductEvents_NoErrors_UpdateMessageHistoryCommandShouldBeCalled()
        {
            // Given.
            var fakeMessageQueueProducts = new List<MessageQueueProduct> { TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.RetailSale) };
            var fakeMessageQueueProductsEmpty = new List<MessageQueueProduct>();

            var queuedMessages = new Queue<List<MessageQueueProduct>>();
            queuedMessages.Enqueue(fakeMessageQueueProducts);
            queuedMessages.Enqueue(fakeMessageQueueProductsEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueProduct>>())).Returns(fakeMessageQueueProducts);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueProduct>>())).Returns(new Contracts.items { item = new Contracts.ItemType[] { new Contracts.ItemType { id = 1 } } });
            mockSerializer.Setup(s => s.Serialize(It.IsAny<Contracts.items>(), It.IsAny<TextWriter>())).Returns("Test");
            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockUpdateMessageHistoryCommandHandler.Verify(c => c.Execute(It.IsAny<UpdateMessageHistoryStatusCommand<MessageHistory>>()), Times.Once);
        }

        [TestMethod]
        public void ProcessQueuedProductEvents_WhenNothingQueued_ShouldNotCallJobMonitor()
        {
            // Given.
            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(new List<MessageQueueProduct>());

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedProductEvents_WhenSuccessful_ShouldCallJobMonitor()
        {
            // Given.
            var fakeMessageQueueProducts = new List<MessageQueueProduct> { TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.RetailSale) };
            var fakeMessageQueueProductsEmpty = new List<MessageQueueProduct>();

            var queuedMessages = new Queue<List<MessageQueueProduct>>();
            queuedMessages.Enqueue(fakeMessageQueueProducts);
            queuedMessages.Enqueue(fakeMessageQueueProductsEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueProduct>>())).Returns(fakeMessageQueueProducts);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueProduct>>()))
                .Returns(new Contracts.items { item = new Contracts.ItemType[] { new Contracts.ItemType { id = 1 } } });
            mockSerializer.Setup(s => s.Serialize(It.IsAny<Contracts.items>(), It.IsAny<TextWriter>())).Returns("Test");
            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Once);
        }

        private void SetupMockMessageQueueSequence(List<MessageQueueProduct> messageQueue, int sequenceIndexToFailSerialization)
        {
            SetupMockMessageQueueSequence(messageQueue, new List<int> { sequenceIndexToFailSerialization });
        }

        private void SetupMockMessageQueueSequence(List<MessageQueueProduct> messageQueue, List<int> sequenceIndicesToFailSerialization = null)
        {
            if (sequenceIndicesToFailSerialization == null) sequenceIndicesToFailSerialization = new List<int>();
            mockQueueReader.SetupSequence(qr => qr.GetQueuedMessages())
                .Returns(messageQueue.Count > 0 ? new List<MessageQueueProduct>() { messageQueue[0] } : new List<MessageQueueProduct>())
                .Returns(messageQueue.Count > 1 ? new List<MessageQueueProduct>() { messageQueue[1] } : new List<MessageQueueProduct>())
                .Returns(messageQueue.Count > 2 ? new List<MessageQueueProduct>() { messageQueue[2] } : new List<MessageQueueProduct>())
                .Returns(messageQueue.Count > 3 ? new List<MessageQueueProduct>() { messageQueue[3] } : new List<MessageQueueProduct>())
                .Returns(messageQueue.Count > 4 ? new List<MessageQueueProduct>() { messageQueue[4] } : new List<MessageQueueProduct>());
            mockQueueReader.SetupSequence(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueProduct>>()))
                .Returns(messageQueue.Count > 0 ? new List<MessageQueueProduct>() { messageQueue[0] } : new List<MessageQueueProduct>())
                .Returns(messageQueue.Count > 1 ? new List<MessageQueueProduct>() { messageQueue[1] } : new List<MessageQueueProduct>())
                .Returns(messageQueue.Count > 2 ? new List<MessageQueueProduct>() { messageQueue[2] } : new List<MessageQueueProduct>())
                .Returns(messageQueue.Count > 3 ? new List<MessageQueueProduct>() { messageQueue[3] } : new List<MessageQueueProduct>())
                .Returns(messageQueue.Count > 4 ? new List<MessageQueueProduct>() { messageQueue[4] } : new List<MessageQueueProduct>());
            mockQueueReader.SetupSequence(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueProduct>>()))
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
        public void ProcessQueuedProductEvents_WhenSuccessful_ShouldCallJobMonitor_WithExpectedValues()
        {
            // Given.
            var fakeMessageQueueProducts = new List<MessageQueueProduct> {
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 2, "0", ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 3, "0", ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 4, "0", ItemTypeCodes.RetailSale)
            };
            SetupMockMessageQueueSequence(fakeMessageQueueProducts);
            int expectedLogCountSuccessful = fakeMessageQueueProducts.Count;
            int expectedLogCountFailed = 0;

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Once);
            Assert.AreEqual(MessageTypes.Product, actualMonitorLogEntry.MessageTypeID);
            Assert.AreEqual(expectedLogCountSuccessful, actualMonitorLogEntry.CountProcessedMessages.GetValueOrDefault(0));
            Assert.AreEqual(expectedLogCountFailed, actualMonitorLogEntry.CountFailedMessages.GetValueOrDefault(0));
            Assert.IsNotNull(actualMonitorLogEntry.StartTime);
            Assert.IsNotNull(actualMonitorLogEntry.EndTime);
            Assert.IsTrue(actualMonitorLogEntry.EndTime >= actualMonitorLogEntry.StartTime);
        }

        [TestMethod]
        public void ProcessQueuedProductEvents_WhenSerializationErrorForOne_ShouldCallJobMonitor_WithExpectedValues()
        {
            // Given.
            var fakeMessageQueueProducts = new List<MessageQueueProduct> {
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 2, "0", ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 3, "0", ItemTypeCodes.RetailSale)
            };
            SetupMockMessageQueueSequence(fakeMessageQueueProducts, 1);
            int expectedLogCountSuccessful = 2;
            int expectedLogCountFailed = 1;

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Once);
            Assert.AreEqual(MessageTypes.Product, actualMonitorLogEntry.MessageTypeID);
            Assert.AreEqual(expectedLogCountSuccessful, actualMonitorLogEntry.CountProcessedMessages.GetValueOrDefault(0));
            Assert.AreEqual(expectedLogCountFailed, actualMonitorLogEntry.CountFailedMessages.GetValueOrDefault(0));
            Assert.IsNotNull(actualMonitorLogEntry.StartTime);
            Assert.IsNotNull(actualMonitorLogEntry.EndTime);
            Assert.IsTrue(actualMonitorLogEntry.EndTime >= actualMonitorLogEntry.StartTime);
        }

        [TestMethod]
        public void ProcessQueuedProductEvents_WhenSerializationErrorForAll_ShouldCallJobMonitor_WithExpectedValues()
        {
            // Given.
            var fakeMessageQueueProducts = new List<MessageQueueProduct> {
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 2, "0", ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 3, "0", ItemTypeCodes.RetailSale)
            };
            SetupMockMessageQueueSequence(fakeMessageQueueProducts, new List<int> { 0, 1, 2 });
            int expectedLogCountSuccessful = 0;
            int expectedLogCountFailed = 3;

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Once);
            Assert.AreEqual(MessageTypes.Product, actualMonitorLogEntry.MessageTypeID);
            Assert.AreEqual(expectedLogCountSuccessful, actualMonitorLogEntry.CountProcessedMessages.GetValueOrDefault(0));
            Assert.AreEqual(expectedLogCountFailed, actualMonitorLogEntry.CountFailedMessages.GetValueOrDefault(0));
            Assert.IsNotNull(actualMonitorLogEntry.StartTime);
            Assert.IsNotNull(actualMonitorLogEntry.EndTime);
            Assert.IsTrue(actualMonitorLogEntry.EndTime >= actualMonitorLogEntry.StartTime);
        }
    }
}
