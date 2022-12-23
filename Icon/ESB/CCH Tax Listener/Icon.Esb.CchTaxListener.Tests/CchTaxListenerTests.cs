using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb.CchTax.Commands;
using Icon.Esb.CchTax.MessageParsers;
using Icon.Esb.CchTax.Models;
using Icon.Logging;
using Icon.Dvs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using TIBCO.EMS;
using Icon.Dvs.Model;
using Icon.Dvs.Subscriber;

namespace Icon.Esb.CchTax.Tests
{
    [TestClass]
    public class CchTaxListenerTests
    {
        private CchTaxListener listener;
        private CchTaxListenerApplicationSettings applicationSettings;
        private DvsListenerSettings listenerSettings;
        private Mock<IDvsSubscriber> mockSubscriber;
        private Mock<IMessageParser<List<TaxHierarchyClassModel>>> mockMessageParser;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<ILogger<CchTaxListener>> mockLogger;
        private Mock<ICommandHandler<SaveTaxHierarchyClassesCommand>> mockSaveTaxHierarchyClassCommandHandler;
        private List<RegionModel> regions;
        private DvsSqsMessage sqsMessage;
        private Mock<ICommandHandler<SaveTaxToMammothCommand>> mockSaveTaxMammothCommandHandler;
        private String exceptionMessage;

        [TestInitialize]
        public void Initialize()
        {
            applicationSettings = new CchTaxListenerApplicationSettings();
            listenerSettings = DvsListenerSettings.CreateSettingsFromConfig();
            mockMessageParser = new Mock<IMessageParser<List<TaxHierarchyClassModel>>>();
            mockEmailClient = new Mock<IEmailClient>();
            mockLogger = new Mock<ILogger<CchTaxListener>>();
            mockSaveTaxHierarchyClassCommandHandler = new Mock<ICommandHandler<SaveTaxHierarchyClassesCommand>>();
            regions = new List<RegionModel>();
            exceptionMessage = "Test Exception";
            sqsMessage = new DvsSqsMessage()
            {
                MessageAttributes = new Dictionary<string, string>() {
                    { "IconMessageID", "1" },
                    { "toBeReceivedBy", "ALL" }
                },
                S3BucketName = "SampleBucket",
                S3Key = "SampleS3Key"
            };
            mockSaveTaxMammothCommandHandler = new Mock<ICommandHandler<SaveTaxToMammothCommand>>();
            mockSubscriber = new Mock<IDvsSubscriber>();

            listener = new CchTaxListener(
                listenerSettings,
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
            var message = new DvsMessage(sqsMessage, System.IO.File.ReadAllText(@"TestMessages\test_tax_message.xml"));

            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<DvsMessage>()))
                .Returns(new List<TaxHierarchyClassModel>());

            //When
            listener.HandleMessage(message);

            //Then
            mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<DvsMessage>()), Times.Once);
            mockSaveTaxHierarchyClassCommandHandler.Verify(m => m.Execute(It.IsAny<SaveTaxHierarchyClassesCommand>()), Times.Once);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Never);
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
           
        }

        [TestMethod]
        public void HandleMessage_ErrorSavingMessage_ShouldLogAndNotifyError()
        {
            //Given 
            var message = new DvsMessage(sqsMessage, System.IO.File.ReadAllText(@"TestMessages\test_tax_message.xml"));
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<DvsMessage>()))
                .Returns(new List<TaxHierarchyClassModel>());
            var exceptionMessageExpected = "Test Exception";
            mockSaveTaxHierarchyClassCommandHandler.Setup(m => m.Execute(It.IsAny<SaveTaxHierarchyClassesCommand>()))
                .Throws(new Exception(exceptionMessageExpected));
            var exceptionMessageCaught = "did not catch an exception";
            //When
            try
            {
                listener.HandleMessage(message);
            }
            //Then
            catch(Exception e)
            {
                exceptionMessageCaught = e.Message;
                mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<DvsMessage>()), Times.Once);
                mockSaveTaxHierarchyClassCommandHandler.Verify(m => m.Execute(It.IsAny<SaveTaxHierarchyClassesCommand>()), Times.Once);
            }
            Assert.AreEqual(exceptionMessageExpected, exceptionMessageCaught);
        }

        [TestMethod]
        public void HandleMessage_ErrorParsingMessage_ShouldLogAndNotifyError()
        {
            //Given 
            var message = new DvsMessage(sqsMessage, System.IO.File.ReadAllText(@"TestMessages\test_tax_message.xml"));
            var exceptionMessageExpected = "Test Exception";
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<DvsMessage>()))
                .Throws(new Exception(exceptionMessage));
            var exceptionMessageCaught = "did not catch an exception";
            //When
            try
            {
                listener.HandleMessage(message);
            }
            //Then
            catch (Exception e)
            {
                exceptionMessageCaught = e.Message;
                mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<DvsMessage>()), Times.Once);
                mockSaveTaxHierarchyClassCommandHandler.Verify(m => m.Execute(It.IsAny<SaveTaxHierarchyClassesCommand>()), Times.Never);
            }
            Assert.AreEqual(exceptionMessageCaught, exceptionMessageExpected);
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
            var message = new DvsMessage(sqsMessage, System.IO.File.ReadAllText(@"TestMessages\test_tax_message.xml"));
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<DvsMessage>()))
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
            listener.HandleMessage(message);

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
            var message = new DvsMessage(sqsMessage, System.IO.File.ReadAllText(@"TestMessages\test_tax_message.xml"));
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<DvsMessage>()))
                .Returns(new List<TaxHierarchyClassModel>());
            var expectedExceptionMessage = "Test Mammoth Exception";
            mockSaveTaxMammothCommandHandler.Setup(m => m.Execute(It.IsAny<SaveTaxToMammothCommand>()))
                .Throws(new Exception(expectedExceptionMessage));

            //When
            var exceptionCaughtMessage = "did not catch and Exception";
            try
            {
                listener.HandleMessage(message);
            }
            //Then
            catch(Exception e) {
                exceptionCaughtMessage = e.Message;
            }
            Assert.AreEqual(expectedExceptionMessage, exceptionCaughtMessage);
        }
    }
}
