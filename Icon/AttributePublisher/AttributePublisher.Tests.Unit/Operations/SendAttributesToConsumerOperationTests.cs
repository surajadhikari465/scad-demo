using AttributePublisher.Models;
using AttributePublisher.Operations;
using AttributePublisher.Services;
using Icon.ActiveMQ.Producer;
using Icon.Esb.Producer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace AttributePublisher.Tests.Unit.Operations
{
    [TestClass]
    public class SendAttributesToConsumerOperationTests
    {
        private SendAttributesToConsumerOperation operation;
        private Mock<IEsbProducer> mockProducer;
        private Mock<IActiveMQProducer> mockActiveMQProducer;

        [TestInitialize]
        public void Initialize()
        {
            mockProducer = new Mock<IEsbProducer>();
            mockActiveMQProducer = new Mock<IActiveMQProducer>();
            operation = new SendAttributesToConsumerOperation(null, mockProducer.Object, mockActiveMQProducer.Object);
        }

        [TestMethod]
        public void SendAttributesToConsumerOperation_MessagesExist_SendsMessages()
        {
            //Given

            //When
            operation.Execute(new AttributePublisherServiceParameters
            {
                AttributeMessages = new List<AttributeMessageModel>
                {
                    new AttributeMessageModel(),
                    new AttributeMessageModel(),
                    new AttributeMessageModel(),
                    new AttributeMessageModel()
                }
            });

            //Then
            mockProducer.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Exactly(4));
            mockActiveMQProducer.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Exactly(4));
        }
    }
}
