using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.Controller.Serializers;
using Icon.ApiController.DataAccess.Commands;
using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.Logging;
using Mammoth.ApiController.QueueProcessors;
using Mammoth.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Esb.Producer;
using System.IO;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;
using Icon.ApiController.Common;

namespace Mammoth.ApiController.Tests.QueueProcessors
{
    [TestClass]
    public class MammothItemLocaleQueueProcessorTests
    {
        private MammothItemLocaleQueueProcessor queueProcessor;
        private Mock<ILogger> mockLogger;
        private Mock<IRenewableContext> mockGlobalContext;
        private Mock<IQueueReader<MessageQueueItemLocale, Contracts.items>> mockQueueReader;
        private Mock<ISerializer<Contracts.items>> mockSerializer;
        private Mock<ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>>> mockSaveToMessageHistoryCommandHandler;
        private Mock<ICommandHandler<AssociateMessageToQueueCommand<MessageQueueItemLocale, MessageHistory>>> mockAssociateMessageToQueueCommandHandler;
        private Mock<ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueueItemLocale>>> mockSetProcessedDataCommandHandler;
        private Mock<ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>> mockUpdateMessageHistoryCommandHandler;
        private Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueItemLocale>>> mockUpdateMessageQueueStatusCommandHandler;
        private Mock<ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueItemLocale>>> mockMarkQueuedEntriesAsInProcessCommandHandler;
        private Mock<IEsbProducer> mockProducer;
        private ApiControllerSettings settings;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger>();
            mockGlobalContext = new Mock<IRenewableContext>();
            mockQueueReader = new Mock<IQueueReader<MessageQueueItemLocale, Contracts.items>>();
            mockSerializer = new Mock<ISerializer<Contracts.items>>();
            mockSaveToMessageHistoryCommandHandler = new Mock<ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>>>();
            mockAssociateMessageToQueueCommandHandler = new Mock<ICommandHandler<AssociateMessageToQueueCommand<MessageQueueItemLocale, MessageHistory>>>();
            mockSetProcessedDataCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueueItemLocale>>>();
            mockUpdateMessageQueueStatusCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueItemLocale>>>();
            mockUpdateMessageHistoryCommandHandler = new Mock<ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>>();
            mockMarkQueuedEntriesAsInProcessCommandHandler = new Mock<ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueItemLocale>>>();
            mockProducer = new Mock<IEsbProducer>();
            settings = new ApiControllerSettings { Instance = 1 };

            queueProcessor = new MammothItemLocaleQueueProcessor(mockLogger.Object,
                mockGlobalContext.Object,
                mockQueueReader.Object,
                mockSerializer.Object,
                mockSaveToMessageHistoryCommandHandler.Object,
                mockAssociateMessageToQueueCommandHandler.Object,
                mockSetProcessedDataCommandHandler.Object,
                mockUpdateMessageHistoryCommandHandler.Object,
                mockUpdateMessageQueueStatusCommandHandler.Object,
                mockMarkQueuedEntriesAsInProcessCommandHandler.Object,
                mockProducer.Object,
                settings);
        }

        [TestMethod]
        public void ProcessMessageQueue_QueueRecordsExist_ShouldPublishMessagesForQueueRecords()
        {
            //Given
            mockQueueReader.SetupSequence(m => m.GetQueuedMessages())
                .Returns(new List<MessageQueueItemLocale> { new MessageQueueItemLocale() })
                .Returns(new List<MessageQueueItemLocale>());
            mockQueueReader.Setup(m => m.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueItemLocale>>()))
                .Returns(new List<MessageQueueItemLocale> { new MessageQueueItemLocale { ItemId = 1 } });
            mockQueueReader.Setup(m => m.BuildMiniBulk(It.IsAny<List<MessageQueueItemLocale>>()))
                .Returns(new Contracts.items { item = new Contracts.ItemType[] { new Contracts.ItemType { id = 1 } } });
            mockSerializer.Setup(m => m.Serialize(It.IsAny<Contracts.items>(), It.IsAny<TextWriter>()))
                .Returns("test message");

            //When
            queueProcessor.ProcessMessageQueue();

