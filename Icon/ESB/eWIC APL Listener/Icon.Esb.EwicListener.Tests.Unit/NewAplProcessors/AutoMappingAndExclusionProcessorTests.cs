using Icon.Esb.EwicAplListener.Common.Models;
using Icon.Esb.EwicAplListener.ExclusionGenerators;
using Icon.Esb.EwicAplListener.MappingGenerators;
using Icon.Esb.EwicAplListener.NewAplProcessors;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Icon.Esb.EwicAplListener.Tests.Unit.NewAplProcessors
{
    [TestClass]
    public class AutoMappingAndExclusionProcessorTests
    {
        private AutoMappingAndExclusionProcessor processor;
        private Mock<ILogger<AutoMappingAndExclusionProcessor>> mockLogger;
        private Mock<IExclusionGenerator> mockExclusionGenerator;
        private Mock<IMappingGenerator> mockMappingGenerator;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger<AutoMappingAndExclusionProcessor>>();
            mockExclusionGenerator = new Mock<IExclusionGenerator>();
            mockMappingGenerator = new Mock<IMappingGenerator>();

            processor = new AutoMappingAndExclusionProcessor(
                mockLogger.Object,
                mockExclusionGenerator.Object,
                mockMappingGenerator.Object);
        }

        [TestMethod]
        public void AutoMappingAndExclusions_ApplyMappings_MappingGeneratorShouldBeCalled()
        {
            // Given.
            var model = new AuthorizedProductListModel { Items = new List<EwicItemModel> { new EwicItemModel() } };

            // When.
            processor.ApplyMappings(model);

            // Then.
            mockMappingGenerator.Verify(g => g.GenerateMappings(It.IsAny<EwicItemModel>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AutoMappingAndExclusions_MappingGeneratorThrowsException_ExceptionShouldBeThrownToCaller()
        {
            // Given.
            mockMappingGenerator.Setup(g => g.GenerateMappings(It.IsAny<EwicItemModel>())).Throws(new Exception());

            var model = new AuthorizedProductListModel { Items = new List<EwicItemModel> { new EwicItemModel() } };

            // When.
            processor.ApplyMappings(model);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        public void AutoMappingAndExclusions_ApplyExclusions_ExclusionGeneratorShouldBeCalled()
        {
            // Given.
            var model = new AuthorizedProductListModel { Items = new List<EwicItemModel> { new EwicItemModel() } };

            // When.
            processor.ApplyExclusions(model);

            // Then.
            mockExclusionGenerator.Verify(g => g.GenerateExclusions(It.IsAny<EwicItemModel>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AutoMappingAndExclusions_ExclusionGeneratorThrowsException_ExceptionShouldBeThrownToCaller()
        {
            // Given.
            mockExclusionGenerator.Setup(g => g.GenerateExclusions(It.IsAny<EwicItemModel>())).Throws(new Exception());

            var model = new AuthorizedProductListModel { Items = new List<EwicItemModel> { new EwicItemModel() } };

            // When.
            processor.ApplyExclusions(model);

            // Then.
            // Expected exception.
        }
    }
}
