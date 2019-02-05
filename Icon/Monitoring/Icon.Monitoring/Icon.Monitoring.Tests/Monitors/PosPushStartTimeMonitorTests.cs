using Icon.Logging;
using Icon.Monitoring.Common.Constants;
using Icon.Monitoring.Common.Opsgenie;
using Icon.Monitoring.Common.Settings;
using Icon.Monitoring.DataAccess.Model;
using Icon.Monitoring.DataAccess.Queries;
using Icon.Monitoring.Monitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NodaTime;
using NodaTime.Testing;
using System;
using System.Collections.Generic;

namespace Icon.Monitoring.Tests.Monitors
{
    [TestClass]
    public class PosPushStartTimeMonitorTests
    {
        private PosPushStartTimeMonitor posPushStartTimeMonitor;
        private Mock<IOpsgenieTrigger> mockOpsgenieTrigger;
        private Mock<IQueryByRegionHandler<GetAppLogByAppAndMessageParameters, AppLog>> mockAppLogQuery;
        private IMonitorSettings settings;
        private IClock fakeClock;
        private IDateTimeZoneProvider dateTimeZoneProvider;
        private Mock<ILogger> mockLogger;

        [TestInitialize]
        public void InitializeTest()
        {
            this.mockOpsgenieTrigger = new Mock<IOpsgenieTrigger>();
            this.mockAppLogQuery = new Mock<IQueryByRegionHandler<GetAppLogByAppAndMessageParameters, AppLog>>();
            this.fakeClock = new FakeClock(Instant.FromDateTimeUtc(DateTime.UtcNow));
            this.dateTimeZoneProvider = DateTimeZoneProviders.Tzdb;
            this.mockLogger = new Mock<ILogger>();
            this.settings = new MonitorSettings();
            SetupMonitorSettings();
            this.posPushStartTimeMonitor = new PosPushStartTimeMonitor(
                this.settings,
                this.mockOpsgenieTrigger.Object,
                this.mockAppLogQuery.Object,
                this.dateTimeZoneProvider,
                this.fakeClock,
                this.mockLogger.Object);
        }

        private void SetupMonitorSettings()
        {
            this.settings.PosPushStartTime_FL = LocalTime.FromHourMinuteSecondTick(0, 1, 0, 0);
            this.settings.PosPushStartTime_MA = LocalTime.FromHourMinuteSecondTick(23, 59, 59, 999999);
            this.settings.PosPushStartTime_MW = LocalTime.FromHourMinuteSecondTick(23, 59, 59, 999999);
            this.settings.PosPushStartTime_NA = LocalTime.FromHourMinuteSecondTick(23, 59, 59, 999999);
            this.settings.PosPushStartTime_NC = LocalTime.FromHourMinuteSecondTick(23, 59, 59, 999999);
            this.settings.PosPushStartTime_NE = LocalTime.FromHourMinuteSecondTick(23, 59, 59, 999999);
            this.settings.PosPushStartTime_PN = LocalTime.FromHourMinuteSecondTick(23, 59, 59, 999999);
            this.settings.PosPushStartTime_RM = LocalTime.FromHourMinuteSecondTick(23, 59, 59, 999999);
            this.settings.PosPushStartTime_SO = LocalTime.FromHourMinuteSecondTick(23, 59, 59, 999999);
            this.settings.PosPushStartTime_SP = LocalTime.FromHourMinuteSecondTick(23, 59, 59, 999999);
            this.settings.PosPushStartTime_SW = LocalTime.FromHourMinuteSecondTick(23, 59, 59, 999999);
            this.settings.PosPushStartTime_UK = LocalTime.FromHourMinuteSecondTick(23, 59, 59, 999999);
        }