            //Then
            mockMarkQueuedEntriesAsInProcessCommandHandler.Verify(m => m.Execute(It.IsAny<MarkQueuedEntriesAsInProcessCommand<MessageQueueItemLocale>>()), Times.Exactly(2));
            mockQueueReader.Verify(m => m.GetQueuedMessages(), Times.Exactly(2));
            mockQueueReader.Verify(m => m.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueItemLocale>>()), Times.Once);
            mockQueueReader.Verify(m => m.BuildMiniBulk(It.IsAny<List<MessageQueueItemLocale>>()), Times.Once);
            mockUpdateMessageQueueStatusCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateMessageQueueStatusCommand<MessageQueueItemLocale>>()), Times.Never);
            mockSaveToMessageHistoryCommandHandler.Verify(m => m.Execute(It.IsAny<SaveToMessageHistoryCommand<MessageHistory>>()), Times.Once);
            mockAssociateMessageToQueueCommandHandler.Verify(m => m.Execute(It.IsAny<AssociateMessageToQueueCommand<MessageQueueItemLocale, MessageHistory>>()), Times.Once);
            mockUpdateMessageHistoryCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateMessageHistoryStatusCommand<MessageHistory>>()), Times.Once);
            mockSetProcessedDataCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateMessageQueueProcessedDateCommand<MessageQueueItemLocale>>()), Times.Once);
            mockProducer.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
            mockGlobalContext.Verify(m => m.Refresh(), Times.Once);
            mockProducer.Verify(m => m.Dispose(), Times.Once);
        }

        [TestMethod]
        public void ProcessMessageQueue_SaveToMessageHistoryCommandHandlerThrowsException_ShouldFailQueuedMessages()
        {
            //Given
            mockQueueReader.SetupSequence(m => m.GetQueuedMessages())
                .Returns(new List<MessageQueueItemLocale> { new MessageQueueItemLocale() })
                .Returns(new List<MessageQueueItemLocale>());
            mockQueueReader.Setup(m => m.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueItemLocale>>()))
                .Returns(new List<MessageQueueItemLocale> { new MessageQueueItemLocale { ItemId = 1 } });
            mockQueueReader.Setup(m => m.BuildMiniBulk(It.IsAny<List<MessageQueueItemLocale>>()))
                .Returns(new Contracts.items { item = new Contracts.ItemType[] { new Contracts.ItemType { id = 1 } } });
            mockSerializer.Setup(m => m.Serialize(It.IsAny<Contracts.items>(), It.IsAny<TextWriter>()))
                .Returns("test message");

            //When
            queueProcessor.ProcessMessageQueue();

            //Then
            mockMarkQueuedEntriesAsInProcessCommandHandler.Verify(m => m.Execute(It.IsAny<MarkQueuedEntriesAsInProcessCommand<MessageQueueItemLocale>>()), Times.Exactly(2));
            mockQueueReader.Verify(m => m.GetQueuedMessages(), Times.Exactly(2));
            mockQueueReader.Verify(m => m.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueueItemLocale>>()), Times.Once);
            mockQueueReader.Verify(m => m.BuildMiniBulk(It.IsAny<List<MessageQueueItemLocale>>()), Times.Once);
            mockUpdateMessageQueueStatusCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateMessageQueueStatusCommand<MessageQueueItemLocale>>()), Times.Never);
            mockSaveToMessageHistoryCommandHandler.Verify(m => m.Execute(It.IsAny<SaveToMessageHistoryCommand<MessageHistory>>()), Times.Once);
            mockAssociateMessageToQueueCommandHandler.Verify(m => m.Execute(It.IsAny<AssociateMessageToQueueCommand<MessageQueueItemLocale, MessageHistory>>()), Times.Once);
            mockUpdateMessageHistoryCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateMessageHistoryStatusCommand<MessageHistory>>()), Times.Once);
            mockSetProcessedDataCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateMessageQueueProcessedDateCommand<MessageQueueItemLocale>>()), Times.Once);
            mockProducer.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
            mockGlobalContext.Verify(m => m.Refresh(), Times.Once);
            mockProducer.Verify(m => m.Dispose(), Times.Once);
        }
    }
}
