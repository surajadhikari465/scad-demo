using Icon.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PushController.Controller.MessageBuilders;
using PushController.Controller.MessageGenerators;
using PushController.Controller.MessageQueueServices;
using System;
using System.Collections.Generic;

namespace PushController.Tests.Controller.MessageGeneratorTests
{
    [TestClass]
    public class ItemLocaleMessageGeneratorTests
    {
        private Mock<IMessageBuilder<MessageQueueItemLocale>> mockMessageBuilder;
        private Mock<IMessageQueueService<MessageQueueItemLocale>> mockMessageQueueService;

        [TestInitialize]
        public void Initialize()
        {
            this.mockMessageBuilder = new Mock<IMessageBuilder<MessageQueueItemLocale>>();
            this.mockMessageQueueService = new Mock<IMessageQueueService<MessageQueueItemLocale>>();
        }

        [TestMethod]
        public void GenerateMessages_PosDataReadyForEsb_MessageBuilderShouldBeCalled()
        {
            // Given.
            var messageGenerator = new ItemLocaleMessageGenerator(mockMessageBuilder.Object, mockMessageQueueService.Object);

            // When.
            messageGenerator.BuildMessages(new List<IRMAPush>());
            
            // Then.
            mockMessageBuilder.Verify(mb => mb.BuildMessages(It.IsAny<List<IRMAPush>>()), Times.Once);
        }

        [TestMethod]
        public void SaveMessages_MessagesReadyToSave_BulkSaveMethodShouldBeCalled()
        {
            // Given.
            var messageGenerator = new ItemLocaleMessageGenerator(mockMessageBuilder.Object, mockMessageQueueService.Object);

            // When.
            messageGenerator.SaveMessages(new List<MessageQueueItemLocale>());

            // Then.
            mockMessageQueueService.Verify(mb => mb.SaveMessagesBulk(It.IsAny<List<MessageQueueItemLocale>>()), Times.Once);
        }

        [TestMethod]
        public void SaveMessages_ErrorInBulkProcessing_FallbackMethodShouldBeCalled()
        {
            // Given.
            mockMessageQueueService.Setup(mq => mq.SaveMessagesBulk(It.IsAny<List<MessageQueueItemLocale>>())).Throws(new Exception());
            var messageGenerator = new ItemLocaleMessageGenerator(mockMessageBuilder.Object, mockMessageQueueService.Object);

            // When.
            messageGenerator.SaveMessages(new List<MessageQueueItemLocale>());

            // Then.
            mockMessageQueueService.Verify(mb => mb.SaveMessagesBulk(It.IsAny<List<MessageQueueItemLocale>>()), Times.Once);
            mockMessageQueueService.Verify(mb => mb.SaveMessagesRowByRow(It.IsAny<List<MessageQueueItemLocale>>()), Times.Once);
        }
    }
}
