using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MammothWebApi.Service.Decorators;
using MammothWebApi.Service.Models;
using System.Collections.Generic;
using MammothWebApi.Service;
using Moq;
using MammothWebApi.Tests.ModelBuilders;
using Mammoth.Common;
using MammothWebApi.Common;
using MammothWebApi.Service.Services;
using Mammoth.Common.Email;

namespace MammothWebApi.Tests.Services
{
    [TestClass]
    public class EmailErrorServiceDecoratorTests
    {
        private EmailErrorServiceDecorator<AddUpdateItemLocale> decorator;
        private Mock<IUpdateService<AddUpdateItemLocale>> mockService;
        private AddUpdateItemLocale itemLocaleData;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IEmailMessageBuilder<AddUpdateItemLocale>> mockEmailBuilder;

        [TestInitialize]
        public void InitializeTest()
        {
            this.mockService = new Mock<IUpdateService<AddUpdateItemLocale>>();
            this.mockEmailClient = new Mock<IEmailClient>();
            this.mockEmailBuilder = new Mock<IEmailMessageBuilder<AddUpdateItemLocale>>();

            this.itemLocaleData = new AddUpdateItemLocale();
            itemLocaleData.ItemLocales = new List<ItemLocaleServiceModel>
            {
                new TestItemLocaleServiceModelBuilder().Build(),
                new TestItemLocaleServiceModelBuilder().Build(),
                new TestItemLocaleServiceModelBuilder().Build()
            };

        }

        private void ConstructDecorator()
        {
            this.decorator = new EmailErrorServiceDecorator<AddUpdateItemLocale> (this.mockService.Object, this.mockEmailClient.Object, this.mockEmailBuilder.Object);
        }

        [TestMethod]
        public void EmailErrorServiceDecorator_ValidCommandServiceData_ServiceHandleMethodCalled()
        {
            // Given
            ConstructDecorator();

            // When
            this.decorator.Handle(this.itemLocaleData);

            // Then
            this.mockService.Verify(s => s.Handle(this.itemLocaleData), Times.Once, "The command service was not called.");
        }

        [TestMethod]
        public void EmailErrorServiceDecorator_ExceptionThrownDuringCommandService_EmailBuilderBuildMessageCalled()
        {
            // Given
            this.mockService.Setup(s => s.Handle(this.itemLocaleData)).Throws(new Exception());
            ConstructDecorator();

            // When
            try 
	        {	        
		        this.decorator.Handle(this.itemLocaleData);
	        }
	        catch (Exception)
	        {
		        // Then
                this.mockEmailBuilder.Verify(b => b.BuildMessage(It.IsAny<AddUpdateItemLocale>(), It.IsAny<Exception>()), Times.Once,
                    "The Email Message Builder BuildMessage method was not called after exception.");
	        }
        }

        [TestMethod]
        public void EmailErrorServiceDecorator_ExceptionThrownDuringCommandService_EmailClientSendCalled()
        {
            // Given
            this.mockService.Setup(s => s.Handle(this.itemLocaleData)).Throws(new Exception());
            ConstructDecorator();

            // When
            try
            {
                this.decorator.Handle(this.itemLocaleData);
            }
            catch (Exception)
            {
                // Then
                this.mockEmailClient.Verify(c => c.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once,
                    "The EmailClient Send method was not called after the exception.");
            }
        }

        [TestMethod]
        public void EmailErrorServiceDecorator_ExceptionThrownDuringCommandService_EmailClientSubjectSetAsExpected()
        {
            // Given
            this.mockService.Setup(s => s.Handle(this.itemLocaleData)).Throws(new Exception());
            ConstructDecorator();

            string expectedSubject = "Mammoth Service Error - Unhandled Exception";

            // When
            try
            {
                this.decorator.Handle(this.itemLocaleData);
            }
            catch (Exception)
            {
                // Then
                this.mockEmailClient.Verify(c => c.Send(It.IsAny<string>(), expectedSubject), Times.Once,
                    "The EmailClient Send method was not called after the exception.");
            }
        }
    }
}
