using Icon.Logging;
using Icon.Monitoring.Common;
using Icon.Monitoring.Common.Constants;
using Icon.Monitoring.Common.Enums;
using Icon.Monitoring.Common.Opsgenie;
using Icon.Monitoring.Common.Settings;
using Icon.Monitoring.DataAccess.Model;
using Icon.Monitoring.DataAccess.Queries;
using Icon.Monitoring.Monitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Monitoring.Tests.Monitors
{
    [TestClass]
    public class InStockDequeueProcessMonitorTests
    {

        private Mock<IOpsgenieTrigger> mockOpsgenieTrigger;
        private Mock<IMonitorSettings> mockSettings;
        private Mock<IInStockDequeueProcessMonitorSettings> mockInStockDequeueProcessMonitorSettings;
        private List<IrmaRegions> allRegions;
        private Mock<ILogger> mockLogger;
        DateTime? currentDate = DateTime.Now;

        [TestInitialize]
        public void Initialize()
        {
            this.allRegions = Enum.GetValues(typeof(IrmaRegions)).Cast<IrmaRegions>().ToList();
            this.mockSettings = new Mock<IMonitorSettings>();
            this.mockInStockDequeueProcessMonitorSettings = new Mock<IInStockDequeueProcessMonitorSettings>();
            this.mockOpsgenieTrigger = new Mock<IOpsgenieTrigger>();
            this.mockLogger = new Mock<ILogger>();

        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void InStockDequeueProcessNotProcessing_ThenAlert()
        {
            // Given

            var inStockQuery = new Mock<IQueryByRegionHandler<GetInStockOldestUnprocessedRecordDateByQueueTableParameters, DateTime?>>();
            inStockQuery.Setup(q => q.Search(It.IsAny<GetInStockOldestUnprocessedRecordDateByQueueTableParameters>()))
                          .Returns(DateTime.Now.AddDays(-1));

            mockInStockDequeueProcessMonitorSettings.SetupGet(m => m.InStockDequeueProcessRegions).Returns( new List<string> { "Sw" });
            mockInStockDequeueProcessMonitorSettings.SetupGet(m => m.NumberOfMaximumMinutesRecordCanBeInUnprocessedStatus).Returns(10);
            var InStockMonitor = new InStockDequeueProcessMonitor(
                 this.mockSettings.Object,
                 this.mockInStockDequeueProcessMonitorSettings.Object,
                 inStockQuery.Object,
                this.mockOpsgenieTrigger.Object,
                this.mockLogger.Object
               );

            // When
            InStockMonitor.CheckStatusAndNotify();

            // Then
            this.mockOpsgenieTrigger.Verify(
                x => x.TriggerAlert(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()),
                Times.Once);
        }

        //[TestMethod]
        public void InStockDequeueProcessProcessing_ThenNoAlert()
        {
            // Given

            var inStockQuery = new Mock<IQueryByRegionHandler<GetInStockOldestUnprocessedRecordDateByQueueTableParameters, DateTime?>>();
            inStockQuery.Setup(q => q.Search(It.IsAny<GetInStockOldestUnprocessedRecordDateByQueueTableParameters>()))
                          .Returns(DateTime.Now);

            mockInStockDequeueProcessMonitorSettings.SetupGet(m => m.InStockDequeueProcessRegions).Returns(new List<string> { "Sw" });
            mockInStockDequeueProcessMonitorSettings.SetupGet(m => m.NumberOfMaximumMinutesRecordCanBeInUnprocessedStatus).Returns(10);
            var InStockMonitor = new InStockDequeueProcessMonitor(
                 this.mockSettings.Object,
                 this.mockInStockDequeueProcessMonitorSettings.Object,
                 inStockQuery.Object,
                this.mockOpsgenieTrigger.Object,
                this.mockLogger.Object
               );

            // When
            InStockMonitor.CheckStatusAndNotify();

            // Then
            this.mockOpsgenieTrigger.Verify(
                x => x.TriggerAlert(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()),
                Times.Never);
        }
    }
}