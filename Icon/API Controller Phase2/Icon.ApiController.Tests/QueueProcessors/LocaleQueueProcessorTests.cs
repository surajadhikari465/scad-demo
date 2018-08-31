using Icon.ApiController.Common;
using Icon.ApiController.Controller.Monitoring;
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
using Contracts = Icon.Esb.Schemas.Wfm.PreGpm.Contracts;

namespace Icon.ApiController.Tests.QueueProcessors
{
    [TestClass]
    public class LocaleQueueProcessorTests
    {
        private LocaleQueueProcessor queueProcessor;

        private Mock<ILogger<LocaleQueueProcessor>> mockLogger;
        private Mock<ISerializer<Contracts.LocaleType>> mockSerializer;
        private Mock<IQueueReader<MessageQueueLocale, Contracts.LocaleType>> mockQueueReader;
        private Mock<ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>>> mockSaveXmlMessageCommandHandler;
        private Mock<ICommandHandler<AssociateMessageToQueueCommand<MessageQueueLocale, MessageHistory>>> mockAssociateMessageToQueueCommandHandler;
        private Mock<ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueueLocale>>> mockSetProcessedDateCommandHandler;
        private Mock<ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>> mockUpdateMessageHistoryCommandHandler;
        private Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueLocale>>> mockUpdateMessageQueueStatusCommandHandler;
        private Mock<ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueLocale>>> mockMarkQueuedEntriesAsInProcessCommandHandler;
        private Mock<IEsbProducer> mockProducer;
        private ApiControllerSettings settings;
        private Mock<IMessageProcessorMonitor> mockMonitor;
        private APIMessageProcessorLogEntry actualMonitorLogEntry;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger<LocaleQueueProcessor>>();
            mockSerializer = new Mock<ISerializer<Contracts.LocaleType>>();
            mockQueueReader = new Mock<IQueueReader<MessageQueueLocale, Contracts.LocaleType>>();
            mockSaveXmlMessageCommandHandler = new Mock<ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>>>();
            mockAssociateMessageToQueueCommandHandler = new Mock<ICommandHandler<AssociateMessageToQueueCommand<MessageQueueLocale, MessageHistory>>>();
            mockSetProcessedDateCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueueLocale>>>();
            mockUpdateMessageHistoryCommandHandler = new Mock<ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>>();
            mockUpdateMessageQueueStatusCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueLocale>>>();
            mockMarkQueuedEntriesAsInProcessCommandHandler = new Mock<ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueLocale>>>();
            mockProducer = new Mock<IEsbProducer>();
            settings = new ApiControllerSettings();
            mockMonitor = new Mock<IMessageProcessorMonitor>();
            actualMonitorLogEntry = new APIMessageProcessorLogEntry();
            //set up the mock's RecordResults() method with a callback so that we can examine the data passed to it
            mockMonitor.Setup(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()))
                .Callback<APIMessageProcessorLogEntry>(methodData => actualMonitorLogEntry = methodData);

