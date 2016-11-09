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
    public class PriceMessageGeneratorTests
    {
        private Mock<IMessageBuilder<MessageQueuePrice>> mockMessageBuilder;
        private Mock<IMessageQueueService<MessageQueuePrice>> mockMessageQueueService;

        [TestInitialize]
        public void Initialize()
        {
            this.mockMessageBuilder = new Mock<IMessageBuilder<MessageQueuePrice>>();
            this.mockMessageQueueService = new Mock<IMessageQueueService<MessageQueuePrice>>();
        }

        [TestMethod]
        public void GenerateMessages_PosDataReadyForEsb_MessageBuilderShouldBeCalled()
        {
            // Given.
            var messageGenerator = new PriceMessageGenerator(mockMessageBuilder.Object, mockMessageQueueService.Object);

            // When.
            messageGenerator.BuildMessages(new List<IRMAPush>());

            // Then.
            mockMessageBuilder.Verify(mb => mb.BuildMessages(It.IsAny<List<IRMAPush>>()), Times.Once);
        }

        [TestMethod]
        public void SaveMessages_MessagesReadyToSave_BulkSaveMethodShouldBeCalled()
        {
            // Given.
            var messageGenerator = new PriceMessageGenerator(mockMessageBuilder.Object, mockMessageQueueService.Object);

            // When.
            messageGenerator.SaveMessages(new List<MessageQueuePrice>());

            // Then.
            mockMessageQueueService.Verify(mb => mb.SaveMessagesBulk(It.IsAny<List<MessageQueuePrice>>()), Times.Once);
        }

        [TestMethod]
        public void SaveMessages_ErrorInBulkProcessing_FallbackMethodShouldBeCalled()
        {
            // Given.
            mockMessageQueueService.Setup(mq => mq.SaveMessagesBulk(It.IsAny<List<MessageQueuePrice>>())).Throws(new Exception());
            var messageGenerator = new PriceMessageGenerator(mockMessageBuilder.Object, mockMessageQueueService.Object);

            // When.
            messageGenerator.SaveMessages(new List<MessageQueuePrice>());

            // Then.
            mockMessageQueueService.Verify(mb => mb.SaveMessagesBulk(It.IsAny<List<MessageQueuePrice>>()), Times.Once);
            mockMessageQueueService.Verify(mb => mb.SaveMessagesRowByRow(It.IsAny<List<MessageQueuePrice>>()), Times.Once);
        }
    }
}
