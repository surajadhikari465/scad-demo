using Icon.Esb;
using Icon.Esb.Factory;
using Icon.Esb.Producer;
using Icon.Ewic.Transmission.Producers;
using Icon.Framework;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace Icon.Ewic.Tests.Transmission.Producers
{
    [TestClass]
    public class EwicMessageProducerTests
    {
        private EwicMessageProducer producer;
        private Mock<IEsbConnectionFactory> mockConnectionFactory;
        private Mock<IEsbProducer> mockProducer;
        private List<MessageHistory> testMessages;

        [TestInitialize]
        public void Initialize()
        {
            mockConnectionFactory = new Mock<IEsbConnectionFactory>();
            mockProducer = new Mock<IEsbProducer>();
            producer = new EwicMessageProducer(mockConnectionFactory.Object);

            mockConnectionFactory.Setup(f => f.CreateProducer(It.IsAny<EsbConnectionSettings>())).Returns(mockProducer.Object);
        }

        [TestMethod]
        public void SendEwicExclusion_OneExclusionToSend_ProducerShouldBeCalledOnce()
        {
            // Given.
            testMessages = new List<MessageHistory> { new TestMessageHistoryBuilder() };

            // When.
            producer.SendMessages(testMessages);

            // Then.
            mockProducer.Verify(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Exactly(testMessages.Count));
        }

        [TestMethod]
        public void SendEwicExclusion_ThreeExclusionsToSend_ProducerShouldBeCalledThreeTimes()
        {
            // Given.
            testMessages = new List<MessageHistory> 
            { 
                new TestMessageHistoryBuilder(),
                new TestMessageHistoryBuilder(),
                new TestMessageHistoryBuilder()
            };

            // When.
            producer.SendMessages(testMessages);

            // Then.
            mockProducer.Verify(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Exactly(testMessages.Count));
        }
    }
}
