using AttributePublisher.Infrastructure.Operations;
using AttributePublisher.Operations.Decorators;
using AttributePublisher.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AttributePublisher.Tests.Unit.Operations.Decorators
{
    [TestClass]
    public class CheckContinueProcessingOperationDecoratorTests
    {
        private CheckContinueProcessingOperationDecorator decorator;
        private Mock<IOperation<AttributePublisherServiceParameters>> mockOperation;

        [TestInitialize]
        public void Initialize()
        {
            mockOperation = new Mock<IOperation<AttributePublisherServiceParameters>>();
            decorator = new CheckContinueProcessingOperationDecorator(mockOperation.Object);
        }

        [TestMethod]
        public void CheckContinueProcessingOperationDecorator_ContinueProcessingTrue_CallsOperation()
        {
            //Given
            AttributePublisherServiceParameters parameters = new AttributePublisherServiceParameters
            {
                ContinueProcessing = true
            };

            //When
            decorator.Execute(parameters);

            //Then
            mockOperation.Verify(m => m.Execute(It.IsAny<AttributePublisherServiceParameters>()), Times.Once);
        }

        [TestMethod]
        public void CheckContinueProcessingOperationDecorator_ContinueProcessingFalse_DoesNotCallOperation()
        {
            //Given
            AttributePublisherServiceParameters parameters = new AttributePublisherServiceParameters
            {
                ContinueProcessing = false
            };

            //When
            decorator.Execute(parameters);

            //Then
            mockOperation.Verify(m => m.Execute(It.IsAny<AttributePublisherServiceParameters>()), Times.Never);
        }
    }
}
