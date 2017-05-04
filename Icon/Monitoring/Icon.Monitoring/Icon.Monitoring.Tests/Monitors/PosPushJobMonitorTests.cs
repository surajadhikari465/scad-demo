using Icon.Logging;
using Icon.Monitoring.Common;
using Icon.Monitoring.Common.Constants;
using Icon.Monitoring.Common.Enums;
using Icon.Monitoring.Common.PagerDuty;
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
    public class PosPushJobMonitorTests
    {
        private const string RunningStatus = "RUNNING";
        private const string CompletedStatus = "COMPLETED";

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
            JobStatusCache.PagerDutyTracker.Clear();
            this.mockLogger = new Mock<ILogger>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            JobStatusCache.PagerDutyTracker.Clear();
        }

        [TestMethod]
        public void CheckStatusAndNotifyWhenRunningTooLongNotPaged_ThenPage()
        {
            // Given
            var posPushJobStatusFirstRegionShouldPage = this.allRegions.Select((r, i) => new JobStatus
            {
                Classname = IrmaJobClassNames.POSPushJob,
                LastRun = DateTime.Now.Subtract(TimeSpan.FromMinutes(120)),
                Region = r,
                Status = i > 0 ? CompletedStatus : RunningStatus
            }).ToList();

            var jobStatusQuerySearchQueue = new Queue<JobStatus>(posPushJobStatusFirstRegionShouldPage);

            var jobStatusQuery = new Mock<IQueryByRegionHandler<GetIrmaJobStatusQueryParameters, JobStatus>>();
            jobStatusQuery.Setup(q => q.Search(It.IsAny<GetIrmaJobStatusQueryParameters>()))
                          .Returns(jobStatusQuerySearchQueue.Dequeue);


            var testee = new PosPushJobMonitor(
                this.mockSettings.Object,
                jobStatusQuery.Object,
                this.mockPagerDutyTrigger.Object,
                this.mockLogger.Object);

            // When
            testee.CheckStatusAndNotify();

            // Then
            this.mockPagerDutyTrigger.Verify(
                x => x.TriggerIncident(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()),
                Times.Once);

            Assert.AreEqual(
                posPushJobStatusFirstRegionShouldPage.First(x => x.Region == IrmaRegions.FL).LastRun,
                JobStatusCache.PagerDutyTracker[IrmaJobClassNames.POSPushJob + "_" + IrmaRegions.FL]);
        }

        [TestMethod]
        public void CheckStatusAndNotifyWhenNotRunningTooLongNotPaged_ThenDontPage()
        {
            // Given
            var posPushJobStatusFirstRegionShouldPage = this.allRegions.Select((r, i) => new JobStatus
            {
                Classname = IrmaJobClassNames.POSPushJob,
                LastRun = DateTime.Now.Subtract(TimeSpan.FromMinutes(20)),
                Region = r,
                Status = i > 0 ? CompletedStatus : RunningStatus
            }).ToList();

            var jobStatusQuerySearchQueue = new Queue<JobStatus>(posPushJobStatusFirstRegionShouldPage);

            var jobStatusQuery = new Mock<IQueryByRegionHandler<GetIrmaJobStatusQueryParameters, JobStatus>>();
            jobStatusQuery.Setup(q => q.Search(It.IsAny<GetIrmaJobStatusQueryParameters>()))
                          .Returns(jobStatusQuerySearchQueue.Dequeue);

            var testee = new PosPushJobMonitor(
                this.mockSettings.Object,
                jobStatusQuery.Object,
                this.mockPagerDutyTrigger.Object,
                this.mockLogger.Object);

            // When
            testee.CheckStatusAndNotify();

            // Then
            this.mockPagerDutyTrigger.Verify(
                x => x.TriggerIncident(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()),
                Times.Never);

            Assert.AreEqual(
                JobStatusCache.PagerDutyTracker.Count, 0);
        }

        [TestMethod]
        public void CheckStatusAndNotifyWhenRunningTooLongPaged_ThenDontPage()
        {
            // Given
            var runTime = DateTime.Now.Subtract(TimeSpan.FromMinutes(120));
            var posPushJobStatusFirstRegionShouldPage = this.allRegions.Select((r, i) => new JobStatus
            {
                Classname = IrmaJobClassNames.POSPushJob,
                LastRun = runTime,
                Region = r,
                Status = i > 0 ? CompletedStatus : RunningStatus
            }).ToList();

            var jobStatusQuerySearchQueue = new Queue<JobStatus>(posPushJobStatusFirstRegionShouldPage);

            var jobStatusQuery = new Mock<IQueryByRegionHandler<GetIrmaJobStatusQueryParameters, JobStatus>>();
            jobStatusQuery.Setup(q => q.Search(It.IsAny<GetIrmaJobStatusQueryParameters>()))
                          .Returns(jobStatusQuerySearchQueue.Dequeue);

            var testee = new PosPushJobMonitor(
                this.mockSettings.Object,
                jobStatusQuery.Object,
                this.mockPagerDutyTrigger.Object,
                this.mockLogger.Object);

            JobStatusCache.PagerDutyTracker.Add(IrmaJobClassNames.POSPushJob + "_" + "FL", runTime);

            // When
            testee.CheckStatusAndNotify();

            // Then
            this.mockPagerDutyTrigger.Verify(
                x => x.TriggerIncident(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()),
                Times.Never);

            Assert.AreEqual(
                JobStatusCache.PagerDutyTracker.Count, 1);
        }

        [TestMethod]
        public void CheckStatusAndNotifyWhenNotRunningTooLongNotPaged_ThenPage()
        {
            // Given
            var posPushJobStatusFirstRegionShouldPage = this.allRegions.Select((r, i) => new JobStatus
            {
                Classname = IrmaJobClassNames.POSPushJob,
                LastRun = DateTime.Now.Subtract(TimeSpan.FromMinutes(120)),
                Region = r,
                Status = CompletedStatus
            }).ToList();

            var jobStatusQuerySearchQueue = new Queue<JobStatus>(posPushJobStatusFirstRegionShouldPage);

            var jobStatusQuery = new Mock<IQueryByRegionHandler<GetIrmaJobStatusQueryParameters, JobStatus>>();
            jobStatusQuery.Setup(q => q.Search(It.IsAny<GetIrmaJobStatusQueryParameters>()))
                          .Returns(jobStatusQuerySearchQueue.Dequeue);

            var testee = new PosPushJobMonitor(
                this.mockSettings.Object,
                jobStatusQuery.Object,
                this.mockPagerDutyTrigger.Object,
                this.mockLogger.Object);

            // When
            testee.CheckStatusAndNotify();

            // Then
            this.mockPagerDutyTrigger.Verify(
                x => x.TriggerIncident(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()),
                Times.Never);

            Assert.AreEqual(
                 JobStatusCache.PagerDutyTracker.Count, 0);
        }

        [TestMethod]
        public void CheckStatusAndNotifyWhenTwoRunningTooLongNotPaged_ThenPage()
        {
            // Given
            var runTime = DateTime.Now.Subtract(TimeSpan.FromMinutes(120));
            var posPushJobStatusFirstRegionShouldPage = this.allRegions.Select((r, i) => new JobStatus
            {
                Classname = IrmaJobClassNames.POSPushJob,
                LastRun = runTime,
                Region = r,
                Status = i > 1 ? CompletedStatus : RunningStatus
            }).ToList();

            var allRegionsQueue = new Queue<IrmaRegions>();
            allRegionsQueue.Enqueue(IrmaRegions.FL); // Because moq runs the call back before the method is called. :(
            this.allRegions.ForEach(allRegionsQueue.Enqueue);

            var jobStatusQuerySearchQueue = new Queue<JobStatus>(posPushJobStatusFirstRegionShouldPage);

            var jobStatusQuery = new Mock<IQueryByRegionHandler<GetIrmaJobStatusQueryParameters, JobStatus>>();

            jobStatusQuery.Setup(q => q.Search(It.IsAny<GetIrmaJobStatusQueryParameters>()))
                          .Callback(() => allRegionsQueue.Dequeue())
                          .Returns(jobStatusQuerySearchQueue.Dequeue);

            jobStatusQuery.SetupGet(q => q.TargetRegion).Returns(allRegionsQueue.Peek);

            var testee = new PosPushJobMonitor(
                this.mockSettings.Object,
                jobStatusQuery.Object,
                this.mockPagerDutyTrigger.Object,
                this.mockLogger.Object);

            // When
            testee.CheckStatusAndNotify();

            // Then
            this.mockPagerDutyTrigger.Verify(
                x => x.TriggerIncident(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()),
                Times.Once);

            Assert.AreEqual(
                posPushJobStatusFirstRegionShouldPage.First(x => x.Region == IrmaRegions.FL).LastRun,
                JobStatusCache.PagerDutyTracker[IrmaJobClassNames.POSPushJob + "_" + IrmaRegions.FL]);

            Assert.AreEqual(
                posPushJobStatusFirstRegionShouldPage.First(x => x.Region == IrmaRegions.MA).LastRun,
                JobStatusCache.PagerDutyTracker[IrmaJobClassNames.POSPushJob + "_" + IrmaRegions.MA]);

        }
    }
}
