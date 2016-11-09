using Irma.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PushController.Common.Models;
using PushController.Controller.PosDataConverters;
using PushController.Controller.PosDataGenerators;
using PushController.Controller.PosDataStagingServices;
using System;
using System.Collections.Generic;

namespace PushController.Tests.Controller.IconPosDataGeneratorTests
{
    [TestClass]
    public class IrmaPushDataGeneratorTests
    {
        private Mock<IPosDataConverter<IrmaPushModel>> mockIrmaPushDataConverter;
        private Mock<IPosDataStagingService<IrmaPushModel>> mockIrmaPushStagingService;

        [TestInitialize]
        public void Initialize()
        {
            this.mockIrmaPushDataConverter = new Mock<IPosDataConverter<IrmaPushModel>>();
            this.mockIrmaPushStagingService = new Mock<IPosDataStagingService<IrmaPushModel>>();
        }

        [TestMethod]
        public void GenerateIrmaPushData_PosDataReadyToStage_PushDataConverterShouldBeCalled()
        {
            // Given.
            var pushDataGenerator = new IrmaPushDataGenerator(this.mockIrmaPushDataConverter.Object, this.mockIrmaPushStagingService.Object);

            // When.
            pushDataGenerator.ConvertPosData(new List<IConPOSPushPublish>());

            // Then.
            mockIrmaPushDataConverter.Verify(push => push.ConvertPosData(It.IsAny<List<IConPOSPushPublish>>()), Times.Once);
        }

        [TestMethod]
        public void SavePosData_EntitiesReadyToSave_BulkSaveMethodShouldBeCalled()
        {
            // Given.
            var pushDataGenerator = new IrmaPushDataGenerator(mockIrmaPushDataConverter.Object, mockIrmaPushStagingService.Object);

            // When.
            pushDataGenerator.StagePosData(new List<IrmaPushModel>());

            // Then.
            mockIrmaPushStagingService.Verify(push => push.StagePosDataBulk(It.IsAny<List<IrmaPushModel>>()), Times.Once);
        }

        [TestMethod]
        public void SavePosData_ErrorInBulkProcessing_FallbackMethodShouldBeCalled()
        {
            // Given.
            mockIrmaPushStagingService.Setup(mq => mq.StagePosDataBulk(It.IsAny<List<IrmaPushModel>>())).Throws(new Exception());
            var pushDataGenerator = new IrmaPushDataGenerator(mockIrmaPushDataConverter.Object, mockIrmaPushStagingService.Object);

            // When.
            pushDataGenerator.StagePosData(new List<IrmaPushModel>());

            // Then.
            mockIrmaPushStagingService.Verify(push => push.StagePosDataBulk(It.IsAny<List<IrmaPushModel>>()), Times.Once);
            mockIrmaPushStagingService.Verify(push => push.StagePosDataRowByRow(It.IsAny<List<IrmaPushModel>>()), Times.Once);
        }
    }
}