        [TestMethod]
        public void PosPushStartTimeMonitor_AppLogRowIsNullAndCurrentTimeIsAfterConfiguredStartTime_ShouldSendsOpsgenieAlert()
        {
            // Given
            AppLog appLogAllOtherRegions = BuildAppLog(DateTime.Now);
            this.settings.PosPushStartTime_FL = LocalTime.FromHourMinuteSecondTick(0, 10, 0, 0); // 00:10:00

            this.mockAppLogQuery.SetupSequence(q => q.Search(It.IsAny<GetAppLogByAppAndMessageParameters>()))
                .Returns((AppLog)null)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions);

            // When
            this.posPushStartTimeMonitor.CheckStatusAndNotify();

            // Then
            this.mockOpsgenieTrigger
                .Verify(pd => pd.TriggerAlert(It.Is<string>(s => s == "POS Push Job Issue"), It.Is<string>(s => s == "POS Push has not started for the following region: FL "),
                    It.IsAny<Dictionary<string, string>>()), Times.Once);
        }

        [TestMethod]
        public void PosPushStartTimeMonitor_AppLogRowIsNullAndCurrentTimeIsBeforeConfiguredStartTime_ShouldNotSendOpsgenieAlert()
        {
            // Given
            this.fakeClock = new FakeClock(Instant.FromDateTimeOffset(DateTime.Today.AddMinutes(5))); // current time is 00:05:00
            AppLog appLogAllOtherRegions = BuildAppLog(DateTime.Now);
            this.settings.PosPushStartTime_FL = LocalTime.FromHourMinuteSecondTick(0, 10, 0, 0); // 00:10:00

            this.mockAppLogQuery.SetupSequence(q => q.Search(It.IsAny<GetAppLogByAppAndMessageParameters>()))
                .Returns((AppLog)null)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions);

            this.posPushStartTimeMonitor = new PosPushStartTimeMonitor(
                this.settings,
                this.mockOpsgenieTrigger.Object,
                this.mockAppLogQuery.Object,
                this.dateTimeZoneProvider,
                this.fakeClock,
                this.mockLogger.Object);

            // When
            this.posPushStartTimeMonitor.CheckStatusAndNotify();

