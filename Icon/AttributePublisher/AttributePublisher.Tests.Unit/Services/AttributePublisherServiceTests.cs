using AttributePublisher.DataAccess.Commands;
using AttributePublisher.DataAccess.Models;
using AttributePublisher.Infrastructure;
using AttributePublisher.Infrastructure.Operations;
using AttributePublisher.Services;
using Icon.Common.DataAccess;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace AttributePublisher.Tests.Unit.Services
{
    [TestClass]
    public class AttributePublisherServiceTests
    {
        private AttributePublisherService service;
        private RecurringServiceSettings settings;
        private Mock<IOperation<AttributePublisherServiceParameters>> mockOperation;
        private Mock<ICommandHandler<AddAttributesToMessageQueueCommand>> mockAddAttributesToMessageQueueCommandHandler;
        private AttributePublisherServiceParameters parameters;
        private Mock<ILogger> mockLogger;

        [TestInitialize]
        public void Initialize()
        {
            settings = new RecurringServiceSettings { RunInterval = 1 };
            mockOperation = new Mock<IOperation<AttributePublisherServiceParameters>>();
            mockAddAttributesToMessageQueueCommandHandler = new Mock<ICommandHandler<AddAttributesToMessageQueueCommand>>();
            parameters = new AttributePublisherServiceParameters();
            mockLogger = new Mock<ILogger>();
            service = new AttributePublisherService(settings, mockOperation.Object, mockAddAttributesToMessageQueueCommandHandler.Object, parameters, mockLogger.Object);
        }

        [TestMethod]
        public void AttributePublisherService_Run_CallsOperationUntilContinueProcessingIsFalse()
        {
            //Given
            int operationInvokedCount = 0;
            mockOperation.Setup(m => m.Execute(It.IsAny<AttributePublisherServiceParameters>()))
                .Callback<AttributePublisherServiceParameters>((p) =>
                {
                    if (operationInvokedCount == 3)
                    {
                        p.ContinueProcessing = false;
                    }
                    else
                    {
                        operationInvokedCount++;
                    }
                });

            //When
            service.Run();

            //Then
            mockOperation.Verify(m => m.Execute(It.IsAny<AttributePublisherServiceParameters>()), Times.Exactly(4));
        }

        [TestMethod]
        public void AttributePublisherService_HandleRunException_LogsException()
        {
            //When
            service.HandleRunException(new Exception());

            //Then
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void AttributePublisherService_HandleServiceStart_LogsStart()
        {
            //When
            service.HandleServiceStart();

            //Then
            mockLogger.Verify(m => m.Info(AttributePublisherResources.StartServiceLogMessage), Times.Once);
        }

        [TestMethod]
        public void AttributePublisherService_HandleServiceStop_LogsStop()
        {
            //Given
            parameters.ContinueProcessing = true;

            //When
            service.HandleServiceStop();

            //Then
            Assert.IsFalse(parameters.ContinueProcessing);
            mockLogger.Verify(m => m.Info(AttributePublisherResources.StopServiceLogMessage), Times.Once);
        }

        [TestMethod]
        public void AttributePublisherService_HandleServiceStopAndModelsExist_LogsStopAndAddsToMessageQueue()
        {
            //Given
            parameters.Attributes = new List<AttributeModel> { new AttributeModel() };

            //When
            service.HandleServiceStop();

            //Then
            Assert.IsFalse(parameters.ContinueProcessing);
            mockLogger.Verify(m => m.Info(AttributePublisherResources.StopServiceLogMessage), Times.Once);
            mockLogger.Verify(m => m.Info(AttributePublisherResources.RequeueingAttributesOnStop), Times.Once); 
            mockAddAttributesToMessageQueueCommandHandler.Verify(m => m.Execute(It.IsAny<AddAttributesToMessageQueueCommand>()), Times.Once);
        }

        [TestMethod]
        public void AttributePublisherService_HandleServiceStopAndModelsExistAndAddingToMessageQueueThrowsException_LogsRequeingError()
        {
            //Given
            parameters.Attributes = new List<AttributeModel> { new AttributeModel() };
            mockAddAttributesToMessageQueueCommandHandler.Setup(m => m.Execute(It.IsAny<AddAttributesToMessageQueueCommand>()))
                .Throws(new Exception());

            //When
            service.HandleServiceStop();

            //Then
            Assert.IsFalse(parameters.ContinueProcessing);
            mockLogger.Verify(m => m.Info(AttributePublisherResources.StopServiceLogMessage), Times.Once);
            mockLogger.Verify(m => m.Info(AttributePublisherResources.RequeueingAttributesOnStop), Times.Once);
            mockAddAttributesToMessageQueueCommandHandler.Verify(m => m.Execute(It.IsAny<AddAttributesToMessageQueueCommand>()), Times.Once);
            mockLogger.Verify(m => m.Error(It.Is<string>(s => s.Contains(AttributePublisherResources.RequeueingAttributesOnStopError))), Times.Once);
        }
    }
}
