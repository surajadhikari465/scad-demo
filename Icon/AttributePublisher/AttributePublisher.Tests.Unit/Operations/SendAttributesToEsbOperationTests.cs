using AttributePublisher.Models;
using AttributePublisher.Operations;
using AttributePublisher.Services;
using Icon.Esb.Producer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace AttributePublisher.Tests.Unit.Operations
{
    [TestClass]
    public class SendAttributesToEsbOperationTests
    {
        private SendAttributesToEsbOperation operation;
        private Mock<IEsbProducer> mockProducer;

        [TestInitialize]
        public void Initialize()
        {
            mockProducer = new Mock<IEsbProducer>();
            operation = new SendAttributesToEsbOperation(null, mockProducer.Object);
        }

        [TestMethod]
        public void SendAttributesToEsbOperation_MessagesExist_SendsMessages()
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
        }
    }
}
