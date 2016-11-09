using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb.CchTax.Commands;
using Icon.Esb.CchTax.MessageParsers;
using Icon.Esb.CchTax.Models;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Subscriber;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using TIBCO.EMS;

namespace Icon.Esb.CchTax.Tests
{
    [TestClass]
    public class CchTaxListenerTests
    {
        private CchTaxListener listener;
        private CchTaxListenerApplicationSettings applicationSettings;
        private EsbConnectionSettings connectionSettings;
        private Mock<IEsbSubscriber> mockSubscriber;
        private Mock<IMessageParser<List<TaxHierarchyClassModel>>> mockMessageParser;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<ILogger<CchTaxListener>> mockLogger;
        private Mock<ICommandHandler<SaveTaxHierarchyClassesCommand>> mockSaveTaxHierarchyClassCommandHandler;
        private List<RegionModel> regions;
        private Mock<IEsbMessage> mockMessage;
        private Mock<ICommandHandler<SaveTaxToMammothCommand>> mockSaveTaxMammothCommandHandler;

        [TestInitialize]
        public void Initialize()
        {
            applicationSettings = new CchTaxListenerApplicationSettings();
            connectionSettings = new EsbConnectionSettings { SessionMode = SessionMode.ClientAcknowledge };
            mockSubscriber = new Mock<IEsbSubscriber>();
            mockMessageParser = new Mock<IMessageParser<List<TaxHierarchyClassModel>>>();
            mockEmailClient = new Mock<IEmailClient>();
            mockLogger = new Mock<ILogger<CchTaxListener>>();
            mockSaveTaxHierarchyClassCommandHandler = new Mock<ICommandHandler<SaveTaxHierarchyClassesCommand>>();
            regions = new List<RegionModel>();
            mockMessage = new Mock<IEsbMessage>();
            mockSaveTaxMammothCommandHandler = new Mock<ICommandHandler<SaveTaxToMammothCommand>>();

            listener = new CchTaxListener(applicationSettings,
                connectionSettings,
                mockSubscriber.Object,
                mockMessageParser.Object,
                mockEmailClient.Object,
                mockLogger.Object,
                mockSaveTaxHierarchyClassCommandHandler.Object,
                mockSaveTaxMammothCommandHandler.Object,
                regions);
        }

        [TestMethod]
        public void HandleMessage_ValidMessage_ShouldSaveMessages()
        {
            //Given 
            var message = File.ReadAllText(@"TestMessages\test_tax_message.xml");
            mockMessage.SetupGet(m => m.MessageText)
                .Returns(message);
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>()))
                .Returns(new List<TaxHierarchyClassModel>());

            //When
            listener.HandleMessage(null, new EsbMessageEventArgs { Message = mockMessage.Object });

            //Then
            mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
            mockSaveTaxHierarchyClassCommandHandler.Verify(m => m.Execute(It.IsAny<SaveTaxHierarchyClassesCommand>()), Times.Once);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Never);
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            mockMessage.Verify(m => m.Acknowledge(), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_ErrorSavingMessage_ShouldLogAndNotifyError()
        {
            //Given 
            var message = File.ReadAllText(@"TestMessages\test_tax_message.xml");
            mockMessage.SetupGet(m => m.MessageText)
                .Returns(message);
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>()))
                .Returns(new List<TaxHierarchyClassModel>());
            mockSaveTaxHierarchyClassCommandHandler.Setup(m => m.Execute(It.IsAny<SaveTaxHierarchyClassesCommand>()))
                .Throws(new Exception("Test Exception"));

            //When
            listener.HandleMessage(null, new EsbMessageEventArgs { Message = mockMessage.Object });

            //Then
            mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
            mockSaveTaxHierarchyClassCommandHandler.Verify(m => m.Execute(It.IsAny<SaveTaxHierarchyClassesCommand>()), Times.Once);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockMessage.Verify(m => m.Acknowledge(), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_ErrorParsingMessage_ShouldLogAndNotifyError()
        {
            //Given 
            var message = File.ReadAllText(@"TestMessages\test_tax_message.xml");
            mockMessage.SetupGet(m => m.MessageText)
                .Returns(message);
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>()))
                .Throws(new Exception("Test Exception"));

            //When
            listener.HandleMessage(null, new EsbMessageEventArgs { Message = mockMessage.Object });

            //Then
            mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
            mockSaveTaxHierarchyClassCommandHandler.Verify(m => m.Execute(It.IsAny<SaveTaxHierarchyClassesCommand>()), Times.Never);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockMessage.Verify(m => m.Acknowledge(), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_ValidMessage_SaveMessageToMammothDatabase()
        {
            // Given
            var testTaxHierarchyClasses = new List<TaxHierarchyClassModel>
            {
                new TaxHierarchyClassModel { HierarchyClassId = 0 },
                new TaxHierarchyClassModel { HierarchyClassId = 0 },
                new TaxHierarchyClassModel { HierarchyClassId = 0 }
            };
            var message = File.ReadAllText(@"TestMessages\test_tax_message.xml");
            mockMessage.SetupGet(m => m.MessageText)
                .Returns(message);
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>()))
                .Returns(testTaxHierarchyClasses);
            //Update the tax HierarchyClassIds when SaveTaxHierarchyClassesCommandHandler is called
            mockSaveTaxHierarchyClassCommandHandler.Setup(m => m.Execute(It.IsAny<SaveTaxHierarchyClassesCommand>()))
                .Callback(() =>
                {
                    for (int i = 0; i < testTaxHierarchyClasses.Count; i++)
                    {
                        testTaxHierarchyClasses[i].HierarchyClassId = i + 55;
                    }
                });

            // When
            listener.HandleMessage(null, new EsbMessageEventArgs { Message = mockMessage.Object });

            // Then
            mockSaveTaxMammothCommandHandler.Verify(
                m => m.Execute(It.Is<SaveTaxToMammothCommand>(
                    c => c.TaxHierarchyClasses == testTaxHierarchyClasses
                        && c.TaxHierarchyClasses[0].HierarchyClassId == 55
                        && c.TaxHierarchyClasses[1].HierarchyClassId == 56
                        && c.TaxHierarchyClasses[2].HierarchyClassId == 57)), 
                Times.Once,
                "The SaveTaxToMammothCommandHandler was not called.");
        }

        [TestMethod]
        public void HandleMessage_SaveMessageToMammothCommandHandlerThrowsException_ShouldLogAndNotify()
        {
            //Given
            var message = File.ReadAllText(@"TestMessages\test_tax_message.xml");
            mockMessage.SetupGet(m => m.MessageText)
                .Returns(message);
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>()))
                .Returns(new List<TaxHierarchyClassModel>());
            mockSaveTaxMammothCommandHandler.Setup(m => m.Execute(It.IsAny<SaveTaxToMammothCommand>()))
                .Throws(new Exception("Test Mammoth Exception"));

            //When
            listener.HandleMessage(null, new EsbMessageEventArgs { Message = mockMessage.Object });

            //Then
            mockLogger.Verify(m => m.Error(It.Is<string>(s => s.Contains("Test Mammoth Exception"))), Times.Once, "The logger was not called.");
            mockEmailClient.Verify(m => m.Send(It.Is<string>(s => s.Contains("Test Mammoth Exception")), It.IsAny<string>()), Times.Once, "The email client was not called.");
            mockMessage.Verify(m => m.Acknowledge(), Times.Once, "The message was not acknowledged.");
        }
    }
}
