﻿using Icon.ApiController.Common;
using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.Controller.Serializers;
using Icon.ApiController.DataAccess.Commands;
using Icon.Common.DataAccess;
using Icon.Esb.Producer;
using Icon.ActiveMQ.Producer;
using Icon.Logging;
using Mammoth.ApiController.QueueProcessors;
using Mammoth.Common.DataAccess;
using Mammoth.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.IO;
using Contracts = Icon.Esb.Schemas.Wfm.PreGpm.Contracts;

namespace Mammoth.ApiController.Tests.QueueProcessors
{
    [TestClass]
    public class MammothPriceQueueProcessorTests
    {
        private MammothPriceQueueProcessor queueProcessor;
        private Mock<ILogger> mockLogger;
        private Mock<IQueueReader<MessageQueuePrice, Contracts.items>> mockQueueReader;
        private Mock<ISerializer<Contracts.items>> mockSerializer;
        private Mock<ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>>> mockSaveToMessageHistoryCommandHandler;
        private Mock<ICommandHandler<AssociateMessageToQueueCommand<MessageQueuePrice, MessageHistory>>> mockAssociateMessageToQueueCommandHandler;
        private Mock<ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueuePrice>>> mockSetProcessedDataCommandHandler;
        private Mock<ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>> mockUpdateMessageHistoryCommandHandler;
        private Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueuePrice>>> mockUpdateMessageQueueStatusCommandHandler;
        private Mock<ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueuePrice>>> mockMarkQueuedEntriesAsInProcessCommandHandler;
        private Mock<IEsbProducer> mockEsbProducer;
        private Mock<IActiveMQProducer> mockActiveMqProducer;
        private ApiControllerSettings settings;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger>();
            mockQueueReader = new Mock<IQueueReader<MessageQueuePrice, Contracts.items>>();
            mockSerializer = new Mock<ISerializer<Contracts.items>>();
            mockSaveToMessageHistoryCommandHandler = new Mock<ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>>>();
            mockAssociateMessageToQueueCommandHandler = new Mock<ICommandHandler<AssociateMessageToQueueCommand<MessageQueuePrice, MessageHistory>>>();
            mockSetProcessedDataCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueuePrice>>>();
            mockUpdateMessageQueueStatusCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueuePrice>>>();
            mockUpdateMessageHistoryCommandHandler = new Mock<ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>>();
            mockMarkQueuedEntriesAsInProcessCommandHandler = new Mock<ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueuePrice>>>();
            mockEsbProducer = new Mock<IEsbProducer>();
            mockActiveMqProducer = new Mock<IActiveMQProducer>();
            settings = new ApiControllerSettings { Instance = 1 };

            queueProcessor = new MammothPriceQueueProcessor(mockLogger.Object,
                mockQueueReader.Object,
                mockSerializer.Object,
                mockSaveToMessageHistoryCommandHandler.Object,
                mockAssociateMessageToQueueCommandHandler.Object,
                mockSetProcessedDataCommandHandler.Object,
                mockUpdateMessageHistoryCommandHandler.Object,
                mockUpdateMessageQueueStatusCommandHandler.Object,
                mockMarkQueuedEntriesAsInProcessCommandHandler.Object,
                mockEsbProducer.Object,
                mockActiveMqProducer.Object,
                settings);
        }

        [TestMethod]
        public void ProcessMessageQueuePrice_QueueRecordsExist_ShouldPublishMessagesForQueueRecords()
        {
            //Given
            mockQueueReader.SetupSequence(m => m.GetQueuedMessages())
                .Returns(new List<MessageQueuePrice> { new MessageQueuePrice() })
                .Returns(new List<MessageQueuePrice>());
            mockQueueReader.Setup(m => m.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueuePrice>>()))
                .Returns(new List<MessageQueuePrice> { new MessageQueuePrice { ItemId = 1 } });
            mockQueueReader.Setup(m => m.BuildMiniBulk(It.IsAny<List<MessageQueuePrice>>()))
                .Returns(new Contracts.items { item = new Contracts.ItemType[] { new Contracts.ItemType { id = 1 } } });
            mockSerializer.Setup(m => m.Serialize(It.IsAny<Contracts.items>(), It.IsAny<TextWriter>()))
                .Returns("test message");
            mockEsbProducer.Setup(m => m.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
            mockActiveMqProducer.Setup(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));

            //When
            queueProcessor.ProcessMessageQueue();