            // Then
            this.mockOpsgenieTrigger
                .Verify(pd => pd.TriggerAlert(It.Is<string>(s => s == "POS Push Job Issue"), It.Is<string>(s => s.Contains("POS Push has not started for the following region:")),
                    It.IsAny<Dictionary<string, string>>()), Times.Never);
        }

        [TestMethod]
        public void PosPushStartTimeMonitor_AppLogRowIsNotNull_ShouldNotSendOpsgenieAlert()
        {
            // Given
            this.fakeClock = new FakeClock(Instant.FromDateTimeOffset(DateTime.Today.AddMinutes(5))); // current time is 00:05:00
            AppLog appLog = BuildAppLog(DateTime.Now);
            this.settings.PosPushStartTime_FL = LocalTime.FromHourMinuteSecondTick(0, 10, 0, 0); // 00:10:00

            this.mockAppLogQuery.SetupSequence(q => q.Search(It.IsAny<GetAppLogByAppAndMessageParameters>()))
                .Returns(appLog)
                .Returns(appLog)
                .Returns(appLog)
                .Returns(appLog)
                .Returns(appLog)
                .Returns(appLog)
                .Returns(appLog)
                .Returns(appLog)
                .Returns(appLog)
                .Returns(appLog)
                .Returns(appLog)
                .Returns(appLog);

            this.posPushStartTimeMonitor = new PosPushStartTimeMonitor(
                this.settings,
                this.mockOpsgenieTrigger.Object,
                this.mockAppLogQuery.Object,
                this.dateTimeZoneProvider,
                this.fakeClock,
                this.mockLogger.Object);

            // When
            this.posPushStartTimeMonitor.CheckStatusAndNotify();

            // Then
            this.mockOpsgenieTrigger
                .Verify(pd => pd.TriggerAlert(It.Is<string>(s => s == "POS Push Job Issue"), It.Is<string>(s => s.Contains("POS Push has not started for the following region:")),
                    It.IsAny<Dictionary<string, string>>()), Times.Never);
        }

        [TestMethod]
        public void PosPushStartTimeMonitor_SentOpsgenieAlertForFloridaRegionFirstTimeAndCheckingTheSecondTime_ShouldOnlySendOneAlert()
        {
            // Given
            DateTime logDate = DateTime.Now.AddDays(-1); // Now minus one day to simulate the log date being on a different day.
            AppLog appLogAllOtherRegions = BuildAppLog(DateTime.Now);
            this.settings.PosPushStartTime_FL = LocalTime.FromHourMinuteSecondTick(0, 10, 0, 0); // 00:10:00

            this.mockAppLogQuery.SetupSequence(q => q.Search(It.IsAny<GetAppLogByAppAndMessageParameters>()))
                .Returns((AppLog)null)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns((AppLog)null)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions)
                .Returns(appLogAllOtherRegions);

            // When
            this.posPushStartTimeMonitor.CheckStatusAndNotify();
            this.posPushStartTimeMonitor.CheckStatusAndNotify();

            // Then
            this.mockOpsgenieTrigger
                .Verify(pd => pd.TriggerAlert(It.Is<string>(s => s == "POS Push Job Issue"), It.Is<string>(s => s == "POS Push has not started for the following region: FL "),
                    It.IsAny<Dictionary<string, string>>()), Times.Once);
        }

        [TestMethod]
        public void PosPushStartTimeMonitor_MultipleRegionsDoNotStartOnTime_ShouldSendOnOpsgenieAlertWithFailedRegionsListed()
        {
            // Given
            AppLog appLogOtherRegions = BuildAppLog(DateTime.Now);
            this.settings.PosPushStartTime_FL = LocalTime.FromHourMinuteSecondTick(0, 10, 0, 0); // 00:10:00
            this.settings.PosPushStartTime_MW = LocalTime.FromHourMinuteSecondTick(0, 10, 0, 0); // 00:10:00
            this.settings.PosPushStartTime_NC = LocalTime.FromHourMinuteSecondTick(0, 10, 0, 0); // 00:10:00
            this.settings.PosPushStartTime_SO = LocalTime.FromHourMinuteSecondTick(0, 10, 0, 0); // 00:10:00

            this.mockAppLogQuery.SetupSequence(q => q.Search(It.IsAny<GetAppLogByAppAndMessageParameters>()))
                .Returns((AppLog)null)                      // FL
                .Returns(appLogOtherRegions)        // MA
                .Returns((AppLog)null)                      // MW
                .Returns(appLogOtherRegions)        // NA
                .Returns((AppLog)null)                      // NC
                .Returns(appLogOtherRegions)        // NE
                .Returns(appLogOtherRegions)        // PN
                .Returns(appLogOtherRegions)        // RM
                .Returns((AppLog)null)                      // SO
                .Returns(appLogOtherRegions)        // SP
                .Returns(appLogOtherRegions)        // SW
                .Returns(appLogOtherRegions);       // UK

            // When
            this.posPushStartTimeMonitor.CheckStatusAndNotify();
            
            // Then
            this.mockOpsgenieTrigger
                .Verify(pd => pd.TriggerAlert(It.Is<string>(s => s == "POS Push Job Issue"), It.Is<string>(s => s == "POS Push has not started for the following region: FL MW NC SO "),
                    It.IsAny<Dictionary<string, string>>()), Times.Once);
        }

        private AppLog BuildAppLog(DateTime logDate)
        {
            AppLog appLog = new AppLog
            {
                HostName = "Test",
                Id = 1,
                InsertDate = DateTime.Now,
                Level = "Info",
                LogDate = logDate,
                Logger = "TestLogger",
                Message = "Scheduled POS Push Job is starting",
                Name = IrmaAppConfigAppNames.PosPushJob,
                Thread = 1,
                UserName = "foo.bar"
            };

            return appLog;
        }
    }
}