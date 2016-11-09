using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MammothWebApi.Service.Decorators;
using MammothWebApi.Service.Models;
using System.Collections.Generic;
using MammothWebApi.Service;
using Moq;
using MammothWebApi.Common;
using Mammoth.Logging;
using MammothWebApi.Tests.ModelBuilders;
using System.Reflection;
using MammothWebApi.Service.Services;


namespace MammothWebApi.Tests.Services
{
    [TestClass]
    public class LoggingServiceDecoratorTests
    {
        private LoggingServiceDecorator<AddUpdateItemLocale> decorator;
        private Mock<IService<AddUpdateItemLocale>> mockService;
        private Mock<ILogger> mockLogger;
        private AddUpdateItemLocale serviceData;
        
        [TestInitialize]
        public void InitializeTest()
        {
            this.mockService = new Mock<IService<AddUpdateItemLocale>>();
            this.mockLogger = new Mock<ILogger>();
            this.serviceData = new AddUpdateItemLocale();
            this.serviceData.ItemLocales = new List<ItemLocaleServiceModel>
            {
                new TestItemLocaleServiceModelBuilder().Build(),
                new TestItemLocaleServiceModelBuilder().Build(),
                new TestItemLocaleServiceModelBuilder().Build()
            };
        }

        private void ConstructDecorator()
        {
            this.decorator = new LoggingServiceDecorator<AddUpdateItemLocale>(this.mockService.Object, this.mockLogger.Object);
        }

        [TestMethod]
        public void LoggingServiceDecorator_ValidCommandServiceData_LoggerDebugCalledOneTime()
        {
            // Given
            ConstructDecorator();

            // When
            this.decorator.Handle(this.serviceData);

            // Then
            this.mockLogger.Verify(l => l.Debug(It.IsAny<String>()), Times.Once, "The Debug method of the logger was not called.");
        }

        [TestMethod]
        public void LoggingServiceDecorator_ValidCommandServiceData_CommandServiceHandleIsCalledWithCommandServiceData()
        {
            // Given
            ConstructDecorator();

            // When
            this.decorator.Handle(this.serviceData);

            // Then
            this.mockService.Verify(s => s.Handle(this.serviceData), Times.Once, "The CommandService is not called one time.");
        }

        [TestMethod]
        public void LoggingServiceDecorator_CommandServiceThrowsException_ExceptionHandlerCalled()
        {
            // Given
            this.mockService.Setup(s => s.Handle(this.serviceData)).Throws(new Exception());
            ConstructDecorator();

            try
            {
                // When
                this.decorator.Handle(this.serviceData);
            }
            catch (Exception e)
            {
                // Then
                this.mockLogger.Verify(h => h.Error(It.IsAny<string>(), e), Times.Once,
                    "The ExceptionHandler did not call HandleException method after an exception was thrown by the Command Service.");                
            }            
        }
    }
}
