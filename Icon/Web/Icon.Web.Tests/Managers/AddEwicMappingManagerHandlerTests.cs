using Icon.Common.DataAccess;
using Icon.Ewic.Models;
using Icon.Ewic.Serialization.Serializers;
using Icon.Ewic.Transmission.Producers;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.Common.Validators;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Icon.Web.Tests.Unit.Managers
{
    [TestClass]
    public class AddEwicMappingManagerHandlerTests
    {
        private AddEwicMappingManagerHandler managerHandler;
        private Mock<IObjectValidator<AddEwicMappingManager>> mockValidator;
        private Mock<ISerializer<EwicMappingMessageModel>> mockSerializer;
        private Mock<IQueryHandler<GetEwicAgenciesWithoutMappingParameters, List<Agency>>> mockGetEwicAgenciesWithoutMappingQuery;
        private Mock<ICommandHandler<AddEwicMappingCommand>> mockAddEwicMappingCommandHandler;
        private Mock<ICommandHandler<SaveToMessageHistoryCommand>> mockSaveToMessageHistoryCommandHandler;
        private Mock<ICommandHandler<UpdateMessageHistoryMessageCommand>> mockUpdateMessageHistoryMessageCommandHandler;
        private Mock<IMessageProducer> mockProducer;
        private string testAplScanCode;
        private string testWfmScanCode;

        [TestInitialize]
        public void Initialize()
        {
            testAplScanCode = "22222222";
            testWfmScanCode = "222222222";

            mockValidator = new Mock<IObjectValidator<AddEwicMappingManager>>();
            mockSerializer = new Mock<ISerializer<EwicMappingMessageModel>>();
            mockGetEwicAgenciesWithoutMappingQuery = new Mock<IQueryHandler<GetEwicAgenciesWithoutMappingParameters, List<Agency>>>();
            mockAddEwicMappingCommandHandler = new Mock<ICommandHandler<AddEwicMappingCommand>>();
            mockSaveToMessageHistoryCommandHandler = new Mock<ICommandHandler<SaveToMessageHistoryCommand>>();
            mockUpdateMessageHistoryMessageCommandHandler = new Mock<ICommandHandler<UpdateMessageHistoryMessageCommand>>();
            mockProducer = new Mock<IMessageProducer>();

            managerHandler = new AddEwicMappingManagerHandler(
                mockValidator.Object,
                mockSerializer.Object,
                mockGetEwicAgenciesWithoutMappingQuery.Object,
                mockAddEwicMappingCommandHandler.Object,
                mockSaveToMessageHistoryCommandHandler.Object,
                mockUpdateMessageHistoryMessageCommandHandler.Object,
                mockProducer.Object);

            mockValidator.Setup(v => v.Validate(It.IsAny<AddEwicMappingManager>())).Returns(new ObjectValidationResult { IsValid = true });
            mockGetEwicAgenciesWithoutMappingQuery.Setup(q => q.Search(It.IsAny<GetEwicAgenciesWithoutMappingParameters>())).Returns(new List<Agency> { new Agency() });

            var mockXml = new XDocument(new XElement("body"));
            mockSerializer.Setup(s => s.Serialize(It.IsAny<EwicMappingMessageModel>())).Returns(mockXml.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddEwicMapping_DataIsInvalid_ExceptionShouldBeThrown()
        {
            // Given.
            mockValidator.Setup(v => v.Validate(It.IsAny<AddEwicMappingManager>())).Returns(new ObjectValidationResult { IsValid = false });

            var manager = new AddEwicMappingManager
            {
                AplScanCode = testAplScanCode,
                WfmScanCode = testWfmScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        public void AddEwicMapping_DataIsValid_AgenciesWithoutTheMappingShouldBeQueried()
        {
            // Given.
            var manager = new AddEwicMappingManager
            {
                AplScanCode = testAplScanCode,
                WfmScanCode = testWfmScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockGetEwicAgenciesWithoutMappingQuery.Verify(q => q.Search(It.IsAny<GetEwicAgenciesWithoutMappingParameters>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddEwicMapping_NoAgenciesNeedTheMapping_ExceptionShouldBeThrown()
        {
            // Given.
            mockGetEwicAgenciesWithoutMappingQuery.Setup(q => q.Search(It.IsAny<GetEwicAgenciesWithoutMappingParameters>())).Returns(new List<Agency>());

            var manager = new AddEwicMappingManager
            {
                AplScanCode = testAplScanCode,
                WfmScanCode = testWfmScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(CommandException))]
        public void AddEwicMapping_AddEwicMappingCommandThrowsException_SerializerShouldNotBeCalled()
        {
            // Given.
            mockAddEwicMappingCommandHandler.Setup(c => c.Execute(It.IsAny<AddEwicMappingCommand>())).Throws(new Exception());

            var manager = new AddEwicMappingManager
            {
                AplScanCode = testAplScanCode,
                WfmScanCode = testWfmScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockAddEwicMappingCommandHandler.Verify(c => c.Execute(It.IsAny<AddEwicMappingCommand>()), Times.Once);
            mockSerializer.Verify(s => s.Serialize(It.IsAny<EwicMappingMessageModel>()), Times.Never);
        }

        [TestMethod]
        public void AddEwicMapping_AddEwicExceptionCommandIsSuccessful_MessagesShouldBeGenerated()
        {
            // Given.
            var manager = new AddEwicMappingManager
            {
                AplScanCode = testAplScanCode,
                WfmScanCode = testWfmScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockAddEwicMappingCommandHandler.Verify(c => c.Execute(It.IsAny<AddEwicMappingCommand>()), Times.Once);
            mockSaveToMessageHistoryCommandHandler.Verify(c => c.Execute(It.IsAny<SaveToMessageHistoryCommand>()), Times.Once);
        }

        [TestMethod]
        public void AddEwicMapping_MessageGenerationIsSuccessful_MessagesShouldBeSerialized()
        {
            // Given.
            var manager = new AddEwicMappingManager
            {
                AplScanCode = testAplScanCode,
                WfmScanCode = testWfmScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockAddEwicMappingCommandHandler.Verify(c => c.Execute(It.IsAny<AddEwicMappingCommand>()), Times.Once);
            mockSaveToMessageHistoryCommandHandler.Verify(c => c.Execute(It.IsAny<SaveToMessageHistoryCommand>()), Times.Once);
            mockSerializer.Verify(s => s.Serialize(It.Is<EwicMappingMessageModel>(m => m.ActionTypeId == MessageActionTypes.AddOrUpdate)), Times.Once);
        }

        [TestMethod]
        public void AddEwicMapping_SerializationIsSuccessful_MessageXmlShouldBePopulated()
        {
            // Given.
            var manager = new AddEwicMappingManager
            {
                AplScanCode = testAplScanCode,
                WfmScanCode = testWfmScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockAddEwicMappingCommandHandler.Verify(c => c.Execute(It.IsAny<AddEwicMappingCommand>()), Times.Once);
            mockSaveToMessageHistoryCommandHandler.Verify(c => c.Execute(It.IsAny<SaveToMessageHistoryCommand>()), Times.Once);
            mockSerializer.Verify(s => s.Serialize(It.Is<EwicMappingMessageModel>(m => m.ActionTypeId == MessageActionTypes.AddOrUpdate)), Times.Once);
            mockUpdateMessageHistoryMessageCommandHandler.Verify(c => c.Execute(It.IsAny<UpdateMessageHistoryMessageCommand>()), Times.Once);
        }

        [TestMethod]
        public void AddEwicMapping_MessageXmlIsPopulated_MessageShouldBeSent()
        {
            // Given.
            var manager = new AddEwicMappingManager
            {
                AplScanCode = testAplScanCode,
                WfmScanCode = testWfmScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockAddEwicMappingCommandHandler.Verify(c => c.Execute(It.IsAny<AddEwicMappingCommand>()), Times.Once);
            mockSaveToMessageHistoryCommandHandler.Verify(c => c.Execute(It.IsAny<SaveToMessageHistoryCommand>()), Times.Once);
            mockSerializer.Verify(s => s.Serialize(It.Is<EwicMappingMessageModel>(m => m.ActionTypeId == MessageActionTypes.AddOrUpdate)), Times.Once);
            mockUpdateMessageHistoryMessageCommandHandler.Verify(c => c.Execute(It.IsAny<UpdateMessageHistoryMessageCommand>()), Times.Once);
            mockProducer.Verify(p => p.SendMessages(It.IsAny<List<MessageHistory>>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(CommandException))]
        public void AddEwicMapping_SendMessageCommandThrowsException_ExceptionShouldBeThrownToCaller()
        {
            // Given.
            mockProducer.Setup(p => p.SendMessages(It.IsAny<List<MessageHistory>>())).Throws(new Exception());

            var manager = new AddEwicMappingManager
            {
                AplScanCode = testAplScanCode,
                WfmScanCode = testWfmScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockAddEwicMappingCommandHandler.Verify(c => c.Execute(It.IsAny<AddEwicMappingCommand>()), Times.Once);
            mockSerializer.Verify(s => s.Serialize(It.Is<EwicMappingMessageModel>(m => m.ActionTypeId == MessageActionTypes.AddOrUpdate)), Times.Once);
            mockProducer.Verify(p => p.SendMessages(It.IsAny<List<MessageHistory>>()), Times.Once);
        }

        [TestMethod]
        public void AddEwicMapping_SendMessageCommandIsSuccessful_MessageShouldBeSavedToMessageHistory()
        {
            // Given.
            var manager = new AddEwicMappingManager
            {
                AplScanCode = testAplScanCode,
                WfmScanCode = testWfmScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockAddEwicMappingCommandHandler.Verify(c => c.Execute(It.IsAny<AddEwicMappingCommand>()), Times.Once);
            mockSerializer.Verify(s => s.Serialize(It.Is<EwicMappingMessageModel>(m => m.ActionTypeId == MessageActionTypes.AddOrUpdate)), Times.Once);
            mockProducer.Verify(p => p.SendMessages(It.IsAny<List<MessageHistory>>()), Times.Once);
            mockSaveToMessageHistoryCommandHandler.Verify(c => c.Execute(It.Is<SaveToMessageHistoryCommand>(h =>
                h.Messages.TrueForAll(m =>
                    m.MessageTypeId == MessageTypes.Ewic &&
                    m.MessageStatusId == MessageStatusTypes.Sent)
                )), Times.Once);
        }
    }
}
