using Icon.Esb.Factory;
using Icon.Esb.Producer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Infrastructure.Esb.Tests
{
    [TestClass()]
    public class EsbClientTests
    {
        [TestMethod]
        public async Task SendMessage_SuccessfulRequest_ShouldReturnSuccess()
        {
            // Given.
            Mock<IEsbConnectionFactory> esbConnectionFactoryMock = new Mock<IEsbConnectionFactory>();
            Mock<IEsbHeaderBuilder> headerBuilderMock = new Mock<IEsbHeaderBuilder>();

            Mock<IEsbProducer> producerMock = new Mock<IEsbProducer>();
            producerMock.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
            esbConnectionFactoryMock.Setup(x => x.CreateProducer(It.IsAny<bool>())).Returns(producerMock.Object);

            EsbClient client = new EsbClient(esbConnectionFactoryMock.Object, headerBuilderMock.Object);

            // When.
            EsbSendResult result = await client.SendMessage("request", new List<string>() { });

            // Then.
            Assert.IsTrue(result.Success);
            Assert.AreEqual("request", result.Request);
            Assert.AreNotEqual(result.MessageId, Guid.Empty);
            Assert.IsTrue(string.IsNullOrWhiteSpace(result.Message));
            Assert.IsTrue(string.IsNullOrWhiteSpace(result.ToString()));
        }

        [TestMethod]
        public async Task SendMessage_ErrorInRequest_ShouldReturnFailure()
        {
            // Given.
            Mock<IEsbConnectionFactory> esbConnectionFactoryMock = new Mock<IEsbConnectionFactory>();
            Mock<IEsbHeaderBuilder> headerBuilderMock = new Mock<IEsbHeaderBuilder>();
            Mock<IEsbProducer> producerMock = new Mock<IEsbProducer>();
            producerMock.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Throws(new Exception("Any ESB Exception"));
            esbConnectionFactoryMock.Setup(x => x.CreateProducer(It.IsAny<bool>())).Returns(producerMock.Object);
            headerBuilderMock.Setup(x => x.BuildMessageHeader(It.IsAny<List<string>>())).Returns(new Dictionary<string, string>() { });
            EsbClient client = new EsbClient(esbConnectionFactoryMock.Object, headerBuilderMock.Object);

            // When.
            EsbSendResult result = await client.SendMessage("request", new List<string>() { });

            // Then.
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Error Occurred", result.Message);
            Assert.IsTrue(result.ToString().StartsWith("Message:Error Occurred"));
        }
    }
}