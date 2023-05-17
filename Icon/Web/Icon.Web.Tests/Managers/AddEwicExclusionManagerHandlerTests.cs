using Icon.Common.DataAccess;
using Icon.Ewic.Models;
using Icon.Ewic.Serialization.Serializers;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Icon.Common.Validators;

namespace Icon.Web.Tests.Unit.Managers
{
    [TestClass]
    public class AddEwicExclusionManagerHandlerTests
    {
        private AddEwicExclusionManagerHandler managerHandler;
        private Mock<IObjectValidator<AddEwicExclusionManager>> mockValidator;
        private Mock<ISerializer<EwicExclusionMessageModel>> mockSerializer;
        private Mock<IQueryHandler<GetEwicAgenciesWithoutExclusionParameters, List<Agency>>> mockGetEwicAgenciesWithoutExclusionQuery;
        private Mock<ICommandHandler<AddEwicExclusionCommand>> mockAddEwicExclusionCommandHandler;
        private Mock<ICommandHandler<SaveToMessageHistoryCommand>> mockSaveToMessageHistoryCommandHandler;
        private Mock<ICommandHandler<UpdateMessageHistoryMessageCommand>> mockUpdateMessageHistoryMessageCommandHandler;
        private string testScanCode;

        [TestInitialize]
        public void Initialize()
        {
            testScanCode = "22222222";

            mockValidator = new Mock<IObjectValidator<AddEwicExclusionManager>>();
            mockSerializer = new Mock<ISerializer<EwicExclusionMessageModel>>();
            mockGetEwicAgenciesWithoutExclusionQuery = new Mock<IQueryHandler<GetEwicAgenciesWithoutExclusionParameters, List<Agency>>>();
            mockAddEwicExclusionCommandHandler = new Mock<ICommandHandler<AddEwicExclusionCommand>>();
            mockSaveToMessageHistoryCommandHandler = new Mock<ICommandHandler<SaveToMessageHistoryCommand>>();
            mockUpdateMessageHistoryMessageCommandHandler = new Mock<ICommandHandler<UpdateMessageHistoryMessageCommand>>();

            managerHandler = new AddEwicExclusionManagerHandler(
                mockValidator.Object,
                mockSerializer.Object,
                mockGetEwicAgenciesWithoutExclusionQuery.Object,
                mockAddEwicExclusionCommandHandler.Object,
                mockSaveToMessageHistoryCommandHandler.Object,
                mockUpdateMessageHistoryMessageCommandHandler.Object);

            mockValidator.Setup(v => v.Validate(It.IsAny<AddEwicExclusionManager>())).Returns(new ObjectValidationResult { IsValid = true });
            mockGetEwicAgenciesWithoutExclusionQuery.Setup(q => q.Search(It.IsAny<GetEwicAgenciesWithoutExclusionParameters>())).Returns(new List<Agency> { new Agency() });

            var mockXml = new XDocument(new XElement("body"));
            mockSerializer.Setup(s => s.Serialize(It.IsAny<EwicExclusionMessageModel>())).Returns(mockXml.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddEwicExclusion_DataIsInvalid_ExceptionShouldBeThrown()
        {
            // Given.
            mockValidator.Setup(v => v.Validate(It.IsAny<AddEwicExclusionManager>())).Returns(new ObjectValidationResult { IsValid = false });

            var manager = new AddEwicExclusionManager
            {
                ScanCode = testScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        public void AddEwicExclusion_DataIsValid_AgenciesWithoutTheExclusionShouldBeQueried()
        {
            // Given.
            var manager = new AddEwicExclusionManager
            {
                ScanCode = testScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockGetEwicAgenciesWithoutExclusionQuery.Verify(q => q.Search(It.IsAny<GetEwicAgenciesWithoutExclusionParameters>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(CommandException))]
        public void AddEwicExclusion_ExclusionsQueryEncountersError_ExceptionShouldBeThrown()
        {
            // Given.
            mockGetEwicAgenciesWithoutExclusionQuery.Setup(q => q.Search(It.IsAny<GetEwicAgenciesWithoutExclusionParameters>())).Throws(new Exception());

            var manager = new AddEwicExclusionManager
            {
                ScanCode = testScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddEwicExclusion_NoAgenciesNeedTheExclusion_ExceptionShouldBeThrown()
        {
            // Given.
            mockGetEwicAgenciesWithoutExclusionQuery.Setup(q => q.Search(It.IsAny<GetEwicAgenciesWithoutExclusionParameters>())).Returns(new List<Agency>());

            var manager = new AddEwicExclusionManager
            {
                ScanCode = testScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(CommandException))]
        public void AddEwicExclusion_AddEwicExceptionCommandThrowsException_SerializerShouldNotBeCalled()
        {
            // Given.
            mockAddEwicExclusionCommandHandler.Setup(c => c.Execute(It.IsAny<AddEwicExclusionCommand>())).Throws(new Exception());

            var manager = new AddEwicExclusionManager
            {
                ScanCode = testScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockAddEwicExclusionCommandHandler.Verify(c => c.Execute(It.IsAny<AddEwicExclusionCommand>()), Times.Once);
            mockSerializer.Verify(s => s.Serialize(It.IsAny<EwicExclusionMessageModel>()), Times.Never);
        }

        [TestMethod]
        public void AddEwicExclusion_AddEwicExceptionCommandIsSuccessful_MessagesShouldBeGenerated()
        {
            // Given.
            var manager = new AddEwicExclusionManager
            {
                ScanCode = testScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockAddEwicExclusionCommandHandler.Verify(c => c.Execute(It.IsAny<AddEwicExclusionCommand>()), Times.Once);
            mockSaveToMessageHistoryCommandHandler.Verify(c => c.Execute(It.IsAny<SaveToMessageHistoryCommand>()), Times.Once);
        }

        [TestMethod]
        public void AddEwicExclusion_MessageGenerationIsSuccessful_MessagesShouldBeSerialized()
        {
            // Given.
            var manager = new AddEwicExclusionManager
            {
                ScanCode = testScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockAddEwicExclusionCommandHandler.Verify(c => c.Execute(It.IsAny<AddEwicExclusionCommand>()), Times.Once);
            mockSaveToMessageHistoryCommandHandler.Verify(c => c.Execute(It.IsAny<SaveToMessageHistoryCommand>()), Times.Once);
            mockSerializer.Verify(s => s.Serialize(It.Is<EwicExclusionMessageModel>(m => m.ActionTypeId == MessageActionTypes.AddOrUpdate)), Times.Once);
        }

        [TestMethod]
        public void AddEwicExclusion_SerializationIsSuccessful_MessageXmlShouldBePopulated()
        {
            // Given.
            var manager = new AddEwicExclusionManager
            {
                ScanCode = testScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockAddEwicExclusionCommandHandler.Verify(c => c.Execute(It.IsAny<AddEwicExclusionCommand>()), Times.Once);
            mockSaveToMessageHistoryCommandHandler.Verify(c => c.Execute(It.IsAny<SaveToMessageHistoryCommand>()), Times.Once);
            mockSerializer.Verify(s => s.Serialize(It.Is<EwicExclusionMessageModel>(m => m.ActionTypeId == MessageActionTypes.AddOrUpdate)), Times.Once);
            mockUpdateMessageHistoryMessageCommandHandler.Verify(c => c.Execute(It.IsAny<UpdateMessageHistoryMessageCommand>()), Times.Once);
        }

        [TestMethod]
        public void AddEwicExclusion_XmlPopulationIsSuccessful_SendMessageCommandShouldBeCalled()
        {
            // Given.
            var manager = new AddEwicExclusionManager
            {
                ScanCode = testScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockAddEwicExclusionCommandHandler.Verify(c => c.Execute(It.IsAny<AddEwicExclusionCommand>()), Times.Once);
            mockSaveToMessageHistoryCommandHandler.Verify(c => c.Execute(It.IsAny<SaveToMessageHistoryCommand>()), Times.Once);
            mockSerializer.Verify(s => s.Serialize(It.Is<EwicExclusionMessageModel>(m => m.ActionTypeId == MessageActionTypes.AddOrUpdate)), Times.Once);
            mockUpdateMessageHistoryMessageCommandHandler.Verify(c => c.Execute(It.IsAny<UpdateMessageHistoryMessageCommand>()), Times.Once);
        }

        /*[TestMethod]
        [ExpectedException(typeof(CommandException))]
        public void AddEwicExclusion_SendMessageCommandEncountersError_ExceptionShouldBeThrown()
        {
            // Given.
            mockProducer.Setup(p => p.SendMessages(It.IsAny<List<MessageHistory>>())).Throws(new Exception());

            var manager = new AddEwicExclusionManager
            {
                ScanCode = testScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockAddEwicExclusionCommandHandler.Verify(c => c.Execute(It.IsAny<AddEwicExclusionCommand>()), Times.Once);
            mockSaveToMessageHistoryCommandHandler.Verify(c => c.Execute(It.IsAny<SaveToMessageHistoryCommand>()), Times.Once);
            mockSerializer.Verify(s => s.Serialize(It.Is<EwicExclusionMessageModel>(m => m.ActionTypeId == MessageActionTypes.AddOrUpdate)), Times.Once);
            mockProducer.Verify(p => p.SendMessages(It.IsAny<List<MessageHistory>>()), Times.Once);
        }*/

        [TestMethod]
        public void AddEwicExclusion_SendMessageCommandIsSuccessful_MessageShouldBeSavedToMessageHistory()
        {
            // Given.
            var manager = new AddEwicExclusionManager
            {
                ScanCode = testScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockAddEwicExclusionCommandHandler.Verify(c => c.Execute(It.IsAny<AddEwicExclusionCommand>()), Times.Once);
            mockSaveToMessageHistoryCommandHandler.Verify(c => c.Execute(It.IsAny<SaveToMessageHistoryCommand>()), Times.Once);
            mockSerializer.Verify(s => s.Serialize(It.Is<EwicExclusionMessageModel>(m => m.ActionTypeId == MessageActionTypes.AddOrUpdate)), Times.Once);
            mockSaveToMessageHistoryCommandHandler.Verify(c => c.Execute(It.Is<SaveToMessageHistoryCommand>(h =>
                h.Messages.TrueForAll(m =>
                    m.MessageTypeId == MessageTypes.Ewic &&
                    m.MessageStatusId == MessageStatusTypes.Sent)
                )), Times.Once);
        }
    }
}
