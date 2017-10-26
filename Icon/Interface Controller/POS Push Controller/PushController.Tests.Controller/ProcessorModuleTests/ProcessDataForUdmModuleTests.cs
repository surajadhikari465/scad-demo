using Icon.RenewableContext;
using Icon.Framework;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PushController.Common;
using PushController.Common.Models;
using PushController.Controller.CacheHelpers;
using PushController.Controller.ProcessorModules;
using PushController.Controller.UdmDeleteServices;
using PushController.Controller.UdmEntityGenerators;
using PushController.DataAccess.Commands;
using PushController.DataAccess.Interfaces;
using PushController.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace PushController.Tests.Controller.ProcessorModuleTests
{
    [TestClass]
    public class ProcessDataForUdmModuleTests
    {
        private ProcessDataForUdmModule processorModule;
        private Mock<IRenewableContext<IconContext>> mockIconContext;
        private Mock<ILogger<ProcessDataForUdmModule>> mockLogger;
        private Mock<IQueryHandler<GetIconPosDataForUdmQuery, List<IRMAPush>>> mockGetIconPosDataQueryHandler;
        private Mock<IQueryHandler<GetIrmaItemSubscriptionsQuery, List<IRMAItemSubscription>>> mockGetSubscriptionsQueryHandler;
        private Mock<ICacheHelper<string, ScanCodeModel>> mockScanCodeCacheHelper;
        private Mock<ICacheHelper<int, Locale>> mockLocaleCacheHelper;
        private Mock<ICommandHandler<MarkStagedRecordsAsInProcessForUdmCommand>> mockMarkRecordsAsInProcessCommandHandler;
        private Mock<ICommandHandler<UpdateStagingTableDatesForUdmCommand>> mockUpdateStagingTableDatesCommandHandler;
        private Mock<IUdmEntityGenerator<ItemPriceModel>> mockItemPriceEntityGenerator;
        private Mock<IUdmEntityGenerator<ItemLinkModel>> mockItemLinkEntityGenerator;
        private Mock<IUdmDeleteService<IRMAItemSubscription>> mockItemSubscriptionDeleteService;
        private Mock<IUdmDeleteService<TemporaryPriceReductionModel>> mockTemporaryPriceReductionDeleteService;
        private Mock<IUdmDeleteService<ItemLinkModel>> mockItemLinkDeleteService;

        [TestInitialize]
        public void Initialize()
        {
            this.mockIconContext = new Mock<IRenewableContext<IconContext>>();
            this.mockLogger = new Mock<ILogger<ProcessDataForUdmModule>>();
            this.mockGetIconPosDataQueryHandler = new Mock<IQueryHandler<GetIconPosDataForUdmQuery, List<IRMAPush>>>();
            this.mockGetSubscriptionsQueryHandler = new Mock<IQueryHandler<GetIrmaItemSubscriptionsQuery, List<IRMAItemSubscription>>>();
            this.mockScanCodeCacheHelper = new Mock<ICacheHelper<string, ScanCodeModel>>();
            this.mockLocaleCacheHelper = new Mock<ICacheHelper<int, Locale>>();
            this.mockMarkRecordsAsInProcessCommandHandler = new Mock<ICommandHandler<MarkStagedRecordsAsInProcessForUdmCommand>>();
            this.mockUpdateStagingTableDatesCommandHandler = new Mock<ICommandHandler<UpdateStagingTableDatesForUdmCommand>>();
            this.mockItemPriceEntityGenerator = new Mock<IUdmEntityGenerator<ItemPriceModel>>();
            this.mockItemLinkEntityGenerator = new Mock<IUdmEntityGenerator<ItemLinkModel>>();
            this.mockItemSubscriptionDeleteService = new Mock<IUdmDeleteService<IRMAItemSubscription>>();
            this.mockTemporaryPriceReductionDeleteService = new Mock<IUdmDeleteService<TemporaryPriceReductionModel>>();
            this.mockItemLinkDeleteService = new Mock<IUdmDeleteService<ItemLinkModel>>();

            StartupOptions.RegionsToProcess = ConfigurationManager.AppSettings["RegionsToProcess"].Split(',');
            SetUpCache(false);

            processorModule = new ProcessDataForUdmModule(
                mockIconContext.Object,
                mockLogger.Object,
                mockGetIconPosDataQueryHandler.Object,
                mockGetSubscriptionsQueryHandler.Object,
                mockScanCodeCacheHelper.Object,
                mockLocaleCacheHelper.Object,
                mockMarkRecordsAsInProcessCommandHandler.Object,
                mockUpdateStagingTableDatesCommandHandler.Object,
                mockItemLinkEntityGenerator.Object,
                mockItemPriceEntityGenerator.Object,
                mockItemSubscriptionDeleteService.Object,
                mockTemporaryPriceReductionDeleteService.Object,
                mockItemLinkDeleteService.Object);
        }

        [TestMethod]
        public void ProcessDataForUdm_NoPosDataReadyForUdm_ItemLinkEntitiesShouldNotBeBuilt()
        {
            // Given.
            mockGetIconPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIconPosDataForUdmQuery>())).Returns(new List<IRMAPush>());

            // When.
            processorModule.Execute();

            // Then.
            mockItemLinkEntityGenerator.Verify(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessDataForUdm_PosDataIsMarkedAndReady_ItemLinkEntitiesShouldBeBuilt()
        {
            // Given.
            var mockPosData = new List<IRMAPush> { new TestIrmaPushBuilder() };
            var mockEmptyPosData = new List<IRMAPush>();

            var queuedPosData = new Queue<List<IRMAPush>>();
            queuedPosData.Enqueue(mockPosData);
            queuedPosData.Enqueue(mockEmptyPosData);

            mockGetIconPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIconPosDataForUdmQuery>())).Returns(queuedPosData.Dequeue);
            mockItemPriceEntityGenerator.Setup(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>())).Returns(new List<ItemPriceModel>());
            mockItemLinkEntityGenerator.Setup(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>())).Returns(new List<ItemLinkModel>());

            // When.
            processorModule.Execute();

            // Then.
            mockItemLinkEntityGenerator.Verify(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>()), Times.Once);
        }

        [TestMethod]
        public void ProcessDataForUdm_ItemLinkEntitiesAreBuilt_ItemLinkEntitiesShouldBeSaved()
        {
            // Given.
            var mockPosData = new List<IRMAPush> { new TestIrmaPushBuilder().WithLinkedIdentifier("12345555") };
            var mockEmptyPosData = new List<IRMAPush>();

            var queuedPosData = new Queue<List<IRMAPush>>();
            queuedPosData.Enqueue(mockPosData);
            queuedPosData.Enqueue(mockEmptyPosData);

            mockGetIconPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIconPosDataForUdmQuery>())).Returns(queuedPosData.Dequeue);
            mockItemPriceEntityGenerator.Setup(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>())).Returns(new List<ItemPriceModel>());
            mockItemLinkEntityGenerator.Setup(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>())).Returns(new List<ItemLinkModel> { new ItemLinkModel() });

            // When.
            processorModule.Execute();

            // Then.
            var childScanCodes = mockPosData.Select(ip => ip.Identifier).ToList();
            var parentScanCodes = mockPosData.Select(ip => ip.LinkedIdentifier).ToList();
            mockScanCodeCacheHelper.Verify(m => m.Populate(It.Is<List<string>>(l => l.SequenceEqual(childScanCodes))), Times.Once);
            mockScanCodeCacheHelper.Verify(m => m.Populate(It.Is<List<string>>(l => l.SequenceEqual(parentScanCodes))), Times.Once);
            mockItemLinkEntityGenerator.Verify(eg => eg.SaveEntities(It.IsAny<List<ItemLinkModel>>()), Times.Once);
        }

        [TestMethod]
        public void ProcessDataForUdm_NoItemLinkEntitiesAreSuccessfullyBuilt_ItemLinkEntitiesShouldNotBeSaved()
        {
            // Given.
            var mockPosData = new List<IRMAPush> { new TestIrmaPushBuilder() };
            var mockEmptyPosData = new List<IRMAPush>();

            var queuedPosData = new Queue<List<IRMAPush>>();
            queuedPosData.Enqueue(mockPosData);
            queuedPosData.Enqueue(mockEmptyPosData);

            mockGetIconPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIconPosDataForUdmQuery>())).Returns(queuedPosData.Dequeue);
            mockItemPriceEntityGenerator.Setup(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>())).Returns(new List<ItemPriceModel>());
            mockItemLinkEntityGenerator.Setup(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>())).Returns(new List<ItemLinkModel>());

            // When.
            processorModule.Execute();

            // Then.
            mockItemLinkEntityGenerator.Verify(eg => eg.SaveEntities(It.IsAny<List<ItemLinkModel>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessDataForUdm_PosDataIsMarkedAndReady_ItemPriceEntitiesShouldBeBuilt()
        {
            // Given.
            var mockPosData = new List<IRMAPush> { new TestIrmaPushBuilder() };
            var mockEmptyPosData = new List<IRMAPush>();

            var queuedPosData = new Queue<List<IRMAPush>>();
            queuedPosData.Enqueue(mockPosData);
            queuedPosData.Enqueue(mockEmptyPosData);

            mockGetIconPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIconPosDataForUdmQuery>())).Returns(queuedPosData.Dequeue);
            mockItemPriceEntityGenerator.Setup(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>())).Returns(new List<ItemPriceModel>());
            mockItemLinkEntityGenerator.Setup(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>())).Returns(new List<ItemLinkModel>());

            // When.
            processorModule.Execute();

            // Then.
            mockItemPriceEntityGenerator.Verify(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>()), Times.Once);
        }

        [TestMethod]
        public void ProcessDataForUdm_ItemPriceEntitiesAreBuilt_ItemPriceEntitiesShouldBeSaved()
        {
            // Given.
            var mockPosData = new List<IRMAPush> { new TestIrmaPushBuilder() };
            var mockEmptyPosData = new List<IRMAPush>();

            var queuedPosData = new Queue<List<IRMAPush>>();
            queuedPosData.Enqueue(mockPosData);
            queuedPosData.Enqueue(mockEmptyPosData);

            mockGetIconPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIconPosDataForUdmQuery>())).Returns(queuedPosData.Dequeue);
            mockItemPriceEntityGenerator.Setup(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>())).Returns(new List<ItemPriceModel> { new ItemPriceModel() });
            mockItemLinkEntityGenerator.Setup(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>())).Returns(new List<ItemLinkModel>());

            // When.
            processorModule.Execute();

            // Then.
            mockItemPriceEntityGenerator.Verify(eg => eg.SaveEntities(It.IsAny<List<ItemPriceModel>>()), Times.Once);
        }

        [TestMethod]
        public void ProcessDataForUdm_PosDataHasScanCodeDeauthorizeChangeType_BulkSubscriptionDeleteServiceShouldNotBeCalled()
        {
            // Given.
            var mockPosData = new List<IRMAPush> { new TestIrmaPushBuilder().WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeDeauthorization) };
            var mockEmptyPosData = new List<IRMAPush>();

            var queuedPosData = new Queue<List<IRMAPush>>();
            queuedPosData.Enqueue(mockPosData);
            queuedPosData.Enqueue(mockEmptyPosData);

            mockGetIconPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIconPosDataForUdmQuery>())).Returns(queuedPosData.Dequeue);
            mockItemPriceEntityGenerator.Setup(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>())).Returns(new List<ItemPriceModel>());
            mockItemLinkEntityGenerator.Setup(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>())).Returns(new List<ItemLinkModel>());
            mockGetSubscriptionsQueryHandler.Setup(q => q.Execute(It.IsAny<GetIrmaItemSubscriptionsQuery>())).Returns(new List<IRMAItemSubscription> { new TestIrmaItemSubscriptionBuilder() });

            // When.
            processorModule.Execute();

            // Then.
            mockItemSubscriptionDeleteService.Verify(eg => eg.DeleteEntitiesBulk(It.IsAny<List<IRMAItemSubscription>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessDataForUdm_PosDataHasScanCodeDeleteChangeType_BulkSubscriptionDeleteServiceShouldBeCalled()
        {
            // Given.
            var mockPosData = new List<IRMAPush> { new TestIrmaPushBuilder().WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeDelete) };
            var mockEmptyPosData = new List<IRMAPush>();

            var queuedPosData = new Queue<List<IRMAPush>>();
            queuedPosData.Enqueue(mockPosData);
            queuedPosData.Enqueue(mockEmptyPosData);

            mockGetIconPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIconPosDataForUdmQuery>())).Returns(queuedPosData.Dequeue);
            mockItemPriceEntityGenerator.Setup(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>())).Returns(new List<ItemPriceModel>());
            mockItemLinkEntityGenerator.Setup(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>())).Returns(new List<ItemLinkModel>());
            mockGetSubscriptionsQueryHandler.Setup(q => q.Execute(It.IsAny<GetIrmaItemSubscriptionsQuery>())).Returns(new List<IRMAItemSubscription> { new TestIrmaItemSubscriptionBuilder() });

            // When.
            processorModule.Execute();

            // Then.
            mockItemSubscriptionDeleteService.Verify(eg => eg.DeleteEntitiesBulk(It.IsAny<List<IRMAItemSubscription>>()), Times.Once);
        }

        [TestMethod]
        public void ProcessDataForUdm_PosDataHasAnyChangeTypeOtherThanDeleteOrDeauthorize_BulkSubscriptionDeleteServiceShouldNotBeCalled()
        {
            // Given.
            var mockPosData = new List<IRMAPush>
            {
                new TestIrmaPushBuilder().WithChangeType(Constants.IrmaPushChangeTypes.ItemLocaleAttributeChange),
                new TestIrmaPushBuilder().WithChangeType(Constants.IrmaPushChangeTypes.NonRegularPriceChange),
                new TestIrmaPushBuilder().WithChangeType(Constants.IrmaPushChangeTypes.RegularPriceChange),
                new TestIrmaPushBuilder().WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeAdd),
                new TestIrmaPushBuilder().WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeAuthorization)
            };

            var mockEmptyPosData = new List<IRMAPush>();

            var queuedPosData = new Queue<List<IRMAPush>>();
            queuedPosData.Enqueue(mockPosData);
            queuedPosData.Enqueue(mockEmptyPosData);

            mockGetIconPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIconPosDataForUdmQuery>())).Returns(queuedPosData.Dequeue);
            mockItemPriceEntityGenerator.Setup(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>())).Returns(new List<ItemPriceModel>());
            mockItemLinkEntityGenerator.Setup(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>())).Returns(new List<ItemLinkModel>());

            // When.
            processorModule.Execute();

            // Then.
            mockItemSubscriptionDeleteService.Verify(eg => eg.DeleteEntitiesBulk(It.IsAny<List<IRMAItemSubscription>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessDataForUdm_BulkSubscriptionDeleteThrowsException_RowByRowSubscriptionDeleteServiceShouldBeCalled()
        {
            // Given.
            var mockPosData = new List<IRMAPush> { new TestIrmaPushBuilder().WithChangeType(Constants.IrmaPushChangeTypes.ScanCodeDelete) };
            var mockEmptyPosData = new List<IRMAPush>();

            var queuedPosData = new Queue<List<IRMAPush>>();
            queuedPosData.Enqueue(mockPosData);
            queuedPosData.Enqueue(mockEmptyPosData);

            mockGetIconPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIconPosDataForUdmQuery>())).Returns(queuedPosData.Dequeue);
            mockItemPriceEntityGenerator.Setup(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>())).Returns(new List<ItemPriceModel>());
            mockItemLinkEntityGenerator.Setup(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>())).Returns(new List<ItemLinkModel>());
            mockGetSubscriptionsQueryHandler.Setup(q => q.Execute(It.IsAny<GetIrmaItemSubscriptionsQuery>())).Returns(new List<IRMAItemSubscription> { new TestIrmaItemSubscriptionBuilder() });
            mockItemSubscriptionDeleteService.Setup(s => s.DeleteEntitiesBulk(It.IsAny<List<IRMAItemSubscription>>())).Throws(new Exception());

            // When.
            processorModule.Execute();

            // Then.
            mockItemSubscriptionDeleteService.Verify(eg => eg.DeleteEntitiesBulk(It.IsAny<List<IRMAItemSubscription>>()), Times.Once);
            mockItemSubscriptionDeleteService.Verify(eg => eg.DeleteEntitiesRowByRow(It.IsAny<List<IRMAItemSubscription>>()), Times.Once);
        }

        [TestMethod]
        public void ProcessDataForUdm_NoItemPriceEntitiesAreSuccessfullyBuilt_ItemPriceEntitiesShouldNotBeSaved()
        {
            // Given.
            var mockPosData = new List<IRMAPush> { new TestIrmaPushBuilder() };
            var mockEmptyPosData = new List<IRMAPush>();

            var queuedPosData = new Queue<List<IRMAPush>>();
            queuedPosData.Enqueue(mockPosData);
            queuedPosData.Enqueue(mockEmptyPosData);

            mockGetIconPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIconPosDataForUdmQuery>())).Returns(queuedPosData.Dequeue);
            mockItemPriceEntityGenerator.Setup(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>())).Returns(new List<ItemPriceModel>());
            mockItemLinkEntityGenerator.Setup(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>())).Returns(new List<ItemLinkModel>());

            // When.
            processorModule.Execute();

            // Then.
            mockItemPriceEntityGenerator.Verify(eg => eg.SaveEntities(It.IsAny<List<ItemPriceModel>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessDataForUdm_AfterItemLinkAndItemPriceEntitiesAreGenerated_StagingTableDateShouldBeUpdated()
        {
            // Given.
            var mockPosData = new List<IRMAPush> { new TestIrmaPushBuilder() };
            var mockEmptyPosData = new List<IRMAPush>();

            var queuedPosData = new Queue<List<IRMAPush>>();
            queuedPosData.Enqueue(mockPosData);
            queuedPosData.Enqueue(mockEmptyPosData);

            mockGetIconPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIconPosDataForUdmQuery>())).Returns(queuedPosData.Dequeue);
            mockItemPriceEntityGenerator.Setup(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>())).Returns(new List<ItemPriceModel> { new ItemPriceModel() });
            mockItemLinkEntityGenerator.Setup(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>())).Returns(new List<ItemLinkModel> { new ItemLinkModel() });

            // When.
            processorModule.Execute();

            // Then.
            mockUpdateStagingTableDatesCommandHandler.Verify(c => c.Execute(It.IsAny<UpdateStagingTableDatesForUdmCommand>()), Times.Once);
        }
        [TestMethod]
        public void ProcessDataForUdm_AllRegionsOnGPM__SaveEntitiesForPriceShouldNotBeCalled()
        {
            // Given.
            var mockPosData = new List<IRMAPush> { new TestIrmaPushBuilder() };
            var mockEmptyPosData = new List<IRMAPush>();
            Cache.ClearAll();
            SetUpCache(true);

            var queuedPosData = new Queue<List<IRMAPush>>();
            queuedPosData.Enqueue(mockPosData);
            queuedPosData.Enqueue(mockEmptyPosData);

            mockGetIconPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIconPosDataForUdmQuery>())).Returns(queuedPosData.Dequeue);
            mockItemPriceEntityGenerator.Setup(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>())).Returns(new List<ItemPriceModel> { new ItemPriceModel() });
            mockItemLinkEntityGenerator.Setup(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>())).Returns(new List<ItemLinkModel> { new ItemLinkModel() });

            // When.
            processorModule.Execute();

            // Then.
            mockItemPriceEntityGenerator.Verify(eg => eg.SaveEntities(It.IsAny<List<ItemPriceModel>>()), Times.Never);
        }

        [TestMethod]
        public void ProcessDataForUdm_NoRegionOnGPM__SaveEntitiesForPriceShouldBeCalled()
        {
            // Given.
            var mockPosData = new List<IRMAPush> { new TestIrmaPushBuilder() };
            var mockEmptyPosData = new List<IRMAPush>();

            var queuedPosData = new Queue<List<IRMAPush>>();
            queuedPosData.Enqueue(mockPosData);
            queuedPosData.Enqueue(mockEmptyPosData);

            mockGetIconPosDataQueryHandler.Setup(q => q.Execute(It.IsAny<GetIconPosDataForUdmQuery>())).Returns(queuedPosData.Dequeue);
            mockItemPriceEntityGenerator.Setup(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>())).Returns(new List<ItemPriceModel> { new ItemPriceModel() });
            mockItemLinkEntityGenerator.Setup(eg => eg.BuildEntities(It.IsAny<List<IRMAPush>>())).Returns(new List<ItemLinkModel> { new ItemLinkModel() });

            // When.
            processorModule.Execute();

            // Then.
            mockItemPriceEntityGenerator.Verify(eg => eg.SaveEntities(It.IsAny<List<ItemPriceModel>>()), Times.Once);
        }
        private void SetUpCache(Boolean isRegionGPM)
        {
            Cache.regionCodeToGPMInstanceDataFlag.Add("FL", isRegionGPM);
            Cache.regionCodeToGPMInstanceDataFlag.Add("MA", isRegionGPM);
            Cache.regionCodeToGPMInstanceDataFlag.Add("MW", isRegionGPM);
            Cache.regionCodeToGPMInstanceDataFlag.Add("NA", isRegionGPM);
            Cache.regionCodeToGPMInstanceDataFlag.Add("NC", isRegionGPM);
            Cache.regionCodeToGPMInstanceDataFlag.Add("NE", isRegionGPM);
            Cache.regionCodeToGPMInstanceDataFlag.Add("PN", isRegionGPM);
            Cache.regionCodeToGPMInstanceDataFlag.Add("RM", isRegionGPM);
            Cache.regionCodeToGPMInstanceDataFlag.Add("SO", isRegionGPM);
            Cache.regionCodeToGPMInstanceDataFlag.Add("SP", isRegionGPM);
            Cache.regionCodeToGPMInstanceDataFlag.Add("SW", isRegionGPM);
        }
    }
}
