using Icon.Common.DataAccess;
using Icon.Esb.EwicAplListener.Common.Models;
using Icon.Esb.EwicAplListener.DataAccess.Commands;
using Icon.Esb.EwicAplListener.DataAccess.Queries;
using Icon.Esb.EwicAplListener.ExclusionGenerators;
using Icon.Ewic.Models;
using Icon.Ewic.Serialization.Serializers;
using Icon.Ewic.Transmission.Producers;
using Icon.Framework;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Icon.Esb.EwicAplListener.Tests.Unit.ExclusionGenerators
{
    [TestClass]
    public class ExclusionGeneratorTests
    {
        private ExclusionGenerator generator;
        private Mock<ILogger<ExclusionGenerator>> mockLogger;
        private Mock<ISerializer<EwicExclusionMessageModel>> mockSerializer;
        private Mock<IQueryHandler<GetExclusionParameters, ScanCodeModel>> mockExclusionExistsQuery;
        private Mock<ICommandHandler<AddExclusionParameters>> mockAddExclusionCommand;
        private Mock<ICommandHandler<SaveToMessageHistoryParameters>> mockSaveToMessageHistoryCommand;
        private Mock<ICommandHandler<UpdateMessageHistoryMessageParameters>> mockUpdateMessageHistoryMessageCommand;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger<ExclusionGenerator>>();
            mockSerializer = new Mock<ISerializer<EwicExclusionMessageModel>>();
            mockExclusionExistsQuery = new Mock<IQueryHandler<GetExclusionParameters, ScanCodeModel>>();
            mockAddExclusionCommand = new Mock<ICommandHandler<AddExclusionParameters>>();
            mockSaveToMessageHistoryCommand = new Mock<ICommandHandler<SaveToMessageHistoryParameters>>();
            mockUpdateMessageHistoryMessageCommand = new Mock<ICommandHandler<UpdateMessageHistoryMessageParameters>>();

            generator = new ExclusionGenerator(
                mockLogger.Object,
                mockSerializer.Object,
                mockExclusionExistsQuery.Object,
                mockAddExclusionCommand.Object,
                mockSaveToMessageHistoryCommand.Object,
                mockUpdateMessageHistoryMessageCommand.Object);

            var mockXml = new XDocument(new XElement("body"));
            mockSerializer.Setup(s => s.Serialize(It.IsAny<EwicExclusionMessageModel>())).Returns(mockXml.ToString());
        }

        [TestMethod]
        public void ExclusionGenerator_ExclusionExists_ExclusionShouldBeSavedAndTransmitted()
        {
            // Given.
            mockExclusionExistsQuery.Setup(q => q.Search(It.IsAny<GetExclusionParameters>())).Returns(new ScanCodeModel { ScanCode = "222222222" });

            var model = new EwicItemModel();

            // When.
            generator.GenerateExclusions(model);

            // Then.
            mockExclusionExistsQuery.Verify(q => q.Search(It.IsAny<GetExclusionParameters>()), Times.Once);
            mockAddExclusionCommand.Verify(c => c.Execute(It.IsAny<AddExclusionParameters>()), Times.Once);
            mockUpdateMessageHistoryMessageCommand.Verify(c => c.Execute(It.IsAny<UpdateMessageHistoryMessageParameters>()), Times.Once);
            mockSerializer.Verify(s => s.Serialize(It.IsAny<EwicExclusionMessageModel>()), Times.Once);
            mockSaveToMessageHistoryCommand.Verify(c => c.Execute(It.IsAny<SaveToMessageHistoryParameters>()), Times.Once);
        }

        [TestMethod]
        public void ExclusionGenerator_ExclusionDoesNotExist_NoActionShouldBeTaken()
        {
            // Given.
            mockExclusionExistsQuery.Setup(q => q.Search(It.IsAny<GetExclusionParameters>())).Returns(new ScanCodeModel());

            var model = new EwicItemModel();

            // When.
            generator.GenerateExclusions(model);

            // Then.
            mockExclusionExistsQuery.Verify(q => q.Search(It.IsAny<GetExclusionParameters>()), Times.Once);
            mockAddExclusionCommand.Verify(c => c.Execute(It.IsAny<AddExclusionParameters>()), Times.Never);
            mockUpdateMessageHistoryMessageCommand.Verify(c => c.Execute(It.IsAny<UpdateMessageHistoryMessageParameters>()), Times.Never);
            mockSerializer.Verify(s => s.Serialize(It.IsAny<EwicExclusionMessageModel>()), Times.Never);
            mockSaveToMessageHistoryCommand.Verify(c => c.Execute(It.IsAny<SaveToMessageHistoryParameters>()), Times.Never);
        }
    }
}
