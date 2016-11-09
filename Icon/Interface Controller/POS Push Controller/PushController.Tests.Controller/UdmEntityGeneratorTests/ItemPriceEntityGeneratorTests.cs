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
    public class ItemPriceEntityGeneratorTests
    {
        private Mock<IUdmEntityBuilder<ItemPriceModel>> mockEntityBuilder;
        private Mock<IUdmUpdateService<ItemPriceModel>> mockEntityUpdateService;

        [TestInitialize]
        public void Initialize()
        {
            this.mockEntityBuilder = new Mock<IUdmEntityBuilder<ItemPriceModel>>();
            this.mockEntityUpdateService = new Mock<IUdmUpdateService<ItemPriceModel>>();
        }

        [TestMethod]
        public void GenerateEntityUpdates_PosDataReadyForUdm_EntityBuilderShouldBeCalled()
        {
            // Given.
            var entityGenerator = new ItemPriceEntityGenerator(this.mockEntityBuilder.Object, this.mockEntityUpdateService.Object);

            // When.
            entityGenerator.BuildEntities(new List<IRMAPush>());

            // Then.
            mockEntityBuilder.Verify(mb => mb.BuildEntities(It.IsAny<List<IRMAPush>>()), Times.Once);
        }

        [TestMethod]
        public void SaveEntities_EntitiesReadyToSave_BulkSaveMethodShouldBeCalled()
        {
            // Given.
            var entityGenerator = new ItemPriceEntityGenerator(mockEntityBuilder.Object, mockEntityUpdateService.Object);

            // When.
            entityGenerator.SaveEntities(new List<ItemPriceModel>());

            // Then.
            mockEntityUpdateService.Verify(mb => mb.SaveEntitiesBulk(It.IsAny<List<ItemPriceModel>>()), Times.Once);
        }

        [TestMethod]
        public void SaveEntities_ErrorInBulkProcessing_FallbackMethodShouldBeCalled()
        {
            // Given.
            mockEntityUpdateService.Setup(mq => mq.SaveEntitiesBulk(It.IsAny<List<ItemPriceModel>>())).Throws(new Exception());
            var entityGenerator = new ItemPriceEntityGenerator(mockEntityBuilder.Object, mockEntityUpdateService.Object);

            // When.
            entityGenerator.SaveEntities(new List<ItemPriceModel>());

            // Then.
            mockEntityUpdateService.Verify(mb => mb.SaveEntitiesBulk(It.IsAny<List<ItemPriceModel>>()), Times.Once);
            mockEntityUpdateService.Verify(mb => mb.SaveEntitiesRowByRow(It.IsAny<List<ItemPriceModel>>()), Times.Once);
        }
    }
}
