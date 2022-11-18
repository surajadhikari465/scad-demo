using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Dvs;
using Icon.Dvs.MessageParser;
using Icon.Dvs.Subscriber;
using Icon.Dvs.Model;
using Icon.Logging;
using Mammoth.Esb.LocaleListener.Commands;
using Mammoth.Esb.LocaleListener.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Mammoth.Esb.LocaleListener.Tests
{
    [TestClass]
    public class MammothLocaleListenerTests
    {
        private MammothLocaleListener listener;
        private DvsListenerSettings listenerApplicationSettings;
        private Mock<IDvsSubscriber> mockSubscriber;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<ILogger<MammothLocaleListener>> mockLogger;
        private Mock<IMessageParser<List<LocaleModel>>> mockMessageParser;
        private Mock<ICommandHandler<AddOrUpdateLocalesCommand>> mockAddOrUpdateLocalesCommandHandler;
        private DvsMessage mockMessage;

        [TestInitialize]
        public void Initialize()
        {
            listenerApplicationSettings = DvsListenerSettings.CreateSettingsFromConfig();
            mockSubscriber = new Mock<IDvsSubscriber>();
            mockEmailClient = new Mock<IEmailClient>();
            mockLogger = new Mock<ILogger<MammothLocaleListener>>();
            mockMessageParser = new Mock<IMessageParser<List<LocaleModel>>>();
            mockAddOrUpdateLocalesCommandHandler = new Mock<ICommandHandler<AddOrUpdateLocalesCommand>>();
            mockMessage = new DvsMessage(new DvsSqsMessage(), "");

            listener = new MammothLocaleListener(listenerApplicationSettings,
                mockSubscriber.Object,
                mockEmailClient.Object,
                mockLogger.Object,
                mockMessageParser.Object,
                mockAddOrUpdateLocalesCommandHandler.Object);
        }

        [TestMethod]
        public void HandleMessage_ParseSuccessful_ShouldAddOrUpdateLocales()
        {
            //Given
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<DvsMessage>())).Returns(new List<LocaleModel> { new LocaleModel() });

            //When
            listener.HandleMessage(mockMessage);

            //Then
            mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<DvsMessage>()), Times.Once);
            mockAddOrUpdateLocalesCommandHandler.Verify(m => m.Execute(It.IsAny<AddOrUpdateLocalesCommand>()), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_ParseNotSuccessful_ShouldLogAndNotify()
        {
            //Given
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<DvsMessage>())).Throws(new Exception("Test Exception"));

            try
            {
                //When
                listener.HandleMessage(mockMessage);
            }
            catch (Exception ex)
            {
                //Then
                Assert.AreEqual(ex.Message, "Test Exception");
                mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<DvsMessage>()), Times.Once);
                mockAddOrUpdateLocalesCommandHandler.Verify(m => m.Execute(It.IsAny<AddOrUpdateLocalesCommand>()), Times.Never);
            }
        }

        [TestMethod]
        public void HandleMessage_UpdateNotSuccessful_ShouldLogAndNotify()
        {
            //Given
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<DvsMessage>())).Returns(new List<LocaleModel> { new LocaleModel() });
            mockAddOrUpdateLocalesCommandHandler.Setup(m => m.Execute(It.IsAny<AddOrUpdateLocalesCommand>())).Throws(new Exception("Test Exception"));

            try
            {
                //When
                listener.HandleMessage(mockMessage);
            }
            catch (Exception ex)
            {
                //Then
                Assert.AreEqual(ex.Message, "Test Exception");
                mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<DvsMessage>()), Times.Once);
                mockAddOrUpdateLocalesCommandHandler.Verify(m => m.Execute(It.IsAny<AddOrUpdateLocalesCommand>()), Times.Once);
            }
        }
    }
}