            queueProcessor = new LocaleQueueProcessor(
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
        public void ProcessQueuedLocaleEvents_NoQueuedMessages_GroupMessagesShouldNotBeCalled()
        {
            // Given.
            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(new List<MessageQueueLocale>());

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockQueueReader.Verify(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueLocale>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedLocaleEvents_GetGroupedMessagesReturnsEmpty_BuildMiniBulkShouldNotBeCalled()
        {
            // Given.
            var fakeMessageQueueLocales = new List<MessageQueueLocale> { TestHelpers.GetFakeMessageQueueLocale() };
            var fakeMessageQueueLocalesEmpty = new List<MessageQueueLocale>();

            var queuedMessages = new Queue<List<MessageQueueLocale>>();
            queuedMessages.Enqueue(fakeMessageQueueLocales);
            queuedMessages.Enqueue(fakeMessageQueueLocalesEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueLocale>>())).Returns(new List<MessageQueueLocale>());

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockQueueReader.Verify(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueLocale>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedLocaleEvents_MiniBulkReturnsEmpty_SerializeShouldNotBeCalled()
        {
            // Given.
            var fakeMessageQueueLocales = new List<MessageQueueLocale> { TestHelpers.GetFakeMessageQueueLocale() };
            var fakeMessageQueueLocalesEmpty = new List<MessageQueueLocale>();

            var queuedMessages = new Queue<List<MessageQueueLocale>>();
            queuedMessages.Enqueue(fakeMessageQueueLocales);
            queuedMessages.Enqueue(fakeMessageQueueLocalesEmpty);

            Contracts.LocaleType locale = null;

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueLocale>>())).Returns(fakeMessageQueueLocales);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueLocale>>())).Returns(locale);

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockSerializer.Verify(s => s.Serialize(It.IsAny<Contracts.LocaleType>(), It.IsAny<TextWriter>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedLocaleEvents_SerializationError_UpdateMessageQueueStatusCommandShouldBeCalled()
        {
            // Given.
            var fakeMessageQueueLocales = new List<MessageQueueLocale> { TestHelpers.GetFakeMessageQueueLocale() };
            var fakeMessageQueueLocalesEmpty = new List<MessageQueueLocale>();

            var queuedMessages = new Queue<List<MessageQueueLocale>>();
            queuedMessages.Enqueue(fakeMessageQueueLocales);
            queuedMessages.Enqueue(fakeMessageQueueLocalesEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueLocale>>())).Returns(fakeMessageQueueLocales);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueLocale>>())).Returns(new Contracts.LocaleType());
            mockSerializer.Setup(s => s.Serialize(It.IsAny<Contracts.LocaleType>(), It.IsAny<TextWriter>())).Returns(string.Empty);

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockUpdateMessageQueueStatusCommandHandler.Verify(c => c.Execute(It.IsAny<UpdateMessageQueueStatusCommand<MessageQueueLocale>>()), Times.Once);
            mockSaveXmlMessageCommandHandler.Verify(c => c.Execute(It.IsAny<SaveToMessageHistoryCommand<MessageHistory>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessQueuedLocaleEvents_NoErrors_UpdateMessageHistoryCommandShouldBeCalled()
        {
            // Given.
            var fakeMessageQueueLocales = new List<MessageQueueLocale> { TestHelpers.GetFakeMessageQueueLocale() };
            var fakeMessageQueueLocalesEmpty = new List<MessageQueueLocale>();

            var queuedMessages = new Queue<List<MessageQueueLocale>>();
            queuedMessages.Enqueue(fakeMessageQueueLocales);
            queuedMessages.Enqueue(fakeMessageQueueLocalesEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueLocale>>())).Returns(fakeMessageQueueLocales);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueLocale>>())).Returns(new Contracts.LocaleType());
            mockSerializer.Setup(s => s.Serialize(It.IsAny<Contracts.LocaleType>(), It.IsAny<TextWriter>())).Returns("Test");
            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockUpdateMessageHistoryCommandHandler.Verify(c => c.Execute(It.IsAny<UpdateMessageHistoryStatusCommand<MessageHistory>>()), Times.Once);
        }

