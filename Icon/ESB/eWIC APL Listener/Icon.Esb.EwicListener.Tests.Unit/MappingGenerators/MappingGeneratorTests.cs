using Icon.Common.DataAccess;
using Icon.Esb.EwicAplListener.Common.Models;
using Icon.Esb.EwicAplListener.DataAccess.Commands;
using Icon.Esb.EwicAplListener.DataAccess.Queries;
using Icon.Esb.EwicAplListener.MappingGenerators;
using Icon.Ewic.Models;
using Icon.Ewic.Serialization.Serializers;
using Icon.Ewic.Transmission.Producers;
using Icon.Framework;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Icon.Esb.EwicAplListener.Tests.Unit.MappingGenerators
{
    [TestClass]
    public class MappingGeneratorTests
    {
        private MappingGenerator generator;
        private Mock<ILogger<MappingGenerator>> mockLogger;
        private Mock<ISerializer<EwicMappingMessageModel>> mockSerializer;
        private Mock<IQueryHandler<GetExistingMappingsParameters, List<ScanCodeModel>>> mockGetExistingMappingsQuery;
        private Mock<ICommandHandler<AddMappingsParameters>> mockAddMappingsCommand;
        private Mock<ICommandHandler<SaveToMessageHistoryParameters>> mockSaveToMessageHistoryCommand;
        private Mock<ICommandHandler<UpdateMessageHistoryMessageParameters>> mockUpdateMessageHistoryMessageCommand;
        private Mock<IMessageProducer> mockProducer;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger<MappingGenerator>>();
            mockSerializer = new Mock<ISerializer<EwicMappingMessageModel>>();
            mockGetExistingMappingsQuery = new Mock<IQueryHandler<GetExistingMappingsParameters, List<ScanCodeModel>>>();
            mockAddMappingsCommand = new Mock<ICommandHandler<AddMappingsParameters>>();
            mockSaveToMessageHistoryCommand = new Mock<ICommandHandler<SaveToMessageHistoryParameters>>();
            mockUpdateMessageHistoryMessageCommand = new Mock<ICommandHandler<UpdateMessageHistoryMessageParameters>>();
            mockProducer = new Mock<IMessageProducer>();

            generator = new MappingGenerator(
                mockLogger.Object,
                mockSerializer.Object,
                mockGetExistingMappingsQuery.Object,
                mockAddMappingsCommand.Object,
                mockSaveToMessageHistoryCommand.Object,
                mockUpdateMessageHistoryMessageCommand.Object,
                mockProducer.Object);

            var mockXml = new XDocument(new XElement("body"));
            mockSerializer.Setup(s => s.Serialize(It.IsAny<EwicMappingMessageModel>())).Returns(mockXml.ToString());
        }
        [TestMethod]
        public void MappingGenerator_MappingsExist_MappingsShouldBeSavedAndTransmitted()
        {
            // Given.
            mockGetExistingMappingsQuery.Setup(q => q.Search(It.IsAny<GetExistingMappingsParameters>())).Returns(new List<ScanCodeModel> { new ScanCodeModel() });

            var model = new EwicItemModel();

            // When.
            generator.GenerateMappings(model);

            // Then.
            mockGetExistingMappingsQuery.Verify(q => q.Search(It.IsAny<GetExistingMappingsParameters>()), Times.Once);
            mockAddMappingsCommand.Verify(c => c.Execute(It.IsAny<AddMappingsParameters>()), Times.Once);
            mockUpdateMessageHistoryMessageCommand.Verify(c => c.Execute(It.IsAny<UpdateMessageHistoryMessageParameters>()), Times.Once);
            mockSerializer.Verify(s => s.Serialize(It.IsAny<EwicMappingMessageModel>()), Times.Once);
            mockSaveToMessageHistoryCommand.Verify(c => c.Execute(It.IsAny<SaveToMessageHistoryParameters>()), Times.Once);
            mockProducer.Verify(p => p.SendMessages(It.IsAny<List<MessageHistory>>()), Times.Once);
        }

        [TestMethod]
        public void MappingGenerator_MappingsDoNotExist_NoActionShouldBeTaken()
        {
            // Given.
            mockGetExistingMappingsQuery.Setup(q => q.Search(It.IsAny<GetExistingMappingsParameters>())).Returns(new List<ScanCodeModel>());

            var model = new EwicItemModel();

            // When.
            generator.GenerateMappings(model);

            // Then.
            mockGetExistingMappingsQuery.Verify(q => q.Search(It.IsAny<GetExistingMappingsParameters>()), Times.Once);
            mockAddMappingsCommand.Verify(c => c.Execute(It.IsAny<AddMappingsParameters>()), Times.Never);
            mockUpdateMessageHistoryMessageCommand.Verify(c => c.Execute(It.IsAny<UpdateMessageHistoryMessageParameters>()), Times.Never);
            mockSerializer.Verify(s => s.Serialize(It.IsAny<EwicMappingMessageModel>()), Times.Never);
            mockSaveToMessageHistoryCommand.Verify(c => c.Execute(It.IsAny<SaveToMessageHistoryParameters>()), Times.Never);
            mockProducer.Verify(p => p.SendMessages(It.IsAny<List<MessageHistory>>()), Times.Never);
        }
    }
}
