using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Infor.Listeners.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Esb.Subscriber;
using Moq;
using Icon.Esb.MessageParsers;
using System.Collections;
using Icon.Infor.Listeners.Item.Models;
using Icon.Infor.Listeners.Item.Services;
using Icon.Esb;
using Icon.Common.Email;
using Icon.Esb.ListenerApplication;
using Icon.Logging;
using TIBCO.EMS;
using Icon.Infor.Listeners.Item.Validators;
using Icon.Common.Context;
using Icon.Framework;
using Icon.Infor.Listeners.Item.Notifiers;

namespace Icon.Infor.Listeners.Item.Tests
{
    [TestClass()]
    public class ItemListenerTests
    {
        private ItemListener itemListener;
        private Mock<IMessageParser<IEnumerable<ItemModel>>> mockMessageParser;
        private Mock<ICollectionValidator<ItemModel>> mockItemModelValidator;
        private Mock<IRenewableContext<IconContext>> mockContext;
        private Mock<IItemService> mockService;
        private ListenerApplicationSettings listenerApplicationSettings;
        private EsbConnectionSettings esbConnectionSettings;
        private Mock<IEsbSubscriber> mockSubscriber;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<ILogger<ItemListener>> mockLogger;
        private Mock<IItemListenerNotifier> mockNotifier;

        private EsbMessageEventArgs args;
        private Mock<IEsbMessage> mockEsbMessage;

        [TestInitialize]
        public void Initialize()
        {
            mockMessageParser = new Mock<IMessageParser<IEnumerable<ItemModel>>>();
            mockService = new Mock<IItemService>();
            listenerApplicationSettings = new ListenerApplicationSettings();
            esbConnectionSettings = new EsbConnectionSettings();
            mockSubscriber = new Mock<IEsbSubscriber>();
            mockEmailClient = new Mock<IEmailClient>();
            mockLogger = new Mock<ILogger<ItemListener>>();
            mockNotifier = new Mock<IItemListenerNotifier>();
            mockItemModelValidator = new Mock<ICollectionValidator<ItemModel>>();
            mockContext = new Mock<IRenewableContext<IconContext>>();

            itemListener = new ItemListener(
                this.mockMessageParser.Object,
                this.mockItemModelValidator.Object,
                this.mockContext.Object,
                this.mockService.Object,
                this.listenerApplicationSettings,
                this.esbConnectionSettings,
                this.mockSubscriber.Object,
                this.mockEmailClient.Object,
                this.mockNotifier.Object,
                this.mockLogger.Object);

            args = new EsbMessageEventArgs();
            mockEsbMessage = new Mock<IEsbMessage>();

            args.Message = mockEsbMessage.Object;
            esbConnectionSettings.SessionMode = SessionMode.ClientAcknowledge;
        }

        [TestMethod()]
        public void HandleMessage_ModelsDontExist_ShouldNotCallAddOrUpdateItemsAndAcknowledgeMsssage()
        {
            //When
            itemListener.HandleMessage(null, args);

            //Then
            mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
            mockService.Verify(m => m.AddOrUpdateItems(It.IsAny<IEnumerable<ItemModel>>()), Times.Never);
            mockEsbMessage.Verify(m => m.Acknowledge(), Times.Once);
        }

        [TestMethod()]
        public void HandleMessage_ModelsDoExist_ShouldCallAddOrUpdateItemsOnceAndAcknowledgeMsssage()
        {
            //Given
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>()))
                .Returns(new List<ItemModel> { new ItemModel() });

            //When
            itemListener.HandleMessage(null, args);

            //Then
            mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
            mockService.Verify(m => m.AddOrUpdateItems(It.IsAny<IEnumerable<ItemModel>>()), Times.Once);
            mockEsbMessage.Verify(m => m.Acknowledge(), Times.Once);
        }

        [TestMethod()]
        public void HandleMessage_ExceptionOccurs_ShouldLogAndNotifyErrorsAndAcknowledgeMsssage()
        {
            //Given
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>()))
                .Throws(new Exception());

            //When
            itemListener.HandleMessage(null, args);

            //Then
            mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
            mockService.Verify(m => m.AddOrUpdateItems(It.IsAny<IEnumerable<ItemModel>>()), Times.Never);
            mockEsbMessage.Verify(m => m.Acknowledge(), Times.Once);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}