using Icon.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PushController.Common;
using PushController.Controller.UdmEntityBuilders;
using PushController.Controller.UdmEntityGenerators;
using PushController.Controller.UdmUpdateServices;
using System;
using System.Collections.Generic;

namespace PushController.Tests.Controller.UdmEntityGeneratorTests
{
    [TestClass]
    public class ItemLinkEntityGeneratorTests
    {
        private Mock<IUdmEntityBuilder<ItemLinkModel>> mockEntityBuilder;
        private Mock<IUdmUpdateService<ItemLinkModel>> mockEntityUpdateService;

        [TestInitialize]
        public void Initialize()
        {
            this.mockEntityBuilder = new Mock<IUdmEntityBuilder<ItemLinkModel>>();
            this.mockEntityUpdateService = new Mock<IUdmUpdateService<ItemLinkModel>>();
        }

        [TestMethod]
        public void GenerateEntityUpdates_PosDataReadyForUdm_EntityBuilderShouldBeCalled()
        {
            // Given.
            var entityGenerator = new ItemLinkEntityGenerator(this.mockEntityBuilder.Object, this.mockEntityUpdateService.Object);

            // When.
            entityGenerator.BuildEntities(new List<IRMAPush>());

            // Then.
            mockEntityBuilder.Verify(mb => mb.BuildEntities(It.IsAny<List<IRMAPush>>()), Times.Once);
        }

        [TestMethod]
        public void SaveEntities_EntitiesReadyToSave_BulkSaveMethodShouldBeCalled()
        {
            // Given.
            var entityGenerator = new ItemLinkEntityGenerator(mockEntityBuilder.Object, mockEntityUpdateService.Object);

            // When.
            entityGenerator.SaveEntities(new List<ItemLinkModel>());

            // Then.
            mockEntityUpdateService.Verify(mb => mb.SaveEntitiesBulk(It.IsAny<List<ItemLinkModel>>()), Times.Once);
        }

        [TestMethod]
        public void SaveEntities_ErrorInBulkProcessing_FallbackMethodShouldBeCalled()
        {
            // Given.
            mockEntityUpdateService.Setup(mq => mq.SaveEntitiesBulk(It.IsAny<List<ItemLinkModel>>())).Throws(new Exception());
            var entityGenerator = new ItemLinkEntityGenerator(mockEntityBuilder.Object, mockEntityUpdateService.Object);

            // When.
            entityGenerator.SaveEntities(new List<ItemLinkModel>());

            // Then.
            mockEntityUpdateService.Verify(mb => mb.SaveEntitiesBulk(It.IsAny<List<ItemLinkModel>>()), Times.Once);
            mockEntityUpdateService.Verify(mb => mb.SaveEntitiesRowByRow(It.IsAny<List<ItemLinkModel>>()), Times.Once);
        }
    }
}
