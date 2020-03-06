﻿using Icon.Common.DataAccess;
using Icon.Logging;
using Icon.Monitoring.Common.Opsgenie;
using Icon.Monitoring.Common.Settings;
using Icon.Monitoring.DataAccess.Queries;
using Icon.Monitoring.Monitors;
using Icon.Monitoring.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Icon.Monitoring.Tests.Monitors
{
    [TestClass]
    public class MessageArchiveMonitorTests
    {
        private Mock<IOpsgenieTrigger> mockOpsgenieTrigger;
        private Mock<IQueryHandler<GetMessageArchiveParameters, int>> mockMessageQueueQuery;
        private Mock<IMonitorSettings> mockSettings;
        private Mock<ILogger> mockLogger;
        private Mock<IDateService> mockDateService;

        [TestInitialize]
        public void Initialize()
        {
            this.mockOpsgenieTrigger = new Mock<IOpsgenieTrigger>();
            this.mockLogger = new Mock<ILogger>();
            this.mockSettings = new Mock<IMonitorSettings>();
            this.mockMessageQueueQuery = new Mock<IQueryHandler<GetMessageArchiveParameters, int>>();
            this.mockDateService = new Mock<IDateService>();
        }


        [TestMethod]
        public void MessageArchiveMonitor_NoRecordsInMessageArchiveQueue_ShouldNotTriggerAlert()
        {
            // Given.
            this.mockMessageQueueQuery.Setup(x => x.Search(It.IsAny<GetMessageArchiveParameters>())).Returns(0);

            var monitor = new MessageArchiveMonitor(this.mockSettings.Object,
                this.mockOpsgenieTrigger.Object, 
                this.mockLogger.Object, 
                this.mockMessageQueueQuery.Object,
                this.mockDateService.Object);
            monitor.ByPassConfiguredRunInterval = true;
            
            // When.
            monitor.CheckStatusAndNotify();

            // Then.
            this.mockOpsgenieTrigger.Verify(x => x.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
        }
        

        [TestMethod]
        public void MessageArchiveMonitor_NoMessageArchiveRecords_ShouldNotTriggerAlert()
        {
            // Given.
            this.mockMessageQueueQuery.Setup(x => x.Search(It.IsAny<GetMessageArchiveParameters>())).Returns(0);

            var monitor = new MessageArchiveMonitor(this.mockSettings.Object,
                this.mockOpsgenieTrigger.Object,
                this.mockLogger.Object,
                this.mockMessageQueueQuery.Object,
                this.mockDateService.Object);
            monitor.ByPassConfiguredRunInterval = true;

            // When.
            monitor.CheckStatusAndNotify();
            monitor.CheckStatusAndNotify();

            // Then.
            this.mockOpsgenieTrigger.Verify(x => x.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
        }
        
        [TestMethod]
        public void MessageArchiveMonitor_MessageArchiveRecordsExist_ShouldTriggerAlert()
        {
            // Given.
            this.mockMessageQueueQuery.Setup(x => x.Search(It.IsAny<GetMessageArchiveParameters>())).Returns(1);

            var monitor = new MessageArchiveMonitor(this.mockSettings.Object,
                this.mockOpsgenieTrigger.Object,
                this.mockLogger.Object,
                this.mockMessageQueueQuery.Object,
                this.mockDateService.Object);
            monitor.ByPassConfiguredRunInterval = true;

            // When.
            monitor.CheckStatusAndNotify();

            // Then.
            this.mockOpsgenieTrigger.Verify(x => x.TriggerAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
        }

        [TestMethod]
        public void MessageArchiveMonitor_ProcessRuns_LastMonitorDateIsUpdatedAndQueryHandlerCalledWithUpdatedDate()
        {
            // Given.
            GetMessageArchiveParameters calledParameters = new GetMessageArchiveParameters();

            this.mockMessageQueueQuery.Setup(x => x.Search(It.IsAny<GetMessageArchiveParameters>())).Returns(0)
                .Callback<GetMessageArchiveParameters>((obj) => calledParameters = obj);

            this.mockDateService.SetupSequence(x => x.UtcNow)
                .Returns(DateTime.Parse("2001-01-01"))
                .Returns(DateTime.Parse("2001-01-02"));

            var monitor = new MessageArchiveMonitor(this.mockSettings.Object,
                this.mockOpsgenieTrigger.Object,
                this.mockLogger.Object,
                this.mockMessageQueueQuery.Object,
                this.mockDateService.Object);
            monitor.ByPassConfiguredRunInterval = true;

            // When.
            monitor.CheckStatusAndNotify();

            // Then.
            Assert.AreEqual(DateTime.Parse("2001-01-01"), calledParameters.LastMonitorDate);

            // When.
            monitor.CheckStatusAndNotify();

            // Then.
            Assert.AreEqual(DateTime.Parse("2001-01-02"), calledParameters.LastMonitorDate);
        }
    }
}