using Icon.Logging;
using Icon.Monitoring.Common;
using Icon.Monitoring.Common.Enums;
using Icon.Monitoring.Common.PagerDuty;
using Icon.Monitoring.Common.Settings;
using Icon.Monitoring.DataAccess.Queries;
using Icon.Monitoring.Monitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Monitoring.Tests.Monitors
{
    [TestClass]
    public class MammothPriceControllerMonitorTests
    {
        private Mock<IPagerDutyTrigger> mockPagerDutyTrigger;
        private Mock<IMonitorSettings> mockSettings;
        private List<IrmaRegions> allRegions;
        private Mock<ILogger> mockLogger;


        [TestInitialize]
        public void Initialize()
        {
            this.allRegions = Enum.GetValues(typeof(IrmaRegions)).Cast<IrmaRegions>().ToList();
            this.mockSettings = new Mock<IMonitorSettings>();
            this.mockPagerDutyTrigger = new Mock<IPagerDutyTrigger>();
            MammothPriceChangeQueueCache.IrmaRegionMapper.Clear();
            this.mockLogger = new Mock<ILogger>();

            MammothPriceChangeQueueCache.IrmaRegionMapper.Add(IrmaRegions.FL, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 });
            MammothPriceChangeQueueCache.IrmaRegionMapper.Add(IrmaRegions.MA, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 });
            MammothPriceChangeQueueCache.IrmaRegionMapper.Add(IrmaRegions.MW, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 });
            MammothPriceChangeQueueCache.IrmaRegionMapper.Add(IrmaRegions.NA, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 });
            MammothPriceChangeQueueCache.IrmaRegionMapper.Add(IrmaRegions.NC, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 });
            MammothPriceChangeQueueCache.IrmaRegionMapper.Add(IrmaRegions.NE, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 });
            MammothPriceChangeQueueCache.IrmaRegionMapper.Add(IrmaRegions.PN, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 });
            MammothPriceChangeQueueCache.IrmaRegionMapper.Add(IrmaRegions.RM, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 });
            MammothPriceChangeQueueCache.IrmaRegionMapper.Add(IrmaRegions.SO, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 });
            MammothPriceChangeQueueCache.IrmaRegionMapper.Add(IrmaRegions.SP, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 });
            MammothPriceChangeQueueCache.IrmaRegionMapper.Add(IrmaRegions.SW, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 });
            MammothPriceChangeQueueCache.IrmaRegionMapper.Add(IrmaRegions.UK, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 });
        }

        [TestCleanup]
        public void Cleanup()
        {
            MammothPriceChangeQueueCache.IrmaRegionMapper.Clear();
        }

        [TestMethod]
        public void CheckStatusAndNotifyWhenQueueNotProcessedNotPaged_ThenPage()
        {
            MammothPriceChangeQueueCache.IrmaRegionMapper[IrmaRegions.FL] = new QueueData { LastMessageQueueId = 100, NumberOfTimesMatched = 0};
         
            //set up query
            var priceChangeQueueQuery = new Mock<IQueryByRegionHandler<GetMammothPriceChangeQueueIdQueryParameters, int>>();
            priceChangeQueueQuery.Setup(q => q.Search(It.IsAny<GetMammothPriceChangeQueueIdQueryParameters>()))
                          .Returns(100);

            //create monitor
            var testee = new MammothPriceControllerMonitor(
                this.mockSettings.Object,
                this.mockPagerDutyTrigger.Object,
                priceChangeQueueQuery.Object,
                this.mockLogger.Object);

            // When
            testee.CheckStatusAndNotify();

            // Then
            this.mockPagerDutyTrigger.Verify(
                x => x.TriggerIncident(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()),
                Times.Once);

            Assert.AreEqual(MammothPriceChangeQueueCache.IrmaRegionMapper[IrmaRegions.FL].NumberOfTimesMatched,1);
        }

        [TestMethod]
        public void CheckStatusAndNotifyWhenQueueProcessed_NotPage()
        {
           
            //set up query
            var priceChangeQueueQuery = new Mock<IQueryByRegionHandler<GetMammothPriceChangeQueueIdQueryParameters, int>>();
            priceChangeQueueQuery.Setup(q => q.Search(It.IsAny<GetMammothPriceChangeQueueIdQueryParameters>()))
                          .Returns(0);

            //create monitor
            var testee = new MammothPriceControllerMonitor(
                this.mockSettings.Object,
                this.mockPagerDutyTrigger.Object,
                priceChangeQueueQuery.Object,
                this.mockLogger.Object);

            // When
            testee.CheckStatusAndNotify();

            // Then
            this.mockPagerDutyTrigger.Verify(
                x => x.TriggerIncident(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()),
                Times.Never);
        }

        [TestMethod]
        public void CheckStatusAndNotifyWhenQueueNotProcessedAndPaged_NotPage()
        {
            MammothPriceChangeQueueCache.IrmaRegionMapper[IrmaRegions.FL] = new QueueData { LastMessageQueueId = 100, NumberOfTimesMatched = 1 };
            //set up query
            var priceChangeQueueQuery = new Mock<IQueryByRegionHandler<GetMammothPriceChangeQueueIdQueryParameters, int>>();
            priceChangeQueueQuery.Setup(q => q.Search(It.IsAny<GetMammothPriceChangeQueueIdQueryParameters>()))
                          .Returns(100);

            //create monitor
            var testee = new MammothPriceControllerMonitor(
                this.mockSettings.Object,
                this.mockPagerDutyTrigger.Object,
                priceChangeQueueQuery.Object,
                this.mockLogger.Object);

            // When
            testee.CheckStatusAndNotify();

            // Then
            this.mockPagerDutyTrigger.Verify(
                x => x.TriggerIncident(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()),
                Times.Never);
            Assert.AreEqual(MammothPriceChangeQueueCache.IrmaRegionMapper[IrmaRegions.FL].NumberOfTimesMatched, 2);
        }

        [TestMethod]
        public void CheckStatusAndNotifyWhenTwoRegionsNotProcessedNotPaged_ThenPageOnce()
        {
            MammothPriceChangeQueueCache.IrmaRegionMapper[IrmaRegions.FL] = new QueueData { LastMessageQueueId = 100, NumberOfTimesMatched = 0 };
            MammothPriceChangeQueueCache.IrmaRegionMapper[IrmaRegions.MA] = new QueueData { LastMessageQueueId = 200, NumberOfTimesMatched = 0 };

            //set up query
            var priceChangeQueueQuery = new Mock<IQueryByRegionHandler<GetMammothPriceChangeQueueIdQueryParameters, int>>();
            //priceChangeQueueQuery.SetupGet(x => x.TargetRegion).Returns(IrmaRegions.FL);
            priceChangeQueueQuery.Setup(q => q.Search(It.IsAny<GetMammothPriceChangeQueueIdQueryParameters>()))
                          .Returns(new Queue<int>(new[] { 100, 200,0,0,0,0,0,0,0,0,0,0 }).Dequeue);
            //priceChangeQueueQuery.SetupSequence(q => q.Search(It.IsAny<GetMammothPriceChangeQueueIdQueryParameters>())).Returns(100);

            //create monitor
            var testee = new MammothPriceControllerMonitor(
                this.mockSettings.Object,
                this.mockPagerDutyTrigger.Object,
                priceChangeQueueQuery.Object,
                this.mockLogger.Object);

            // When
            testee.CheckStatusAndNotify();

            // Then
            this.mockPagerDutyTrigger.Verify(
                x => x.TriggerIncident(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()),
                Times.Once);

            Assert.AreEqual(MammothPriceChangeQueueCache.IrmaRegionMapper[IrmaRegions.FL].NumberOfTimesMatched, 1);
            Assert.AreEqual(MammothPriceChangeQueueCache.IrmaRegionMapper[IrmaRegions.MA].NumberOfTimesMatched, 1);
        }

        [TestMethod]
        public void CheckStatusAndNotifyWhenTwoRegionsNotProcessedAndOnePaged_ThenPageOnceAndBothListed()
        {
            MammothPriceChangeQueueCache.IrmaRegionMapper[IrmaRegions.FL] = new QueueData { LastMessageQueueId = 100, NumberOfTimesMatched = 1 };
            MammothPriceChangeQueueCache.IrmaRegionMapper[IrmaRegions.MA] = new QueueData { LastMessageQueueId = 200, NumberOfTimesMatched = 0 };

            //set up query
            var priceChangeQueueQuery = new Mock<IQueryByRegionHandler<GetMammothPriceChangeQueueIdQueryParameters, int>>();
            //priceChangeQueueQuery.SetupGet(x => x.TargetRegion).Returns(IrmaRegions.FL);
            priceChangeQueueQuery.Setup(q => q.Search(It.IsAny<GetMammothPriceChangeQueueIdQueryParameters>()))
                          .Returns(new Queue<int>(new[] { 100, 200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }).Dequeue);
            //priceChangeQueueQuery.SetupSequence(q => q.Search(It.IsAny<GetMammothPriceChangeQueueIdQueryParameters>())).Returns(100);

            //create monitor
            var testee = new MammothPriceControllerMonitor(
                this.mockSettings.Object,
                this.mockPagerDutyTrigger.Object,
                priceChangeQueueQuery.Object,
                this.mockLogger.Object);

            // When
            testee.CheckStatusAndNotify();

            // Then
            this.mockPagerDutyTrigger.Verify(
                x => x.TriggerIncident(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()),
                Times.Once);

            Assert.AreEqual(MammothPriceChangeQueueCache.IrmaRegionMapper[IrmaRegions.FL].NumberOfTimesMatched, 2);
            Assert.AreEqual(MammothPriceChangeQueueCache.IrmaRegionMapper[IrmaRegions.MA].NumberOfTimesMatched, 1);
        }
    }
}
