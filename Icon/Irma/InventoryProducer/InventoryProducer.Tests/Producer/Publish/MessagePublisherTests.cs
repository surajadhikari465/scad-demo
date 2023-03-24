using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Icon.ActiveMQ.Producer;
using InventoryProducer.Producer.Publish;

namespace InventoryProducer.Tests.Producer.Publish
{
    [TestClass]
    public class MessagePublisherTests
    {
        private IMessagePublisher messagePublisher;
        private Mock<IActiveMQProducer> activeMqProducerMock;

        [TestInitialize]
        public void Initialize()
        {
            activeMqProducerMock = new Mock<IActiveMQProducer>();
            messagePublisher = new MessagePublisher(activeMqProducerMock.Object);
        }

        [TestMethod]
        public void PublishMessageSuccess_NoAction_Test()
        {
            // Given
            activeMqProducerMock.Setup(a => a.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));

            // When
            messagePublisher.PublishMessage("message", null, null, null);

            // Assert
            activeMqProducerMock.Verify(a => a.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
        }

        [TestMethod]
        public void PublishMessageSuccess_WithAction_Test()
        {
            // Given
            var onSuccessInvoked = false;
            var onErrorInvoked = false;
            activeMqProducerMock.Setup(a => a.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));

            // When
            messagePublisher.PublishMessage("message", null, () =>
            {
                onSuccessInvoked = true;
            }, (Exception ex) =>
            {
                onErrorInvoked = true;
            });

            // Assert
            activeMqProducerMock.Verify(a => a.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
            Assert.IsTrue(onSuccessInvoked);
            Assert.IsFalse(onErrorInvoked);
        }

        [TestMethod]
        public void PublishMessageError_WithAction_Test()
        {
            // Given
            var onSuccessInvoked = false;
            var onErrorInvoked = false;
            activeMqProducerMock.Setup(a => a.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Throws(new Exception("Test"));

            // When
            messagePublisher.PublishMessage("message", null, () =>
            {
                onSuccessInvoked = true;
            }, (Exception ex) =>
            {
                onErrorInvoked = true;
            });

            // Assert
            activeMqProducerMock.Verify(a => a.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
            Assert.IsTrue(onErrorInvoked);
            Assert.IsFalse(onSuccessInvoked);
        }
    }
}
