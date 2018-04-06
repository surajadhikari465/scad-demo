using Icon.Common.Email;
using Icon.Logging;
using Icon.Monitoring.Common.ApiController;
using Icon.Monitoring.Common.PagerDuty;
using Icon.Monitoring.Common.Settings;
using Icon.Monitoring.DataAccess.Queries;
using Icon.Monitoring.Monitors;
using Icon.Monitoring.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NodaTime;
using NodaTime.Testing;
using System;
using System.Collections.Generic;

namespace Icon.Monitoring.Tests.Monitors
{
    [TestClass]
    public class MonitorServiceTests
    {
        private Mock<IMonitorSettings> mockSettings;
        private MonitorService monitorService;
        private Mock<IEmailClient> mockEmailClient;
        private IClock fakeClock;
        private IDateTimeZoneProvider dateTimeZoneProvider;
        private ApiControllerMonitor apiControllerMonitor;
        private Mock<Icon.Common.DataAccess.IQueryHandler<GetApiMessageQueueIdParameters, int>> mockMessageQueueQuery;
        private Mock<Icon.Common.DataAccess.IQueryHandler<GetApiMessageUnprocessedRowCountParameters, int>> mockMessageUnprocessedRowCountQuery;
        private Mock<IPagerDutyTrigger> mockPagerDutyTrigger;
        private Mock<ILogger> logger;
        private MessageQueueCache messageQueueCache;

        [TestInitialize]
        public void Initialize()
        {
            this.mockPagerDutyTrigger = new Mock<IPagerDutyTrigger>();
            this.mockMessageQueueQuery = new Mock<Icon.Common.DataAccess.IQueryHandler<GetApiMessageQueueIdParameters, int>>();
            this.mockMessageUnprocessedRowCountQuery = new Mock<Icon.Common.DataAccess.IQueryHandler<GetApiMessageUnprocessedRowCountParameters, int>>();
            this.fakeClock = new FakeClock(Instant.FromDateTimeUtc(DateTime.UtcNow));
            this.dateTimeZoneProvider = DateTimeZoneProviders.Tzdb;
            this.mockSettings = new Mock<IMonitorSettings>();
            this.mockEmailClient = new Mock<IEmailClient>();
            this.logger = new Mock<ILogger>();
            this.messageQueueCache = new MessageQueueCache();

            mockSettings.SetupGet(m => m.MaintenanceStartTime).Returns(new LocalTime(1, 0, 0));
            mockSettings.SetupGet(m => m.MaintenanceEndTime).Returns(new LocalTime(23, 59, 59));
            mockSettings.SetupGet(m => m.MaintenanceDay).Returns("4");
            mockSettings.SetupGet(m => m.MonitorServiceTimer).Returns(1);
            mockSettings.SetupGet(m => m.SendPagerDutyNotifications).Returns(true);

            this.apiControllerMonitor = new ApiControllerMonitor(
                this.mockSettings.Object,
                this.mockMessageQueueQuery.Object,
                this.mockMessageUnprocessedRowCountQuery.Object,
                this.mockPagerDutyTrigger.Object,
                this.dateTimeZoneProvider,
                this.fakeClock,
                new Mock<ILogger>().Object,
                messageQueueCache);
            this.monitorService = new MonitorService(
                new List<IMonitor>() { this.apiControllerMonitor },
                mockSettings.Object,
                mockEmailClient.Object,
                dateTimeZoneProvider,
                fakeClock,
                logger.Object);
        }

        // one way to test montor service is to make method RunService public and other way is to use isolation framework -something to be looked at 
        [TestMethod]
        public void MonitorServiceStart_MaintenanceTime_ShouldNotSendPagerDutyAlert()
        {
            this.monitorService.Start();
            mockPagerDutyTrigger.Verify(m => m.TriggerIncident(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
        }
    }
}