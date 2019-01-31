﻿using Icon.Logging;
using Icon.Monitoring.Common;
using Icon.Monitoring.Common.Opsgenie;
using Icon.Monitoring.Common.Settings;
using Icon.Monitoring.DataAccess.Model;
using Icon.Monitoring.DataAccess.Queries;
using Icon.Monitoring.Monitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Icon.Monitoring.Tests.Monitors
{
    [TestClass]
    public class MammothPrimeAffinityControllerMonitorTests
    {
        private const string JobStatusReady = "ready";
        private const string JobStatusFailed = "failed";

        private MammothPrimeAffinityControllerMonitor monitor;
        private Mock<IMonitorSettings> mockSettings;
        private Mock<IMammothPrimeAffinityControllerMonitorSettings> mockPrimeAffinitySettings;
        private Mock<IQueryHandlerMammoth<GetMammothJobScheduleParameters, JobSchedule>> mockGetMammothJobScheduleQuery;
        private Mock<IOpsgenieTrigger> mockOpsgenieTrigger;
        private Mock<IMonitorCache> mockCache;
        private Mock<ILogger> mockLogger;
        private Dictionary<string, bool> enabledRegions;
        private Dictionary<string, DateTime> regionCompletionUtcTimes;

        [TestInitialize]
        public void Initialize()
        {
            mockSettings = new Mock<IMonitorSettings>();
            mockPrimeAffinitySettings = new Mock<IMammothPrimeAffinityControllerMonitorSettings>();
            mockGetMammothJobScheduleQuery = new Mock<IQueryHandlerMammoth<GetMammothJobScheduleParameters, JobSchedule>>();
            this.mockOpsgenieTrigger = new Mock<IOpsgenieTrigger>();
            mockCache = new Mock<IMonitorCache>();
            mockLogger = new Mock<ILogger>();

            monitor = new MammothPrimeAffinityControllerMonitor(
                mockSettings.Object,
                mockPrimeAffinitySettings.Object,
                mockGetMammothJobScheduleQuery.Object,
                mockOpsgenieTrigger.Object,
                mockCache.Object,
                mockLogger.Object);
            enabledRegions = new Dictionary<string, bool>();
            regionCompletionUtcTimes = new Dictionary<string, DateTime>();
            mockPrimeAffinitySettings.SetupGet(m => m.PrimeAffinityControllerMonitorEnabledByRegion)
                .Returns(enabledRegions);
            mockPrimeAffinitySettings.SetupGet(m => m.PrimeAffinityPsgCompletionUtcTimeByRegion)
                .Returns(regionCompletionUtcTimes);

            monitor.ByPassConfiguredRunInterval = true;
        }

        [TestMethod]
        public void MammothPrimeAffinityControllerMonitorCheckStatusAndNotify_JobCompletedOnTime_DoesNotTriggerAlert()
        {
            //Given
            enabledRegions.Add("FL", true);
            enabledRegions.Add("MA", true);
            enabledRegions.Add("MW", true);
            regionCompletionUtcTimes.Add("FL", DateTime.UtcNow.AddMinutes(-5));
            regionCompletionUtcTimes.Add("MA", DateTime.UtcNow.AddMinutes(-5));
            regionCompletionUtcTimes.Add("MW", DateTime.UtcNow.AddMinutes(-5));
            mockGetMammothJobScheduleQuery.Setup(m => m.Search(It.IsAny<GetMammothJobScheduleParameters>()))
                .Returns(new JobSchedule
                {
                    Enabled = true,
                    LastRunEndDateTimeUtc = DateTime.UtcNow.AddMinutes(-30),
                    Status = JobStatusReady
                });

            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockPrimeAffinitySettings.VerifyGet(m => m.PrimeAffinityControllerMonitorEnabledByRegion, Times.Once);
            mockPrimeAffinitySettings.VerifyGet(m => m.PrimeAffinityPsgCompletionUtcTimeByRegion, Times.Exactly(3));
            mockGetMammothJobScheduleQuery.Verify(m => m.Search(It.IsAny<GetMammothJobScheduleParameters>()), Times.Exactly(3));
            mockOpsgenieTrigger.Verify(m => m.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
            mockLogger.Verify(m => m.Info(It.IsRegex("Mammoth Prime Affinity Controller completed on time.*Region", RegexOptions.None)), Times.Exactly(3));
            mockCache.Verify(m => m.Set(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<DateTimeOffset>()), Times.Never);
        }

        [TestMethod]
        public void MammothPrimeAffinityControllerMonitorCheckStatusAndNotify_RegionIsDisabled_SkipsMonitoringRegion()
        {
            //Given
            enabledRegions.Add("FL", false);
            enabledRegions.Add("MA", false);
            enabledRegions.Add("MW", false);

            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockPrimeAffinitySettings.VerifyGet(m => m.PrimeAffinityControllerMonitorEnabledByRegion, Times.Once);
            mockPrimeAffinitySettings.VerifyGet(m => m.PrimeAffinityPsgCompletionUtcTimeByRegion, Times.Never);
            mockGetMammothJobScheduleQuery.Verify(m => m.Search(It.IsAny<GetMammothJobScheduleParameters>()), Times.Never);
            mockOpsgenieTrigger.Verify(m => m.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
            mockLogger.Verify(m => m.Info(It.IsRegex("Skipping Mammoth Prime Affinity Controller.*Monitoring for region is disabled", RegexOptions.None)), Times.Exactly(3));
            mockCache.Verify(m => m.Set(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<DateTimeOffset>()), Times.Never);
        }

        [TestMethod]
        public void MammothPrimeAffinityControllerMonitorCheckStatusAndNotify_CurrentUtcTimeIsLessThanRegionsExpectedCompletionTime_SkipsMonitoringRegion()
        {
            //Given
            enabledRegions.Add("FL", true);
            enabledRegions.Add("MA", true);
            enabledRegions.Add("MW", true);
            regionCompletionUtcTimes.Add("FL", DateTime.UtcNow.AddMinutes(5));
            regionCompletionUtcTimes.Add("MA", DateTime.UtcNow.AddMinutes(5));
            regionCompletionUtcTimes.Add("MW", DateTime.UtcNow.AddMinutes(5));

            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockPrimeAffinitySettings.VerifyGet(m => m.PrimeAffinityControllerMonitorEnabledByRegion, Times.Once);
            mockPrimeAffinitySettings.VerifyGet(m => m.PrimeAffinityPsgCompletionUtcTimeByRegion, Times.Exactly(3));
            mockGetMammothJobScheduleQuery.Verify(m => m.Search(It.IsAny<GetMammothJobScheduleParameters>()), Times.Never);
            mockOpsgenieTrigger.Verify(m => m.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
            mockLogger.Verify(m => m.Info(It.IsRegex("Skipping Mammoth Prime Affinity Controller.*Current time is less than expected complete time", RegexOptions.None)), Times.Exactly(3));
            mockCache.Verify(m => m.Set(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<DateTimeOffset>()), Times.Never);
        }

        [TestMethod]
        public void MammothPrimeAffinityControllerMonitorCheckStatusAndNotify_CurrentUtcTimeIsGreaterThanRegionsExpectedCompletionTime_ChecksJobScheduleForRegion()
        {
            //Given
            enabledRegions.Add("FL", true);
            enabledRegions.Add("MA", true);
            enabledRegions.Add("MW", true);
            regionCompletionUtcTimes.Add("FL", DateTime.UtcNow.AddMinutes(-5));
            regionCompletionUtcTimes.Add("MA", DateTime.UtcNow.AddMinutes(-5));
            regionCompletionUtcTimes.Add("MW", DateTime.UtcNow.AddMinutes(-5));
            mockGetMammothJobScheduleQuery.Setup(m => m.Search(It.IsAny<GetMammothJobScheduleParameters>()))
                .Returns(new JobSchedule
                {
                    Enabled = true,
                    LastRunEndDateTimeUtc = DateTime.UtcNow.AddMinutes(-30),
                    Status = JobStatusReady
                });

            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockPrimeAffinitySettings.VerifyGet(m => m.PrimeAffinityControllerMonitorEnabledByRegion, Times.Once);
            mockPrimeAffinitySettings.VerifyGet(m => m.PrimeAffinityPsgCompletionUtcTimeByRegion, Times.Exactly(3));
            mockGetMammothJobScheduleQuery.Verify(m => m.Search(It.IsAny<GetMammothJobScheduleParameters>()), Times.Exactly(3));
            mockOpsgenieTrigger.Verify(m => m.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
            mockLogger.Verify(m => m.Info(It.IsRegex("Mammoth Prime Affinity Controller completed on time.*Region", RegexOptions.None)), Times.Exactly(3));
            mockCache.Verify(m => m.Set(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<DateTimeOffset>()), Times.Never);
        }

        [TestMethod]
        public void MammothPrimeAffinityControllerMonitorCheckStatusAndNotify_JobScheduleIsNull_LogsJobScheduleDoesntExist()
        {
            //Given
            JobSchedule jobSchedule = null;
            enabledRegions.Add("FL", true);
            enabledRegions.Add("MA", true);
            enabledRegions.Add("MW", true);
            regionCompletionUtcTimes.Add("FL", DateTime.UtcNow.AddMinutes(-5));
            regionCompletionUtcTimes.Add("MA", DateTime.UtcNow.AddMinutes(-5));
            regionCompletionUtcTimes.Add("MW", DateTime.UtcNow.AddMinutes(-5));
            mockGetMammothJobScheduleQuery.Setup(m => m.Search(It.IsAny<GetMammothJobScheduleParameters>()))
                .Returns(jobSchedule);

            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockPrimeAffinitySettings.VerifyGet(m => m.PrimeAffinityControllerMonitorEnabledByRegion, Times.Once);
            mockPrimeAffinitySettings.VerifyGet(m => m.PrimeAffinityPsgCompletionUtcTimeByRegion, Times.Exactly(3));
            mockGetMammothJobScheduleQuery.Verify(m => m.Search(It.IsAny<GetMammothJobScheduleParameters>()), Times.Exactly(3));
            mockOpsgenieTrigger.Verify(m => m.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
            mockLogger.Verify(m => m.Info(It.IsRegex("Skipping Mammoth Prime Affinity Controller.*Job schedule.*does not exist or is disabled", RegexOptions.None)), Times.Exactly(3));
            mockCache.Verify(m => m.Set(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<DateTimeOffset>()), Times.Never);
        }

        [TestMethod]
        public void MammothPrimeAffinityControllerMonitorCheckStatusAndNotify_JobScheduleIsDisabled_LogsJobScheduleIsDisabled()
        {
            //Given
            enabledRegions.Add("FL", true);
            enabledRegions.Add("MA", true);
            enabledRegions.Add("MW", true);
            regionCompletionUtcTimes.Add("FL", DateTime.UtcNow.AddMinutes(-5));
            regionCompletionUtcTimes.Add("MA", DateTime.UtcNow.AddMinutes(-5));
            regionCompletionUtcTimes.Add("MW", DateTime.UtcNow.AddMinutes(-5));
            mockGetMammothJobScheduleQuery.Setup(m => m.Search(It.IsAny<GetMammothJobScheduleParameters>()))
                .Returns(new JobSchedule
                {
                    Enabled = false,
                    LastRunEndDateTimeUtc = DateTime.UtcNow.AddMinutes(-30),
                    Status = JobStatusReady
                });

            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockPrimeAffinitySettings.VerifyGet(m => m.PrimeAffinityControllerMonitorEnabledByRegion, Times.Once);
            mockPrimeAffinitySettings.VerifyGet(m => m.PrimeAffinityPsgCompletionUtcTimeByRegion, Times.Exactly(3));
            mockGetMammothJobScheduleQuery.Verify(m => m.Search(It.IsAny<GetMammothJobScheduleParameters>()), Times.Exactly(3));
            mockOpsgenieTrigger.Verify(m => m.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
            mockLogger.Verify(m => m.Info(It.IsRegex("Skipping Mammoth Prime Affinity Controller .* monitoring. Job schedule .* does not exist or is disabled.", RegexOptions.None)), Times.Exactly(3));
            mockCache.Verify(m => m.Set(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<DateTimeOffset>()), Times.Never);
        }

        [TestMethod]
        public void MammothPrimeAffinityControllerMonitorCheckStatusAndNotify_JobStatusIsNotReady_TriggersOpsgenieAlert()
        {
            //Given
            enabledRegions.Add("FL", true);
            enabledRegions.Add("MA", true);
            enabledRegions.Add("MW", true);
            regionCompletionUtcTimes.Add("FL", DateTime.UtcNow.AddMinutes(-5));
            regionCompletionUtcTimes.Add("MA", DateTime.UtcNow.AddMinutes(-5));
            regionCompletionUtcTimes.Add("MW", DateTime.UtcNow.AddMinutes(-5));
            mockGetMammothJobScheduleQuery.Setup(m => m.Search(It.IsAny<GetMammothJobScheduleParameters>()))
                .Returns(new JobSchedule
                {
                    Enabled = true,
                    LastRunEndDateTimeUtc = DateTime.UtcNow.AddMinutes(-30),
                    Status = JobStatusFailed
                });
            mockCache.Setup(m => m.Contains(It.IsAny<string>()))
                .Returns(false);

            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockPrimeAffinitySettings.VerifyGet(m => m.PrimeAffinityControllerMonitorEnabledByRegion, Times.Once);
            mockPrimeAffinitySettings.VerifyGet(m => m.PrimeAffinityPsgCompletionUtcTimeByRegion, Times.Exactly(3));
            mockGetMammothJobScheduleQuery.Verify(m => m.Search(It.IsAny<GetMammothJobScheduleParameters>()), Times.Exactly(3));
            mockOpsgenieTrigger.Verify(m => m.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Exactly(3));
            mockCache.Verify(m => m.Set(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<DateTimeOffset>()), Times.Exactly(3));
        }

        [TestMethod]
        public void MammothPrimeAffinityControllerMonitorCheckStatusAndNotify_LastRunEndTimeIsLessThenToday_TriggersOpsgenieAlert()
        {
            //Given
            enabledRegions.Add("FL", true);
            enabledRegions.Add("MA", true);
            enabledRegions.Add("MW", true);
            regionCompletionUtcTimes.Add("FL", DateTime.UtcNow.AddMinutes(-5));
            regionCompletionUtcTimes.Add("MA", DateTime.UtcNow.AddMinutes(-5));
            regionCompletionUtcTimes.Add("MW", DateTime.UtcNow.AddMinutes(-5));
            mockGetMammothJobScheduleQuery.Setup(m => m.Search(It.IsAny<GetMammothJobScheduleParameters>()))
                .Returns(new JobSchedule
                {
                    Enabled = true,
                    LastRunEndDateTimeUtc = DateTime.UtcNow.Date.AddMinutes(-30),
                    Status = JobStatusReady
                });
            mockCache.Setup(m => m.Contains(It.IsAny<string>()))
                .Returns(false);

            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockPrimeAffinitySettings.VerifyGet(m => m.PrimeAffinityControllerMonitorEnabledByRegion, Times.Once);
            mockPrimeAffinitySettings.VerifyGet(m => m.PrimeAffinityPsgCompletionUtcTimeByRegion, Times.Exactly(3));
            mockGetMammothJobScheduleQuery.Verify(m => m.Search(It.IsAny<GetMammothJobScheduleParameters>()), Times.Exactly(3));
            mockOpsgenieTrigger.Verify(m => m.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Exactly(3));
            mockCache.Verify(m => m.Set(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<DateTimeOffset>()), Times.Exactly(3));
        }

        [TestMethod]
        public void MammothPrimeAffinityControllerMonitorCheckStatusAndNotify_CacheForRegionIsAlreadySet_DoesNotTriggerOpsgenieAlert()
        {
            //Given
            enabledRegions.Add("FL", true);
            enabledRegions.Add("MA", true);
            enabledRegions.Add("MW", true);
            regionCompletionUtcTimes.Add("FL", DateTime.UtcNow.AddMinutes(-5));
            regionCompletionUtcTimes.Add("MA", DateTime.UtcNow.AddMinutes(-5));
            regionCompletionUtcTimes.Add("MW", DateTime.UtcNow.AddMinutes(-5));
            mockGetMammothJobScheduleQuery.Setup(m => m.Search(It.IsAny<GetMammothJobScheduleParameters>()))
                .Returns(new JobSchedule
                {
                    Enabled = true,
                    LastRunEndDateTimeUtc = DateTime.Today.ToUniversalTime().AddMinutes(-30),
                    Status = JobStatusReady
                });
            mockCache.Setup(m => m.Contains(It.IsAny<string>()))
                .Returns(true);

            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockPrimeAffinitySettings.VerifyGet(m => m.PrimeAffinityControllerMonitorEnabledByRegion, Times.Once);
            mockPrimeAffinitySettings.VerifyGet(m => m.PrimeAffinityPsgCompletionUtcTimeByRegion, Times.Exactly(3));
            mockGetMammothJobScheduleQuery.Verify(m => m.Search(It.IsAny<GetMammothJobScheduleParameters>()), Times.Exactly(3));
            mockOpsgenieTrigger.Verify(m => m.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
            mockCache.Verify(m => m.Set(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<DateTimeOffset>()), Times.Never);
        }

        [TestMethod]
        public void MammothPrimeAffinityControllerMonitorCheckStatusAndNotify_SomeJobsCompletedOnTimeAndSomeDidNot_DoesNotTriggerAlertForCompletedJobsAndTriggersAlertsForNonCompletedJobs()
        {
            //Given
            enabledRegions.Add("FL", true);
            enabledRegions.Add("MA", true);
            enabledRegions.Add("MW", true);
            regionCompletionUtcTimes.Add("FL", DateTime.UtcNow.AddMinutes(-5));
            regionCompletionUtcTimes.Add("MA", DateTime.UtcNow.AddMinutes(-5));
            regionCompletionUtcTimes.Add("MW", DateTime.UtcNow.AddMinutes(-5));
            mockGetMammothJobScheduleQuery.Setup(m => m.Search(It.Is<GetMammothJobScheduleParameters>(
                (p) => p.Region == "FL")))
                .Returns(new JobSchedule
                {
                    Enabled = true,
                    LastRunEndDateTimeUtc = DateTime.UtcNow.AddMinutes(-30),
                    Status = JobStatusReady
                });
            mockGetMammothJobScheduleQuery.Setup(m => m.Search(It.Is<GetMammothJobScheduleParameters>(
                (p) => p.Region == "MA")))
                .Returns(new JobSchedule
                {
                    Enabled = true,
                    LastRunEndDateTimeUtc = DateTime.UtcNow.AddMinutes(30),
                    Status = JobStatusReady
                });
            mockGetMammothJobScheduleQuery.Setup(m => m.Search(It.Is<GetMammothJobScheduleParameters>(
                (p) => p.Region == "MW")))
                .Returns(new JobSchedule
                {
                    Enabled = true,
                    LastRunEndDateTimeUtc = DateTime.UtcNow.AddMinutes(-30),
                    Status = JobStatusReady
                });

            //When
            monitor.CheckStatusAndNotify();

            //Then
            mockPrimeAffinitySettings.VerifyGet(m => m.PrimeAffinityControllerMonitorEnabledByRegion, Times.Once);
            mockPrimeAffinitySettings.VerifyGet(m => m.PrimeAffinityPsgCompletionUtcTimeByRegion, Times.Exactly(3));
            mockGetMammothJobScheduleQuery.Verify(m => m.Search(It.IsAny<GetMammothJobScheduleParameters>()), Times.Exactly(3));
            mockOpsgenieTrigger.Verify(m => m.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
            mockLogger.Verify(m => m.Info(It.IsRegex("Mammoth Prime Affinity Controller completed on time.*Region", RegexOptions.None)), Times.Exactly(2));
            mockLogger.Verify(m => m.Info(It.IsRegex("Mammoth Prime Affinity Controller has not completed by expected time for region.", RegexOptions.None)), Times.Once);
            mockCache.Verify(m => m.Set(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<DateTimeOffset>()), Times.Once);
        }
    }
}
