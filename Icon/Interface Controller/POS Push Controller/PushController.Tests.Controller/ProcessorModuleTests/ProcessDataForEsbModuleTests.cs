﻿using Icon.RenewableContext;
using Icon.Framework;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PushController.Common;
using PushController.Common.Models;
using PushController.Controller.CacheHelpers;
using PushController.Controller.MessageGenerators;
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
    public class ProcessDataForEsbModuleTests
    {
        private ProcessDataForEsbModule processorModule;
        private Mock<IRenewableContext<IconContext>> mockIconContext;
        private Mock<ILogger<ProcessDataForEsbModule>> mockLogger;
        private Mock<IQueryHandler<GetIconPosDataForEsbQuery, List<IRMAPush>>> mockGetIconPosDataQueryHandler;
        private Mock<ICacheHelper<string, ScanCodeModel>> mockScanCodeCacheHelper;
        private Mock<ICacheHelper<int, Locale>> mockLocaleCacheHelper;
        private Mock<ICacheHelper<Tuple<string, int>, string>> mockLinkedScanCodeCacheHelper;
        private Mock<ICommandHandler<MarkStagedRecordsAsInProcessForEsbCommand>> mockMarkPosDataAsInProcessCommandHandler;
        private Mock<ICommandHandler<UpdateStagingTableDatesForEsbCommand>> mockUpdateStagingTableDatesForEsbCommandHandler;
        private Mock<IMessageGenerator<MessageQueueItemLocale>> mockMessageGeneratorItemLocale;
        private Mock<IMessageGenerator<MessageQueuePrice>> mockMessageGeneratorPrice;
        private List<IRMAPush> mockPosData;

        [TestInitialize]
        public void Initialize()
        {
            this.mockIconContext = new Mock<IRenewableContext<IconContext>>();
            this.mockLogger = new Mock<ILogger<ProcessDataForEsbModule>>();
            this.mockGetIconPosDataQueryHandler = new Mock<IQueryHandler<GetIconPosDataForEsbQuery, List<IRMAPush>>>();
            this.mockScanCodeCacheHelper = new Mock<ICacheHelper<string, ScanCodeModel>>();
            this.mockLocaleCacheHelper = new Mock<ICacheHelper<int, Locale>>();
            this.mockLinkedScanCodeCacheHelper = new Mock<ICacheHelper<Tuple<string, int>, string>>();
            this.mockMarkPosDataAsInProcessCommandHandler = new Mock<ICommandHandler<MarkStagedRecordsAsInProcessForEsbCommand>>();
            this.mockUpdateStagingTableDatesForEsbCommandHandler = new Mock<ICommandHandler<UpdateStagingTableDatesForEsbCommand>>();
            this.mockMessageGeneratorItemLocale = new Mock<IMessageGenerator<MessageQueueItemLocale>>();
            this.mockMessageGeneratorPrice = new Mock<IMessageGenerator<MessageQueuePrice>>();

            StartupOptions.RegionsToProcess = ConfigurationManager.AppSettings["RegionsToProcess"].Split(',');

            processorModule = new ProcessDataForEsbModule(
                mockIconContext.Object,
                mockLogger.Object,
                mockScanCodeCacheHelper.Object,
                mockLocaleCacheHelper.Object,
                mockLinkedScanCodeCacheHelper.Object,
                mockGetIconPosDataQueryHandler.Object,
                mockMarkPosDataAsInProcessCommandHandler.Object,
                mockUpdateStagingTableDatesForEsbCommandHandler.Object,
                mockMessageGeneratorItemLocale.Object,
                mockMessageGeneratorPrice.Object);
        }

        [TestMethod]
        public void ProcessDataForEsb_InitialExecutionOfProcessingLoop_CachesShouldBePopulated()
        {
            // Given.
            this.mockPosData = new List<IRMAPush> { new TestIrmaPushBuilder() };
            var mockEmptyPosData = new List<IRMAPush>();

            var queuedPosData = new Queue<List<IRMAPush>>();
            queuedPosData.Enqueue(this.mockPosData);
            queuedPosData.Enqueue(mockEmptyPosData);

            mockGetIconPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIconPosDataForEsbQuery>())).Returns(queuedPosData.Dequeue);
            mockMessageGeneratorItemLocale.Setup(mg => mg.BuildMessages(It.IsAny<List<IRMAPush>>())).Returns(new List<MessageQueueItemLocale>());
            mockMessageGeneratorPrice.Setup(mg => mg.BuildMessages(It.IsAny<List<IRMAPush>>())).Returns(new List<MessageQueuePrice>());

            // When.
            processorModule.Execute();

            // Then.
            mockScanCodeCacheHelper.Verify(c => c.Populate(It.IsAny<List<string>>()), Times.Exactly(2));
            mockLocaleCacheHelper.Verify(c => c.Populate(It.IsAny<List<int>>()), Times.Once);
            mockLinkedScanCodeCacheHelper.Verify(c => c.Populate(It.IsAny<List<Tuple<string, int>>>()), Times.Once);
        }

        [TestMethod]
        public void ProcessDataForEsb_NoPosDataIsMarkedAndReady_ItemLocaleMessagesShouldNotBeBuilt()
        {
            // Given.
            mockGetIconPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIconPosDataForEsbQuery>())).Returns(new List<IRMAPush>());

            // When.
            processorModule.Execute();

            // Then.
            mockMessageGeneratorItemLocale.Verify(mg => mg.BuildMessages(It.IsAny<List<IRMAPush>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessDataForEsb_PosDataIsMarkedAndReady_ItemLocaleMessagesShouldBeBuilt()
        {
            // Given.
            this.mockPosData = new List<IRMAPush> { new TestIrmaPushBuilder() };
            var mockEmptyPosData = new List<IRMAPush>();

            var queuedPosData = new Queue<List<IRMAPush>>();
            queuedPosData.Enqueue(this.mockPosData);
            queuedPosData.Enqueue(mockEmptyPosData);

            mockGetIconPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIconPosDataForEsbQuery>())).Returns(queuedPosData.Dequeue);
            mockMessageGeneratorItemLocale.Setup(mg => mg.BuildMessages(It.IsAny<List<IRMAPush>>())).Returns(new List<MessageQueueItemLocale>());
            mockMessageGeneratorPrice.Setup(mg => mg.BuildMessages(It.IsAny<List<IRMAPush>>())).Returns(new List<MessageQueuePrice>());

            // When.
            processorModule.Execute();

            // Then.
            mockMessageGeneratorItemLocale.Verify(mg => mg.BuildMessages(It.IsAny<List<IRMAPush>>()), Times.Once);
        }

        [TestMethod]
        public void ProcessDataForEsb_ItemLocaleMessagesAreBuilt_ItemLocaleMessagesShouldBeSaved()
        {
            // Given.
            this.mockPosData = new List<IRMAPush> { new TestIrmaPushBuilder() };
            var mockEmptyPosData = new List<IRMAPush>();

            var queuedPosData = new Queue<List<IRMAPush>>();
            queuedPosData.Enqueue(this.mockPosData);
            queuedPosData.Enqueue(mockEmptyPosData);

            mockGetIconPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIconPosDataForEsbQuery>())).Returns(queuedPosData.Dequeue);
            mockMessageGeneratorItemLocale.Setup(mg => mg.BuildMessages(It.IsAny<List<IRMAPush>>())).Returns(new List<MessageQueueItemLocale> { new TestItemLocaleMessageBuilder() });
            mockMessageGeneratorPrice.Setup(mg => mg.BuildMessages(It.IsAny<List<IRMAPush>>())).Returns(new List<MessageQueuePrice>());

            // When.
            processorModule.Execute();

            // Then.
            mockMessageGeneratorItemLocale.Verify(mg => mg.SaveMessages(It.IsAny<List<MessageQueueItemLocale>>()), Times.Once);
        }

        [TestMethod]
        public void ProcessDataForEsb_NoItemLocaleMessagesAreSuccessfullyBuilt_NoItemLocaleMessagesShouldBeSaved()
        {
            // Given.
            this.mockPosData = new List<IRMAPush> { new TestIrmaPushBuilder() };
            var mockEmptyPosData = new List<IRMAPush>();

            var queuedPosData = new Queue<List<IRMAPush>>();
            queuedPosData.Enqueue(this.mockPosData);
            queuedPosData.Enqueue(mockEmptyPosData);

            mockGetIconPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIconPosDataForEsbQuery>())).Returns(queuedPosData.Dequeue);
            mockMessageGeneratorItemLocale.Setup(mg => mg.BuildMessages(It.IsAny<List<IRMAPush>>())).Returns(new List<MessageQueueItemLocale>());
            mockMessageGeneratorPrice.Setup(mg => mg.BuildMessages(It.IsAny<List<IRMAPush>>())).Returns(new List<MessageQueuePrice>());

            // When.
            processorModule.Execute();

            // Then.
            mockMessageGeneratorItemLocale.Verify(mg => mg.SaveMessages(It.IsAny<List<MessageQueueItemLocale>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessDataForEsb_PosDataIsMarkedAndReady_PriceMessagesShouldBeBuilt()
        {
            // Given.
            this.mockPosData = new List<IRMAPush> { new TestIrmaPushBuilder() };
            var mockEmptyPosData = new List<IRMAPush>();
            Cache.ClearAll();
            // setup cache to have Bu not part of Gpm to verify MessageGeneratorPrice.BuildMessages is called
            this.mockPosData.ForEach(x => SetUpNonGpmStoreListCache(x.BusinessUnit_ID));

            var queuedPosData = new Queue<List<IRMAPush>>();
            queuedPosData.Enqueue(this.mockPosData);
            queuedPosData.Enqueue(mockEmptyPosData);

            mockGetIconPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIconPosDataForEsbQuery>())).Returns(queuedPosData.Dequeue);
            mockMessageGeneratorItemLocale.Setup(mg => mg.BuildMessages(It.IsAny<List<IRMAPush>>())).Returns(new List<MessageQueueItemLocale>());
            mockMessageGeneratorPrice.Setup(mg => mg.BuildMessages(It.IsAny<List<IRMAPush>>())).Returns(new List<MessageQueuePrice>());

            // When.
            processorModule.Execute();

            // Then.
            mockMessageGeneratorPrice.Verify(mg => mg.BuildMessages(It.IsAny<List<IRMAPush>>()), Times.Once);
        }

        [TestMethod]
        public void ProcessDataForEsb_PriceMessagesAreBuilt_PriceMessagesShouldBeSaved()
        {
            // Given.
            this.mockPosData = new List<IRMAPush> { new TestIrmaPushBuilder() };
            var mockEmptyPosData = new List<IRMAPush>();
            Cache.ClearAll();
            // setup cache to have Bu not part of Gpm to verify MessageGeneratorPrice.SaveMessages is called
            this.mockPosData.ForEach(x => SetUpNonGpmStoreListCache(x.BusinessUnit_ID));

            var queuedPosData = new Queue<List<IRMAPush>>();
            queuedPosData.Enqueue(this.mockPosData);
            queuedPosData.Enqueue(mockEmptyPosData);

            mockGetIconPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIconPosDataForEsbQuery>())).Returns(queuedPosData.Dequeue);
            mockMessageGeneratorItemLocale.Setup(mg => mg.BuildMessages(It.IsAny<List<IRMAPush>>())).Returns(new List<MessageQueueItemLocale> { new TestItemLocaleMessageBuilder() });
            mockMessageGeneratorPrice.Setup(mg => mg.BuildMessages(It.IsAny<List<IRMAPush>>())).Returns(new List<MessageQueuePrice> { new TestPriceMessageBuilder() });

            // When.
            processorModule.Execute();

            // Then.
            mockMessageGeneratorPrice.Verify(mg => mg.SaveMessages(It.IsAny<List<MessageQueuePrice>>()), Times.Once);
        }

        [TestMethod]
        public void ProcessDataForEsb_NoPriceMessagesAreSuccessfullyBuilt_NoPriceMessagesShouldBeSaved()
        {
            // Given.
            this.mockPosData = new List<IRMAPush> { new TestIrmaPushBuilder() };
            var mockEmptyPosData = new List<IRMAPush>();

            var queuedPosData = new Queue<List<IRMAPush>>();
            queuedPosData.Enqueue(this.mockPosData);
            queuedPosData.Enqueue(mockEmptyPosData);

            mockGetIconPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIconPosDataForEsbQuery>())).Returns(queuedPosData.Dequeue);
            mockMessageGeneratorItemLocale.Setup(mg => mg.BuildMessages(It.IsAny<List<IRMAPush>>())).Returns(new List<MessageQueueItemLocale> { new TestItemLocaleMessageBuilder() });
            mockMessageGeneratorPrice.Setup(mg => mg.BuildMessages(It.IsAny<List<IRMAPush>>())).Returns(new List<MessageQueuePrice>());

            // When.
            processorModule.Execute();

            // Then.
            mockMessageGeneratorPrice.Verify(mg => mg.SaveMessages(It.IsAny<List<MessageQueuePrice>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessDataForEsb_AfterItemLocaleAndPriceMessagesAreBuilt_StagingTableDateShouldBeUpdated()
        {
            // Given.
            this.mockPosData = new List<IRMAPush> { new TestIrmaPushBuilder() };
            var mockEmptyPosData = new List<IRMAPush>();

            var queuedPosData = new Queue<List<IRMAPush>>();
            queuedPosData.Enqueue(this.mockPosData);
            queuedPosData.Enqueue(mockEmptyPosData);

            mockGetIconPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIconPosDataForEsbQuery>())).Returns(queuedPosData.Dequeue);
            mockMessageGeneratorItemLocale.Setup(mg => mg.BuildMessages(It.IsAny<List<IRMAPush>>())).Returns(new List<MessageQueueItemLocale> { new TestItemLocaleMessageBuilder() });
            mockMessageGeneratorPrice.Setup(mg => mg.BuildMessages(It.IsAny<List<IRMAPush>>())).Returns(new List<MessageQueuePrice> { new TestPriceMessageBuilder() });

            // When.
            processorModule.Execute();

            // Then.
            mockUpdateStagingTableDatesForEsbCommandHandler.Verify(c => c.Execute(It.IsAny<UpdateStagingTableDatesForEsbCommand>()), Times.Once);
        }

        [TestMethod]
        public void ProcessDataForEsb_AllStoresOnGpm_SaveMessagesForPriceShouldNotBeCalled()
        {
            // Given.
            this.mockPosData = new List<IRMAPush> { new TestIrmaPushBuilder() };
            var mockEmptyPosData = new List<IRMAPush>();
            Cache.ClearAll();
            // input businessUnitId that isn't part of data being processed.
            this.mockPosData.ForEach(x => SetUpNonGpmStoreListCache(x.BusinessUnit_ID + 1)); 

            var queuedPosData = new Queue<List<IRMAPush>>();
            queuedPosData.Enqueue(this.mockPosData);
            queuedPosData.Enqueue(mockEmptyPosData);
            mockGetIconPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIconPosDataForEsbQuery>())).Returns(queuedPosData.Dequeue);
            mockMessageGeneratorItemLocale.Setup(mg => mg.BuildMessages(It.IsAny<List<IRMAPush>>())).Returns(new List<MessageQueueItemLocale> { new TestItemLocaleMessageBuilder() });
            mockMessageGeneratorPrice.Setup(mg => mg.BuildMessages(It.IsAny<List<IRMAPush>>())).Returns(new List<MessageQueuePrice> { new TestPriceMessageBuilder() });

            // When.
            processorModule.Execute();

            // Then.
            mockMessageGeneratorPrice.Verify(mg => mg.SaveMessages(It.IsAny<List<MessageQueuePrice>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessDataForEsb_NoStoresOnGpm_SaveMessagesForPriceShouldBeCalled()
        {
            // Given.
            this.mockPosData = new List<IRMAPush> { new TestIrmaPushBuilder() };
            var mockEmptyPosData = new List<IRMAPush>();
            Cache.ClearAll();
            // make sure BUs being processed are not marked on GPM
            this.mockPosData.ForEach(x => SetUpNonGpmStoreListCache(x.BusinessUnit_ID)); 

            var queuedPosData = new Queue<List<IRMAPush>>();
            queuedPosData.Enqueue(this.mockPosData);
            queuedPosData.Enqueue(mockEmptyPosData);

            mockGetIconPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIconPosDataForEsbQuery>())).Returns(queuedPosData.Dequeue);
            mockMessageGeneratorItemLocale.Setup(mg => mg.BuildMessages(It.IsAny<List<IRMAPush>>())).Returns(new List<MessageQueueItemLocale> { new TestItemLocaleMessageBuilder() });
            mockMessageGeneratorPrice.Setup(mg => mg.BuildMessages(It.IsAny<List<IRMAPush>>())).Returns(new List<MessageQueuePrice> { new TestPriceMessageBuilder() });

            // When.
            processorModule.Execute();

            // Then.
            mockMessageGeneratorPrice.Verify(mg => mg.SaveMessages(It.IsAny<List<MessageQueuePrice>>()), Times.Once);
        }

        private void SetUpNonGpmStoreListCache(int businessUnitId)
        {
            Cache.nonGpmStores.Add(businessUnitId);
        }
    }
}
