using Icon.Common.DataAccess;
using Icon.Ewic.Models;
using Icon.Ewic.Serialization.Serializers;
using Icon.Ewic.Transmission.Producers;
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

namespace Icon.Web.Tests.Unit.Managers
{
    [TestClass] [Ignore]
    public class RemoveEwicMappingManagerHandlerTests
    {
        private RemoveEwicMappingManagerHandler managerHandler;
        private Mock<ISerializer<EwicMappingMessageModel>> mockSerializer;
        private Mock<IQueryHandler<GetEwicAgenciesWithMappingParameters, List<Agency>>> mockGetEwicAgenciesWithMappingQuery;
        private Mock<ICommandHandler<RemoveEwicMappingCommand>> mockRemoveEwicMappingCommandHandler;
        private Mock<ICommandHandler<SaveToMessageHistoryCommand>> mockSaveToMessageHistoryCommandHandler;
        private Mock<ICommandHandler<UpdateMessageHistoryMessageCommand>> mockUpdateMessageHistoryMessageCommandHandler;
        private Mock<IMessageProducer> mockProducer;
        private string testAplScanCode;
        private string testWfmScanCode;

        [TestInitialize]
        public void Initialize()
        {
            testAplScanCode = "22222222";
            testWfmScanCode = "22222223";

            mockSerializer = new Mock<ISerializer<EwicMappingMessageModel>>();
            mockGetEwicAgenciesWithMappingQuery = new Mock<IQueryHandler<GetEwicAgenciesWithMappingParameters, List<Agency>>>();
            mockRemoveEwicMappingCommandHandler = new Mock<ICommandHandler<RemoveEwicMappingCommand>>();
            mockSaveToMessageHistoryCommandHandler = new Mock<ICommandHandler<SaveToMessageHistoryCommand>>();
            mockUpdateMessageHistoryMessageCommandHandler = new Mock<ICommandHandler<UpdateMessageHistoryMessageCommand>>();
            mockProducer = new Mock<IMessageProducer>();

            managerHandler = new RemoveEwicMappingManagerHandler(
                mockSerializer.Object,
                mockGetEwicAgenciesWithMappingQuery.Object,
                mockRemoveEwicMappingCommandHandler.Object,
                mockSaveToMessageHistoryCommandHandler.Object,
                mockUpdateMessageHistoryMessageCommandHandler.Object,
                mockProducer.Object);

            mockGetEwicAgenciesWithMappingQuery.Setup(q => q.Search(It.IsAny<GetEwicAgenciesWithMappingParameters>())).Returns(new List<Agency> { new Agency() });

            var mockXml = new XDocument(new XElement("body"));
            mockSerializer.Setup(s => s.Serialize(It.IsAny<EwicMappingMessageModel>())).Returns(mockXml.ToString());
        }

        [TestMethod]
        public void RemoveEwicMapping_Execute_AgenciesWithTheMappingShouldBeQueried()
        {
            // Given.
            var manager = new RemoveEwicMappingManager
            {
                AplScanCode = testAplScanCode,
                WfmScanCode = testWfmScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockGetEwicAgenciesWithMappingQuery.Verify(q => q.Search(It.IsAny<GetEwicAgenciesWithMappingParameters>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(CommandException))]
        public void RemoveEwicMapping_MappingQueryEncountersError_ExceptionShouldBeThrown()
        {
            // Given.
            mockGetEwicAgenciesWithMappingQuery.Setup(q => q.Search(It.IsAny<GetEwicAgenciesWithMappingParameters>())).Throws(new Exception());

            var manager = new RemoveEwicMappingManager
            {
                AplScanCode = testAplScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RemoveEwicMapping_NoAgenciesHaveTheMapping_ExceptionShouldBeThrown()
        {
            // Given.
            mockGetEwicAgenciesWithMappingQuery.Setup(q => q.Search(It.IsAny<GetEwicAgenciesWithMappingParameters>())).Returns(new List<Agency>());

            var manager = new RemoveEwicMappingManager
            {
                AplScanCode = testAplScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(CommandException))]
        public void RemoveEwicMapping_RemoveEwicMappingCommandThrowsException_SerializerShouldNotBeCalled()
        {
            // Given.
            mockRemoveEwicMappingCommandHandler.Setup(c => c.Execute(It.IsAny<RemoveEwicMappingCommand>())).Throws(new Exception());

            var manager = new RemoveEwicMappingManager
            {
                AplScanCode = testAplScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockRemoveEwicMappingCommandHandler.Verify(c => c.Execute(It.IsAny<RemoveEwicMappingCommand>()), Times.Once);
            mockSerializer.Verify(s => s.Serialize(It.Is<EwicMappingMessageModel>(m => m.ActionTypeId == MessageActionTypes.Delete)), Times.Never);
        }

        [TestMethod]
        public void RemoveEwicMapping_SerializationIsSuccessful_MessageShouldBeSerialized()
        {
            // Given.
            var manager = new RemoveEwicMappingManager
            {
                AplScanCode = testAplScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockRemoveEwicMappingCommandHandler.Verify(c => c.Execute(It.IsAny<RemoveEwicMappingCommand>()), Times.Once);
            mockSerializer.Verify(s => s.Serialize(It.Is<EwicMappingMessageModel>(m => m.ActionTypeId == MessageActionTypes.Delete)), Times.Once);
            mockProducer.Verify(p => p.SendMessages(It.IsAny<List<MessageHistory>>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(CommandException))]
        public void RemoveEwicMapping_SendMessageCommandEncountersError_ExceptionShouldBeThrown()
        {
            // Given.
            mockProducer.Setup(p => p.SendMessages(It.IsAny<List<MessageHistory>>())).Throws(new Exception());

            var manager = new RemoveEwicMappingManager
            {
                AplScanCode = testAplScanCode,
                WfmScanCode = testWfmScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockRemoveEwicMappingCommandHandler.Verify(c => c.Execute(It.IsAny<RemoveEwicMappingCommand>()), Times.Once);
            mockSerializer.Verify(s => s.Serialize(It.Is<EwicMappingMessageModel>(m => m.ActionTypeId == MessageActionTypes.Delete)), Times.Once);
            mockProducer.Verify(p => p.SendMessages(It.IsAny<List<MessageHistory>>()), Times.Once);
        }

        [TestMethod]
        public void RemoveEwicMapping_SendIsSuccessful_MessagesShouldBeSaved()
        {
            // Given.
            var manager = new RemoveEwicMappingManager
            {
                AplScanCode = testAplScanCode,
                WfmScanCode = testWfmScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockRemoveEwicMappingCommandHandler.Verify(c => c.Execute(It.IsAny<RemoveEwicMappingCommand>()), Times.Once);
            mockSaveToMessageHistoryCommandHandler.Verify(c => c.Execute(It.IsAny<SaveToMessageHistoryCommand>()), Times.Once);
            mockSerializer.Verify(s => s.Serialize(It.Is<EwicMappingMessageModel>(m => m.ActionTypeId == MessageActionTypes.Delete)), Times.Once);
            mockProducer.Verify(p => p.SendMessages(It.IsAny<List<MessageHistory>>()), Times.Once);
        }
    }
}
