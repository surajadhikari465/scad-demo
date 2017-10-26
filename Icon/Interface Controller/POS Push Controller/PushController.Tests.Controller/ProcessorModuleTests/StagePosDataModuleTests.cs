using Icon.Common.Email;
using Icon.Logging;
using Icon.Testing.Builders;
using InterfaceController.Common;
using Irma.Framework;
using Irma.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PushController.Common;
using PushController.Common.Models;
using PushController.Controller.PosDataGenerators;
using PushController.Controller.ProcessorModules;
using PushController.DataAccess.Commands;
using PushController.DataAccess.Interfaces;
using PushController.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace PushController.Tests.Controller.ProcessorModuleTests
{
    [TestClass]
    public class StagePosDataModuleTests
    {
        private StagePosDataModule processorModule;
        private Mock<ILogger<StagePosDataModule>> mockLogger;
        private Mock<IIrmaContextProvider> mockIrmaContextProvider;
        private Mock<IQueryHandler<GetJobStatusQuery, JobStatus>> mockGetJobStatusQueryHandler;
        private Mock<IQueryHandler<GetIrmaPosDataQuery, List<IConPOSPushPublish>>> mockGetIrmaPosDataQueryHandler;
        private Mock<ICommandHandler<MarkPublishedRecordsAsInProcessCommand>> mockMarkRecordsAsInProcessCommandHandler;
        private Mock<ICommandHandler<UpdatePublishTableDatesCommand>> mockUpdatePublishTableDatesCommandHandler;
        private Mock<IPosDataGenerator<IrmaPushModel>> mockIrmaPushGenerator;
        private Mock<IEmailClient> mockEmailClient;

        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ILogger<StagePosDataModule>>();
            this.mockIrmaContextProvider = new Mock<IIrmaContextProvider>();
            this.mockGetJobStatusQueryHandler = new Mock<IQueryHandler<GetJobStatusQuery, JobStatus>>();
            this.mockGetIrmaPosDataQueryHandler = new Mock<IQueryHandler<GetIrmaPosDataQuery, List<IConPOSPushPublish>>>();
            this.mockMarkRecordsAsInProcessCommandHandler = new Mock<ICommandHandler<MarkPublishedRecordsAsInProcessCommand>>();
            this.mockUpdatePublishTableDatesCommandHandler = new Mock<ICommandHandler<UpdatePublishTableDatesCommand>>();
            this.mockIrmaPushGenerator = new Mock<IPosDataGenerator<IrmaPushModel>>();
            this.mockEmailClient = new Mock<IEmailClient>();

            StartupOptions.Instance = 1;
            StartupOptions.RegionsToProcess = new [] { "FL" };

            mockGetJobStatusQueryHandler.Setup(q => q.Execute(It.IsAny<GetJobStatusQuery>())).Returns(new JobStatus
            {
                Status = "NOT RUNNING",
                StatusDescription = "IconPOSPushPublish",
                LastRun = DateTime.Now.AddHours(-2)
            });
        }

        private void BuildProcessorModule()
        {
            processorModule = new StagePosDataModule(
                mockLogger.Object,
                mockIrmaContextProvider.Object,
                mockGetJobStatusQueryHandler.Object,
                mockGetIrmaPosDataQueryHandler.Object,
                mockMarkRecordsAsInProcessCommandHandler.Object,
                mockUpdatePublishTableDatesCommandHandler.Object,
                mockIrmaPushGenerator.Object,
                mockEmailClient.Object);
        }

        [TestMethod]
        public void StagePosDataModule_NoPosDataReadyToStage_PosDataShouldNotBeConverted()
        {
            // Given.
            mockGetIrmaPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIrmaPosDataQuery>())).Returns(new List<IConPOSPushPublish>());

            BuildProcessorModule();

            // When.
            processorModule.Execute();

            // Then.
            mockMarkRecordsAsInProcessCommandHandler.Verify(c => c.Execute(It.IsAny<MarkPublishedRecordsAsInProcessCommand>()), Times.Once);
            mockGetIrmaPosDataQueryHandler.Verify(q => q.Execute(It.IsAny<GetIrmaPosDataQuery>()), Times.Once);
            mockIrmaPushGenerator.Verify(g => g.ConvertPosData(It.IsAny<List<IConPOSPushPublish>>()), Times.Never);
        }

        [TestMethod]
        public void StagePosDataModule_PosDataIsMarkedAndReady_PosDataShouldBeConverted()
        {
            // Given.
            var testPosData = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder()
            };

            var emptyTestPosData = new List<IConPOSPushPublish>();

            var queuedPosData = new Queue<List<IConPOSPushPublish>>();
            queuedPosData.Enqueue(testPosData);
            queuedPosData.Enqueue(emptyTestPosData);

            mockGetIrmaPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIrmaPosDataQuery>())).Returns(queuedPosData.Dequeue);
            mockIrmaPushGenerator.Setup(g => g.ConvertPosData(It.IsAny<List<IConPOSPushPublish>>())).Returns(new List<IrmaPushModel>());

            BuildProcessorModule();

            // Then.
            processorModule.Execute();

            // When.
            mockIrmaPushGenerator.Verify(c => c.ConvertPosData(It.IsAny<List<IConPOSPushPublish>>()), Times.Once);
        }

        [TestMethod]
        public void StagePosDataModule_ConvertedPosDataIsAvailable_PosDataShouldBeStagedInIconAndProcessedDateUpdated()
        {
            // Given.
            var testPosData = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder()
            };

            var emptyTestPosData = new List<IConPOSPushPublish>();

            var queuedPosData = new Queue<List<IConPOSPushPublish>>();
            queuedPosData.Enqueue(testPosData);
            queuedPosData.Enqueue(emptyTestPosData);

            mockGetIrmaPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIrmaPosDataQuery>())).Returns(queuedPosData.Dequeue);
            mockIrmaPushGenerator.Setup(g => g.ConvertPosData(It.IsAny<List<IConPOSPushPublish>>())).Returns(new List<IrmaPushModel> { new IrmaPushModel() });

            BuildProcessorModule();

            // Then.
            processorModule.Execute();

            // When.
            mockIrmaPushGenerator.Verify(c => c.StagePosData(It.IsAny<List<IrmaPushModel>>()), Times.Once);
            mockUpdatePublishTableDatesCommandHandler.Verify(c => c.Execute(It.IsAny<UpdatePublishTableDatesCommand>()), Times.Exactly(1));
        }

        [TestMethod]
        public void StagePosDataModule_ConvertedPosDataIsNotAvailable_PosDataShouldNotBeStagedInIconAndProcessedDateNotUpdated()
        {
            // Given.
            var testPosData = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder()
            };

            var emptyTestPosData = new List<IConPOSPushPublish>();

            var queuedPosData = new Queue<List<IConPOSPushPublish>>();
            queuedPosData.Enqueue(testPosData);
            queuedPosData.Enqueue(emptyTestPosData);

            mockGetIrmaPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIrmaPosDataQuery>())).Returns(queuedPosData.Dequeue);
            mockIrmaPushGenerator.Setup(g => g.ConvertPosData(It.IsAny<List<IConPOSPushPublish>>())).Returns(new List<IrmaPushModel>());

            BuildProcessorModule();

            // Then.
            processorModule.Execute();

            // When.
            mockIrmaPushGenerator.Verify(c => c.StagePosData(It.IsAny<List<IrmaPushModel>>()), Times.Never);
            mockUpdatePublishTableDatesCommandHandler.Verify(c => c.Execute(It.IsAny<UpdatePublishTableDatesCommand>()), Times.Never);
        }

        [TestMethod]
        public void StagePosDataModule_PosPushIsPublishingData_NoDataShouldBeMarkedForProcessing()
        {
            // Given.
            mockGetJobStatusQueryHandler.Setup(q => q.Execute(It.IsAny<GetJobStatusQuery>())).Returns(new JobStatus
                {
                    Status = "RUNNING",
                    StatusDescription = "IconPOSPushPublish",
                    LastRun = DateTime.Now
                });

            BuildProcessorModule();

            // When.
            processorModule.Execute();

            // Then.
            mockMarkRecordsAsInProcessCommandHandler.Verify(c => c.Execute(It.IsAny<MarkPublishedRecordsAsInProcessCommand>()), Times.Never);
            mockLogger.Verify(l => l.Info(It.IsAny<string>()), Times.Exactly(StartupOptions.RegionsToProcess.Length));
        }

        [TestMethod]
        public void StagePosDataModule_PosPushIsPublishingDataForLongerThanMaxTime_NoDataShouldBeMarkedForProcessingAndWarningShouldBeLogged()
        {
            // Given.
            mockGetJobStatusQueryHandler.Setup(q => q.Execute(It.IsAny<GetJobStatusQuery>())).Returns(new JobStatus
            {
                Status = "RUNNING",
                StatusDescription = "IconPOSPushPublish",
                LastRun = DateTime.Now.AddHours(-2)
            });

            BuildProcessorModule();

            // When.
            processorModule.Execute();

            // Then.
            mockMarkRecordsAsInProcessCommandHandler.Verify(c => c.Execute(It.IsAny<MarkPublishedRecordsAsInProcessCommand>()), Times.Never);
            mockLogger.Verify(l => l.Warn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void StagePosDataModule_PosPushIsNotPublishingData_RecordsShouldBeMarkedForProcessing()
        {
            // Given.
            var testPosData = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder()
            };

            var emptyTestPosData = new List<IConPOSPushPublish>();

            var queuedPosData = new Queue<List<IConPOSPushPublish>>();
            queuedPosData.Enqueue(testPosData);
            queuedPosData.Enqueue(emptyTestPosData);

            mockGetIrmaPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIrmaPosDataQuery>())).Returns(queuedPosData.Dequeue);
            mockIrmaPushGenerator.Setup(g => g.ConvertPosData(It.IsAny<List<IConPOSPushPublish>>())).Returns(new List<IrmaPushModel>());

            BuildProcessorModule();

            // When.
            processorModule.Execute();

            // Then.
            mockMarkRecordsAsInProcessCommandHandler.Verify(c => c.Execute(It.IsAny<MarkPublishedRecordsAsInProcessCommand>()), Times.Exactly(2));
        }
    }
}