            //Then
            mockMarkQueuedEntriesAsInProcessCommandHandler.Verify(m => m.Execute(It.IsAny<MarkQueuedEntriesAsInProcessCommand<MessageQueuePrice>>()), Times.Exactly(2));
            mockQueueReader.Verify(m => m.GetQueuedMessages(), Times.Exactly(2));
            mockQueueReader.Verify(m => m.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueuePrice>>()), Times.Once);
            mockQueueReader.Verify(m => m.BuildMiniBulk(It.IsAny<List<MessageQueuePrice>>()), Times.Once);
            mockUpdateMessageQueueStatusCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateMessageQueueStatusCommand<MessageQueuePrice>>()), Times.Never);
            mockSaveToMessageHistoryCommandHandler.Verify(m => m.Execute(It.IsAny<SaveToMessageHistoryCommand<MessageHistory>>()), Times.Once);
            mockAssociateMessageToQueueCommandHandler.Verify(m => m.Execute(It.IsAny<AssociateMessageToQueueCommand<MessageQueuePrice, MessageHistory>>()), Times.Once);
            mockUpdateMessageHistoryCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateMessageHistoryStatusCommand<MessageHistory>>()), Times.Once);
            mockSetProcessedDataCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateMessageQueueProcessedDateCommand<MessageQueuePrice>>()), Times.Once);
            mockEsbProducer.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
            mockActiveMqProducer.Verify(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
        }

        [TestMethod]
        public void ProcessMessageQueuePrice_SaveToMessageHistoryCommandHandlerThrowsException_ShouldFailQueuedMessages()
        {
            //Given
            mockQueueReader.SetupSequence(m => m.GetQueuedMessages())
                .Returns(new List<MessageQueuePrice> { new MessageQueuePrice() })
                .Returns(new List<MessageQueuePrice>());
            mockQueueReader.Setup(m => m.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueuePrice>>()))
                .Returns(new List<MessageQueuePrice> { new MessageQueuePrice { ItemId = 1 } });
            mockQueueReader.Setup(m => m.BuildMiniBulk(It.IsAny<List<MessageQueuePrice>>()))
                .Returns(new Contracts.items { item = new Contracts.ItemType[] { new Contracts.ItemType { id = 1 } } });
            mockSerializer.Setup(m => m.Serialize(It.IsAny<Contracts.items>(), It.IsAny<TextWriter>()))
                .Returns("test message");
            mockEsbProducer.Setup(m => m.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
            mockActiveMqProducer.Setup(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));

            //When
            queueProcessor.ProcessMessageQueue();

            //Then
            mockMarkQueuedEntriesAsInProcessCommandHandler.Verify(m => m.Execute(It.IsAny<MarkQueuedEntriesAsInProcessCommand<MessageQueuePrice>>()), Times.Exactly(2));
            mockQueueReader.Verify(m => m.GetQueuedMessages(), Times.Exactly(2));
            mockQueueReader.Verify(m => m.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueuePrice>>()), Times.Once);
            mockQueueReader.Verify(m => m.BuildMiniBulk(It.IsAny<List<MessageQueuePrice>>()), Times.Once);
            mockUpdateMessageQueueStatusCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateMessageQueueStatusCommand<MessageQueuePrice>>()), Times.Never);
            mockSaveToMessageHistoryCommandHandler.Verify(m => m.Execute(It.IsAny<SaveToMessageHistoryCommand<MessageHistory>>()), Times.Once);
            mockAssociateMessageToQueueCommandHandler.Verify(m => m.Execute(It.IsAny<AssociateMessageToQueueCommand<MessageQueuePrice, MessageHistory>>()), Times.Once);
            mockUpdateMessageHistoryCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateMessageHistoryStatusCommand<MessageHistory>>()), Times.Once);
            mockSetProcessedDataCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateMessageQueueProcessedDateCommand<MessageQueuePrice>>()), Times.Once);
            mockEsbProducer.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
            mockActiveMqProducer.Verify(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);

            mockSaveToMessageHistoryCommandHandler.Verify(m => 
                m.Execute(It.Is<SaveToMessageHistoryCommand<MessageHistory>>(data=>
                            data.Message.MessageTypeId == MessageTypes.Price)),
                Times.Once);

        }

        [TestMethod]
        public void ProcessMessageQueuePrice_QueueRecordsExist_ShouldPublishMessage_WithExpectedMessageType()
        {
            //Given
            mockQueueReader.SetupSequence(m => m.GetQueuedMessages())
                .Returns(new List<MessageQueuePrice> { new MessageQueuePrice() })
                .Returns(new List<MessageQueuePrice>());
            mockQueueReader.Setup(m => m.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueuePrice>>()))
                .Returns(new List<MessageQueuePrice> { new MessageQueuePrice { ItemId = 1 } });
            mockQueueReader.Setup(m => m.BuildMiniBulk(It.IsAny<List<MessageQueuePrice>>()))
                .Returns(new Contracts.items { item = new Contracts.ItemType[] { new Contracts.ItemType { id = 1 } } });
            mockSerializer.Setup(m => m.Serialize(It.IsAny<Contracts.items>(), It.IsAny<TextWriter>()))
                .Returns("test message");
            mockEsbProducer.Setup(m => m.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
            mockActiveMqProducer.Setup(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));

            //When
            queueProcessor.ProcessMessageQueue();

            //Then
            mockUpdateMessageHistoryCommandHandler.Verify(m =>
                m.Execute(
                    It.Is<UpdateMessageHistoryStatusCommand<MessageHistory>>(data =>
                    data.MessageStatusId == MessageStatusTypes.Sent &&
                    data.Message.MessageTypeId == MessageTypes.Price)
                    ),
                Times.Once);
        }

        [TestMethod]
        public void ProcessMessageQueuePrice_WhenEsbFails_MessageStatusShouldBe_SentToActiveMq()
        {
            // Given
            mockQueueReader.SetupSequence(m => m.GetQueuedMessages())
                .Returns(new List<MessageQueuePrice> { new MessageQueuePrice() })
                .Returns(new List<MessageQueuePrice>());
            mockQueueReader.Setup(m => m.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueuePrice>>()))
                .Returns(new List<MessageQueuePrice> { new MessageQueuePrice { ItemId = 1 } });
            mockQueueReader.Setup(m => m.BuildMiniBulk(It.IsAny<List<MessageQueuePrice>>()))
                .Returns(new Contracts.items { item = new Contracts.ItemType[] { new Contracts.ItemType { id = 1 } } });
            mockSerializer.Setup(m => m.Serialize(It.IsAny<Contracts.items>(), It.IsAny<TextWriter>()))
                .Returns("test message");
            mockEsbProducer.Setup(m => m.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Throws(new System.Exception("Test Exception"));
            mockActiveMqProducer.Setup(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));

            // When
            queueProcessor.ProcessMessageQueue();

            // Then
            mockUpdateMessageHistoryCommandHandler.Verify(m =>
                m.Execute(
                    It.Is<UpdateMessageHistoryStatusCommand<MessageHistory>>(data =>
                    data.MessageStatusId == MessageStatusTypes.SentToActiveMq &&
                    data.Message.MessageTypeId == MessageTypes.Price)
                    ),
                Times.Once);
            mockActiveMqProducer.Verify(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
        }

        [TestMethod]
        public void ProcessMessageQueuePrice_WhenActiveMQFails_MessageStatusShouldBe_SentToEsb()
        {
            // Given
            mockQueueReader.SetupSequence(m => m.GetQueuedMessages())
                .Returns(new List<MessageQueuePrice> { new MessageQueuePrice() })
                .Returns(new List<MessageQueuePrice>());
            mockQueueReader.Setup(m => m.GroupMessagesForMiniBulk(It.IsAny<List<MessageQueuePrice>>()))
                .Returns(new List<MessageQueuePrice> { new MessageQueuePrice { ItemId = 1 } });
            mockQueueReader.Setup(m => m.BuildMiniBulk(It.IsAny<List<MessageQueuePrice>>()))
                .Returns(new Contracts.items { item = new Contracts.ItemType[] { new Contracts.ItemType { id = 1 } } });
            mockSerializer.Setup(m => m.Serialize(It.IsAny<Contracts.items>(), It.IsAny<TextWriter>()))
                .Returns("test message");
            mockEsbProducer.Setup(m => m.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
            mockActiveMqProducer.Setup(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Throws(new System.Exception("Test Exception"));

            // When
            queueProcessor.ProcessMessageQueue();

            // Then
            mockUpdateMessageHistoryCommandHandler.Verify(m =>
                m.Execute(
                    It.Is<UpdateMessageHistoryStatusCommand<MessageHistory>>(data =>
                    data.MessageStatusId == MessageStatusTypes.SentToEsb &&
                    data.Message.MessageTypeId == MessageTypes.Price)
                    ),
                Times.Once);
            mockEsbProducer.Verify(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
        }
    }
}
