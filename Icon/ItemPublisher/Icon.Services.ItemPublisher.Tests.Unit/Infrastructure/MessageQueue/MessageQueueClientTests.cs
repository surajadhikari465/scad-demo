using Icon.Esb.Factory;
using Icon.Esb.Producer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Icon.Services.ItemPublisher.Infrastructure.MessageQueue.Communication;
using Icon.ActiveMQ.Factory;
using Icon.ActiveMQ.Producer;

namespace Icon.Services.ItemPublisher.Infrastructure.MessageQueue.Tests
{
    [TestClass()]
    public class MessageQueueClientTests
    {
        [TestMethod]
        public async Task SendMessage_SuccessfulRequest_ShouldReturnSuccess()
        {

            // Given.
            Mock<IEsbConnectionFactory> esbConnectionFactoryMock = new Mock<IEsbConnectionFactory>();
            Mock<IActiveMQConnectionFactory> activeMqConnectionFactoryMock = new Mock<IActiveMQConnectionFactory>();
            Mock<IMessageHeaderBuilder> headerBuilderMock = new Mock<IMessageHeaderBuilder>();
            Mock<IClientIdManager> clientIdMangerMock = new Mock<IClientIdManager>();

            Mock<IEsbProducer> producerMock = new Mock<IEsbProducer>();
            Mock<IActiveMQProducer> activeMqProducerMock = new Mock<IActiveMQProducer>();

            producerMock.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
            activeMqProducerMock.Setup(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
            esbConnectionFactoryMock.Setup(x => x.CreateProducer(It.IsAny<string>(), It.IsAny<bool>())).Returns(producerMock.Object);
            activeMqConnectionFactoryMock.Setup(ap => ap.CreateProducer(It.IsAny<string>(), It.IsAny<bool>())).Returns(activeMqProducerMock.Object);
            clientIdMangerMock.Setup(x => x.GetClientId()).Returns($"clientid");
            MessageQueueClient client = new MessageQueueClient(esbConnectionFactoryMock.Object, headerBuilderMock.Object, clientIdMangerMock.Object, activeMqConnectionFactoryMock.Object);

            // When.
            MessageSendResult result = await client.SendMessage("request", new List<string>() { });

            // Then.
            producerMock.Verify(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
            activeMqProducerMock.Verify(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
            Assert.IsTrue(result.Success);
            Assert.AreEqual("request", result.Request);
            Assert.AreNotEqual(result.MessageId, Guid.Empty);
            Assert.IsTrue(string.IsNullOrWhiteSpace(result.Message));
            Assert.IsTrue(string.IsNullOrWhiteSpace(result.ToString()));
        }

        [TestMethod]
        public async Task SendMessage_ErrorInEsbSend_ShouldReturnFailure()
        {
            // Given.
            Mock<IEsbConnectionFactory> esbConnectionFactoryMock = new Mock<IEsbConnectionFactory>();
            Mock<IActiveMQConnectionFactory> activeMqConnectionFactoryMock = new Mock<IActiveMQConnectionFactory>();
            Mock<IMessageHeaderBuilder> headerBuilderMock = new Mock<IMessageHeaderBuilder>();
            Mock<IEsbProducer> producerMock = new Mock<IEsbProducer>();
            Mock<IActiveMQProducer> activeMqProducerMock = new Mock<IActiveMQProducer>();
            Mock<IClientIdManager> clientIdMangerMock = new Mock<IClientIdManager>();

            producerMock.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Throws(new Exception("Any ESB Exception"));
            esbConnectionFactoryMock.Setup(x => x.CreateProducer(It.IsAny<string>(), It.IsAny<bool>())).Returns(producerMock.Object);
            headerBuilderMock.Setup(x => x.BuildMessageHeader(It.IsAny<List<string>>(), It.IsAny<string>())).Returns(new Dictionary<string, string>() { });
            clientIdMangerMock.Setup(x => x.GetClientId()).Returns($"clientid");
            activeMqProducerMock.Setup(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
            activeMqConnectionFactoryMock.Setup(ap => ap.CreateProducer(It.IsAny<string>(), It.IsAny<bool>())).Returns(activeMqProducerMock.Object);

            MessageQueueClient client = new MessageQueueClient(esbConnectionFactoryMock.Object, headerBuilderMock.Object, clientIdMangerMock.Object, activeMqConnectionFactoryMock.Object);

            // When.
            MessageSendResult result = await client.SendMessage("request", new List<string>() { });

            // Then.
            string errorMessage = "Error Occurred while sending to ESB, message sent to ActiveMQ successfully";

            // Should call both ESB, ActiveMQ send
            activeMqProducerMock.Verify(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
            producerMock.Verify(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(errorMessage, result.Message);
            Assert.IsTrue(result.ToString().StartsWith("Message:" + errorMessage));
        }

        [TestMethod]
        public async Task SendMessage_ErrorInActiveMqSend_ShouldReturnFailure()
        {
            // Given.
            Mock<IEsbConnectionFactory> esbConnectionFactoryMock = new Mock<IEsbConnectionFactory>();
            Mock<IActiveMQConnectionFactory> activeMqConnectionFactoryMock = new Mock<IActiveMQConnectionFactory>();
            Mock<IMessageHeaderBuilder> headerBuilderMock = new Mock<IMessageHeaderBuilder>();
            Mock<IEsbProducer> producerMock = new Mock<IEsbProducer>();
            Mock<IActiveMQProducer> activeMqProducerMock = new Mock<IActiveMQProducer>();
            Mock<IClientIdManager> clientIdMangerMock = new Mock<IClientIdManager>();

            producerMock.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
            esbConnectionFactoryMock.Setup(x => x.CreateProducer(It.IsAny<string>(), It.IsAny<bool>())).Returns(producerMock.Object);
            headerBuilderMock.Setup(x => x.BuildMessageHeader(It.IsAny<List<string>>(), It.IsAny<string>())).Returns(new Dictionary<string, string>() { });
            clientIdMangerMock.Setup(x => x.GetClientId()).Returns($"clientid");
            activeMqProducerMock.Setup(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Throws(new Exception("Any ActiveMQ Exception"));
            activeMqConnectionFactoryMock.Setup(ap => ap.CreateProducer(It.IsAny<string>(), It.IsAny<bool>())).Returns(activeMqProducerMock.Object);

            MessageQueueClient client = new MessageQueueClient(esbConnectionFactoryMock.Object, headerBuilderMock.Object, clientIdMangerMock.Object, activeMqConnectionFactoryMock.Object);

            // When.
            MessageSendResult result = await client.SendMessage("request", new List<string>() { });

            // Then.
            string errorMessage = "Error Occurred while sending to ActiveMQ, message sent to ESB successfully";

            // Should call both ESB, ActiveMQ send
            activeMqProducerMock.Verify(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
            producerMock.Verify(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(errorMessage, result.Message);
            Assert.IsTrue(result.ToString().StartsWith("Message:" + errorMessage));
        }

        [TestMethod]
        public async Task SendMessage_ErrorInEsbAndActiveMqSend_ShouldReturnFailure()
        {
            // Given.
            Mock<IEsbConnectionFactory> esbConnectionFactoryMock = new Mock<IEsbConnectionFactory>();
            Mock<IActiveMQConnectionFactory> activeMqConnectionFactoryMock = new Mock<IActiveMQConnectionFactory>();
            Mock<IMessageHeaderBuilder> headerBuilderMock = new Mock<IMessageHeaderBuilder>();
            Mock<IEsbProducer> producerMock = new Mock<IEsbProducer>();
            Mock<IActiveMQProducer> activeMqProducerMock = new Mock<IActiveMQProducer>();
            Mock<IClientIdManager> clientIdMangerMock = new Mock<IClientIdManager>();

            producerMock.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Throws(new Exception("Any ESB Exception"));
            esbConnectionFactoryMock.Setup(x => x.CreateProducer(It.IsAny<string>(), It.IsAny<bool>())).Returns(producerMock.Object);
            headerBuilderMock.Setup(x => x.BuildMessageHeader(It.IsAny<List<string>>(), It.IsAny<string>())).Returns(new Dictionary<string, string>() { });
            clientIdMangerMock.Setup(x => x.GetClientId()).Returns($"clientid");
            activeMqProducerMock.Setup(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Throws(new Exception("Any ActiveMQ Exception"));
            activeMqConnectionFactoryMock.Setup(ap => ap.CreateProducer(It.IsAny<string>(), It.IsAny<bool>())).Returns(activeMqProducerMock.Object);

            MessageQueueClient client = new MessageQueueClient(esbConnectionFactoryMock.Object, headerBuilderMock.Object, clientIdMangerMock.Object, activeMqConnectionFactoryMock.Object);

            // When.
            MessageSendResult result = await client.SendMessage("request", new List<string>() { });

            // Then.
            string errorMessage = "Error Occurred while sending to both ESB and ActiveMQ";

            // Should call both ESB, ActiveMQ send
            activeMqProducerMock.Verify(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
            producerMock.Verify(ap => ap.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);

            Assert.IsFalse(result.Success);
            Assert.AreEqual(errorMessage, result.Message);
            Assert.IsTrue(result.ToString().StartsWith("Message:" + errorMessage));
        }
    }
}