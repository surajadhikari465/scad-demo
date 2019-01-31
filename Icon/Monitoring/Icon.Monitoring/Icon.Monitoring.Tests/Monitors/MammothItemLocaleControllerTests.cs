using Icon.Logging;
using Icon.Monitoring.Common;
using Icon.Monitoring.Common.Enums;
using Icon.Monitoring.Common.Opsgenie;
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
    public class MammothItemLocaleControllerMonitorTests
    {
        private Mock<IOpsgenieTrigger> mockOpsgenieTrigger;
        private Mock<IMonitorSettings> mockSettings;
        private List<IrmaRegions> allRegions;
        private Mock<ILogger> mockLogger;


        [TestInitialize]
        public void Initialize()
        {
            this.allRegions = Enum.GetValues(typeof(IrmaRegions)).Cast<IrmaRegions>().ToList();
            this.mockSettings = new Mock<IMonitorSettings>();
            this.mockOpsgenieTrigger = new Mock<IOpsgenieTrigger>();
            MammothItemLocaleChangeQueueCache.IrmaRegionMapper.Clear();
            this.mockLogger = new Mock<ILogger>();

            MammothItemLocaleChangeQueueCache.IrmaRegionMapper.Add(IrmaRegions.FL, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 });
            MammothItemLocaleChangeQueueCache.IrmaRegionMapper.Add(IrmaRegions.MA, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 });
            MammothItemLocaleChangeQueueCache.IrmaRegionMapper.Add(IrmaRegions.MW, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 });
            MammothItemLocaleChangeQueueCache.IrmaRegionMapper.Add(IrmaRegions.NA, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 });
            MammothItemLocaleChangeQueueCache.IrmaRegionMapper.Add(IrmaRegions.NC, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 });
            MammothItemLocaleChangeQueueCache.IrmaRegionMapper.Add(IrmaRegions.NE, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 });
            MammothItemLocaleChangeQueueCache.IrmaRegionMapper.Add(IrmaRegions.PN, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 });
            MammothItemLocaleChangeQueueCache.IrmaRegionMapper.Add(IrmaRegions.RM, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 });
            MammothItemLocaleChangeQueueCache.IrmaRegionMapper.Add(IrmaRegions.SO, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 });
            MammothItemLocaleChangeQueueCache.IrmaRegionMapper.Add(IrmaRegions.SP, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 });
            MammothItemLocaleChangeQueueCache.IrmaRegionMapper.Add(IrmaRegions.SW, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 });
            MammothItemLocaleChangeQueueCache.IrmaRegionMapper.Add(IrmaRegions.UK, new QueueData { LastMessageQueueId = 0, NumberOfTimesMatched = 0 });
        }

        [TestCleanup]
        public void Cleanup()
        {
            MammothItemLocaleChangeQueueCache.IrmaRegionMapper.Clear();
        }

        [TestMethod]
        public void CheckStatusAndNotifyWhenQueueNotProcessedNotPaged_ThenPage()
        {
            MammothItemLocaleChangeQueueCache.IrmaRegionMapper[IrmaRegions.FL] = new QueueData { LastMessageQueueId = 100, NumberOfTimesMatched = 0 };

            //set up query
            var ItemLocaleQueueQuery = new Mock<IQueryByRegionHandler<GetMammothItemLocaleChangeQueueIdQueryParameters, int>>();
            ItemLocaleQueueQuery.Setup(q => q.Search(It.IsAny<GetMammothItemLocaleChangeQueueIdQueryParameters>()))
                          .Returns(100);

            //create monitor
            var testee = new MammothItemLocaleControllerMonitor(
                this.mockSettings.Object,
                this.mockOpsgenieTrigger.Object,
                ItemLocaleQueueQuery.Object,
                this.mockLogger.Object);

            // When
            testee.CheckStatusAndNotify();

            // Then
            this.mockOpsgenieTrigger.Verify(
                x => x.TriggerAlert(
                      It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()),
                Times.Once);

            Assert.AreEqual(MammothItemLocaleChangeQueueCache.IrmaRegionMapper[IrmaRegions.FL].NumberOfTimesMatched, 1);
        }

        [TestMethod]
        public void CheckStatusAndNotifyWhenQueueProcessed_NotPage()
        {

            //set up query
            var ItemLocaleQueueQuery = new Mock<IQueryByRegionHandler<GetMammothItemLocaleChangeQueueIdQueryParameters, int>>();
            ItemLocaleQueueQuery.Setup(q => q.Search(It.IsAny<GetMammothItemLocaleChangeQueueIdQueryParameters>()))
                          .Returns(0);

            //create monitor
            var testee = new MammothItemLocaleControllerMonitor(
                this.mockSettings.Object,
                this.mockOpsgenieTrigger.Object,
                ItemLocaleQueueQuery.Object,
                this.mockLogger.Object);

            // When
            testee.CheckStatusAndNotify();

            // Then
            this.mockOpsgenieTrigger.Verify(
                x => x.TriggerAlert(
                     It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()),
                Times.Never);
        }

        [TestMethod]
        public void CheckStatusAndNotifyWhenQueueNotProcessedAndPaged_NotPage()
        {
            MammothItemLocaleChangeQueueCache.IrmaRegionMapper[IrmaRegions.FL] = new QueueData { LastMessageQueueId = 100, NumberOfTimesMatched = 1 };
            //set up query
            var ItemLocaleQueueQuery = new Mock<IQueryByRegionHandler<GetMammothItemLocaleChangeQueueIdQueryParameters, int>>();
            ItemLocaleQueueQuery.Setup(q => q.Search(It.IsAny<GetMammothItemLocaleChangeQueueIdQueryParameters>()))
                          .Returns(100);

            //create monitor
            var testee = new MammothItemLocaleControllerMonitor(
                this.mockSettings.Object,
                this.mockOpsgenieTrigger.Object,
                ItemLocaleQueueQuery.Object,
                this.mockLogger.Object);

            // When
            testee.CheckStatusAndNotify();

            // Then
            this.mockOpsgenieTrigger.Verify(
                x => x.TriggerAlert(
                     It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()),
                Times.Never);
            Assert.AreEqual(MammothItemLocaleChangeQueueCache.IrmaRegionMapper[IrmaRegions.FL].NumberOfTimesMatched, 2);
        }

        [TestMethod]
        public void CheckStatusAndNotifyWhenTwoRegionsNotProcessedNotPaged_ThenPageOnce()
        {
            MammothItemLocaleChangeQueueCache.IrmaRegionMapper[IrmaRegions.FL] = new QueueData { LastMessageQueueId = 100, NumberOfTimesMatched = 0 };
            MammothItemLocaleChangeQueueCache.IrmaRegionMapper[IrmaRegions.MA] = new QueueData { LastMessageQueueId = 200, NumberOfTimesMatched = 0 };

            //set up query
            var ItemLocaleQueueQuery = new Mock<IQueryByRegionHandler<GetMammothItemLocaleChangeQueueIdQueryParameters, int>>();
            ItemLocaleQueueQuery.Setup(q => q.Search(It.IsAny<GetMammothItemLocaleChangeQueueIdQueryParameters>()))
                          .Returns(new Queue<int>(new[] { 100, 200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }).Dequeue);

            //create monitor
            var testee = new MammothItemLocaleControllerMonitor(
                this.mockSettings.Object,
                this.mockOpsgenieTrigger.Object,
                ItemLocaleQueueQuery.Object,
                this.mockLogger.Object);

            // When
            testee.CheckStatusAndNotify();

            // Then
            this.mockOpsgenieTrigger.Verify(
                x => x.TriggerAlert(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()),
                Times.Once);

            Assert.AreEqual(MammothItemLocaleChangeQueueCache.IrmaRegionMapper[IrmaRegions.FL].NumberOfTimesMatched, 1);
            Assert.AreEqual(MammothItemLocaleChangeQueueCache.IrmaRegionMapper[IrmaRegions.MA].NumberOfTimesMatched, 1);
        }

        [TestMethod]
        public void CheckStatusAndNotifyWhenTwoRegionsNotProcessedAndOnePaged_ThenPageOnceAndBothListed()
        {
            MammothItemLocaleChangeQueueCache.IrmaRegionMapper[IrmaRegions.FL] = new QueueData { LastMessageQueueId = 100, NumberOfTimesMatched = 1 };
            MammothItemLocaleChangeQueueCache.IrmaRegionMapper[IrmaRegions.MA] = new QueueData { LastMessageQueueId = 200, NumberOfTimesMatched = 0 };

            //set up query
            var ItemLocaleQueueQuery = new Mock<IQueryByRegionHandler<GetMammothItemLocaleChangeQueueIdQueryParameters, int>>();
            ItemLocaleQueueQuery.Setup(q => q.Search(It.IsAny<GetMammothItemLocaleChangeQueueIdQueryParameters>()))
                          .Returns(new Queue<int>(new[] { 100, 200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }).Dequeue);

            //create monitor
            var testee = new MammothItemLocaleControllerMonitor(
                this.mockSettings.Object,
                this.mockOpsgenieTrigger.Object,
                ItemLocaleQueueQuery.Object,
                this.mockLogger.Object);

            // When
            testee.CheckStatusAndNotify();

            // Then
            this.mockOpsgenieTrigger.Verify(
                x => x.TriggerAlert(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()),
                Times.Once);

            Assert.AreEqual(MammothItemLocaleChangeQueueCache.IrmaRegionMapper[IrmaRegions.FL].NumberOfTimesMatched, 2);
            Assert.AreEqual(MammothItemLocaleChangeQueueCache.IrmaRegionMapper[IrmaRegions.MA].NumberOfTimesMatched, 1);
        }
    }
}
