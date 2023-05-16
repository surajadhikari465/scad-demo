using AttributePublisher.Models;
using AttributePublisher.Operations;
using AttributePublisher.Services;
using Icon.ActiveMQ.Producer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace AttributePublisher.Tests.Unit.Operations
{
    [TestClass]
    public class SendAttributesToConsumerOperationTests
    {
        private SendAttributesToConsumerOperation operation;
        private Mock<IActiveMQProducer> mockActiveMQProducer;

        [TestInitialize]
        public void Initialize()
        {
            mockActiveMQProducer = new Mock<IActiveMQProducer>();
            operation = new SendAttributesToConsumerOperation(null, mockActiveMQProducer.Object);
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
            mockActiveMQProducer.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Exactly(4));
        }
    }
}
