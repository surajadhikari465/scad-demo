using AttributePublisher.DataAccess.Commands;
using AttributePublisher.DataAccess.Models;
using AttributePublisher.Infrastructure.Operations;
using AttributePublisher.Operations.Decorators;
using AttributePublisher.Services;
using Icon.Common.DataAccess;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace AttributePublisher.Tests.Unit.Operations.Decorators
{
    [TestClass]
    public class ErrorHandlingOperationDecoratorTests
    {
        private ErrorHandlingOperationDecorator decorator;
        private Mock<ILogger> mockLogger;
        private Mock<IOperation<AttributePublisherServiceParameters>> mockOperation;
        private Mock<ICommandHandler<AddAttributesToMessageQueueCommand>> mockAddAttributesToMessageQueueCommandHandler;

        [TestInitialize]
        public void Initialize()
        {
            mockOperation = new Mock<IOperation<AttributePublisherServiceParameters>>();
            mockAddAttributesToMessageQueueCommandHandler = new Mock<ICommandHandler<AddAttributesToMessageQueueCommand>>();
            mockLogger = new Mock<ILogger>();
            decorator = new ErrorHandlingOperationDecorator(mockOperation.Object, mockAddAttributesToMessageQueueCommandHandler.Object, mockLogger.Object);
        }

        [TestMethod]
        public void ErrorHandlingOperationDecorator_OperationThrowsError_CallsLogger()
        {
            //Given
            mockOperation.Setup(m => m.Execute(It.IsAny<AttributePublisherServiceParameters>()))
                .Throws(new Exception());
            var parameters = new AttributePublisherServiceParameters { ContinueProcessing = true };

            //When
            decorator.Execute(parameters);

            //Then
            Assert.IsFalse(parameters.ContinueProcessing);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
            mockLogger.Verify(m => m.Info(It.IsAny<string>()), Times.Never);
            mockAddAttributesToMessageQueueCommandHandler.Verify(m => m.Execute(It.IsAny<AddAttributesToMessageQueueCommand>()), Times.Never);
        }

        [TestMethod]
        public void ErrorHandlingOperationDecorator_OperationThrowsErrorAndAttributesExist_AddsModelsToMessageQueue()
        {
            //Given
            mockOperation.Setup(m => m.Execute(It.IsAny<AttributePublisherServiceParameters>()))
                .Throws(new Exception());
            var parameters = new AttributePublisherServiceParameters { ContinueProcessing = true, Attributes = new List<AttributeModel> { new AttributeModel() } };

            //When
            decorator.Execute(parameters);

            //Then
            Assert.IsFalse(parameters.ContinueProcessing);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
            mockLogger.Verify(m => m.Info(It.IsAny<string>()), Times.Once);
            mockAddAttributesToMessageQueueCommandHandler.Verify(m => m.Execute(It.IsAny<AddAttributesToMessageQueueCommand>()), Times.Once);
        }

        [TestMethod]
        public void ErrorHandlingOperationDecorator_OperationThrowsErrorAndAddAttributesThrowsException_LogsError()
        {
            //Given
            mockOperation.Setup(m => m.Execute(It.IsAny<AttributePublisherServiceParameters>()))
                .Throws(new Exception());
            mockAddAttributesToMessageQueueCommandHandler.Setup(m => m.Execute(It.IsAny<AddAttributesToMessageQueueCommand>()))
                .Throws(new Exception());
            var parameters = new AttributePublisherServiceParameters { ContinueProcessing = true, Attributes = new List<AttributeModel> { new AttributeModel() } };

            //When
            decorator.Execute(parameters);

            //Then
            Assert.IsFalse(parameters.ContinueProcessing);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Exactly(2));
            mockLogger.Verify(m => m.Info(It.IsAny<string>()), Times.Once);
            mockAddAttributesToMessageQueueCommandHandler.Verify(m => m.Execute(It.IsAny<AddAttributesToMessageQueueCommand>()), Times.Once);
        }
    }
}
