using Icon.Esb.Factory;
using Icon.Esb.Producer;
using Icon.Framework;
using Icon.Logging;
using Infor.Services.NewItem.MessageBuilders;
using Infor.Services.NewItem.Models;
using Infor.Services.NewItem.Services;
using Infor.Services.NewItem.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infor.Services.NewItem.Tests.Services
{
    [TestClass]
    public class InforItemServiceTests
    {
        private InforItemService service;
        private AddNewItemsToInforRequest request;
        private Mock<IEsbConnectionFactory> mockEsbConnectionFactory;
        private Mock<IEsbProducer> mockProducer;
        private Mock<IMessageBuilder<IEnumerable<NewItemModel>>> mockMessageBuilder;
        private IconContext context;
        private DbContextTransaction transaction;
        private Mock<ILogger<InforItemService>> mockLogger;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            transaction = context.Database.BeginTransaction();

            mockEsbConnectionFactory = new Mock<IEsbConnectionFactory>();
            mockMessageBuilder = new Mock<IMessageBuilder<IEnumerable<NewItemModel>>>();
            mockLogger = new Mock<ILogger<InforItemService>>();

            service = new InforItemService(mockEsbConnectionFactory.Object, mockMessageBuilder.Object, context, mockLogger.Object);
            request = new AddNewItemsToInforRequest();

            mockProducer = new Mock<IEsbProducer>();
            mockEsbConnectionFactory.Setup(m => m.CreateProducer(true))
                .Returns(mockProducer.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (transaction.UnderlyingTransaction.Connection != null)
            {
                transaction.Rollback();
                transaction.Dispose();
            }
            context.Dispose();
        }

        [TestMethod]
        public void AddNewItemsToInfor_GivenAnEmptyListOfNewItems_ShouldNotSendItemsToInforAndResponseErrorOccuredIsFalse()
        {
            //Given
            request.NewItems = new List<NewItemModel>();

            //When
            var response = service.AddNewItemsToInfor(request);

            //Then
            Assert.AreEqual(false, response.ErrorOccurred);
            mockMessageBuilder.Verify(m => m.BuildMessage(It.IsAny<IEnumerable<NewItemModel>>()), Times.Never);
            mockProducer.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
        }

        [TestMethod]
        public void AddNewItemsToInfor_GivenNewItems_ShouldSendItemsToInforAndResponseErrorOccuredIsFalse()
        {
            //Given
            request.NewItems = new List<NewItemModel> { new NewItemModel() };
            mockMessageBuilder.Setup(m => m.BuildMessage(It.IsAny<IEnumerable<NewItemModel>>()))
                .Returns("<Test>test</Test>");

            //When
            var response = service.AddNewItemsToInfor(request);

            //Then
            Assert.AreEqual(false, response.ErrorOccurred);
            mockMessageBuilder.Verify(m => m.BuildMessage(It.IsAny<IEnumerable<NewItemModel>>()), Times.Once);
            mockProducer.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);

            var messageHistory = context.MessageHistory.AsNoTracking().Single(mh => mh.MessageHistoryId == response.MessageHistoryId);
            Assert.AreEqual(MessageTypes.InforNewItem, messageHistory.MessageTypeId);
            Assert.AreEqual(MessageStatusTypes.Sent, messageHistory.MessageStatusId);
            Assert.AreEqual("<Test>test</Test>", messageHistory.Message);
            Assert.IsTrue(DateTime.Today < messageHistory.InsertDate);
            Assert.IsTrue(DateTime.Today < messageHistory.ProcessedDate);
            Assert.IsTrue(request.NewItems.All(i => i.MessageSentToInfor));
        }

        [TestMethod]
        public void AddNewItemsToInfor_SendingItemsToInforThrowsAnException_ShouldMarkMessageHistoryAsFailedAndResponseErrorOccurredIsTrue()
        {
            //Given
            request.NewItems = new List<NewItemModel> { new NewItemModel() };
            mockMessageBuilder.Setup(m => m.BuildMessage(It.IsAny<IEnumerable<NewItemModel>>()))
                .Returns("<Test>test</Test>");
            mockProducer.Setup(m => m.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Throws(new Exception("Test"));

            //When
            var response = service.AddNewItemsToInfor(request);

            //Then
            Assert.AreEqual(true, response.ErrorOccurred);
            mockMessageBuilder.Verify(m => m.BuildMessage(It.IsAny<IEnumerable<NewItemModel>>()), Times.Once);
            mockProducer.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);

            var messageHistory = context.MessageHistory.AsNoTracking().Single(mh => mh.MessageHistoryId == response.MessageHistoryId);
            Assert.AreEqual(MessageTypes.InforNewItem, messageHistory.MessageTypeId);
            Assert.AreEqual(MessageStatusTypes.Failed, messageHistory.MessageStatusId);
            Assert.AreEqual("<Test>test</Test>", messageHistory.Message);
        }

        [TestMethod]
        public void AddNewItemsToInfor_BuildingMessageThrowsAnException_ShouldNotSendItemRequestToInforAndResponseErrorOccurredIsTrue()
        {
            //Given
            request.NewItems = new List<NewItemModel> { new NewItemModel() };
            mockMessageBuilder.Setup(m => m.BuildMessage(It.IsAny<IEnumerable<NewItemModel>>()))
                .Throws(new Exception("Test"));

            //When
            var response = service.AddNewItemsToInfor(request);

            //Then
            Assert.AreEqual(true, response.ErrorOccurred);
            mockMessageBuilder.Verify(m => m.BuildMessage(It.IsAny<IEnumerable<NewItemModel>>()), Times.Once);
            mockProducer.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);

            var messageHistory = context.MessageHistory.AsNoTracking().SingleOrDefault(mh => mh.MessageHistoryId == response.MessageHistoryId);
            Assert.IsNull(messageHistory);
        }

        [TestMethod]
        public void AddNewItemsToInfor_SavingMessageHistoryToIconThrowsAnException_ResponseErrorOccurredIsTrue()
        {
            //Given
            request.NewItems = new List<NewItemModel> { new NewItemModel() };
            mockMessageBuilder.Setup(m => m.BuildMessage(It.IsAny<IEnumerable<NewItemModel>>()))
                .Returns("<<<Not valid XML. Should create an exception.");

            //When
            var response = service.AddNewItemsToInfor(request);

            //Then
            Assert.AreEqual(true, response.ErrorOccurred);
            mockMessageBuilder.Verify(m => m.BuildMessage(It.IsAny<IEnumerable<NewItemModel>>()), Times.Once);
            mockProducer.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);

            Assert.AreEqual(0, response.MessageHistoryId);
        }
    }
}
