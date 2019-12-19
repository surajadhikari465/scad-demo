using AttributePublisher.Infrastructure.Operations;
using AttributePublisher.Operations.Decorators;
using AttributePublisher.Services;
using Icon.Esb.Producer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AttributePublisher.Tests.Unit.Operations.Decorators
{
    [TestClass]
    public class ManageEsbConnectionOperationDecoratorTests
    {
        private ManageEsbConnectionOperationDecorator decorator;
        private Mock<IOperation<AttributePublisherServiceParameters>> mockOperation;
        private Mock<IEsbProducer> mockProducer;

        [TestInitialize]
        public void Initialize()
        {
            mockOperation = new Mock<IOperation<AttributePublisherServiceParameters>>();
            mockProducer = new Mock<IEsbProducer>();
            decorator = new ManageEsbConnectionOperationDecorator(mockOperation.Object, mockProducer.Object);
        }

        [TestMethod]
        public void ManageEsbConnectionOperationDecorator_Execute_OpensAndDisposesProducer()
        {
            //When
            decorator.Execute(new AttributePublisherServiceParameters());

            //Then
            mockOperation.Verify(m => m.Execute(It.IsAny<AttributePublisherServiceParameters>()), Times.Once);
            mockProducer.Verify(m => m.OpenConnection(), Times.Once);
            mockProducer.Verify(m => m.Dispose(), Times.Once);
        }
    }
}
