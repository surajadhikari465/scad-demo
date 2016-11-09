using Icon.Common.Email;
using Icon.Infor.Listeners.Item.Models;
using Icon.Infor.Listeners.Item.Notifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.Item.Constants;

namespace Icon.Infor.Listeners.Item.Tests.Notifiers
{
    [TestClass]
    public class ItemListenerNotifierTests
    {
        private ItemListenerNotifier notifier;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IEsbMessage> mockMessage;

        [TestInitialize]
        public void Initialize()
        {
            mockEmailClient = new Mock<IEmailClient>();
            notifier = new ItemListenerNotifier(mockEmailClient.Object);

            mockMessage = new Mock<IEsbMessage>();
            mockMessage.Setup(m => m.GetProperty("IconMessageID")).Returns("123");
        }

        [TestMethod]
        public void NotifyOfItemError_NoItems_ShouldNotNotify()
        {
            //Given
            List<ItemModel> itemModels = new List<ItemModel>();

            //When
            notifier.NotifyOfItemError(mockMessage.Object, itemModels);

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void NotifyOfItemError_ItemsHaveErrors_ShouldNotify()
        {
            //Given
            List<ItemModel> itemModels = new List<ItemModel> { new ItemModel() };

            //When
            notifier.NotifyOfItemError(mockMessage.Object, itemModels);

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