        [TestMethod]
        public void ProcessQueuedLocaleEvents_WhenNothingQueued_ShouldNotCallJobMonitor()
        {
            // Given.
            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(new List<MessageQueueLocale>());

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Never);
        }

        private void SetupMockMessageQueueSequence(List<MessageQueueLocale> messageQueue, int sequenceIndexToFailSerialization)
        {
            SetupMockMessageQueueSequence(messageQueue, new List<int> { sequenceIndexToFailSerialization });
        }

        private void SetupMockMessageQueueSequence(List<MessageQueueLocale> messageQueue, List<int> sequenceIndicesToFailSerialization = null)
        {
            if (sequenceIndicesToFailSerialization == null) sequenceIndicesToFailSerialization = new List<int>();
            mockQueueReader.SetupSequence(qr => qr.GetQueuedMessages())
                .Returns(messageQueue.Count > 0 ? new List<MessageQueueLocale>() { messageQueue[0] } : new List<MessageQueueLocale>())
                .Returns(messageQueue.Count > 1 ? new List<MessageQueueLocale>() { messageQueue[1] } : new List<MessageQueueLocale>())
                .Returns(messageQueue.Count > 2 ? new List<MessageQueueLocale>() { messageQueue[2] } : new List<MessageQueueLocale>())
                .Returns(messageQueue.Count > 3 ? new List<MessageQueueLocale>() { messageQueue[3] } : new List<MessageQueueLocale>())
                .Returns(messageQueue.Count > 4 ? new List<MessageQueueLocale>() { messageQueue[4] } : new List<MessageQueueLocale>());
            mockQueueReader.SetupSequence(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueLocale>>()))
                .Returns(messageQueue.Count > 0 ? new List<MessageQueueLocale>() { messageQueue[0] } : new List<MessageQueueLocale>())
                .Returns(messageQueue.Count > 1 ? new List<MessageQueueLocale>() { messageQueue[1] } : new List<MessageQueueLocale>())
                .Returns(messageQueue.Count > 2 ? new List<MessageQueueLocale>() { messageQueue[2] } : new List<MessageQueueLocale>())
                .Returns(messageQueue.Count > 3 ? new List<MessageQueueLocale>() { messageQueue[3] } : new List<MessageQueueLocale>())
                .Returns(messageQueue.Count > 4 ? new List<MessageQueueLocale>() { messageQueue[4] } : new List<MessageQueueLocale>());
            mockQueueReader.SetupSequence(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueLocale>>()))
                .Returns(messageQueue.Count > 0 ? new Contracts.LocaleType() { id = messageQueue[0].LocaleId.ToString() } : null)
                .Returns(messageQueue.Count > 1 ? new Contracts.LocaleType() { id = messageQueue[1].LocaleId.ToString() } : null)
                .Returns(messageQueue.Count > 2 ? new Contracts.LocaleType() { id = messageQueue[2].LocaleId.ToString() } : null)
                .Returns(messageQueue.Count > 3 ? new Contracts.LocaleType() { id = messageQueue[3].LocaleId.ToString() } : null)
                .Returns(messageQueue.Count > 4 ? new Contracts.LocaleType() { id = messageQueue[4].LocaleId.ToString() } : null);
            mockSerializer.SetupSequence(s => s.Serialize(It.IsAny<Contracts.LocaleType>(), It.IsAny<TextWriter>()))
                .Returns(messageQueue.Count > 0 && !sequenceIndicesToFailSerialization.Contains(0) ? "Test1" : null)
                .Returns(messageQueue.Count > 1 && !sequenceIndicesToFailSerialization.Contains(1) ? "Test2" : null)
                .Returns(messageQueue.Count > 2 && !sequenceIndicesToFailSerialization.Contains(2) ? "Test3" : null)
                .Returns(messageQueue.Count > 3 && !sequenceIndicesToFailSerialization.Contains(3) ? "Test4" : null)
                .Returns(messageQueue.Count > 4 && !sequenceIndicesToFailSerialization.Contains(4) ? "Test5" : null);
            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
        }

        [TestMethod]
        public void ProcessQueuedLocaleEvents_WhenSuccessful_ShouldCallJobMonitor()
        {
            // Given.
            var fakeMessageQueueLocales = new List<MessageQueueLocale> { TestHelpers.GetFakeMessageQueueLocale(1) };
            var fakeMessageQueueLocalesEmpty = new List<MessageQueueLocale>();

            var queuedMessages = new Queue<List<MessageQueueLocale>>();
            queuedMessages.Enqueue(fakeMessageQueueLocales);
            queuedMessages.Enqueue(fakeMessageQueueLocalesEmpty);

            mockQueueReader.Setup(qr => qr.GetQueuedMessages()).Returns(queuedMessages.Dequeue);
            mockQueueReader.Setup(qr => qr.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueLocale>>())).Returns(fakeMessageQueueLocales);
            mockQueueReader.Setup(qr => qr.BuildMiniBulk(It.IsAny<List<MessageQueueLocale>>())).Returns(new Contracts.LocaleType());
            mockSerializer.Setup(s => s.Serialize(It.IsAny<Contracts.LocaleType>(), It.IsAny<TextWriter>())).Returns("Test");
            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Once);
        }

        [TestMethod]
        public void ProcessQueuedLocaleEvents_WhenSuccessful_ShouldCallJobMonitor_WithExpectedValues()
        {
            // Given.
            var fakeMessageQueueLocales = new List<MessageQueueLocale> {
                TestHelpers.GetFakeMessageQueueLocale(1),
                TestHelpers.GetFakeMessageQueueLocale(2),
                TestHelpers.GetFakeMessageQueueLocale(3),
                TestHelpers.GetFakeMessageQueueLocale(4)
            };
            SetupMockMessageQueueSequence(fakeMessageQueueLocales);
            int expectedLogCountSuccessful = fakeMessageQueueLocales.Count;
            int expectedLogCountFailed = 0;

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Once);
            Assert.AreEqual(MessageTypes.Locale, actualMonitorLogEntry.MessageTypeID);
            Assert.AreEqual(expectedLogCountSuccessful, actualMonitorLogEntry.CountProcessedMessages.GetValueOrDefault(0));
            Assert.AreEqual(expectedLogCountFailed, actualMonitorLogEntry.CountFailedMessages.GetValueOrDefault(0));
            Assert.IsNotNull(actualMonitorLogEntry.StartTime);
            Assert.IsNotNull(actualMonitorLogEntry.EndTime);
            Assert.IsTrue(actualMonitorLogEntry.EndTime >= actualMonitorLogEntry.StartTime);
        }

        [TestMethod]
        public void ProcessQueuedLocaleEvents_WhenSerializationErrorForOne_ShouldCallJobMonitor_WithExpectedValues()
        {
            // Given.
            var fakeMessageQueueLocales = new List<MessageQueueLocale> {
                TestHelpers.GetFakeMessageQueueLocale(1),
                TestHelpers.GetFakeMessageQueueLocale(2),
                TestHelpers.GetFakeMessageQueueLocale(3)
            };
            SetupMockMessageQueueSequence(fakeMessageQueueLocales, 1);
            int expectedLogCountSuccessful = 2;
            int expectedLogCountFailed = 1;

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Once);
            Assert.AreEqual(MessageTypes.Locale, actualMonitorLogEntry.MessageTypeID);
            Assert.AreEqual(expectedLogCountSuccessful, actualMonitorLogEntry.CountProcessedMessages.GetValueOrDefault(0));
            Assert.AreEqual(expectedLogCountFailed, actualMonitorLogEntry.CountFailedMessages.GetValueOrDefault(0));
            Assert.IsNotNull(actualMonitorLogEntry.StartTime);
            Assert.IsNotNull(actualMonitorLogEntry.EndTime);
            Assert.IsTrue(actualMonitorLogEntry.EndTime >= actualMonitorLogEntry.StartTime);
        }

        [TestMethod]
        public void ProcessQueuedLocaleEvents_WhenSerializationErrorForAllne_ShouldCallJobMonitor_WithExpectedValues()
        {
            // Given.
            var fakeMessageQueueLocales = new List<MessageQueueLocale> {
                TestHelpers.GetFakeMessageQueueLocale(1),
                TestHelpers.GetFakeMessageQueueLocale(2),
                TestHelpers.GetFakeMessageQueueLocale(3)
            };
            SetupMockMessageQueueSequence(fakeMessageQueueLocales, new List<int> { 0, 1, 2 });
            int expectedLogCountSuccessful = 0;
            int expectedLogCountFailed = 3;

            // When.
            queueProcessor.ProcessMessageQueue();

            // Then.
            mockMonitor.Verify(m => m.RecordResults(It.IsAny<APIMessageProcessorLogEntry>()), Times.Once);
            Assert.AreEqual(MessageTypes.Locale, actualMonitorLogEntry.MessageTypeID);
            Assert.AreEqual(expectedLogCountSuccessful, actualMonitorLogEntry.CountProcessedMessages.GetValueOrDefault(0));
            Assert.AreEqual(expectedLogCountFailed, actualMonitorLogEntry.CountFailedMessages.GetValueOrDefault(0));
            Assert.IsNotNull(actualMonitorLogEntry.StartTime);
            Assert.IsNotNull(actualMonitorLogEntry.EndTime);
            Assert.IsTrue(actualMonitorLogEntry.EndTime >= actualMonitorLogEntry.StartTime);
        }
    }
}
