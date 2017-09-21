using Esb.Core.EsbServices;
using Icon.Common.Email;
using Icon.Esb.Services.ConfirmationBod;
using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.Item.Constants;
using Icon.Infor.Listeners.Item.Models;
using Icon.Infor.Listeners.Item.Notifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace Icon.Infor.Listeners.Item.Tests.Notifiers
{
    [TestClass]
    public class ItemListenerNotifierTests
    {
        private ItemListenerNotifier notifier;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IEsbMessage> mockMessage;
        private ItemListenerSettings settings;
        private Mock<IEsbService<ConfirmationBodEsbRequest>> mockConfirmBodRequest;

        [TestInitialize]
        public void Initialize()
        {
            mockEmailClient = new Mock<IEmailClient>();
            settings = new ItemListenerSettings();
            mockConfirmBodRequest = new Mock<IEsbService<ConfirmationBodEsbRequest>>();
            notifier = new ItemListenerNotifier(mockEmailClient.Object, settings, mockConfirmBodRequest.Object);

            mockMessage = new Mock<IEsbMessage>();
            mockMessage.Setup(m => m.GetProperty("IconMessageID")).Returns("123");
        }

        [TestMethod]
        public void NotifyOfItemError_NoItems_ShouldNotNotify()
        {
            //Given
            List<ItemModel> itemModels = new List<ItemModel>();

            //When
            notifier.NotifyOfItemError(mockMessage.Object, false, itemModels);

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            mockConfirmBodRequest.Verify(m => m.Send(It.IsAny<ConfirmationBodEsbRequest>()), Times.Never);
        }

        [TestMethod]
        public void NotifyOfItemError_ItemsHaveErrors_ShouldNotify()
        {
            //Given
            List<ItemModel> itemModels = new List<ItemModel> { new ItemModel() };

            //When
            notifier.NotifyOfItemError(mockMessage.Object, false, itemModels);

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockConfirmBodRequest.Verify(m => m.Send(It.IsAny<ConfirmationBodEsbRequest>()), Times.Never);
        }

        [TestMethod]
        public void NotifyOfItemError_EnableConfirmBodIsTrueAndSchemaErrorIsTrue_ShouldSendSchemaError()
        {
            //Given
            settings.EnableConfirmBods = true;
            List<ItemModel> itemModels = new List<ItemModel> { new ItemModel() };

            //When
            notifier.NotifyOfItemError(mockMessage.Object, true, itemModels);

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockConfirmBodRequest.Verify(
                m => m.Send(It.Is<ConfirmationBodEsbRequest>(
                    r => r.ErrorReasonCode == ApplicationErrors.Codes.UnableToParseMessage 
                        && r.ErrorDescription == ApplicationErrors.Messages.UnableToParseMessage
                        && r.ErrorType == ConfirmationBodEsbErrorTypes.Schema)), 
                Times.Once);
        }

        [TestMethod]
        public void NotifyOfItemError_EnableConfirmBodIsTrueAndSchemaErrorIsFalse_ShouldSendItemError()
        {
            //Given
            settings.EnableConfirmBods = true;
            ItemModel model = new ItemModel
            {
                ErrorCode = "Test Error Code",
                ErrorDetails = "Test Error Details"
            };
            List<ItemModel> itemModels = new List<ItemModel> { model };

            //When
            notifier.NotifyOfItemError(mockMessage.Object, false, itemModels);

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockConfirmBodRequest.Verify(
                m => m.Send(It.Is<ConfirmationBodEsbRequest>(
                    r => r.ErrorReasonCode == model.ErrorCode
                        && r.ErrorDescription == model.ErrorDetails
                        && r.ErrorType == ConfirmationBodEsbErrorTypes.Data)),
                Times.Once);
        }

        [TestMethod]
        public void NotifyOfItemError_EnableConfirmBodIsTrueAndItemAddOrUpdateError_ShouldSendDatabaseConstraintError()
        {
            //Given
            settings.EnableConfirmBods = true;
            ItemModel model = new ItemModel
            {
                ErrorCode = ApplicationErrors.Codes.ItemAddOrUpdateError,
                ErrorDetails = ApplicationErrors.Messages.ItemAddOrUpdateError
            };
            List<ItemModel> itemModels = new List<ItemModel> { model };

            //When
            notifier.NotifyOfItemError(mockMessage.Object, false, itemModels);

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockConfirmBodRequest.Verify(
                m => m.Send(It.Is<ConfirmationBodEsbRequest>(
                    r => r.ErrorReasonCode == model.ErrorCode
                        && r.ErrorDescription == model.ErrorDetails
                        && r.ErrorType == ConfirmationBodEsbErrorTypes.DatabaseConstraint)),
                Times.Once);
        }

        [TestMethod]
        public void NotifyOfItemError_EnableConfirmBodIsTrueAndGenerateItemMessagesError_ShouldSendDatabaseConstraintError()
        {
            //Given
            settings.EnableConfirmBods = true;
            ItemModel model = new ItemModel
            {
                ErrorCode = ApplicationErrors.Codes.GenerateItemMessagesError,
                ErrorDetails = ApplicationErrors.Codes.GenerateItemMessagesError
            };
            List<ItemModel> itemModels = new List<ItemModel> { model };

            //When
            notifier.NotifyOfItemError(mockMessage.Object, false, itemModels);

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockConfirmBodRequest.Verify(
                m => m.Send(It.Is<ConfirmationBodEsbRequest>(
                    r => r.ErrorReasonCode == model.ErrorCode
                        && r.ErrorDescription == model.ErrorDetails
                        && r.ErrorType == ConfirmationBodEsbErrorTypes.DatabaseConstraint)),
                Times.Once);
        }
    }
}
