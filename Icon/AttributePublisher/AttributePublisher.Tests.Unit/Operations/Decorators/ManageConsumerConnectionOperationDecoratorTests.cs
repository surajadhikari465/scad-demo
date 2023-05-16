using AttributePublisher.Infrastructure.Operations;
using AttributePublisher.Operations.Decorators;
using AttributePublisher.Services;
using Icon.ActiveMQ.Producer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AttributePublisher.Tests.Unit.Operations.Decorators
{
    [TestClass]
    public class ManageConsumerConnectionOperationDecoratorTests
    {
        private ManageConsumerConnectionOperationDecorator decorator;
        private Mock<IOperation<AttributePublisherServiceParameters>> mockOperation;
        private Mock<IActiveMQProducer> mockActiveMQProducer;

        [TestInitialize]
        public void Initialize()
        {
            mockOperation = new Mock<IOperation<AttributePublisherServiceParameters>>();
            mockActiveMQProducer = new Mock<IActiveMQProducer>();
            decorator = new ManageConsumerConnectionOperationDecorator(mockOperation.Object, mockActiveMQProducer.Object);
        }

        [TestMethod]
        public void ManageActiveMQConnectionOperationDecorator_Execute_OpensAndDisposesProducer()
        {
            //When
            decorator.Execute(new AttributePublisherServiceParameters());

            //Then
            mockOperation.Verify(m => m.Execute(It.IsAny<AttributePublisherServiceParameters>()), Times.Once);
            mockActiveMQProducer.Verify(m => m.Dispose(), Times.Once);
        }
    }
}
