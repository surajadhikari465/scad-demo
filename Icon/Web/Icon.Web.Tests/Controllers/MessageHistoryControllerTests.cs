using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Controllers;
using Icon.Web.Mvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Icon.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class MessageHistoryControllerTests
    {
        private MessageHistoryController controller;
        private Mock<IQueryHandler<GetFailedMessagesParameters, List<MessageModel>>> mockGetFailedMessagesQueryHandler;
        private Mock<IQueryHandler<GetMessageHistoryParameters, List<MessageHistory>>> mockGetMessageHistoryXmlQueryHandler;
        private Mock<ICommandHandler<ReprocessFailedMessagesCommand>> mockReprocessFailedMessagesCommandHandler;
        private Mock<ICommandHandler<UpdateMessageHistoryStatusCommand>> mockUpdateMessageHistoryStatusCommandHandler;

        [TestInitialize]
        public void Initialize()
        {
            mockGetFailedMessagesQueryHandler = new Mock<IQueryHandler<GetFailedMessagesParameters, List<MessageModel>>>();
            mockGetMessageHistoryXmlQueryHandler = new Mock<IQueryHandler<GetMessageHistoryParameters, List<MessageHistory>>>();
            mockReprocessFailedMessagesCommandHandler = new Mock<ICommandHandler<ReprocessFailedMessagesCommand>>();
            mockUpdateMessageHistoryStatusCommandHandler = new Mock<ICommandHandler<UpdateMessageHistoryStatusCommand>>();

            controller = new MessageHistoryController(
                mockGetFailedMessagesQueryHandler.Object,
                mockGetMessageHistoryXmlQueryHandler.Object,
                mockReprocessFailedMessagesCommandHandler.Object,
                mockUpdateMessageHistoryStatusCommandHandler.Object);
        }

        [TestMethod]
        public void Index_NoFailedEntries_ViewModelShouldContainEmtpyList()
        {
            // Given.
            var returnedMessages = new List<MessageModel>();

            mockGetFailedMessagesQueryHandler.Setup(q => q.Search(It.IsAny<GetFailedMessagesParameters>())).Returns(returnedMessages);

            // When.
            var result = controller.Index() as ViewResult;

            // Then.
            var viewModel = result.Model as List<FailedMessageViewModel>;

            Assert.AreEqual(returnedMessages.Count, viewModel.Count);
        }

        [TestMethod]
        public void Index_TwoFailedEntries_ViewModelShouldContainTwoMessages()
        {
            // Given.
            var returnedMessages = new List<MessageModel>
            {
                new MessageModel { MessageTypeId = 1 },
                new MessageModel { MessageTypeId = 1 }
            };

            mockGetFailedMessagesQueryHandler.Setup(q => q.Search(It.IsAny<GetFailedMessagesParameters>())).Returns(returnedMessages);

            // When.
            var result = controller.Index() as ViewResult;

            // Then.
            var viewModel = result.Model as List<FailedMessageViewModel>;

            Assert.AreEqual(returnedMessages.Count, viewModel.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Index_UnknownMessageType_ExceptionShouldBeThrown()
        {
            // Given.
            var returnedMessages = new List<MessageModel>
            {
                new MessageModel { MessageTypeId = 0 },
                new MessageModel { MessageTypeId = 0 }
            };

            mockGetFailedMessagesQueryHandler.Setup(q => q.Search(It.IsAny<GetFailedMessagesParameters>())).Returns(returnedMessages);

            // When.
            var result = controller.Index() as ViewResult;

            // Then.
            // Expected exception.
        }

        [TestMethod]
        public void Reprocess_PostContainsEwicMessages_ProcessEwicMessagesShouldBeCalled()
        {
            // Given.
            var viewModels = new List<FailedMessageViewModel>
            {
                new FailedMessageViewModel { MessageTypeId = MessageTypes.Ewic },
                new FailedMessageViewModel { MessageTypeId = MessageTypes.Ewic }
            };

            // When.
            controller.Reprocess(viewModels);

            // Then.
            mockGetMessageHistoryXmlQueryHandler.Verify(q => q.Search(It.IsAny<GetMessageHistoryParameters>()), Times.Once);
            mockReprocessFailedMessagesCommandHandler.Verify(c => c.Execute(It.IsAny<ReprocessFailedMessagesCommand>()), Times.Never);
        }

        [TestMethod]
        public void Reprocess_PostContainsNonEwicMessages_ReprocessMessagesCommandShouldBeCalled()
        {
            // Given.
            var viewModels = new List<FailedMessageViewModel>
            {
                new FailedMessageViewModel { MessageTypeId = MessageTypes.Price },
                new FailedMessageViewModel { MessageTypeId = MessageTypes.Product }
            };

            // When.
            controller.Reprocess(viewModels);

            // Then.
            mockGetMessageHistoryXmlQueryHandler.Verify(q => q.Search(It.IsAny<GetMessageHistoryParameters>()), Times.Never);
            mockReprocessFailedMessagesCommandHandler.Verify(c => c.Execute(It.IsAny<ReprocessFailedMessagesCommand>()), Times.Once);
        }

        [TestMethod]
        public void Reprocess_PostContainsEwicAndNonEwicMessages_BothTypesOfMessagesShouldBeReprocessed()
        {
            // Given.
            var viewModels = new List<FailedMessageViewModel>
            {
                new FailedMessageViewModel { MessageTypeId = MessageTypes.Price },
                new FailedMessageViewModel { MessageTypeId = MessageTypes.Product },
                new FailedMessageViewModel { MessageTypeId = MessageTypes.Ewic },
                new FailedMessageViewModel { MessageTypeId = MessageTypes.Ewic }
            };

            // When.
            controller.Reprocess(viewModels);

            // Then.
            mockGetMessageHistoryXmlQueryHandler.Verify(q => q.Search(It.IsAny<GetMessageHistoryParameters>()), Times.Once);
            mockReprocessFailedMessagesCommandHandler.Verify(c => c.Execute(It.IsAny<ReprocessFailedMessagesCommand>()), Times.Once);
        }

        [TestMethod]
        public void Reprocess_GetMessageHistoryQueryThrowsException_ErrorJsonShouldBeReturned()
        {
            // Given.
            mockGetMessageHistoryXmlQueryHandler.Setup(q => q.Search(It.IsAny<GetMessageHistoryParameters>())).Throws(new Exception());

            var viewModels = new List<FailedMessageViewModel>();

            // When.
            var json = controller.Reprocess(viewModels) as JsonResult;

            // Then.
            var success = json.Data.GetType().GetProperties().Single(p => p.Name == "Success").GetValue(json.Data);
            var error = json.Data.GetType().GetProperties().Single(p => p.Name == "Error").GetValue(json.Data);

            Assert.AreEqual(success, false);
            Assert.IsFalse(String.IsNullOrEmpty((string)error));
        }

        [TestMethod]
        public void Reprocess_SendEwicMessagesQueryThrowsException_ErrorJsonShouldBeReturned()
        {
            // Given.

            var viewModels = new List<FailedMessageViewModel>();

            // When.
            var json = controller.Reprocess(viewModels) as JsonResult;

            // Then.
            var success = json.Data.GetType().GetProperties().Single(p => p.Name == "Success").GetValue(json.Data);
            var error = json.Data.GetType().GetProperties().Single(p => p.Name == "Error").GetValue(json.Data);

            Assert.AreEqual(success, false);
            Assert.IsFalse(String.IsNullOrEmpty((string)error));
        }

        [TestMethod]
        public void Reprocess_MessageReprocessCommandThrowsException_ErrorJsonShouldBeReturned()
        {
            // Given.
            mockReprocessFailedMessagesCommandHandler.Setup(q => q.Execute(It.IsAny<ReprocessFailedMessagesCommand>())).Throws(new Exception());

            var viewModels = new List<FailedMessageViewModel>();

            // When.
            var json = controller.Reprocess(viewModels) as JsonResult;

            // Then.
            var success = json.Data.GetType().GetProperties().Single(p => p.Name == "Success").GetValue(json.Data);
            var error = json.Data.GetType().GetProperties().Single(p => p.Name == "Error").GetValue(json.Data);

            Assert.AreEqual(success, false);
            Assert.IsFalse(String.IsNullOrEmpty((string)error));
        }
    }
}
